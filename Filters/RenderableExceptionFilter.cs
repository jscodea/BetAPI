using BetAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace BetAPI.Filters
{
    public class RenderableExceptionFilter : ExceptionFilterAttribute, IExceptionFilter
    {
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is BaseAPIException e)
            {
                context.Result = new JsonResult(e.GetRenderObj())
                {
                    StatusCode = e.GetHttpCode(),
                };
            } else
            {
                var baseException = new BaseAPIException("Uncaught unknown error");
                //Converting all exceptions to base exception
                context.Result = new JsonResult(baseException.GetRenderObj())
                {
                    StatusCode = baseException.GetHttpCode(),
                };
            }
        }
    }
}
