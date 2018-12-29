using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Parkrun.Models;

namespace Parkrun.Services
{
	public class ParkrunClientService : HttpClientService<ParkrunModel>
	{
		public ParkrunClientService(UrlService urlService, ILogger<HttpClientService<ParkrunModel>> logger) : base(urlService, logger)
		{
		}

		public override async Task<IEnumerable<ParkrunModel>> GetAllAsync(string requestUri)
		{
			IEnumerable<ParkrunModel> parkruns = await base.GetAllAsync(requestUri);
			return parkruns.OrderBy(p => p.Id);
		}
	}
}
