"use client";

import { useState } from "react";

export default function EmployeeFormClient({ 
  employee, 
  sites, 
  depts, 
  positions, 
  teams, 
  action 
}: any) {
  // Default site is the FIRST one in the DB natively mapping the user's explicit request
  const initialSiteId = employee?.siteId || (sites.length > 0 ? sites[0].id : "");
  
  const [selectedSiteId, setSelectedSiteId] = useState<string>(initialSiteId.toString());
  const [selectedDeptId, setSelectedDeptId] = useState<string>(employee?.departmentId?.toString() || "");

  // Clear Department natively when changing geographical limits structurally
  const handleSiteChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedSiteId(e.target.value);
    setSelectedDeptId(""); 
  };

  const handleDeptChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSelectedDeptId(e.target.value);
  };

  // Natively filter internal DTO matrix
  const availableDepts = depts.filter((d: any) => d.siteId.toString() === selectedSiteId);
  const availableTeams = teams.filter((t: any) => t.departmentId.toString() === selectedDeptId);

  return (
    <form action={action} className="space-y-6">
      {employee && <input type="hidden" name="id" value={employee.id} />}
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="space-y-2">
          <label className="text-sm font-bold text-foreground/90">Full Name</label>
          <input type="text" name="fullName" defaultValue={employee?.fullName} required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm" placeholder="e.g. John Doe" />
        </div>
        <div className="space-y-2">
          <label className="text-sm font-bold text-foreground/90">Email Address</label>
          <input type="email" name="email" defaultValue={employee?.email} required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm" placeholder="john.doe@company.com" />
        </div>
      </div>

      <div className="space-y-2">
        <label className="text-sm font-bold text-foreground/90">Official Job Title</label>
        <select name="positionId" defaultValue={employee?.positionId || ""} className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-card shadow-sm appearance-none">
          <option value="">Select Organizational Position...</option>
          {positions.map((p: any) => <option key={p.id} value={p.id}>{p.name}</option>)}
        </select>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 pb-6 mt-4">
        
        <div className="space-y-2">
          <label className="text-sm font-bold text-foreground/90">Geographic Site Boundaries</label>
          <select name="siteId" value={selectedSiteId} onChange={handleSiteChange} required className="w-full px-4 py-3 rounded-xl border border-emerald-200 border-l-4 focus:ring-2 focus:ring-emerald-500 outline-none bg-emerald-50/50 shadow-sm appearance-none font-semibold text-emerald-950">
            {sites.length === 0 && <option value="">No Sites Available</option>}
            {sites.map((s: any) => <option key={s.id} value={s.id}>{s.name} - {s.address || "HQ"}</option>)}
          </select>
        </div>

        <div className="space-y-2">
          <label className="text-sm font-bold text-foreground/90">Department</label>
          <select name="departmentId" value={selectedDeptId} onChange={handleDeptChange} required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-card shadow-sm appearance-none">
            <option value="">Select Department...</option>
            {availableDepts.map((d: any) => <option key={d.id} value={d.id}>{d.name}</option>)}
          </select>
        </div>
        
        <div className="space-y-2">
          <label className="text-sm font-bold text-foreground/90">Sub-Team / Channel</label>
          <select name="teamId" defaultValue={employee?.teamId || ""} className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none bg-card shadow-sm appearance-none">
            <option value="">No Active Channel Assignment...</option>
            {availableTeams.map((t: any) => (
              <option key={t.id} value={t.id}>{t.name}</option>
            ))}
          </select>
        </div>

      </div>

      <div className="pt-6 pb-2 border-t border-border/50 flex items-center justify-between">
        <div>
          <h4 className="text-sm font-bold text-foreground">Software Access Provisioning</h4>
          <p className="text-xs font-medium text-foreground/60">Automatically generate or sustain an active Login Credential for this personnel.</p>
        </div>
        <label className="relative inline-flex items-center cursor-pointer">
          {/* Hidden native input parsed by FormData natively */}
          <input type="checkbox" name="allowLogin" value="true" defaultChecked={employee ? employee.allowLogin : true} className="sr-only peer" />
          <div className="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 rounded-full peer peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-card after:border-border after:border after:rounded-full after:h-5 after:w-5 after:transition-all peer-checked:bg-blue-600"></div>
        </label>
      </div>

      <div className="pt-4 flex justify-end">
          <button type="submit" className="w-full md:w-auto px-10 py-3.5 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-xl shadow-md transition-all text-center">
            {employee ? "Save Personnel Changes" : "Establish Employee Profile"}
          </button>
      </div>
    </form>
  );
}
