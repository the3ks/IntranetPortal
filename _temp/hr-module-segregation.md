# HR Module Segregation - Progress Summary

Date: 2026-04-25

## What has been completed so far

1. Module-level segregation is now in place across layers.
- Backend controllers are split by domain under Assets, Core, and HR.
- Data configurations are split by domain under Configurations/Assets, Configurations/Core, and Configurations/HR.
- Data models for HR are isolated under Models/HR.

2. Core table prefixing has been applied.
- Core entities are mapped with Core_ table naming via dedicated configuration classes.
- This aligns with the modular monolith naming convention.

3. HR schema has been introduced and expanded.
- HR tables added for positions, departments, employees, leave, onboarding, and attendance.
- HR-specific entity configurations map to HR_ table names.

4. Key migration milestones were implemented.
- AddCorePrefixesToTables: standardized Core_ naming.
- PartitionPositionToHRModule: moved positions from Core_Positions to HR_Positions.
- InitialHRSchema: introduced initial HR domain schema.
- AddAttendanceLogs: added HR attendance tracking.
- UnifyPersonnelToHR: consolidated personnel references to HR_Employees and HR_Departments and rewired many cross-module foreign keys.

5. API surface for HR domain has been added.
- HR controllers now exist for positions, employees, departments, leave, onboarding, and attendance.
- Routes are grouped under api/hr/...

6. Frontend HR route space has been scaffolded.
- Dedicated HR route group and pages were added for positions, departments, employee records, leave, onboarding, and attendance.

## Current technical state

1. The HR segregation direction is implemented end-to-end in structure and migrations.
2. The repository currently contains legacy Core personnel traces plus new HR personnel models in parallel, with migration-driven convergence in progress.
3. A recent compile blocker in Data was fixed.
- Missing type resolution for Position in the Core Employee model was resolved by importing the HR namespace.
- IntranetPortal.Data now builds successfully after that fix.

## Practical outcome

- Core concerns remain in Core_ tables.
- HR domain entities and workflows are moving under HR_ tables and HR namespaces.
- Cross-module references (including Assets and selected Core links) are being redirected to HR personnel entities as part of the unification step.

## Next consolidation pass suggested

1. Remove or finalize remaining legacy Core personnel model paths once all code paths are switched to HR models.
2. Confirm all DbSet and controller usages consistently target the unified HR model set.
3. Run full solution build and migration smoke tests to verify no stale references remain.
