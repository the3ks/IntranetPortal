import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";
import MainLayout from "@/components/layout/MainLayout";

async function createRole(formData: FormData) {
  "use server";
  const data = {
    name: formData.get("name"),
    description: formData.get("description"),
  };
  await fetchWithAuth("/api/roles", {
    method: "POST",
    body: JSON.stringify(data)
  });
  revalidatePath("/admin/roles");
}

async function deleteRole(id: number) {
  "use server";
  await fetchWithAuth(`/api/roles/${id}`, { method: "DELETE" });
  revalidatePath("/admin/roles");
}

export default async function RolesAdminPage() {
  const res = await fetchWithAuth("/api/roles", { cache: "no-store" });
  const roles = res.ok ? await res.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8 py-6">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Security Roles Matrix</h1>
            <p className="text-gray-500 mt-2 text-lg">Administrate Enterprise Security Roles and Data Capability Definitions.</p>
          </div>
        </header>

        <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
          {/* Creation Panel */}
          <div className="lg:col-span-1">
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8 sticky top-8">
              <h2 className="text-xl font-bold text-gray-900 mb-6">Create System Role</h2>
              <form action={createRole} className="space-y-5">
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700">Role Identifier</label>
                  <input type="text" name="name" required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all" placeholder="e.g. Asset Manager" />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700">Security Description</label>
                  <textarea name="description" rows={3} className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all" placeholder="Controls warehouse assets..." />
                </div>
                <button type="submit" className="w-full py-3.5 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl shadow-md transition-all text-center">
                  Establish Role
                </button>
              </form>
            </div>
          </div>

          {/* Datatable View */}
          <div className="lg:col-span-2">
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
              {roles.length === 0 ? (
                <div className="p-16 text-center">
                  <h3 className="text-xl font-bold text-gray-900 mb-2">No roles registered</h3>
                </div>
              ) : (
                <table className="w-full text-left border-collapse">
                  <thead>
                    <tr className="bg-gray-50/80 border-b border-gray-100 text-xs uppercase tracking-wider text-gray-500 font-bold">
                      <th className="px-6 py-5">Security Role</th>
                      <th className="px-6 py-5">Description</th>
                      <th className="px-6 py-5 text-right">Actions</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-100">
                    {roles.map((r: any) => (
                      <tr key={r.id} className="hover:bg-indigo-50/40 transition-colors">
                        <td className="px-6 py-5">
                          <span className="font-bold text-indigo-900 border border-indigo-200 bg-indigo-50 shadow-sm px-3 py-1.5 rounded-lg">
                            {r.name}
                          </span>
                        </td>
                        <td className="px-6 py-5 text-sm text-gray-500 font-medium">
                          {r.description || <span className="text-gray-300 italic">No description</span>}
                        </td>
                        <td className="px-6 py-5 text-right">
                          <form action={deleteRole.bind(null, r.id)}>
                            <button disabled={r.name === "Admin"} type="submit" className="disabled:opacity-50 disabled:cursor-not-allowed text-red-500 hover:text-red-700 bg-red-50 hover:bg-red-100 px-3 py-1.5 rounded-lg transition-colors font-semibold text-sm">
                              Delete
                            </button>
                          </form>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              )}
            </div>
          </div>
        </div>
      </div>
    </MainLayout>
  );
}
