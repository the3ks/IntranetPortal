"use client";
import { useState, useEffect } from "react";
import { getAccessoriesAction, getCategoriesAction, createAccessoryAction, addAccessoryStockAction } from "@/app/actions/assets";

export default function AccessoriesPage() {
  const [accessories, setAccessories] = useState<any[]>([]);
  const [categories, setCategories] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  // Form State
  const [showForm, setShowForm] = useState(false);
  const [newAcc, setNewAcc] = useState({
    Name: "",
    CategoryId: "",
    TotalQuantity: 10,
    MinStockThreshold: 5
  });

  const fetchData = async () => {
    setLoading(true);
    const [accData, catData] = await Promise.all([getAccessoriesAction(), getCategoriesAction()]);
    setAccessories(accData);
    setCategories(catData); // Any category can have bulk items, regardless of approval constraints
    setLoading(false);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleCreate = async (e: React.FormEvent) => {
    e.preventDefault();
    await createAccessoryAction({
      Name: newAcc.Name,
      CategoryId: parseInt(newAcc.CategoryId),
      TotalQuantity: newAcc.TotalQuantity,
      MinStockThreshold: newAcc.MinStockThreshold,
    });
    setShowForm(false);
    setNewAcc({ Name: "", CategoryId: "", TotalQuantity: 10, MinStockThreshold: 5 });
    fetchData();
  };

  const handleAddStock = async (id: number) => {
    const qty = prompt("How many bulk units to add?");
    if (qty && !isNaN(parseInt(qty))) {
      await addAccessoryStockAction(id, parseInt(qty));
      fetchData();
    }
  };

  return (
    <div className="flex flex-col gap-6">
      <div className="flex justify-between items-center bg-card p-4 rounded-xl border border-border/50 shadow-sm">
        <h2 className="text-xl font-bold text-foreground flex items-center gap-3">
          <div className="p-2 bg-fuchsia-50 text-fuchsia-500 rounded-lg">
            <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 002-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
          </div>
          Bulk Inventory Stockpile
        </h2>
        <button onClick={() => setShowForm(!showForm)} className="bg-fuchsia-600 hover:bg-fuchsia-700 text-white px-5 py-2.5 rounded-lg font-bold shadow-md shadow-fuchsia-200 transition-all">
          Register Bulk Resource
        </button>
      </div>

      {showForm && (
        <div className="bg-fuchsia-50/30 p-6 rounded-2xl border border-fuchsia-100/50">
          <form onSubmit={handleCreate} className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4">
            <div>
              <label className="text-xs font-bold text-foreground/60 uppercase">Item Description *</label>
              <input required className="w-full mt-1 border border-border/50 rounded-lg p-2.5 outline-none focus:border-fuchsia-400 focus:ring-2 focus:ring-fuchsia-100" value={newAcc.Name} onChange={e => setNewAcc({ ...newAcc, Name: e.target.value })} placeholder="e.g. Ergonomic Mouse or Box of Folders" />
            </div>
            <div>
              <label className="text-xs font-bold text-foreground/60 uppercase">Flexible Category *</label>
              <select required className="w-full mt-1 border border-border/50 rounded-lg p-2.5 outline-none focus:border-fuchsia-400 bg-card focus:ring-2 focus:ring-fuchsia-100" value={newAcc.CategoryId} onChange={e => setNewAcc({ ...newAcc, CategoryId: e.target.value })}>
                <option value="">Select Category...</option>
                {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
              </select>
            </div>
            <div>
              <label className="text-xs font-bold text-foreground/60 uppercase">Initial Stock Qty *</label>
              <input type="number" required min="0" className="w-full mt-1 border border-border/50 rounded-lg p-2.5 outline-none focus:border-fuchsia-400 focus:ring-2 focus:ring-fuchsia-100" value={newAcc.TotalQuantity} onChange={e => setNewAcc({ ...newAcc, TotalQuantity: parseInt(e.target.value) })} />
            </div>
            <div>
              <label className="text-xs font-bold text-foreground/60 uppercase">Low Warn Threshold</label>
              <input type="number" required min="0" className="w-full mt-1 border border-border/50 rounded-lg p-2.5 outline-none focus:border-fuchsia-400 focus:ring-2 focus:ring-fuchsia-100" value={newAcc.MinStockThreshold} onChange={e => setNewAcc({ ...newAcc, MinStockThreshold: parseInt(e.target.value) })} />
            </div>

            <div className="md:col-span-2 lg:col-span-4 flex justify-end gap-3 mt-2">
              <button type="button" onClick={() => setShowForm(false)} className="px-5 py-2.5 font-semibold text-foreground/60 hover:bg-background rounded-xl transition-colors">Cancel</button>
              <button type="submit" className="px-6 py-2.5 bg-fuchsia-600 hover:bg-fuchsia-700 text-white font-bold rounded-xl shadow-md transition-all">Submit Registration</button>
            </div>
          </form>
        </div>
      )}

      {loading ? (
        <div className="text-center py-20 text-foreground/40">Loading stockpile...</div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {accessories.map(a => (
            <div key={a.id} className="bg-card border border-border/50 rounded-2xl shadow-sm hover:shadow-md transition-shadow p-6 flex flex-col relative overflow-hidden">
              {a.availableQuantity <= (a.minStockThreshold || 0) && (
                <div className="absolute top-0 right-0 bg-red-500 text-white text-[10px] font-bold px-3 py-1 rounded-bl-lg uppercase tracking-wider shadow-sm">
                  Low Stock
                </div>
              )}

              <div className="flex items-start justify-between mb-4">
                <div>
                  <h3 className="font-bold text-foreground text-lg leading-tight">{a.name}</h3>
                  <span className="text-xs font-medium text-foreground/40 uppercase tracking-widest">{a.categoryName}</span>
                </div>
              </div>

              <div className="flex items-end justify-between mt-auto pt-6">
                <div>
                  <div className="text-3xl font-black text-foreground">{a.availableQuantity} <span className="text-sm font-semibold text-foreground/40">/ {a.totalQuantity}</span></div>
                  <div className="text-xs text-foreground/60 font-medium">Units Available</div>
                </div>

                <button
                  onClick={() => handleAddStock(a.id)}
                  className="bg-background/50 hover:bg-fuchsia-50 text-fuchsia-600 border border-border/50 hover:border-fuchsia-200 p-2.5 rounded-xl transition-all shadow-sm"
                  title="Add Stock"
                >
                  <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4v16m8-8H4" /></svg>
                </button>
              </div>
            </div>
          ))}
          {accessories.length === 0 && (
            <div className="col-span-1 md:col-span-2 lg:col-span-3 text-center py-20 text-foreground/40 border-2 border-dashed border-border/50 rounded-2xl">
              No accessories found. Register your first consumable to start tracking stock.
            </div>
          )}
        </div>
      )}
    </div>
  );
}
