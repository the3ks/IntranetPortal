"use client";

import { usePathname } from "next/navigation";
import Link from "next/link";
import { logoutAction } from "@/app/actions/auth";
import { ThemeToggle } from "@/components/ui/ThemeToggle";

export default function Header({ user }: { user?: any }) {
  const pathname = usePathname();
  const userRoleClaim = user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || user?.role || user?.Role;
  const isAdmin = userRoleClaim === "Admin" || (Array.isArray(userRoleClaim) && userRoleClaim.includes("Admin"));
  
  const getPageTitle = (path: string) => {
    if (!path || path === "/") return "Dashboard Overview";
    
    const segments = path.split("/").filter(Boolean);
    if (segments.length === 0) return "Dashboard Overview";
    
    const lastSegment = segments[segments.length - 1];
    
    const titleMap: Record<string, string> = {
      modules: "Module Registry",
      docs: "Wiki / Documentation",
      employees: "Employee Directory",
      departments: "Organization Departments",
      sites: "Facilities & Sites",
      "quick-setup": "Rapid Configuration",
      positions: "HR Positions",
      roles: "Security Roles",
      permissions: "Permission Registry",
    };
    
    if (titleMap[lastSegment]) return titleMap[lastSegment];
    
    return lastSegment
      .split("-")
      .map((word) => word.charAt(0).toUpperCase() + word.slice(1))
      .join(" ");
  };

  return (
    <header className="bg-background/80 backdrop-blur-md shadow-sm h-20 flex items-center justify-between px-8 sticky top-0 z-20 transition-all border-b border-border/50">
      <div className="flex items-center space-x-4">
        {pathname !== "/" && (
          <Link href="/" className="text-foreground/40 hover:text-blue-500 transition-colors p-2 -ml-2 rounded-xl hover:bg-blue-500/10 flex items-center justify-center" title="Back to Universal Portal">
             <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6"/>
            </svg>
          </Link>
        )}
        <h1 className="text-2xl font-bold text-foreground tracking-tight">{getPageTitle(pathname)}</h1>
        <span className="hidden sm:inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-green-100/80 text-green-800 dark:bg-green-900/30 dark:text-green-400">
          Live System
        </span>
      </div>
      <div className="flex items-center space-x-6">
        <div className="flex items-center space-x-4">
          <button className="text-foreground/40 hover:text-foreground/60 transition-colors relative" title="Notifications">
            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
            </svg>
            <span className="absolute top-0 right-0 block h-2 w-2 rounded-full ring-2 ring-background bg-red-400"></span>
          </button>
        </div>
        
        <div className="h-8 w-px bg-gray-200 hidden sm:block"></div>
        
        <div className="flex items-center space-x-3">
          <ThemeToggle />
          <div className="text-right hidden lg:block ml-2">
            <p className="text-foreground dark:text-gray-100 text-sm font-bold leading-tight">{isAdmin ? "Admin User" : "Staff User"}</p>
            <p className="text-foreground/60 dark:text-foreground/40 text-xs leading-none mt-1">{user?.email || (user?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]) || "Not logged in"}</p>
          </div>
          <div className="w-10 h-10 rounded-full bg-indigo-500 flex items-center justify-center text-white font-bold uppercase shadow-md ring-2 ring-background">
            {user?.email?.substring(0, 2) || "U"}
          </div>
          <form action={logoutAction} className="ml-1 flex items-center">
            <button type="submit" className="text-foreground/40 hover:text-red-500 transition-colors p-1.5 rounded-full hover:bg-red-50" title="Logout">
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1"/></svg>
            </button>
          </form>
        </div>
      </div>
    </header>
  );
}
