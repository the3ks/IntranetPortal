import { fetchWithAuth } from "@/lib/api";
import { createEmployeeAction } from "@/app/actions/employees";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import EmployeeFormClient from "@/app/(core)/employees/EmployeeFormClient";

export default async function NewEmployeePage() {
  const [sitesRes, deptsRes, posRes, teamsRes] = await Promise.all([
    fetchWithAuth("/api/sites"),
    fetchWithAuth("/api/departments"),
    fetchWithAuth("/api/positions"),
    fetchWithAuth("/api/teams")
  ]);

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
        
        <div className="bg-card p-8 sm:p-12 rounded-3xl shadow-sm border border-border/50">
          <header className="mb-10 pb-8 border-b border-border/50">
            <h1 className="text-3xl font-extrabold text-foreground tracking-tight">Onboard Personnel</h1>
            <p className="mt-2 text-foreground/60">Add a new team member and assign their dimensional matrix parameters.</p>
          </header>

          <EmployeeFormClient 
            sites={sites} 
            depts={depts} 
            positions={positions} 
            teams={teams} 
            action={createEmployeeAction} 
          />
        </div>
      </div>
    </MainLayout>
  );
}

