using KIB_Service.Filters;
using KIB_Service.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KIB_Service.Tests
{
    public class GlobalExceptionHandlingTests
    {
        [Fact]
        public void InDevelopmentTheExceptionHandlerShouldReturnExceptionMessage()
        {
            var message = GetExceptionMEssageForEnvironment("Development");
            Assert.True(message.Contains("EXCEPTION TEXT"));
        }

        [Fact]
        public void InProductionTheExceptionHandlerShouldNoReturnExceptionMessage()
        {
            var message = GetExceptionMEssageForEnvironment("Production");
            Assert.False(message.Contains("EXCEPTION TEXT"));
        }

        private string GetExceptionMEssageForEnvironment(string environment)
        {
            var hostingEnv = new Mock<IHostingEnvironment>();
            hostingEnv.SetupGet(h => h.EnvironmentName)
                      .Returns(environment);

            var filter = new GlobalAPIExceptionFilter(hostingEnv.Object);

            var mockHttpRequest = new Mock<HttpRequest>();
            mockHttpRequest.SetupGet(x => x.Path)
                           .Returns(new PathString("/api/test/test"));

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.SetupGet(x => x.Request)
                           .Returns(mockHttpRequest.Object);

            var actionContext = new ActionContext();
            actionContext.RouteData = new RouteData();
            actionContext.HttpContext = mockHttpContext.Object;
            actionContext.ActionDescriptor = new ActionDescriptor();

            var context = new ExceptionContext(actionContext, new List<IFilterMetadata> { filter });
            context.Exception = new Exception("EXCEPTION TEXT");

            filter.OnException(context);

            return ((context.Result as ObjectResult).Value as ErrorResponseModel).Message;
        }
    }
}
