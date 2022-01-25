using System.ComponentModel;

namespace Logic
{
    public enum NotificationImage
    {
        [Description("telegram")]
        Telegram,
        [Description("whatsapp")]
        WhatsApp,
        [Description("yandexpost")]
        YandexPost,
        [Description("youtube")]
        YouTube,
        [Description("_silent_")]
        Silent
    }
}

