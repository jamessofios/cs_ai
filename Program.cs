using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Program {

	public class ChatData {
		public string? model {get; set;}
		public List<Dictionary<string, string>>? messages {get; set;}

		public ChatData() {
			model = "gpt-3.5-turbo";
			messages = new List<Dictionary<string, string>>();
		}

	}

	class Program {
		static async Task Main(string[] args) {

			string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

			if (apiKey == null) {
				Console.WriteLine("Error: No API Key Found");
				return;
			}


			if (args.Length == 0) {
				Console.WriteLine("Error: Please enter a chat message");
				return;
			}

			StringBuilder builder = new StringBuilder();

			foreach (var arg in args) {
				builder.Append($"{arg} ");
			}

			string userPrompt = builder.ToString().TrimEnd();

			ChatData chatData = new ChatData();

			if (addChatMessage(chatData, "user", userPrompt) == null) {
				Console.WriteLine("Error: Could not add chat message");
				return;
			}

			string myJson = JsonSerializer.Serialize(chatData);


			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

			StringContent content = new StringContent(myJson, Encoding.UTF8, "application/json");

			HttpResponseMessage response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

			string responseBody = await response.Content.ReadAsStringAsync();

			Console.WriteLine(responseBody);

		}

		public static ChatData? addChatMessage(ChatData? chatData, string? role, string? content) {

			if (chatData == null || chatData.messages == null || role == null || content == null) {
				return null;
			}

			chatData.messages.Add(new Dictionary<string,string>());

			chatData.messages[chatData.messages.Count - 1].Add("role", role);
			chatData.messages[chatData.messages.Count - 1].Add("content", content);

			return chatData;
		}
	}
}
