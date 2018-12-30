using System;
using System.Collections.Generic;
using System.Text;

namespace Parkrun.Services
{
	public class UrlService
	{
		public string BaseAddress => "https://localhost:7272/";
		public string ParkrunApi => "api/parkrun/";
		public string UserApi => "api/users/authenticate";
	}
}
