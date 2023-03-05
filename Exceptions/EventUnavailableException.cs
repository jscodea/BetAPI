namespace BetAPI.Exceptions
{
    [Serializable]
    public class EventUnavailableException : Exception
    {
        public EventUnavailableException() { }

        public EventUnavailableException(string message)
            : base(message) { }

        public EventUnavailableException(string message, Exception inner)
            : base(message, inner) { }
    }
}
