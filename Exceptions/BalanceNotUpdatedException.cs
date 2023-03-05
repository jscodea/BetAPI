namespace BetAPI.Exceptions
{
    [Serializable]
    public class BalanceNotUpdatedException : Exception
    {
        public BalanceNotUpdatedException() { }

        public BalanceNotUpdatedException(string message)
            : base(message) { }

        public BalanceNotUpdatedException(string message, Exception inner)
            : base(message, inner) { }
    }
}
