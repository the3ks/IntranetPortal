import fs from "fs";
import path from "path";
import matter from "gray-matter";

const docsDirectory = path.join(process.cwd(), "..", "docs");

export type DocMeta = {
  slug: string;
  title: string;
  description: string;
  date: string;
  author: string;
};

export type DocFile = DocMeta & {
  content: string;
};

export function getSortedDocs(): DocMeta[] {
  if (!fs.existsSync(docsDirectory)) return [];
  
  const fileNames = fs.readdirSync(docsDirectory);
  const allDocs = fileNames
    .filter((fileName) => fileName.endsWith(".md"))
    .map((fileName) => {
      const slug = fileName.replace(/\.md$/, "");
      const fullPath = path.join(docsDirectory, fileName);
      const fileContents = fs.readFileSync(fullPath, "utf8");

      // Parse metadata section
      const matterResult = matter(fileContents);

      return {
        slug,
        title: matterResult.data.title || slug,
        description: matterResult.data.description || "",
        date: matterResult.data.date || "",
        author: matterResult.data.author || "System",
      };
    });

  return allDocs.sort((a, b) => {
    const aIsDev = a.title.startsWith("[DEV]");
    const bIsDev = b.title.startsWith("[DEV]");
    
    // If one is DEV and the other is not, the non-DEV one comes first
    if (aIsDev && !bIsDev) return 1;
    if (!aIsDev && bIsDev) return -1;
    
    // Otherwise, sort by date (newest first)
    return a.date < b.date ? 1 : -1;
  });
}

export function getDocBySlug(slug: string): DocFile | null {
  try {
    const fullPath = path.join(docsDirectory, `${slug}.md`);
    const fileContents = fs.readFileSync(fullPath, "utf8");
    const matterResult = matter(fileContents);

    return {
      slug,
      title: matterResult.data.title || slug,
      description: matterResult.data.description || "",
      date: matterResult.data.date || "",
      author: matterResult.data.author || "System",
      content: matterResult.content,
    };
  } catch (e) {
    return null;
  }
}
