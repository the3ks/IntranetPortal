import MainLayout from "@/components/layout/MainLayout";

import { CartProvider } from "./CartContext";
import GlobalCartUI from "./GlobalCartUI";

export default function AssetsLayout({ children }: { children: React.ReactNode }) {
  return (
    <MainLayout>
      <CartProvider>
        <div className="max-w-7xl mx-auto space-y-8 relative">
          {children}
        </div>
        <GlobalCartUI />
      </CartProvider>
    </MainLayout>
  );
}
