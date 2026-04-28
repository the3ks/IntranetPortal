import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

export function proxy(request: NextRequest) {
  const token = request.cookies.get("auth_token")?.value;
  const isLoginPage = request.nextUrl.pathname.startsWith("/login");
  const logoutReason = request.nextUrl.searchParams.get("reason");
  const isLogoutRedirect = logoutReason === "signed_out" || request.nextUrl.searchParams.has("kickout");

  // Handle centralized logout redirect and clear auth cookie.
  // Preferred flag is /login?reason=signed_out (legacy kickout still supported).
  // Future enhancements (refresh token revocation, etc.) should be added here.
  if (isLoginPage && isLogoutRedirect) {
    const response = NextResponse.next();
    response.cookies.set({
      name: "auth_token",
      value: "",
      path: "/",
      maxAge: 0,
    });
    return response;
  }

  // Protect internal routes: Redirect unauthenticated users to the login screen
  if (!token && !isLoginPage) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  // Block authenticated users from viewing the login screen
  if (token && isLoginPage && !isLogoutRedirect) {
    return NextResponse.redirect(new URL("/", request.url));
  }

  return NextResponse.next();
}

// Ensure middleware applies everywhere except static assets and API routes
export const config = {
  matcher: ["/((?!api|_next/static|_next/image|favicon.ico|manifest.json|manifest.webmanifest|sw.js|icon-.*).*)"],
};
