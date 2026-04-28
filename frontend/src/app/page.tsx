import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";
import { fetchWithAuth } from "@/lib/api";
import AttendanceWidget from "@/components/hr/AttendanceWidget";

export default async function Home() {
  const res = await fetchWithAuth("/api/announcements?limit=2", { cache: 'no-store' });
  const announcements = res.ok ? await res.json() : [];

  const modulesRes = await fetchWithAuth("/api/modules", { cache: 'no-store' });
  const modules = modulesRes.ok ? await modulesRes.json() : [];

  const colors = [
    { from: "from-blue-500", to: "to-indigo-600", text: "text-blue-500", shadow: "shadow-blue-500/20", border: "hover:border-blue-500/30", bg: "bg-blue-500/10", hoverBg: "group-hover:bg-blue-500/20" },
    { from: "from-emerald-500", to: "to-teal-600", text: "text-emerald-500", shadow: "shadow-emerald-500/20", border: "hover:border-emerald-500/30", bg: "bg-emerald-500/10", hoverBg: "group-hover:bg-emerald-500/20" },
    { from: "from-rose-500", to: "to-pink-600", text: "text-rose-500", shadow: "shadow-rose-500/20", border: "hover:border-rose-500/30", bg: "bg-rose-500/10", hoverBg: "group-hover:bg-rose-500/20" },
    { from: "from-amber-500", to: "to-orange-600", text: "text-amber-500", shadow: "shadow-amber-500/20", border: "hover:border-amber-500/30", bg: "bg-amber-500/10", hoverBg: "group-hover:bg-amber-500/20" },
    { from: "from-fuchsia-500", to: "to-purple-600", text: "text-fuchsia-500", shadow: "shadow-fuchsia-500/20", border: "hover:border-fuchsia-500/30", bg: "bg-fuchsia-500/10", hoverBg: "group-hover:bg-fuchsia-500/20" }
  ];

  return (
    <MainLayout hideSidebar>
      <div className="mb-14 text-center max-w-2xl mx-auto pt-8">
        <h2 className="text-4xl sm:text-5xl font-black text-foreground tracking-tight mb-4">Universal Portal</h2>
        <p className="text-foreground/70 text-xl font-medium">Select an application below to contextually launch its capabilities.</p>
      </div>

      <AttendanceWidget />

      <div className="grid grid-cols-2 md:grid-cols-4 lg:grid-cols-6 gap-4 w-full px-4">
        {modules.map((mod: any, index: number) => {
          const color = colors[index % colors.length];
          const isExternal = mod.url.startsWith("http");
          const LinkTag = isExternal ? "a" : Link;
          
          return (
            <LinkTag key={mod.id} href={mod.url} className={`group bg-card p-4 rounded-xl shadow-[0_2px_8px_rgb(0,0,0,0.04)] hover:shadow-[0_4px_12px_rgb(0,0,0,0.12)] border border-border/50 ${color.border} transition-all transform hover:-translate-y-1 block relative overflow-hidden`}>
              <div className={`absolute top-0 right-0 w-32 h-32 ${color.bg} rounded-full blur-2xl -mr-8 -mt-8 ${color.hoverBg} transition-colors`}></div>
              <div 
                className={`w-12 h-12 bg-gradient-to-br ${color.from} ${color.to} rounded-lg flex items-center justify-center text-white shadow-lg ${color.shadow} mb-3 transform group-hover:scale-110 transition-transform duration-300 cursor-help`} 
                dangerouslySetInnerHTML={{ __html: mod.iconSvg }}
                title={mod.description}
              />
              <h3 className={`text-sm font-bold text-foreground group-hover:${color.text} transition-colors line-clamp-2`}>{mod.name}</h3>
            </LinkTag>
          );
        })}
      </div>

      {/* Announcements */}
      <div className="max-w-4xl mx-auto mt-16 bg-card rounded-3xl shadow-sm border border-border/50 p-10">
        <div className="flex items-center justify-between mb-8">
          <h2 className="text-2xl font-bold text-foreground">Recent Announcements</h2>
          <button className="text-blue-500 hover:text-blue-400 font-bold transition-colors uppercase tracking-wider text-sm flex items-center">
            View All
            <svg className="w-4 h-4 ml-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M9 5l7 7-7 7" /></svg>
          </button>
        </div>
        <div className="grid grid-cols-1 gap-6">
          {announcements.length === 0 ? (
            <div className="p-8 rounded-[1.5rem] bg-background/50 border border-transparent text-center">
              <p className="text-foreground/60 font-medium">No recent announcements to display.</p>
            </div>
          ) : (
            announcements.map((ann: any) => (
              <Link href="/announcements" key={ann.id} className="p-8 rounded-[1.5rem] bg-background/50 hover:bg-emerald-500/5 hover:border-emerald-500/20 border border-transparent transition-all group cursor-pointer shadow-sm hover:shadow-md block">
                <div className="flex justify-between items-start mb-4">
                  <span className="text-xs bg-emerald-500/20 text-emerald-600 dark:text-emerald-400 px-3 py-1.5 rounded-full font-bold uppercase tracking-wide">Notice</span>
                  <span className="text-xs font-bold text-foreground/40">{new Date(ann.createdAt).toLocaleDateString()}</span>
                </div>
                <h3 className="font-extrabold text-foreground text-xl group-hover:text-emerald-500 transition-colors mb-2">{ann.title}</h3>
                <p className="text-foreground/70 font-medium leading-relaxed line-clamp-2">{ann.content}</p>
              </Link>
            ))
          )}
        </div>
      </div>
    </MainLayout>
  );
}
