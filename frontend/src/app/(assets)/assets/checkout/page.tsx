import CheckoutForm from "./CheckoutForm";
import { getUserSession } from "@/app/actions/auth";

export default async function CheckoutPage() {
  const user = await getUserSession();
  
  return (
    <div className="max-w-4xl mx-auto space-y-6">
      <h1 className="text-3xl font-bold text-gray-900">Requisition Checkout</h1>
      <p className="text-gray-500">Review your requested items and configure dynamic routing per category.</p>

      {/* We pass down the user's Department ID to initialize any specific category routing endpoints */}
      <CheckoutForm departmentId={user?.departmentId || 1} />
    </div>
  );
}
