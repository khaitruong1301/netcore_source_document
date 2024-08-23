using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;

public class CustomHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var customHeaders = context.MethodInfo.GetCustomAttributes(true)
                            .OfType<CustomHeaderAttribute>()
                            .ToList();

        foreach (var header in customHeaders)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = header.HeaderName,
                In = ParameterLocation.Header,
                Description = header.Description,
                Required = false,
                Schema = new OpenApiSchema
                {
                    Type = "string"
                }
            });
        }
    }
}
