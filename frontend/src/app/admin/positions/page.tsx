import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";
import MainLayout from "@/components/layout/MainLayout";

async function createPosition(formData: FormData) {
  "use server";
  const data = {
    name: formData.get("name"),
    description: formData.get("description"),
  };
  await fetchWithAuth("/api/positions", {
    method: "POST",
    body: JSON.stringify(data)
  });
  revalidatePath("/admin/positions");
}

async function deletePosition(id: number) {
  "use server";
  await fetchWithAuth(`/api/positions/${id}`, { method: "DELETE" });
  revalidatePath("/admin/positions");
}

export default async function PositionsAdminPage() {
  const res = await fetchWithAuth("/api/positions", { cache: "no-store" });
  const positions = res.ok ? await res.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <p className="text-gray-500 mt-2 text-lg">Define the official HR Job Titles available for Organizational Mapping.</p>
          </div>
        </header>

        <div className="space-y-8">
          {/* Creation Panel */}
          <div className="w-full">
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8">
              <h2 className="text-xl font-bold text-gray-900 mb-6">Create New Title</h2>
              <form action={createPosition} className="flex flex-col md:flex-row gap-6 items-start">
                <div className="flex-1 w-full space-y-4">
                  <div className="space-y-2">
                    <label className="text-sm font-bold text-gray-700">Official Title</label>
                    <input type="text" name="name" required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all font-bold text-gray-900" placeholder="e.g. Lead Engineer" />
                  </div>
                  <div className="space-y-2">
                    <label className="text-sm font-bold text-gray-700">Description</label>
                    <input type="text" name="description" className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all text-gray-700" placeholder="Responsibilities overview..." />
                  </div>
                </div>
                <div className="w-full md:w-64 pt-6 md:pt-8 shrink-0 flex flex-col justify-end h-full">
                  <button type="submit" className="w-full py-4 bg-blue-600 hover:bg-blue-700 active:scale-[0.98] text-white font-bold rounded-xl shadow-md transition-all text-center flex items-center justify-center gap-2">
                    <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                    Register Position
                  </button>
                </div>
              </form>
            </div>
          </div>

          {/* Datatable View */}
          <div className="w-full">
            <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
              {positions.length === 0 ? (
                <div className="p-16 text-center">
                  <h3 className="text-xl font-bold text-gray-900 mb-2">No positions registered</h3>
                  <p className="text-gray-500">Use the creation panel to establish your first organizational title.</p>
                </div>
              ) : (
                <table className="w-full text-left border-collapse">
                  <thead>
                    <tr className="bg-gray-50/80 border-b border-gray-100 text-xs uppercase tracking-wider text-gray-500 font-bold">
                      <th className="px-6 py-5">Job Title</th>
                      <th className="px-6 py-5">Description</th>
                      <th className="px-6 py-5 text-right w-48">Actions</th>
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-100">
                    {positions.map((pos: any) => (
                      <tr key={pos.id} className="hover:bg-blue-50/40 transition-colors">
                        <td className="px-6 py-5">
                          <span className="font-bold text-gray-900 border border-gray-200 bg-white shadow-sm px-3 py-1.5 rounded-lg">{pos.name}</span>
                        </td>
                        <td className="px-6 py-5 text-sm text-gray-500 font-medium">
                          {pos.description || <span className="text-gray-300 italic">No description</span>}
                        </td>
                        <td className="px-6 py-5 text-right w-48">
                          <form action={deletePosition.bind(null, pos.id)}>
                            <button type="submit" className="text-rose-600 hover:text-white bg-rose-50 hover:bg-rose-600 px-4 py-2 rounded-xl transition-all font-bold text-sm shadow-sm">
                              Delete Title
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
