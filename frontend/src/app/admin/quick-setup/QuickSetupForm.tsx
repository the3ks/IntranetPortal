"use client";

import { useState, useRef, useEffect } from "react";
import { submitQuickSetupAction, importEmployeesCsvAction } from "@/app/actions/setup";

export default function QuickSetupForm() {
  const [loading, setLoading] = useState(false);
  const [loadingCsv, setLoadingCsv] = useState(false);
  const [success, setSuccess] = useState<any>(null);
  const [csvSuccess, setCsvSuccess] = useState<any>(null);
  const resultsRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (csvSuccess) {
      setTimeout(() => {
        resultsRef.current?.scrollIntoView({ behavior: "smooth", block: "start" });
      }, 100);
    }
  }, [csvSuccess]);

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

      <div className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8 space-y-6 mt-8 relative overflow-hidden">
        <div className="absolute top-0 right-0 w-32 h-32 bg-blue-50 rounded-bl-full -mr-10 -mt-10 pointer-events-none"></div>
        <header className="relative z-10">
          <h2 className="text-2xl font-extrabold text-gray-900 tracking-tight">Batch Employee Importer (CSV)</h2>
          <p className="text-gray-500 mt-1">Upload a CSV payload to mass-generate geographic matrices and HR dependencies identically. Natively supports up to 5,000 strings processing synchronously.</p>
        </header>

        <div className="flex flex-col sm:flex-row items-center gap-4 relative z-10">
          <a 
            href={`data:text/csv;charset=utf-8,${encodeURIComponent("FullName,Email,Position,Department,Team,Site\nJohn Doe,john.doe@company.com,Software Engineer,IT,App Team,Head Office\nJane Smith,jane.smith@company.com,HR Specialist,Human Resources,,Head Office")}`}
            download="employee_template.csv"
            className="px-6 py-3.5 bg-gray-50 hover:bg-gray-100 text-gray-800 font-bold rounded-xl transition-all shadow-sm flex items-center gap-2 border border-gray-200"
          >
            <svg className="w-5 h-5 text-gray-500" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 16v1a3 3 0 003 3h10a3 3 0 003-3v-1m-4-4l-4 4m0 0l-4-4m4 4V4" /></svg>
            Download CSV Template
          </a>
          
          <form 
            action={async (formData) => {
              setLoadingCsv(true);
              setCsvSuccess(null);
              try {
                const resText = await importEmployeesCsvAction(formData);
                const res = JSON.parse(resText);
                if (res.success) {
                  setCsvSuccess(res.data);
                } else {
                  alert("Failed to parse structural CSV grid organically: " + res.error);
                }
              } catch (e) {
                alert("Network boundary failure mathematically intercepting CSV block.");
              }
              setLoadingCsv(false);
            }} 
            className="flex-1 flex gap-3 w-full"
          >
            <input type="file" name="file" accept=".csv" required className="flex-1 px-4 py-2.5 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 bg-white text-gray-700 font-medium file:mr-4 file:py-2 file:px-4 file:rounded-full file:border-0 file:text-sm file:font-bold file:bg-blue-50 file:text-blue-700 hover:file:bg-blue-100 transition-all cursor-pointer" />
            <button type="submit" disabled={loadingCsv} className="px-8 py-3.5 bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-xl shadow-md transition-all disabled:opacity-50 min-w-[160px]">
              {loadingCsv ? "Processing..." : "Upload Payload"}
            </button>
          </form>
        </div>

        {csvSuccess && csvSuccess.employees && (
          <div ref={resultsRef} className="mt-8 pt-6 border-t border-gray-100 animate-in fade-in slide-in-from-top-4 duration-500">
            <div className="flex justify-between items-center mb-4">
              <h3 className="font-bold text-gray-900 text-lg flex items-center gap-2">
                <div className="w-2.5 h-2.5 rounded-full bg-blue-500"></div>
                Successfully Evaluated {csvSuccess.employees.length} Rows
              </h3>
              <div className="flex gap-3 text-sm font-semibold">
                <span className="text-emerald-700 bg-emerald-50 px-3 py-1 rounded-full border border-emerald-100">{csvSuccess.inserted} Inserted</span>
                <span className="text-blue-700 bg-blue-50 px-3 py-1 rounded-full border border-blue-100">{csvSuccess.updated} Updated</span>
                <span className="text-gray-600 bg-gray-100 px-3 py-1 rounded-full border border-gray-200">{csvSuccess.skipped} Skipped</span>
              </div>
            </div>
            
            <div className="overflow-x-auto rounded-xl border border-gray-200">
              <table className="min-w-full divide-y divide-gray-200 text-sm">
                <thead className="bg-gray-50">
                  <tr>
                    <th scope="col" className="px-4 py-3 text-left font-bold text-gray-500 uppercase tracking-wider">Name</th>
                    <th scope="col" className="px-4 py-3 text-left font-bold text-gray-500 uppercase tracking-wider">Position</th>
                    <th scope="col" className="px-4 py-3 text-left font-bold text-gray-500 uppercase tracking-wider">Department</th>
                    <th scope="col" className="px-4 py-3 text-left font-bold text-gray-500 uppercase tracking-wider">Site</th>
                    <th scope="col" className="px-4 py-3 text-right font-bold text-gray-500 uppercase tracking-wider">Status</th>
                  </tr>
                </thead>
                <tbody className="bg-white divide-y divide-gray-100">
                  {csvSuccess.employees.map((emp: any, idx: number) => (
                    <tr key={idx} className="hover:bg-gray-50 transition-colors">
                      <td className="px-4 py-3 whitespace-nowrap">
                        <div className="font-bold text-gray-900">{emp.fullName}</div>
                        <div className="text-gray-500 text-xs">{emp.email}</div>
                      </td>
                      <td className="px-4 py-3 whitespace-nowrap font-medium text-gray-700">{emp.positionName}</td>
                      <td className="px-4 py-3 whitespace-nowrap font-medium text-gray-700">{emp.departmentName}</td>
                      <td className="px-4 py-3 whitespace-nowrap font-medium text-gray-700">{emp.siteName}</td>
                      <td className="px-4 py-3 whitespace-nowrap text-right font-bold">
                        {emp.action === "Inserted" && <span className="text-emerald-600">New</span>}
                        {emp.action === "Updated" && <span className="text-blue-600">Updated</span>}
                        {emp.action === "Skipped" && <span className="text-gray-400 font-medium">Unchanged</span>}
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </div>

    </div>
  );
}
