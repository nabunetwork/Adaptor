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
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Class to encapsulate all info necessary to fire an event for updating a progress bar when downloading a nabu segment
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Current segment number
        /// </summary>
        private readonly int curr;

        /// <summary>
        /// Total number of segments
        /// </summary>
        private readonly int max;

        /// <summary>
        /// Create an instance of the ProgressEventArgs class
        /// </summary>
        /// <param name="segment">Segment name</param>
        /// <param name="curr">current segment</param>
        /// <param name="max">Max number of segments</param>
        public ProgressEventArgs(int segment, int curr, int max)
        {
            this.Segment = segment;
            this.curr = curr;
            this.max = max;
        }

        /// <summary>
        /// Get/Set the segment number
        /// </summary>
        public int Segment
        {
            get; set;
        }

        /// <summary>
        /// Gets the maximum segment number
        /// </summary>
        public int Max
        {
            get { return this.max; }
        }

        /// <summary>
        /// Gets the current segment number
        /// </summary>
        public int Curr
        {
            get { return this.curr; }
        }
    }
}
