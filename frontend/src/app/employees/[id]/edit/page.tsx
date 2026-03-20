import { fetchWithAuth } from "@/lib/api";
import { updateEmployeeAction } from "@/app/actions/employees";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import { notFound } from "next/navigation";

export default async function EditEmployeePage({ params }: { params: { id: string } }) {
  const [empRes, sitesRes, deptsRes, posRes, teamsRes] = await Promise.all([
    fetchWithAuth(`/api/employees/${params.id}`, { cache: 'no-store' }),
    fetchWithAuth("/api/sites"),
    fetchWithAuth("/api/departments"),
    fetchWithAuth("/api/positions"),
    fetchWithAuth("/api/teams")
  ]);

  if (!empRes.ok) notFound();

  const employee = await empRes.json();
  const sites = sitesRes.ok ? await sitesRes.json() : [];
  const depts = deptsRes.ok ? await deptsRes.json() : [];
  const positions = posRes.ok ? await posRes.json() : [];
  const teams = teamsRes.ok ? await teamsRes.json() : [];

  return (
    <MainLayout>
      <div className="max-w-3xl mx-auto py-8">
        <Link href="/employees" className="text-blue-600 hover:text-blue-800 font-medium text-sm flex items-center mb-8 bg-blue-50 w-fit px-4 py-2 rounded-full transition-colors">
          &larr; Back to Employees Registry
        </Link>
        
        <div className="bg-white p-8 sm:p-12 rounded-3xl shadow-sm border border-gray-100">
          <header className="mb-10 pb-8 border-b border-gray-100">
            <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Edit Personnel Profile</h1>
            <p className="mt-2 text-gray-500">Modify demographic or organizational assignment structures securely.</p>
          </header>

          <form action={updateEmployeeAction} className="space-y-6">
            <input type="hidden" name="id" value={employee.id} />
            
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700">Full Name</label>
                <input type="text" name="fullName" defaultValue={employee.fullName} required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm" />
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700">Email Address</label>
                <input type="email" name="email" defaultValue={employee.email} required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm" />
              </div>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700">Official Job Title</label>
              <select name="positionId" defaultValue={employee.positionId || ""} className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-white shadow-sm appearance-none">
                <option value="">Select Organizational Position...</option>
                {positions.map((p: any) => <option key={p.id} value={p.id}>{p.name}</option>)}
              </select>
            </div>

            <div className="grid grid-cols-1 md:grid-cols-3 gap-6 pb-6 mt-4">
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700">Department</label>
                <select name="departmentId" defaultValue={employee.departmentId} required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-white shadow-sm appearance-none">
                  {depts.map((d: any) => <option key={d.id} value={d.id}>{d.name}</option>)}
                </select>
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700">Sub-Team / Channel</label>
                <select name="teamId" defaultValue={employee.teamId || ""} className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-white shadow-sm appearance-none">
                  <option value="">No Active Channel Assignment...</option>
                  {depts.map((d: any) => (
                    <optgroup key={d.id} label={d.name}>
                      {teams.filter((t: any) => t.departmentId === d.id).map((t: any) => (
                        <option key={t.id} value={t.id}>{t.name}</option>
                      ))}
                    </optgroup>
                  ))}
                </select>
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700">Site Boundaries</label>
                <select name="siteId" defaultValue={employee.siteId} required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-white shadow-sm appearance-none">
                  {sites.map((s: any) => <option key={s.id} value={s.id}>{s.name} - {s.address}</option>)}
                </select>
              </div>
            </div>

            <div className="pt-4 flex justify-end">
                <button type="submit" className="w-full md:w-auto px-10 py-3.5 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-xl shadow-md transition-all text-center">
                  Save Changes
                </button>
            </div>
          </form>
        </div>
      </div>
    </MainLayout>
  );
}
