namespace BetAPI.Exceptions
{
    [Serializable]
    public class MalformedDataException : Exception
    {
        public MalformedDataException() { }

        public MalformedDataException(string message)
            : base(message) { }

        public MalformedDataException(string message, Exception inner)
            : base(message, inner) { }
    }
}
