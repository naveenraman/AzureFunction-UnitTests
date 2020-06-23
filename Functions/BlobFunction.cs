using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Functions
{
    public static class BlobFunction
    {
        [FunctionName("BlobFunction")]
        public static void Run([BlobTrigger("samples-workitems/{name}")]Stream myBlob, string name, ILogger log)
        {
            log.LogInformation($"C# Blob trigger function Processed blob Name:{name}");
        }
    }
}
