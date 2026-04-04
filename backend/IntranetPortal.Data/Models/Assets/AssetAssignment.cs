using System;
using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models.Assets
{
    public class AssetAssignment
    {
        public int Id { get; set; }

        public int AssetId { get; set; }
        public Asset? Asset { get; set; }

        public int? AssignedToEmployeeId { get; set; }
        public Employee? AssignedToEmployee { get; set; }

        public int? AssignedToTeamId { get; set; }
        public Team? AssignedToTeam { get; set; }

        public DateTime DateAssigned { get; set; }

        public int AssignedByEmployeeId { get; set; }
        public Employee? AssignedByEmployee { get; set; }

        public DateTime? ExpectedReturnDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        public int? ReturnedByEmployeeId { get; set; }
        public Employee? ReturnedByEmployee { get; set; }

        [MaxLength(500)]
        public string? ConditionOnAssign { get; set; }

        [MaxLength(500)]
        public string? ConditionOnReturn { get; set; }
    }
}
