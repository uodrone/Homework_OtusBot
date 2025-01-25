using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static Telegram.Bot.TelegramBotClient;

namespace MyOtusBot
{
    internal class Program
    {
        private static string _botToken = "7716203098:AAHBqQWCPqLa5eGDeQ91ICZN8zA_s-NU1m0";
        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient(_botToken);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                DropPendingUpdates = true
            };
            var handler = new UpdateHandler();
            
            

            try
            {
                botClient.StartReceiving(handler, receiverOptions);
                handler.OnHandleUpdateStarted += Handler_OnHandleUpdateStarted;
                handler.OnHandleUpdateCompleted += Handler_OnHandleUpdateCompleted;
                var me = await botClient.GetMe();
                Console.WriteLine($"{me.FirstName} запущен!");
            }
            finally
            {
                // Отписываемся от событий
                handler.OnHandleUpdateStarted -= Handler_OnHandleUpdateStarted;
                handler.OnHandleUpdateCompleted -= Handler_OnHandleUpdateCompleted;
            }


            await Task.Delay(-1);
        }

        private static void Handler_OnHandleUpdateCompleted(string message)
        {
            Console.WriteLine($"Закончилась обработка сообщения {message}");
        }

        private static void Handler_OnHandleUpdateStarted(string message)
        {
            Console.WriteLine($"Началась обработка сообщения {message}");
        }
    }
}
