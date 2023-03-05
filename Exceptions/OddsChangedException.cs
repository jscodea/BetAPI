namespace BetAPI.Exceptions
{
    [Serializable]
    public class OddsChangedException : Exception
    {
        public OddsChangedException() { }

        public OddsChangedException(string message)
            : base(message) { }

        public OddsChangedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
