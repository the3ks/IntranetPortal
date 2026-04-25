"use client";
import { useState, useEffect, useCallback } from "react";
import Link from "next/link";

interface AttendanceStatus {
  hasRecord: boolean;
  isClockedIn: boolean;
  clockInTime: string | null;
  clockOutTime: string | null;
  pendingLeaveCount: number;
  date: string;
}

function formatTime(iso: string | null) {
  if (!iso) return "--:--";
  return new Date(iso).toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" });
}

function elapsed(from: string | null) {
  if (!from) return null;
  const diff = Date.now() - new Date(from).getTime();
  const h = Math.floor(diff / 3600000);
  const m = Math.floor((diff % 3600000) / 60000);
  return `${h}h ${m}m`;
}

export default function AttendanceWidget() {
  const [status, setStatus] = useState<AttendanceStatus | null>(null);
  const [loading, setLoading] = useState(true);
  const [actionLoading, setActionLoading] = useState(false);
  const [toast, setToast] = useState<{ msg: string; ok: boolean } | null>(null);
  const [tick, setTick] = useState(0);

  const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";

  const fetchStatus = useCallback(async () => {
    try {
      const res = await fetch(`${backendUrl}/api/hr/attendance/today`, { credentials: "include" });
      if (res.ok) setStatus(await res.json());
    } catch { /* non-HR users simply won't see data */ }
    finally { setLoading(false); }
  }, [backendUrl]);

  useEffect(() => {
    fetchStatus();
    // Live elapsed timer
    const interval = setInterval(() => setTick(t => t + 1), 60000);
    return () => clearInterval(interval);
  }, [fetchStatus]);

  const showToast = (msg: string, ok: boolean) => {
    setToast({ msg, ok });
    setTimeout(() => setToast(null), 3500);
  };

  const handleClockIn = async () => {
    setActionLoading(true);
    const res = await fetch(`${backendUrl}/api/hr/attendance/clock-in`, { method: "POST", credentials: "include" });
    const data = await res.json();
    if (res.ok) { showToast(data.message, true); fetchStatus(); }
    else showToast(data.message || "Error clocking in.", false);
    setActionLoading(false);
  };

  const handleClockOut = async () => {
    setActionLoading(true);
    const res = await fetch(`${backendUrl}/api/hr/attendance/clock-out`, { method: "POST", credentials: "include" });
    const data = await res.json();
    if (res.ok) { showToast(`${data.message} Duration: ${data.duration}`, true); fetchStatus(); }
    else showToast(data.message || "Error clocking out.", false);
    setActionLoading(false);
  };

  // If the user has no HR record, show nothing (they might be an admin without a record)
  if (!loading && (!status || !status.hasRecord)) return null;

  const today = new Date().toLocaleDateString("en-US", { weekday: "long", month: "long", day: "numeric" });

  return (
    <div className="max-w-4xl mx-auto mb-10">
      {/* Toast */}
      {toast && (
        <div className={`fixed top-6 right-6 z-50 px-5 py-3.5 rounded-2xl shadow-2xl font-bold text-white text-sm transition-all flex items-center gap-3 ${toast.ok ? "bg-emerald-600" : "bg-red-600"}`}>
          {toast.ok
            ? <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M5 13l4 4L19 7" /></svg>
            : <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M6 18L18 6M6 6l12 12" /></svg>
          }
          {toast.msg}
        </div>
      )}

      <div className="bg-card border border-border/50 rounded-3xl p-8 shadow-sm grid grid-cols-1 md:grid-cols-3 gap-6">
        
        {/* Clock In/Out button area */}
        <div className="col-span-1 flex flex-col items-center justify-center gap-4 text-center border-r border-border/30 pr-6">
          <div className="text-xs font-bold uppercase tracking-widest text-foreground/40">{today}</div>

          {loading ? (
            <div className="w-32 h-32 rounded-full border-4 border-border/30 animate-pulse" />
          ) : status?.clockOutTime ? (
            // Fully done for today
            <div className="flex flex-col items-center gap-3">
              <div className="w-28 h-28 rounded-full bg-emerald-500/10 border-4 border-emerald-500/30 flex items-center justify-center">
                <svg className="w-12 h-12 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" /></svg>
              </div>
              <p className="text-sm font-bold text-emerald-500">Day complete!</p>
            </div>
          ) : status?.isClockedIn ? (
            // Currently clocked in
            <button
              onClick={handleClockOut}
              disabled={actionLoading}
              className="w-32 h-32 rounded-full bg-gradient-to-br from-rose-500 to-orange-500 text-white font-black text-lg shadow-xl shadow-rose-500/30 hover:shadow-rose-500/50 hover:scale-105 active:scale-95 transition-all duration-200 flex flex-col items-center justify-center gap-1 disabled:opacity-60"
            >
              <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5.636 5.636a9 9 0 1012.728 0M12 3v9" /></svg>
              <span className="text-sm">Clock Out</span>
            </button>
          ) : (
            // Not yet clocked in
            <button
              onClick={handleClockIn}
              disabled={actionLoading}
              className="w-32 h-32 rounded-full bg-gradient-to-br from-emerald-500 to-teal-500 text-white font-black text-lg shadow-xl shadow-emerald-500/30 hover:shadow-emerald-500/50 hover:scale-105 active:scale-95 transition-all duration-200 flex flex-col items-center justify-center gap-1 disabled:opacity-60"
            >
              <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5.636 5.636a9 9 0 1012.728 0M12 3v9" /></svg>
              <span className="text-sm">Clock In</span>
            </button>
          )}
        </div>

        {/* Time details */}
        <div className="col-span-1 flex flex-col justify-center gap-5 px-4">
          <div className="flex justify-between items-center">
            <div>
              <p className="text-xs font-bold text-foreground/40 uppercase tracking-wider mb-1">Clock In</p>
              <p className="text-2xl font-black text-foreground">{formatTime(status?.clockInTime ?? null)}</p>
            </div>
            <div className="text-right">
              <p className="text-xs font-bold text-foreground/40 uppercase tracking-wider mb-1">Clock Out</p>
              <p className="text-2xl font-black text-foreground">{formatTime(status?.clockOutTime ?? null)}</p>
            </div>
          </div>

          {status?.isClockedIn && status.clockInTime && (
            <div className="bg-emerald-500/10 border border-emerald-500/20 rounded-2xl px-5 py-3 text-center">
              <p className="text-xs font-bold text-emerald-500/70 uppercase tracking-wider mb-0.5">Time Elapsed</p>
              <p className="text-3xl font-black text-emerald-500">{elapsed(status.clockInTime) ?? "0h 0m"}</p>
            </div>
          )}
        </div>

        {/* Quick action links */}
        <div className="col-span-1 flex flex-col justify-center gap-3 pl-4 border-l border-border/30">
          <p className="text-xs font-bold uppercase tracking-widest text-foreground/40 mb-1">Quick Actions</p>

          <Link href="/hr/leave" className="flex items-center gap-3 p-3 rounded-xl hover:bg-background transition-colors group">
            <div className="w-9 h-9 rounded-xl bg-amber-500/10 flex items-center justify-center flex-shrink-0 group-hover:bg-amber-500/20 transition-colors">
              <svg className="w-5 h-5 text-amber-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" /></svg>
            </div>
            <div>
              <p className="font-bold text-sm text-foreground group-hover:text-amber-500 transition-colors">Request Leave</p>
              {(status?.pendingLeaveCount ?? 0) > 0 && (
                <p className="text-xs text-amber-500 font-bold">{status!.pendingLeaveCount} pending</p>
              )}
            </div>
          </Link>

          <Link href="/hr/attendance" className="flex items-center gap-3 p-3 rounded-xl hover:bg-background transition-colors group">
            <div className="w-9 h-9 rounded-xl bg-blue-500/10 flex items-center justify-center flex-shrink-0 group-hover:bg-blue-500/20 transition-colors">
              <svg className="w-5 h-5 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2" /></svg>
            </div>
            <p className="font-bold text-sm text-foreground group-hover:text-blue-500 transition-colors">Attendance History</p>
          </Link>

          <Link href="/hr/onboarding" className="flex items-center gap-3 p-3 rounded-xl hover:bg-background transition-colors group">
            <div className="w-9 h-9 rounded-xl bg-fuchsia-500/10 flex items-center justify-center flex-shrink-0 group-hover:bg-fuchsia-500/20 transition-colors">
              <svg className="w-5 h-5 text-fuchsia-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            </div>
            <p className="font-bold text-sm text-foreground group-hover:text-fuchsia-500 transition-colors">My Onboarding</p>
          </Link>
        </div>
      </div>
    </div>
  );
}
