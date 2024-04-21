using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace InvoicrInfrastructure.JSONSerializer
{
    public static class JsonSerializerOptionsDefault
    {
        public static JsonSerializerOptions GetOptions()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            jsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            return jsonSerializerOptions;
        }
    }
}
