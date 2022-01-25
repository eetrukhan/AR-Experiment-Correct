using System.ComponentModel;

namespace Logic
{
    public enum NotificationHeader
    {
        [Description("Очень важно!")]
        header1,    
        [Description("Тут может быть ваша реклама")]
        header2,    
        [Description("Пам-пам")]
        header3,
        [Description("Только сегодня!")]
        header4, 
        [Description("А вы знали?")]
        header5,    
        [Description("Ответ")]
        header6,
        [Description("Вопрос")]
        header7, 
        [Description("Внимание!")]
        header8,    
        [Description("Ты не поверишь!")]
        header9,
        [Description("Супер интересно")]
        header10, 
    }
}
