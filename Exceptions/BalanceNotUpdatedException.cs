namespace BetAPI.Exceptions
{
    public class BalanceNotUpdatedException : BaseAPIException
    {
        public BalanceNotUpdatedException(
            string internalMessage,
            string customMessage = "Balance could not be updated"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 1; } }

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
