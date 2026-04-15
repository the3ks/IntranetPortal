"use client";
import React, { useState } from "react";
import { UserGroupIcon, PlusIcon, TrashIcon } from "@heroicons/react/24/outline";
import {
  createApproverGroupAction,
  getGroupMembersAction,
  getEligibleApproversAction,
  addGroupMemberAction,
  removeGroupMemberAction
} from "@/app/actions/assets";

type Group = {
  id: number;
  name: string;
  isActive: boolean;
};

export default function ApproverGroupsManager({ groups, onUpdate }: { groups: Group[], onUpdate: () => void }) {
  const [showForm, setShowForm] = useState(false);
  const [newGroupName, setNewGroupName] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const handleCreateGroup = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newGroupName.trim()) return;

    setIsSubmitting(true);
    const res = await createApproverGroupAction(newGroupName);

    setIsSubmitting(false);
    if (res.success) {
      setNewGroupName("");
      setShowForm(false);
      onUpdate();
    } else {
      alert("Failed to create group.");
    }
  };

  return (
    <div className="flex-1 bg-card border border-border/50 rounded-2xl shadow-sm overflow-hidden flex flex-col">
      <div className="p-5 border-b border-border/50 flex justify-between items-center bg-background/50/50">
        <h2 className="text-lg font-bold text-foreground flex items-center gap-2">
          <UserGroupIcon className="w-5 h-5 text-purple-600" />
          Approver Groups
        </h2>
        <button
          onClick={() => setShowForm(!showForm)}
          className="text-sm bg-purple-50 text-purple-700 hover:bg-purple-100 px-3 py-1.5 rounded-lg font-semibold transition-colors flex items-center gap-1"
        >
          <PlusIcon className="w-4 h-4" /> New Group
        </button>
      </div>

      {showForm && (
        <form onSubmit={handleCreateGroup} className="p-5 border-b border-purple-50 bg-purple-50/20 grid gap-4">
          <div>
            <label className="text-xs font-bold text-foreground/60 uppercase">Group Name</label>
            <input
              required
              placeholder="e.g. IT Procurement Team"
              className="w-full mt-1 border border-border/50 rounded-lg p-2 text-sm outline-none focus:border-purple-400"
              value={newGroupName}
              onChange={e => setNewGroupName(e.target.value)}
            />
          </div>
          <div className="flex justify-end gap-2 mt-2">
            <button type="button" onClick={() => setShowForm(false)} className="px-4 py-2 text-sm text-foreground/60 hover:bg-background rounded-lg">Cancel</button>
            <button disabled={isSubmitting} type="submit" className="px-4 py-2 text-sm bg-purple-600 hover:bg-purple-700 text-white font-bold rounded-lg shadow-sm">
              {isSubmitting ? "Saving..." : "Save Group"}
            </button>
          </div>
        </form>
      )}

      <div className="flex-1 overflow-auto max-h-[600px] p-4 text-sm">
        {groups.length === 0 ? (
          <div className="text-center py-6 text-gray-400">No Approver Groups defined.</div>
        ) : (
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
            {groups.map(g => (
              <GroupCard key={g.id} group={g} />
            ))}
          </div>
        )}
      </div>
    </div>
  );
}

function GroupCard({ group }: { group: Group }) {
  const [members, setMembers] = useState<{ id: number, fullName: string, email: string }[]>([]);
  const [showAdd, setShowAdd] = useState(false);
  const [eligible, setEligible] = useState<{ id: number, fullName: string, email: string }[]>([]);
  const [search, setSearch] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  React.useEffect(() => {
    loadMembers();
  }, [group.id]);

  const loadMembers = async () => {
    const data = await getGroupMembersAction(group.id);
    setMembers(data);
  };

  const handleSearch = async () => {
    const data = await getEligibleApproversAction(search);
    setEligible(data);
  };

  React.useEffect(() => {
    if (showAdd) {
      handleSearch();
    }
  }, [showAdd, search]);

  const onAdd = async (empId: number) => {
    setIsSubmitting(true);
    await addGroupMemberAction(group.id, empId);
    await loadMembers();
    setIsSubmitting(false);
  };

  const onRemove = async (empId: number) => {
    if (!confirm("Remove this member?")) return;
    setIsSubmitting(true);
    await removeGroupMemberAction(group.id, empId);
    await loadMembers();
    setIsSubmitting(false);
  };

  return (
    <div className="border border-gray-200 rounded-xl p-4 shadow-sm hover:shadow relative bg-white flex flex-col h-full">
      <h3 className="font-bold text-gray-800 text-lg break-words">{group.name}</h3>
      <div>
        <span className={`inline-block mt-2 px-2 py-0.5 text-xs font-semibold rounded-md ${group.isActive ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'}`}>
          {group.isActive ? 'Active' : 'Archived'}
        </span>
      </div>

      <div className="mt-4 pt-4 border-t border-gray-100 flex-1 flex flex-col">
        <div className="flex justify-between items-center mb-2">
          <p className="text-xs text-gray-500 font-bold uppercase tracking-wide">Members ({members.length})</p>
          <button onClick={() => setShowAdd(!showAdd)} className="text-xs font-medium text-purple-600 hover:text-purple-800 px-2 py-1 bg-purple-50 hover:bg-purple-100 rounded transition-colors flex items-center gap-1">
            <PlusIcon className="w-3 h-3" /> Add
          </button>
        </div>

        {showAdd && (
          <div className="mb-3 p-3 bg-purple-50/50 rounded-lg border border-purple-100 animate-in fade-in zoom-in duration-200">
            <input
              autoFocus
              placeholder="Search eligible managers..."
              value={search} onChange={e => setSearch(e.target.value)}
              className="w-full mb-2 p-1.5 text-xs rounded border border-purple-200 outline-none focus:border-purple-400 bg-white"
            />
            {eligible.length === 0 ? (
              <p className="text-xs text-gray-400 italic text-center py-1">No eligible users found</p>
            ) : (
              <div className="max-h-32 overflow-auto flex flex-col gap-1 pr-1 custom-scrollbar">
                {eligible.map(e => (
                  <div key={e.id} className="flex justify-between items-center text-xs p-1.5 hover:bg-white rounded border border-transparent hover:border-gray-200 transition-colors">
                    <span className="truncate flex-1 font-medium text-gray-800" title={e.fullName}>{e.fullName}</span>
                    {members.some(m => m.id === e.id) ? (
                      <span className="text-green-600 font-medium px-1 flex-shrink-0">Added</span>
                    ) : (
                      <button disabled={isSubmitting} onClick={() => onAdd(e.id)} className="text-purple-600 hover:text-white hover:bg-purple-600 px-2 py-0.5 rounded transition-colors flex-shrink-0 font-medium">Add</button>
                    )}
                  </div>
                ))}
              </div>
            )}
            <div className="mt-2 text-right">
              <button disabled={isSubmitting} onClick={() => setShowAdd(false)} className="text-xs text-gray-500 hover:text-gray-700 font-medium">Close Setup</button>
            </div>
          </div>
        )}

        <div className="flex flex-col gap-1.5 flex-1 overflow-auto max-h-48 custom-scrollbar">
          {members.length === 0 && !showAdd && (
            <p className="text-xs text-gray-400 italic">No members assigned.</p>
          )}
          {members.map(m => (
            <div key={m.id} className="flex justify-between items-center bg-gray-50/80 p-2 rounded-lg border border-gray-100 group">
              <div className="min-w-0">
                <p className="text-xs font-semibold text-gray-800 truncate">{m.fullName}</p>
                <p className="text-[10px] text-gray-500 truncate">{m.email}</p>
              </div>
              <button onClick={() => onRemove(m.id)} disabled={isSubmitting} className="opacity-0 group-hover:opacity-100 text-red-500 hover:bg-red-50 p-1.5 rounded transition-all">
                <TrashIcon className="w-3.5 h-3.5" />
              </button>
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}
