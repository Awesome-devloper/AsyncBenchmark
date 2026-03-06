#!/usr/bin/env bash
# Quick benchmark run (~1–2 min). From repo root: ./run-benchmark.sh [AsyncBenchmark|AsyncBenchmark.Classic]
set -e
cd "$(dirname "$0")"
PROJECT="${1:-AsyncBenchmark}"
echo "Running $PROJECT (--job short, no menu)..."
dotnet run -c Release --project "$PROJECT" -- --filter "*AsyncBenchmarks*" --job short 0
