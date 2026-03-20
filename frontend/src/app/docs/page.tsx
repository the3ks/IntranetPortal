import { getSortedDocs } from "@/lib/docs";
import MainLayout from "@/components/layout/MainLayout";
import Link from "next/link";

export default function DocsIndex() {
  const docs = getSortedDocs();

  return (
    <MainLayout>
      <div className="max-w-5xl mx-auto py-8">
        <div className="mb-10">
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Documentation Hub</h1>
          <p className="mt-2 text-lg text-gray-500">Official technical specifications and organizational guides.</p>
        </div>
        
        <div className="grid gap-6 md:grid-cols-2">
          {docs.length === 0 ? (
            <div className="col-span-2 text-center py-12 bg-gray-50 rounded-2xl border border-dashed border-gray-200">
              <p className="text-gray-500 font-medium">No documentation files found in /docs.</p>
            </div>
          ) : docs.map(doc => (
            <Link key={doc.slug} href={`/docs/${doc.slug}`} className="block group">
              <div className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 hover:border-blue-200 hover:bg-blue-50/50 transition-all h-full relative overflow-hidden">
                <div className="absolute left-0 top-0 bottom-0 w-1 bg-blue-500 opacity-0 group-hover:opacity-100 transition-opacity"></div>
                <h3 className="text-xl font-bold text-gray-900 group-hover:text-blue-700 transition-colors">{doc.title}</h3>
                <p className="text-gray-500 mt-3 line-clamp-2 leading-relaxed">{doc.description}</p>
                <div className="mt-5 flex items-center text-xs text-gray-400 font-medium">
                  <span className="flex items-center">
                    <div className="w-5 h-5 rounded-full bg-gray-200 text-gray-600 flex items-center justify-center mr-2 text-[10px] font-bold">
                        {doc.author.substring(0,2).toUpperCase()}
                    </div> 
                    {doc.author}
                  </span>
                  <span className="mx-3">•</span> 
                  <span>{doc.date}</span>
                </div>
              </div>
            </Link>
          ))}
        </div>
      </div>
    </MainLayout>
  );
}
