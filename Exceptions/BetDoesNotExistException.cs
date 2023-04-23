namespace BetAPI.Exceptions
{
    public class BetDoesNotExistException : BaseAPIException
    {

        /// <summary>
        ///This error requires error message.
        ///Inherits <see cref="BaseAPIException"/>
        ///</summary>
        ///<param name="message"> This is a message</param>
        ///<returns>Returns error</returns>
        public BetDoesNotExistException(
            string internalMessage,
            string customMessage = "Bet does not exist"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 3; } }

        public override string RenderMessage
        {
            get { return RenderMessage; }
            set
            {
                RenderMessage = value;
            }
        }
        public override int HTTPCode { get { return 404; } }
    }
}
