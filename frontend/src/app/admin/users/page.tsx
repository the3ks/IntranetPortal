import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import SearchFilter from "@/components/SearchFilter";
import UserManager from "./UserManager";

export default async function UsersPage({ searchParams }: { searchParams: Promise<{ search?: string, filter?: string }> }) {
  const resolvedParams = await searchParams;
  const search = resolvedParams?.search || "";
  const filter = resolvedParams?.filter || "elevated"; // default to Elevated Rights

  // Parallel fetch Users, Roles, and Sites natively
  const [usersRes, rolesRes, sitesRes] = await Promise.all([
    fetchWithAuth(`/api/users?search=${encodeURIComponent(search)}&filter=${encodeURIComponent(filter)}`, { cache: 'no-store' }),
    fetchWithAuth("/api/roles", { cache: 'no-store' }),
    fetchWithAuth("/api/sites", { cache: 'no-store' })
  ]);

  let users = [];
  let roles = [];
  let sites = [];

  if (usersRes.ok) users = await usersRes.json();
  if (rolesRes.ok) roles = await rolesRes.json();
  if (sitesRes.ok) sites = await sitesRes.json();

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8 py-6">
        <header className="flex flex-col xl:flex-row xl:items-center justify-between gap-6 bg-white p-6 rounded-3xl shadow-sm border border-gray-100">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Access Control</h1>
            <p className="text-gray-500 mt-2 text-base">Manage software logins and geometric role permissions securely.</p>
          </div>
          <div className="flex flex-col sm:flex-row items-start sm:items-center gap-6 divide-y sm:divide-y-0 sm:divide-x divide-gray-200">
            <div className="pt-4 sm:pt-0 sm:pr-6 w-full sm:w-auto">
              <SearchFilter placeholder="Search personnel by name or email..." />
            </div>
            
            <div className="pt-4 sm:pt-0 sm:px-6 flex items-center space-x-2 w-full sm:w-auto">
              <span className="text-xs font-bold text-gray-400 uppercase tracking-wider">Access Scope:</span>
              <div className="bg-gray-100 p-1 rounded-xl flex space-x-1">
                <Link 
                  href={`?filter=elevated${search ? '&search=' + encodeURIComponent(search) : ''}`} 
                  className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all ${filter === 'elevated' ? 'bg-white text-blue-700 shadow-sm' : 'text-gray-500 hover:text-gray-700'}`}
                >
                  Elevated Rights
                </Link>
                <Link 
                  href={`?filter=basic${search ? '&search=' + encodeURIComponent(search) : ''}`} 
                  className={`px-4 py-2 rounded-lg text-sm font-semibold transition-all ${filter === 'basic' ? 'bg-white text-gray-900 shadow-sm' : 'text-gray-500 hover:text-gray-700'}`}
                >
                  Basic Logins
                </Link>
              </div>
            </div>
          </div>
        </header>

        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
          {users.length === 0 ? (
            <div className="p-16 text-center">
              <div className="w-20 h-20 mx-auto bg-gray-50 text-gray-300 rounded-full flex items-center justify-center mb-6">
                <svg className="w-10 h-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 15v2m-6 4h12a2 2 0 002-2v-6a2 2 0 00-2-2H6a2 2 0 00-2 2v6a2 2 0 002 2zm10-10V7a4 4 0 00-8 0v4h8z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-gray-900 mb-2">No active credentials mapped</h3>
              <p className="text-gray-500 max-w-sm mx-auto">Either no personnel match your search intent, or no users fit this specific operational parameter.</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse">
                <thead>
                  <tr className="bg-gray-50/80 border-b border-gray-100 text-xs uppercase tracking-wider text-gray-500 font-bold">
                    <th className="px-8 py-5">Software Identity</th>
                    <th className="px-8 py-5">Structural Bindings</th>
                    <th className="px-8 py-5 text-right w-64">Security Operations</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-gray-100">
                  {users.map((acc: any) => (
                    <tr key={acc.id} className="hover:bg-blue-50/30 transition-colors group">
                      <td className="px-8 py-5">
                        <div className="flex items-center space-x-4">
                          <div className="w-10 h-10 rounded-xl bg-gray-900 text-white font-bold flex items-center justify-center shadow-md border border-white/50">
                            {acc.employeeName !== "System Account" ? acc.employeeName.substring(0, 2).toUpperCase() : "SA"}
                          </div>
                          <div>
                            <div className="font-bold text-gray-900 flex items-center gap-2">
                              {acc.employeeName}
                              {!acc.isActive && <span className="text-[10px] uppercase font-black px-2 py-0.5 rounded-full bg-rose-100 text-rose-700">Disabled</span>}
                            </div>
                            <div className="text-sm font-medium text-gray-500 mt-0.5">{acc.email}</div>
                          </div>
                        </div>
                      </td>
                      <td className="px-8 py-5">
                        {acc.roles && acc.roles.length > 0 ? (
                          <div className="flex flex-wrap gap-2">
                            {acc.roles.map((r: any) => (
                              <span key={r.id} className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-indigo-50 text-indigo-700 border border-indigo-100 shadow-sm">
                                <svg className="w-3.5 h-3.5 mr-1.5 opacity-70" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z"/></svg>
                                {r.roleName} <span className="mx-1.5 opacity-40">|</span> <span className="text-indigo-900/60 font-medium">{r.siteName}</span>
                              </span>
                            ))}
                          </div>
                        ) : (
                          <span className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-gray-100 text-gray-500 border border-gray-200">
                            Basic Permissions Only
                          </span>
                        )}
                      </td>
                      <td className="px-8 py-5 flex items-center justify-end">
                        <UserManager user={acc} roles={roles} sites={sites} />
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
