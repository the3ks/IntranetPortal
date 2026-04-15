"use client";

import React, { createContext, useContext, useState, ReactNode } from "react";

export type CartItemType = "Asset" | "Accessory" | "Custom";

export interface CartItem {
  id: string; // generated uuid for front-end tracking
  type: CartItemType;
  categoryId?: number;
  categoryName?: string;
  modelId?: number;
  modelName?: string;
  accessoryId?: number;
  accessoryName?: string;
  quantity: number;
  justification: string;
  // Routing context
  allowRequesterToSelectApprover?: boolean;
}

interface CartContextProps {
  items: CartItem[];
  addToCart: (item: Omit<CartItem, "id">) => void;
  removeFromCart: (id: string) => void;
  clearCart: () => void;
  updateItemJustification: (id: string, justification: string) => void;
  updateItemQuantity: (id: string, quantity: number) => void;
}

const CartContext = createContext<CartContextProps | undefined>(undefined);

export function CartProvider({ children }: { children: ReactNode }) {
  const [items, setItems] = useState<CartItem[]>([]);

  const addToCart = (item: Omit<CartItem, "id">) => {
    const newItem = { ...item, id: crypto.randomUUID() };
    setItems((prev) => [...prev, newItem]);
  };

  const removeFromCart = (id: string) => {
    setItems((prev) => prev.filter((i) => i.id !== id));
  };

  const clearCart = () => setItems([]);

  const updateItemJustification = (id: string, justification: string) => {
    setItems((prev) =>
      prev.map((i) => (i.id === id ? { ...i, justification } : i))
    );
  };

  const updateItemQuantity = (id: string, quantity: number) => {
    setItems((prev) =>
      prev.map((i) => (i.id === id ? { ...i, quantity } : i))
    );
  };

  return (
    <CartContext.Provider
      value={{
        items,
        addToCart,
        removeFromCart,
        clearCart,
        updateItemJustification,
        updateItemQuantity,
      }}
    >
      {children}
    </CartContext.Provider>
  );
}

export function useCart() {
  const context = useContext(CartContext);
  if (!context) {
    throw new Error("useCart must be used within a CartProvider");
  }
  return context;
}
