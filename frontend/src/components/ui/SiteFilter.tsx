"use client";

import { useRouter } from "next/navigation";

export default function SiteFilter({ 
  sites, 
  currentSiteId, 
  disabled 
}: { 
  sites: {id: number, name: string}[], 
  currentSiteId?: string,
  disabled: boolean 
}) {
  const router = useRouter();
  
  return (
    <div className="flex items-center gap-3">
      <label className="text-sm font-bold text-gray-700 whitespace-nowrap">Location Filter:</label>
      <select 
        disabled={disabled}
        value={currentSiteId || ""}
        onChange={(e) => {
          const val = e.target.value;
          if (val) {
            router.push(`?siteId=${val}`);
          } else {
            router.push(`?`);
          }
        }}
        className={`px-4 py-2.5 rounded-xl border border-gray-200 outline-none text-sm font-semibold transition-all shadow-sm ${disabled ? 'bg-gray-100 text-gray-400 cursor-not-allowed opacity-70' : 'bg-white text-gray-900 border-blue-200 hover:border-blue-400 focus:ring-4 focus:ring-blue-500/20'} min-w-[240px] appearance-none`}
        style={{ backgroundImage: `url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' fill='none' viewBox='0 0 20 20'%3e%3cpath stroke='%236b7280' stroke-linecap='round' stroke-linejoin='round' stroke-width='1.5' d='M6 8l4 4 4-4'/%3e%3c/svg%3e")`, backgroundPosition: `right 0.5rem center`, backgroundRepeat: `no-repeat`, backgroundSize: `1.5em 1.5em`, paddingRight: `2.5rem` }}
      >
        <option value="">All Authorized Sites</option>
        {sites.map(s => (
          <option key={s.id} value={s.id}>{s.name}</option>
        ))}
      </select>
      {disabled && (
        <span className="text-xs font-bold text-rose-500 uppercase tracking-wider bg-rose-50 px-2 py-1 rounded-md">Locked (Single-Site)</span>
      )}
    </div>
  );
}
