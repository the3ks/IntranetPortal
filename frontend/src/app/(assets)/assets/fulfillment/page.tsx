"use client";
import { useState, useEffect } from "react";
import { getFulfillmentQueueAction, fulfillRequestAction, getAssetsListAction } from "@/app/actions/assets";

export default function FulfillmentPage() {
  const [requests, setRequests] = useState<any[]>([]);
  const [availableAssets, setAvailableAssets] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  // Fulfillment State map: request Id -> selected asset Id
  const [selections, setSelections] = useState<Record<number, string>>({});

  const fetchData = async () => {
    setLoading(true);
    const [queueData, assetsData] = await Promise.all([getFulfillmentQueueAction(), getAssetsListAction()]);
    setRequests(queueData);
    setAvailableAssets(assetsData.filter((a: any) => a.status === "Available"));
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleFulfill = async (id: number, type: string) => {
    let payload = {};
    if (type === "HardwareAsset") {
      const selectedAssetId = selections[id];
      if (!selectedAssetId) {
        alert("Please select a physical asset to bind to this request before fulfilling.");
        return;
      }
      payload = { AssignedAssetId: parseInt(selectedAssetId) };
    }

    if (confirm("Permanently transfer custody of this asset? This will update the primary ledger immediately.")) {
      await fulfillRequestAction(id, payload);
      fetchData();
    }
  };

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center bg-card p-4 rounded-xl border border-border/50 shadow-sm">
        <h2 className="text-xl font-bold text-foreground flex items-center gap-3">
          <div className="p-2 bg-rose-50 text-rose-500 rounded-lg">
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" /></svg>
          </div>
          Decentralized Fulfillment Queue
        </h2>
      </div>

      {loading ? (
        <div className="text-center py-20 text-foreground/40">Loading fulfillment queue...</div>
      ) : (
        <div className="bg-card border border-border/50 rounded-2xl shadow-sm overflow-hidden p-6 text-sm">
          <div className="space-y-6">
            {requests.map(r => (
              <div key={r.id} className="border border-border/50 rounded-xl p-5 block sm:flex justify-between items-start bg-background/50/30">
                <div className="mb-4 sm:mb-0">
                  <div className="flex items-center gap-3 mb-2">
                    <span className="font-bold text-foreground border-b border-border pb-0.5">REQ-{r.id.toString().padStart(4, '0')}</span>
                    <span className="px-2.5 py-1 bg-amber-100 text-amber-700 rounded-md text-[10px] font-bold uppercase tracking-widest border border-amber-200">
                      {r.status}
                    </span>
                    <span className="px-2.5 py-1 bg-gray-200 text-foreground/90 rounded-md text-[10px] font-bold uppercase tracking-widest">
                      {r.type}
                    </span>
                  </div>
                  
                  <div className="text-foreground font-medium mb-1">
                    Requested specific <span className="font-bold underline decoration-rose-300 decoration-2">{r.type === 'BulkAccessory' ? r.accessoryName : (r.modelName || r.categoryName)}</span> 
                    {r.type === 'BulkAccessory' && <span className="text-rose-600 font-black ml-1">x {r.quantity}</span>}
                  </div>
                  
                  <div className="text-xs text-foreground/60 mt-2 p-2 bg-card border border-border/50 rounded-lg inline-block shadow-sm">
                    <span className="font-semibold text-foreground/90">Justification: </span> {r.justification}
                  </div>

                  <div className="mt-4 text-xs font-semibold text-foreground/40">
                    Handing over custody to <span className="text-blue-500 italic">{r.requestedForName}</span>
                  </div>
                </div>

                <div className="sm:w-80 flex flex-col items-end gap-3 shrink-0">
                  {r.type === "HardwareAsset" ? (
                    <div className="w-full text-right bg-card p-3 border border-border/50 rounded-xl shadow-sm">
                      <label className="block text-[10px] font-bold text-foreground/40 uppercase tracking-wider mb-2">Select Serial to Fulfill</label>
                      <select 
                        className="w-full border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-rose-400 bg-background/50 font-medium text-foreground focus:bg-card transition-colors"
                        value={selections[r.id] || ""}
                        onChange={(e) => setSelections({...selections, [r.id]: e.target.value})}
                      >
                        <option value="">-- Choose verified inventory --</option>
                        {availableAssets.map(a => (
                          <option key={a.id} value={a.id}>{a.assetTag} - {a.manufacturer} {a.modelName} ({a.serialNumber || 'No SN'})</option>
                        ))}
                      </select>
                    </div>
                  ) : (
                    <div className="text-[10px] text-foreground/40 font-medium bg-card p-2 border border-border/50 rounded-lg w-full text-center">
                      Auto-Deducts {r.quantity} bulk units of {r.accessoryName} on completion.
                    </div>
                  )}

                  <button 
                    onClick={() => handleFulfill(r.id, r.type)} 
                    className="w-full px-5 py-2.5 bg-rose-600 hover:bg-rose-700 text-white font-bold rounded-xl shadow-md transition-all flex items-center justify-center gap-2 group"
                  >
                    Transfer Custody
                    <svg className="w-4 h-4 group-hover:translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
                  </button>
                </div>
              </div>
            ))}

            {requests.length === 0 && (
              <div className="text-center py-16">
                <svg className="w-16 h-16 text-gray-200 mx-auto mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                <div className="text-foreground/60 font-medium">The queue is completely clear.</div>
                <div className="text-xs text-foreground/40 mt-1">No requisitions are currently waiting for your department's fulfillment.</div>
              </div>
            )}
          </div>
        </div>
      )}
    </div>
  );
}
