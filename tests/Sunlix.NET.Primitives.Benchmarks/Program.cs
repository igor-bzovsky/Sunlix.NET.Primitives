// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using Sunlix.NET.Primitives.Benchmarks;

var config = ManualConfig
    .Create(DefaultConfig.Instance)
    .WithOptions(ConfigOptions.JoinSummary)
    .WithOptions(ConfigOptions.DisableLogFile);

var summary = BenchmarkRunner.Run(new[] {
            typeof(SunlixEnumerationBenchmarks),
            typeof(MicrosoftEnumerationBenchmarks)
        }, config);
