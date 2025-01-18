using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace vicuna_infra.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthorize = context.ApiDescription.CustomAttributes().OfType<AuthorizeAttribute>().Any();
            if (hasAuthorize)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new()
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme, Id = "oauth2"
                                }
                            },
                            new[] { "openid" }
                        }
                    }
                };
            }
        }
    }
}