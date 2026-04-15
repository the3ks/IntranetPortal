"use client";

import React, { useState, useEffect } from "react";
import { useCart, CartItem } from "../CartContext";
import { useRouter } from "next/navigation";
import { TrashIcon, UserCircleIcon } from "@heroicons/react/24/outline";
import { getCategoryApproversAction, createRequestAction } from "@/app/actions/assets";

export default function CheckoutForm({ departmentId }: { departmentId: number }) {
  const { items, removeFromCart, updateItemJustification, updateItemQuantity, clearCart } = useCart();
  const router = useRouter();
  
  // Mapping of categoryId -> available approvers (Employee array)
  const [approversCache, setApproversCache] = useState<Record<number, any[]>>({});
  
  // Mapping of CartItem.id -> selectedApproverEmployeeId
  const [selectedApprovers, setSelectedApprovers] = useState<Record<string, number>>({});
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    // For every item in the cart that requires manual approval selection, fetch the eligible user list
    items.forEach((item) => {
      if (item.allowRequesterToSelectApprover && item.categoryId && !approversCache[item.categoryId]) {
        getCategoryApproversAction(item.categoryId, departmentId).then((data) => {
          setApproversCache((prev) => ({ ...prev, [item.categoryId!]: data }));
        });
      }
    });
  }, [items, departmentId, approversCache]);

  const handleSubmit = async () => {
    // Validation: make sure all manual override items have an approver chosen
    const missingApprovers = items.some(
      i => i.allowRequesterToSelectApprover && !selectedApprovers[i.id]
    );
    if (missingApprovers) {
      alert("Please select a reviewing manager for the designated line items.");
      return;
    }
    
    // Validate empty
    if (items.length === 0) return;

    setIsSubmitting(true);
    
    const payload = {
      Items: items.map((i) => ({
        Type: i.type,
        RequestedCategoryId: i.categoryId,
        RequestedModelId: i.modelId,
        RequestedAccessoryId: i.accessoryId,
        Quantity: i.quantity,
        Justification: i.justification || "Requisitioning new asset.",
        SelectedApproverEmployeeId: selectedApprovers[i.id] || null,
      }))
    };

    const res = await createRequestAction(payload);
    if (res.success) {
      clearCart();
      router.push("/assets"); // Redirect back to request history
    } else {
      alert("Error submitting request: " + res.error);
      setIsSubmitting(false);
    }
  };

  if (items.length === 0) {
    return (
      <div className="bg-white p-12 text-center rounded-xl border border-gray-200">
        <p className="text-gray-500 mb-4">Your cart is completely empty.</p>
        <button onClick={() => router.push("/assets/catalog")} className="text-indigo-600 font-semibold hover:underline">
          Return to Catalog
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-8">
      {/* Line Items List */}
      <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden divide-y divide-gray-100">
        {items.map((item) => {
          const requiresApprover = item.allowRequesterToSelectApprover;
          const approversList = (item.categoryId && approversCache[item.categoryId]) || [];

          return (
            <div key={item.id} className="p-6 md:flex gap-6 items-start hover:bg-gray-50 transition">
              <div className="flex-1 space-y-4">
                <div>
                  <div className="flex items-center gap-3 mb-1">
                    <span className="text-xs font-bold px-2 py-1 rounded bg-indigo-100 text-indigo-800 uppercase tracking-wider">
                      {item.type}
                    </span>
                    <span className="text-gray-500 text-sm">{item.categoryName || "General Custom"}</span>
                  </div>
                  <h3 className="text-xl font-bold text-gray-900">
                    {item.type === "Custom" ? "Custom Asset Reference" : (item.modelName || item.accessoryName)}
                  </h3>
                </div>

                <div className="grid md:grid-cols-2 gap-4">
                  <div>
                    <label className="block text-sm font-medium text-gray-700 mb-1">Quantity</label>
                    <input
                      type="number"
                      min={1}
                      className="form-input rounded-md border-gray-300 w-full sm:w-24"
                      value={item.quantity}
                      onChange={(e) => updateItemQuantity(item.id, parseInt(e.target.value) || 1)}
                    />
                  </div>
                  <div className="md:col-span-2">
                    <label className="block text-sm font-medium text-gray-700 mb-1">Business Justification *</label>
                    <textarea
                      required
                      placeholder="Why do you need this?"
                      className="form-textarea rounded-md border-gray-300 w-full"
                      rows={2}
                      value={item.justification}
                      onChange={(e) => updateItemJustification(item.id, e.target.value)}
                    />
                  </div>
                </div>

                {/* DYNAMIC CATEGORY ROUTING UI - Shows conditionally */}
                {requiresApprover && (
                  <div className="bg-yellow-50 border border-yellow-200 rounded-lg p-4 mt-4">
                    <label className="flex items-center gap-2 text-sm font-bold text-yellow-800 mb-2">
                      <UserCircleIcon className="h-5 w-5" />
                      Select Reviewing Manager
                    </label>
                    <p className="text-xs text-yellow-700 mb-3">
                      This category ({item.categoryName}) requires you to manually select the specific individual responsible for authorizing this purchase.
                    </p>
                    <select
                      className="form-select text-sm rounded-md border-gray-300 w-full max-w-sm"
                      value={selectedApprovers[item.id] || ""}
                      onChange={(e) => setSelectedApprovers(prev => ({ ...prev, [item.id]: parseInt(e.target.value) }))}
                    >
                      <option value="" disabled>-- Select an authorized manager --</option>
                      {approversList.map(a => (
                        <option key={a.employeeId} value={a.employeeId}>
                          {a.fullName} ({a.email})
                        </option>
                      ))}
                    </select>
                  </div>
                )}
                
                {!requiresApprover && (
                   <div className="text-sm mt-4 text-green-600 font-medium">
                      ✓ This item will be automatically routed to the default departmental queue.
                   </div>
                )}

              </div>
              
              <div className="mt-4 md:mt-0 flex">
                <button
                  onClick={() => removeFromCart(item.id)}
                  className="text-red-500 hover:text-red-700 p-2 rounded-md hover:bg-red-50 transition"
                  title="Remove from requisition"
                >
                  <TrashIcon className="h-6 w-6" />
                </button>
              </div>
            </div>
          );
        })}
      </div>

      <div className="flex justify-end pt-4">
        <button
          onClick={handleSubmit}
          disabled={isSubmitting}
          className="bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 text-white px-8 py-3 rounded-lg font-bold shadow-lg transition-transform hover:scale-105 active:scale-95"
        >
          {isSubmitting ? "Submitting..." : `Submit Requisition (${items.length} items)`}
        </button>
      </div>
    </div>
  );
}
