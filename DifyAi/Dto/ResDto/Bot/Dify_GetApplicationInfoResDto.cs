using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class Dify_GetApplicationInfoResDto
{
    [JsonProperty("opening_statement")] public string OpeningStatement { get; set; }

    [JsonProperty("suggested_questions_after_answer")]
    public Dify_AppInfo_EnabledSetting SuggestedQuestionsAfterAnswer { get; set; }

    [JsonProperty("speech_to_text")] public Dify_AppInfo_EnabledSetting SpeechToText { get; set; }

    [JsonProperty("retriever_resource")] public Dify_AppInfo_EnabledSetting RetrieverResource { get; set; }

    [JsonProperty("annotation_reply")] public Dify_AppInfo_EnabledSetting AnnotationReply { get; set; }

    [JsonProperty("user_input_form")] public List<Dify_AppInfo_UserInputForm> UserInputForm { get; set; }

    [JsonProperty("file_upload")] public Dify_AppInfo_FileUpload DifyAppInfoFileUpload { get; set; }

    [JsonProperty("system_parameters")] public Dify_AppInfo_SystemParameters DifyAppInfoSystemParameters { get; set; }
}

public class Dify_AppInfo_EnabledSetting
{
    [JsonProperty("enabled")] public bool Enabled { get; set; }
}

public class Dify_AppInfo_UserInputForm
{
    [JsonProperty("paragraph")] public Dify_AppInfo_Paragraph DifyAppInfoParagraph { get; set; }
}

public class Dify_AppInfo_Paragraph
{
    [JsonProperty("label")] public string Label { get; set; }

    [JsonProperty("variable")] public string Variable { get; set; }

    [JsonProperty("required")] public bool Required { get; set; }

    [JsonProperty("default")] public string Default { get; set; }
}

public class Dify_AppInfo_FileUpload
{
    [JsonProperty("image")] public Dify_AppInfo_Image DifyAppInfoImage { get; set; }
}

public class Dify_AppInfo_Image
{
    [JsonProperty("enabled")] public bool Enabled { get; set; }

    [JsonProperty("number_limits")] public int NumberLimits { get; set; }

    [JsonProperty("detail")] public string Detail { get; set; }

    [JsonProperty("transfer_methods")] public List<string> TransferMethods { get; set; }
}

public class Dify_AppInfo_SystemParameters
{
    [JsonProperty("image_file_size_limit")] public string ImageFileSizeLimit { get; set; }
}