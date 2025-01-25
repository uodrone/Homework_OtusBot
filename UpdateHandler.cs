using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
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
                string message = update.Message.Text;
                // Вызываем событие начала обработки
                OnHandleUpdateStarted?.Invoke(message);
                Console.WriteLine("Сообщение успешно принято");

                if (message == "/cat")
                {
                    var client = new HttpClient();
                    var catFact = await client.GetFromJsonAsync<CatFact>("https://catfact.ninja/fact", cancellationToken);
                    Console.WriteLine($"Занимательный факт о котах и кошках:\n{catFact?.Fact}");

                    await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Занимательный факт о котах и кошках: {catFact.Fact}",
                    cancellationToken: cancellationToken);
                }
                await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: $"Эхо: {update.Message.Text}",
                    cancellationToken: cancellationToken);
                // Вызываем событие завершения обработки
                OnHandleUpdateCompleted?.Invoke(message);

                //Если передана отмена
                cancellationToken.ThrowIfCancellationRequested();
            }            
        }
    }
}
