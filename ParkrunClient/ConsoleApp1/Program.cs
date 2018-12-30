using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parkrun.Models;
using Parkrun.Services;

namespace Parkrun
{
	class Program
	{
		static async Task Main(string[] args)
		{
			Console.WriteLine("Waiting for the service");
			Console.ReadLine();
			RegisterServices();
			var test = ApplicationServices.GetRequiredService<SampleRequest>();
			var token = await test.Login();
			await test.ReadParkrunsAsync(token);
			Console.WriteLine();
			await test.ReadParkrunAsync(token, 1);
			Console.WriteLine();
			ParkrunModel parkrun = new ParkrunModel
			{
				Race = 289,
				RaceDate = new DateTime(2018, 12, 22),
				Grade = "50.92",
				Minutes = 28,
				Seconds = 58,
				Position = 231
			};
			await test.AddParkrunAsync(token, parkrun);

			Console.ReadLine();
		}

		public static void RegisterServices()
		{
			var services = new ServiceCollection();
			services.AddSingleton<UrlService>();
			services.AddSingleton<ParkrunClientService>();
			services.AddSingleton<UserClientService>();
			services.AddTransient<SampleRequest>();
			services.AddLogging(logger => { logger.AddConsole(); });

			ApplicationServices = services.BuildServiceProvider();
		}

		public static IServiceProvider ApplicationServices { get; private set; }
	}
}
