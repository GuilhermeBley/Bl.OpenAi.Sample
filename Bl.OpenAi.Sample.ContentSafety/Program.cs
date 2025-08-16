using Bl.OpenAi.Sample.ContentSafety;

string endpoint = "";
string subscriptionKey = "";

// Initialize the ContentSafety object
ContentSafetyApi contentSafety = new(endpoint, subscriptionKey);

// Set the media type and blocklists
MediaType mediaType = MediaType.Text;
string[] blocklists = { };

do
{
    // Set the content to be tested
    Console.WriteLine("Enter the content to be tested or type 'exit' to leave:");
    string? content = Console.ReadLine();

    if (content == "exit")
    {
        Console.WriteLine("Type any key to close the app.");
        Console.ReadKey();
        return;
    }

    // Detect content safety
    DetectionResult detectionResult = await contentSafety.Detect(mediaType, content ?? string.Empty, blocklists);

    // Set the reject thresholds for each category
    Dictionary<Category, int> rejectThresholds = new Dictionary<Category, int> {
    { Category.Hate, 4 },
    { Category.SelfHarm, 4 },
    { Category.Sexual, 4 },
    { Category.Violence, 4 }
};

    // Make a decision based on the detection result and reject thresholds
    Decision decisionResult = contentSafety.MakeDecision(detectionResult, rejectThresholds);

    Console.WriteLine($"Action: {decisionResult.SuggestedAction}.");
    Console.WriteLine($"Action details:");

    foreach ( var act in decisionResult.ActionByCategory)
    {
        Console.WriteLine($"{act.Key} - {act.Value}");
    }

} while (true);