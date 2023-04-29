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
    /// Serializable class to encapsulate all the info we need to categorize the cycles and load
    /// </summary>
    [Serializable()]   
    public class Target
    {
        /// <summary>
        /// Target Enum
        /// </summary>
        public enum TargetEnum
        {
            NabuNetwork,
            Homebrew,
            Gameroom
        }

        /// <summary>
        /// Gets or sets the TargetType
        /// </summary>
        public TargetEnum TargetType { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Url(Path)
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Target"/> class.
        /// </summary>
        public Target()
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="Target"/> class.
        /// </summary>
        /// <param name="name">Name of the Program</param>
        /// <param name="url">The Path/Url to the program</param>
        /// <param name="targetType">Target Type</param>
        public Target(string name, string url, TargetEnum targetType)
        {
            this.Name = name;
            this.Url = url;
            this.TargetType = targetType;
        }

        /// <summary>
        /// Override the ToString() on this class
        /// </summary>
        /// <returns>returns Name as the value</returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
