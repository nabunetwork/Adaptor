// Copyright(c) 2022 NabuNetwork.com
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
namespace NabuAdaptor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class to wrap the web client
    /// </summary>
    public static class WebClientWrapper
    {
        /// <summary>
        /// WebClient to download segment files from cloud
        /// </summary>
        private static WebClient webClient;

        /// <summary>
        /// Static constructor - set the global headers and SSL/TLS settings
        /// </summary>
        static WebClientWrapper()
        {
            webClient = new WebClient();
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            webClient.Headers.Add("user-agent", "Nabu Network Adapter 1.0");
            webClient.Headers.Add("Content-Type", "application/octet-stream");
            webClient.Headers.Add("Content-Transfer-Encoding", "binary");
        }

        /// <summary>
        /// Download the specified URL as a byte array
        /// </summary>
        /// <param name="url">URL to download</param>
        /// <returns>contents as bytes</returns>
        public static byte[] DownloadData(string url)
        {
            return webClient.DownloadData(url);
        }

        /// <summary>
        /// Download the specified URL as a string
        /// </summary>
        /// <param name="url">URL to download</param>
        /// <returns>contents as string</returns>
        public static string DownloadString(string url)
        {
            return webClient.DownloadString(url);
        }
    }
}
