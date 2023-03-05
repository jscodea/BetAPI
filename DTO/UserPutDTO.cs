namespace BetAPI.DTO
{
    public class UserPutDTO
    {
        public bool? IsActive { get; set; }
        public decimal? Balance { get; set; }
        public DateTime? BalanceUpdateStarted { get; set; }
    }
}
