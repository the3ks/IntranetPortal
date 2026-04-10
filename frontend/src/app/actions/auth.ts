"use server";

import { cookies } from "next/headers";
import { redirect } from "next/navigation";
import { jwtDecode } from "jwt-decode";
import { API_BASE_URL } from "@/lib/api";

export async function getUserSession() {
  const cookieStore = await cookies();
  const token = cookieStore.get("auth_token")?.value;
  if (!token) return null;
  try {
    return jwtDecode(token) as any;
  } catch { return null; }
}

export async function loginAction(prevState: any, formData: FormData) {
  const email = formData.get("email")?.toString();
  const password = formData.get("password")?.toString();

  if (!email || !password) {
    return { error: "Email and password are required." };
  }

  try {
    const res = await fetch(`${API_BASE_URL}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    if (!res.ok) {
      const errorData = await res.json().catch(() => ({}));
      return { error: errorData.message || "Invalid credentials." };
    }

    const data = await res.json();
    const token = data.token;
    
    // Compute domain sharing dynamically for Microservice Subdomains
    const isProd = process.env.NODE_ENV === "production";
    
    // In production, strip the exact host from API_BASE_URL to allow wildcard cookies.
    // e.g. "https://api.yourdomain.com" -> ".yourdomain.com" (to allow drinks.yourdomain.com)
    // If not properly parseable, we skip the domain field.
    let cookieDomain = undefined;
    if (isProd && API_BASE_URL) {
      try {
        const urlObj = new URL(API_BASE_URL);
        const hostParts = urlObj.hostname.split('.');
        if (hostParts.length >= 2 && urlObj.hostname !== 'localhost') {
           // Extract base domain like 'yourdomain.com'
           cookieDomain = `.${hostParts.slice(-2).join('.')}`;
        }
      } catch (e) {
        console.warn("Failed to parse API_BASE_URL for wildcard cookie domain");
      }
    }

    // Encapsulate token securely in Next.js Server Cookie
    const cookieStore = await cookies();
    cookieStore.set("auth_token", token, {
      httpOnly: true,
      secure: isProd,
      sameSite: "lax",
      path: "/",
      domain: cookieDomain, // Explicit wildcard for microservice sharing!
      maxAge: 60 * 60 * 8, // 8 hour working session
    });
  } catch (error: any) {
    console.error("LOGIN FETCH ERROR:", error);
    return { error: `Connection failed: ${error?.message || "Unknown error"}. Target URL: ${API_BASE_URL}` };
  }

  // Redirect runs outside try/catch to natively throw the Next.js redirect boundary
  redirect("/");
}

export async function logoutAction() {
  const cookieStore = await cookies();
  cookieStore.delete("auth_token");
  redirect("/login");
}
