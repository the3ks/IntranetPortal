"use client";

import React, { useEffect, useState } from "react";
import { getModelsAction, getAccessoriesAction, getCategoriesAction } from "@/app/actions/assets";
import { useCart } from "../CartContext";
import { PlusIcon } from "@heroicons/react/24/outline";

export default function CatalogPage() {
  const [categories, setCategories] = useState<any[]>([]);
  const [models, setModels] = useState<any[]>([]);
  const [accessories, setAccessories] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<"models" | "accessories" | "custom">("models");
  const [customCategory, setCustomCategory] = useState<number | "">("");
  const [customItemName, setCustomItemName] = useState("");

  const { addToCart } = useCart();

  useEffect(() => {
    async function loadData() {
      const [cats, mods, accs] = await Promise.all([
        getCategoriesAction(),
        getModelsAction(),
        getAccessoriesAction(),
      ]);
      setCategories(cats);
      setModels(mods.filter((m: any) => m.isActive));
      setAccessories(accs);
      setLoading(false);
    }
    loadData();
  }, []);

  const handleAddModel = (model: any) => {
    const category = categories.find((c) => c.id === model.categoryId);
    addToCart({
      type: "Asset",
      modelId: model.id,
      modelName: `${model.manufacturer} ${model.name}`,
      categoryId: model.categoryId,
      categoryName: category?.name,
      allowRequesterToSelectApprover: category?.allowRequesterToSelectApprover,
      quantity: 1,
      justification: "",
    });
  };

  const handleAddAccessory = (acc: any) => {
    const category = categories.find((c) => c.id === acc.categoryId);
    addToCart({
      type: "Accessory",
      accessoryId: acc.id,
      accessoryName: `${acc.manufacturer} ${acc.name}`,
      categoryId: acc.categoryId,
      categoryName: category?.name,
      allowRequesterToSelectApprover: category?.allowRequesterToSelectApprover,
      quantity: 1,
      justification: "",
    });
  };

  const handleAddCustom = (e: React.FormEvent) => {
    e.preventDefault();
    if (!customCategory || !customItemName.trim()) return;

    const category = categories.find((c) => c.id === customCategory);
    addToCart({
      type: "Custom",
      accessoryName: customItemName, // Stored in accessoryName field as generic visual desc or we can use custom tracking
      modelName: customItemName,
      categoryId: category?.id,
      categoryName: category?.name,
      allowRequesterToSelectApprover: category?.allowRequesterToSelectApprover,
      quantity: 1,
      justification: "",
    });
    setCustomItemName("");
    setCustomCategory("");
    alert(`Added Custom Item: ${customItemName}`);
  };

  if (loading) return <div className="text-center py-20 animate-pulse">Loading Catalog...</div>;

  return (
    <div className="space-y-6">
      <div className="flex justify-between items-center">
        <h1 className="text-3xl font-bold text-gray-900 tracking-tight">Requisition Catalog</h1>
        <div className="flex bg-gray-100 p-1 rounded-lg">
          <button
            onClick={() => setActiveTab("models")}
            className={`px-4 py-2 rounded-md text-sm font-medium transition ${activeTab === "models"
              ? "bg-white text-indigo-700 shadow"
              : "text-gray-500 hover:text-gray-700"
              }`}
          >
            Hard Assets
          </button>
          <button
            onClick={() => setActiveTab("accessories")}
            className={`px-4 py-2 rounded-md text-sm font-medium transition ${activeTab === "accessories"
                ? "bg-white text-indigo-700 shadow"
                : "text-gray-500 hover:text-gray-700"
              }`}
          >
            Accessories
          </button>
          <button
            onClick={() => setActiveTab("custom")}
            className={`px-4 py-2 rounded-md text-sm font-medium transition ${activeTab === "custom"
                ? "bg-white text-indigo-700 shadow"
                : "text-gray-500 hover:text-gray-700"
              }`}
          >
            Custom Request
          </button>
        </div>
      </div>

      {activeTab === "models" && (
        <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {models.map((model) => (
            <div key={model.id} className="bg-white border focus-within:ring-2 focus-within:ring-indigo-500 border-gray-200 rounded-xl overflow-hidden shadow-sm hover:shadow-md transition group flex flex-col">
              <div className="p-6 flex-1">
                <p className="text-xs font-semibold uppercase tracking-wider text-indigo-600 mb-1">
                  {categories.find((c) => c.id === model.categoryId)?.name || "Uncategorized"}
                </p>
                <h3 className="text-lg font-bold text-gray-900 group-hover:text-indigo-600 transition">{model.name}</h3>
                <p className="text-gray-500 text-sm">{model.manufacturer}</p>
                <p className="text-xs text-gray-400 mt-2 line-clamp-3">{model.description}</p>
              </div>
              <div className="p-4 bg-gray-50 border-t border-gray-100 mt-auto">
                <button
                  onClick={() => handleAddModel(model)}
                  className="w-full flex items-center justify-center gap-2 bg-white border border-gray-300 text-gray-700 hover:bg-slate-50 hover:text-indigo-700 font-medium py-2 px-4 rounded-lg transition"
                >
                  <PlusIcon className="h-4 w-4" />
                  Add to Cart
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {activeTab === "accessories" && (
        <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {accessories.map((acc) => (
            <div key={acc.id} className="bg-white border focus-within:ring-2 focus-within:ring-indigo-500 border-gray-200 rounded-xl overflow-hidden shadow-sm hover:shadow-md transition group flex flex-col">
              <div className="p-6 flex-1">
                <p className="text-xs font-semibold uppercase tracking-wider text-indigo-600 mb-1">
                  {categories.find((c) => c.id === acc.categoryId)?.name || "Uncategorized"}
                </p>
                <h3 className="text-lg font-bold text-gray-900 group-hover:text-indigo-600 transition">{acc.name}</h3>
                <p className="text-gray-500 text-sm mb-2">{acc.manufacturer}</p>
                <span className={`inline-flex items-center px-2 py-0.5 rounded text-xs font-medium ${acc.availableQuantity > 0 ? "bg-green-100 text-green-800" : "bg-red-100 text-red-800"
                  }`}>
                  {acc.availableQuantity > 0 ? `${acc.availableQuantity} in stock` : "Out of stock (Backorder)"}
                </span>
                <p className="text-xs text-gray-400 mt-4 line-clamp-2">{acc.description}</p>
              </div>
              <div className="p-4 bg-gray-50 border-t border-gray-100 mt-auto">
                <button
                  onClick={() => handleAddAccessory(acc)}
                  className="w-full flex items-center justify-center gap-2 bg-white border border-gray-300 text-gray-700 hover:bg-slate-50 hover:text-indigo-700 font-medium py-2 px-4 rounded-lg transition"
                >
                  <PlusIcon className="h-4 w-4" />
                  Add to Cart
                </button>
              </div>
            </div>
          ))}
        </div>
      )}

      {activeTab === "custom" && (
        <div className="max-w-2xl bg-white border border-gray-200 rounded-xl shadow-sm overflow-hidden p-6">
          <h2 className="text-xl font-bold text-gray-900 mb-2">Custom Requisition</h2>
          <p className="text-gray-500 text-sm mb-6">
            If what you need is not listed under Hard Assets or Accessories (such as General Stationaries, Software Licenses, or unique furniture), please submit a custom line item here.
          </p>
          <form onSubmit={handleAddCustom} className="space-y-4">
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1">Select Asset Category</label>
              <select
                required
                className="w-full border-gray-300 rounded-lg shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                value={customCategory}
                onChange={(e) => setCustomCategory(parseInt(e.target.value) || "")}
              >
                <option value="" disabled>-- Select a bounded category --</option>
                {categories.filter(c => c.isActive).map(c => (
                  <option key={c.id} value={c.id}>{c.name}</option>
                ))}
              </select>
            </div>
            <div>
              <label className="block text-sm font-bold text-gray-700 mb-1">Specific Item Name</label>
              <input
                required
                placeholder="e.g. 5x Boxes of Blue Ballpoint Pens"
                className="w-full border-gray-300 rounded-lg shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                value={customItemName}
                onChange={(e) => setCustomItemName(e.target.value)}
              />
            </div>
            <div className="pt-2">
              <button
                type="submit"
                className="w-full flex items-center justify-center gap-2 bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-4 rounded-lg shadow transition"
              >
                <PlusIcon className="h-5 w-5" />
                Add Custom Item to Cart
              </button>
            </div>
          </form>
        </div>
      )}
    </div>
  );
}
