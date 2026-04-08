"use client";
import { useState, useEffect } from "react";
import { approveRequestAction, getAssetRequestsAction } from "@/app/actions/assets";

interface AssetRequestsViewProps {
  type: "mine" | "approvals";
}

export default function AssetRequestsView({ type }: AssetRequestsViewProps) {
  const [requests, setRequests] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadRequests();
  }, [type]);

  async function loadRequests() {
    try {
      setLoading(true);
      const data = await getAssetRequestsAction(type);
      setRequests(data);
    } catch (e) {
      console.error(e);
    } finally {
      setLoading(false);
    }
  }

  const handleApprove = async (id: number) => {
    try {
      const res = await approveRequestAction(id);
      if (res.success) {
        loadRequests();
      } else {
        alert("Action failed. You may not have the required permissions.");
      }
    } catch (e) {
      console.error(e);
      alert("An error occurred");
    }
  };

  const StatusBadge = ({ status }: { status: string }) => {
    switch (status) {
      case "PendingApproval":
        return <span className="inline-flex items-center gap-1 px-3 py-1 bg-yellow-100 text-yellow-700 text-xs font-semibold rounded-full"><svg className="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg> Pending Manager</span>;
      case "PendingFulfillment":
        return <span className="inline-flex items-center gap-1 px-3 py-1 bg-blue-100 text-blue-700 text-xs font-semibold rounded-full"><svg className="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg> Awaiting IT</span>;
      case "InProcurement":
        return <span className="inline-flex items-center gap-1 px-3 py-1 bg-purple-100 text-purple-700 text-xs font-semibold rounded-full"><svg className="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg> Ordering</span>;
      case "Fulfilled":
        return <span className="inline-flex items-center gap-1 px-3 py-1 bg-green-100 text-green-700 text-xs font-semibold rounded-full"><svg className="w-3 h-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" /></svg> Fulfilled</span>;
      default:
        return <span className="px-3 py-1 bg-background text-foreground/90 text-xs font-semibold rounded-full">{status}</span>;
    }
  };

  if (loading) return <div className="h-32 flex items-center justify-center text-foreground/40">Loading Requisition Engine...</div>;

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
      
      <div className="flex justify-between items-center bg-background/50 p-6 rounded-2xl border border-border/50">
        <div>
          <h2 className="text-xl font-bold text-foreground">
            {type === "mine" ? "My Requisitions" : "Team Approvals"}
          </h2>
          <p className="text-sm text-foreground/60 mt-1">
            {type === "mine" ? "Track the status of hardware and accessories you requested." : "Review business-need justifications for your department."}
          </p>
        </div>
        {type === "mine" && (
          <button className="flex items-center gap-2 bg-gray-900 text-white px-5 py-2.5 rounded-xl text-sm font-semibold hover:bg-blue-600 transition-colors shadow-sm">
            <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg> New Request
          </button>
        )}
      </div>

      <div className="overflow-hidden bg-card border border-border/50 shadow-sm rounded-3xl">
        <table className="min-w-full divide-y divide-border/50">
          <thead className="bg-background/50 backdrop-blur-sm">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-bold text-foreground/60 uppercase tracking-wider">Item Details</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-foreground/60 uppercase tracking-wider">Requested For</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-foreground/60 uppercase tracking-wider">Status</th>
              <th className="px-6 py-4 text-left text-xs font-bold text-foreground/60 uppercase tracking-wider">Date</th>
              {type === "approvals" && <th className="px-6 py-4 text-right text-xs font-bold text-foreground/60 uppercase tracking-wider">Action</th>}
            </tr>
          </thead>
          <tbody className="bg-card divide-y divide-gray-50">
            {requests.length === 0 ? (
              <tr>
                <td colSpan={5} className="px-6 py-12 text-center text-foreground/40">
                  No requests found in this queue.
                </td>
              </tr>
            ) : (
              requests.map((r) => (
                <tr key={r.id} className="hover:bg-background/50/50 transition-colors">
                  <td className="px-6 py-5 whitespace-nowrap">
                    <div className="text-sm font-bold text-foreground">{r.type === "SerializedAsset" ? (r.modelName || r.categoryName) : r.accessoryName}</div>
                    <div className="text-xs text-foreground/60 truncate max-w-[200px]" title={r.justification}>{r.justification}</div>
                  </td>
                  <td className="px-6 py-5 whitespace-nowrap text-sm text-foreground/90">
                    {r.requestedForName}
                  </td>
                  <td className="px-6 py-5 whitespace-nowrap">
                    <StatusBadge status={r.status} />
                  </td>
                  <td className="px-6 py-5 whitespace-nowrap text-sm text-foreground/60 font-mono">
                    {new Date(r.createdAt).toLocaleDateString()}
                  </td>
                  {type === "approvals" && (
                     <td className="px-6 py-5 whitespace-nowrap text-right text-sm">
                       <button onClick={() => handleApprove(r.id)} className="bg-blue-50 text-blue-700 hover:bg-blue-600 hover:text-white px-4 py-2 rounded-xl font-semibold transition-colors">
                         Approve
                       </button>
                     </td>
                  )}
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
