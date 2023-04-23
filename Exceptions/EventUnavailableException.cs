namespace BetAPI.Exceptions
{
    public class EventUnavailableException : BaseAPIException
    {
        public EventUnavailableException(
            string internalMessage,
            string customMessage = "Event is not available"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 4; } }

        public override string RenderMessage
        {
            get { return RenderMessage; }
            set
            {
                RenderMessage = value;
            }
        }
        public override int HTTPCode { get { return 400; } }
    }
}
