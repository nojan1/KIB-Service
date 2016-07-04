using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using KIB_Service.Models;

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
                var errorResponse = env.EnvironmentName == "Development" ? new ErrorResponseModel { Message = context.Exception.Message }
                                                                         : new ErrorResponseModel { Message = "An error has ocurred" };

                context.Result = new ObjectResult(errorResponse)
                {
                    StatusCode = 500,
                    DeclaredType = typeof(ErrorResponseModel)
                };
            }
        }
    }
}
