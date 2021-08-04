using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace dotNetCore5WebAPI_0323.data.spatial
{
    public class GeometryJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Geometry_);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if(objectType == typeof(Geometry_))
                return new Geometry_() { Json = existingValue.ToString() };
            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if(value is Geometry_)
            {
                writer.WriteRawValue((value as Geometry_).Json);
            }
        }
    }
}
