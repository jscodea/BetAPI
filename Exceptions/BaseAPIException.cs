using BetAPI.DTO;
using System.Text.Json;

namespace BetAPI.Exceptions
{
    [Serializable]
    public class BaseAPIException : Exception
    {
        internal int RenderCode = -1;
        internal string RenderMessage = "Generic error";
        internal int HTTPCode = 500;
        public BaseAPIException() { }

        public BaseAPIException(string message)
            : base(message) { }

        public BaseAPIException(string message, Exception inner)
            : base(message, inner) { }

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
