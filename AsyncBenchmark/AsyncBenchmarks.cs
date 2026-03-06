using BenchmarkDotNet.Attributes;

namespace AsyncBenchmark;

[MemoryDiagnoser]
public class AsyncBenchmarks
{
    [Params(0, 1, 5)]
    public int DelayMs { get; set; }

    [Benchmark(Baseline = true)]
    public async Task SimpleAsync()
    {
        if (DelayMs > 0)
            await Task.Delay(DelayMs);
    }

    [Benchmark]
    public async Task NestedAsyncImpl()
    {
        await NestedAsyncImplCore(3);
    }

    private async Task NestedAsyncImplCore(int depth)
    {
        if (depth <= 0) return;
        if (DelayMs > 0)
            await Task.Delay(DelayMs);
        await NestedAsyncImplCore(depth - 1);
    }
}
