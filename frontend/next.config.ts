import type { NextConfig } from "next";
import withSerwistInit from "@serwist/next";

const withSerwist = withSerwistInit({
  swSrc: "src/app/sw.ts",
  swDest: "public/sw.js",
  disable: process.env.NODE_ENV === "development",
});

const nextConfig: NextConfig = {
  // Any specific next.js config goes here
};

const finalConfig = withSerwist(nextConfig);

// Turbopack strictly errors out if it sees a Webpack configuration object.
// Since Serwist is disabled in development anyway, we can cleanly strip out the webpack property 
// before handing it to Next.js, allowing Turbopack to compile at lightning speed!
if (process.env.NODE_ENV === "development" && finalConfig.webpack) {
  delete finalConfig.webpack;
}

export default finalConfig;
