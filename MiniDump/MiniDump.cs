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

    internal static class MiniDump
    {
        /// <summary>
        /// Creates a Mini-dump in the file specified.
        /// </summary>
        /// <param name="fileToDump">The file path to dump to.</param>
        internal static void MiniDumpToFile(string fileToDump)
        {
            if (RuntimeInformation.IsOSPlatform(
                OSPlatform.Windows))
            {
                // file does not exist until this line, but this throws a
                // "System.IO.IOException: The process cannot access the file
                // '%LocalAppData%\Els_kom-[Process ID].mdmp' because it is being used by another process."
                using (var fsToDump = new FileStream(fileToDump, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                using (var thisProcess = Process.GetCurrentProcess())
                {
                    var mINIDUMP_EXCEPTION_INFORMATION = new MINIDUMP_EXCEPTION_INFORMATION
                    {
                        ClientPointers = false,
                        ExceptionPointers = Marshal.GetExceptionPointers(),
                        ThreadId = SafeNativeMethods.GetCurrentThreadId(),
                    };
                    SafeNativeMethods.MiniDumpWriteDump(
                        thisProcess.Handle,
                        thisProcess.Id,
                        fsToDump.SafeFileHandle,
                        MINIDUMP_TYPE.Normal,
                        ref mINIDUMP_EXCEPTION_INFORMATION,
                        IntPtr.Zero,
                        IntPtr.Zero);
                    var error = Marshal.GetLastWin32Error();
                    if (error > 0)
                    {
                        MessageManager.ShowError(
                            $"Mini-dumping failed with Code: {error}",
                            "Error!",
                            Convert.ToBoolean(Convert.ToInt32(SettingsFile.Settingsxml?.TryRead("UseNotifications") != string.Empty ? SettingsFile.Settingsxml?.TryRead("UseNotifications") : "0")));
                    }
                }
            }
        }

        /// <summary>
        /// Creates a Full Mini-dump in the file specified.
        /// </summary>
        /// <param name="fileToDump">The file path to dump to.</param>
        internal static void FullMiniDumpToFile(string fileToDump)
        {
            if (RuntimeInformation.IsOSPlatform(
                OSPlatform.Windows))
            {
                // file does not exist until this line, but this throws a
                // "System.IO.IOException: The process cannot access the file
                // '%LocalAppData%\Els_kom-[Process ID].mdmp' because it is being used by another process."
                using (var fsToDump = new FileStream(fileToDump, FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
                using (var thisProcess = Process.GetCurrentProcess())
                {
                    var mINIDUMP_EXCEPTION_INFORMATION = new MINIDUMP_EXCEPTION_INFORMATION
                    {
                        ClientPointers = false,
                        ExceptionPointers = Marshal.GetExceptionPointers(),
                        ThreadId = SafeNativeMethods.GetCurrentThreadId(),
                    };
                    SafeNativeMethods.MiniDumpWriteDump(
                        thisProcess.Handle,
                        thisProcess.Id,
                        fsToDump.SafeFileHandle,
                        MINIDUMP_TYPE.WithDataSegs | MINIDUMP_TYPE.WithFullMemory | MINIDUMP_TYPE.WithProcessThreadData | MINIDUMP_TYPE.WithFullMemoryInfo | MINIDUMP_TYPE.WithThreadInfo | MINIDUMP_TYPE.WithCodeSegs,
                        ref mINIDUMP_EXCEPTION_INFORMATION,
                        IntPtr.Zero,
                        IntPtr.Zero);
                    var error = Marshal.GetLastWin32Error();
                    if (error > 0)
                    {
                        MessageManager.ShowError(
                            $"Mini-dumping failed with Code: {error}",
                            "Error!",
                            Convert.ToBoolean(Convert.ToInt32(SettingsFile.Settingsxml?.TryRead("UseNotifications") != string.Empty ? SettingsFile.Settingsxml?.TryRead("UseNotifications") : "0")));
                    }
                }
            }
        }
    }
}
