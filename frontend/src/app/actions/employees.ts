"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";
import { redirect } from "next/navigation";

export async function createEmployeeAction(formData: FormData) {
  const positionStr = formData.get("positionId") as string;
  const teamStr = formData.get("teamId") as string;
  const data = {
    fullName: formData.get("fullName"),
    email: formData.get("email"),
    positionId: positionStr ? parseInt(positionStr) : null,
    departmentId: parseInt(formData.get("departmentId") as string),
    teamId: teamStr ? parseInt(teamStr) : null,
    siteId: parseInt(formData.get("siteId") as string),
  };

  const res = await fetchWithAuth("/api/employees", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to create profile securely");
  }
  redirect("/employees");
}

export async function updateEmployeeAction(formData: FormData) {
  const id = formData.get("id");
  const positionStr = formData.get("positionId") as string;
  const teamStr = formData.get("teamId") as string;
  const data = {
    fullName: formData.get("fullName"),
    email: formData.get("email"),
    positionId: positionStr ? parseInt(positionStr) : null,
    departmentId: parseInt(formData.get("departmentId") as string),
    teamId: teamStr ? parseInt(teamStr) : null,
    siteId: parseInt(formData.get("siteId") as string),
  };

  const res = await fetchWithAuth(`/api/employees/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data)
  });

  if (res.ok) {
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to save assignment variables safely");
  }
  redirect("/employees");
}

export async function deleteEmployeeAction(id: number) {
  const res = await fetchWithAuth(`/api/employees/${id}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/employees");
  } else {
    throw new Error("Failed to structurally delete the entity");
  }
}
