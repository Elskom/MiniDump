// Copyright (c) 2018-2019, Els_kom org.
// https://github.com/Elskom/
// All rights reserved.
// license: MIT, see LICENSE for more details.

namespace Elskom.Generic.Libs
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Windows Native Methods.
    /// </summary>
    internal static class SafeNativeMethods
    {
        /// <summary>
        /// Gets the current thread.
        /// </summary>
        /// <returns>The current native thread id.</returns>
        [DllImport("kernel32.dll", EntryPoint = "GetCurrentThreadId", ExactSpelling = true)]
        internal static extern uint GetCurrentThreadId();

        /// <summary>
        /// Writes the process execution information to a mini-dump for debugging.
        /// </summary>
        /// <param name="hProcess">The process handle.</param>
        /// <param name="ProcessId">The process ID.</param>
        /// <param name="hFile">The file handle to dump to.</param>
        /// <param name="DumpType">The type of mini-dump.</param>
        /// <param name="ExceptionParam">The exception information stuff.</param>
        /// <param name="UserStreamParam">User stream stuff for the dumps.</param>
        /// <param name="CallackParam">Callback function pointer?.</param>
        /// <returns>if the mini-dump worked or not?.</returns>
        [DllImport("dbghelp.dll", EntryPoint = "MiniDumpWriteDump", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        internal static extern bool MiniDumpWriteDump(IntPtr hProcess, int ProcessId, SafeHandle hFile, MINIDUMP_TYPE DumpType, ref MINIDUMP_EXCEPTION_INFORMATION ExceptionParam, IntPtr UserStreamParam, IntPtr CallackParam);
    }
}
