namespace BetAPI.Exceptions
{
    [Serializable]
    public class UserUnavailableException : BaseAPIException {

        public UserUnavailableException() { }

        public UserUnavailableException(string message)
            : base(message) {
            RenderCode = 7;
            RenderMessage  = "User is not available";
            HTTPCode = 400;
        }

        public UserUnavailableException(string message, Exception inner)
            : base(message, inner) { }
    }
}
