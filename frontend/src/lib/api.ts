import { cookies } from "next/headers";

export const API_BASE_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";

/**
 * A Next.js Server Component utility to seamlessly inject the JWT 
 * HTTP-Only session token into C# Backend API requests.
 */
export async function fetchWithAuth(endpoint: string, options: RequestInit = {}) {
  const cookieStore = await cookies();
  const token = cookieStore.get("auth_token")?.value;

  const headers = new Headers(options.headers);
  
  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }
  
  const isFormData = typeof FormData !== "undefined" && options.body instanceof FormData;
  if (!headers.has("Content-Type") && !isFormData) {
    headers.set("Content-Type", "application/json");
  }

  // Attempt to parse dynamic full URLs or relative API paths gracefully
  const url = endpoint.startsWith("http") ? endpoint : `${API_BASE_URL}${endpoint}`;

  const res = await fetch(url, {
    ...options,
    headers,
  });

  return res;
}
