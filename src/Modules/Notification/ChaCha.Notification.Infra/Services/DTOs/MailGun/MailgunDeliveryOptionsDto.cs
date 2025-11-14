namespace ChaCha.Notification.Infra.Services.DTOs.MailGun;

public class MailgunDeliveryOptionsDto
{
    public string? DeliveryTimeRfc2822 { get; init; }
    public string? DeliveryTimeOptimizePeriod { get; init; }
    public string? TimeZoneLocalize { get; init; }
    public bool? TestMode { get; init; }
    public EMailgunTriStateHtmlOnly? TrackingClicks { get; init; }
    public EMailgunTriState? TrackingOpens { get; init; }
    public EMailgunTriState? Tracking { get; init; }
    public EMailgunTriState? RequireTls { get; init; }
    public EMailgunTriState? SkipVerification { get; init; }
    public EMailgunTriState? Dkim { get; init; }
    public string? SecondaryDkim { get; init; }
    public string? SecondaryDkimPublic { get; init; }
    public string? SendingIp { get; init; }
    public string? SendingIpPool { get; init; }
    public bool? TrackingPixelLocationTop { get; init; }
    public string? ArchiveTo { get; init; }
    public string? SuppressHeaders
    {
        get; init;
    }
}