namespace HyMall_App.Models
{
    public class TenantProfile
    {
        public int Id { get; set; }

        // Link to the ApplicationUser (tenant's account)
        public string UserId { get; set; } = string.Empty;

        // Shop Details (filled by tenant)
        public string ShopName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // e.g., "Clothing"
        public string OperatingHours { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}