﻿// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

#nullable disable

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.DotNet.Cli;

namespace Microsoft.NET.Build.Tasks
{
    static partial class FileUtilities
    {
        public static Version GetFileVersion(string sourcePath)
        {
            if (sourcePath != null)
            {
                var fvi = FileVersionInfo.GetVersionInfo(sourcePath);

                if (fvi != null)
                {
                    return new Version(fvi.FileMajorPart, fvi.FileMinorPart, fvi.FileBuildPart, fvi.FilePrivatePart);
                }
            }

            return null;
        }

        static readonly HashSet<string> s_assemblyExtensions = new HashSet<string>(new[] { ".dll", ".exe", ".winmd" }, StringComparer.OrdinalIgnoreCase);
        public static Version TryGetAssemblyVersion(string sourcePath)
        {
            var extension = Path.GetExtension(sourcePath);

            return s_assemblyExtensions.Contains(extension) ? GetAssemblyVersion(sourcePath) : null;
        }

        [DllImport("libc", SetLastError = true)]
        public static extern int mkdir(string pathname, uint mode);
        public static string CreateTempPath()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                int dotnet_version = 7; // Changed in backported code. This can be out of date for .NET 8+ and still be fine.
                if (dotnet_version < 7)
                    mkdir(path, 007000);
                else
                    Directory.CreateDirectory(path, UnixFileMode.UserWrite | UnixFileMode.UserExecute | UnixFileMode.UserRead);
            }
            else
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }

        public static string CreateTempFile(string tempDirectory, string extension = "")
        {
            if (extension == "")
            {
                extension = Path.GetExtension(Path.GetRandomFileName());
            }
            string fileName = Path.ChangeExtension(Path.Combine(tempDirectory, Path.GetTempFileName()), extension);
            File.Create(fileName.ToString());
            return fileName;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns>The full path of a newly created temp file with OK permissions.</returns>
        public static string CreateTempFile()
        {
            return Path.GetTempFileName();
        }

    }
}
