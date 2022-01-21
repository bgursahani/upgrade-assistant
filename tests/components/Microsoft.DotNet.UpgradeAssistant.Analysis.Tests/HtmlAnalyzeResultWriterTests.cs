﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Microsoft.DotNet.UpgradeAssistant.Analysis.Tests
{
    public class HtmlAnalyzeResultWriterTests
    {
        [Fact]
        public async Task ValidateHTML()
        {
            var serializer = new JsonSerializer();
            var writer = new HtmlAnalyzeResultWriter(serializer);

            var analyzeResults = new List<AnalyzeResult>
            {
                new AnalyzeResult
                {
                    FileLocation = "some-file-path",
                    LineNumber = 1,
                    ResultMessage = "some result message",
                    RuleId = "RULE0001",
                    RuleName = "RuleName0001"
                }
            };

            var analyzeResultMap = new List<AnalyzeResultDefinition>
            {
                new AnalyzeResultDefinition
                {
                    Name = "some-name",
                    Version = "1.0.0",
                    InformationUri = new Uri("https://github.com/dotnet/upgrade-assistant"),
                    AnalysisResults = analyzeResults.ToAsyncEnumerable()
                }
            };

            await writer.WriteAsync(analyzeResultMap.ToAsyncEnumerable(), "html", CancellationToken.None).ConfigureAwait(false);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "AnalysisReport.html");
            if (!File.Exists(filePath))
            {
                Assert.True(false, "File wasn't exported successfully.");
            }
        }
    }
}