import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import { createDepartmentAction, deleteDepartmentAction } from "@/app/actions/departments";
import { createTeamAction, deleteTeamAction } from "@/app/actions/teams";
import Link from "next/link";
import { redirect } from "next/navigation";
import { cookies } from "next/headers";

export default async function DepartmentsAdminPage() {
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
  
  if (!isAdmin) {
    redirect("/");
  }

  const [deptsRes, teamsRes] = await Promise.all([
    fetchWithAuth("/api/departments", { cache: 'no-store' }),
    fetchWithAuth("/api/teams", { cache: 'no-store' })
  ]);

  const departments = deptsRes.ok ? await deptsRes.json() : [];
  const teams = teamsRes.ok ? await teamsRes.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto py-8 space-y-8">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Organization Matrix</h1>
            <p className="mt-2 text-gray-500 text-lg">Manage Core Departments and provision their nested operational Sub-Teams natively.</p>
          </div>
          <Link href="/admin/quick-setup" className="text-indigo-600 font-bold hover:text-indigo-800 bg-indigo-50 px-5 py-2.5 rounded-xl transition-colors">
            Bulk Provisioning Hub &rarr;
          </Link>
        </header>

        {/* Add New Department Form */}
        <div className="bg-white p-6 sm:p-8 rounded-3xl shadow-sm border border-gray-100 flex flex-col md:flex-row items-center gap-6">
          <div className="shrink-0 bg-indigo-100 text-indigo-600 p-4 rounded-2xl">
            <svg className="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" />
            </svg>
          </div>
          <div className="flex-1 w-full">
            <h2 className="text-lg font-bold text-gray-800 mb-2">Provision Root Department</h2>
            <form action={createDepartmentAction} className="flex flex-col sm:flex-row gap-4">
              <input type="text" name="name" required placeholder="e.g. Finance & Accounting" className="flex-1 px-5 py-3.5 rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none text-gray-800 shadow-sm" />
              <button type="submit" className="px-8 py-3.5 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl shadow-md transition-all text-center">
                Initialize Division
              </button>
            </form>
          </div>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {departments.map((dept: any) => (
            <div key={dept.id} className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden flex flex-col transition-all hover:shadow-md">
              <div className="p-6 bg-gradient-to-r from-indigo-50/50 to-blue-50/50 border-b border-gray-100 flex justify-between items-center group">
                <h3 className="text-xl font-bold text-indigo-950 truncate pr-4">{dept.name}</h3>
                <form action={deleteDepartmentAction.bind(null, dept.id)}>
                   <button type="submit" className="text-gray-400 hover:text-rose-600 font-bold px-3 py-1.5 bg-white hover:bg-rose-50 rounded-lg text-sm border border-transparent hover:border-rose-100 transition-colors">Delete</button>
                </form>
              </div>
              
              <div className="p-6 flex-1 flex flex-col bg-white">
                <h4 className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-4">Assigned Operational Channels</h4>
                <ul className="space-y-3 mb-6 flex-1">
                  {teams.filter((t: any) => t.departmentId === dept.id).map((t: any) => (
                    <li key={t.id} className="flex items-center justify-between text-sm p-3.5 font-medium bg-gray-50/80 rounded-xl border border-gray-100">
                      <span className="text-gray-700 truncate">{t.name}</span>
                      <form action={deleteTeamAction.bind(null, t.id)}>
                        <button type="submit" className="p-1.5 text-gray-400 hover:text-rose-500 hover:bg-rose-50 rounded-lg transition-colors ml-2">
                           <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M6 18L18 6M6 6l12 12" /></svg>
                        </button>
                      </form>
                    </li>
                  ))}
                  {teams.filter((t: any) => t.departmentId === dept.id).length === 0 && (
                     <li className="text-sm font-medium text-gray-400 italic py-3 bg-gray-50/50 rounded-xl border border-dashed border-gray-200 text-center">No sub-teams constructed.</li>
                  )}
                </ul>

                <div className="pt-4 border-t border-gray-100 mt-auto">
                   <form action={createTeamAction} className="grid grid-cols-[1fr_auto] gap-3">
                     <input type="hidden" name="departmentId" value={dept.id} />
                     <input type="text" name="name" required placeholder="New Sub-Channel..." className="w-full px-4 py-2.5 text-sm rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none shadow-sm" />
                     <button type="submit" className="px-5 py-2.5 bg-indigo-50 hover:bg-indigo-600 hover:text-white text-indigo-700 font-bold text-sm rounded-xl transition-all shadow-sm">
                       Add
                     </button>
                   </form>
                </div>
              </div>
            </div>
          ))}
        </div>
      </div>
    </MainLayout>
  );
}
