namespace BetAPI.Exceptions
{
    public class BalanceTooLowException : BaseAPIException
    {
        public BalanceTooLowException(
            string internalMessage,
            string customMessage = "Balance is too low"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 2; } }

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
