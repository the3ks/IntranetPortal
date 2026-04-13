import { fetchWithAuth } from "@/lib/api";
import AdminModulesClient from "./AdminModulesClient";
import MainLayout from "@/components/layout/MainLayout";

export default async function ModulesAdminPage() {
  const res = await fetchWithAuth("/api/admin/modules", { cache: "no-store" });
  const modules = res.ok ? await res.json() : [];

  const sitesRes = await fetchWithAuth("/api/sites", { cache: "no-store" });
  const sites = sitesRes.ok ? await sitesRes.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8 p-6">
        <div>
          <p className="text-foreground/70">Enable or disable modules globally or granularly for specific sites.</p>
        </div>

        <AdminModulesClient initialModules={modules} sites={sites} />
      </div>
    </MainLayout>
  );
}
