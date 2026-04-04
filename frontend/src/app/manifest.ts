import { MetadataRoute } from 'next'

export default function manifest(): MetadataRoute.Manifest {
  const isDev = process.env.NODE_ENV === 'development'

  return {
    name: 'Intranet Portal',
    short_name: 'Portal',
    description: 'Internal Company Portal and Process Management',
    start_url: '/',
    display: 'standalone',
    background_color: isDev ? '#9333ea' : '#2563eb',
    theme_color: isDev ? '#9333ea' : '#2563eb',
    icons: [
      {
        src: isDev ? '/icon-512x512-purple.png' : '/icon-512x512-blue.png',
        sizes: '512x512',
        type: 'image/png',
        purpose: 'any'
      },
      {
        src: isDev ? '/icon-512x512-purple.png' : '/icon-512x512-blue.png',
        sizes: '512x512',
        type: 'image/png',
        purpose: 'maskable'
      }
    ]
  }
}
