// Copyright (c) 2018-2019, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Security.Permissions;
    using System.Text;
    using System.Threading;
    using System.Windows.Forms;

    // do not use this attribute for anything but classes, the assembly, or the Main() method.

    /// <summary>
    /// Attribute for creating MiniDumps.
    ///
    /// This registers Thread and Unhandled exception
    /// handlers to do it.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Assembly | AttributeTargets.Method)]
    public class MiniDumpAttribute : Attribute
    {
        private readonly string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="MiniDumpAttribute"/> class.
        /// </summary>
        /// <param name="text">Exception message text.</param>
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.ControlAppDomain)]
        public MiniDumpAttribute(string text)
        {
            this.text = text;
            var currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(this.ExceptionHandler);
            Application.ThreadException += new ThreadExceptionEventHandler(this.ThreadExceptionHandler);
        }

        /// <summary>
        /// Occurs when a mini-dump is generated.
        /// </summary>
        public static event EventHandler<MiniDumpEventArgs> DumpGenerated;

        /// <summary>
        /// Gets or sets the mini-dump type.
        /// </summary>
        public MinidumpTypes DumpType { get; set; }

        /// <summary>
        /// Gets or sets the title of the unhandled exception messagebox.
        /// </summary>
        public string ExceptionTitle { get; set; }

        /// <summary>
        /// Gets or sets the title of the unhandled thread exception messagebox.
        /// </summary>
        public string ThreadExceptionTitle { get; set; }

        private void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var e = (Exception)args.ExceptionObject;
            var exceptionData = $"{e.GetType()}: {e.Message}{Environment.NewLine}{e.StackTrace}{Environment.NewLine}";
            var outputData = Encoding.ASCII.GetBytes(exceptionData);

            // do not dump or close if in a debugger.
            if (!Debugger.IsAttached)
            {
                ForceClosure.ForceClose = true;
                using (var fileStream = File.OpenWrite(SettingsFile.ErrorLogPath))
                {
                    fileStream.Write(outputData, 0, outputData.Length);
                }

                MiniDump.MiniDumpToFile(SettingsFile.MiniDumpPath, this.DumpType);
                DumpGenerated?.Invoke(this, new MiniDumpEventArgs(string.Format(this.text, SettingsFile.ErrorLogPath), this.ExceptionTitle));
            }
        }

        private void ThreadExceptionHandler(object sender, ThreadExceptionEventArgs e)
        {
            var ex = e.Exception;
            var exceptionData = $"{ex.GetType()}: {ex.Message}{Environment.NewLine}{ex.StackTrace}{Environment.NewLine}";
            var outputData = Encoding.ASCII.GetBytes(exceptionData);

            // do not dump or close if in a debugger.
            if (!Debugger.IsAttached)
            {
                ForceClosure.ForceClose = true;
                using (var fileStream = File.OpenWrite(SettingsFile.ErrorLogPath))
                {
                    fileStream.Write(outputData, 0, outputData.Length);
                }

                MiniDump.MiniDumpToFile(SettingsFile.MiniDumpPath, this.DumpType);
                DumpGenerated?.Invoke(this, new MiniDumpEventArgs(string.Format(this.text, SettingsFile.ErrorLogPath), this.ThreadExceptionTitle));
            }
        }
    }
}
