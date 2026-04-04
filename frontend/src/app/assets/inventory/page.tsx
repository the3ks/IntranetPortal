"use client";
import { useState, useEffect } from "react";
import { getAssetsListAction, getModelsAction, createAssetAction } from "../../actions/assets";

export default function InventoryPage() {
  const [assets, setAssets] = useState<any[]>([]);
  const [models, setModels] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  // Form State
  const [showForm, setShowForm] = useState(false);
  const [newAsset, setNewAsset] = useState({
    AssetTag: "",
    SerialNumber: "",
    ModelId: "",
    PhysicalLocation: ""
  });

  const fetchData = async () => {
    setLoading(true);
    const [assetData, modelData] = await Promise.all([getAssetsListAction(), getModelsAction()]);
    setAssets(assetData);
    setModels(modelData);
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    await createAssetAction({
      AssetTag: newAsset.AssetTag,
      SerialNumber: newAsset.SerialNumber,
      ModelId: parseInt(newAsset.ModelId),
      PhysicalLocation: newAsset.PhysicalLocation,
      Status: 0 // Available
    });
    setShowForm(false);
    setNewAsset({ AssetTag: "", SerialNumber: "", ModelId: "", PhysicalLocation: "" });
    fetchData();
  };

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center bg-white p-4 rounded-xl border border-gray-100 shadow-sm">
        <h2 className="text-xl font-bold text-gray-800 flex items-center gap-3">
          <div className="p-2 bg-indigo-50 text-indigo-500 rounded-lg">
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 7v10c0 2.21 3.582 4 8 4s8-1.79 8-4V7M4 7c0 2.21 3.582 4 8 4s8-1.79 8-4M4 7c0-2.21 3.582-4 8-4s8 1.79 8 4m0 5c0 2.21-3.582 4-8 4s-8-1.79-8-4" /></svg>
          </div>
          Hardware Capital Ledger
        </h2>
        <button onClick={() => setShowForm(!showForm)} className="bg-indigo-600 hover:bg-indigo-700 text-white px-5 py-2.5 rounded-lg font-bold shadow-md shadow-indigo-200 transition-all">
          Register New Asset
        </button>
      </div>

      {showForm && (
        <div className="bg-indigo-50/30 p-6 rounded-2xl border border-indigo-100/50">
          <form onSubmit={handleCreate} className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div>
              <label className="text-xs font-bold text-gray-500 uppercase">Asset Tag *</label>
              <input required className="w-full mt-1 border border-gray-200 rounded-lg p-2.5 outline-none focus:border-indigo-400 focus:ring-2 focus:ring-indigo-100" value={newAsset.AssetTag} onChange={e => setNewAsset({...newAsset, AssetTag: e.target.value})} placeholder="e.g. IT-LPT-001" />
            </div>
            <div>
              <label className="text-xs font-bold text-gray-500 uppercase">Hardware Model *</label>
              <select required className="w-full mt-1 border border-gray-200 rounded-lg p-2.5 outline-none focus:border-indigo-400 bg-white focus:ring-2 focus:ring-indigo-100" value={newAsset.ModelId} onChange={e => setNewAsset({...newAsset, ModelId: e.target.value})}>
                <option value="">Select Bounded Model...</option>
                {models.map(m => <option key={m.id} value={m.id}>{m.manufacturer} {m.name} ({m.categoryName})</option>)}
              </select>
            </div>
            <div>
              <label className="text-xs font-bold text-gray-500 uppercase">Serial Number (Optional)</label>
              <input className="w-full mt-1 border border-gray-200 rounded-lg p-2.5 outline-none focus:border-indigo-400 focus:ring-2 focus:ring-indigo-100" value={newAsset.SerialNumber} onChange={e => setNewAsset({...newAsset, SerialNumber: e.target.value})} placeholder="Serial or Service Tag" />
            </div>
            <div>
              <label className="text-xs font-bold text-gray-500 uppercase">Physical Location</label>
              <input className="w-full mt-1 border border-gray-200 rounded-lg p-2.5 outline-none focus:border-indigo-400 focus:ring-2 focus:ring-indigo-100" value={newAsset.PhysicalLocation} onChange={e => setNewAsset({...newAsset, PhysicalLocation: e.target.value})} placeholder="e.g. Server Room B" />
            </div>

            <div className="md:col-span-2 lg:col-span-4 flex justify-end gap-3 mt-2">
              <button type="button" onClick={() => setShowForm(false)} className="px-5 py-2.5 font-semibold text-gray-500 hover:bg-gray-100 rounded-xl transition-colors">Cancel</button>
              <button type="submit" className="px-6 py-2.5 bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-xl shadow-md transition-all">Submit Registration</button>
            </div>
          </form>
        </div>
      )}

      {loading ? (
        <div className="text-center py-20 text-gray-400">Loading ledger...</div>
      ) : (
        <div className="bg-white rounded-2xl border border-gray-100 shadow-sm overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-left text-sm">
              <thead className="bg-gray-50 text-xs uppercase text-gray-500 font-bold tracking-wider">
                <tr>
                  <th className="px-6 py-4">Asset Identification</th>
                  <th className="px-6 py-4">Model & Make</th>
                  <th className="px-6 py-4">Current Status</th>
                  <th className="px-6 py-4">Assignment / Location</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {assets.map(a => (
                  <tr key={a.id} className="hover:bg-gray-50/50 transition-colors group">
                    <td className="px-6 py-4">
                      <div className="font-bold text-gray-900">{a.assetTag}</div>
                      <div className="text-xs text-gray-400 font-mono mt-0.5">{a.serialNumber || 'No Serial'}</div>
                    </td>
                    <td className="px-6 py-4">
                      <div className="font-medium text-gray-800">{a.manufacturer} {a.modelName}</div>
                    </td>
                    <td className="px-6 py-4">
                      <span className={`inline-flex items-center px-2.5 py-1 rounded-full text-xs font-bold border
                        ${a.status === 'Available' ? 'bg-emerald-50 text-emerald-700 border-emerald-200' : 
                          a.status === 'Assigned' || a.status === 'Deployed' ? 'bg-blue-50 text-blue-700 border-blue-200' : 
                          'bg-gray-50 text-gray-700 border-gray-200'}`}>
                        {a.status}
                      </span>
                    </td>
                    <td className="px-6 py-4">
                      {a.assignedToName ? (
                        <div className="flex items-center gap-2">
                          <div className="w-6 h-6 rounded-full bg-blue-100 text-blue-600 flex items-center justify-center font-bold text-[10px]">
                            {a.assignedToName.charAt(0)}
                          </div>
                          <span className="font-medium text-gray-700">{a.assignedToName}</span>
                        </div>
                      ) : (
                        <span className="text-gray-400 italic text-xs">{a.physicalLocation || 'Unassigned / Vault'}</span>
                      )}
                    </td>
                  </tr>
                ))}
                {assets.length === 0 && (
                  <tr>
                    <td colSpan={4} className="px-6 py-12 text-center text-gray-400">
                      No assets found in organizational ledger.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
}
