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

- **SyncSleep** (baseline): `Thread.Sleep` for sync comparison.
- **SimpleAsync**: single `await Task.Delay(DelayMs)`.
- **SimpleValueTaskAsync**: single `await` of a `ValueTask`-returning delay.
- **NestedAsync**: nested async calls (3 levels) with `Task.Delay` at each level.

`[Params(0, 1, 5)]` varies `DelayMs` (0, 1, 5 ms). `[MemoryDiagnoser]` reports allocations (Gen0, Allocated) so you can compare heap usage between runtime-async and classic.

## Comparing results

1. Run **AsyncBenchmark** (runtime-async=on), note **Mean** and **Allocated** for each method.
2. Run **AsyncBenchmark.Classic** with the same args, note the same columns.
3. Compare: early runtime-async benchmarks often show gains in nested cases (e.g. faster execution, less memory); your mileage may vary until more libraries are recompiled for runtime-async.

Run on hardware similar to production for realistic numbers.
