"use client";

import React, { useState } from "react";
import { useCart } from "./CartContext";
import { Dialog } from "@headlessui/react";
import { XMarkIcon, ShoppingCartIcon } from "@heroicons/react/24/outline";
import { useRouter } from "next/navigation";

export default function GlobalCartUI() {
  const { items } = useCart();
  const [isOpen, setIsOpen] = useState(false);
  const router = useRouter();

  if (items.length === 0) return null;

  return (
    <>
      <button
        onClick={() => setIsOpen(true)}
        className="fixed bottom-8 right-8 p-4 bg-indigo-600 text-white rounded-full shadow-lg hover:bg-indigo-700 hover:scale-105 transition-all z-50 flex items-center gap-2"
      >
        <ShoppingCartIcon className="h-6 w-6" />
        <span className="font-semibold bg-white text-indigo-600 rounded-full px-2 py-0.5 text-sm">
          {items.length}
        </span>
      </button>

      <Dialog open={isOpen} onClose={() => setIsOpen(false)} className="relative z-50">
        <div className="fixed inset-0 bg-black/30 backdrop-blur-sm" aria-hidden="true" />
        
        <div className="fixed inset-y-0 right-0 max-w-sm w-full bg-white shadow-xl flex flex-col animate-in slide-in-from-right">
          <div className="flex items-center justify-between p-6 border-b border-gray-100">
            <h2 className="text-xl font-semibold text-gray-900">Requisition Cart</h2>
            <button onClick={() => setIsOpen(false)} className="text-gray-400 hover:text-gray-600 border border-gray-200 rounded-md p-1">
              <XMarkIcon className="h-5 w-5" />
            </button>
          </div>

          <div className="flex-1 overflow-y-auto p-6 space-y-4">
            {items.map((item) => (
              <div key={item.id} className="p-4 rounded-lg flex flex-col gap-2 relative bg-gray-50 border border-gray-100">
                <div className="flex justify-between items-start">
                  <div>
                    <p className="font-semibold text-gray-900">
                      {item.type === "Custom" ? "Custom Request" : (item.modelName || item.accessoryName)}
                    </p>
                    <p className="text-sm text-gray-500">{item.categoryName || "No Category Selected"}</p>
                  </div>
                  <span className="bg-indigo-50 text-indigo-700 text-xs px-2 py-1 rounded-md font-medium">Qty: {item.quantity}</span>
                </div>
              </div>
            ))}
          </div>

          <div className="p-6 border-t border-gray-100 bg-gray-50 space-y-4">
            <p className="text-sm text-gray-500 text-center">
              Items will be routed for approval according to their predefined categories.
            </p>
            <button
              onClick={() => {
                setIsOpen(false);
                router.push("/assets/checkout");
              }}
              className="w-full bg-indigo-600 text-white rounded-lg py-3 font-semibold shadow hover:bg-indigo-700 transition"
            >
              Proceed to Checkout
            </button>
          </div>
        </div>
      </Dialog>
    </>
  );
}
