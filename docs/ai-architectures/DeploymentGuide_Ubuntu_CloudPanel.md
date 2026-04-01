# Deployment Guide: Ubuntu 24.04 + CloudPanel + Nginx + MariaDB

This guide outlines the complete process to deploy the **IntranetPortal** application (Next.js Frontend + .NET 10 Web API Backend) to an Ubuntu 24.04 server managed by CloudPanel.

---

## 🏗️ 1. Architecture Overview
CloudPanel simplifies server management but uses specific user structures (`clp` user) and Nginx virtual hosts (vhosts). We will deploy the application as follows:
- **Database**: Native MariaDB 11.4.7 (managed via CloudPanel).
- **Backend (.NET 10 API)**: Runs as a `systemd` background service (e.g., on port `8000`), exposed via a CloudPanel reverse proxy vhost (e.g., `api.yourdomain.com`).
- **Frontend (Next.js)**: Runs via CloudPanel's built-in **Node.js Site** manager (e.g., on port `3000`), exposed on your main domain (e.g., `yourdomain.com`).

---

## 🛠️ 2. Prerequisites & Server Setup

**1. Install .NET 10 SDK & Runtime on Ubuntu**
SSH into your Ubuntu 24.04 server as `root` or a `sudo` user and install the .NET 10 runtime so your backend can execute.
```bash
wget https://packages.microsoft.com/config/ubuntu/24.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo rm packages-microsoft-prod.deb

sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update
sudo apt-get install -y aspnetcore-runtime-10.0
```

**2. Prepare the Database in CloudPanel**
1. Log into your **CloudPanel Admin Area**.
2. Go to **Databases** ➔ **Add Database**.
3. Create a new Database Name, Database User, and Password.
4. Save these credentials; you will need them for your `.NET` connection string.

---

## 🚀 3. Backend Deployment (.NET 10 API)

**1. Create a Reverse Proxy Site in CloudPanel**
1. In CloudPanel, click **Add Site** ➔ **Reverse Proxy**.
2. **Domain Name**: `api.yourdomain.com`.
3. **Reverse Proxy URL**: `http://127.0.0.1:8000`.
4. Click **Create**.
5. Once created, click on the site in CloudPanel, go to the **SSL** tab, and issue a Let's Encrypt Certificate.

**2. Publish your .NET App**
On your local machine, run the following command inside the `backend/IntranetPortal.Api` folder:
```bash
dotnet publish -c Release -o ./publish
```

**3. Upload Backend Files**
1. Compress the `./publish` folder into a `.zip`.
2. Upload and extract it to your CloudPanel server, typically under:
   `/home/clp-user/htdocs/api.yourdomain.com/`
3. Edit the `appsettings.Production.json` file in that folder to update the MariaDB connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=127.0.0.1;Port=3306;Database=YOUR_DB_NAME;User=YOUR_DB_USER;Password=YOUR_DB_PASSWORD;"
   }
   ```
4. *Important: Apply Entity Framework migrations to the database from your local machine, or generate an SQL script (`dotnet ef migrations script`) and run it via phpMyAdmin in CloudPanel.*

**4. Create the Systemd Service**
SSH into your server and create a service file to keep the backend running:
```bash
sudo nano /etc/systemd/system/intranet-api.service
```
Paste the following (adjust paths and user if needed):
```ini
[Unit]
Description=IntranetPortal .NET 10 Web API

[Service]
WorkingDirectory=/home/your-clp-user/htdocs/api.yourdomain.com/
ExecStart=/usr/bin/dotnet /home/your-clp-user/htdocs/api.yourdomain.com/IntranetPortal.Api.dll
Restart=always
RestartSec=10
SyslogIdentifier=intranet-api
User=your-clp-user
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=ASPNETCORE_URLS=http://127.0.0.1:8000

[Install]
WantedBy=multi-user.target
```
Enable and start the site:
```bash
sudo systemctl daemon-reload
sudo systemctl enable intranet-api.service
sudo systemctl start intranet-api.service
sudo systemctl status intranet-api.service
```

---

## 🌐 4. Frontend Deployment (Next.js)

**1. Create a Node.js Site in CloudPanel**
1. In CloudPanel, click **Add Site** ➔ **Node.js Site**.
2. **Domain Name**: `yourdomain.com`.
3. **Node.js Version**: Select an active LTS version (e.g., 20 or newer).
4. **App Port**: `3000`.
5. Click **Create** and issue an SSL certificate in the **SSL** tab.

**2. Upload Frontend Source Code**
1. Zip your local `frontend` directory (excluding `node_modules` and `.next`).
2. Upload it to `/home/your-clp-user/htdocs/yourdomain.com/`.
3. Extract the files.
4. *Important Note for Wiki:* The app natively reads Wiki files one level above the frontend root. Ensure you also zip the repository's `docs` directory and upload it directly to `/home/your-clp-user/htdocs/docs/` so the Next.js document viewer functions correctly!
5. Edit `.env.production` and verify it has the correct API url:
   ```env
   NEXT_PUBLIC_API_URL=https://api.yourdomain.com
   ```

**3. Install Dependencies & Build**
SSH into the server as the CloudPanel user (or `su - your-clp-user`), navigate to the frontend folder, and build the app:
```bash
cd /home/your-clp-user/htdocs/yourdomain.com
npm install
npm run build
```
*(Note: Your `package.json` runs `next build --webpack` in production to forcefully bypass Turbopack. This ensures the `@serwist/next` Webpack plugin can successfully build your offline Service Worker for PWA features. Your local `next dev` still uses lightning-fast Turbopack since Serwist is disabled locally!)*

**4. Start the Application via PM2**
If PM2 is not already installed on your server, install it globally first:
```bash
npm install -g pm2
```
Then start your Next.js frontend server:
```bash
pm2 start npm --name "intranet-frontend" -- run start
pm2 save
```
*(Alternatively, you can start it directly from the CloudPanel UI if "App Service" is enabled).*

---

## 🔐 5. Final Verification
- Navigate to `https://api.yourdomain.com/swagger` (if Swagger is enabled in production) or an API health check route to ensure the backend is connected to MariaDB.
- Navigate to `https://yourdomain.com` to check the Next.js frontend.
- Check CloudPanel's Nginx error logs (available in the CloudPanel UI under the site's Logs tab) if you receive a `502 Bad Gateway`.
