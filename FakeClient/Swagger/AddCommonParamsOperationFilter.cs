using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FakeClient.Swagger
{
    public class AddCommonParamsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null) operation.Parameters = new List<OpenApiParameter>();

            var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (descriptor != null && !descriptor.ControllerName.StartsWith("Weather"))
            {
                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "X-ApiKey",
                    In = ParameterLocation.Header,
                    Description = "Api key",
                    Required = true
                });
            }
        }
    }
}