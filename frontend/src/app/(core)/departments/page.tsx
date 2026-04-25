import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import SiteFilter from "@/components/ui/SiteFilter";
import SearchFilter from "@/components/SearchFilter";

// Metadata for standard Next.js Head SEO/titles
export const metadata = {
  title: "Departments | Intranet Portal",
};

interface Department {
  id: number;
  name: string;
  description?: string;
  parentDepartmentId?: number;
  parentDepartmentName?: string;
  managerId?: number;
  managerName?: string;
  siteId?: number;
  siteName?: string;
}

export default async function DepartmentsPage({ searchParams }: { searchParams: Promise<{ siteId?: string, search?: string }> }) {
  const { siteId, search } = await searchParams;

  let departments: Department[] = [];
  let sites: any[] = [];
  
  try {
    const [deptRes, siteRes] = await Promise.all([
      fetchWithAuth("/api/hr/departments", { next: { tags: ["departments"] }, cache: "no-store" }),
      fetchWithAuth("/api/sites", { cache: "no-store" })
    ]);
    
    if (deptRes.ok) departments = await deptRes.json();
    if (siteRes.ok) sites = await siteRes.json();
  } catch (error) {
    console.error("Error connecting to backend API:", error);
  }

  if (search) {
    const q = search.toLowerCase();
    departments = departments.filter((d) =>
      d.name.toLowerCase().includes(q) ||
      (d.managerName || "").toLowerCase().includes(q) ||
      (d.description || "").toLowerCase().includes(q)
    );
  }

  if (siteId) {
    departments = departments.filter(d => d.siteId?.toString() === siteId);
  }

  const visibleSiteIds = new Set(departments.map((d) => d.siteId).filter((s): s is number => typeof s === "number"));
  const filterSites = sites.filter((s) => visibleSiteIds.has(s.id));
  const filterDisabled = filterSites.length <= 1;

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8">
        <header className="bg-card p-6 sm:p-8 rounded-3xl shadow-sm border border-border/50 space-y-6">
          <div className="flex flex-col lg:flex-row lg:items-center justify-end gap-6">
            <div className="flex flex-col sm:flex-row items-center gap-4 w-full lg:w-full justify-end">
              <div className="w-full sm:w-full relative z-20">
                <SearchFilter placeholder="Search departments or manager..." />
              </div>
              <div className="w-full sm:w-auto relative z-10 shrink-0">
                <SiteFilter sites={filterSites} currentSiteId={siteId} disabled={filterDisabled} />
              </div>
            </div>
          </div>
        </header>

        <div className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden">
          {departments.length === 0 ? (
            <div className="p-16 text-center">
              <div className="w-20 h-20 mx-auto bg-indigo-50 text-indigo-300 rounded-full flex items-center justify-center mb-6">
                <svg className="w-10 h-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 7h18M3 12h18M3 17h18" />
                </svg>
              </div>
              <h3 className="text-xl font-bold text-foreground mb-2">No departments found</h3>
              <p className="text-foreground/60 max-w-sm mx-auto">No departments match the current filters.</p>
            </div>
          ) : (
            <div className="overflow-x-auto">
              <table className="w-full text-left border-collapse">
                <thead>
                  <tr className="bg-background/50 border-b border-border/50 text-xs uppercase tracking-wider text-foreground/60 font-bold">
                    <th className="px-8 py-5">Department</th>
                    <th className="px-8 py-5">Manager</th>
                    <th className="px-8 py-5">Parent</th>
                    <th className="px-8 py-5">Site</th>
                  </tr>
                </thead>
                <tbody className="divide-y divide-border/50">
                  {departments.map((dept) => (
                    <tr key={dept.id} className="hover:bg-indigo-50/40 transition-colors group">
                      <td className="px-8 py-5">
                        <div className="font-bold text-foreground group-hover:text-indigo-500 transition-colors">{dept.name}</div>
                        <div className="text-sm text-foreground/60 mt-1">{dept.description || "No description"}</div>
                      </td>
                      <td className="px-8 py-5">
                        <span className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-blue-50 text-blue-700 border border-blue-100">
                          {dept.managerName || "Unassigned"}
                        </span>
                      </td>
                      <td className="px-8 py-5 text-sm text-foreground/70">{dept.parentDepartmentName || "-"}</td>
                      <td className="px-8 py-5">
                        <span className="inline-flex items-center px-3 py-1.5 rounded-lg text-xs font-bold bg-emerald-50 text-emerald-700 border border-emerald-100">
                          {dept.siteName || "-"}
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
