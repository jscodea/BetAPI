﻿namespace BetAPI.Exceptions
{
    public class OddsChangedException : BaseAPIException
    {
        public OddsChangedException(
            string internalMessage,
            string customMessage = "Odds have changed"
        ) : base(internalMessage, customMessage)
        {
            RenderMessage = customMessage;
        }
        public override int RenderCode { get { return 6; } }

        public override string RenderMessage
        {
            get { return RenderMessage; }
            set
            {
                RenderMessage = value;
            }
        }
        public override int HTTPCode { get { return 400; } }
    }
}
