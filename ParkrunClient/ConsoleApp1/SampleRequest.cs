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
		private readonly UserClientService _userClient;

		public SampleRequest(UrlService urlService, ParkrunClientService client,UserClientService userClient)
		{
			_urlService = urlService;
			_client = client;
			_userClient = userClient;
		}

		public async Task ReadParkrunAsync(string token, int race)
		{
			Console.WriteLine(nameof(ReadParkrunAsync));
			ParkrunModel parkrun = await _client.GetAsync(_urlService.ParkrunApi + race.ToString(), token);
			Console.WriteLine(parkrun.Id + " " + parkrun.Race + " " + parkrun.RaceDate + " " + parkrun.Position + " " + parkrun.Minutes + ":" + parkrun.Seconds + " " + parkrun.Grade);
		}

		public async Task AddParkrunAsync(string token, ParkrunModel parkrun)
		{
			Console.WriteLine(nameof(AddParkrunAsync));
			ParkrunModel retrieved = await _client.PostAsync(_urlService.ParkrunApi, parkrun, token);
		}

		public async Task<ParkrunModel> UpdateParkrunAsync(string token, ParkrunModel parkrun)
		{
			Console.WriteLine(nameof(UpdateParkrunAsync));
			ParkrunModel retrieved = await _client.PutAsync(_urlService.ParkrunApi, parkrun, token);
			return retrieved;
		}

		public async Task DeleteParkrunAsync(string token,int id)
		{
			Console.WriteLine(nameof(DeleteParkrunAsync));
			await _client.DeleteAsync(_urlService.ParkrunApi + id.ToString());
		}

		public async Task<IEnumerable<ParkrunModel>> ReadParkrunsAsync(string token)
		{
			Console.WriteLine(nameof(ReadParkrunsAsync));
			IEnumerable<ParkrunModel> parkruns = await _client.GetAllAsync(_urlService.ParkrunApi,token);
			foreach(var parkrun in parkruns)
			{
				Console.WriteLine(parkrun.Id + " " + parkrun.Race + " " + parkrun.RaceDate + " " + parkrun.Position + " " + parkrun.Minutes + ":" + parkrun.Seconds + " " + parkrun.Grade);
			}
			return parkruns;
		}

		public async Task<string> Login()
		{
			UserModel user = new UserModel
			{
				UserName = "Jeff",
				Password = "Pass4Jeff"
			};
			Console.WriteLine(nameof(Login));
			UserModel retrieved = await  _userClient.PostAsync(_urlService.UserApi, user);
			Console.WriteLine("Token: " + retrieved.Token);
			return retrieved.Token;
		}
	}
}
