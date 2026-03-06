using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

namespace AsyncBenchmark;

internal static class Program
{
    public static void Main(string[] args)
    {
        // InProcess only — no CsProj toolchain, so .NET 11 (NotRecognized) is never validated
        var config = ManualConfig.CreateEmpty()
            .AddJob(Job.InProcess)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
            .AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray())
            .AddLogger(DefaultConfig.Instance.GetLoggers().ToArray())
            .AddDiagnoser(DefaultConfig.Instance.GetDiagnosers().ToArray());

        // Run AsyncBenchmarks directly with our config (avoids switcher merging in default job)
        BenchmarkRunner.Run<AsyncBenchmarks>(config);
    }
}
