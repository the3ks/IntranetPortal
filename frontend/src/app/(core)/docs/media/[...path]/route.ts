import { NextResponse } from 'next/server';
import fs from 'fs/promises';
import { existsSync } from 'fs';
import path from 'path';

export async function GET(request: Request, { params }: { params: Promise<{ path: string[] }> }) {
  try {
    const resolvedParams = await params;
    const filePathArray = resolvedParams.path;
    
    // process.cwd() is the root of the 'frontend' directory during execution.
    // We navigate up one level, and then into the docs/media repository to find the true file.
    const docsMediaDir = path.resolve(process.cwd(), '..', 'docs', 'media');
    const absolutePath = path.resolve(docsMediaDir, ...filePathArray);

    // Prevent Directory Traversal Attacks (e.g. asking for ../../../etc/passwd)
    if (!absolutePath.startsWith(docsMediaDir)) {
      return new NextResponse('Access Denied', { status: 403 });
    }

    if (!existsSync(absolutePath)) {
      return new NextResponse('File not found', { status: 404 });
    }

    const fileBuffer = await fs.readFile(absolutePath);
    
    // Determine exact mime type
    const ext = path.extname(absolutePath).toLowerCase();
    let mimeType = 'application/octet-stream';
    if (ext === '.png') mimeType = 'image/png';
    else if (ext === '.jpg' || ext === '.jpeg') mimeType = 'image/jpeg';
    else if (ext === '.gif') mimeType = 'image/gif';
    else if (ext === '.svg') mimeType = 'image/svg+xml';
    else if (ext === '.webp') mimeType = 'image/webp';

    return new NextResponse(fileBuffer, {
      headers: {
        'Content-Type': mimeType,
        'Cache-Control': 'public, max-age=86400, stale-while-revalidate=43200',
      },
    });
  } catch (err) {
    console.error("Documentation media router failed:", err);
    return new NextResponse('Internal Server Error', { status: 500 });
  }
}
