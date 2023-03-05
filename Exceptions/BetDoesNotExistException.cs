namespace BetAPI.Exceptions
{
    [Serializable]
    public class BetDoesNotExistException : Exception
    {
        public BetDoesNotExistException() { }

        public BetDoesNotExistException(string message)
            : base(message) { }

        public BetDoesNotExistException(string message, Exception inner)
            : base(message, inner) { }
    }
}
