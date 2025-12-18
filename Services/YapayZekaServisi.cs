using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using FitnessCenterReservationSystem.ViewModels;

namespace FitnessCenterReservationSystem.Services
{
	public class YapayZekaServisi
	{
		private readonly HttpClient _httpClient;
		private readonly string _apiKey;

		public YapayZekaServisi(IConfiguration config, IHttpClientFactory httpFactory)
		{
			_httpClient = httpFactory.CreateClient();
			_apiKey = config["OpenAI:ApiKey"] ?? throw new ArgumentNullException("OpenAI API key bulunamadı");
			_httpClient.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", _apiKey);
		}

		public async Task<string> EgzersizVeBeslenmeOnerisi(KullaniciBilgiViewModel model)
		{
			string prompt = $@"
Kullanıcı bilgileri:
- Boy: {model.Boy} cm
- Kilo: {model.Kilo} kg
- Doğum Tarihi: {model.DogumTarihi?.ToString("yyyy-MM-dd")}
- Hedef: {model.Hedef}

Ona uygun haftalık egzersiz ve beslenme planı hazırla. 
Basit anlaşılır olsun, örnek egzersizler ve öğünler ver. 300 kelimeyi geçmesin.
";

			var requestBody = new
			{
				model = "gpt-3.5-turbo",
				messages = new[]
				{
					new { role = "user", content = prompt }
				},
				max_tokens = 300,
				temperature = 0.7
			};

			string json = JsonSerializer.Serialize(requestBody);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			int retry = 0;
			while (retry < 3)
			{
				var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

				if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
				{
					retry++;
					await Task.Delay(2000); // 2 saniye bekle
					continue;
				}

				response.EnsureSuccessStatusCode();

				var responseContent = await response.Content.ReadAsStringAsync();

				using var doc = JsonDocument.Parse(responseContent);
				var mesaj = doc.RootElement
					.GetProperty("choices")[0]
					.GetProperty("message")
					.GetProperty("content")
					.GetString();

				return mesaj ?? "AI'den geçerli bir yanıt alınamadı.";
			}

			return "AI servisine ulaşılamıyor. Lütfen daha sonra tekrar deneyin.";
		}
	}
}
