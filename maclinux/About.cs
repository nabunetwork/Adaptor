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
namespace NabuAdaptorLinux
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Gtk;
    using NabuAdaptor;

    /// <summary>
    /// Form to show user about information
    /// </summary>
    public partial class About : Gtk.Dialog
    {
        /// <summary>
        /// The tag pages.
        /// </summary>
        IDictionary<TextTag, int> tag_pages = new Dictionary<TextTag, int>();

        /// <summary>
        /// The hand cursor.
        /// </summary>
        Gdk.Cursor handCursor, regularCursor;

        /// <summary>
        /// The hovering over link.
        /// </summary>
        bool hoveringOverLink = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:NabuAdaptorLinux.About"/> class.
        /// </summary>
        public About()
        {
            this.Build();
            this.BuildString();
            this.Title = $"NabuNetwork.com Internet Adapter (v{NabuAdaptor.Settings.majorVersion}.{NabuAdaptor.Settings.minorVersion})";
            handCursor = new Gdk.Cursor(Gdk.CursorType.Hand2);
            regularCursor = new Gdk.Cursor(Gdk.CursorType.Xterm);

        }

        /// <summary>
        /// Ons the button ok clicked.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected void OnButtonOkClicked(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Builds the string.
        /// </summary>
        private void BuildString()
        {
            textview1.Buffer.Text = "NABU Network Internet Adaptor\n\n";
            TextIter textIter = textview1.Buffer.EndIter;
            this.InsertLink(textview1.Buffer, ref textIter, "https://www.nabunetwork.com", 0);
            textview1.Buffer.Insert(ref textIter, "\n\n");
            textview1.Buffer.Insert(ref textIter, "A non-commercial enthusiast project supporting all things NABU PC maintained by Geek With Social Skills.\n\n");
            textview1.Buffer.Insert(ref textIter, "YouTube: ");
            this.InsertLink(textview1.Buffer, ref textIter, "https://www.youtube.com/user/geekwithsocialskills", 1);
            textview1.Buffer.Insert(ref textIter, "\n\n");
            textview1.Buffer.Insert(ref textIter, "NabuNetwork.com. software development team: ");
            this.InsertLink(textview1.Buffer, ref textIter, "https://www.nabunetwork.com/about-us", 2);
            textview1.Buffer.Insert(ref textIter, "\n\nSpecial thanks to:\n");
            textview1.Buffer.Insert(ref textIter, "Leo Binkowski for preserving the original NABU Network Cycle from 1984 and for DJ Sures for cracking the original NABU PAK code\n\n");
            textview1.Buffer.Insert(ref textIter, "Additional thanks to various members of the NABU open source community.\n\n");
            textview1.Buffer.Insert(ref textIter, "Remember: NABU Forever!");
        }

        /// <summary>
        /// Inserts the link.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="iter">Iter.</param>
        /// <param name="text">Text.</param>
        /// <param name="page">Page.</param>
        private void InsertLink(TextBuffer buffer, ref TextIter iter, string text, int page)
        {
            TextTag tag = new TextTag(null);
            tag.Foreground = "blue";
            tag.Underline = Pango.Underline.Single;
            tag_pages[tag] = page;
            buffer.TagTable.Add(tag);
            buffer.InsertWithTags(ref iter, text, tag);
        }

        /// <summary>
        /// Sets the cursor if appropriate.
        /// </summary>
        /// <param name="view">View.</param>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        void SetCursorIfAppropriate(TextView view, int x, int y)
        {
            bool hovering = false;
            TextIter iter = view.GetIterAtLocation(x, y);

            foreach (TextTag tag in iter.Tags)
            {
                hovering = true;
                break;
            }

            if (hovering != hoveringOverLink)
            {
                Gdk.Window window = view.GetWindow(Gtk.TextWindowType.Text);

                hoveringOverLink = hovering;
                if (hoveringOverLink)
                    window.Cursor = handCursor;
                else
                    window.Cursor = regularCursor;
            }
        }

        // Update the cursor image if the pointer moved.
        void MotionNotify(object sender, MotionNotifyEventArgs args)
        {
            TextView view = sender as TextView;
            int x, y;
            //Gdk.ModifierType state;

            view.WindowToBufferCoords(TextWindowType.Widget, (int)args.Event.X, (int)args.Event.Y, out x, out y);
            SetCursorIfAppropriate(view, x, y);
        }

        /// <summary>
        /// Events the after.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="args">Arguments.</param>
        void EventAfter(object sender, WidgetEventAfterArgs args)
        {
            if (args.Event.Type != Gdk.EventType.ButtonRelease)
                return;

            Gdk.EventButton evt = (Gdk.EventButton)args.Event;

            if (evt.Button != 1)
                return;

            TextView view = sender as TextView;
            TextIter start, end, iter;
            int x, y;

            // we shouldn't follow a link if the user has selected something
            view.Buffer.GetSelectionBounds(out start, out end);
            if (start.Offset != end.Offset)
                return;

            view.WindowToBufferCoords(TextWindowType.Widget, (int)evt.X, (int)evt.Y, out x, out y);
            iter = view.GetIterAtLocation(x, y);

            FollowIfLink(view, iter);
        }

        // Looks at all tags covering the position of iter in the text view,
        // and if one of them is a link, follow it by showing the page identified
        // by the data attached to it.
        void FollowIfLink(TextView view, TextIter iter)
        {
            if (iter.Tags.Any())
            {
                // get the tag
                int page = tag_pages[iter.Tags[0]];
                switch (page)
                {
                    case 0:
                        Process.Start("https://www.nabunetwork.com");
                        break;
                    case 1:
                        Process.Start("https://www.youtube.com/user/geekwithsocialskills");
                        break;
                    case 2:
                        Process.Start("https://www.nabunetwork.com/about-us");
                        break;
                }

            }
        }
    }
}


