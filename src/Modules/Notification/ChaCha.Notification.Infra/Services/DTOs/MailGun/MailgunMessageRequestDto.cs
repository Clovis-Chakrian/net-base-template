using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ChaCha.Notification.Infra.Services.DTOs.MailGun;

public class MailGunMessageRequestDto
{
    // --- Essenciais/endereçamento
    public string From { get; init; } = default!;
    public IReadOnlyList<string> To { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Cc { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> Bcc { get; init; } = Array.Empty<string>();
    public string? Subject { get; init; }

    // --- Conteúdo
    public string? Text { get; init; }
    public string? Html { get; init; }
    public string? AmpHtml { get; init; }

    // --- Template
    public string? Template { get; init; }
    public string? TemplateVersion { get; init; }     // t:version
    public bool? TemplateGenerateTextPart { get; init; } // t:text
    public IReadOnlyDictionary<string, object> TemplateVariables { get; init; } = new Dictionary<string, object>();

    // --- Tags / variáveis
    public IReadOnlyList<string> Tags { get; init; } = Array.Empty<string>(); // o:tag
    public IReadOnlyDictionary<string, object> CustomVariables { get; init; } = new Dictionary<string, object>(); // v:*
    public IReadOnlyDictionary<string, Dictionary<string, object>> RecipientVariables { get; init; } = new Dictionary<string, Dictionary<string, object>>();

    // --- Headers h:*
    public IReadOnlyDictionary<string, string> Headers { get; init; } = new Dictionary<string, string>();

    // --- Opções o:*
    public MailgunDeliveryOptionsDto? DeliveryOptions { get; init; }

    // --- Anexos
    public IReadOnlyList<MailgunAttachmentDto> Attachments { get; init; } = Array.Empty<MailgunAttachmentDto>();
    public IReadOnlyList<MailgunAttachmentDto> InlineAttachments { get; init; } = Array.Empty<MailgunAttachmentDto>();

    private MailGunMessageRequestDto() { }

    // ----------- BUILDER -----------
    public sealed class Builder
    {
        private string? _from;
        private readonly List<string> _to = new();
        private readonly List<string> _cc = new();
        private readonly List<string> _bcc = new();

        private string? _subject;
        private string? _text;
        private string? _html;
        private string? _ampHtml;

        private string? _template;
        private string? _templateVersion;
        private bool? _templateGenerateTextPart;
        private readonly Dictionary<string, object> _templateVars = new();

        private readonly List<string> _tags = new();
        private readonly Dictionary<string, object> _customVars = new();
        private readonly Dictionary<string, Dictionary<string, object>> _recipientVars = new();

        private readonly Dictionary<string, string> _headers = new();

        private MailgunDeliveryOptionsDto? _delivery;

        private readonly List<MailgunAttachmentDto> _attachments = new();
        private readonly List<MailgunAttachmentDto> _inline = new();

        public Builder From(string from) { _from = from; return this; }
        public Builder To(params string[] to) { _to.AddRange(to.Where(s => !string.IsNullOrWhiteSpace(s))); return this; }
        public Builder Cc(params string[] cc) { _cc.AddRange(cc.Where(s => !string.IsNullOrWhiteSpace(s))); return this; }
        public Builder Bcc(params string[] bcc) { _bcc.AddRange(bcc.Where(s => !string.IsNullOrWhiteSpace(s))); return this; }

        public Builder Subject(string subject) { _subject = subject; return this; }
        public Builder Text(string text) { _text = text; return this; }
        public Builder Html(string html) { _html = html; return this; }
        public Builder AmpHtml(string ampHtml) { _ampHtml = ampHtml; return this; }

        public Builder UseTemplate(string template, string? version = null, bool? generateTextPart = null)
        { _template = template; _templateVersion = version; _templateGenerateTextPart = generateTextPart; return this; }

        public Builder TemplateVar(string key, object value) { _templateVars[key] = value; return this; }
        public Builder TemplateVars(IDictionary<string, object> values) { foreach (var kv in values) _templateVars[kv.Key] = kv.Value; return this; }

        public Builder Tag(params string[] tags) { _tags.AddRange(tags.Where(t => !string.IsNullOrWhiteSpace(t))); return this; }
        public Builder Header(string name, string value) { _headers[name] = value; return this; }
        public Builder Headers(IDictionary<string, string> headers) { foreach (var kv in headers) _headers[kv.Key] = kv.Value; return this; }

        public Builder CustomVar(string key, object value) { _customVars[key] = value; return this; }
        public Builder CustomVars(IDictionary<string, object> values) { foreach (var kv in values) _customVars[kv.Key] = kv.Value; return this; }

        public Builder RecipientVars(string email, IDictionary<string, object> vars)
        {
            if (!_recipientVars.TryGetValue(email, out var dict))
            {
                dict = new Dictionary<string, object>();
                _recipientVars[email] = dict.ToDictionary(k => k.Key, v => v.Value); // clone
            }
            foreach (var kv in vars) _recipientVars[email][kv.Key] = kv.Value;
            return this;
        }

        public Builder Delivery(MailgunDeliveryOptionsDto options) { _delivery = options; return this; }

        public Builder Attach(params MailgunAttachmentDto[] attachments) { _attachments.AddRange(attachments); return this; }
        public Builder AttachInline(params MailgunAttachmentDto[] attachments) { _inline.AddRange(attachments); return this; }

        public MailGunMessageRequestDto Build()
        {
            if (string.IsNullOrWhiteSpace(_from)) throw new InvalidOperationException("From é obrigatório.");
            if (_to.Count == 0 && _cc.Count == 0 && _bcc.Count == 0) throw new InvalidOperationException("Pelo menos um destinatário (To/Cc/Bcc) é obrigatório.");

            if (_template is null && string.IsNullOrWhiteSpace(_text) && string.IsNullOrWhiteSpace(_html) && string.IsNullOrWhiteSpace(_ampHtml))
                throw new InvalidOperationException("É necessário informar Text, Html, AmpHtml ou Template.");

            return new MailGunMessageRequestDto
            {
                From = _from!,
                To = _to.ToArray(),
                Cc = _cc.ToArray(),
                Bcc = _bcc.ToArray(),
                Subject = _subject,
                Text = _text,
                Html = _html,
                AmpHtml = _ampHtml,
                Template = _template,
                TemplateVersion = _templateVersion,
                TemplateGenerateTextPart = _templateGenerateTextPart,
                TemplateVariables = new Dictionary<string, object>(_templateVars),
                Tags = _tags.ToArray(),
                CustomVariables = new Dictionary<string, object>(_customVars),
                RecipientVariables = _recipientVars.ToDictionary(k => k.Key, v => new Dictionary<string, object>(v.Value)),
                Headers = new Dictionary<string, string>(_headers),
                DeliveryOptions = _delivery,
                Attachments = _attachments.ToArray(),
                InlineAttachments = _inline.ToArray()
            };
        }
    }

    // ----------- Helper: converte para MultipartFormDataContent -----------
    public MultipartFormDataContent ToMultipartFormData()
    {
        var mp = new MultipartFormDataContent();

        // básicos:
        mp.Add(new StringContent(From), "from");
        foreach (var t in To) mp.Add(new StringContent(t), "to");
        foreach (var c in Cc) mp.Add(new StringContent(c), "cc");
        foreach (var b in Bcc) mp.Add(new StringContent(b), "bcc");

        if (!string.IsNullOrWhiteSpace(Subject)) mp.Add(new StringContent(Subject), "subject");
        if (!string.IsNullOrWhiteSpace(Text)) mp.Add(new StringContent(Text), "text");
        if (!string.IsNullOrWhiteSpace(Html)) mp.Add(new StringContent(Html), "html");
        if (!string.IsNullOrWhiteSpace(AmpHtml)) mp.Add(new StringContent(AmpHtml), "amp-html");

        // template:
        if (!string.IsNullOrWhiteSpace(Template)) mp.Add(new StringContent(Template), "template");
        if (!string.IsNullOrWhiteSpace(TemplateVersion)) mp.Add(new StringContent(TemplateVersion), "t:version");
        if (TemplateGenerateTextPart.HasValue) mp.Add(new StringContent(TemplateGenerateTextPart.Value ? "yes" : "no"), "t:text");
        if (TemplateVariables is { Count: > 0 })
        {
            var json = JsonSerializer.Serialize(TemplateVariables);
            mp.Add(new StringContent(json, Encoding.UTF8, "application/json"), "t:variables");
        }

        // tags:
        foreach (var tag in Tags) mp.Add(new StringContent(tag), "o:tag");

        // v:* custom vars:
        foreach (var kv in CustomVariables)
        {
            var json = JsonSerializer.Serialize(kv.Value);
            mp.Add(new StringContent(json, Encoding.UTF8, "application/json"), $"v:{kv.Key}");
        }

        // recipient-variables:
        if (RecipientVariables is { Count: > 0 })
        {
            var json = JsonSerializer.Serialize(RecipientVariables);
            mp.Add(new StringContent(json, Encoding.UTF8, "application/json"), "recipient-variables");
        }

        // headers h:*
        foreach (var h in Headers)
            mp.Add(new StringContent(h.Value), $"h:{h.Key}");

        // o:* delivery options
        if (DeliveryOptions is not null)
        {
            void addIf(string name, string? value) { if (!string.IsNullOrWhiteSpace(value)) mp.Add(new StringContent(value), name); }
            void addTri(string name, EMailgunTriState? v) { if (v.HasValue) mp.Add(new StringContent(ToMailgunFlag(v.Value)), name); }
            void addTriClicks(string name, EMailgunTriStateHtmlOnly? v) { if (v.HasValue) mp.Add(new StringContent(ToMailgunFlag(v.Value)), name); }

            addIf("o:deliverytime", DeliveryOptions.DeliveryTimeRfc2822);
            addIf("o:deliverytime-optimize-period", DeliveryOptions.DeliveryTimeOptimizePeriod);
            addIf("o:time-zone-localize", DeliveryOptions.TimeZoneLocalize);
            if (DeliveryOptions.TestMode.HasValue) mp.Add(new StringContent(DeliveryOptions.TestMode.Value ? "yes" : "no"), "o:testmode");
            addTriClicks("o:tracking-clicks", DeliveryOptions.TrackingClicks);
            addTri("o:tracking-opens", DeliveryOptions.TrackingOpens);
            addTri("o:tracking", DeliveryOptions.Tracking);
            addTri("o:require-tls", DeliveryOptions.RequireTls);
            addTri("o:skip-verification", DeliveryOptions.SkipVerification);
            addTri("o:dkim", DeliveryOptions.Dkim);
            addIf("o:secondary-dkim", DeliveryOptions.SecondaryDkim);
            addIf("o:secondary-dkim-public", DeliveryOptions.SecondaryDkimPublic);
            addIf("o:sending-ip", DeliveryOptions.SendingIp);
            addIf("o:sending-ip-pool", DeliveryOptions.SendingIpPool);
            if (DeliveryOptions.TrackingPixelLocationTop.HasValue)
                mp.Add(new StringContent(DeliveryOptions.TrackingPixelLocationTop.Value ? "yes" : "no"), "o:tracking-pixel-location-top");
            addIf("o:archive-to", DeliveryOptions.ArchiveTo);
            addIf("o:suppress-headers", DeliveryOptions.SuppressHeaders);
        }

        // anexos (attachment / inline)
        foreach (var att in Attachments)
        {
            var content = new ByteArrayContent(att.Content);
            if (!string.IsNullOrWhiteSpace(att.ContentType))
                content.Headers.ContentType = new MediaTypeHeaderValue(att.ContentType);
            mp.Add(content, "attachment", att.FileName);
        }

        foreach (var att in InlineAttachments)
        {
            var content = new ByteArrayContent(att.Content);
            if (!string.IsNullOrWhiteSpace(att.ContentType))
                content.Headers.ContentType = new MediaTypeHeaderValue(att.ContentType);
            // Mailgun usa "inline" como nome do campo; ContentId vira "Content-ID" no MIME
            if (!string.IsNullOrWhiteSpace(att.ContentId))
                content.Headers.Add("Content-ID", $"<{att.ContentId}>");
            mp.Add(content, "inline", att.FileName);
        }

        return mp;
    }

    private static string ToMailgunFlag(EMailgunTriState v) => v switch
    {
        EMailgunTriState.Yes => "yes",
        EMailgunTriState.No => "no",
        EMailgunTriState.True => "true",
        EMailgunTriState.False => "false",
        _ => "no"
    };

    private static string ToMailgunFlag(EMailgunTriStateHtmlOnly v) => v switch
    {
        EMailgunTriStateHtmlOnly.Yes => "yes",
        EMailgunTriStateHtmlOnly.No => "no",
        EMailgunTriStateHtmlOnly.True => "true",
        EMailgunTriStateHtmlOnly.False => "false",
        EMailgunTriStateHtmlOnly.HtmlOnly => "htmlonly",
        _ => "no"
    };
}