namespace Advanced.Samples.Performance;


using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using FluentAssertions;
using NUnit.Framework;

[MemoryDiagnoser(true)]
public class Benchmarks
{
    private const long num = long.MaxValue;

    [Benchmark]
    public void GetDigitsFromLeastSignificant_String()
    {
        num
            .ToString()
            .Select(x => Convert.ToByte(x.ToString()))
            .ToArray();
    }

    [Benchmark]
    public void GetDigitsFromLeastSignificant_MathWithSpan()
    {
        var result = new byte[20];
        var span = new Span<byte>(result);
        var n = num;
        var index = 0;
        while (n > 0)
        {
            span[index] = (byte)(n % 10);
            n /= 10;
            index++;
        }

        var res = span[..index].ToArray();
    }

    [Benchmark]
    public void GetDigitsFromLeastSignificant_MathWithList()
    {
        var result = new List<byte>();
        var n = num;
        while (n > 0)
        {
            result.Add((byte)(n % 10));
            n /= 10;
        }
    }

    [Benchmark]
    public void GetDigitsFromLeastSignificant_MathWithYield()
    {
        IEnumerable<byte> Inner()
        {
            var n = num;
            while (n > 0)
            {
                yield return (byte)(n % 10);
                n /= 10;
            }
        }

        Inner().ToArray();
    }
}

[TestFixture]
[Explicit]
public class BenchmarkTests
{
    [Test]
    public void GetDigitsFromLeastSignificant()
    {
        var config = ManualConfig
            .CreateMinimumViable()
            .WithOption(ConfigOptions.DisableOptimizationsValidator, true);

        BenchmarkRunner.Run<Benchmarks>(config);
    }
}