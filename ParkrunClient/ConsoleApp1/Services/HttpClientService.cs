using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
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

		private async Task<string> GetInternalAsync(string requestUri, string token = null)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			if (_objectDisposed) { throw new ObjectDisposedException(nameof(_httpClient)); }

			_httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			}

			HttpResponseMessage resp = await _httpClient.GetAsync(requestUri);
			LogInformation($"status from GET { resp.StatusCode}");
			resp.EnsureSuccessStatusCode();
			return await resp.Content.ReadAsStringAsync();
		}

		public async virtual Task<T> GetAsync(string requestUri, string token = null)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			string json = await GetInternalAsync(requestUri, token);
			return JsonConvert.DeserializeObject<T>(json);
		}

		public async virtual Task<IEnumerable<T>> GetAllAsync(string requestUri, string token = null)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			string json = await GetInternalAsync(requestUri, token);
			return JsonConvert.DeserializeObject<IEnumerable<T>>(json);
		}

		public async virtual Task<T> PostAsync(string requestUri, T item, string token = null)
		{
			if (requestUri == null) { throw new ArgumentNullException(nameof(requestUri)); }
			if (item == null) { throw new ArgumentNullException(nameof(item)); }

			string json = JsonConvert.SerializeObject(item);
			if (token != null)
			{
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
				json = "\"" + json.Replace("\"", "'") + "\"";
			}
			
			HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
			HttpResponseMessage resp = await _httpClient.PostAsync(requestUri, content);
			LogInformation($"status from POST { resp.StatusCode}");
			resp.EnsureSuccessStatusCode();
			LogInformation($"added resource at {resp.Headers.Location}");
			json = await resp.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(json);
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
