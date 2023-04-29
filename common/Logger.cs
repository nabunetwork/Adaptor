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

    /// <summary>
    /// Logger class
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Log event handler - used for UI
        /// </summary>
        private EventHandler<string> logEvent;

        /// <summary>
        ///  Name of log file
        /// </summary>
        private const string logFile = "nabu.log";

        /// <summary>
        /// Destination for the log
        /// </summary>
        public enum Target
        {
            // output to file
            file,

            // output to console.
            console
        }

        /// <summary>
        /// Create an instance of the logger class with an event handler (used for UI scenarios)
        /// </summary>
        /// <param name="logEventHandler"></param>
        public Logger(EventHandler<string> logEventHandler)
        {
            this.logEvent = logEventHandler;
        }

        /// <summary>
        /// Create an instance of the logger class without an event handler
        /// </summary>
        public Logger()
        {
            this.logEvent = null;
        }

        /// <summary>
        /// Log method, right now, just outvdp_print to the console but we could do other things here like add a trace level, write to a file, etc..
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message, Target target)
        {
            switch (target)
            {
                case Target.console:
                    if (this.logEvent != null)
                    {
                        logEvent(this, message);
                    }
                    else
                    {
                        Console.WriteLine(message);
                    }

                    break;
                case Target.file:
                    if (this.logEvent != null)
                    {
                        logEvent(this, message);
                    }
                    else
                    {
                        System.IO.File.AppendAllText(logFile, message + System.Environment.NewLine);
                    }

                    break;
            }
        }
    }
}
