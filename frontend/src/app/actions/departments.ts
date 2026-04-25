"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function createDepartmentAction(formData: FormData) {
  const siteIdRaw = formData.get("siteId");
  const data = { 
    name: formData.get("name"),
    siteId: siteIdRaw ? parseInt(siteIdRaw as string) : null
  };

  const res = await fetchWithAuth("/api/hr/departments", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/hr/departments");
    revalidatePath("/hr/employee-records");
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

  const res = await fetchWithAuth(`/api/hr/departments/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/hr/departments");
    revalidatePath("/hr/employee-records");
  } else {
    throw new Error("Failed to update department");
  }
}

export async function deleteDepartmentAction(id: number) {
  const res = await fetchWithAuth(`/api/hr/departments/${id}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/hr/departments");
    revalidatePath("/hr/employee-records");
  } else {
    throw new Error("Failed to delete department");
  }
}
