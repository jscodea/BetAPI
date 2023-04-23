using BetAPI.DTO;
using System.Text.Json;

namespace BetAPI.Exceptions
{
    public abstract class BaseAPIException : Exception
    {
        public abstract int RenderCode { get; }
        public abstract string RenderMessage { get; set; }
        public abstract int HTTPCode { get; }
        public BaseAPIException(string message, string customMessage) : base(message)
        {
            RenderMessage = customMessage;
        }
        public string GetRenderJSON()
        {
            return JsonSerializer.Serialize(GetRenderObj());
        }
        public int GetHttpCode()
        {
            return HTTPCode;
        }
        public APIExceptionDTO GetRenderObj()
        {
            return new APIExceptionDTO
            {
                Code = RenderCode,
                Message = RenderMessage
            };
        }
    }
}
