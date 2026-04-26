"use client";
import { useState, useEffect } from "react";
import Link from "next/link";

function formatTime(iso: string | null) {
  if (!iso) return "—";
  return new Date(iso).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
}

function formatDuration(clockIn: string | null, clockOut: string | null) {
  if (!clockIn || !clockOut) return "—";
  const diff = new Date(clockOut).getTime() - new Date(clockIn).getTime();
  const h = Math.floor(diff / 3600000);
  const m = Math.floor((diff % 3600000) / 60000);
  return `${h}h ${m}m`;
}

export default function AttendanceHistoryClient() {
  const [logs, setLogs] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    fetch(`${backendUrl}/api/hr/attendance/history?days=30`, { credentials: "include" })
      .then(r => r.ok ? r.json() : [])
      .then(data => { setLogs(data); setLoading(false); });
  }, []);

  return (
    <div className="max-w-7xl mx-auto py-8 space-y-8">
      <div className="flex items-center gap-4">
        <Link href="/" className="p-2 rounded-xl hover:bg-background border border-border/50 text-foreground/50 hover:text-foreground transition-colors">
          <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 19l-7-7 7-7" /></svg>
        </Link>
        <div>
          <h1 className="text-3xl font-black text-foreground">Attendance History</h1>
          <p className="text-foreground/70">Your clock-in/out records for the past 30 days.</p>
        </div>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <div className="w-8 h-8 border-4 border-border/50 border-t-emerald-500 rounded-full animate-spin" />
        </div>
      ) : (
        <div className="bg-card border border-border/50 rounded-3xl overflow-hidden shadow-sm">
          <table className="w-full text-left">
            <thead>
              <tr className="bg-background/50 border-b border-border/50">
                <th className="p-4 pl-6 text-xs font-bold uppercase tracking-wider text-foreground/50">Date</th>
                <th className="p-4 text-xs font-bold uppercase tracking-wider text-foreground/50">Clock In</th>
                <th className="p-4 text-xs font-bold uppercase tracking-wider text-foreground/50">Clock Out</th>
                <th className="p-4 pr-6 text-xs font-bold uppercase tracking-wider text-foreground/50 text-right">Duration</th>
              </tr>
            </thead>
            <tbody>
              {logs.length === 0 ? (
                <tr><td colSpan={4} className="p-10 text-center text-foreground/50 font-medium">No attendance records found.</td></tr>
              ) : logs.map((log: any) => {
                const isToday = new Date(log.date).toDateString() === new Date().toDateString();
                return (
                  <tr key={log.id} className={`border-b border-border/30 last:border-0 transition-colors ${isToday ? "bg-emerald-500/5" : "hover:bg-background/40"}`}>
                    <td className="p-4 pl-6">
                      <div className="flex items-center gap-2">
                        <span className="font-bold">{new Date(log.date).toLocaleDateString("en-US", { weekday: "short", month: "short", day: "numeric" })}</span>
                        {isToday && <span className="text-xs bg-emerald-500/20 text-emerald-500 px-2 py-0.5 rounded-full font-bold uppercase">Today</span>}
                      </div>
                    </td>
                    <td className="p-4 font-mono font-bold text-foreground/80">{formatTime(log.clockInTime)}</td>
                    <td className="p-4 font-mono font-bold text-foreground/80">{formatTime(log.clockOutTime)}</td>
                    <td className="p-4 pr-6 text-right">
                      <span className={`font-bold ${log.clockInTime && log.clockOutTime ? "text-emerald-500" : "text-amber-500"}`}>
                        {formatDuration(log.clockInTime, log.clockOutTime)}
                      </span>
                    </td>
                  </tr>
                );
              })}
            </tbody>
          </table>
        </div>
      )}
    </div>
  );
}
