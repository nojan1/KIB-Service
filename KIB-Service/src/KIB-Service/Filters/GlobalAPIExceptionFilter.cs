using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace KIB_Service.Filters
{
    public class GlobalAPIExceptionFilter : IExceptionFilter
    {
        private IHostingEnvironment env;

        public GlobalAPIExceptionFilter(IHostingEnvironment env)
        {
            this.env = env;
        }

        public void OnException(ExceptionContext context)
        {
            if(context.HttpContext.Request.Path.Value.StartsWith("/api/"))
            {
                var errorResponse = env.IsDevelopment() ? new { Message = context.Exception.Message }
                                                        : new { Message = "An error has ocurred" };

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 500
                };
            }
        }
    }
}
