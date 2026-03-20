import { getDocBySlug } from "@/lib/docs";
import { notFound } from "next/navigation";
import ReactMarkdown from "react-markdown";
import remarkGfm from "remark-gfm";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";

export default async function DocPage({ params }: { params: Promise<{ slug: string }> }) {
  const resolvedParams = await params;
  const doc = getDocBySlug(resolvedParams.slug);
  
  if (!doc) notFound();

  return (
    <MainLayout>
      <div className="max-w-4xl mx-auto py-8">
        <Link href="/docs" className="text-blue-600 hover:text-blue-800 font-medium text-sm flex items-center mb-8 bg-blue-50 w-fit px-4 py-2 rounded-full transition-colors">
          &larr; Back to Documentation Hub
        </Link>
        
        <div className="bg-white p-8 sm:p-12 rounded-3xl shadow-sm border border-gray-100 relative overflow-hidden">
          <header className="mb-10 pb-8 border-b border-gray-100">
            <h1 className="text-4xl font-extrabold text-gray-900 tracking-tight leading-tight">{doc.title}</h1>
            <p className="mt-4 text-xl text-gray-500">{doc.description}</p>
            <div className="mt-8 flex items-center text-sm text-gray-400 font-medium">
              <span className="flex items-center">
                 <div className="w-6 h-6 rounded-full bg-blue-100 text-blue-700 flex items-center justify-center mr-2 text-[10px] font-bold">
                     {doc.author.substring(0,2).toUpperCase()}
                 </div> 
                 Written by {doc.author}
              </span>
              <span className="mx-3">•</span>
              <span>Published on {doc.date}</span>
            </div>
          </header>
          
          <article className="prose prose-blue prose-lg max-w-none text-gray-700 prose-headings:font-bold prose-a:text-blue-600 hover:prose-a:text-blue-500 prose-img:rounded-xl">
            <ReactMarkdown remarkPlugins={[remarkGfm]}>{doc.content}</ReactMarkdown>
          </article>
        </div>
      </div>
    </MainLayout>
  );
}
