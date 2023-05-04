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
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Main entry point
    /// </summary>
    class Program
    {
        /// <summary>
        /// Nabu server
        /// </summary>
        /// <param name="args">command line arguments</param>
        static void Main(string[] args)
        {
            // Get the settings from the command line.
            Settings settings = new Settings(args);

            // Create the server
            Server server = new Server(settings);

            if (settings.IgnoreCertificateErrors)
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            }

            CancellationTokenSource source = new CancellationTokenSource();

            // Run the server
            do
            {
                Task task = Task.Run(() => server.RunServer(source.Token));
                Console.ReadLine();

                do
                {
                    task.Wait(1000);
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.Enter:
                                source.Cancel();
                                System.Environment.Exit(0);
                                break;
                        }
                    }
                    if (task.IsCompleted || task.IsCanceled)
                    {
                        break;
                    }
                } while (true);

            } while (true);
        }
    }
}