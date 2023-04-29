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
    using System.Net;

    /// <summary>
    /// Class to load nabu files on the web
    /// </summary>
    public class WebLoader : ILoader
    {
        /// <summary>
        /// Try to get the contents of the nabu file located at the specified path
        /// </summary>
        /// <param name="path">Path to nabu file</param>
        /// <param name="data">contents of file</param>
        /// <returns>returns true/false if successful or not</returns>
        public bool TryGetData(string path, out byte[] data)
        {
            data = null;

            try
            {
                data = WebClientWrapper.DownloadData(path);
                return true;
            }
            catch (WebException)
            {
                return false;
            }
        }

        /// <summary>
        /// Try to get the parent directory of the specified file
        /// </summary>
        /// <param name="path">Path to get the parent directory</param>
        /// <param name="directoryPath">Parent directory path</param>
        /// <returns>returns true/false if successful or not</returns>
        public bool TryGetDirectory(string path, out string directoryPath)
        {
            directoryPath = string.Empty;

            try
            {
                if (path.ToLowerInvariant().EndsWith(".pak") || (path.ToLowerInvariant().EndsWith(".nabu")))
                {
                    Uri uri = new Uri(path);

                    directoryPath = string.Format("{0}://{1}", uri.Scheme, uri.Authority);

                    for (int i = 0; i < uri.Segments.Length - 1; i++)
                    {
                        directoryPath += uri.Segments[i];
                    }

                    directoryPath = directoryPath.Trim("/".ToCharArray());
                }
                else
                {
                    directoryPath = path.Trim("/".ToCharArray());
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
