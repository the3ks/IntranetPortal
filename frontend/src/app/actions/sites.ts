"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function createSiteAction(formData: FormData) {
  const data = { 
      name: formData.get("name"),
      address: formData.get("address")
  };

  const res = await fetchWithAuth("/api/sites", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/sites");
    revalidatePath("/employees/new");
    revalidatePath("/employees/[id]/edit");
  } else {
    throw new Error("Failed to create site");
  }
}

export async function updateSiteAction(formData: FormData) {
  const id = formData.get("id");
  const data = { 
      name: formData.get("name"),
      address: formData.get("address")
  };

  const res = await fetchWithAuth(`/api/sites/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/sites");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to update site");
  }
}

export async function deleteSiteAction(id: number) {
  const res = await fetchWithAuth(`/api/sites/${id}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/admin/sites");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to delete site");
  }
}
