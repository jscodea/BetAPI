namespace BetAPI.Exceptions
{
    public class GenericException : BaseAPIException
    {
        public GenericException(
            string internalMessage,
            string customMessage = "Unknown error"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return -1; } }

        public override string RenderMessage
        {
            get { return RenderMessage; }
            set
            {
                RenderMessage = value;
            }
        }
        public override int HTTPCode { get { return 500; } }
    }
}
