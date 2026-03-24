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
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Configure Multi-Tenant Security Engine native contexts
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IntranetPortal.Api.Security.IPermissionService, IntranetPortal.Api.Security.PermissionService>();

builder.Services.AddControllers();
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

app.UseCors("AllowAll");

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
