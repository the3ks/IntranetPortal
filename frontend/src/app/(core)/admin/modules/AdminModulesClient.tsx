"use client";
import { useState } from "react";

export default function AdminModulesClient({ initialModules, sites }: { initialModules: any[], sites: any[] }) {
  const [modules, setModules] = useState(initialModules);
  const [loadingId, setLoadingId] = useState<number | null>(null);
  
  // Modal state
  const [showModal, setShowModal] = useState(false);
  const [editMod, setEditMod] = useState<any>(null); // null = create new

  const [formData, setFormData] = useState({ name: "", description: "", url: "", iconSvg: "", isActiveGlobally: true, order: 0 });

  const openAdd = () => {
    setEditMod(null);
    setFormData({ name: "", description: "", url: "", iconSvg: "", isActiveGlobally: true, order: 0 });
    setShowModal(true);
  };

  const openEdit = (mod: any) => {
    setEditMod(mod);
    setFormData({ name: mod.name, description: mod.description, url: mod.url, iconSvg: mod.iconSvg, isActiveGlobally: mod.isActiveGlobally, order: mod.order || 0 });
    setShowModal(true);
  };

  const saveModule = async () => {
    const isNew = !editMod;
    const url = isNew ? `/api/admin/modules` : `/api/admin/modules/${editMod.id}`;
    const method = isNew ? "POST" : "PUT";
    const bodyObj = isNew ? { ...formData, isActive: true, allowedSites: sites } : { ...editMod, ...formData };
    
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    const res = await fetch(`${backendUrl}${url}`, {
      method,
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(bodyObj)
    });

    if (res.ok) {
      const saved = await res.json();
      if (isNew) {
        setModules([...modules, saved].sort((a: any, b: any) => (a.order || 0) - (b.order || 0)));
      } else {
        setModules(modules.map((m: any) => m.id === saved.id ? saved : m).sort((a: any, b: any) => (a.order || 0) - (b.order || 0)));
      }
      setShowModal(false);
    }
  };

  const deleteModule = async (id: number) => {
    if (!confirm("Are you sure you want to delete this module?")) return;
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    const res = await fetch(`${backendUrl}/api/admin/modules/${id}`, {
      method: "DELETE",
      credentials: "include"
    });
    if (res.ok) {
      setModules(modules.filter((m: any) => m.id !== id));
    }
  };

  const toggleStatus = async (mod: any) => {
    setLoadingId(mod.id);
    const updated = { ...mod, isActive: !mod.isActive };
    
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    const res = await fetch(`${backendUrl}/api/admin/modules/${mod.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(updated)
    });
    
    if (res.ok) {
      const updatedMod = await res.json();
      setModules(modules.map((m: any) => m.id === mod.id ? updatedMod : m));
    }
    setLoadingId(null);
  };

  const toggleSite = async (mod: any, targetSite: any) => {
    setLoadingId(mod.id);
    
    const alreadyAllowed = mod.allowedSites?.some((s:any) => s.id === targetSite.id);
    let newSites = mod.allowedSites ? [...mod.allowedSites] : [];
    
    if (alreadyAllowed) {
       newSites = newSites.filter((s:any) => s.id !== targetSite.id);
    } else {
       newSites.push(targetSite);
    }

    const updated = { ...mod, allowedSites: newSites };
    
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    const res = await fetch(`${backendUrl}/api/admin/modules/${mod.id}`, {
      method: "PUT",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify(updated)
    });
    
    if (res.ok) {
      const updatedMod = await res.json();
      setModules(modules.map((m: any) => m.id === mod.id ? updatedMod : m));
    }
    setLoadingId(null);
  };

  return (
    <>
      <div className="flex justify-end mb-6">
        <button onClick={openAdd} className="bg-blue-600 hover:bg-blue-500 text-white px-4 py-2 rounded-xl font-bold transition-colors">
          + Add New Module
        </button>
      </div>

      <div className="grid grid-cols-1 gap-6">
        {modules.map((mod: any) => {
          const isCore = mod.name === "The Hub" || mod.name === "Administration";
          
          return (
            <div key={mod.id} className={`p-6 rounded-2xl border ${mod.isActive ? 'border-border/50 bg-card' : 'border-red-500/30 bg-red-500/5'} shadow-sm flex flex-col md:flex-row gap-6 relative`}>
              <div className="flex-shrink-0 flex items-center justify-center w-16 h-16 rounded-xl bg-background/50 border border-border/50">
                <div dangerouslySetInnerHTML={{ __html: mod.iconSvg }} className={`[&>svg]:w-6 [&>svg]:h-6 ${mod.isActive ? 'text-blue-500' : 'text-red-500'}`} />
              </div>
              
              <div className="flex-1">
                <div className="flex justify-between items-start mb-2">
                  <div>
                    <h3 className="text-xl font-bold text-foreground inline-flex items-center gap-2">
                       {mod.name} <span className="text-sm font-normal text-foreground/40 ml-2">(Order: {mod.order})</span>
                       {isCore && <span className="bg-fuchsia-500/20 text-fuchsia-500 text-[10px] uppercase font-bold tracking-widest px-2 py-0.5 rounded-full ml-2">Core</span>}
                    </h3>
                    <div className="text-sm font-medium text-foreground/50">{mod.url}</div>
                  </div>
                  <div className="flex gap-2">
                    <button onClick={() => openEdit(mod)} className="p-2 text-foreground/50 hover:text-blue-500 transition-colors">
                      <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                    </button>
                    {!isCore && (
                      <button onClick={() => deleteModule(mod.id)} className="p-2 text-foreground/50 hover:text-red-500 transition-colors">
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    )}
                  </div>
                </div>
                <p className="text-foreground/70 mb-6">{mod.description}</p>
                
                <div className="bg-background/40 p-4 rounded-xl border border-border/30">
                  <div className="flex items-center justify-between mb-4">
                    <div>
                      <h4 className="font-bold text-foreground">Module Status</h4>
                      <p className="text-sm text-foreground/60">{isCore ? "Core Modules cannot be disabled." : "Toggle master kill-switch to entirely enable or disable this module."}</p>
                    </div>
                    <button 
                      onClick={() => toggleStatus(mod)}
                      disabled={loadingId === mod.id || isCore}
                      className={`relative inline-flex h-6 w-11 items-center rounded-full transition-colors ${mod.isActive ? 'bg-emerald-500' : 'bg-gray-400'} ${isCore ? 'opacity-50 cursor-not-allowed' : ''}`}
                    >
                      <span className={`inline-block h-4 w-4 transform rounded-full bg-white transition-transform ${mod.isActive ? 'translate-x-6' : 'translate-x-1'}`} />
                    </button>
                  </div>

                  {!isCore && (
                    <div className="mt-4 pt-4 border-t border-border/30">
                       <h4 className="font-bold text-foreground mb-3 text-sm">Site Authorizations</h4>
                       <div className="flex flex-wrap gap-2">
                         {sites.map(site => {
                            const isAllowed = mod.allowedSites?.some((s:any) => s.id === site.id);
                            return (
                              <button
                                key={site.id}
                                onClick={() => toggleSite(mod, site)}
                                disabled={loadingId === mod.id}
                                className={`px-3 py-1.5 rounded-lg text-sm font-medium border transition-colors ${isAllowed ? 'bg-blue-500/10 text-blue-500 border-blue-500/30' : 'bg-background hover:bg-background/80 text-foreground/60 border-border/50'}`}
                              >
                                {site.name}
                              </button>
                            );
                         })}
                       </div>
                    </div>
                  )}
                </div>
              </div>
            </div>
          );
        })}
      </div>

      {showModal && (
        <div className="fixed inset-0 bg-black/60 backdrop-blur-sm z-50 flex items-center justify-center p-4">
          <div className="bg-card w-full max-w-lg rounded-[2rem] border border-border/50 shadow-2xl p-8 relative">
             <button onClick={() => setShowModal(false)} className="absolute top-6 right-6 text-foreground/50 hover:text-foreground">
               <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
             </button>
             <h2 className="text-2xl font-bold mb-6">{editMod ? "Edit Module" : "Create New Module"}</h2>
             
              {(() => {
                 const isEditingCore = editMod && (editMod.name === "The Hub" || editMod.name === "Administration");
                 return (
                   <div className="space-y-4">
                      <div>
                        <label className="block text-sm font-bold text-foreground/70 mb-1">Module Name</label>
                        <input type="text" disabled={isEditingCore} value={formData.name} onChange={e => setFormData({...formData, name: e.target.value})} className={`w-full bg-background border border-border/50 p-3 rounded-xl focus:outline-none focus:border-blue-500 ${isEditingCore ? "opacity-50 cursor-not-allowed" : ""}`} />
                      </div>
                <div>
                  <label className="block text-sm font-bold text-foreground/70 mb-1">Display Order</label>
                  <input type="number" value={formData.order} onChange={e => setFormData({...formData, order: parseInt(e.target.value) || 0})} className="w-full bg-background border border-border/50 p-3 rounded-xl focus:outline-none focus:border-blue-500" />
                </div>
                <div>
                  <label className="block text-sm font-bold text-foreground/70 mb-1">Module Description</label>
                  <input type="text" value={formData.description} onChange={e => setFormData({...formData, description: e.target.value})} className="w-full bg-background border border-border/50 p-3 rounded-xl focus:outline-none focus:border-blue-500" />
                </div>
                      <div>
                        <label className="block text-sm font-bold text-foreground/70 mb-1">Target URL</label>
                        <input type="text" disabled={isEditingCore} value={formData.url} onChange={e => setFormData({...formData, url: e.target.value})} placeholder="e.g. /drinks or http://localhost:3001" className={`w-full bg-background border border-border/50 p-3 rounded-xl focus:outline-none focus:border-blue-500 ${isEditingCore ? "opacity-50 cursor-not-allowed" : ""}`} />
                      </div>
                      <div>
                        <label className="block text-sm font-bold text-foreground/70 mb-1">SVG Icon (Raw string)</label>
                        <textarea value={formData.iconSvg} onChange={e => setFormData({...formData, iconSvg: e.target.value})} className="w-full bg-background border border-border/50 p-3 rounded-xl focus:outline-none focus:border-blue-500 h-24 font-mono text-xs"></textarea>
                      </div>
                   </div>
                 );
              })()}
             
             <div className="mt-8 flex justify-end gap-3">
                <button onClick={() => setShowModal(false)} className="px-5 py-2.5 rounded-xl text-foreground/60 font-bold hover:bg-background transition-colors">Cancel</button>
                <button onClick={saveModule} className="bg-blue-600 hover:bg-blue-500 text-white px-6 py-2.5 rounded-xl font-bold transition-colors">Save Module</button>
             </div>
          </div>
        </div>
      )}
    </>
  );
}
