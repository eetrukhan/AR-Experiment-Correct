using System.ComponentModel;

namespace Logic
{
    public enum NotificationSource
    {
        [Description("Telegram")]
        Telegram,
        [Description("WhatsApp")]
        WhatsApp,
        [Description("Яндекс.Почта")]
        YandexPost,
        [Description("YouTube")]
        YouTube
    }
}
