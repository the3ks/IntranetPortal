import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import { fetchWithAuth } from "@/lib/api";

export default async function Home() {
  const res = await fetchWithAuth("/api/announcements?limit=2", { cache: 'no-store' });
  const announcements = res.ok ? await res.json() : [];

  return (
    <MainLayout hideSidebar>
      <div className="mb-14 text-center max-w-2xl mx-auto pt-8">
        <h2 className="text-4xl sm:text-5xl font-black text-gray-900 tracking-tight mb-4">Universal Portal</h2>
        <p className="text-gray-500 text-xl font-medium">Select an application below to contextually launch its capabilities.</p>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-10 max-w-4xl mx-auto">
      
        {/* App: The Hub */}
        <Link href="/employees" className="group bg-white p-8 rounded-[2rem] shadow-[0_8px_30px_rgb(0,0,0,0.04)] hover:shadow-[0_8px_30px_rgb(0,0,0,0.12)] border border-gray-100 hover:border-blue-200 transition-all transform hover:-translate-y-1 block relative overflow-hidden">
          <div className="absolute top-0 right-0 w-48 h-48 bg-blue-50/50 rounded-full blur-3xl -mr-16 -mt-16 group-hover:bg-blue-100/70 transition-colors"></div>
          <div className="w-16 h-16 bg-gradient-to-br from-blue-500 to-indigo-600 rounded-2xl flex items-center justify-center text-white shadow-xl shadow-blue-500/20 mb-6 transform group-hover:scale-110 transition-transform duration-300">
             <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"/></svg>
          </div>
          <h3 className="text-2xl font-bold text-gray-900 mb-3 group-hover:text-blue-700 transition-colors">The Hub</h3>
          <p className="text-gray-500 font-medium leading-relaxed">The central employee experience. Browse the corporate directory, internal wikis, documentation, and company announcements.</p>
        </Link>
        
        {/* App: Admin */}
        <Link href="/admin/quick-setup" className="group bg-white p-8 rounded-[2rem] shadow-[0_8px_30px_rgb(0,0,0,0.04)] hover:shadow-[0_8px_30px_rgb(0,0,0,0.12)] border border-gray-100 hover:border-rose-200 transition-all transform hover:-translate-y-1 block relative overflow-hidden">
          <div className="absolute top-0 right-0 w-48 h-48 bg-rose-50/50 rounded-full blur-3xl -mr-16 -mt-16 group-hover:bg-rose-100/70 transition-colors"></div>
          <div className="w-16 h-16 bg-gradient-to-br from-rose-500 to-pink-600 rounded-2xl flex items-center justify-center text-white shadow-xl shadow-rose-500/20 mb-6 transform group-hover:scale-110 transition-transform duration-300">
             <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"/><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/></svg>
          </div>
          <h3 className="text-2xl font-bold text-gray-900 mb-3 group-hover:text-rose-700 transition-colors">Administration</h3>
          <p className="text-gray-500 font-medium leading-relaxed">Manage system configuration, personnel roles, permissions, sites, and geographical footprints.</p>
        </Link>
        
      </div>
      
      {/* Announcements */}
        <div className="max-w-4xl mx-auto mt-16 bg-white rounded-3xl shadow-sm border border-gray-100 p-10">
          <div className="flex items-center justify-between mb-8">
            <h2 className="text-2xl font-bold text-gray-900">Recent Announcements</h2>
            <button className="text-blue-600 hover:text-blue-800 font-bold transition-colors uppercase tracking-wider text-sm flex items-center">
              View All 
              <svg className="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M9 5l7 7-7 7"/></svg>
            </button>
          </div>
          <div className="grid grid-cols-1 gap-6">
            {announcements.length === 0 ? (
              <div className="p-8 rounded-[1.5rem] bg-gray-50/80 border border-transparent text-center">
                <p className="text-gray-500 font-medium">No recent announcements to display.</p>
              </div>
            ) : (
              announcements.map((ann: any) => (
                <Link href="/announcements" key={ann.id} className="p-8 rounded-[1.5rem] bg-gray-50/80 hover:bg-emerald-50/50 hover:border-emerald-100 border border-transparent transition-all group cursor-pointer shadow-sm hover:shadow-md block">
                  <div className="flex justify-between items-start mb-4">
                    <span className="text-xs bg-emerald-100 text-emerald-800 px-3 py-1.5 rounded-full font-bold uppercase tracking-wide">Notice</span>
                    <span className="text-xs font-bold text-gray-400">{new Date(ann.createdAt).toLocaleDateString()}</span>
                  </div>
                  <h3 className="font-extrabold text-gray-900 text-xl group-hover:text-emerald-700 transition-colors mb-2">{ann.title}</h3>
                  <p className="text-gray-500 font-medium leading-relaxed line-clamp-2">{ann.content}</p>
                </Link>
              ))
            )}
          </div>
        </div>
    </MainLayout>
  );
}
