using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FileExchange.Infrastructure.CustomHttpContent
{
    public class JsonHttpContent : HttpContent
    {
        private readonly JToken _value;

        public JsonHttpContent(string value)
        {
            _value = JObject.Parse(value);
            Headers.ContentType = new MediaTypeHeaderValue("application/json")
            {
                CharSet = "UTF-8"
            };
        }

        protected override Task SerializeToStreamAsync(Stream stream,
         TransportContext context)
        {
            var jw = new JsonTextWriter(new StreamWriter(stream))
            {
                Formatting = Formatting.Indented
            };
            _value.WriteTo(jw);
            jw.Flush();
            return Task.FromResult<object>(null);
        }

        protected override bool TryComputeLength(out long length)
        {
            length = -1;
            return false;
        }
    }
}