using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parkrun.Services;

namespace Parkrun
{
	class Program
	{
		static async Task Main()
		{
			Console.WriteLine("Hello World!");
		}

		public static void ConfigureServices()
		{
			var services = new ServiceCollection();
			services.AddSingleton<UrlService>();
			services.AddSingleton<ParkrunClientService>();
			services.AddTransient<SampleRequest>();
			services.AddLogging(logger => { logger.AddConsole(); });

			ApplicationServices = services.BuildServiceProvider();
		}

		public static IServiceProvider ApplicationServices { get; private set; }
	}
}
