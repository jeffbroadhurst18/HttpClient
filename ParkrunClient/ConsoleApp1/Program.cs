using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Parkrun.Models;
using Parkrun.Services;
using Microsoft.Extensions.Options;
using System.IO;


namespace Parkrun
{
	class Program
	{
		static async Task Main(string[] args)
		{
			DefineConfiguration();
			RegisterServices();
			var month = Configuration.GetSection("Birthday").GetValue<string>("Month");
			Console.WriteLine($"Birthday in {month}");
			Console.WriteLine("Waiting for the service");
			Console.ReadLine();
			
			var test = ApplicationServices.GetRequiredService<SampleRequest>();
			var token = await test.Login();
			var parkruns = await test.ReadParkrunsAsync(token);
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

			Console.WriteLine("Now do PUT");
			var toUpdate = parkruns.OrderByDescending(c => c.Id).First();
			Console.WriteLine("Minutes was " + toUpdate.Minutes);
			toUpdate.Minutes += 1;
			var updated = await test.UpdateParkrunAsync(token, toUpdate);
			Console.WriteLine("Minutes is now " + updated.Minutes);
			Console.WriteLine("Now Delete");
			await test.DeleteParkrunAsync(token, updated.Id);
			Console.ReadLine();
		}

		private static void DefineConfiguration()
		{
			IConfigurationBuilder configBuilder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");
			Configuration = configBuilder.Build();
		}

		public static IConfiguration Configuration { get; set; }

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
