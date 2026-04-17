using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Caching.Memory;
using IntranetPortal.Data.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 32)),
        x => x.MigrationsAssembly("IntranetPortal.Data")));

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:3000", "http://localhost:3001")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        }
        else
        {
            var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        }
    });
});

// Configure Multi-Tenant Security Engine native contexts
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IntranetPortal.Api.Security.IPermissionService, IntranetPortal.Api.Security.PermissionService>();
builder.Services.AddSingleton<IntranetPortal.Api.Security.IChallengeCryptoService, IntranetPortal.Api.Security.ChallengeCryptoService>();

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("login", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10;
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
        limiterOptions.QueueLimit = 0;
    });
    options.RejectionStatusCode = 429;
});

builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is missing.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("auth_token"))
            {
                context.Token = context.Request.Cookies["auth_token"];
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = async context =>
        {
            var config = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            if (config.GetValue<bool>("Security:RequireStatefulTokenValidation", true))
            {
                var principal = context.Principal;
                if (principal == null) return;
                
                var subClaim = principal.FindFirst(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
                if (string.IsNullOrEmpty(subClaim) || !int.TryParse(subClaim, out int userId)) return;
                
                var memoryCache = context.HttpContext.RequestServices.GetRequiredService<Microsoft.Extensions.Caching.Memory.IMemoryCache>();
                var cacheKey = $"UserState_{userId}";
                
                var cachedObj = memoryCache.Get(cacheKey);
                IntranetPortal.Data.Models.UserAccount? cachedUser = cachedObj as IntranetPortal.Data.Models.UserAccount;
                
                if (cachedUser == null)
                {
                    var dbContext = context.HttpContext.RequestServices.GetRequiredService<ApplicationDbContext>();
                    cachedUser = await dbContext.UserAccounts.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
                    if (cachedUser != null)
                    {
                        memoryCache.Set(cacheKey, cachedUser, TimeSpan.FromMinutes(2));
                    }
                }
                
                if (cachedUser == null || !cachedUser.IsActive)
                {
                    context.Fail("TOKEN_INVALID");
                    return;
                }
                
                if (cachedUser.LockedUntil.HasValue && cachedUser.LockedUntil > DateTimeOffset.UtcNow)
                {
                    context.Response.Headers.Append("WWW-Authenticate", "Bearer error=\"invalid_token\", error_description=\"LOCKED_OUT\"");
                    context.Fail("LOCKED_OUT");
                    return;
                }
                
                var tokenStampStr = principal.FindFirst("SecurityStamp")?.Value;
                if (!string.IsNullOrEmpty(tokenStampStr) && int.TryParse(tokenStampStr, out int tokenStamp))
                {
                    if (cachedUser.SecurityStamp != tokenStamp)
                    {
                        context.Response.Headers.Append("WWW-Authenticate", "Bearer error=\"invalid_token\", error_description=\"STAMP_INVALID\"");
                        context.Fail("STAMP_INVALID");
                        return;
                    }
                }
            }
        }
    };
});

// Register the dynamic Policy Authorization engine overrides natively
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationPolicyProvider, IntranetPortal.Api.Security.PermissionPolicyProvider>();
builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, IntranetPortal.Api.Security.PermissionAuthorizationHandler>();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRateLimiter();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Default root endpoint to verify API is running
app.MapGet("/", () => new { Message = "IntranetPortal API is running!", Status = "Healthy" });

// Run the Production Initializer securely at startup
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<IntranetPortal.Data.Data.ApplicationDbContext>();
    var globalAdminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!");
    
    // 1. First establish God-mode and Roles
    await IntranetPortal.Data.Data.DatabaseSeeder.InitializeAsync(context, globalAdminHash);
    
    // 2. Dynamically scrape all endpoints to pull custom Developer Capabilities seamlessly
    await IntranetPortal.Api.Security.PermissionScanner.SyncPoliciesAsync(context, typeof(Program).Assembly);
}

app.Run();
