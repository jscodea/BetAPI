namespace BetAPI.Exceptions
{
    [Serializable]
    public class BalanceTooLowException : Exception
    {
        public BalanceTooLowException() { }

        public BalanceTooLowException(string message)
            : base(message) { }

        public BalanceTooLowException(string message, Exception inner)
            : base(message, inner) { }
    }
}
