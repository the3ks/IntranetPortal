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
      <div className="max-w-7xl mx-auto space-y-8">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <p className="text-gray-500 mt-2 text-lg">Administrate Enterprise Security Roles and Data Capability Definitions.</p>
          </div>
        </header>

        <div className="space-y-8">
          {/* Creation Panel */}
          <div className="w-full">
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
              <h2 className="text-xl font-bold text-gray-900 mb-6">Create System Role</h2>
              <form action={createRole} className="flex flex-col md:flex-row gap-6 items-start">
                <div className="flex-1 w-full space-y-4">
                  <div className="space-y-2">
                    <label className="text-sm font-bold text-gray-700">Role Identifier</label>
                    <input type="text" name="name" required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all font-bold text-gray-900" placeholder="e.g. Asset Manager" />
                  </div>
                  <div className="space-y-2">
                    <label className="text-sm font-bold text-gray-700">Security Description</label>
                    <input type="text" name="description" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all text-gray-700" placeholder="Controls warehouse assets..." />
                  </div>
                </div>
                <div className="w-full md:w-64 pt-6 md:pt-8 shrink-0 flex flex-col justify-end h-full">
                  <button type="submit" className="w-full py-4 bg-indigo-600 hover:bg-indigo-700 active:scale-[0.98] text-white font-bold rounded-xl shadow-md transition-all text-center flex items-center justify-center gap-2">
                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                    Establish Role
                  </button>
                </div>
              </form>
            </div>
          </div>

          {/* Datatable View */}
          <div className="w-full">
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
                      <th className="px-6 py-5 text-right w-48">Actions</th>
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
                        <td className="px-6 py-5 text-right w-48">
                          <form action={deleteRole.bind(null, r.id)}>
                            <button disabled={r.name === "Admin"} type="submit" className="disabled:opacity-50 disabled:cursor-not-allowed text-rose-600 hover:text-white bg-rose-50 hover:bg-rose-600 px-4 py-2 rounded-xl transition-all font-bold text-sm shadow-sm">
                              Delete Role
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
