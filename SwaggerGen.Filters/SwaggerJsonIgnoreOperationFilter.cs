namespace SwaggerGen.Filters
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public sealed class SwaggerJsonIgnoreOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var ignoredProperties = context.MethodInfo.GetParameters()
                                                      .SelectMany(p => p.ParameterType.GetProperties()
                                                                                      .Where(y => y.GetCustomAttribute<JsonIgnoreAttribute>() != null));

            foreach (var property in ignoredProperties)
                operation.Parameters = operation.Parameters
                    .Where(p => !p.Name.Equals(property.Name, StringComparison.InvariantCulture)).ToList();
        }
    }
}