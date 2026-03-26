"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function assignRoleAction(formData: FormData) {
  const userId = parseInt(formData.get("userId") as string);
  const roleId = parseInt(formData.get("roleId") as string);
  const siteStr = formData.get("siteId") as string;
  const siteId = siteStr ? parseInt(siteStr) : null;

  const res = await fetchWithAuth(`/api/users/${userId}/roles`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ roleId, siteId }),
  });
  
  if (res.ok) {
    revalidatePath("/admin/users");
    return { success: true };
  } else {
    try {
      const txt = await res.text();
      return { success: false, error: txt };
    } catch {
      return { success: false, error: "Failed to map capability to account." };
    }
  }
}

export async function removeRoleAction(userId: number, mappingId: number) {
  const res = await fetchWithAuth(`/api/users/${userId}/roles/${mappingId}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/admin/users");
    return { success: true };
  } else {
    return { success: false, error: "Failed to revoke operational clearance." };
  }
}
