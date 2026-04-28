import { NextResponse } from "next/server";
import { API_BASE_URL } from "@/lib/api";

// Handle CORS preflight from cross-domain microservice frontends
export async function OPTIONS(request: Request) {
  const origin = request.headers.get("origin") ?? "";
  return new NextResponse(null, {
    status: 204,
    headers: {
      "Access-Control-Allow-Origin": origin,
      "Access-Control-Allow-Methods": "POST, OPTIONS",
      "Access-Control-Allow-Headers": "Content-Type",
      "Access-Control-Allow-Credentials": "true",
    },
  });
}

export async function POST(request: Request) {
  const isProd = process.env.NODE_ENV === "production";
  let cookieDomain: string | undefined;

  if (isProd && API_BASE_URL) {
    try {
      const urlObj = new URL(API_BASE_URL);
      const hostParts = urlObj.hostname.split(".");
      if (hostParts.length >= 2 && urlObj.hostname !== "localhost") {
        cookieDomain = `.${hostParts.slice(-2).join(".")}`;
      }
    } catch {
      cookieDomain = undefined;
    }
  }

  const origin = request.headers.get("origin") ?? "";
  const response = NextResponse.json({ success: true });
  response.headers.set("Access-Control-Allow-Origin", origin);
  response.headers.set("Access-Control-Allow-Credentials", "true");
  response.cookies.set({
    name: "auth_token",
    value: "",
    httpOnly: true,
    secure: isProd,
    sameSite: "lax",
    path: "/",
    ...(cookieDomain ? { domain: cookieDomain } : {}),
    maxAge: 0,
  });

  return response;
}
