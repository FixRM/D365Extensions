using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using D365Extensions.Tests;
using D365Extensions.Tests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365Extensions.Benchmark
{
    [MemoryDiagnoser]
    public class PEBenchmark
    {
        [Benchmark]
        public string LambdaRef() => ProperyExpression.GetName<CustomEntity>(e => e.Property_1);

        [Benchmark]
        public string LambdaVal() => ProperyExpression.GetName<CustomEntity>(e => e.Property_2);

        [Benchmark]
        public string ReflectionRef() => ProperyExpression.GetNameR<CustomEntity>(e => e.Property_1);

        [Benchmark]
        public string ReflectionVal() => ProperyExpression.GetNameR<CustomEntity>(e => e.Property_2);

        [Benchmark]
        public string ChacheRef() => ProperyExpression.GetNameC<CustomEntity>(e => e.Property_1);

        [Benchmark]
        public string ChacheVal() => ProperyExpression.GetNameC<CustomEntity>(e => e.Property_2);

        [Benchmark]
        public string Chache2Ref() => ProperyExpression.GetNameC2<CustomEntity>(e => e.Property_1);

        [Benchmark]
        public string Chache2Val() => ProperyExpression.GetNameC2<CustomEntity>(e => e.Property_2);
    }

    public class ToLowerBenchmark
    {
        [Benchmark]
        public string ToLowerInvariant() => "AccountNumber".ToLowerInvariant();

        [Benchmark]
        public string ToLower() => "AccountNumber".ToLower();
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PEBenchmark>();
            //var summary = BenchmarkRunner.Run<ToLowerBenchmark>();

            Console.ReadKey();
        }
    }
}
