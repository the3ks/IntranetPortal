namespace IntranetPortal.Data.Models
{
    public class LoginChallenge
    {
        public int Id { get; set; }
        public required string ChallengeId { get; set; }
        public required string NormalizedEmail { get; set; }
        public required string Nonce { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset? ConsumedAt { get; set; }

        public int? UserAccountId { get; set; }
        public UserAccount? UserAccount { get; set; }
    }
}