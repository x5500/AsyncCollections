using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Diagnostics.Windows;
using BenchmarkDotNet.Jobs;

namespace HellBrick.AsyncCollections.Benchmark
{
	internal class BenchmarkConfig : ManualConfig
	{
		public BenchmarkConfig()
		{
			AddJob( Job.RyuJitX64.WithLaunchCount( 1 ) );
			AddDiagnoser( new MemoryDiagnoser(new MemoryDiagnoserConfig()) );
		}
	}
}
