namespace BetAPI.Exceptions
{
    [Serializable]
    public class BetDoesNotExistException : BaseAPIException
    {

        public BetDoesNotExistException() {
            RenderCode = 3;
            RenderMessage = "Bet does not exist";
            HTTPCode = 404;
        }

        public BetDoesNotExistException(string message)
            : base(message) {
            RenderCode = 3;
            RenderMessage = "Bet does not exist";
            HTTPCode = 404;
        }

        public BetDoesNotExistException(string message, Exception inner)
            : base(message, inner) {
            RenderCode = 3;
            RenderMessage = "Bet does not exist";
            HTTPCode = 404;
        }
    }
}
