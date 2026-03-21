import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import { deleteEmployeeAction } from "@/app/actions/employees";
import { cookies } from "next/headers";

interface Employee {
  id: number;
  fullName: string;
  email: string;
  jobTitle: string;
  departmentId: number;
  departmentName: string;
  teamId: number | null;
  teamName: string | null;
  siteId: number;
  siteName: string;
}

export default async function EmployeesPage() {
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
  
  const scopedPermsClaim = user?.ScopedPerm || user?.scopedperm || [];
  const scopedPerms = Array.isArray(scopedPermsClaim) ? scopedPermsClaim : [scopedPermsClaim];

  const canCreate = isAdmin || permissions.includes("HR.Employee.Create");

  const hasSitePermission = (permission: string, siteId: number) => {
    if (isAdmin) return true;
    if (scopedPerms.includes(`${permission}:Global`)) return true;
    return scopedPerms.includes(`${permission}:${siteId}`);
  };

  const res = await fetchWithAuth("/api/employees", { cache: 'no-store' });
  let employees: Employee[] = [];
  
  if (res.ok) {
    employees = await res.json();
  }

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8 py-6">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Employees</h1>
            <p className="text-gray-500 mt-2 text-lg">Manage personnel, organizational structures, and location assignments safely.</p>
          </div>
          {canCreate && (
            <Link href="/employees/new" className="inline-flex items-center justify-center px-6 py-3 rounded-xl bg-blue-600 hover:bg-blue-700 text-white font-semibold transition-all shadow-md hover:shadow-lg focus:ring-4 focus:ring-blue-100">
              <svg className="w-5 h-5 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
              </svg>
              Add New Employee
            </Link>
          )}
        </header>

        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
          {employees.length === 0 ? (
            <div className="p-16 text-center">
              <div className="w-20 h-20 mx-auto bg-blue-50 text-blue-300 rounded-full flex items-center justify-center mb-6">
                <svg className="w-10 h-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">No employees found</h3>
              <p className="text-gray-500 max-w-sm mx-auto">Create your first staff member to populate the platform's database.</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse">
                <thead>
                  <tr className="bg-gray-50/80 border-b border-gray-100 text-xs uppercase tracking-wider text-gray-500 font-bold">
                    <th className="px-8 py-5">Personnel</th>
                    <th className="px-8 py-5">Department</th>
                    <th className="px-8 py-5">Site / Location</th>
                    <th className="px-8 py-5 text-right w-32">Actions</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {employees.map((emp) => (
                    <tr key={emp.id} className="hover:bg-blue-50/40 transition-colors group">
                      <td className="px-8 py-5">
                        <div className="flex items-center space-x-5">
                          <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-blue-100 to-indigo-50 text-indigo-700 font-bold flex items-center justify-center shadow-inner border border-white/50 text-lg">
                            {emp.fullName.substring(0, 2).toUpperCase()}
                          </div>
                          <div>
                            <div className="font-bold text-gray-900 group-hover:text-blue-700 transition-colors">{emp.fullName}</div>
                            <div className="text-sm font-medium text-gray-500 mt-1">{emp.jobTitle || 'No Title'} <span className="mx-2 text-gray-300">•</span> {emp.email}</div>
                          </div>
                        </div>
                      </td>
                      <td className="px-8 py-5">
                        <span className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-blue-50 text-blue-700 border border-blue-100">
                          {emp.departmentName} {emp.teamName && emp.teamName !== "Unassigned" ? ` » ${emp.teamName}` : ''}
                        </span>
                      </td>
                      <td className="px-8 py-5">
                        <span className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-emerald-50 text-emerald-700 border border-emerald-100">
                          <svg className="w-3.5 h-3.5 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"/>
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"/>
                          </svg>
                          {emp.siteName}
                        </span>
                      </td>
                      <td className="px-8 py-5 flex items-center justify-end space-x-2">
                        {hasSitePermission("HR.Employee.Edit", emp.siteId) && (
                          <Link href={`/employees/${emp.id}/edit`} className="text-gray-400 hover:text-blue-600 transition-colors font-semibold text-sm bg-white hover:bg-blue-50 px-4 py-2 rounded-lg border border-transparent hover:border-blue-100">
                            Edit
                          </Link>
                        )}
                        {hasSitePermission("HR.Employee.Edit", emp.siteId) && (
                          <form action={deleteEmployeeAction.bind(null, emp.id)}>
                            <button type="submit" className="text-gray-400 hover:text-rose-600 transition-colors font-semibold text-sm bg-white hover:bg-rose-50 px-4 py-2 rounded-lg border border-transparent hover:border-rose-100">Delete</button>
                          </form>
                        )}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </div>
      </div>
    </MainLayout>
  );
}
