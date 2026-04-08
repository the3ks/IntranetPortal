"use client";

import { useState, useEffect } from "react";
import { assignRoleAction, removeRoleAction, createDelegationAction, revokeDelegationAction, searchUsersAction, getDelegationsAction } from "@/app/actions/users";

export default function UserManager({ user, roles, sites, departments }: { user: any, roles: any[], sites: any[], departments: any[] }) {
  const [isOpen, setIsOpen] = useState(false);
  const [activeTab, setActiveTab] = useState<'roles' | 'delegations'>('roles');
  const [isPending, setIsPending] = useState(false);
  const [errorMsg, setErrorMsg] = useState<string | null>(null);

  // Role Form State
  const [scopeType, setScopeType] = useState<"global" | "site" | "dept">("global");
  const [selectedSiteId, setSelectedSiteId] = useState<string>("");

  // Delegation Form State
  const [delegations, setDelegations] = useState<any[]>([]);
  const [searchQuery, setSearchQuery] = useState("");
  const [searchResults, setSearchResults] = useState<any[]>([]);
  const [substituteUser, setSubstituteUser] = useState<any | null>(null);

  const availableDepts = scopeType === "dept" && selectedSiteId 
    ? departments.filter(d => d.siteId === parseInt(selectedSiteId))
    : departments;

  useEffect(() => {
    if (isOpen && activeTab === 'delegations') {
      loadDelegations();
    }
    setErrorMsg(null);
  }, [isOpen, activeTab]);

  const loadDelegations = async () => {
    const data = await getDelegationsAction(user.id);
    setDelegations(data || []);
  };

  const handleAssignRole = async (formData: FormData) => {
    setIsPending(true); setErrorMsg(null);
    try {
      const result = await assignRoleAction(formData);
      if (!result?.success) setErrorMsg(result?.error || "Unknown network degradation.");
      else {
        setScopeType("global");
        setSelectedSiteId("");
      }
    } catch (e: any) { setErrorMsg(e.message); } finally { setIsPending(false); }
  };

  const handleRemoveRole = async (mappingId: number) => {
    if (!confirm("Are you certain you wish to revoke this explicitly mapped Capability from this Software Identity?")) return;
    setIsPending(true); setErrorMsg(null);
    try { await removeRoleAction(user.id, mappingId); } 
    catch (e: any) { setErrorMsg(e.message); } finally { setIsPending(false); }
  };

  const handleCreateDelegation = async (formData: FormData) => {
    if (!substituteUser) {
      setErrorMsg("You must explicitly select an async Substitute User from the dynamic dropdown.");
      return;
    }
    setIsPending(true); setErrorMsg(null);
    try {
      formData.set("substituteUserId", substituteUser.id.toString());
      formData.set("sourceUserId", user.id.toString());
      const result = await createDelegationAction(formData);
      if (!result?.success) setErrorMsg(result?.error || "Failed capability proxy generation.");
      else {
        setSubstituteUser(null);
        setSearchQuery("");
        loadDelegations();
      }
    } catch (e: any) { setErrorMsg(e.message); } finally { setIsPending(false); }
  };

  const handleRevokeDelegation = async (id: number) => {
    setIsPending(true); setErrorMsg(null);
    try {
      const result = await revokeDelegationAction(id);
      if (!result?.success) setErrorMsg(result?.error || "Failed revocation.");
      else loadDelegations();
    } catch (e: any) { setErrorMsg(e.message); } finally { setIsPending(false); }
  };

  const handleSearchUsers = async (val: string) => {
    setSearchQuery(val);
    if (val.length < 2) { setSearchResults([]); return; }
    const res = await searchUsersAction(val);
    setSearchResults(res.filter((u: any) => u.id !== user.id));
  };

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
        className="px-6 py-2.5 rounded-xl border border-border/50 bg-card text-foreground/90 font-bold shadow-sm hover:border-indigo-300 hover:text-indigo-700 hover:bg-indigo-50 hover:shadow-md transition-all text-sm flex items-center gap-2 whitespace-nowrap"
      >
        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
        Manage Security
      </button>

      {isOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
          <div className="absolute inset-0 bg-gray-900/60 backdrop-blur-sm" onClick={() => setIsOpen(false)}></div>
          <div className="relative bg-card rounded-3xl shadow-2xl w-full max-w-3xl overflow-hidden animate-in fade-in flex flex-col max-h-[90vh]">
            
            <div className="p-8 border-b border-border/50 flex items-center justify-between shrink-0">
              <div>
                <h2 className="text-2xl font-black text-foreground">Security & RBAC Controls</h2>
                <p className="text-sm font-medium text-foreground/60 mt-1">Identity operations for <strong className="text-foreground">{user.employeeName}</strong></p>
              </div>
              <button onClick={() => setIsOpen(false)} className="w-10 h-10 bg-background/50 rounded-full flex items-center justify-center text-foreground/40 hover:text-foreground hover:bg-background transition-colors">
                <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>

            {/* TAB SYSTEM */}
            <div className="flex border-b border-border/50 bg-background/50/50 shrink-0 px-8 pt-4 space-x-6">
              <button
                onClick={() => setActiveTab('roles')}
                className={`pb-4 text-sm font-bold uppercase tracking-wider relative transition-colors ${activeTab === 'roles' ? 'text-indigo-600' : 'text-foreground/40 hover:text-foreground/80'}`}
              >
                Capability Scopes
                {activeTab === 'roles' && <div className="absolute bottom-0 left-0 right-0 h-1 bg-indigo-600 rounded-t-full"></div>}
              </button>
              <button
                onClick={() => setActiveTab('delegations')}
                className={`pb-4 text-sm font-bold uppercase tracking-wider relative transition-colors ${activeTab === 'delegations' ? 'text-blue-600' : 'text-foreground/40 hover:text-foreground/80'}`}
              >
                Temporary Delegations
                {activeTab === 'delegations' && <div className="absolute bottom-0 left-0 right-0 h-1 bg-blue-600 rounded-t-full"></div>}
              </button>
            </div>

            <div className="flex-1 overflow-y-auto p-8 space-y-8 bg-background/50/50">
              {errorMsg && (
                <div className="bg-rose-50 text-rose-700 p-4 border border-rose-100 rounded-xl text-sm font-bold flex items-start gap-3">
                  <svg className="w-5 h-5 shrink-0" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                  {errorMsg}
                </div>
              )}

              {activeTab === 'roles' && (
                <div className="space-y-8 animate-in slide-in-from-right-4 fade-in duration-200">
                  <div className="space-y-4">
                    <h3 className="text-sm font-black text-foreground uppercase tracking-widest flex items-center gap-2"><div className="w-2 h-2 rounded-full bg-emerald-500"></div> Active Execution Bindings</h3>
                    <div className="bg-card border text-left border-border/50 rounded-2xl overflow-hidden shadow-sm">
                      {user.roles.length === 0 ? (
                        <div className="p-6 text-center text-sm font-bold text-foreground/40">Standard Issue Employee (No special clearance mapped)</div>
                      ) : (
                        <div className="divide-y divide-border/50">
                          {user.roles.map((r: any) => (
                            <div key={r.id} className="p-4 flex flex-col sm:flex-row sm:items-center justify-between gap-4 hover:bg-background/50">
                              <div>
                                <div className="font-bold text-foreground text-base">{r.roleName}</div>
                                <div className="text-xs font-semibold text-indigo-600 mt-0.5 uppercase tracking-wider">
                                  {r.departmentName ? `${r.departmentName} Dept (Hierarchical)` : (r.siteName === 'Global Scope' ? 'Global Capability' : `${r.siteName} (Functional)`)}
                                </div>
                              </div>
                              <button disabled={isPending} onClick={() => handleRemoveRole(r.id)} className="shrink-0 text-sm font-bold px-4 py-2 rounded-xl text-rose-600 bg-rose-50 border border-rose-100 hover:bg-rose-100 transition-colors disabled:opacity-50">Revoke Matrix</button>
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  </div>

                  <div className="space-y-4 text-left">
                    <h3 className="text-sm font-black text-foreground uppercase tracking-widest flex items-center gap-2"><div className="w-2 h-2 rounded-full bg-indigo-500"></div> Assign New Role</h3>
                    <form action={handleAssignRole} className="bg-card p-6 rounded-2xl border border-border/50 shadow-sm space-y-5">
                      <input type="hidden" name="userId" value={user.id} />
                      <div className="space-y-2">
                        <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Analytical Capability</label>
                        <select name="roleId" required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-indigo-500 outline-none shadow-sm font-medium">
                          <option value="">Select Security Role...</option>
                          {roles.map((r: any) => (<option key={r.id} value={r.id}>{r.name} - {r.description}</option>))}
                        </select>
                      </div>

                      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Scope Target</label>
                          <select value={scopeType} onChange={(e) => setScopeType(e.target.value as any)} className="w-full px-4 py-3 rounded-xl border border-border/50 font-bold bg-background/50">
                            <option value="global">Global Definition</option>
                            <option value="site">Functional (Select Site)</option>
                            <option value="dept">Hierarchical (Select Department)</option>
                          </select>
                        </div>
                        
                        {(scopeType === 'site' || scopeType === 'dept') && (
                          <div className="space-y-2">
                            <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Physical Site Array</label>
                            <select name={scopeType === 'site' ? "siteId" : ""} value={selectedSiteId} onChange={(e) => setSelectedSiteId(e.target.value)} required className="w-full px-4 py-3 rounded-xl border border-indigo-200 focus:ring-2 focus:ring-indigo-500">
                              <option value="">Select Primary Site...</option>
                              {sites.map((s: any) => (<option key={s.id} value={s.id}>{s.name}</option>))}
                            </select>
                          </div>
                        )}
                      </div>

                      {scopeType === 'dept' && (
                        <div className="space-y-2 animate-in fade-in duration-200">
                          <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Organizational Chain</label>
                          <select name="departmentId" required className="w-full px-4 py-3 rounded-xl border border-blue-200 focus:ring-2 focus:ring-blue-500 disabled:opacity-50" disabled={!selectedSiteId}>
                            <option value="">{selectedSiteId ? "Select Target Department..." : "Awaiting Site Linkage..."}</option>
                            {availableDepts.map((d: any) => (<option key={d.id} value={d.id}>{d.name}</option>))}
                          </select>
                        </div>
                      )}

                      <button type="submit" disabled={isPending} className="w-full py-4 bg-gray-900 hover:bg-black text-white font-bold rounded-xl shadow-lg transition-all active:scale-95 disabled:opacity-70">Enforce Capability Form</button>
                    </form>
                  </div>
                </div>
              )}

              {activeTab === 'delegations' && (
                <div className="space-y-8 animate-in slide-in-from-left-4 fade-in duration-200">
                  <div className="space-y-4">
                    <h3 className="text-sm font-black text-foreground uppercase tracking-widest flex items-center gap-2"><div className="w-2 h-2 rounded-full bg-blue-500"></div> Active Proxy Subs</h3>
                    <div className="bg-card border text-left border-border/50 rounded-2xl overflow-hidden shadow-sm">
                      {delegations.length === 0 ? (
                        <div className="p-6 text-center text-sm font-bold text-foreground/40">No organizational delegations currently routed for this user.</div>
                      ) : (
                        <div className="divide-y divide-border/50">
                          {delegations.map((d: any) => (
                            <div key={d.id} className={`p-4 flex flex-col sm:flex-row sm:items-center justify-between gap-4 transition-colors ${d.isExpired ? 'opacity-50 grayscale' : 'hover:bg-blue-50/20'}`}>
                              <div>
                                <div className="font-bold text-foreground flex items-center gap-2">
                                  {d.substituteName} 
                                  {d.isExpired ? <span className="text-[10px] uppercase font-black px-2 py-0.5 rounded bg-gray-200 text-foreground/80">Expired</span> : <span className="text-[10px] uppercase font-black px-2 py-0.5 rounded bg-blue-100 text-blue-700">Active</span>}
                                </div>
                                <div className="text-sm font-bold mt-1 text-foreground/90">{d.roleName}</div>
                                <div className="text-xs font-semibold text-foreground/60 mt-0.5">Scope: {d.departmentName ? `${d.departmentName} Dept` : d.siteName}</div>
                                <div className="text-[11px] font-mono text-foreground/60 mt-2">
                                  {new Date(d.startDate).toLocaleDateString()} to {new Date(d.endDate).toLocaleDateString()}
                                </div>
                              </div>
                              <button disabled={isPending} onClick={() => handleRevokeDelegation(d.id)} className="shrink-0 text-sm font-bold px-4 py-2 rounded-xl text-amber-700 bg-amber-50 border border-amber-100 hover:bg-amber-100 transition-colors disabled:opacity-50">Revoke Link</button>
                            </div>
                          ))}
                        </div>
                      )}
                    </div>
                  </div>

                  <div className="space-y-4 text-left">
                    <h3 className="text-sm font-black text-foreground uppercase tracking-widest flex items-center gap-2"><div className="w-2 h-2 rounded-full bg-violet-500"></div> Issue New Delegation</h3>
                    <form action={handleCreateDelegation} className="bg-card p-6 rounded-2xl border border-border/50 shadow-sm space-y-5">
                      <div className="space-y-2">
                        <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Source Capability Array</label>
                        <select name="userRoleId" required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-violet-500 outline-none shadow-sm font-medium">
                          <option value="">Select actively bound Role Matrix...</option>
                          {user.roles.map((r: any) => (<option key={r.id} value={r.id}>{r.roleName} - {r.departmentName ? `${r.departmentName} Dept` : r.siteName}</option>))}
                        </select>
                      </div>

                      <div className="space-y-2 relative">
                        <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Substitute Identity Target</label>
                        {!substituteUser ? (
                          <>
                            <input 
                              type="text" 
                              value={searchQuery}
                              onChange={(e) => handleSearchUsers(e.target.value)}
                              placeholder="Type name or email to construct asynchronous graph..." 
                              className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-violet-500 outline-none shadow-sm"
                            />
                            {searchResults.length > 0 && typeof window !== "undefined" && (
                              <div className="absolute z-10 top-full left-0 right-0 mt-1 bg-card border border-border/50 rounded-xl shadow-xl max-h-48 overflow-y-auto w-full">
                                {searchResults.map((u: any) => (
                                  <div key={u.id} onClick={() => { setSubstituteUser(u); setSearchResults([]); setSearchQuery(""); }} className="p-3 border-b border-gray-50 hover:bg-background/50 cursor-pointer">
                                    <div className="font-bold text-sm text-foreground">{u.employeeName}</div>
                                    <div className="text-xs text-foreground/60">{u.email}</div>
                                  </div>
                                ))}
                              </div>
                            )}
                          </>
                        ) : (
                          <div className="flex items-center justify-between p-3 border border-violet-200 bg-violet-50 rounded-xl">
                            <div>
                              <div className="font-bold text-violet-900">{substituteUser.employeeName}</div>
                              <div className="text-xs text-violet-600">{substituteUser.email}</div>
                            </div>
                            <button type="button" onClick={() => setSubstituteUser(null)} className="text-violet-400 hover:text-violet-700"><svg className="w-5 h-5" fill="none" viewBox="0 0 24 24" stroke="currentColor"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg></button>
                          </div>
                        )}
                      </div>

                      <div className="grid grid-cols-2 gap-4">
                        <div className="space-y-2">
                          <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Activation Date</label>
                          <input type="datetime-local" name="startDate" required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-violet-500" />
                        </div>
                        <div className="space-y-2">
                          <label className="text-xs font-bold text-foreground/90 uppercase tracking-wider">Expiration Date</label>
                          <input type="datetime-local" name="endDate" required className="w-full px-4 py-3 rounded-xl border border-border/50 focus:ring-2 focus:ring-violet-500" />
                        </div>
                      </div>

                      <button type="submit" disabled={isPending || !substituteUser} className="w-full py-4 bg-violet-600 hover:bg-violet-700 text-white font-bold rounded-xl shadow-lg transition-all active:scale-95 disabled:opacity-50">Generate Delegation Proxy</button>
                    </form>
                  </div>
                </div>
              )}

            </div>
          </div>
        </div>
      )}
    </>
  );
}
