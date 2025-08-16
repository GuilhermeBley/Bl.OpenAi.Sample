namespace Bl.OpenAi.Sample.ContentSafety;

public enum MediaType
{
    Text = 0,
    Image = 1
}

// Enumeration for categories
public enum Category
{
    Hate = 0,
    SelfHarm = 1,
    Sexual = 2,
    Violence = 3
}

// Enumeration for actions
public enum Action
{
    Accept = 0,
    Reject = 1
}

/// <summary>
/// Exception raised when there is an error in detecting the content.
/// </summary>
public class DetectionException : Exception
{
    public string Code { get; set; }

    /// <summary>
    /// Constructor for the DetectionException class.
    /// </summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public DetectionException(string code, string message) : base(message)
    {
        Code = code;
    }
}

/// <summary>
/// Class representing the decision made by the content moderation system.
/// </summary>
public class Decision
{
    public Action SuggestedAction { get; set; }
    public Dictionary<Category, Action> ActionByCategory { get; set; }

    /// <summary>
    /// Constructor for the Decision class.
    /// </summary>
    /// <param name="suggestedAction">The overall action suggested by the system.</param>
    /// <param name="actionByCategory">The actions suggested by the system for each category.</param>
    public Decision(Action suggestedAction, Dictionary<Category, Action> actionByCategory)
    {
        SuggestedAction = suggestedAction;
        ActionByCategory = actionByCategory;
    }
}

/// <summary>
/// Base class for detection requests.
/// </summary>
public class DetectionRequest
{
}

/// <summary>
/// Class representing an image.
/// </summary>
public class Image
{
    public string Content { get; set; }

    /// <summary>
    /// Constructor for the Image class.
    /// </summary>
    /// <param name="content">The base64-encoded content of the image.</param>
    public Image(string content)
    {
        Content = content;
    }
}

/// <summary>
/// Class representing an image detection request.
/// </summary>
public class ImageDetectionRequest : DetectionRequest
{
    public Image Image { get; set; }

    /// <summary>
    /// Constructor for the ImageDetectionRequest class.
    /// </summary>
    /// <param name="content">The base64-encoded content of the image.</param>
    public ImageDetectionRequest(string content)
    {
        Image = new Image(content);
    }
}

/// <summary>
/// Class representing a text detection request.
/// </summary>
public class TextDetectionRequest : DetectionRequest
{
    public string Text { get; set; }
    public string[] BlocklistNames { get; set; }

    /// <summary>
    /// Constructor for the TextDetectionRequest class.
    /// </summary>
    /// <param name="text">The text to be detected.</param>
    /// <param name="blocklistNames">The names of the blocklists to use for detecting the text.</param>
    public TextDetectionRequest(string text, string[] blocklistNames)
    {
        Text = text;
        BlocklistNames = blocklistNames;
    }
}

/// <summary>
/// Class representing a detailed detection result for a specific category.
/// </summary>
public class CategoriesAnalysis
{
    /// <summary>
    /// The category of the detection result.
    /// </summary>
    public string? Category { get; set; }
    /// <summary>
    /// The severity of the detection result.
    /// </summary>
    public int? Severity { get; set; }
}

/// <summary>
/// Base class for detection result.
/// </summary>
public class DetectionResult
{
    /// <summary>
    /// The detailed result for categories analysis.
    /// </summary>
    public List<CategoriesAnalysis>? CategoriesAnalysis { get; set; }
}

/// <summary>
/// Class representing an image detection result.
/// </summary>
public class ImageDetectionResult : DetectionResult
{

}

/// <summary>
/// Class representing a detailed detection result for a blocklist match.
/// </summary>
public class BlocklistsMatch
{
    /// <summary>
    /// The name of the blocklist.
    /// </summary>
    public string? BlocklistName { get; set; }
    /// <summary>
    /// The ID of the block item that matched.
    /// </summary>
    public string? BlocklistItemId { get; set; }
    /// <summary>
    /// The text of the block item that matched.
    /// </summary>
    public string? BlocklistItemText { get; set; }
}

/// <summary>
/// Class representing a text detection result.
/// </summary>
public class TextDetectionResult : DetectionResult
{
    /// <summary>
    /// The list of detailed results for blocklist matches.
    /// </summary>
    public List<BlocklistsMatch>? BlocklistsMatch { get; set; }
}

/// <summary>
/// Class representing a detection error response.
/// </summary>
public class DetectionErrorResponse
{
    /// <summary>
    /// The detection error.
    /// </summary>
    public DetectionError? error { get; set; }
}

/// <summary>
/// Class representing a detection error.
/// </summary>
public class DetectionError
{
    /// <summary>
    /// The error code.
    /// </summary>
    public string? code { get; set; }
    /// <summary>
    /// The error message.
    /// </summary>
    public string? message { get; set; }
    /// <summary>
    /// The error target.
    /// </summary>
    public string? target { get; set; }
    /// <summary>
    /// The error details.
    /// </summary>
    public string[]? details { get; set; }
    /// <summary>
    /// The inner error.
    /// </summary>
    public DetectionInnerError? innererror { get; set; }
}

/// <summary>
/// Class representing a detection inner error.
/// </summary>
public class DetectionInnerError
{
    /// <summary>
    /// The inner error code.
    /// </summary>
    public string? code { get; set; }
    /// <summary>
    /// The inner error message.
    /// </summary>
    public string? innererror { get; set; }
}