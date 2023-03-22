namespace BetAPI.Exceptions
{
    [Serializable]
    public class OddsChangedException : BaseAPIException
    {
        public OddsChangedException() { }

        public OddsChangedException(string message)
            : base(message)
        {
            RenderCode = 6;
            RenderMessage = "Odds have changed";
            HTTPCode = 400;
        }

            public OddsChangedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
