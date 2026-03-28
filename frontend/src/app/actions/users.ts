"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function assignRoleAction(formData: FormData) {
  const userId = parseInt(formData.get("userId") as string);
  const roleId = parseInt(formData.get("roleId") as string);
  const siteStr = formData.get("siteId") as string;
  const siteId = siteStr ? parseInt(siteStr) : null;
  const deptStr = formData.get("departmentId") as string;
  const departmentId = deptStr ? parseInt(deptStr) : null;

  const res = await fetchWithAuth(`/api/users/${userId}/roles`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ roleId, siteId, departmentId }),
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

export async function resetUserPasswordAction(userId: number, newPassword: string) {
  const res = await fetchWithAuth(`/api/users/${userId}/reset-password`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ newPassword }),
  });
  
  if (res.ok) {
    return { success: true };
  } else {
    try {
      const txt = await res.text();
      return { success: false, error: txt };
    } catch {
      return { success: false, error: "Failed to execute global password mutation securely." };
    }
  }
}

export async function createDelegationAction(formData: FormData) {
  const SourceUserId = parseInt(formData.get("sourceUserId") as string);
  const SubstituteUserId = parseInt(formData.get("substituteUserId") as string);
  const UserRoleId = parseInt(formData.get("userRoleId") as string);
  const StartDate = formData.get("startDate") as string;
  const EndDate = formData.get("endDate") as string;

  const res = await fetchWithAuth(`/api/roledelegations`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ SourceUserId, SubstituteUserId, UserRoleId, StartDate, EndDate }),
  });
  
  if (res.ok) {
    revalidatePath("/admin/users");
    return { success: true };
  } else {
    try {
      const txt = await res.text();
      return { success: false, error: txt };
    } catch {
      return { success: false, error: "Failed to delegate." };
    }
  }
}

export async function revokeDelegationAction(delegationId: number) {
  const res = await fetchWithAuth(`/api/roledelegations/${delegationId}`, { method: "DELETE" });
  if (res.ok) {
    revalidatePath("/admin/users");
    return { success: true };
  } else {
    return { success: false, error: "Failed to revoke delegation." };
  }
}

export async function searchUsersAction(query: string) {
  const res = await fetchWithAuth(`/api/users?search=${encodeURIComponent(query)}`);
  if (res.ok) {
    return await res.json();
  }
  return [];
}

export async function getDelegationsAction(userId: number) {
  const res = await fetchWithAuth(`/api/roledelegations/user/${userId}`);
  if (res.ok) {
    return await res.json();
  }
  return [];
}
