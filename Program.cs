/* James Sofios
 * 2024-06-01
 */

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text;

namespace chatGptApiExample {

	public class ChatData {
		public string? model {get; set;}
		public List<Dictionary<string, string>>? messages {get; set;}
	}

	class Program {
		static void Main(string[] args) {

			string? apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");

			if (apiKey == null) {
				Console.WriteLine("Error: No API Key Found");
				return;
			}


			if (args.Length == 0) {
				Console.WriteLine("Error: Please enter a chat message");
				return;
			}

			StringBuilder builder = new();

			foreach (var arg in args) {
				builder.Append($"{arg} ");
			}

			string userPrompt = builder.ToString().TrimEnd();

			ChatData chatData = new ChatData {
				model = "gpt-3.5-turbo",
				messages = new List<Dictionary<string, string>>()
			};


			addChatMessage(chatData, "user", userPrompt);
			addChatMessage(chatData, "system", userPrompt);

			string myJson = JsonSerializer.Serialize(chatData);
			Console.WriteLine(myJson);
		}

		public static void addChatMessage(ChatData? chatData, string? role, string? content) {

			if (chatData == null || role == null || content == null) {
				return;
			}

			chatData.messages.Add(new Dictionary<string,string>());

			chatData.messages[chatData.messages.Count - 1].Add("role", role);
			chatData.messages[chatData.messages.Count - 1].Add("content", content);

		}
	}
}
