"use client";
import { useState, useEffect } from "react";

export default function LeaveClient() {
  const [requests, setRequests] = useState([]);
  const [approvals, setApprovals] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    Promise.all([
      fetch(`${backendUrl}/api/hr/leave/my-requests`, { credentials: "include" }),
      fetch(`${backendUrl}/api/hr/leave/approvals`, { credentials: "include" })
    ]).then(async ([reqRes, appRes]) => {
      if (reqRes.ok) setRequests(await reqRes.json());
      if (appRes.ok) setApprovals(await appRes.json());
      setLoading(false);
    }).catch(() => setLoading(false));
  }, []);

  return (
    <div className="max-w-7xl mx-auto py-8 space-y-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-black text-foreground">Time Off & Leave</h1>
          <p className="text-foreground/70">Request time off and manage team approvals.</p>
        </div>
        <div className="flex gap-3">
          <button className="bg-background border border-border/50 hover:bg-background/80 text-foreground px-5 py-2.5 rounded-xl font-bold transition-colors">Leave Settings</button>
          <button className="bg-emerald-600 hover:bg-emerald-500 text-white px-5 py-2.5 rounded-xl font-bold transition-colors">+ Request Leave</button>
        </div>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <div className="w-8 h-8 border-4 border-border/50 border-t-emerald-500 rounded-full animate-spin" />
        </div>
      ) : (
        <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
          <div className="bg-card border border-border/50 rounded-3xl p-8 shadow-sm">
            <h2 className="text-2xl font-bold mb-6">My Recent Requests</h2>
            {requests.length === 0 ? (
              <p className="text-foreground/50 italic">You have no recent leave requests.</p>
            ) : (
              <div className="space-y-4">
                {requests.map((r: any) => (
                  <div key={r.id} className="p-4 bg-background/50 rounded-xl border border-border/30 flex justify-between items-center">
                    <div>
                      <h4 className="font-bold">{r.leaveTypeName}</h4>
                      <p className="text-sm text-foreground/60">{new Date(r.startDate).toLocaleDateString()} - {new Date(r.endDate).toLocaleDateString()}</p>
                    </div>
                    <span className={`px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider ${r.status === 'Approved' ? 'bg-emerald-500/20 text-emerald-500' : r.status === 'Rejected' ? 'bg-red-500/20 text-red-500' : 'bg-amber-500/20 text-amber-500'}`}>{r.status}</span>
                  </div>
                ))}
              </div>
            )}
          </div>

          <div className="bg-card border border-border/50 rounded-3xl p-8 shadow-sm">
            <h2 className="text-2xl font-bold mb-6">Pending Approvals</h2>
            {approvals.filter((a: any) => a.status === 'Pending').length === 0 ? (
              <p className="text-foreground/50 italic">No leave requests pending your approval.</p>
            ) : (
              <div className="space-y-4">
                {approvals.filter((a: any) => a.status === 'Pending').map((a: any) => (
                  <div key={a.id} className="p-4 bg-background/50 rounded-xl border border-border/30">
                    <div className="mb-3">
                      <h4 className="font-bold">{a.employeeName}</h4>
                      <p className="text-sm text-foreground/60">{a.leaveTypeName} &bull; {new Date(a.startDate).toLocaleDateString()} to {new Date(a.endDate).toLocaleDateString()}</p>
                    </div>
                    <div className="flex gap-2">
                      <button className="flex-1 bg-emerald-500/10 text-emerald-500 hover:bg-emerald-500/20 py-2 rounded-lg font-bold text-sm transition-colors">Approve</button>
                      <button className="flex-1 bg-red-500/10 text-red-500 hover:bg-red-500/20 py-2 rounded-lg font-bold text-sm transition-colors">Reject</button>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
