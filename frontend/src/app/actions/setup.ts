"use server";
import { fetchWithAuth } from "@/lib/api";

export async function submitQuickSetupAction(payload: any) {
  try {
    const res = await fetchWithAuth("/api/setup/quick-setup", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload)
    });

    if (res.ok) {
      const data = await res.json();
      return JSON.stringify({ success: true, data });
    } else {
      let errStr = "Failed API call";
      try { errStr = await res.text(); } catch {}
      return JSON.stringify({ success: false, error: errStr });
    }
  } catch (error: any) {
    return JSON.stringify({ success: false, error: error.message });
  }
}

export async function importEmployeesCsvAction(formData: FormData) {
  try {
    const res = await fetchWithAuth("/api/setup/import-employees", {
      method: "POST",
      body: formData
    });

    if (res.ok) {
      const data = await res.json();
      return JSON.stringify({ success: true, data });
    } else {
      let errStr = "Failed API call";
      try { errStr = await res.text(); } catch {}
      return JSON.stringify({ success: false, error: errStr });
    }
  } catch (error: any) {
    return JSON.stringify({ success: false, error: error.message });
  }
}

export async function seedAssetsAction() {
  try {
    const res = await fetchWithAuth("/api/setup/seed-assets", {
      method: "POST"
    });

    if (res.ok) {
      const data = await res.json();
      return JSON.stringify({ success: true, data });
    } else {
      let errStr = "Failed API call";
      try { errStr = await res.text(); } catch {}
      return JSON.stringify({ success: false, error: errStr });
    }
  } catch (error: any) {
    return JSON.stringify({ success: false, error: error.message });
  }
}
