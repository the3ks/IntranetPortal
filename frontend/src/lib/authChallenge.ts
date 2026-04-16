type ChallengeStartResponse = {
  challengeId: string;
  nonce: string;
  expiresAt: string;
  algorithm: string;
  publicKeyPem: string;
};

function pemToArrayBuffer(pem: string): ArrayBuffer {
  const base64 = pem
    .replace("-----BEGIN PUBLIC KEY-----", "")
    .replace("-----END PUBLIC KEY-----", "")
    .replace(/\s+/g, "");
  const binary = atob(base64);
  const bytes = new Uint8Array(binary.length);
  for (let i = 0; i < binary.length; i++) {
    bytes[i] = binary.charCodeAt(i);
  }
  return bytes.buffer;
}

function arrayBufferToBase64(buffer: ArrayBuffer): string {
  const bytes = new Uint8Array(buffer);
  let binary = "";
  for (let i = 0; i < bytes.byteLength; i++) {
    binary += String.fromCharCode(bytes[i]);
  }
  return btoa(binary);
}

async function importRsaPublicKey(publicKeyPem: string): Promise<CryptoKey> {
  const keyData = pemToArrayBuffer(publicKeyPem);
  return await crypto.subtle.importKey(
    "spki",
    keyData,
    {
      name: "RSA-OAEP",
      hash: "SHA-256",
    },
    false,
    ["encrypt"]
  );
}

async function encryptPassword(password: string, publicKeyPem: string): Promise<string> {
  const key = await importRsaPublicKey(publicKeyPem);
  const encoded = new TextEncoder().encode(password);
  const encrypted = await crypto.subtle.encrypt({ name: "RSA-OAEP" }, key, encoded);
  return arrayBufferToBase64(encrypted);
}

export async function loginWithChallenge(email: string, password: string): Promise<Response> {
  const startRes = await fetch(`/api/auth/challenge/start`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ email }),
  });

  if (!startRes.ok) {
    return startRes;
  }

  const challenge = (await startRes.json()) as ChallengeStartResponse;
  const encryptedPassword = await encryptPassword(password, challenge.publicKeyPem);

  return await fetch(`/api/auth/challenge/complete`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({
      email,
      challengeId: challenge.challengeId,
      encryptedPassword,
    }),
  });
}
