import MainLayout from "@/components/layout/MainLayout";

export default function Home() {
  return (
    <MainLayout>
      <div className="mb-8 p-1">
        <h2 className="text-3xl font-extrabold text-gray-900 tracking-tight">Good morning, Admin.</h2>
        <p className="text-gray-500 mt-2 text-lg">Here is what's happening across the organization today.</p>
      </div>

      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6">
        {/* Metric Cards */}
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 relative overflow-hidden group hover:shadow-md transition-all">
          <div className="absolute top-0 right-0 p-4 opacity-10 filter blur-[1px] transform group-hover:scale-110 transition-transform duration-300">
            <svg className="w-16 h-16 text-blue-500" fill="currentColor" viewBox="0 0 24 24"><path d="M12 12c2.21 0 4-1.79 4-4s-1.79-4-4-4-4 1.79-4 4 1.79 4 4 4zm0 2c-2.67 0-8 1.34-8 4v2h16v-2c0-2.66-5.33-4-8-4z"/></svg>
          </div>
          <h3 className="text-gray-500 text-sm font-semibold uppercase tracking-wider relative z-10">Total Employees</h3>
          <p className="text-4xl font-black text-gray-900 mt-3 relative z-10">124</p>
          <div className="mt-2 text-sm text-green-600 font-medium relative z-10 flex items-center">
            <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 10l7-7m0 0l7 7m-7-7v18"/></svg>
            +12% this month
          </div>
        </div>

        <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 relative overflow-hidden group hover:shadow-md transition-all">
          <div className="absolute top-0 right-0 p-4 opacity-10 filter blur-[1px] transform group-hover:scale-110 transition-transform duration-300">
            <svg className="w-16 h-16 text-indigo-500" fill="currentColor" viewBox="0 0 24 24"><path d="M19 21V5a2 2 0 00-2-2H7a2 2 0 00-2 2v16m14 0h2m-2 0h-5m-9 0H3m2 0h5M9 7h1m-1 4h1m4-4h1m-1 4h1m-5 10v-5a1 1 0 011-1h2a1 1 0 011 1v5m-4 0h4"/></svg>
          </div>
          <h3 className="text-gray-500 text-sm font-semibold uppercase tracking-wider relative z-10">Active Departments</h3>
          <p className="text-4xl font-black text-gray-900 mt-3 relative z-10">8</p>
          <div className="mt-2 text-sm text-gray-400 font-medium relative z-10">
            Stable
          </div>
        </div>

        <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 relative overflow-hidden group hover:shadow-md transition-all">
          <div className="absolute top-0 right-0 p-4 opacity-10 filter blur-[1px] transform group-hover:scale-110 transition-transform duration-300">
            <svg className="w-16 h-16 text-purple-500" fill="currentColor" viewBox="0 0 24 24"><path d="M17.657 16.657L13.414 20.9a1.998 1.998 0 01-2.827 0l-4.244-4.243a8 8 0 1111.314 0z"/><path d="M15 11a3 3 0 11-6 0 3 3 0 016 0z"/></svg>
          </div>
          <h3 className="text-gray-500 text-sm font-semibold uppercase tracking-wider relative z-10">Sites</h3>
          <p className="text-4xl font-black text-gray-900 mt-3 relative z-10">4</p>
          <div className="mt-2 text-sm text-blue-600 font-medium relative z-10">
            Across 2 regions
          </div>
        </div>

        <div className="bg-gradient-to-br from-blue-600 to-indigo-700 p-6 rounded-2xl shadow-lg relative overflow-hidden group hover:shadow-xl transition-all text-white">
          <div className="absolute top-0 right-0 p-4 opacity-20 transform group-hover:-rotate-6 scale-110 transition-transform duration-300">
             <svg className="w-20 h-20 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5.882V19.24a1.76 1.76 0 01-3.417.592l-2.147-6.15M18 13a3 3 0 100-6M5.436 13.683A4.001 4.001 0 017 6h1.832c4.1 0 7.625-1.234 9.168-3v14c-1.543-1.766-5.067-3-9.168-3H7a3.988 3.988 0 01-1.564-.317z"/></svg>
          </div>
          <h3 className="text-blue-100 text-sm font-semibold uppercase tracking-wider relative z-10">New Announcements</h3>
          <p className="text-4xl font-black text-white mt-3 relative z-10">3</p>
          <div className="mt-2 text-sm text-blue-100 font-medium relative z-10 flex items-center">
            <span className="w-2 h-2 rounded-full bg-red-400 mr-2 animate-pulse"></span>
            Requires action
          </div>
        </div>
      </div>

      {/* Main Content Area */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8 mt-8">
        <div className="lg:col-span-2 bg-white rounded-2xl shadow-sm border border-gray-100 p-8">
          <div className="flex items-center justify-between mb-6">
            <h2 className="text-xl font-bold text-gray-900">Latest Announcements</h2>
            <button className="text-blue-600 hover:text-blue-800 text-sm font-semibold transition-colors">View all →</button>
          </div>
          
          <div className="space-y-4">
            <div className="group border border-gray-100 rounded-xl p-5 hover:border-blue-200 hover:bg-blue-50/50 transition-all cursor-pointer shadow-sm relative overflow-hidden">
              <div className="absolute left-0 top-0 bottom-0 w-1 bg-blue-500 opacity-0 group-hover:opacity-100 transition-opacity"></div>
              <div className="flex justify-between items-start mb-2">
                <h3 className="font-semibold text-gray-900 text-lg group-hover:text-blue-700 transition-colors">Q3 Company Retreat</h3>
                <span className="text-xs bg-blue-100 text-blue-800 px-2.5 py-1 rounded-full font-bold">Event</span>
              </div>
              <p className="text-gray-500 mb-3 line-clamp-2">Join us for the annual company retreat! We have planned an incredible weekend of team building, workshops, and relaxation. Please RSVP by Friday so we can finalize the booking with the venue.</p>
              <div className="flex items-center text-xs text-gray-400 font-medium font-mono">
                <span className="flex items-center"><div className="w-5 h-5 rounded-full bg-gray-200 text-gray-600 flex items-center justify-center mr-2 text-[10px] font-bold">HR</div> By HR Department</span>
                <span className="mx-2">•</span>
                <span>2 hours ago</span>
              </div>
            </div>

            <div className="group border border-gray-100 rounded-xl p-5 hover:border-green-200 hover:bg-green-50/50 transition-all cursor-pointer shadow-sm relative overflow-hidden">
              <div className="absolute left-0 top-0 bottom-0 w-1 bg-green-500 opacity-0 group-hover:opacity-100 transition-opacity"></div>
              <div className="flex justify-between items-start mb-2">
                <h3 className="font-semibold text-gray-900 text-lg group-hover:text-green-700 transition-colors">Welcome to the New Intranet</h3>
                <span className="text-xs bg-green-100 text-green-800 px-2.5 py-1 rounded-full font-bold">Update</span>
              </div>
              <p className="text-gray-500 mb-3 line-clamp-2">We are excited to launch our new mobile-friendly portal! Built from the ground up to be lightning fast, installable as a mobile app, and seamlessly integrated with our backend systems.</p>
              <div className="flex items-center text-xs text-gray-400 font-medium font-mono">
                <span className="flex items-center"><div className="w-5 h-5 rounded-full bg-gray-200 text-gray-600 flex items-center justify-center mr-2 text-[10px] font-bold">IT</div> By IT Department</span>
                <span className="mx-2">•</span>
                <span>1 day ago</span>
              </div>
            </div>
          </div>
        </div>

        {/* Quick Actions / Recent Activity */}
        <div className="bg-white rounded-2xl shadow-sm border border-gray-100 p-8">
          <h2 className="text-xl font-bold text-gray-900 mb-6">Recent Hires</h2>
          <div className="space-y-6">
            {[1, 2, 3, 4].map((i) => (
              <div key={i} className="flex items-center space-x-4">
                <div className="w-12 h-12 rounded-full bg-gradient-to-tr from-blue-100 to-indigo-50 border-2 border-white shadow-md flex items-center justify-center text-blue-700 font-bold text-lg">
                  {['JD', 'SM', 'KL', 'RJ'][i-1]}
                </div>
                <div>
                  <h4 className="text-sm font-bold text-gray-900 hover:text-blue-600 cursor-pointer transition-colors">{['Jane Doe', 'Sam Mitchell', 'Kendra Lin', 'Robert Jones'][i-1]}</h4>
                  <p className="text-xs text-gray-500 font-medium mt-0.5">{['Software Engineer', 'Marketing Manager', 'HR Specialist', 'Warehouse Staff'][i-1]}</p>
                </div>
              </div>
            ))}
          </div>
          <button className="w-full mt-8 bg-gray-50 hover:bg-gray-100 hover:border-gray-300 text-gray-700 py-3 rounded-xl font-semibold transition-all border border-gray-200 shadow-sm flex items-center justify-center group">
            <span>View Full Directory</span>
            <svg className="w-4 h-4 ml-2 group-hover:translate-x-1 transition-transform" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M14 5l7 7m0 0l-7 7m7-7H3"/></svg>
          </button>
        </div>
      </div>
    </MainLayout>
  );
}
