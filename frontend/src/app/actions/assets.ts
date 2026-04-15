"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

// --- VIEW/FETCH ACTIONS ---
export async function getAssetsListAction(mode?: string) {
  const url = mode ? `/api/assets?mode=${mode}` : "/api/assets";
  const res = await fetchWithAuth(url, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function getAssetRequestsAction(type: "mine" | "approvals") {
  const url = type === "mine" ? "/api/assetrequests/my-requests" : "/api/assetrequests/pending-approvals";
  const res = await fetchWithAuth(url, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function getCategoryApproversAction(categoryId: number, departmentId: number) {
  const res = await fetchWithAuth(`/api/assetrequests/approvers?categoryId=${categoryId}&departmentId=${departmentId}`, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

// --- DICTIONARIES (Categories & Models) ---
export async function getCategoriesAction() {
  const res = await fetchWithAuth("/api/assetdictionaries/categories", { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function getApproverGroupsAction() {
  const res = await fetchWithAuth("/api/assetdictionaries/groups", { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function createApproverGroupAction(name: string) {
  const res = await fetchWithAuth("/api/assetdictionaries/groups", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ Name: name })
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false };
}

export async function getGroupMembersAction(groupId: number) {
  const res = await fetchWithAuth(`/api/assetdictionaries/groups/${groupId}/members`, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function getEligibleApproversAction(search?: string) {
  const url = search ? `/api/assetdictionaries/groups/eligible-approvers?search=${encodeURIComponent(search)}` : "/api/assetdictionaries/groups/eligible-approvers";
  const res = await fetchWithAuth(url, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function addGroupMemberAction(groupId: number, employeeId: number) {
  const res = await fetchWithAuth(`/api/assetdictionaries/groups/${groupId}/members`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(employeeId)
  });
  if (res.ok) revalidatePath("/assets");
  return res.ok;
}

export async function removeGroupMemberAction(groupId: number, employeeId: number) {
  const res = await fetchWithAuth(`/api/assetdictionaries/groups/${groupId}/members/${employeeId}`, {
    method: "DELETE"
  });
  if (res.ok) revalidatePath("/assets");
  return res.ok;
}

export async function getModelsAction(categoryId?: number) {
  const url = categoryId ? `/api/assetdictionaries/models?categoryId=${categoryId}` : "/api/assetdictionaries/models";
  const res = await fetchWithAuth(url, { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function createCategoryAction(data: any) {
  const res = await fetchWithAuth("/api/assetdictionaries/categories", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to create category" };
}

export async function createModelAction(data: any) {
  const res = await fetchWithAuth("/api/assetdictionaries/models", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to create model" };
}

// --- HARD ASSETS ---
export async function createAssetAction(data: any) {
  const res = await fetchWithAuth("/api/assets", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to create asset" };
}

export async function assignAssetAction(id: number, data: any) {
  const res = await fetchWithAuth(`/api/assets/${id}/assign`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to assign asset" };
}

export async function returnAssetAction(id: number, data: any) {
  const res = await fetchWithAuth(`/api/assets/${id}/return`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to return asset" };
}

// --- ACCESSORIES ---
export async function getAccessoriesAction() {
  const res = await fetchWithAuth("/api/accessories", { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function createAccessoryAction(data: any) {
  const res = await fetchWithAuth("/api/accessories", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to create accessory" };
}

export async function addAccessoryStockAction(id: number, quantity: number) {
  const res = await fetchWithAuth(`/api/accessories/${id}/add-stock`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(quantity),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to add accessory stock" };
}

// --- REQUESTS WORKFLOW ---
export async function getFulfillmentQueueAction() {
  const res = await fetchWithAuth("/api/assetrequests/fulfillment-queue", { cache: "no-store" });
  if (res.ok) return await res.json();
  return [];
}

export async function createRequestAction(data: any) {
  const res = await fetchWithAuth("/api/assetrequests", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to create request" };
}

export async function approveRequestAction(id: number) {
  const res = await fetchWithAuth(`/api/assetrequests/line-items/${id}/approve`, {
    method: "POST",
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to approve request" };
}

export async function fulfillRequestAction(id: number, data: any) {
  const res = await fetchWithAuth(`/api/assetrequests/line-items/${id}/fulfill`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  if (res.ok) {
    revalidatePath("/assets");
    return { success: true };
  }
  return { success: false, error: "Failed to fulfill request" };
}

export async function updateCategoryAction(id: number, payload: any) {
  await fetchWithAuth(`/api/assetdictionaries/categories/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });
}

export async function toggleCategoryAction(id: number) {
  await fetchWithAuth(`/api/assetdictionaries/categories/${id}/toggle-active`, {
    method: "PUT"
  });
}

export async function updateModelAction(id: number, payload: any) {
  await fetchWithAuth(`/api/assetdictionaries/models/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload)
  });
}

export async function toggleModelAction(id: number) {
  await fetchWithAuth(`/api/assetdictionaries/models/${id}/toggle-active`, {
    method: "PUT"
  });
}
