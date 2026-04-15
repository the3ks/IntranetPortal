"use client";
import { useState, useEffect } from "react";
import { getCategoriesAction, getModelsAction, getApproverGroupsAction, createCategoryAction, createModelAction, updateCategoryAction, toggleCategoryAction, updateModelAction, toggleModelAction } from "@/app/actions/assets";
import ApproverGroupsManager from "./ApproverGroupsManager";

export default function DictionariesPage() {
  const [categories, setCategories] = useState<any[]>([]);
  const [models, setModels] = useState<any[]>([]);
  const [groups, setGroups] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  // Forms State
  const [showCatForm, setShowCatForm] = useState(false);
  const [newCatName, setNewCatName] = useState("");
  const [newCatDesc, setNewCatDesc] = useState("");
  const [newCatParentId, setNewCatParentId] = useState("");
  const [newCatReqApp, setNewCatReqApp] = useState(true);
  const [newCatAllowManual, setNewCatAllowManual] = useState(false);
  const [newCatGroupId, setNewCatGroupId] = useState("");
  const [newCatFulfillmentGroupId, setNewCatFulfillmentGroupId] = useState("");

  const [showModelForm, setShowModelForm] = useState(false);
  const [newModelName, setNewModelName] = useState("");
  const [newModelManuf, setNewModelManuf] = useState("");
  const [newModelCatId, setNewModelCatId] = useState("");

  const [editingCategoryId, setEditingCategoryId] = useState<number | null>(null);
  const [editingModel, setEditingModel] = useState<number | null>(null);

  const fetchData = async () => {
    setLoading(true);
    const [cats, mods, grps] = await Promise.all([getCategoriesAction(), getModelsAction(), getApproverGroupsAction()]);
    setCategories(cats);
    setModels(mods);
    setGroups(grps);
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSaveCategory = async (e: React.FormEvent) => {
    e.preventDefault();
    const payload = {
      Name: newCatName,
      Description: newCatDesc,
      RequiresApproval: newCatReqApp,
      AllowRequesterToSelectApprover: newCatAllowManual,
      DefaultApproverGroupId: newCatGroupId ? parseInt(newCatGroupId) : null,
      FulfillmentGroupId: newCatFulfillmentGroupId ? parseInt(newCatFulfillmentGroupId) : null,
      ParentCategoryId: newCatParentId ? parseInt(newCatParentId) : null
    };

    if (editingCategoryId) {
      await updateCategoryAction(editingCategoryId, payload);
    } else {
      await createCategoryAction(payload);
    }

    setShowCatForm(false);
    resetCatForm();
    fetchData();
  };

  const resetCatForm = () => {
    setNewCatName(""); setNewCatDesc(""); setNewCatParentId(""); setNewCatReqApp(true);
    setNewCatAllowManual(false); setNewCatGroupId(""); setNewCatFulfillmentGroupId("");
    setEditingCategoryId(null);
  };

  const handleEditCategoryBtn = (c: any) => {
    setNewCatName(c.name);
    setNewCatDesc(c.description || "");
    setNewCatParentId(c.parentCategoryId?.toString() || "");
    setNewCatReqApp(c.requiresApproval);
    setNewCatAllowManual(c.allowRequesterToSelectApprover);
    setNewCatGroupId(c.defaultApproverGroupId?.toString() || "");
    setNewCatFulfillmentGroupId(c.fulfillmentGroupId?.toString() || "");
    setEditingCategoryId(c.id);
    setShowCatForm(true);
  };

  const handleCreateModel = async (e: React.FormEvent) => {
    e.preventDefault();
    await createModelAction({ Name: newModelName, Manufacturer: newModelManuf, CategoryId: parseInt(newModelCatId) });
    setShowModelForm(false);
    setNewModelName(""); setNewModelManuf(""); setNewModelCatId("");
    fetchData();
  };

  if (loading) return <div className="text-center py-10 opacity-50">Loading dictionaries...</div>;

  return (
    <div className="flex flex-col gap-8">
      {/* Categories Panel */}
      <div className="flex-1 bg-card border border-border/50 rounded-2xl shadow-sm overflow-hidden flex flex-col">
        <div className="p-5 border-b border-border/50 flex justify-between items-center bg-background/50/50">
          <h2 className="text-lg font-bold text-foreground flex items-center gap-2">
            <svg className="w-5 h-5 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6h16M4 10h16M4 14h16M4 18h16" /></svg>
            Categories
          </h2>
          <button onClick={() => { resetCatForm(); setShowCatForm(!showCatForm); }} className="text-sm bg-blue-50 text-blue-600 hover:bg-blue-100 px-3 py-1.5 rounded-lg font-semibold transition-colors">
            {showCatForm && !editingCategoryId ? 'Cancel' : '+ New Category'}
          </button>
        </div>

        {showCatForm && (
          <form onSubmit={handleSaveCategory} className="p-5 border-b border-blue-50 bg-blue-50/20 grid gap-4">
            <h3 className="font-bold text-blue-700">{editingCategoryId ? "Edit Category" : "Create New Category"}</h3>
            <div><label className="text-xs font-bold text-foreground/60 uppercase">Category Name</label><input required className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-blue-400 bg-white" value={newCatName} onChange={e => setNewCatName(e.target.value)} /></div>
            <div>
              <label className="text-xs font-bold text-foreground/60 uppercase">Parent Category (Optional)</label>
              <select className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-blue-400 bg-card" value={newCatParentId} onChange={e => setNewCatParentId(e.target.value)}>
                <option value="">None (Top-Level)</option>
                {categories.filter(c => c.isActive && c.id !== editingCategoryId).map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>
            <div><label className="text-xs font-bold text-foreground/60 uppercase">Description</label><input className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-blue-400 bg-white" value={newCatDesc} onChange={e => setNewCatDesc(e.target.value)} /></div>

            {!newCatParentId && (
              <div className="flex flex-col gap-2 p-3 bg-white rounded-lg border border-gray-200">
                <label className="flex items-center gap-2 text-sm font-medium text-foreground/90 cursor-pointer">
                  <input type="checkbox" checked={newCatReqApp} onChange={e => setNewCatReqApp(e.target.checked)} className="w-4 h-4 text-blue-600 rounded" />
                  Requires Manager Approval to Request
                </label>

                {newCatReqApp && (
                  <>
                    <label className="flex items-center gap-2 text-sm font-medium text-foreground/90 cursor-pointer ml-6">
                      <input type="checkbox" checked={newCatAllowManual} onChange={e => setNewCatAllowManual(e.target.checked)} className="w-4 h-4 text-blue-600 rounded" />
                      Allow Requester to Override and Select Specific Approver
                    </label>

                    {!newCatAllowManual && (
                      <div className="ml-6 mt-1">
                        <label className="text-xs font-bold text-foreground/60 uppercase">Default Approver Group</label>
                        <select className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-blue-400 bg-card" value={newCatGroupId} onChange={e => setNewCatGroupId(e.target.value)}>
                          <option value="">-- No Default Group selected --</option>
                          {groups.map(g => <option key={g.id} value={g.id}>{g.name}</option>)}
                        </select>
                      </div>
                    )}
                  </>
                )}

                <div className="mt-3 pt-3 border-t border-gray-100">
                  <label className="text-xs font-bold text-emerald-600 uppercase">Admin Management Group (Fulfillment)</label>
                  <select className="w-full mt-1 border border-emerald-500/50 rounded-lg p-2 text-sm outline-none focus:border-emerald-400 bg-emerald-50/10 text-emerald-900" value={newCatFulfillmentGroupId} onChange={e => setNewCatFulfillmentGroupId(e.target.value)}>
                    <option value="">-- Global Admins Only --</option>
                    {groups.map(g => <option key={g.id} value={g.id}>{g.name}</option>)}
                  </select>
                </div>
              </div>
            )}

            <div className="flex justify-end gap-2 mt-2">
              <button type="button" onClick={() => { setShowCatForm(false); resetCatForm(); }} className="px-4 py-2 text-sm text-foreground/60 hover:bg-white rounded-lg transition-colors">Cancel</button>
              <button type="submit" className="px-4 py-2 text-sm bg-blue-600 hover:bg-blue-700 text-white font-bold rounded-lg shadow-sm">{editingCategoryId ? "Update Category" : "Save Category"}</button>
            </div>
          </form>
        )}

        <div className="flex-1 overflow-auto max-h-[600px] p-2">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-foreground/40 uppercase bg-background/50">
              <tr>
                <th className="px-4 py-3">Name</th>
                <th className="px-4 py-3">Approval Routing</th>
                <th className="px-4 py-3">Fulfillment Group</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {categories.map(c => (
                <tr key={c.id} className={`border-b border-gray-50 hover:bg-background/50/50 ${!c.isActive ? 'opacity-50 grayscale' : ''}`}>
                  <td className="px-4 py-3 font-medium text-foreground">
                    {c.parentCategoryId && <span className="text-foreground/40 text-xs mr-1">{categories.find(p => p.id === c.parentCategoryId)?.name} /</span>}
                    {c.name}
                    {c.description && <span className="block text-xs text-foreground/40 font-normal">{c.description}</span>}
                  </td>
                  <td className="px-4 py-3">
                    {c.requiresApproval ? (
                      <div className="flex flex-col gap-1">
                        <span className="px-2 py-1 bg-amber-100 text-amber-700 rounded-full text-xs font-bold w-fit">Manager</span>
                        {c.allowRequesterToSelectApprover && <span className="text-[10px] text-gray-500 font-semibold uppercase">User Selected</span>}
                        {c.defaultApproverGroupId && !c.allowRequesterToSelectApprover && (
                          <span className="text-[10px] text-indigo-500 font-semibold">Group: {groups.find(g => g.id === c.defaultApproverGroupId)?.name}</span>
                        )}
                      </div>
                    ) : (
                      <span className="px-2 py-1 bg-emerald-100 text-emerald-700 rounded-full text-xs font-bold">Auto</span>
                    )}
                  </td>
                  <td className="px-4 py-3">
                    {c.fulfillmentGroupId ? (
                      <span className="text-[10px] bg-emerald-100 text-emerald-700 px-2 py-1 rounded-md font-bold block w-fit">
                        {groups.find(g => g.id === c.fulfillmentGroupId)?.name || 'Unknown'}
                      </span>
                    ) : (
                      <span className="text-[10px] bg-gray-100 text-gray-500 px-2 py-1 rounded-md font-bold block w-fit">Global Admins</span>
                    )}
                  </td>
                  <td className="px-4 py-3">
                    {c.isActive ? <span className="text-emerald-600 font-bold text-xs">Active</span> : <span className="text-foreground/40 font-bold text-xs">Disabled</span>}
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex justify-end gap-2 text-foreground/40">
                      <button onClick={() => handleEditCategoryBtn(c)} className="hover:text-blue-600" title="Edit configuration"><svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" /></svg></button>
                      <button onClick={async () => { await toggleCategoryAction(c.id); fetchData(); }} className={c.isActive ? "hover:text-amber-500" : "hover:text-emerald-500"} title={c.isActive ? "Disable" : "Enable"}>
                        {c.isActive ? (
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636" /></svg>
                        ) : (
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                        )}
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {categories.length === 0 && <tr><td colSpan={2} className="px-4 py-8 text-center text-foreground/40">No categories found.</td></tr>}
            </tbody>
          </table>
        </div>
      </div>

      {/* Models Panel */}
      <div className="flex-1 bg-card border border-border/50 rounded-2xl shadow-sm overflow-hidden flex flex-col">
        <div className="p-5 border-b border-border/50 flex justify-between items-center bg-background/50/50">
          <h2 className="text-lg font-bold text-foreground flex items-center gap-2">
            <svg className="w-5 h-5 text-indigo-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 002-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
            Bounded Models
          </h2>
          <button onClick={() => setShowModelForm(!showModelForm)} className="text-sm bg-indigo-50 text-indigo-600 hover:bg-indigo-100 px-3 py-1.5 rounded-lg font-semibold transition-colors">
            + New Model
          </button>
        </div>

        {showModelForm && (
          <form onSubmit={handleCreateModel} className="p-5 border-b border-indigo-50 bg-indigo-50/20 grid gap-4">
            <div><label className="text-xs font-bold text-foreground/60 uppercase">Parent Category</label>
              <select required className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-indigo-400 bg-card" value={newModelCatId} onChange={e => setNewModelCatId(e.target.value)}>
                <option value="">Select Category...</option>
                {categories.filter(c => c.isActive).map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>
            <div><label className="text-xs font-bold text-foreground/60 uppercase">Manufacturer</label><input required className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-indigo-400" value={newModelManuf} onChange={e => setNewModelManuf(e.target.value)} placeholder="e.g. Dell, Apple" /></div>
            <div><label className="text-xs font-bold text-foreground/60 uppercase">Model Name</label><input required className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-indigo-400" value={newModelName} onChange={e => setNewModelName(e.target.value)} placeholder="e.g. XPS 15 9500" /></div>

            <div className="flex justify-end gap-2 mt-2">
              <button type="button" onClick={() => setShowModelForm(false)} className="px-4 py-2 text-sm text-foreground/60 hover:bg-background rounded-lg">Cancel</button>
              <button type="submit" className="px-4 py-2 text-sm bg-indigo-600 hover:bg-indigo-700 text-white font-bold rounded-lg shadow-sm">Save Model</button>
            </div>
          </form>
        )}

        <div className="flex-1 overflow-auto max-h-[600px] p-2">
          <table className="w-full text-sm text-left">
            <thead className="text-xs text-foreground/40 uppercase bg-background/50">
              <tr>
                <th className="px-4 py-3">Category</th>
                <th className="px-4 py-3">Manufacturer</th>
                <th className="px-4 py-3">Model</th>
                <th className="px-4 py-3">Status</th>
                <th className="px-4 py-3 text-right">Actions</th>
              </tr>
            </thead>
            <tbody>
              {models.map(m => (
                <tr key={m.id} className={`border-b border-gray-50 hover:bg-background/50/50 ${!m.isActive ? 'opacity-50 grayscale' : ''}`}>
                  <td className="px-4 py-3 font-medium text-foreground/60"><span className="px-2 py-1 bg-background rounded-md text-xs">{m.categoryName}</span></td>
                  <td className="px-4 py-3 text-foreground">{m.manufacturer}</td>
                  <td className="px-4 py-3 font-bold text-foreground">
                    {editingModel === m.id ? (
                      <input type="text" className="border border-border/50 p-1 text-sm rounded w-full" defaultValue={m.name}
                        onBlur={async (e) => {
                          const val = e.target.value;
                          if (val && val !== m.name) {
                            await updateModelAction(m.id, { ...m, Name: val });
                            fetchData();
                          }
                          setEditingModel(null);
                        }}
                        onKeyDown={(e) => { if (e.key === 'Enter') e.currentTarget.blur(); }}
                        autoFocus
                      />
                    ) : m.name}
                  </td>
                  <td className="px-4 py-3">
                    {m.isActive ? <span className="text-indigo-600 font-bold text-xs">Active</span> : <span className="text-foreground/40 font-bold text-xs">Disabled</span>}
                  </td>
                  <td className="px-4 py-3 text-right">
                    <div className="flex justify-end gap-2 text-foreground/40">
                      <button onClick={() => setEditingModel(m.id)} className="hover:text-indigo-600" title="Edit model inline"><svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" /></svg></button>
                      <button onClick={async () => { await toggleModelAction(m.id); fetchData(); }} className={m.isActive ? "hover:text-amber-500" : "hover:text-emerald-500"} title={m.isActive ? "Disable" : "Enable"}>
                        {m.isActive ? (
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M18.364 18.364A9 9 0 005.636 5.636m12.728 12.728A9 9 0 015.636 5.636m12.728 12.728L5.636 5.636" /></svg>
                        ) : (
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                        )}
                      </button>
                    </div>
                  </td>
                </tr>
              ))}
              {models.length === 0 && <tr><td colSpan={3} className="px-4 py-8 text-center text-foreground/40">No models bounded.</td></tr>}
            </tbody>
          </table>
        </div>
      </div>
      <ApproverGroupsManager groups={groups} onUpdate={fetchData} />
    </div>
  );
}
