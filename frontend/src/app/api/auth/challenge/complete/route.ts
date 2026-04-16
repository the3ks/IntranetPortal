import { NextResponse } from "next/server";
import { API_BASE_URL } from "@/lib/api";

export async function POST(request: Request) {
  const payload = await request.json();

  const upstream = await fetch(`${API_BASE_URL}/api/auth/challenge/complete`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(payload),
  });

  const body = await upstream.json().catch(() => ({}));
  if (!upstream.ok) {
    return NextResponse.json(body, { status: upstream.status });
  }

  const token = body.token as string | undefined;
  if (!token) {
    return NextResponse.json({ error: "Missing token in challenge response." }, { status: 500 });
  }

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

  const response = NextResponse.json({ role: body.role ?? "Staff" });
  response.cookies.set("auth_token", token, {
    httpOnly: true,
    secure: isProd,
    sameSite: "lax",
    path: "/",
    domain: cookieDomain,
    maxAge: 60 * 60 * 8,
  });

  return response;
}
