# Enterprise Intranet Portal - Backend API

This is the ASP.NET Core Web API backend for the Enterprise Intranet Portal. It exposes a secure REST API consumed by the Next.js frontend and any authorized internal microservices.

## Features
- **Dynamic RBAC Engine**: Permission policies are auto-scanned from controller attributes at startup and synced into the database — no manual registration required.
- **Challenge-Response Authentication**: Optional RSA-OAEP encrypted password transfer for defense-in-depth (requires frontend `ENABLE_CHALLENGE_LOGIN=true`).
- **Stateful Token Validation**: Every JWT request is validated against a live database/cache check for `SecurityStamp` and account state, preventing stale tokens from working after account changes.
- **IP + Account Lockout**: Brute-force protection via in-memory cache; locks both the IP+email pair and the user account after 10 failed attempts.
- **Audit Logging**: All login events (success, failure, lockout, challenge) are persisted to `Core_AuditLogs`.
- **Internal API Security**: Microservice-to-microservice communication is authenticated via a shared secret header (`InternalApiSettings:Secret`).

---

## Configuration Reference

The API uses ASP.NET Core's cascading configuration system: `appsettings.json` → `appsettings.{Environment}.json` → Environment Variables.

### Dev vs Prod Quick Checklist

| Setting | Development | Production |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Point to local/dev MariaDB or MySQL. | Point to the production database with least-privilege credentials. |
| `JwtSettings:Key` | Dev secret is acceptable for local use only. | Replace with a strong secret from environment variables or a secrets manager. |
| `JwtSettings:Issuer` | Must match the frontend's expected API issuer. | Must stay aligned with the deployed frontend and API token validation. |
| `JwtSettings:Audience` | Must match the local frontend audience expectation. | Must match the deployed frontend audience expectation. |
| `InternalApiSettings:Secret` | Set a local shared secret if internal endpoints are used in dev. | Required for internal service-to-service endpoints; rotate if exposed. |
| `AllowedOrigins` | Not required; localhost origins are allowed automatically. | Required; list every allowed frontend origin explicitly. |
| `AllowedHosts` | Usually `"*"`. | Set to the public API host name. |
| `Security:CookieDomain` | Usually leave unset. | Set only when auth cookies must work across subdomains, for example `.company.com`. |
| `ChallengeEncryption:PrivateKeyPem` | Optional unless challenge login is enabled and you need stable keys across restarts. | Strongly recommended when challenge login is enabled, especially in multi-instance deployments. |
| `Modules:DrinkOrders:Url` | Optional; defaults to `http://localhost:3001` during initial seed. | Optional; set only if the initial seeded module should point to a deployed Drink Orders URL. |

> **Deployment note**: Prefer environment variables or a secrets manager for production secrets instead of storing real credentials or keys in `appsettings.Production.json`.

### `JwtSettings` ⚠️ Required

| Key | Description | Example |
|---|---|---|
| `JwtSettings:Key` | HMAC-SHA256 signing secret. **Must be ≥ 32 characters.** | `"MySecretKey..."` |
| `JwtSettings:Issuer` | JWT `iss` claim. Must match the frontend's expected issuer. | `"IntranetPortalAPI"` |
| `JwtSettings:Audience` | JWT `aud` claim. Must match the frontend's expected audience. | `"IntranetPortalFrontend"` |

> **Production**: Replace the key in `appsettings.Production.json` or inject it as an environment variable / secrets manager entry. Never commit a production key to source control.

---

### `ConnectionStrings` ⚠️ Required

| Key | Description | Example |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | MariaDB/MySQL connection string. | `"Server=127.0.0.1;Port=3306;Database=IntranetPortal;User=root;Password=pass;"` |

---

### `InternalApiSettings` ⚠️ Required

Used by controllers that expose internal-only endpoints consumed by microservices.

| Key | Description | Example |
|---|---|---|
| `InternalApiSettings:Secret` | Shared bearer secret for microservice-to-microservice calls. | `"MyInternalSecret"` |

> **Production**: Use a long, randomly generated string. Rotate it if compromised.

---

### `Security` (optional — hardcoded defaults apply)

| Key | Default | Description |
|---|---|---|
| `Security:AccessTokenExpirationDays` | `7` | Lifetime of the JWT access token (and its `auth_token` cookie). |
| `Security:LockoutDurationMinutes` | `30` | How long an IP+email pair or account stays locked after 10 failed login attempts. |
| `Security:RequireStatefulTokenValidation` | `true` | When `true`, every authenticated request checks the DB/cache for account state and `SecurityStamp`. Set to `false` to fall back to pure stateless JWT (not recommended for production). |
| `Security:CookieDomain` | *(unset)* | Optional shared cookie domain used in non-development environments (for example `.company.com`) so auth cookies can be sent across subdomains. Leave unset for single-host deployments. |

---

### `ChallengeEncryption` (optional)

Required only when the frontend has `ENABLE_CHALLENGE_LOGIN=true`.

| Key | Default | Description |
|---|---|---|
| `ChallengeEncryption:PrivateKeyPem` | *(auto-generated in-memory)* | PEM-encoded RSA private key used to decrypt challenge passwords. Without this, a new key is generated on every API startup, which causes intermittent failures after restarts or across multiple API instances. |

#### Generating a stable RSA key (PowerShell / OpenSSL)
```bash
openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:2048 -out private.pem
```
Then copy the contents of `private.pem` into your secrets store or `appsettings.Production.json` under `ChallengeEncryption:PrivateKeyPem`.

---

### `AllowedOrigins` (Production only)

In production, CORS is restricted to an explicit allowlist. Configure in `appsettings.Production.json`:

```json
"AllowedOrigins": [
  "https://portal.yourcompany.com",
  "https://drinks.yourcompany.com"
]
```

In development, `http://localhost:3000` and `http://localhost:3001` are allowed automatically.

---

### `AllowedHosts`

Standard ASP.NET Core host filtering.

| Environment | Recommended Value |
|---|---|
| Development | `"*"` |
| Production | `"api.yourcompany.com"` |

---

### `Modules` (optional)

Used by startup seeding/initial bootstrap defaults. After bootstrap, modules are managed through the System Modules management function and persisted in the database.

| Key | Default | Description |
|---|---|---|
| `Modules:DrinkOrders:Url` | `http://localhost:3001` | Optional URL used when seeding the default Drink Orders module if it does not already exist in the database. Set this in production only if you want the initial seeded value to point to a public endpoint. |

> **Note**: Changing this setting does not retroactively update existing module records in the database.

---

## 🚀 Getting Started Locally

```bash
# Restore dependencies
dotnet restore

# Apply database migrations
dotnet ef database update --project ../IntranetPortal.Data

# Run the development server
dotnet run
```

The API will be available at `https://localhost:5254` by default.

A Swagger UI is available in Development at: `https://localhost:5254/swagger`

A seed endpoint creates a default admin account for first-time setup (Development only):
```
POST https://localhost:5254/api/auth/seed-test-admin
```
Credentials: `admin@company.com` / `Admin123!`

---

## Environment Files

| File | Purpose |
|---|---|
| `appsettings.json` | Shared baseline config (logging, security tunables) |
| `appsettings.Development.json` | Local dev overrides — JWT keys, DB connection, internal secret |
| `appsettings.Production.json` | Production overrides — **update before deploying** |
