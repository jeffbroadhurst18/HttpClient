using Parkrun.Models;
using Parkrun.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parkrun
{
	public class SampleRequest
	{
		private readonly UrlService _urlService;
		private readonly ParkrunClientService _client;

		public SampleRequest(UrlService urlService, ParkrunClientService client)
		{
			_urlService = urlService;
			_client = client;
		}

		public async Task ReadParkrunsAsync()
		{
			Console.WriteLine(nameof(ReadParkrunsAsync));
			IEnumerable<ParkrunModel> parkruns = await _client.GetAllAsync(_urlService.ParkrunApi);
			foreach(var parkrun in parkruns)
			{
				Console.WriteLine(parkrun.Race + " " + parkrun.RaceDate);
			}
		}
	}
}
