import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import DepartmentManager, { Department } from "./DepartmentManager";

// Metadata for standard Next.js Head SEO/titles
export const metadata = {
  title: "Departments | Intranet Portal Admin",
};

import { cookies } from "next/headers";

export default async function DepartmentsPage() {
  const cookieStore = await cookies();
  const token = cookieStore.get("auth_token")?.value || "";
  let user: any = null;
  if (token) {
    try {
      user = JSON.parse(Buffer.from(token.split('.')[1], 'base64').toString());
    } catch {}
  }
  
  const userRoleClaim = user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || user?.role || user?.Role;
  const isAdmin = userRoleClaim === "Admin" || (Array.isArray(userRoleClaim) && userRoleClaim.includes("Admin"));
  const permissionsClaim = user?.Permission || user?.permission || [];
  const permissions = Array.isArray(permissionsClaim) ? permissionsClaim : [permissionsClaim];

  const canCreate = isAdmin || permissions.includes("Structure.Department.Create");
  const canEdit = isAdmin || permissions.includes("Structure.Department.Edit");
  const canDelete = isAdmin || permissions.includes("Structure.Department.Delete");

  let departments: Department[] = [];
  let sites: any[] = [];
  
  try {
    const [deptRes, siteRes] = await Promise.all([
      fetchWithAuth("/api/departments", { next: { tags: ["departments"] }, cache: "no-store" }),
      fetchWithAuth("/api/sites", { cache: "no-store" })
    ]);
    
    if (deptRes.ok) departments = await deptRes.json();
    if (siteRes.ok) sites = await siteRes.json();
  } catch (error) {
    console.error("Error connecting to backend API:", error);
  }

  return (
    <MainLayout>
      <DepartmentManager 
          initialDepartments={departments} 
          initialSites={sites}
          canCreate={canCreate} 
          canEdit={canEdit} 
          canDelete={canDelete} 
      />
    </MainLayout>
  );
}
