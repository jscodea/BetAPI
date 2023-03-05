namespace BetAPI.Exceptions
{
    [Serializable]
    public class UserUnavailableException : Exception {
        public UserUnavailableException() { }

        public UserUnavailableException(string message)
            : base(message) { }

        public UserUnavailableException(string message, Exception inner)
            : base(message, inner) { }
    }
}
