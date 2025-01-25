using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MyOtusBot
{
    internal class UpdateHandler : IUpdateHandler
    {
        public delegate void MessageHandler(string message);
        // События, которые будут вызываться в начале и конце обработки сообщения
        public event MessageHandler OnHandleUpdateStarted;
        public event MessageHandler OnHandleUpdateCompleted;
        public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type == UpdateType.Message)
            {
                // Вызываем событие начала обработки
                OnHandleUpdateStarted?.Invoke(update.Message.Text);
                Console.WriteLine("Сообщение успешно принято");
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: update.Message.Text,
                    cancellationToken: cancellationToken);
                // Вызываем событие завершения обработки
                OnHandleUpdateCompleted?.Invoke(update.Message.Text);
            }            
        }
    }
}
