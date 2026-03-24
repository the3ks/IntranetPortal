"use server";
import { fetchWithAuth } from "@/lib/api";
import { revalidatePath } from "next/cache";

export async function createAnnouncementAction(formData: FormData) {
  const title = formData.get("title");
  const content = formData.get("content");
  const siteIdRaw = formData.get("siteId");
  
  if (!title || !content) {
    throw new Error("Missing fields");
  }

  const siteId = siteIdRaw ? parseInt(siteIdRaw as string) : null;

  const res = await fetchWithAuth("/api/announcements", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ title, content, siteId })
  });

  if (res.ok) {
    revalidatePath("/announcements");
    revalidatePath("/");
  } else {
    throw new Error("Failed to post announcement");
  }
}

export async function deleteAnnouncementAction(id: number) {
  const res = await fetchWithAuth(`/api/announcements/${id}`, {
    method: "DELETE"
  });

  if (res.ok) {
    revalidatePath("/announcements");
    revalidatePath("/");
  } else {
    throw new Error("Failed to delete announcement");
  }
}
