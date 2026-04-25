"use client";
import { useState, useEffect } from "react";

export default function EmployeeRecordsClient() {
  const [employees, setEmployees] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    fetch(`${backendUrl}/api/hr/employees`, { credentials: "include" })
      .then(r => r.ok ? r.json() : [])
      .then(data => { setEmployees(data); setLoading(false); });
  }, []);

  return (
    <div className="max-w-7xl mx-auto space-y-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-black text-foreground">Employee Directory</h1>
          <p className="text-foreground/70">View and manage HR personnel records.</p>
        </div>
        <button className="bg-emerald-600 hover:bg-emerald-500 text-white px-5 py-2.5 rounded-xl font-bold transition-colors">
          + Onboard Employee
        </button>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <div className="w-8 h-8 border-4 border-border/50 border-t-emerald-500 rounded-full animate-spin" />
        </div>
      ) : (
        <div className="bg-card border border-border/50 rounded-3xl overflow-hidden shadow-sm">
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-background/50 border-b border-border/50 text-foreground/60 uppercase text-xs tracking-wider font-bold">
                <th className="p-4 pl-6">Employee</th>
                <th className="p-4">Emp ID</th>
                <th className="p-4">Department</th>
                <th className="p-4">Position</th>
                <th className="p-4 text-right pr-6">Actions</th>
              </tr>
            </thead>
            <tbody>
              {employees.length === 0 ? (
                <tr>
                  <td colSpan={5} className="p-10 text-center text-foreground/50 font-medium">No HR records found.</td>
                </tr>
              ) : employees.map((emp: any) => (
                <tr key={emp.id} className="border-b border-border/30 last:border-0 hover:bg-background/40 transition-colors">
                  <td className="p-4 pl-6 font-bold">{emp.fullName || "Unknown"}</td>
                  <td className="p-4 text-foreground/70 font-mono text-sm">{emp.employeeNumber}</td>
                  <td className="p-4 text-foreground/70">{emp.departmentName || "—"}</td>
                  <td className="p-4 text-foreground/70">{emp.positionName || "—"}</td>
                  <td className="p-4 text-right pr-6">
                    <button className="text-emerald-500 hover:text-emerald-400 font-bold text-sm">View Profile</button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
