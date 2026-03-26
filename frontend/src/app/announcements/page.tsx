import { fetchWithAuth } from "@/lib/api";
import MainLayout from "@/components/layout/MainLayout";
import { createAnnouncementAction, deleteAnnouncementAction } from "@/app/actions/announcements";
import { cookies } from "next/headers";

export default async function AnnouncementsPage() {
  const cookieStore = await cookies();
  const token = cookieStore.get("auth_token")?.value || "";
  let user: any = null;
  if (token) {
    try {
      user = JSON.parse(Buffer.from(token.split('.')[1], 'base64').toString());
    } catch { }
  }

  const userRoleClaim = user?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || user?.role || user?.Role;
  const isAdmin = userRoleClaim === "Admin" || (Array.isArray(userRoleClaim) && userRoleClaim.includes("Admin"));

  const permissionsClaim = user?.Permission || user?.permission || [];
  const permissions = Array.isArray(permissionsClaim) ? permissionsClaim : [permissionsClaim];

  const canPostAnnouncement = isAdmin || permissions.includes("Announcements.Create");
  const canDeleteAnnouncement = isAdmin || permissions.includes("Announcements.Delete");

  const [annRes, siteRes] = await Promise.all([
    fetchWithAuth("/api/announcements", { cache: 'no-store' }),
    fetchWithAuth("/api/sites", { cache: 'no-store' })
  ]);
  const announcements = annRes.ok ? await annRes.json() : [];
  const sites = siteRes.ok ? await siteRes.json() : [];

  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto py-8 lg:py-12 space-y-10">
        <header className="text-center sm:text-left">
          <h1 className="text-4xl sm:text-5xl font-black text-gray-900 tracking-tight">Corporate Announcements</h1>
          <p className="mt-4 text-gray-500 text-lg sm:text-xl font-medium max-w-2xl">The official source for urgent updates, policy shifts, and company-wide news broadcasts from leadership.</p>
        </header>

        {canPostAnnouncement && (
          <div className="bg-white p-6 sm:p-8 rounded-[2rem] shadow-sm border border-gray-100 flex flex-col gap-6 relative overflow-hidden">
            <div className="absolute top-0 right-0 w-32 h-32 bg-indigo-50/50 rounded-full blur-3xl -mr-10 -mt-10"></div>
            <div className="flex items-center space-x-4 relative z-10">
              <div className="shrink-0 bg-gradient-to-br from-indigo-500 to-purple-600 text-white p-4 rounded-2xl shadow-lg shadow-indigo-500/20">
                <svg className="w-6 h-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M11 5.882V19.24a1.76 1.76 0 01-3.417.592l-2.147-6.15M18 13a3 3 0 100-6M5.436 13.683A4.001 4.001 0 017 6h1.832c4.1 0 7.625-1.234 9.168-3v14c-1.543-1.766-5.067-3-9.168-3H7a3.988 3.988 0 01-1.564-.317z" />
                </svg>
              </div>
              <h2 className="text-xl font-bold text-gray-900">Broadcast Update</h2>
            </div>

            <form action={createAnnouncementAction} className="flex flex-col gap-4 relative z-10">
              <input type="text" name="title" required placeholder="Headline (e.g., Q3 Bonus Structure Updated)" className="px-5 py-4 text-lg font-bold rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none text-gray-900 shadow-sm bg-gray-50 align-top transition-all" />
              <select name="siteId" className="px-5 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none text-gray-700 shadow-sm bg-gray-50 font-medium transition-all appearance-none cursor-pointer">
                <option value="">Global Broadcast (Transmits across all standard Sites natively)</option>
                {sites.map((s: any) => <option key={s.id} value={s.id}>{s.name} - {s.address}</option>)}
              </select>
              <textarea name="content" required rows={4} placeholder="Write your announcement details here... Markdown is supported internally." className="px-5 py-4 rounded-xl border border-gray-200 focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none text-gray-700 shadow-sm bg-gray-50 resize-y transition-all" />
              <div className="flex justify-end pt-2">
                <button type="submit" className="px-8 py-3.5 bg-gray-900 hover:bg-black text-white font-bold rounded-xl shadow-lg transition-all active:scale-95 flex items-center space-x-2">
                  <span>Publish Broadcast</span>
                  <svg className="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M12 19l9 2-9-18-9 18 9-2zm0 0v-8" /></svg>
                </button>
              </div>
            </form>
          </div>
        )}

        {/* Feed */}
        <div className="space-y-6">
          {announcements.length === 0 ? (
            <div className="text-center py-20 bg-white rounded-3xl border border-dashed border-gray-200">
              <svg className="w-16 h-16 mx-auto text-gray-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1.5} d="M19 20H5a2 2 0 01-2-2V6a2 2 0 012-2h10a2 2 0 012 2v1m2 13a2 2 0 01-2-2V7m2 13a2 2 0 002-2V9a2 2 0 00-2-2h-2m-4-3H9M7 16h6M7 8h6v4H7V8z" /></svg>
              <h3 className="text-xl font-bold text-gray-900">No signals found</h3>
              <p className="text-gray-500 mt-2">The company broadcast channel is currently quiet.</p>
            </div>
          ) : (
            announcements.map((ann: any) => (
              <article key={ann.id} className="bg-white p-8 sm:p-10 rounded-[2.5rem] shadow-sm border border-gray-100 hover:shadow-md transition-shadow group relative">
                <div className="flex items-start justify-between gap-4 mb-6">
                  <div>
                    <h2 className="text-2xl sm:text-3xl font-extrabold text-gray-900 leading-tight mb-2 tracking-tight">{ann.title}</h2>
                    <div className="flex flex-wrap items-center gap-3 text-sm font-medium text-gray-500">
                      <span className="flex items-center text-indigo-700 bg-indigo-50 px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider">
                        <svg className="w-3.5 h-3.5 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M16 7a4 4 0 11-8 0 4 4 0 018 0zM12 14a7 7 0 00-7 7h14a7 7 0 00-7-7z" /></svg>
                        {ann.authorName}
                      </span>
                      <span className="hidden sm:inline">•</span>
                      <time dateTime={ann.createdAt} className="flex items-center">
                        <svg className="w-4 h-4 mr-1.5 text-gray-400" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
                        {new Date(ann.createdAt).toLocaleDateString(undefined, { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}
                      </time>
                    </div>
                  </div>
                  {canDeleteAnnouncement && (
                    <form action={deleteAnnouncementAction.bind(null, ann.id)}>
                      <button type="submit" className="text-gray-300 hover:text-rose-500 p-2 rounded-xl hover:bg-rose-50 transition-colors shrink-0" title="Delete Broadcast">
                        <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                      </button>
                    </form>
                  )}
                </div>
                <div className="prose prose-lg max-w-none text-gray-600 leading-relaxed whitespace-pre-wrap font-medium">
                  {ann.content}
                </div>
              </article>
            ))
          )}
        </div>
      </div>
    </MainLayout>
  );
}
