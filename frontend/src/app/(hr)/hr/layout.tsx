// Rule 4 (.geminirules): Shared layout.tsx for the HR module route group
// Provides the shared MainLayout wrapper for all sub-pages, avoiding repetition in each page.tsx
import MainLayout from "@/components/layout/MainLayout";

export default function HRLayout({ children }: { children: React.ReactNode }) {
  return <MainLayout>{children}</MainLayout>;
}
