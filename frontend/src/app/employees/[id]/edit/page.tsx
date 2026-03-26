import { fetchWithAuth } from "@/lib/api";
import { updateEmployeeAction } from "@/app/actions/employees";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import { notFound } from "next/navigation";
import EmployeeFormClient from "@/app/employees/EmployeeFormClient";

export default async function EditEmployeePage({ params }: { params: Promise<{ id: string }> }) {
  const { id } = await params;
  
  const [empRes, sitesRes, deptsRes, posRes, teamsRes] = await Promise.all([
    fetchWithAuth(`/api/employees/${id}`, { cache: 'no-store' }),
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

          <EmployeeFormClient 
            employee={employee}
            sites={sites} 
            depts={depts} 
            positions={positions} 
            teams={teams} 
            action={updateEmployeeAction} 
          />
        </div>
      </div>
    </MainLayout>
  );
}
