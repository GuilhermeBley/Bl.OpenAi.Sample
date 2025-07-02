var config = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
string model = config["ModelName"];
string key = config["OpenAIKey"];

// Create the IChatClient
IChatClient chatClient = new OpenAIClient(key).GetChatClient(model).AsIChatClient();