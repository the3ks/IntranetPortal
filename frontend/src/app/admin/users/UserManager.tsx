"use client";

import { useState } from "react";
import { assignRoleAction, removeRoleAction } from "@/app/actions/users";

export default function UserManager({ user, roles, sites }: { user: any, roles: any[], sites: any[] }) {
  const [isOpen, setIsOpen] = useState(false);
  const [isPending, setIsPending] = useState(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  const handleAssignRole = async (formData: FormData) => {
    setIsPending(true);
    setErrorMsg(null);
    try {
      const result = await assignRoleAction(formData);
      if (!result?.success) {
        setErrorMsg(result?.error || "Unknown network degradation.");
      }
    } catch (e: any) {
      setErrorMsg(e.message);
    } finally {
      setIsPending(false);
    }
  };

  const handleRemoveRole = async (mappingId: number) => {
    if (!confirm("Are you certain you wish to revoke this explicitly mapped Capability from this Software Identity?")) return;
    setIsPending(true);
    try {
      await removeRoleAction(user.id, mappingId);
    } catch (e: any) {
      setErrorMsg(e.message);
    } finally {
      setIsPending(false);
    }
  };

  return (
    <>
      <button 
        onClick={() => setIsOpen(true)}
        className="px-6 py-2.5 rounded-xl border border-gray-200 bg-white text-gray-700 font-bold shadow-sm hover:border-indigo-300 hover:text-indigo-700 hover:bg-indigo-50 hover:shadow-md transition-all text-sm flex items-center gap-2 whitespace-nowrap"
      >
        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"/><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/></svg>
        Manage Security
      </button>

      {isOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-gray-900/40 backdrop-blur-sm" onClick={() => setIsOpen(false)}></div>
          
          <div className="relative bg-white rounded-3xl shadow-2xl w-full max-w-2xl overflow-hidden animate-in fade-in zoom-in-95 duration-200">
            <div className="p-8 border-b border-gray-100 flex items-center justify-between">
              <div>
                <h2 className="text-2xl font-black text-gray-900">Security & RBAC Controls</h2>
                <p className="text-sm font-medium text-gray-500 mt-1">Modifying operational clearances for <strong className="text-gray-900">{user.employeeName}</strong></p>
              </div>
              <button onClick={() => setIsOpen(false)} className="w-10 h-10 bg-gray-50 rounded-full flex items-center justify-center text-gray-400 hover:text-gray-900 hover:bg-gray-100 transition-colors">
                <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>

            <div className="p-8 space-y-8 bg-gray-50/50">
              
              {errorMsg && (
                <div className="bg-rose-50 text-rose-700 p-4 border border-rose-100 rounded-xl text-sm font-bold flex items-start gap-3">
                  <svg className="w-5 h-5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                  {errorMsg}
                </div>
              )}

              {/* Active Bindings Matrix */}
              <div className="space-y-4">
                <h3 className="text-sm font-black text-gray-900 uppercase tracking-widest flex items-center gap-2">
                  <div className="w-2 h-2 rounded-full bg-emerald-500"></div> Active Execution Bindings
                </h3>
                
                <div className="bg-white border text-left border-gray-200 rounded-2xl overflow-hidden shadow-sm">
                  {user.roles.length === 0 ? (
                    <div className="p-6 text-center text-sm font-bold text-gray-400">
                      Standard Issue Employee (No special clearance mapped)
                    </div>
                  ) : (
                    <div className="divide-y divide-gray-100">
                      {user.roles.map((r: any) => (
                        <div key={r.id} className="p-4 flex flex-col sm:flex-row sm:items-center justify-between gap-4 hover:bg-gray-50 transition-colors">
                          <div>
                            <div className="font-bold text-gray-900 text-base">{r.roleName}</div>
                            <div className="text-xs font-semibold text-indigo-600 mt-0.5 uppercase tracking-wider">{r.siteName}</div>
                          </div>
                          <button 
                            disabled={isPending}
                            onClick={() => handleRemoveRole(r.id)}
                            className="shrink-0 text-sm font-bold px-4 py-2 rounded-xl text-rose-600 bg-rose-50 border border-rose-100 hover:bg-rose-100 transition-colors disabled:opacity-50"
                          >
                            Revoke Matrix
                          </button>
                        </div>
                      ))}
                    </div>
                  )}
                </div>
              </div>

              <div className="h-px bg-gray-200/60"></div>

              {/* Secure Role Injector */}
              <div className="space-y-4 text-left">
                 <h3 className="text-sm font-black text-gray-900 uppercase tracking-widest flex items-center gap-2">
                  <div className="w-2 h-2 rounded-full bg-blue-500"></div> Establish Formal Command Identity
                </h3>
                <form action={handleAssignRole} className="bg-white p-6 rounded-2xl border border-gray-200 shadow-sm space-y-5">
                  <input type="hidden" name="userId" value={user.id} />
                  
                  <div className="space-y-2">
                    <label className="text-xs font-bold text-gray-700 uppercase tracking-wider">Analytical Capability</label>
                    <select name="roleId" required className="w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 outline-none shadow-sm font-medium">
                      <option value="">Select Operational Role...</option>
                      {roles.map((r: any) => (
                        <option key={r.id} value={r.id}>{r.name} - {r.description}</option>
                      ))}
                    </select>
                  </div>

                  <div className="space-y-2">
                    <label className="text-xs font-bold text-gray-700 uppercase tracking-wider">Geographic Boundary Limit</label>
                    <select name="siteId" className="w-full px-4 py-3 rounded-xl border border-indigo-200 border-l-4 focus:ring-2 focus:ring-indigo-500 outline-none shadow-sm font-bold text-indigo-900 bg-indigo-50/30">
                      <option value="">Global Structural Clearance (Unbounded)</option>
                      {sites.map((s: any) => (
                        <option key={s.id} value={s.id}>{s.name} Constraint</option>
                      ))}
                    </select>
                  </div>

                  <button 
                    type="submit" 
                    disabled={isPending}
                    className="w-full py-4 bg-gray-900 hover:bg-black text-white font-bold rounded-xl shadow-lg transition-all active:scale-95 disabled:opacity-70"
                  >
                    {isPending ? "Evaluating Security Graph..." : "Enforce Capability Form"}
                  </button>
                </form>
              </div>

            </div>
          </div>
        </div>
      )}
    </>
  );
}
