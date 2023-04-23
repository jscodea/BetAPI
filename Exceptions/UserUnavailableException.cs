namespace BetAPI.Exceptions
{
    public class UserUnavailableException : BaseAPIException {

        public UserUnavailableException(
            string internalMessage,
            string customMessage = "User is not available"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 7; } }

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
