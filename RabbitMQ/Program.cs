using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using Newtonsoft.Json;


namespace RabbitMQ
{
    class Program
    {
        static void Main(string[] args)
        {

            var messageNotification = new Message
            {
                Content = "Hey, we've recently added a new news app. Click to get the news app for free!",
                Heading = "New App Just For You !",
                Link = "https://www.onelaunch.com/apps"
            };

            string CONNECTION_STRING = "host=localhost";

            // Message Object that is converted to JSON before sending
            //var messageNotification = new Message
            //{
            //    Content = "Click to learn more about your horoscope and with which horoscope are you compatible with.",
            //    Heading = "Aries are the best.",
            //    Link = "https://www.onelaunch.com/dashboard"
            //};
            String jsonObject = JsonConvert.SerializeObject(messageNotification);

            // Using EasyNetQ Library to connect to RabbitMQ service
            using var bus = RabbitHutch.CreateBus(CONNECTION_STRING);

            // Publish the message on the RabbitMQ service
            bus.PubSub.PublishAsync(jsonObject).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    Console.WriteLine("Completed");
                }

                if (task.IsFaulted)
                {
                    Console.WriteLine("\n\n");
                    Console.WriteLine(task.Exception);
                }
            });
        }
    }
}
