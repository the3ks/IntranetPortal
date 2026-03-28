"use client";
import { useState } from "react";
import { resetUserPasswordAction } from "@/app/actions/users";

export default function PasswordResetButton({ user }: { user: any }) {
  const [isOpen, setIsOpen] = useState(false);
  const [newPassword, setNewPassword] = useState("");
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [success, setSuccess] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newPassword || newPassword.length < 6) {
      setError("Password must be at least 6 characters.");
      return;
    }

    setIsLoading(true);
    setError(null);
    setSuccess(false);

    const res = await resetUserPasswordAction(user.id, newPassword);
    setIsLoading(false);

    if (res.success) {
      setSuccess(true);
    } else {
      setError(res.error || "Failed to reset password.");
    }
  };

  const generateRandomPassword = () => {
    const chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*";
    let pass = "";
    for (let i = 0; i < 12; i++) {
      pass += chars.charAt(Math.floor(Math.random() * chars.length));
    }
    setNewPassword(pass);
  };

  return (
    <>
      <button 
        onClick={() => setIsOpen(true)}
        className="p-1.5 rounded-lg text-gray-400 hover:text-blue-600 hover:bg-blue-50 transition-all ml-1 group relative"
        title="Reset Password"
      >
        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" />
        </svg>
      </button>

      {isOpen && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-gray-900/60 backdrop-blur-sm animate-in fade-in duration-200">
          <div className="bg-white rounded-2xl shadow-2xl w-full max-w-sm overflow-hidden animate-in zoom-in-95 duration-200">
            <div className="px-6 py-5 border-b border-gray-100 flex items-center justify-between bg-gray-50/50">
              <h3 className="text-lg font-bold text-gray-900 flex items-center">
                <svg className="w-5 h-5 text-blue-500 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M15 7a2 2 0 012 2m4 0a6 6 0 01-7.743 5.743L11 17H9v2H7v2H4a1 1 0 01-1-1v-2.586a1 1 0 01.293-.707l5.964-5.964A6 6 0 1121 9z" />
                </svg>
                {success ? "Reset Complete" : "Reset Password"}
              </h3>
              <button 
                onClick={() => { setIsOpen(false); setNewPassword(""); setSuccess(false); setError(null); }} 
                className="text-gray-400 hover:text-gray-600 p-1 rounded-lg hover:bg-gray-200 transition-colors"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" /></svg>
              </button>
            </div>
            
            <div className="p-6">
              <div className="mb-4">
                <p className="text-sm text-gray-500 font-medium">Resetting credentials for:</p>
                <div className="text-gray-900 font-bold bg-gray-50 border border-gray-100 rounded-lg p-2.5 mt-1">{user.employeeName} <span className="text-gray-400 font-normal ml-1">({user.email})</span></div>
              </div>

              {success ? (
                <div className="text-center py-2">
                  <div className="w-16 h-16 bg-emerald-50 text-emerald-600 rounded-full flex items-center justify-center mx-auto mb-4 border border-emerald-100">
                    <svg className="w-8 h-8" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={3} d="M5 13l4 4L19 7" /></svg>
                  </div>
                  <h4 className="text-xl font-extrabold text-gray-900 mb-2">Password Reset Successful!</h4>
                  <p className="text-sm text-gray-500 mb-6">The new credentials are securely active. Please copy the exact string below and deliver it to the user.</p>
                  
                  <div className="bg-gray-50 border border-gray-200 rounded-xl p-4 flex items-center justify-between mb-8 shadow-inner">
                    <code className="text-lg font-bold text-gray-900 break-all select-all flex-1 text-left">{newPassword}</code>
                    <button 
                      onClick={() => navigator.clipboard.writeText(newPassword)} 
                      className="ml-4 p-2.5 bg-white border border-gray-200 rounded-lg hover:bg-blue-50 hover:text-blue-700 hover:border-blue-200 text-gray-600 transition-all focus:ring-4 focus:ring-blue-100 shadow-sm shrink-0 group relative" 
                      title="Copy to clipboard"
                    >
                      <svg className="w-5 h-5 transition-transform group-active:scale-90" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z" /></svg>
                    </button>
                  </div>
                  
                  <button onClick={() => { setIsOpen(false); setNewPassword(""); setSuccess(false); setError(null); }} className="w-full px-5 py-3.5 rounded-xl bg-gray-900 hover:bg-black active:scale-[0.98] text-white font-bold transition-all shadow-md shadow-gray-900/20">
                    Done
                  </button>
                </div>
              ) : (
                <>
                  {error && <div className="mb-4 text-sm text-rose-600 bg-rose-50 border border-rose-100 p-3 rounded-xl font-medium">{error}</div>}

                  <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                      <div className="flex items-center justify-between mb-1.5">
                        <label className="block text-sm font-bold text-gray-700">New Password</label>
                        <button type="button" onClick={generateRandomPassword} className="text-xs font-bold text-blue-600 hover:text-blue-800 flex items-center transition-colors bg-blue-50 px-2 py-1 rounded-md">
                          <svg className="w-3.5 h-3.5 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" /></svg>
                          Generate Random
                        </button>
                      </div>
                      <input
                        type="password"
                        value={newPassword}
                        onChange={(e) => setNewPassword(e.target.value)}
                        placeholder="Enter new strong password"
                        className="w-full px-4 py-2.5 rounded-xl border border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-blue-500 outline-none transition-all shadow-sm font-mono tracking-wider"
                        autoFocus
                      />
                      <p className="text-xs text-gray-400 mt-1.5 font-medium ml-1">Must be at least 6 characters explicitly.</p>
                    </div>
                    
                    <div className="pt-2 flex justify-end gap-3">
                      <button type="button" onClick={() => { setIsOpen(false); setNewPassword(""); setError(null); }} className="px-5 py-2.5 rounded-xl font-bold text-gray-600 hover:bg-gray-100 transition-colors">Cancel</button>
                      <button type="submit" disabled={isLoading || !newPassword} className="px-5 py-2.5 rounded-xl bg-blue-600 hover:bg-blue-700 active:bg-blue-800 text-white font-bold transition-all shadow-md disabled:opacity-50 flex items-center">
                        {isLoading ? "Saving..." : "Confirm Reset"}
                      </button>
                    </div>
                  </form>
                </>
              )}
            </div>
          </div>
        </div>
      )}
    </>
  );
}
