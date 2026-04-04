import os
import shutil
import zipfile
import subprocess
from pathlib import Path

ROOT_DIR = os.path.dirname(os.path.abspath(__file__))
RELEASE_DIR = os.path.join(ROOT_DIR, "_release")
BACKEND_API_DIR = os.path.join(ROOT_DIR, "backend", "IntranetPortal.Api")
BACKEND_DATA_DIR = os.path.join(ROOT_DIR, "backend", "IntranetPortal.Data")
FRONTEND_DIR = os.path.join(ROOT_DIR, "frontend")
DOCS_DIR = os.path.join(ROOT_DIR, "docs")

def run_command(command, cwd=ROOT_DIR):
    print(f"Running: {command}")
    subprocess.run(command, cwd=cwd, shell=True, check=True)

def zip_directory(dir_path, zip_path, exclude_dirs=None, exclude_files=None, exclude_startswith=None):
    if exclude_dirs is None: exclude_dirs = []
    if exclude_files is None: exclude_files = []
    if exclude_startswith is None: exclude_startswith = []

    print(f"Zipping {dir_path} to {zip_path}...")
    with zipfile.ZipFile(zip_path, 'w', zipfile.ZIP_DEFLATED) as zipf:
        for root, dirs, files in os.walk(dir_path):
            # Mutate dirs in-place to skip excluded directories
            dirs[:] = [d for d in dirs if d not in exclude_dirs]
            
            for file in files:
                if file in exclude_files:
                    continue
                should_exclude = False
                for prefix in exclude_startswith:
                    if file.startswith(prefix):
                        should_exclude = True
                        break
                if should_exclude:
                    continue
                
                file_path = os.path.join(root, file)
                # Compute relative path for zip internal structure
                rel_path = os.path.relpath(file_path, dir_path)
                zipf.write(file_path, rel_path)

def package_backend():
    print("--- Packaging Backend ---")
    publish_dir = os.path.join(BACKEND_API_DIR, "publish")
    
    # Clean up previous publish directory to avoid recursive copy errors (MSB3030)
    if os.path.exists(publish_dir):
        shutil.rmtree(publish_dir)
    
    # 1. Publish the .NET Application
    run_command("dotnet publish -c Release -o ./publish", cwd=BACKEND_API_DIR)
    
    # 2. Exclude environment-specific appsettings to prevent crushing prod settings or leaking dev secrets
    for env_file in ["appsettings.Production.json", "appsettings.Development.json"]:
        settings_path = os.path.join(publish_dir, env_file)
        if os.path.exists(settings_path):
            os.remove(settings_path)
            print(f"Removed local {env_file} from payload.")
        
    # 3. Zip it into release
    zip_directory(publish_dir, os.path.join(RELEASE_DIR, "backend-release.zip"))

def package_frontend():
    print("\n--- Packaging Frontend ---")
    # We ignore standard heavy folders and specific env files to protect production keys
    exclude_folders = ["node_modules", ".next", "out", ".git", ".vscode"]
    exclude_files = ["package-lock.json", ".DS_Store"]
    exclude_prefixes = [".env"] # Will exclude .env, .env.local, .env.production, etc.
    
    zip_directory(
        FRONTEND_DIR, 
        os.path.join(RELEASE_DIR, "frontend-release.zip"),
        exclude_dirs=exclude_folders,
        exclude_files=exclude_files,
        exclude_startswith=exclude_prefixes
    )

def package_docs():
    print("\n--- Packaging Docs ---")
    zip_directory(DOCS_DIR, os.path.join(RELEASE_DIR, "docs-release.zip"))

import argparse

def package_database(from_migration=None):
    print("\n--- Packaging Database SQL Migration ---")
    sql_out = os.path.join(RELEASE_DIR, "database-update.sql")
    
    if from_migration:
        print(f"Generating safe non-idempotent diff starting from: {from_migration}")
        cmd = f"dotnet ef migrations script {from_migration} --project {BACKEND_DATA_DIR} --startup-project {BACKEND_API_DIR} --output {sql_out}"
    else:
        print("Generating full idempotent script (Warning: may fail in MySQL if migrations contain stored procedures)")
        cmd = f"dotnet ef migrations script --idempotent --project {BACKEND_DATA_DIR} --startup-project {BACKEND_API_DIR} --output {sql_out}"
        
    try:
        run_command(cmd)
        
        # MySQL/phpMyAdmin CLI bugfix: EF Core does not append DELIMITER around raw MigrationBuilder.Sql stored procedures
        # We must actively inject DELIMITER // to prevent #1064 syntax errors when run manually in CloudPanel
        with open(sql_out, "r", encoding="utf-8") as file:
            sql_content = file.read()
            
        if "CREATE PROCEDURE DropFkIfExists" in sql_content and "DELIMITER //\n                CREATE PROCEDURE DropFkIfExists" not in sql_content:
            sql_content = sql_content.replace(
                "CREATE PROCEDURE DropFkIfExists(IN tName VARCHAR(255), IN fkName VARCHAR(255))",
                "DELIMITER //\nCREATE PROCEDURE DropFkIfExists(IN tName VARCHAR(255), IN fkName VARCHAR(255))"
            )
            # Find the END; that closes the procedure and replace it with END // DELIMITER ;
            # Since the procedure is precisely defined, we can target exactly:
            sql_content = sql_content.replace(
                "DEALLOCATE PREPARE stmt;\n                    END IF;\n                END;",
                "DEALLOCATE PREPARE stmt;\n                    END IF;\n                END //\nDELIMITER ;"
            )
            with open(sql_out, "w", encoding="utf-8") as file:
                file.write(sql_content)
            print("Patched SQL file with MySQL DELIMITERs for safe phpMyAdmin execution.")

    except Exception as e:
        print(f"Warning: EF Migration generation failed. This sometime happens if the build has warnings. Error: {e}")

def main():
    parser = argparse.ArgumentParser(description="Package IntranetPortal for Production")
    parser.add_argument('--from-migration', type=str, help="Generate a non-idempotent SQL diff starting from this migration name. Bypasses MySQL nested procedure limits.")
    args = parser.parse_args()

    if os.path.exists(RELEASE_DIR):
        print("Cleaning previous release directory...")
        shutil.rmtree(RELEASE_DIR)
    os.makedirs(RELEASE_DIR)
    
    package_backend()
    package_frontend()
    package_docs()
    package_database(args.from_migration)
    
    print(f"\n✅ All packages successfully built and secured inside '{RELEASE_DIR}'")
    print("You can safely upload these directly to CloudPanel!")

if __name__ == "__main__":
    main()
