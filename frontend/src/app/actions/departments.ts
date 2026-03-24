"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function createDepartmentAction(formData: FormData) {
  const siteIdRaw = formData.get("siteId");
  const data = { 
    name: formData.get("name"),
    siteId: siteIdRaw ? parseInt(siteIdRaw as string) : null
  };

  const res = await fetchWithAuth("/api/departments", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/departments");
    revalidatePath("/employees/new");
    revalidatePath("/employees/[id]/edit");
  } else {
    throw new Error("Failed to create department");
  }
}

export async function updateDepartmentAction(formData: FormData) {
  const id = formData.get("id");
  const siteIdRaw = formData.get("siteId");
  const data = { 
    name: formData.get("name"),
    siteId: siteIdRaw ? parseInt(siteIdRaw as string) : null
  };

  const res = await fetchWithAuth(`/api/departments/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/departments");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to update department");
  }
}

export async function deleteDepartmentAction(id: number) {
  const res = await fetchWithAuth(`/api/departments/${id}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/departments");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to delete department");
  }
}
