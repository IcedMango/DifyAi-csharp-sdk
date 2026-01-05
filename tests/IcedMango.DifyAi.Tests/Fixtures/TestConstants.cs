namespace IcedMango.DifyAi.Tests.Fixtures;

/// <summary>
/// Test constants for unit tests
/// </summary>
public static class TestConstants
{
    // API Keys
    public const string ValidBotApiKey = "app-test-valid-bot-key";
    public const string ValidDatasetApiKey = "dataset-test-valid-dataset-key";
    public const string InvalidApiKey = "invalid-key";

    // Base URLs
    public const string DefaultBaseUrl = "https://api.dify.ai/v1/";
    public const string CustomBaseUrl = "https://custom.dify.ai/v1/";

    // Instance Names
    public const string BotInstanceName = "TestBot";
    public const string DatasetInstanceName = "TestDataset";
    public const string NonExistentInstanceName = "NonExistent";

    // Test User
    public const string TestUserId = "user-test-123";

    // Test Conversation
    public const string TestConversationId = "conv-test-456";

    // Test Message
    public const string TestMessageId = "msg-test-789";

    // Test Dataset
    public const string TestDatasetId = "dataset-test-001";

    // Test Document
    public const string TestDocumentId = "doc-test-002";

    // Test Segment
    public const string TestSegmentId = "seg-test-003";

    // Test Task
    public const string TestTaskId = "task-test-004";

    /// <summary>
    /// HTTP Client naming conventions
    /// </summary>
    public static class HttpClientNames
    {
        public static string Bot(string name) => $"DifyAi.Bot.{name}";
        public static string Dataset(string name) => $"DifyAi.Dataset.{name}";

        public const string TestBotClient = "DifyAi.Bot.TestBot";
        public const string TestDatasetClient = "DifyAi.Dataset.TestDataset";
    }

    /// <summary>
    /// Test file paths
    /// </summary>
    public static class TestFiles
    {
        public const string AudioMp3 = "TestData/Files/test_audio.mp3";
        public const string AudioWav = "TestData/Files/test_audio.wav";
        public const string DocumentTxt = "TestData/Files/test_document.txt";
        public const string DocumentPdf = "TestData/Files/test_document.pdf";
    }

    /// <summary>
    /// Mock response file paths
    /// </summary>
    public static class MockResponses
    {
        public const string ChatCompletion = "TestData/Responses/chat_completion.json";
        public const string ConversationList = "TestData/Responses/conversation_list.json";
        public const string DatasetList = "TestData/Responses/dataset_list.json";
        public const string DocumentList = "TestData/Responses/document_list.json";
        public const string ErrorResponse = "TestData/Responses/error_response.json";
        public const string SuccessResult = "TestData/Responses/success_result.json";
    }
}
