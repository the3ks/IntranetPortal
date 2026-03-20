import Sidebar from "./Sidebar";
import Header from "./Header";
import { getUserSession } from "@/app/actions/auth";

export default async function MainLayout({ children }: { children: React.ReactNode }) {
  const user = await getUserSession();

  return (
    <div className="min-h-screen bg-[#f8fafc] flex">
      <Sidebar user={user} />
      <div className="flex-1 flex flex-col md:ml-72 transition-all">
        <Header />
        <main className="p-4 sm:p-8 flex-1 overflow-auto">
          <div className="max-w-7xl mx-auto">
            {children}
          </div>
        </main>
      </div>
    </div>
  );
}
