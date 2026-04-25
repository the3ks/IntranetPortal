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
        <header>
          <p className="mt-2 text-foreground/60 text-lg">Browse corporate facilities, global branches, and physical structures across the organization.</p>
        </header>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {sites.length === 0 ? (
            <div className="col-span-full py-20 text-center bg-card rounded-3xl border border-dashed border-border/50">
              <p className="text-foreground/40 font-medium">No sites have been provisioned in the directory yet.</p>
            </div>
          ) : (
            sites.map((site: any) => (
              <div key={site.id} className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden flex flex-col transition-all hover:shadow-md">
                <div className="p-6 bg-gradient-to-r from-blue-50/50 to-indigo-50/50 border-b border-border/50">
                  <h3 className="text-xl font-bold text-slate-900 mb-1">{site.name}</h3>
                  <p className="text-sm font-medium text-slate-600 flex items-start mt-2">
                    <svg className="w-4 h-4 mr-1.5 mt-0.5 shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                    {site.address || "No physical address bound"}
                  </p>
                </div>
                <div className="px-6 py-4 bg-card/50 flex items-center justify-between">
                  <span className="text-[10px] font-bold text-slate-400 uppercase tracking-widest">Site Identity: #{site.id}</span>
                </div>
              </div>
            ))
          )}
        </div>
      </div>
    </MainLayout>
  );
}
