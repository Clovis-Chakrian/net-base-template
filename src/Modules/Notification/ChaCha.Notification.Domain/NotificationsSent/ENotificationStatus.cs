using System.ComponentModel;

namespace ChaCha.Notification.Domain.NotificationsSent;

/// <summary>
/// Representa os possíveis estados do ciclo de vida de uma notificação.
/// </summary>
/// <remarks>
/// Use esses valores para rastrear e controlar o processamento, a entrega e a interação do usuário com notificações.
/// </remarks>
public enum ENotificationStatus
{
  /// <summary>
  /// Notificação criada e enfileirada, ainda não enviada.
  /// </summary>
  [Description("PENDING")]
  Pending,

  /// <summary>
  /// Notificação entregue ao mecanismo de envio.
  /// </summary>
  [Description("SENT")]
  Sent,

  /// <summary>
  /// Tentativa de envio falhou; pode exigir novas tentativas ou tratamento de erro.
  /// </summary>
  [Description("FAILED")]
  Failed,

  /// <summary>
  /// Notificação alcançou com sucesso o dispositivo ou caixa de entrada do destinatário.
  /// </summary>
  [Description("DELIVERED")]
  Delivered,

  /// <summary>
  /// Destinatário abriu ou reconheceu a notificação.
  /// </summary>
  [Description("READ")]
  Read,

  /// <summary>
  /// Notificação cancelada antes de ser enviada ou entregue.
  /// </summary>
  [Description("CANCELLED")]
  Cancelled
}