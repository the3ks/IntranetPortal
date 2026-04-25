import Link from "next/link";

export default function HRDashboardPage() {
  return (
      <div className="max-w-7xl mx-auto space-y-8">
        <header className="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
          <div>
            <h1 className="text-3xl font-extrabold text-foreground tracking-tight">Human Resources</h1>
            <p className="text-foreground/60 mt-2 text-lg">Manage organizational structure and job mapping.</p>
          </div>
        </header>

        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
          {/* Quick Action Card for Employees */}
          <Link href="/hr/employees" className="block group">
            <div className="bg-card rounded-3xl shadow-sm border border-border/50 p-8 h-full hover:shadow-md hover:border-emerald-500/50 transition-all duration-300 relative overflow-hidden">
              <div className="absolute top-0 right-0 p-6 opacity-10 group-hover:opacity-20 group-hover:scale-110 transition-all duration-300">
                <svg className="w-24 h-24 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 4.354a4 4 0 110 5.292M15 21H3v-1a6 6 0 0112 0v1zm0 0h6v-1a6 6 0 00-9-5.197M13 7a4 4 0 11-8 0 4 4 0 018 0z" /></svg>
              </div>
              <h3 className="text-xl font-bold text-foreground mb-2">Employees & Personnel</h3>
              <p className="text-foreground/60 mb-6 relative z-10">Manage unified employee records, update profiles, and maintain organizational alignment.</p>
              <div className="flex items-center text-emerald-600 font-bold group-hover:translate-x-2 transition-transform">
                <span>View Directory</span>
                <svg className="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" /></svg>
              </div>
            </div>
          </Link>

          {/* Quick Action Card for Job Titles */}
          <Link href="/hr/positions" className="block group">
            <div className="bg-card rounded-3xl shadow-sm border border-border/50 p-8 h-full hover:shadow-md hover:border-pink-500/50 transition-all duration-300 relative overflow-hidden">
              <div className="absolute top-0 right-0 p-6 opacity-10 group-hover:opacity-20 group-hover:scale-110 transition-all duration-300">
                <svg className="w-24 h-24 text-pink-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M21 13.255A23.931 23.931 0 0112 15c-3.183 0-6.22-.62-9-1.745M16 6V4a2 2 0 00-2-2h-4a2 2 0 00-2 2v2m4 6h.01M5 20h14a2 2 0 002-2V8a2 2 0 00-2-2H5a2 2 0 00-2 2v10a2 2 0 002 2z" /></svg>
              </div>
              <h3 className="text-xl font-bold text-foreground mb-2">Job Titles & Org Chart</h3>
              <p className="text-foreground/60 mb-6 relative z-10">Define official company positions, update descriptions, and manage responsibilities.</p>
              <div className="flex items-center text-pink-600 font-bold group-hover:translate-x-2 transition-transform">
                <span>Manage Titles</span>
                <svg className="w-5 h-5 ml-2" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M17 8l4 4m0 0l-4 4m4-4H3" /></svg>
              </div>
            </div>
          </Link>
        </div>
      </div>
  );
}
