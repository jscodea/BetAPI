namespace BetAPI.Exceptions
{
    [Serializable]
    public class EventUnavailableException : BaseAPIException
    {
        public EventUnavailableException() { }

        public EventUnavailableException(string message)
            : base(message) {
            RenderCode = 4;
            RenderMessage = "Event is not available.";
            HTTPCode = 400;
        }

        public EventUnavailableException(string message, Exception inner)
            : base(message, inner) { }
    }
}
