"use client";
import { useState, useEffect } from "react";
import { getAssetsListAction } from "../actions/assets";

export default function MyAssetsView() {
  const [assets, setAssets] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function loadAssets() {
      try {
        const data = await getAssetsListAction();
        setAssets(data); 
      } catch (e) {
        console.error(e);
      } finally {
        setLoading(false);
      }
    }
    loadAssets();
  }, []);

  if (loading) {
    return <div className="animate-pulse flex space-x-4"><div className="h-4 bg-gray-200 rounded w-3/4"></div></div>;
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500">
      
      <div className="flex justify-between items-center bg-blue-50/50 p-6 rounded-2xl border border-blue-100">
        <div>
          <h2 className="text-xl font-bold text-foreground">Your Assigned Hardware</h2>
          <p className="text-sm text-foreground/60 mt-1">Review the capital assets currently in your physical custody.</p>
        </div>
        <div className="bg-card p-3 rounded-xl shadow-sm border border-border/50 font-mono text-sm text-blue-700">
          Custody Count: {assets.length}
        </div>
      </div>

      {assets.length === 0 ? (
        <div className="text-center py-12 text-foreground/40">
          <svg className="mx-auto mb-4 opacity-50 w-12 h-12" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
          <p>You have no hardware currently assigned to you.</p>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {assets.map((asset) => (
            <div key={asset.id} className="group relative bg-card border border-border/50 rounded-3xl p-6 shadow-sm hover:shadow-xl hover:-translate-y-1 transition-all duration-300">
              <div className="absolute top-4 right-4 text-green-500 bg-green-50 p-2 rounded-full">
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
              </div>
              <div className="bg-background/50 h-12 w-12 rounded-2xl flex items-center justify-center mb-4 text-foreground/90 border border-border/50 group-hover:bg-blue-600 group-hover:text-white transition-colors">
                <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 12h14M5 12a2 2 0 01-2-2V6a2 2 0 012-2h14a2 2 0 012 2v4a2 2 0 01-2 2M5 12a2 2 0 00-2 2v4a2 2 0 002 2h14a2 2 0 002-2v-4a2 2 0 00-2-2" /></svg>
              </div>
              <h3 className="font-bold text-foreground text-lg">{asset.modelName || "Generic Asset"}</h3>
              <p className="text-sm text-foreground/60 mb-4">{asset.manufacturer}</p>
              
              <div className="space-y-2 mt-4 pt-4 border-t border-gray-50 font-mono text-xs text-foreground/80">
                <div className="flex justify-between">
                  <span>Tag ID:</span>
                  <span className="font-semibold">{asset.assetTag}</span>
                </div>
                <div className="flex justify-between">
                  <span>Serial:</span>
                  <span className="font-semibold">{asset.serialNumber || "N/A"}</span>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
