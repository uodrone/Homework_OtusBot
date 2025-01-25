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
        private static string _botToken;
        static async Task Main(string[] args)
        {
            string filePath = "tg_bot_token.txt";
            if (File.Exists(filePath))
            {
                string tgtoken = File.ReadAllText(filePath);
                _botToken = tgtoken;
            }

            var botClient = new TelegramBotClient(_botToken);
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = [UpdateType.Message],
                DropPendingUpdates = true
            };
            var handler = new UpdateHandler();
            var cts = new CancellationTokenSource();
            var token = cts.Token;

            try
            {                
                handler.OnHandleUpdateStarted += Handler_OnHandleUpdateStarted;
                handler.OnHandleUpdateCompleted += Handler_OnHandleUpdateCompleted;
                botClient.StartReceiving(handler, receiverOptions, token);
                var me = await botClient.GetMe();
                Console.WriteLine($"{me.FirstName} запущен!");

                CloseBot(me, cts);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Бот всё, закрыт, совсем закрыт");
            }
            finally
            {
                // Отписываемся от событий
                handler.OnHandleUpdateStarted -= Handler_OnHandleUpdateStarted;
                handler.OnHandleUpdateCompleted -= Handler_OnHandleUpdateCompleted;
            }
            
        }

        private static void Handler_OnHandleUpdateCompleted(string message)
        {
            Console.WriteLine($"Закончилась обработка сообщения {message}");            
        }

        private static void Handler_OnHandleUpdateStarted(string message)
        {
            Console.WriteLine($"Началась обработка сообщения {message}");
        }

        private static void CloseBot (User? me, CancellationTokenSource cts)
        {
            Console.WriteLine("Нажмите клавишу A для выхода");
            ConsoleKeyInfo symbol;
            do
            {
                symbol = Console.ReadKey();
                if (symbol.Key == ConsoleKey.A)
                {
                    cts.Cancel();
                    Console.WriteLine("\nБот закрыт");
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine($"Информация о боте.\nНазвание бота {me.FirstName}\nСсылка на бот {me.Username}");
                }
            } while (symbol.Key != ConsoleKey.A);
        }
    }
}
