using Newtonsoft.Json;

namespace DifyAi.Dto.ResDto;

public class DifyGetApplicationInfoResDto
{
    [JsonProperty("opening_statement")] public string OpeningStatement { get; set; }

    [JsonProperty("suggested_questions_after_answer")] public DifyAppInfo_EnabledSetting SuggestedQuestionsAfterAnswer { get; set; }

    [JsonProperty("speech_to_text")] public DifyAppInfo_EnabledSetting SpeechToText { get; set; }

    [JsonProperty("retriever_resource")] public DifyAppInfo_EnabledSetting RetrieverResource { get; set; }

    [JsonProperty("annotation_reply")] public DifyAppInfo_EnabledSetting AnnotationReply { get; set; }

    [JsonProperty("user_input_form")] public List<DifyAppInfo_UserInputForm> UserInputForm { get; set; }

    [JsonProperty("file_upload")] public DifyAppInfo_FileUpload DifyAppInfoFileUpload { get; set; }

    [JsonProperty("system_parameters")] public DifyAppInfo_SystemParameters DifyAppInfoSystemParameters { get; set; }
}

public class DifyAppInfo_EnabledSetting
{
    [JsonProperty("enabled")] public bool Enabled { get; set; }
}

public class DifyAppInfo_UserInputForm
{
    [JsonProperty("paragraph")] public DifyAppInfo_Paragraph DifyAppInfoParagraph { get; set; }
}

public class DifyAppInfo_Paragraph
{
    [JsonProperty("label")] public string Label { get; set; }

    [JsonProperty("variable")] public string Variable { get; set; }

    [JsonProperty("required")] public bool Required { get; set; }

    [JsonProperty("default")] public string Default { get; set; }
}

public class DifyAppInfo_FileUpload
{
    [JsonProperty("image")] public DifyAppInfo_Image DifyAppInfoImage { get; set; }
}

public class DifyAppInfo_Image
{
    [JsonProperty("enabled")] public bool Enabled { get; set; }

    [JsonProperty("number_limits")] public int NumberLimits { get; set; }

    [JsonProperty("detail")] public string Detail { get; set; }

    [JsonProperty("transfer_methods")] public List<string> TransferMethods { get; set; }
}

public class DifyAppInfo_SystemParameters
{
    [JsonProperty("image_file_size_limit")] public string ImageFileSizeLimit { get; set; }
}