// Copyright (c) 2018-2019, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;

    /// <summary>
    /// A class that can generate mini-dumps.
    /// </summary>
    public static class MiniDump
    {
        /// <summary>
        /// Occurs when a mini-dump fails with any sort of error code.
        /// </summary>
        public static event EventHandler<MiniDumpEventArgs> DumpFailed;

        internal static void MiniDumpToFile(string fileToDump, MinidumpTypes dumpType)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var fsToDump = File.Open(fileToDump, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                using (var thisProcess = Process.GetCurrentProcess())
                {
                    var mINIDUMP_EXCEPTION_INFORMATION = new MINIDUMP_EXCEPTION_INFORMATION
                    {
                        ClientPointers = false,
                        ExceptionPointers = Marshal.GetExceptionPointers(),
                        ThreadId = SafeNativeMethods.GetCurrentThreadId(),
                    };
                    var error = SafeNativeMethods.MiniDumpWriteDump(
                        thisProcess.Handle,
                        thisProcess.Id,
                        fsToDump.SafeFileHandle,
                        dumpType,
                        ref mINIDUMP_EXCEPTION_INFORMATION,
                        IntPtr.Zero,
                        IntPtr.Zero);
                    if (error > 0)
                    {
                        DumpFailed?.Invoke(typeof(MiniDump), new MiniDumpEventArgs($"Mini-dumping failed with Code: {error}", "Error!"));
                    }
                }
            }
        }
    }
}
