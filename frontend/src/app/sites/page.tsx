import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import { createSiteAction, deleteSiteAction, updateSiteAction } from "@/app/actions/sites";
import Link from "next/link";
import { redirect } from "next/navigation";
import { cookies } from "next/headers";

export default async function SitesAdminPage() {
  const cookieStore = await cookies();
  const token = cookieStore.get("auth_token")?.value || "";
  let user: any = null;
  if (token) {
    try {
      user = JSON.parse(Buffer.from(token.split('.')[1], 'base64').toString());
    } catch { }
  }

  const userRoleClaim = user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || user?.role || user?.Role;
  const isAdmin = userRoleClaim === "Admin" || (Array.isArray(userRoleClaim) && userRoleClaim.includes("Admin"));
  const permissionsClaim = user?.Permission || user?.permission || [];
  const permissions = Array.isArray(permissionsClaim) ? permissionsClaim : [permissionsClaim];

  const canCreate = isAdmin || permissions.includes("Structure.Site.Create");
  const canEdit = isAdmin || permissions.includes("Structure.Site.Edit");
  const canDelete = isAdmin || permissions.includes("Structure.Site.Delete");

  const res = await fetchWithAuth("/api/sites", { cache: 'no-store' });
  const sites = res.ok ? await res.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto py-8 space-y-8">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <p className="mt-2 text-foreground/60 text-lg">Administrate corporate facilities, global branches, and physical structures mapping Multi-Tenant borders.</p>
          </div>
          <Link href="/admin/quick-setup" className="text-emerald-600 font-bold hover:text-emerald-800 bg-emerald-50 px-5 py-2.5 rounded-xl transition-colors">
            Bulk Provisioning Hub &rarr;
          </Link>
        </header>

        {canCreate && (
          <div className="bg-card p-6 sm:p-8 rounded-3xl shadow-sm border border-border/50 flex flex-col md:flex-row items-center gap-6">
            <div className="shrink-0 bg-emerald-100 text-emerald-600 p-4 rounded-2xl">
              <svg className="w-8 h-8" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" />
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" />
              </svg>
            </div>
            <div className="flex-1 w-full">
              <h2 className="text-lg font-bold text-foreground mb-4">Provision Geographical Site</h2>
              <form action={createSiteAction} className="grid grid-cols-1 sm:grid-cols-[1fr_1fr_auto] gap-4">
                <input type="text" name="name" required placeholder="Site Name (e.g. Tokyo Data Center)" className="px-5 py-2.5 rounded-xl border border-border/50 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none text-foreground shadow-sm" />
                <input type="text" name="address" placeholder="Physical Address (Optional)" className="px-5 py-2.5 rounded-xl border border-border/50 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none text-foreground shadow-sm" />
                <button type="submit" className="px-6 py-2.5 bg-emerald-600 hover:bg-emerald-700 text-white font-semibold rounded-xl shadow-md transition-all flex items-center justify-center">
                  Initialize Site
                </button>
              </form>
            </div>
          </div>
        )}

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {sites.map((site: any) => (
            <div key={site.id} className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden flex flex-col transition-all hover:shadow-md">
              <div className="p-6 bg-gradient-to-r from-emerald-50/50 to-teal-50/50 border-b border-border/50 flex justify-between items-start group relative">
                <div className="flex-1 pr-6">
                  <h3 className="text-xl font-bold text-emerald-950 mb-1">{site.name}</h3>
                  <p className="text-sm font-medium text-emerald-700/80 flex items-start mt-2">
                    <svg className="w-4 h-4 mr-1.5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                    {site.address || "No physical address bound"}
                  </p>
                </div>
                {canDelete && (
                  <form action={deleteSiteAction.bind(null, site.id)}>
                    <button type="submit" className="text-foreground/40 hover:text-rose-600 font-bold p-2 bg-card hover:bg-rose-50 rounded-lg text-sm border border-transparent hover:border-rose-100 transition-colors shadow-sm" title="Delete Site">
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                    </button>
                  </form>
                )}
              </div>

              {canEdit && (
                <div className="px-6 py-5 bg-card border-t border-gray-50 flex-1 flex flex-col justify-end">
                  <form action={updateSiteAction} className="flex flex-col gap-3">
                    <input type="hidden" name="id" value={site.id} />
                    <div className="flex flex-col gap-1.5">
                      <label className="text-xs font-bold text-foreground/40 uppercase tracking-widest">Update Identity</label>
                      <input type="text" name="name" defaultValue={site.name} required className="w-full px-4 py-2 text-sm font-semibold rounded-xl border border-border/50 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none shadow-sm text-foreground/90 bg-background/50 focus:bg-card transition-colors" />
                    </div>
                    <div className="flex gap-2">
                      <input type="text" name="address" defaultValue={site.address || ""} placeholder="Update Address..." className="flex-1 px-4 py-2 text-sm rounded-xl border border-border/50 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none shadow-sm bg-background/50 focus:bg-card transition-colors" />
                      <button type="submit" className="px-5 py-2 bg-emerald-50 hover:bg-emerald-600 hover:text-white text-emerald-700 font-bold text-sm rounded-xl transition-all shadow-sm border border-emerald-100 hover:border-emerald-600">
                        Save
                      </button>
                    </div>
                  </form>
                </div>
              )}
            </div>
          ))}
        </div>
      </div>
    </MainLayout>
  );
}
