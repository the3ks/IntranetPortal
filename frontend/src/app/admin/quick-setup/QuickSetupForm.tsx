"use client";

import { useState } from "react";
import { submitQuickSetupAction } from "@/app/actions/setup";

export default function QuickSetupForm() {
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState<any>(null);

  // Default prefilled state mappings
  const [siteName, setSiteName] = useState("Head Office");
  const [departments, setDepartments] = useState("Human Resources\nInformation Technology\nFinance\nSales\nCustomer Service\nMarketing\nProcurement & Supply Chain\nLogistics & Warehousing\nLegal & Compliance\nBusiness Development\nOperations");
  const [roles, setRoles] = useState("Content Editor\nInventory Manager\nCustomer Support\nHR Generalist\nIT Support Specialist\nFinancial Analyst\nSales Representative\nMarketing Coordinator\nProcurement Officer\nLogistics Coordinator\nLegal Counsel\nBusiness Development Manager\nOperations Supervisor");
  const [positions, setPositions] = useState("Chief Executive Officer\nLead Engineer\nJunior Developer\nAccountant\nHR Generalist\nIT Support Specialist\nFinancial Analyst\nSales Representative\nMarketing Coordinator\nProcurement Officer\nLogistics Coordinator\nLegal Counsel\nBusiness Development Manager\nOperations Supervisor");

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault();
    setLoading(true);
    setSuccess(null);

    const parseLines = (text: string) => text.split('\n').map(t => t.trim()).filter(t => t.length > 0);

    const payload = {
      sites: [siteName.trim()].filter(t => t.length > 0),
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
              <div className="w-2 h-2 rounded-full bg-blue-500"></div> Physical Site (Required)
            </label>
            <input
              type="text"
              required
              value={siteName}
              onChange={e => setSiteName(e.target.value)}
              className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm text-sm"
              placeholder="e.g. Head Office"
            />
          </div>

          <div className="space-y-3">
            <label className="text-sm font-bold text-gray-900 flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-purple-500"></div> Departments
            </label>
            <p className="text-xs text-gray-500">Departments securely bind to the <strong>Physical Site</strong> specified above.</p>
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

        <div className="pt-6 border-t border-gray-100">
          
          <div className="bg-blue-50 border border-blue-100 p-5 rounded-2xl mb-8 flex gap-4 text-blue-900">
            <svg className="w-6 h-6 flex-shrink-0 text-blue-600 mt-1" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            <div>
              <p className="font-bold text-sm mb-2 opacity-90 tracking-wide uppercase">Architectural Data Bounds Context</p>
              <ul className="list-disc pl-5 space-y-2 text-sm opacity-80">
                <li><strong className="font-semibold text-blue-950">Location:</strong> The Physical Site input is strictly a single variable per deployment ensuring guaranteed localization matrices. If the target Site mathematically exists on the DB, the system elegantly bypasses its creation and snaps back directly onto it.</li>
                <li><strong className="font-semibold text-blue-950">Departments:</strong> Because they intrinsically harbor `NOT NULL` local constraints natively, bulk department arrays fundamentally lock 1:1 specifically against exactly whatever Single Site identity generated above.</li>
                <li><strong className="font-semibold text-blue-950">Administrative Tags (Roles/Positions):</strong> Natively function as massive overarching corporate dictionaries dynamically unbounded universally devoid entirely of targeted physical geography endpoints.</li>
              </ul>
            </div>
          </div>

          <div className="flex justify-end">
            <button disabled={loading} type="submit" className="w-full md:w-auto px-10 py-4 bg-gray-900 hover:bg-black text-white font-bold rounded-xl shadow-lg hover:shadow-xl transition-all disabled:opacity-50 disabled:cursor-wait">
              {loading ? "Transmitting Batch Stream..." : "Deploy Structural Dictionaries"}
            </button>
          </div>
        </div>
      </form>
    </div>
  );
}
