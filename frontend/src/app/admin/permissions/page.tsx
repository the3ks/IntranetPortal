import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";

async function getPermissions() {
  try {
    const res = await fetchWithAuth("/api/permissions", { cache: "no-store" });
    if (!res.ok) return [];
    return await res.json();
  } catch {
    return [];
  }
}

export default async function PermissionsPage() {
  const data = await getPermissions();

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-6 py-8">
        <header className="flex flex-col sm:flex-row justify-between gap-4">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">System Permissions Map</h1>
            <p className="text-gray-500 mt-2 text-lg">Read-only matrix defining all registered Developer Constants governing platform execution.</p>
          </div>
        </header>

        <div className="bg-white rounded-2xl shadow-sm border border-gray-100 overflow-hidden">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-gray-50/50 border-b border-gray-100">
                <th className="py-4 px-6 text-xs font-bold text-gray-400 uppercase tracking-wider">Ref ID</th>
                <th className="py-4 px-6 text-xs font-bold text-gray-400 uppercase tracking-wider">Capability Constant</th>
                <th className="py-4 px-6 text-xs font-bold text-gray-400 uppercase tracking-wider">Architecture Description</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {data.map((p: any) => (
                <tr key={p.id} className="hover:bg-gray-50/50 transition-colors">
                  <td className="py-4 px-6 text-sm text-gray-500 font-medium">#{p.id}</td>
                  <td className="py-4 px-6">
                    <span className="bg-rose-100 text-rose-800 text-xs font-bold px-3 py-1 rounded-full border border-rose-200">
                      {p.name}
                    </span>
                  </td>
                  <td className="py-4 px-6 text-sm text-gray-600 font-medium">{p.description}</td>
                </tr>
              ))}
              {data.length === 0 && (
                <tr>
                  <td colSpan={3} className="py-12 px-6 text-center text-gray-500 italic">No permissions successfully retrieved from SQL configuration block.</td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </MainLayout>
  );
}
