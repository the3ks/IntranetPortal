export default function AssetsWikiPage() {
  return (
    <div className="space-y-8">
      <div className="bg-card rounded-3xl p-8 border border-border/50 shadow-sm relative overflow-hidden">
        <div className="absolute top-0 right-0 w-64 h-64 bg-emerald-50 rounded-full blur-3xl -mr-20 -mt-20"></div>
        <div className="relative z-10">
          <h1 className="text-3xl font-black text-foreground mb-4 flex items-center gap-3">
            <svg className="w-8 h-8 text-emerald-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2.5} d="M12 6.253v13m0-13C10.832 5.477 9.246 5 7.5 5S4.168 5.477 3 6.253v13C4.168 18.477 5.754 18 7.5 18s3.332.477 4.5 1.253m0-13C13.168 5.477 14.754 5 16.5 5c1.747 0 3.332.477-4.5 1.253v13C19.832 18.477 18.247 18 16.5 18c-1.746 0-3.332.477-4.5 1.253" /></svg>
            Assets Management Wiki
          </h1>
          <p className="text-foreground/60 text-lg max-w-3xl">
            Welcome to the definitive guide for navigating the Intranet's physical inventory system. 
            This module organizes company hardware, software bindings, and organizational custody in real time.
          </p>
        </div>
      </div>

      <div className="bg-card rounded-3xl shadow-sm border border-border/50 overflow-hidden">
        <div className="px-8 py-6 border-b border-gray-50 bg-background/50/50">
          <h2 className="text-xl font-bold text-foreground flex items-center gap-2">
            <svg className="w-6 h-6 text-blue-500" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M13 16h-1v-4h-1m1-4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" /></svg>
            How it works
          </h2>
        </div>
        <div className="p-8 prose dark:prose-invert max-w-none prose-emerald prose-p:text-foreground/80 text-foreground prose-headings:text-foreground prose-strong:text-foreground prose-code:text-foreground">
          
          <h3 className="text-xl font-bold mt-0 mb-4 border-b border-border/50 pb-2">1. For General Employees</h3>
          <p className="mb-6">
            As an employee, your primary interaction with this module will be tracking what you own and requesting what you need:
          </p>
          <ul className="list-disc pl-5 mb-8 space-y-3">
            <li><strong>My Assets:</strong> This panel shows every serialized device or bulk accessory currently bound to your employment ledger. If you lose a device, contact IT immediately.</li>
            <li><strong>Requisition Center:</strong> Need a new keyboard or a specialized laptop? Check the internal dictionary for available bounded models and submit a formal request.</li>
            <li><strong>Auto-Approval vs Manager Approval:</strong> Low-friction items (like bulk mice or cables) might not require your manager's explicit signature and will flow directly to Fulfillment. Expensive serialized hardware (like MacBooks) strictly mandates approval routing.</li>
          </ul>

          <h3 className="text-xl font-bold mt-10 mb-4 border-b border-border/50 pb-2">2. For Line Managers</h3>
          <p className="mb-6">
            If you manage personnel, you will occasionally receive pings in your <strong>Pending Approvals</strong> queue when your direct reports issue requisitions that require budgetary sign-off.
            You must manually hit "Approve" or "Reject". Once approved, the IT or Facilities team will handle physical custody transfer.
          </p>

          <h3 className="text-xl font-bold mt-10 mb-4 border-b border-border/50 pb-2">3. For Administrators & Fulfillment Agents</h3>
          <p className="mb-6 border-l-4 border-emerald-500 pl-4 py-1 italic bg-emerald-50/50 rounded-r-lg">
            Note: You must possess the <code>Perm:Assets.Manage</code> permission to interact with administrative layers.
          </p>
          <ul className="list-disc pl-5 mb-4 space-y-3">
            <li><strong>Fulfillment Queue:</strong> Acts as the staging area where approved requests land. When a physical asset is handed to the employee, the agent "Fulfills" the request here, binding the asset to their profile.</li>
            <li><strong>System Dictionaries:</strong> A strict topology defining what exists in the system. <em>Categories</em> (e.g. Computers) hold <em>Bounded Models</em> (e.g. Dell XPS). You cannot inject raw unknown models into the system; they must be strictly indexed here first.</li>
            <li><strong>Hardware Inventory vs Accessories:</strong> 
              <br />- <strong>Hardware</strong> implies serialized, high-value entities. Each entity is uniquely tracked using its Serial Tag across its lifecycle (Available → Deployed → InMaintenance).
              <br />- <strong>Accessories</strong> are bulk entities. We simply track the stockpile integer (e.g. 50 extra keyboards). Fulfilling an accessory request simply decrements the stock count without creating a uniquely identifying database row.
            </li>
          </ul>

        </div>
      </div>
    </div>
  );
}
