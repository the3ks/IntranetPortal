import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";

interface Employee {
  id: number;
  fullName: string;
  email: string;
  positionName: string;
  departmentId: number;
  departmentName: string;
  teamId: number | null;
  teamName: string | null;
  siteId: number;
  siteName: string;
}

import SiteFilter from "@/components/ui/SiteFilter";
import SearchFilter from "@/components/SearchFilter";

export default async function EmployeesPage({ searchParams }: { searchParams: Promise<{ siteId?: string, search?: string }> }) {
  const { siteId, search } = await searchParams;

  const [res, siteRes] = await Promise.all([
    fetchWithAuth(`/api/hr/employees?search=${search ? encodeURIComponent(search) : ''}`, { cache: 'no-store' }),
    fetchWithAuth("/api/sites", { cache: 'no-store' })
  ]);

  let employees: Employee[] = [];
  let allSites: any[] = [];

  if (res.ok) employees = await res.json();
  if (siteRes.ok) allSites = await siteRes.json();

  if (siteId) {
    employees = employees.filter(e => e.siteId.toString() === siteId);
  }

  const visibleSiteIds = new Set(employees.map((e) => e.siteId));
  const filterSites = allSites.filter((s) => visibleSiteIds.has(s.id));
  const filterDisabled = filterSites.length <= 1;

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8">
        <header className="bg-card p-6 sm:p-8 rounded-3xl shadow-sm border border-border/50 space-y-6">
          <div className="flex flex-col lg:flex-row lg:items-center justify-end gap-6">
            <div className="flex flex-col sm:flex-row items-center gap-4 w-full lg:w-full justify-end">
              <div className="w-full sm:w-full relative z-20">
                <SearchFilter placeholder="Search personnel by name or email..." />
              </div>
              <div className="w-full sm:w-auto relative z-10 shrink-0">
                <SiteFilter sites={filterSites} currentSiteId={siteId} disabled={filterDisabled} />
              </div>
            </div>
          </div>
        </header>

        <div className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden">
          {employees.length === 0 ? (
            <div className="p-16 text-center">
              <div className="w-20 h-20 mx-auto bg-blue-50 text-blue-300 rounded-full flex items-center justify-center mb-6">
                <svg className="w-10 h-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-foreground mb-2">No employees found</h3>
              <p className="text-foreground/60 max-w-sm mx-auto">Create your first staff member to populate the platform's database.</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse">
                <thead>
                  <tr className="bg-background/50 border-b border-border/50 text-xs uppercase tracking-wider text-foreground/60 font-bold">
                    <th className="px-8 py-5">Personnel</th>
                    <th className="px-8 py-5">Department</th>
                    <th className="px-8 py-5">Site / Location</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-border/50">
                  {employees.map((emp) => (
                    <tr key={emp.id} className="hover:bg-blue-50/40 transition-colors group">
                      <td className="px-8 py-5">
                        <div className="flex items-center space-x-5">
                          <div className="w-12 h-12 rounded-2xl bg-gradient-to-br from-blue-100 to-indigo-50 text-indigo-700 font-bold flex items-center justify-center shadow-inner border border-white/50 text-lg">
                            {emp.fullName.substring(0, 2).toUpperCase()}
                          </div>
                          <div>
                            <div className="font-bold text-foreground group-hover:text-blue-500 transition-colors">{emp.fullName}</div>
                            <div className="text-sm font-medium text-foreground/60 mt-1">{emp.positionName || 'No Title'} <span className="mx-2 text-border">•</span> {emp.email}</div>
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
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
                          </svg>
                          {emp.siteName}
                        </span>
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
