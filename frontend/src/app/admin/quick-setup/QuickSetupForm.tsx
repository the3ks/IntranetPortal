"use client";

import { useState } from "react";
import { submitQuickSetupAction } from "@/app/actions/setup";

export default function QuickSetupForm() {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState<any>(null);

  // Default prefilled state mappings
  const [sites, setSites] = useState("Head Office");
  const [departments, setDepartments] = useState("Human Resources\nInformation Technology\nFinance\nSales\nCustomer Service\nMarketing\nProcurement & Supply Chain\nLogistics & Warehousing\nLegal & Compliance\nBusiness Development\nOperations");
  const [roles, setRoles] = useState("Content Editor\nInventory Manager\nCustomer Support\nHR Generalist\nIT Support Specialist\nFinancial Analyst\nSales Representative\nMarketing Coordinator\nProcurement Officer\nLogistics Coordinator\nLegal Counsel\nBusiness Development Manager\nOperations Supervisor");
  const [positions, setPositions] = useState("Chief Executive Officer\nLead Engineer\nJunior Developer\nAccountant\nHR Generalist\nIT Support Specialist\nFinancial Analyst\nSales Representative\nMarketing Coordinator\nProcurement Officer\nLogistics Coordinator\nLegal Counsel\nBusiness Development Manager\nOperations Supervisor");

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setSuccess(null);

    const parseLines = (text: string) => text.split('\n').map(t => t.trim()).filter(t => t.length > 0);

    const payload = {
      sites: parseLines(sites),
      departments: parseLines(departments),
      roles: parseLines(roles),
      positions: parseLines(positions)
    };

    try {
      // Execute standard React Server Action passing payload natively bypassing frontend cookie locks
      const responseText = await submitQuickSetupAction(payload);
      const result = JSON.parse(responseText);

      if (result.success) {
        setSuccess(result.data);
      } else {
        alert("Failed to execute quick setup: " + result.error);
      }
    } catch (error) {
      console.error(error);
      alert("Critical network failure reaching setup endpoint.");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="max-w-7xl mx-auto space-y-8 py-6">
      <header className="flex flex-col sm:flex-row justify-between gap-4">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Rapid Infrastructure Seeding</h1>
          <p className="text-gray-500 mt-2 text-lg">Bulk insert organizational dictionaries simultaneously. Exactly one value per line.</p>
        </div>
      </header>

      {success && (
        <div className="bg-emerald-50 text-emerald-800 p-6 rounded-2xl border border-emerald-200 shadow-sm flex items-center justify-between animate-fade-in">
          <div>
            <h3 className="font-bold text-lg mb-1">Batch Seeding Successful!</h3>
            <p className="text-sm">Added new entries gracefully: <strong className="text-emerald-900">{success.stats?.sites}</strong> Sites, <strong className="text-emerald-900">{success.stats?.departments}</strong> Departments, <strong className="text-emerald-900">{success.stats?.roles}</strong> Roles, and <strong className="text-emerald-900">{success.stats?.positions}</strong> Positions.</p>
          </div>
          <button onClick={() => setSuccess(null)} className="text-emerald-600 hover:text-emerald-900 font-bold px-4 py-2 bg-emerald-100 rounded-lg transition-colors">Dismiss</button>
        </div>
      )}

      <form onSubmit={handleSubmit} className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8 space-y-8">
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">

          <div className="space-y-3">
            <label className="text-sm font-bold text-gray-900 flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-blue-500"></div> Physical Sites
            </label>
            <textarea
              rows={12}
              value={sites}
              onChange={e => setSites(e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm text-sm"
              placeholder="1 Head Office&#10;New York Branch"
            />
          </div>

          <div className="space-y-3">
            <label className="text-sm font-bold text-gray-900 flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-purple-500"></div> Departments
            </label>
            <textarea
              rows={12}
              value={departments}
              onChange={e => setDepartments(e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-purple-500 focus:border-purple-500 outline-none transition-all shadow-sm text-sm"
              placeholder="Human Resources"
            />
          </div>

          <div className="space-y-3">
            <label className="text-sm font-bold text-gray-900 flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-emerald-500"></div> HR Job Titles
            </label>
            <textarea
              rows={12}
              value={positions}
              onChange={e => setPositions(e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-emerald-500 focus:border-emerald-500 outline-none transition-all shadow-sm text-sm"
              placeholder="Backend Developer"
            />
          </div>

          <div className="space-y-3">
            <label className="text-sm font-bold text-gray-900 flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-indigo-500"></div> Security Roles
            </label>
            <textarea
              rows={12}
              value={roles}
              onChange={e => setRoles(e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all shadow-sm text-sm"
              placeholder="Content Editor"
            />
          </div>

        </div>

        <div className="pt-4 border-t border-gray-100 flex justify-end">
          <button disabled={loading} type="submit" className="w-full md:w-auto px-10 py-4 bg-gray-900 hover:bg-black text-white font-bold rounded-xl shadow-lg hover:shadow-xl transition-all disabled:opacity-50 disabled:cursor-wait">
            {loading ? "Transmitting Batch Stream..." : "Deploy Structural Dictionaries"}
          </button>
        </div>
      </form>
    </div>
  );
}
