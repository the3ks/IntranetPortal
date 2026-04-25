"use client";
import { useState, useEffect } from "react";

export default function OnboardingClient() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    fetch(`${backendUrl}/api/hr/onboarding/my-tasks`, { credentials: "include" })
      .then(r => r.ok ? r.json() : [])
      .then(data => { setTasks(data); setLoading(false); });
  }, []);

  const completeTask = async (id: number) => {
    const backendUrl = process.env.NEXT_PUBLIC_API_URL || "http://localhost:5254";
    const res = await fetch(`${backendUrl}/api/hr/onboarding/tasks/${id}/complete`, { method: "POST", credentials: "include" });
    if (res.ok) {
      setTasks(tasks.map((t: any) => t.id === id ? { ...t, isCompleted: true, completedAt: new Date().toISOString() } : t));
    }
  };

  return (
    <div className="max-w-7xl mx-auto py-8 space-y-8">
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-black text-foreground">My Onboarding Tasks</h1>
          <p className="text-foreground/70">Complete your required checklists.</p>
        </div>
        <button className="bg-background border border-border/50 hover:bg-background/80 text-foreground px-5 py-2.5 rounded-xl font-bold transition-colors">
          Manage Templates
        </button>
      </div>

      {loading ? (
        <div className="flex items-center justify-center py-16">
          <div className="w-8 h-8 border-4 border-border/50 border-t-emerald-500 rounded-full animate-spin" />
        </div>
      ) : (
        <div className="bg-card border border-border/50 rounded-3xl p-8 shadow-sm">
          {tasks.length === 0 ? (
            <div className="text-center py-10">
              <div className="w-16 h-16 bg-emerald-500/10 text-emerald-500 rounded-full flex items-center justify-center mx-auto mb-4">
                <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M5 13l4 4L19 7" /></svg>
              </div>
              <h3 className="text-xl font-bold mb-2">You're all caught up!</h3>
              <p className="text-foreground/60">There are no onboarding tasks assigned to you right now.</p>
            </div>
          ) : (
            <div className="space-y-4">
              {tasks.map((task: any) => (
                <div key={task.id} className={`p-5 rounded-2xl border flex gap-4 ${task.isCompleted ? 'bg-background/30 border-border/30 opacity-70' : 'bg-background border-border/50 shadow-sm'}`}>
                  <div className="pt-1">
                    {task.isCompleted ? (
                      <div className="w-6 h-6 rounded-full bg-emerald-500 text-white flex items-center justify-center">
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M5 13l4 4L19 7" /></svg>
                      </div>
                    ) : (
                      <button onClick={() => completeTask(task.id)} className="w-6 h-6 rounded-full border-2 border-border/50 hover:border-emerald-500 hover:bg-emerald-500/10 transition-colors" />
                    )}
                  </div>
                  <div>
                    <h4 className={`font-bold text-lg mb-1 ${task.isCompleted ? 'line-through text-foreground/50' : 'text-foreground'}`}>{task.title}</h4>
                    {task.description && <p className="text-foreground/70 text-sm">{task.description}</p>}
                    {task.isCompleted && task.completedAt && (
                      <p className="text-xs text-emerald-500 font-bold mt-2 uppercase tracking-wide">Completed {new Date(task.completedAt).toLocaleDateString()}</p>
                    )}
                  </div>
                </div>
              ))}
            </div>
          )}
        </div>
      )}
    </div>
  );
}
