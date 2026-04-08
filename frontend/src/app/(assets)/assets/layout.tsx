import MainLayout from "@/components/layout/MainLayout";

export default function AssetsLayout({ children }: { children: React.ReactNode }) {
  return (
    <MainLayout>
      <div className="max-w-7xl mx-auto space-y-8">
        {children}
      </div>
    </MainLayout>
  );
}
