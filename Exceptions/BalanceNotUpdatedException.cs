namespace BetAPI.Exceptions
{
    [Serializable]
    public class BalanceNotUpdatedException : BaseAPIException
    {
        public BalanceNotUpdatedException() { }

        public BalanceNotUpdatedException(string message)
            : base(message) {
            RenderCode = 2;
            RenderMessage = "Balance could not be updated";
            HTTPCode = 400;
        }

        public BalanceNotUpdatedException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}
