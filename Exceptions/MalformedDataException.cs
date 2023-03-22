namespace BetAPI.Exceptions
{
    [Serializable]
    public class MalformedDataException : BaseAPIException
    {
        public MalformedDataException() { }

        public MalformedDataException(string message)
            : base(message) {
            RenderCode = 5;
            RenderMessage = "Data is malformed";
            HTTPCode = 400;
        }

        public MalformedDataException(string message, Exception inner)
            : base(message, inner) { }
    }
}
