namespace SwaggerGen.Filters
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.OpenApi.Models;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public sealed class SwaggerQueryParameterRequiredFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.ApiDescription.ParameterDescriptions.Any())
            {
                return;
            }

            var parameterDescriptions = context.ApiDescription.ParameterDescriptions;

            foreach (var parameterDescription in parameterDescriptions)
            {
                JsonRequiredAttribute jsonRequiredAttribute = null;
                var type = parameterDescription.ModelMetadata?.ContainerType;

                if (type != null)
                {
                    jsonRequiredAttribute = type.GetProperty(parameterDescription.Name)?.GetCustomAttribute<JsonRequiredAttribute>();
                }

                if (jsonRequiredAttribute != null)
                {
                    var parameter = operation.Parameters.SingleOrDefault(p => p.Name == parameterDescription.Name);

                    if (parameter != null)
                    {
                        parameter.Required = true;
                    }
                }
            }
        }
    }
}