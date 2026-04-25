"use client";
import Link from "next/link";
import { useState, useEffect } from "react";
import { siteConfig } from "@/config/site";
import Image from "next/image";
import { usePathname } from "next/navigation";


export default function Sidebar({ user }: { user?: any }) {
  const [isOpen, setIsOpen] = useState(false);
  const pathname = usePathname() || "";
  const [dynamicModules, setDynamicModules] = useState<any[]>([]);
  const [isCategoryManager, setIsCategoryManager] = useState(false);
  const [isApprover, setIsApprover] = useState(false);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    fetch(`${backendUrl}/api/modules`, { credentials: 'include' })
      .then(r => {
        if (r.status === 401 && (r.headers.get("WWW-Authenticate") || "").includes("LOCKED_OUT")) window.location.href = "/login?reason=locked";
        return r.ok ? r.json() : [];
      })
      .then(data => {
        // Filter out the Monolith's standard static hardcoded directories
        const filtered = data.filter((m: any) => !["The Hub", "Assets Management", "Administration", "Human Resources"].includes(m.name));
        setDynamicModules(filtered);
      })
      .catch(() => { });

    fetch(`${backendUrl}/api/assetrequests/is-manager`, { credentials: 'include' })
      .then(r => {
        if (r.status === 401 && (r.headers.get("WWW-Authenticate") || "").includes("LOCKED_OUT")) window.location.href = "/login?reason=locked";
        return r.ok ? r.json() : false;
      })
      .then(data => setIsCategoryManager(data))
      .catch(() => { });

    fetch(`${backendUrl}/api/assetrequests/is-approver`, { credentials: 'include' })
      .then(r => {
        if (r.status === 401 && (r.headers.get("WWW-Authenticate") || "").includes("LOCKED_OUT")) window.location.href = "/login?reason=locked";
        return r.ok ? r.json() : false;
      })
      .then(data => setIsApprover(data))
      .catch(() => { });
  }, []);

  // Safely map the role variable regardless of string vs array Claim permutation from .NET Core
  const userRoleClaim = user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || user?.role || user?.Role;
  const isAdmin = userRoleClaim === "Admin" || (Array.isArray(userRoleClaim) && userRoleClaim.includes("Admin"));

  const permissionsClaim = user?.Permission || user?.permission || [];
  const permissions = Array.isArray(permissionsClaim) ? permissionsClaim : [permissionsClaim];
  const canManageAssets = isAdmin || permissions.includes("Perm:Assets.Manage");
  const canManageDictionaries = isAdmin || permissions.includes("Perm:Assets.Dictionaries.Manage") || canManageAssets;
  const showInventoryMgmt = canManageAssets || isCategoryManager;
  const showApprovals = isAdmin || isApprover;

  let activeModule = "home";
  if (pathname.startsWith("/admin")) activeModule = "admin";
  else if (pathname.startsWith("/assets")) activeModule = "assets";
  else if (pathname.startsWith("/hr")) activeModule = "hr";
  else if (pathname.startsWith("/docs") || pathname.startsWith("/employees") || pathname.startsWith("/departments") || pathname.startsWith("/sites") || pathname.startsWith("/announcements")) activeModule = "hub";

  const isLinkActive = (href: string) => pathname === href || pathname.startsWith(`${href}/`);

  let sidebarTitle = siteConfig.name;
  if (activeModule === "hub") sidebarTitle = "The Hub";
  if (activeModule === "admin") sidebarTitle = "Administration";
  if (activeModule === "assets") sidebarTitle = "Assets Center";
  if (activeModule === "hr") sidebarTitle = "Human Resources";

  return (
    <>
      {/* Mobile Toggle Button */}
      <button
        className="md:hidden fixed bottom-6 right-6 z-50 bg-blue-600 hover:bg-blue-700 text-white p-4 rounded-full shadow-2xl transition-all"
        onClick={() => setIsOpen(!isOpen)}
        aria-label="Toggle Menu"
      >
        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d={isOpen ? "M6 18L18 6M6 6l12 12" : "M4 6h16M4 12h16M4 18h16"} />
        </svg>
      </button>

      {/* Sidebar background overlay for mobile */}
      {isOpen && (
        <div
          className="fixed inset-0 bg-gray-900/60 backdrop-blur-sm z-30 md:hidden transition-opacity"
          onClick={() => setIsOpen(false)}
        />
      )}

      {/* Sidebar Content */}
      <aside className={`fixed inset-y-0 left-0 bg-gray-900 text-gray-300 w-72 p-6 shadow-2xl transform transition-transform duration-300 ease-in-out z-40 ${isOpen ? "translate-x-0" : "-translate-x-full"} md:translate-x-0 flex flex-col`}>
        <div className="flex items-center justify-between mb-10 mt-2">
          <div className="flex items-center space-x-3 overflow-hidden">
            <div className="w-10 h-10 rounded-xl shadow-lg flex items-center justify-center overflow-hidden border border-gray-700/50 bg-gray-800 shrink-0">
              {activeModule === "hub" ? (
                <svg className="w-6 h-6 text-blue-400 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" /></svg>
              ) : activeModule === "assets" ? (
                <svg className="w-6 h-6 text-emerald-400 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9.75 17L9 20l-1 1h8l-1-1-.75-3M3 13h18M5 17h14a2 2 0 002-2V5a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
              ) : activeModule === "admin" ? (
                <svg className="w-6 h-6 text-amber-500 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
              ) : activeModule === "hr" ? (
                <svg className="w-6 h-6 text-pink-400 group-hover:scale-110 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
              ) : (
                <Image src="/icon-512x512.png" alt="App Logo" width={40} height={40} className="object-cover group-hover:scale-105 transition-transform" priority />
              )}
            </div>
            <span className="text-xl sm:text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-blue-400 to-indigo-300 truncate">{sidebarTitle}</span>
          </div>

          {(activeModule === "assets" || activeModule === "hub") && (
            <Link href={activeModule === "assets" ? "/assets/wiki" : "/docs"} className="text-gray-500 hover:text-white bg-gray-800 hover:bg-gray-700 p-1.5 rounded-lg transition-colors ml-2 shrink-0" title="Module Documentation">
              <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            </Link>
          )}
        </div>

        <nav className="flex flex-col space-y-2 flex-1 overflow-y-auto overflow-x-hidden elegant-scrollbar pb-4 pr-1">
          {/* Context: The Hub */}
          {activeModule === "hub" && (
            <>
              {/* Directory Content */}
              <div className="pt-2 pb-2">
                <p className="px-4 text-[11px] font-bold text-blue-400 uppercase tracking-widest opacity-80">Corporate Directory</p>
              </div>
              <Link href="/employees" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/employees') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-blue-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 20h5v-2a3 3 0 00-5.356-1.857M17 20H7m10 0v-2c0-.656-.126-1.283-.356-1.857M7 20H2v-2a3 3 0 015.356-1.857M7 20v-2c0-.656.126-1.283.356-1.857m0 0a5.002 5.002 0 019.288 0M15 7a3 3 0 11-6 0 3 3 0 016 0zm6 3a2 2 0 11-4 0 2 2 0 014 0zM7 10a2 2 0 11-4 0 2 2 0 014 0z" /></svg>
                <span>Employees</span>
              </Link>
              <Link href="/departments" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/departments') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" /></svg>
                <span>Departments</span>
              </Link>
              <Link href="/sites" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/sites') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-teal-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                <span>Geographic Sites</span>
              </Link>

              {/* Knowledge Base Content */}
              <div className="pt-6 pb-2">
                <p className="px-4 text-[11px] font-bold text-emerald-400 uppercase tracking-widest opacity-80">Knowledge Base</p>
              </div>
              <Link href="/docs" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/docs') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477-4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" /></svg>
                <span>Wiki & Guides</span>
              </Link>
              <Link href="/announcements" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/announcements') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-cyan-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5.882V19.24a1.76 1.76 0 01-3.417.592l-2.147-6.15M18 13a3 3 0 100-6M5.436 13.683A4.001 4.001 0 017 6h1.832c4.1 0 7.625-1.234 9.168-3v14c-1.543-1.766-5.067-3-9.168-3H7a3.988 3.988 0 01-1.564-.317z" /></svg>
                <span>Announcements</span>
              </Link>

              {/* Microservices external bridge */}
              {dynamicModules.length > 0 && (
                <>
                  <div className="pt-6 pb-2">
                    <p className="px-4 text-[11px] font-bold text-fuchsia-400 uppercase tracking-widest opacity-80">Employee Services</p>
                  </div>
                  {dynamicModules.map(mod => {
                    const isExternal = mod.url.startsWith("http");
                    const LinkTag = isExternal ? "a" : Link;
                    return (
                      <LinkTag key={mod.id} href={mod.url} className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3`}>
                        <div dangerouslySetInnerHTML={{ __html: mod.iconSvg }} className="flex-shrink-0 text-fuchsia-400 [&>svg]:w-5 [&>svg]:h-5" />
                        <span>{mod.name}</span>
                        {isExternal && <svg className="w-4 h-4 ml-auto opacity-50 flex-shrink-0" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 6H6a2 2 0 00-2 2v10a2 2 0 002 2h10a2 2 0 002-2v-4M14 4h6m0 0v6m0-6L10 14" /></svg>}
                      </LinkTag>
                    );
                  })}
                </>
              )}
            </>
          )}

          {/* Context: Admin */}
          {activeModule === "admin" && isAdmin && (
            <>
              <div className="pt-2 pb-2">
                <p className="px-4 text-[11px] font-bold text-emerald-400 uppercase tracking-widest opacity-80">System Monitoring</p>
              </div>
              <Link href="/admin/audit-logs" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/audit-logs') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-6 9l2 2 4-4" /></svg>
                <span>Login Monitoring</span>
              </Link>

              <div className="pt-6 pb-2">
                <p className="px-4 text-[11px] font-bold text-rose-400 uppercase tracking-widest opacity-80">Administration</p>
              </div>
              <Link href="/admin/quick-setup" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/quick-setup') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-amber-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M13 10V3L4 14h7v7l9-11h-7z" /></svg>
                <span>Rapid Config</span>
              </Link>

              <Link href="/admin/users" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/users') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-sky-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
                <span>Access Control</span>
              </Link>


              <Link href="/admin/roles" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/roles') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-purple-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m5.618-4.016A11.955 11.955 0 0112 2.944a11.955 11.955 0 01-8.618 3.04A12.02 12.02 0 003 9c0 5.591 3.824 10.29 9 11.622 5.176-1.332 9-6.03 9-11.622 0-1.042-.133-2.052-.382-3.016z" /></svg>
                <span>Security Roles</span>
              </Link>
              <Link href="/admin/permissions" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/permissions') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-rose-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 11c0 3.517-1.009 6.799-2.753 9.571m-3.44-2.04l.054-.09A13.916 13.916 0 008 11a4 4 0 118 0c0 1.017-.07 2.019-.203 3m-2.118 6.844A21.88 21.88 0 0015.171 17m3.839 1.132c.645-2.266.99-4.659.99-7.132A8 8 0 008 4.07M3 15.364c.64-1.319 1-2.8 1-4.364 0-1.457.39-2.823 1.07-4" /></svg>
                <span>Permission Registry</span>
              </Link>
              <Link href="/admin/sites" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/sites') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-teal-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z" /><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 11a3 3 0 11-6 0 3 3 0 016 0z" /></svg>
                <span>Geographic Sites</span>
              </Link>
              <Link href="/admin/modules" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/admin/modules') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-fuchsia-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2V6zM14 6a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2V6zM4 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2H6a2 2 0 01-2-2v-2zM14 16a2 2 0 012-2h2a2 2 0 012 2v2a2 2 0 01-2 2h-2a2 2 0 01-2-2v-2z" /></svg>
                <span>System Modules</span>
              </Link>
            </>
          )}

          {/* Context: Assets */}
          {activeModule === "assets" && (
            <>
              <div className="pt-2 pb-2">
                <p className="px-4 text-[11px] font-bold text-emerald-400 uppercase tracking-widest opacity-80">Hardware & Equipment</p>
              </div>
              <Link href="/assets" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${pathname === '/assets' ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-emerald-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20 7l-8-4-8 4m16 0l-8 4m8-4v10l-8 4m0-10L4 7m8 4v10M4 7v10l8 4" /></svg>
                <span>My Assets</span>
              </Link>

              {showApprovals && (
                <Link href="/assets/approvals" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/assets/approvals') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                  <svg className="w-5 h-5 text-amber-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                  <span>Pending Approvals</span>
                </Link>
              )}

              {(showInventoryMgmt || canManageDictionaries) && (
                <>
                  <div className="pt-6 pb-2">
                    <p className="px-4 text-[11px] font-bold text-rose-400 uppercase tracking-widest opacity-80">Administration</p>
                  </div>

                  {showInventoryMgmt && (
                    <>
                      <Link href="/assets/fulfillment" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/assets/fulfillment') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                        <svg className="w-5 h-5 text-rose-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" /></svg>
                        <span>Fulfillment Queue</span>
                      </Link>

                      <Link href="/assets/inventory" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/assets/inventory') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                        <svg className="w-5 h-5 text-indigo-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 7v10c0 2.21 3.582 4 8 4s8-1.79 8-4V7M4 7c0 2.21 3.582 4 8 4s8-1.79 8-4M4 7c0-2.21 3.582-4 8-4s8 1.79 8 4m0 5c0 2.21-3.582 4-8 4s-8-1.79-8-4" /></svg>
                        <span>Asset Inventory</span>
                      </Link>

                      <Link href="/assets/accessories" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/assets/accessories') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                        <svg className="w-5 h-5 text-fuchsia-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 11H5m14 0a2 2 0 012 2v6a2 2 0 01-2 2H5a2 2 0 01-2-2v-6a2 2 0 012-2m14 0V9a2 2 0 00-2-2M5 11V9a2 2 0 002-2m0 0V5a2 2 0 012-2h6a2 2 0 012 2v2M7 7h10" /></svg>
                        <span>Bulk Inventory Stockpile</span>
                      </Link>
                    </>
                  )}

                  {canManageDictionaries && (
                    <Link href="/assets/dictionaries" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${isLinkActive('/assets/dictionaries') ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                      <svg className="w-5 h-5 text-sky-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 9l3 3-3 3m5 0h3M5 20h14a2 2 0 002-2V6a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                      <span>System Dictionaries</span>
                    </Link>
                  )}
                </>
              )}


            </>
          )}

          {/* Context: HR */}
          {activeModule === "hr" && (
            <>
              <div className="pt-2 pb-2">
                <p className="px-4 text-[11px] font-bold text-pink-400 uppercase tracking-widest opacity-80">Human Resources</p>
              </div>
              <Link href="/hr" className={`hover:bg-gray-800 hover:text-white px-4 py-3 rounded-xl transition-all font-medium flex items-center space-x-3 ${pathname === '/hr' ? 'bg-gray-800 text-white shadow-sm' : ''}`}>
                <svg className="w-5 h-5 text-pink-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M3 12l2-2m0 0l7-7 7 7M5 10v10a1 1 0 001 1h3m10-11l2 2m-2-2v10a1 1 0 01-1 1h-3m-6 0a1 1 0 001-1v-4a1 1 0 011-1h2a1 1 0 011 1v4a1 1 0 001 1m-6 0h6" /></svg>
                <span>HR Dashboard</span>
              </Link>
              
              <div className="pt-6 pb-2">
                <p className="px-4 text-[11px] font-bold text-gray-400 uppercase tracking-widest opacity-80">Organization</p>
              </div>
              <Link href="/hr/employees" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/employees") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
              <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
              Employees
            </Link>
            <Link href="/hr/departments" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/departments") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
              <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4" /></svg>
              Departments
            </Link>
            <Link href="/hr/positions" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/positions") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
              <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
              Job Titles
            </Link>
            <div className="pt-4 mt-2 border-t border-white/10">
              <p className="px-4 text-xs font-bold text-emerald-200/50 uppercase tracking-wider mb-2">Operations</p>
              <Link href="/hr/leave" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/leave") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
                <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                Time Off & Leave
              </Link>
              <Link href="/hr/attendance" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/attendance") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
                <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                Attendance History
              </Link>
              <Link href="/hr/onboarding" className={`flex items-center px-4 py-3 rounded-xl font-bold transition-all ${pathname.startsWith("/hr/onboarding") ? "bg-white text-emerald-600 shadow-md shadow-emerald-500/10" : "text-emerald-100 hover:bg-emerald-500/50 hover:text-white"}`}>
                <svg className="w-5 h-5 mr-3" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
                Onboarding
              </Link>
            </div>
            </>
          )}

        </nav>

      </aside>
    </>
  );
}
