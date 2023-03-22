namespace BetAPI.Exceptions
{
    [Serializable]
    public class BalanceTooLowException : BaseAPIException
    {
        public BalanceTooLowException() { }

        public BalanceTooLowException(string message)
            : base(message) {
            RenderCode = 1;
            RenderMessage = "Balance is too low";
            HTTPCode = 400;
        }

        public BalanceTooLowException(string message, Exception inner)
            : base(message, inner) { }
    }
}
