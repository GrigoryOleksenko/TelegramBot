using System;
using HtmlAgilityPack;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;


namespace TelegramBot
{
    public class Program
    {
        public static string downloads;
        
        public static void Main(string[] args)
        {
            var bot = new TelegramBotClient("1475689242:AAGwqxkOankZt1BvXqOPGXzty3fvOa6Zh9U");

            bot.OnMessage += TelegramBotClient_OnMessage;
            bot.OnMessageEdited += TelegramBotClient_OnMessage;
            bot.OnCallbackQuery += TelegramBotClient_OnCallbackQuery;

            bot.StartReceiving();

            Console.ReadLine();

            bot.StopReceiving();
        }

        static void Parse()
        {
            do
            {
                downloads = null;
                Random random = new Random();
                int n = random.Next(0, 50);
                string a = "//*[@id=" + n +"]/div[1]";
                var web = new HtmlWeb();
                HtmlDocument doc = web.Load("https://www.anekdot.ru/random/anekdot/");
                try
                {
                    downloads = doc.DocumentNode.SelectSingleNode(a).InnerText.Trim();
                }
                catch 
                {
                    continue;
                }
                
            } while (downloads == null);
        }

       private static async void TelegramBotClient_OnCallbackQuery(object sender, Telegram.Bot.Args.CallbackQueryEventArgs e)
        {
            var bot = (TelegramBotClient)sender;
            var message = e.CallbackQuery;
            Parse();
            await bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, downloads);
            var inlineKeyboard = new InlineKeyboardMarkup(
                InlineKeyboardButton.WithCallbackData("Да!")
            );

            await bot.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, "Еще?", replyMarkup: inlineKeyboard);
        }

        private static async void TelegramBotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var bot = (TelegramBotClient)sender;
            var message = e.Message;

            if (message.Text == "/start")
            {
                var inlineKeyboard = new InlineKeyboardMarkup(
                        InlineKeyboardButton.WithCallbackData("Да!")
                    );

                    await bot.SendTextMessageAsync(e.Message.Chat.Id, "Привет. Хочешь расскажу анекдот?", replyMarkup: inlineKeyboard);
            }
            else
            {
                return;
            }
        }
    }
}
