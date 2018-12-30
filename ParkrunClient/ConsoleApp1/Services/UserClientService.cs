using Microsoft.Extensions.Logging;
using Parkrun.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Parkrun.Services
{
	public class UserClientService : HttpClientService<UserModel>
	{
		public UserClientService(UrlService urlService, ILogger<HttpClientService<UserModel>> logger) : base(urlService, logger)
		{
		}

		public async override Task<UserModel> PostAsync(string requestUri, UserModel user, string token = null)
		{
			UserModel returnedUser = await base.PostAsync(requestUri, user);
			return returnedUser;
		}
	}
}
