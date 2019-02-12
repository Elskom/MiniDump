// Copyright (c) 2018-2019, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;

    /// <summary>
    /// Holds the message text and title to pass to the event.
    /// </summary>
    public class MiniDumpEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MiniDumpEventArgs"/> class.
        /// </summary>
        /// <param name="text">The message text to pass to the event.</param>
        /// <param name="caption">The message title to pass to the event.</param>
        public MiniDumpEventArgs(string text, string caption)
        {
            this.Text = text;
            this.Caption = caption;
        }

        /// <summary>
        /// Gets the message text passed to the event.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Gets the message title passed to the event.
        /// </summary>
        public string Caption { get; private set; }
    }
}
