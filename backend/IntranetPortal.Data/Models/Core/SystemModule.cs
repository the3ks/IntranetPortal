namespace IntranetPortal.Data.Models
{
    public class SystemModule
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string IconSvg { get; set; }
        public required string Url { get; set; }
        public bool IsActiveGlobally { get; set; } = true;
        public bool IsActive { get; set; } = true;
        public int Order { get; set; } = 0;

        public ICollection<Site> AllowedSites { get; set; } = new List<Site>();
    }
}
