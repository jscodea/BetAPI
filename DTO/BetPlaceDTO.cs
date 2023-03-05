namespace BetAPI.DTO
{
    public class BetPlaceDTO
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int Opt { get; set; }
        public decimal Stake { get; set; }
        public decimal Odds { get; set; }
    }
}
