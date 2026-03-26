"use client";

import { useState, useTransition } from "react";
import { createDepartmentAction, updateDepartmentAction, deleteDepartmentAction } from "@/app/actions/departments";
import SiteFilter from "@/components/ui/SiteFilter";

export interface Department {
  id: number;
  name: string;
  siteId?: number;
}

export default function DepartmentManager({
  initialDepartments,
  initialSites,
  canCreate,
  canEdit,
  canDelete,
  permittedSites,
  currentSiteId,
  filterDisabled
}: {
  initialDepartments: Department[],
  initialSites: any[],
  canCreate: boolean,
  canEdit: boolean,
  canDelete: boolean,
  permittedSites?: any[],
  currentSiteId?: string,
  filterDisabled?: boolean
}) {
  const [isOpen, setIsOpen] = useState(false);
  const [editingId, setEditingId] = useState<number | null>(null);
  const [name, setName] = useState("");
  const [siteId, setSiteId] = useState<string>("");
  const [isPending, startTransition] = useTransition();

  const openModal = (dept?: Department) => {
    if (dept) {
      setEditingId(dept.id);
      setName(dept.name);
      setSiteId(dept.siteId?.toString() || (initialSites.length > 0 ? initialSites[0].id.toString() : ""));
    } else {
      setEditingId(null);
      setName("");
      setSiteId(initialSites.length > 0 ? initialSites[0].id.toString() : "");
    }
    setIsOpen(true);
  };

  const closeModal = () => {
    setIsOpen(false);
    setName("");
    setSiteId("");
    setEditingId(null);
  };

  const handleSave = async (e: React.FormEvent) => {
    e.preventDefault();
    startTransition(async () => {
      try {
        const formData = new FormData();
        formData.append("name", name);
        if (siteId) formData.append("siteId", siteId);
        if (editingId) formData.append("id", editingId.toString());

        if (editingId) {
          await updateDepartmentAction(formData);
        } else {
          await createDepartmentAction(formData);
        }
        closeModal();
      } catch (error) {
        console.error("Failed to save department", error);
        alert("An error occurred while saving the department.");
      }
    });
  };

  const handleDelete = (id: number) => {
    if (confirm("Are you sure you want to completely remove this department?")) {
      startTransition(async () => {
        try {
          await deleteDepartmentAction(id);
        } catch (error) {
          console.error("Failed to delete", error);
          alert("Failed to delete the department.");
        }
      });
    }
  };

  return (
    <div className="space-y-6 max-w-7xl mx-auto">
      <div className="flex flex-col xl:flex-row xl:items-center justify-between gap-6 bg-white p-6 rounded-3xl shadow-sm border border-gray-100">
        <div>
          <h2 className="text-3xl font-extrabold text-gray-900 tracking-tight">Organization Departments</h2>
          <p className="text-gray-500 mt-2 text-base">Manage global divisions and organizational units.</p>
        </div>
        <div className="flex flex-col sm:flex-row items-start sm:items-center gap-6 divide-y sm:divide-y-0 sm:divide-x divide-gray-200">
          {permittedSites && (
            <div className="pt-4 sm:pt-0 sm:pr-6 w-full sm:w-auto">
              <SiteFilter sites={permittedSites} currentSiteId={currentSiteId} disabled={!!filterDisabled} />
            </div>
          )}
          {canCreate && (
            <div className="pt-4 sm:pt-0 sm:pl-6">
              <button
                onClick={() => openModal()}
                className="bg-indigo-600 hover:bg-indigo-700 text-white px-6 py-3 rounded-xl font-semibold shadow-md transition-all flex items-center space-x-2 shrink-0 whitespace-nowrap"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M12 4v16m8-8H4" />
                </svg>
                <span>New Department</span>
              </button>
            </div>
          )}
        </div>
      </div>

      <div className="bg-white rounded-2xl shadow-xl shadow-gray-200/50 border border-gray-100 overflow-hidden relative min-h-[200px]">
        {isPending && (
          <div className="absolute inset-0 bg-white/50 backdrop-blur-sm z-10 flex items-center justify-center">
            <svg className="animate-spin h-8 w-8 text-indigo-600" fill="none" viewBox="0 0 24 24">
              <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
              <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
            </svg>
          </div>
        )}
        <div className="overflow-x-auto">
          <table className="min-w-full divide-y divide-gray-200">
            <thead className="bg-gray-50">
              <tr>
                <th scope="col" className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider w-24">ID</th>
                <th scope="col" className="px-6 py-4 text-left text-xs font-bold text-gray-500 uppercase tracking-wider">Department Name</th>
                <th scope="col" className="px-6 py-4 text-right text-xs font-bold text-gray-500 uppercase tracking-wider w-32">Actions</th>
              </tr>
            </thead>
            <tbody className="bg-white divide-y divide-gray-100">
              {initialDepartments.length === 0 ? (
                <tr>
                  <td colSpan={3} className="px-6 py-16 text-center text-gray-400 italic font-medium">
                    No departments configured yet. Click "New Department" to get started.
                  </td>
                </tr>
              ) : (
                initialDepartments.map((dept) => (
                  <tr key={dept.id} className="hover:bg-indigo-50/50 transition-colors group">
                    <td className="px-6 py-5 whitespace-nowrap text-sm font-medium text-gray-400">#{dept.id}</td>
                    <td className="px-6 py-5 whitespace-nowrap text-sm font-bold text-gray-800">{dept.name}</td>
                    <td className="px-6 py-5 whitespace-nowrap text-right text-sm font-medium space-x-2 sm:opacity-0 group-hover:opacity-100 transition-opacity flex justify-end">
                      {canEdit && (
                        <button onClick={() => openModal(dept)} className="text-indigo-600 hover:text-indigo-900 bg-indigo-50 hover:bg-indigo-100 p-2.5 rounded-lg transition-colors inline-block" title="Edit">
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z" /></svg>
                        </button>
                      )}
                      {canDelete && (
                        <button onClick={() => handleDelete(dept.id)} className="text-red-600 hover:text-red-900 bg-red-50 hover:bg-red-100 p-2.5 rounded-lg transition-colors inline-block" title="Delete">
                          <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                        </button>
                      )}
                    </td>
                  </tr>
                ))
              )}
            </tbody>
          </table>
        </div>
      </div>

      {isOpen && (
        <div className="fixed inset-0 z-[100] flex items-center justify-center p-4 sm:p-0">
          <div className="absolute inset-0 bg-gray-900/40 backdrop-blur-sm transition-opacity" onClick={closeModal}></div>
          <div className="relative bg-white rounded-3xl shadow-2xl w-full max-w-md overflow-hidden transform transition-all border border-gray-100 animate-in fade-in zoom-in-95 duration-200">
            <div className="px-6 py-5 border-b border-gray-100 bg-gray-50/50">
              <h3 className="text-xl font-extrabold text-gray-900">{editingId ? "Edit Department" : "Create Department"}</h3>
              <p className="text-sm text-gray-500 mt-1">Fill out the information below.</p>
            </div>
            <form onSubmit={handleSave} className="p-6">
              <div className="mb-6">
                <label htmlFor="name" className="block text-sm font-bold text-gray-700 mb-2">Department Name</label>
                <input
                  type="text"
                  id="name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-4 focus:ring-indigo-500/20 focus:border-indigo-500 transition-all bg-gray-50 text-gray-900 outline-none font-medium"
                  placeholder="e.g. Finance & Accounting"
                  required
                  autoFocus
                />
              </div>
              <div className="mb-6">
                <label className="block text-sm font-bold text-gray-700 mb-2">Physical Location Extent (Site Boundaries)</label>
                <select
                  name="siteId"
                  value={siteId}
                  onChange={e => setSiteId(e.target.value)}
                  className="w-full px-4 py-3 border border-gray-200 rounded-xl focus:ring-4 focus:ring-indigo-500/20 focus:border-indigo-500 transition-all bg-gray-50 text-gray-900 outline-none font-medium appearance-none"
                  required
                >
                  {initialSites.length === 0 && <option value="">No Sites Found.</option>}
                  {initialSites.map(s => (
                    <option key={s.id} value={s.id}>{s.name} - {s.address}</option>
                  ))}
                </select>
              </div>
              <div className="flex justify-end gap-3 pt-4">
                <button type="button" onClick={closeModal} className="px-5 py-2.5 text-gray-600 hover:bg-gray-100 rounded-xl transition-colors font-semibold">
                  Cancel
                </button>
                <button type="submit" disabled={isPending} className="bg-indigo-600 hover:bg-indigo-700 disabled:opacity-50 text-white px-6 py-2.5 rounded-xl transition-all font-bold shadow-md shadow-indigo-200 flex items-center">
                  {isPending && (
                    <svg className="animate-spin -ml-1 mr-2 h-4 w-4 text-white" fill="none" viewBox="0 0 24 24"><circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle><path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path></svg>
                  )}
                  {editingId ? "Save Changes" : "Create"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
