using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Configs;

namespace AsyncBenchmark;

internal static class Program
{
    public static void Main(string[] args)
    {
        var config = ManualConfig.CreateEmpty()
            .AddJob(Job.InProcess)
            .WithOptions(ConfigOptions.DisableOptimizationsValidator)
            .AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray())
            .AddLogger(DefaultConfig.Instance.GetLoggers().ToArray())
            .AddDiagnoser(DefaultConfig.Instance.GetDiagnosers().ToArray());

        BenchmarkRunner.Run<AsyncBenchmarks>(config);
    }
}
