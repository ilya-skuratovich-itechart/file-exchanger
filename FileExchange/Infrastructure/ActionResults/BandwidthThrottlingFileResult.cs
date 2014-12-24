using System;
using System.IO;
using System.Threading;
using System.Web.Mvc;

namespace FileExchange.Infrastructure.ActionResults
{
    public class BandwidthThrottlingFileResult : FilePathResult
    {
        private int _limitSpeedKbps { get; set; }
        private string _originalFileName { get; set; }

        public BandwidthThrottlingFileResult(string filePath, string originalFileName, string contentType,
            int limitSpeedKbps)
            : base(filePath, contentType)
        {
            _limitSpeedKbps = limitSpeedKbps;
            _originalFileName = originalFileName;
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            int bufferSize = 1024 * _limitSpeedKbps;
            byte[] buffer = new byte[bufferSize];
            Stream outputStream = response.OutputStream;
            using (var stream = File.OpenRead(FileName))
            {
                response.AddHeader("Cache-control", "private");
                response.AddHeader("Content-Type", "application/octet-stream");
                response.AddHeader("Content-Length", stream.Length.ToString());
                response.AddHeader("Content-Disposition", String.Format("filename={0}", _originalFileName));
                response.Flush();
                while (true)
                {
                    if (!response.IsClientConnected)
                        break;
                    int bytesRead = stream.Read(buffer, 0, bufferSize);

                    if (bytesRead == 0)
                        break;

                    outputStream.Write(buffer, 0, bytesRead);
                    response.Flush();
                    Thread.Sleep(1000);
                }
            }
        }
    }
}