using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Net.Http;
using Microsoft.Build.Utilities;
using Microsoft.DotNet.Cli.Build.Framework;
using Microsoft.DotNet.InternalAbstractions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using static Microsoft.DotNet.Cli.Build.Framework.BuildHelpers;

namespace Microsoft.DotNet.Cli.Build
{
    public class CleanPublishOutput : Task
    {
        public string Path { get; set; }

        public string Name { get; set; }

        public bool DeleteRuntimeConfigJson { get; set; }

        public bool DeleteDepsJson { get; set; }

        public override bool Execute()
        {
            PublishMutationUtilties.CleanPublishOutput(Path, Name, DeleteRuntimeConfigJson, DeleteDepsJson);

            return true;
        }
    }
}
