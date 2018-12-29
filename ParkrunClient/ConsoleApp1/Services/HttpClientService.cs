using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Parkrun.Services
{
	public class HttpClientService<T> : IDisposable where T : class
	{
		private HttpClient _httpClient;
		private readonly UrlService _urlService;
		private readonly ILogger<HttpClientService<T>> _logger;

		public HttpClientService(UrlService urlService, ILogger<HttpClientService<T>> logger)
		{
			_urlService = urlService ?? throw new ArgumentNullException(nameof(urlService));
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));

			_httpClient = new HttpClient();
			_httpClient.BaseAddress = new Uri(urlService.BaseAddress);
		}

		private async Task<string> GetInternalAsync(string requestUri)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			if (_objectDisposed) { throw new ObjectDisposedException(nameof(_httpClient)); }

			HttpResponseMessage resp = await _httpClient.GetAsync(requestUri);
			LogInformation($"status from GET { resp.StatusCode}");
			resp.EnsureSuccessStatusCode();
			return await resp.Content.ReadAsStringAsync();
		}

		public async virtual Task<T> GetAsync(string requestUri)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			string json = await GetInternalAsync(requestUri);
			return JsonConvert.DeserializeObject<T>(json);
		}

		public async virtual Task<IEnumerable<T>> GetAllAsync(string requestUri)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			string json = await GetInternalAsync(requestUri);
			return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
		}

		private void LogInformation(string message, [CallerMemberName] string callerName = null) =>
			_logger.LogInformation($"{nameof(HttpClientService<T>)}.{callerName}: message");

		#region IDisposable Support
		private bool _objectDisposed = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!_objectDisposed)
			{
				if (disposing)
				{
					_httpClient?.Dispose();
				}
				_objectDisposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
		}
		#endregion 
	}
}
