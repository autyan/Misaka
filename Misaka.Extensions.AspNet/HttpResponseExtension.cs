using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Misaka.Extensions.AspNet
{
    public static class HttpResponseExtension
    {
        public static async Task<string> GetResponseBodyStringAsync(this HttpResponse response)
        {
            string bodyString = null;
            if (response.Body.CanSeek)
            {
                var position = response.Body.Position;
                response.Body.Seek(0, SeekOrigin.Begin);
                var streamReader = new StreamReader(response.Body);
                response.RegisterForDispose(streamReader);
                bodyString = await streamReader.ReadToEndAsync();
                response.Body.Seek(position, SeekOrigin.Begin);
            }
            return bodyString;
        }

        public static HttpResponse EnableRewind(this HttpResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException(nameof(response));
            }
            var body = response.Body;
            if (!body.CanSeek)
            {
                var responseBodyStream = new WriteSyncMemoryStream(body);
                response.Body = responseBodyStream;
                response.HttpContext.Response.RegisterForDispose(responseBodyStream);
            }
            return response;
        }
    }
}