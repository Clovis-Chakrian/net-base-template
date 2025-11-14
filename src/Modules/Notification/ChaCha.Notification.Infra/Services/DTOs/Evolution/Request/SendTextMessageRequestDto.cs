using System.Text.Json.Serialization;

namespace ChaCha.Notification.Infra.Services.DTOs.Evolution.Request;

public class SendTextMessageRequestDto
{
    [JsonPropertyName("number")]
    public string Number { get; private set; }

    [JsonPropertyName("text")]
    public string Text { get; private set; }

    [JsonPropertyName("delay")]
    public int? Delay { get; private set; }

    [JsonPropertyName("linkPreview")]
    public bool? LinkPreview { get; private set; }

    [JsonPropertyName("mentionsEveryOne")]
    public bool? MentionsEveryOne { get; private set; }

    [JsonPropertyName("mentioned")]
    public List<string>? Mentioned { get; private set; }

    [JsonPropertyName("quoted")]
    public QuotedMessageDto? Quoted { get; private set; }

    private SendTextMessageRequestDto() { }

    public static SendTextMessageRequestBuilder Builder =>
        new SendTextMessageRequestBuilder();

    public class SendTextMessageRequestBuilder
    {
        private readonly SendTextMessageRequestDto _instance = new();

        public SendTextMessageRequestBuilder WithNumber(string number)
        {
            _instance.Number = number;
            return this;
        }

        public SendTextMessageRequestBuilder WithText(string text)
        {
            _instance.Text = text;
            return this;
        }

        public SendTextMessageRequestBuilder WithDelay(int delay)
        {
            _instance.Delay = delay;
            return this;
        }

        public SendTextMessageRequestBuilder WithLinkPreview(bool enabled)
        {
            _instance.LinkPreview = enabled;
            return this;
        }

        public SendTextMessageRequestBuilder MentionEveryOne(bool enabled)
        {
            _instance.MentionsEveryOne = enabled;
            return this;
        }

        public SendTextMessageRequestBuilder AddMention(string number)
        {
            _instance.Mentioned ??= new List<string>();
            _instance.Mentioned.Add(number);
            return this;
        }

        public SendTextMessageRequestBuilder AddMentions(IEnumerable<string> numbers)
        {
            _instance.Mentioned ??= new List<string>();
            _instance.Mentioned.AddRange(numbers);
            return this;
        }

        public SendTextMessageRequestBuilder WithQuoted(QuotedMessageDto quoted)
        {
            _instance.Quoted = quoted;
            return this;
        }

        public SendTextMessageRequestDto Build()
        {
            if (string.IsNullOrWhiteSpace(_instance.Number))
                throw new InvalidOperationException("Number é obrigatório.");
            if (string.IsNullOrWhiteSpace(_instance.Text))
                throw new InvalidOperationException("Text é obrigatório.");

            return _instance;
        }
    }
}