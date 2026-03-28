using System.ComponentModel.DataAnnotations;

namespace IntranetPortal.Data.Models
{
    public class RoleDelegation
    {
        public int Id { get; set; }

        public int SourceUserId { get; set; }
        public UserAccount SourceUser { get; set; } = null!;

        public int SubstituteUserId { get; set; }
        public UserAccount SubstituteUser { get; set; } = null!;

        public int UserRoleId { get; set; }
        public UserRole UserRole { get; set; } = null!;

        public DateTimeOffset StartDate { get; set; }
        public DateTimeOffset EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
