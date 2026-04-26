"use client";
import { useState, useEffect } from "react";

export default function DepartmentsClient() {
  const [departments, setDepartments] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    fetch(`${backendUrl}/api/hr/departments`, { credentials: "include" })
      .then(r => r.ok ? r.json() : [])
      .then(data => { setDepartments(data); setLoading(false); });
  }, []);

  return (
    <div className="max-w-7xl mx-auto py-8 space-y-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-black text-foreground">Departments</h1>
          <p className="text-foreground/70">Manage the organizational structure and hierarchies.</p>
        </div>
        <button className="bg-emerald-600 hover:bg-emerald-500 text-white px-5 py-2.5 rounded-xl font-bold transition-colors">
          + New Department
        </button>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <div className="w-8 h-8 border-4 border-border/50 border-t-emerald-500 rounded-full animate-spin" />
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {departments.map((dept: any) => (
            <div key={dept.id} className="bg-card border border-border/50 rounded-2xl p-6 shadow-sm hover:shadow-md transition-shadow">
              <h3 className="text-xl font-bold mb-2">{dept.name}</h3>
              <p className="text-foreground/70 text-sm mb-4 line-clamp-2">{dept.description || "No description provided."}</p>
              <div className="text-sm">
                <span className="font-bold text-foreground/60">Manager:</span> {dept.managerName || "Unassigned"}
              </div>
            </div>
          ))}
          {departments.length === 0 && (
            <div className="col-span-full p-10 text-center bg-background/50 rounded-2xl border border-dashed border-border">
              <p className="text-foreground/60 font-medium">No departments have been configured yet.</p>
            </div>
          )}
        </div>
      )}
    </div>
  );
}
