# Async Benchmark (runtime-async vs classic)

BenchmarkDotNet project to compare **.NET 11 runtime-async** vs **classic compiler** async behavior (throughput and allocations).

## Projects

| Project | Feature | Purpose |
|--------|---------|--------|
| **AsyncBenchmark** | `runtime-async=on` | Run with runtime-async (new .NET 11 behavior) |
| **AsyncBenchmark.Classic** | (no Features line) | Run with classic compiler async behavior |

## Requirements

- .NET 11 SDK (preview)
- For running benchmarks: BenchmarkDotNet 0.15.x does not yet recognize .NET 11 runtime (`GetRuntimeVersion not implemented for NotRecognized`). Either:
  - **Option A:** Temporarily change both `.csproj` files to `<TargetFramework>net8.0</TargetFramework>` and remove `<EnablePreviewFeatures>` / `<Features>`, then run and compare. Revert to `net11.0` when BDN adds support.
  - **Option B:** Wait for a BenchmarkDotNet release that supports .NET 11, then run as below.

## How to run

**Easiest — one command (quick run, ~1–2 min):**

Open **Terminal**, go to the project folder, then paste and run:

```bash
cd /Users/aref/RiderProjects/AsyncBenchmark
dotnet run -c Release --project AsyncBenchmark -- --filter "*AsyncBenchmarks*" --job short 0
```

The `0` selects the benchmark so you don’t get a menu. When it finishes, you’ll see a results table.

**Or use the script (same folder):**
```bash
cd /Users/aref/RiderProjects/AsyncBenchmark
chmod +x run-benchmark.sh
./run-benchmark.sh
```
(Runtime-async run. For classic: `./run-benchmark.sh AsyncBenchmark.Classic`)

**Full run (no `--job short`, ~6+ min):**
```bash
cd /Users/aref/RiderProjects/AsyncBenchmark
dotnet run -c Release --project AsyncBenchmark -- --filter "*AsyncBenchmarks*" 0
```

**If you see a menu** (“Select the target benchmark”), type `0` and press Enter.

## What gets measured

- **SimpleAsync** (baseline): single `await Task.Delay(DelayMs)`.
- **NestedAsyncImpl**: 3 levels of nested async, each with `await Task.Delay(DelayMs)`.

`[Params(0, 1, 5)]` varies `DelayMs` (0, 1, 5 ms). `[MemoryDiagnoser]` reports **Gen0** and **Allocated** so you can compare heap usage between runtime-async and classic.

---

## Results

**Summary** (BenchmarkDotNet v0.15.8)  
**OS:** macOS Tahoe 26.3 (25D125) [Darwin 25.3.0] · **Hardware:** Apple M3, 1 CPU, 8 logical and 8 physical cores  
**.NET:** 11.0.0 (11.0.0-preview.1.26104.118), Arm64 RyuJIT armv8.0-a · **Job:** InProcess, Toolchain=InProcessEmitToolchain

| Method          | DelayMs | Mean              | Error          | StdDev         | Ratio | RatioSD | Allocated | Alloc Ratio |
|---------------- |--------|------------------:|---------------:|---------------:|------:|--------:|----------:|------------:|
| SimpleAsync     | 0      |          4.020 ns |      0.0080 ns |      0.0071 ns |  1.00 |    0.00 |         - |          NA |
| NestedAsyncImpl | 0      |         16.103 ns |      0.0216 ns |      0.0202 ns |  4.01 |    0.01 |         - |          NA |
| SimpleAsync     | 1      |  1,209,332.764 ns |  7,532.4966 ns |  7,045.9020 ns |  1.00 |    0.01 |     581 B |        1.00 |
| NestedAsyncImpl | 1      |  3,589,936.875 ns | 19,727.6058 ns | 18,453.2148 ns |  2.97 |    0.02 |    1344 B |        2.31 |
| SimpleAsync     | 5      |  5,676,772.460 ns | 32,431.5515 ns | 28,749.7257 ns |  1.00 |    0.01 |     578 B |        1.00 |
| NestedAsyncImpl | 5      | 16,996,254.248 ns | 81,274.7596 ns | 76,024.4606 ns |  2.99 |    0.02 |    1388 B |        2.40 |

**Takeaway:** At DelayMs=0, NestedAsyncImpl is ~4× slower (16 ns vs 4 ns) with no allocation. At 1 ms and 5 ms delay, NestedAsyncImpl is ~3× slower and allocates ~2.3× more (e.g. 1344 B vs 581 B at 1 ms). Run **AsyncBenchmark.Classic** with the same command to compare runtime-async vs classic.



---

## Comparing results

1. Run **AsyncBenchmark** (runtime-async=on), note **Mean** and **Allocated** for each method.
2. Run **AsyncBenchmark.Classic** with the same args, note the same columns.
3. Compare: early runtime-async benchmarks often show gains in nested cases (e.g. faster execution, less memory); your mileage may vary until more libraries are recompiled for runtime-async.

Run on hardware similar to production for realistic numbers.
