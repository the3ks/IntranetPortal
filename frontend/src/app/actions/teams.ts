"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function createTeamAction(formData: FormData) {
  const data = { 
      name: formData.get("name"),
      departmentId: parseInt(formData.get("departmentId") as string)
  };

  const res = await fetchWithAuth("/api/teams", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/admin/departments");
    revalidatePath("/employees/new");
  } else {
    throw new Error("Failed to create team");
  }
}

export async function updateTeamAction(formData: FormData) {
  const id = formData.get("id");
  const data = { 
      name: formData.get("name"),
      departmentId: parseInt(formData.get("departmentId") as string)
  };

  const res = await fetchWithAuth(`/api/teams/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/admin/departments");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to update team");
  }
}

export async function deleteTeamAction(id: number) {
  const res = await fetchWithAuth(`/api/teams/${id}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/admin/departments");
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to delete team");
  }
}
