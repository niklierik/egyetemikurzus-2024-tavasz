using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Text.Json;

using Shell.Domain;

namespace Shell.Infrastructure
{
    internal class DagattMacskaSerializer
    {
        public void SerializeToXml(Stream target, DagattMacska instance)
        {
            var serializer = new XmlSerializer(typeof(DagattMacska));
            serializer.Serialize(target, instance);
        }

        public void SerializeToJson(Stream target, DagattMacska instance)
        {
            JsonSerializer.Serialize(target, instance, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
