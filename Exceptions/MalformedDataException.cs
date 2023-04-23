namespace BetAPI.Exceptions
{
    public class MalformedDataException : BaseAPIException
    {
        public MalformedDataException(
            string internalMessage,
            string customMessage = "Data is malformed"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 5; } }

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
