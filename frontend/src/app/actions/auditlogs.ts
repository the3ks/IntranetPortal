"use server";
import { fetchWithAuth } from "@/lib/api";

export async function getAuditLogsAction(page: number = 1, pageSize: number = 50) {
    const res = await fetchWithAuth(`/api/auditlogs?page=${page}&pageSize=${pageSize}`);
    if (res.ok) {
        return await res.json();
    }
    return { data: [], totalCount: 0, page: 1, pageSize: 50 };
}
