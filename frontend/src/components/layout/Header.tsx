export default function Header() {
  return (
    <header className="bg-white/80 backdrop-blur-md shadow-sm h-20 flex items-center justify-between px-8 sticky top-0 z-20 transition-all border-b border-gray-100">
      <div className="flex items-center space-x-4">
        <h1 className="text-2xl font-bold text-gray-800 tracking-tight">Dashboard Overview</h1>
        <span className="hidden sm:inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-green-100 text-green-800">
          Live System
        </span>
      </div>
      <div className="flex items-center space-x-5">
        <button className="text-gray-400 hover:text-gray-500 transition-colors relative">
          <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 17h5l-1.405-1.405A2.032 2.032 0 0118 14.158V11a6.002 6.002 0 00-4-5.659V5a2 2 0 10-4 0v.341C7.67 6.165 6 8.388 6 11v3.159c0 .538-.214 1.055-.595 1.436L4 17h5m6 0v1a3 3 0 11-6 0v-1m6 0H9" />
          </svg>
          <span className="absolute top-0 right-0 block h-2 w-2 rounded-full ring-2 ring-white bg-red-400"></span>
        </button>
      </div>
    </header>
  );
}
