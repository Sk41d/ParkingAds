﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EmailService
{
    class MailServiceProgram
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the MailService!");
            //RabbitMQRecieve rabbitMQ = new RabbitMQRecieve();
            while (true) ;
        }
    }

    public class RabbitMQRecieve
    {
        public RabbitMQRecieve()
        {
            var factory = new ConnectionFactory()
            {
                UserName = "guest",
                Password = "guest",
                HostName = "localhost"
            };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                string message = ""; 
                channel.QueueDeclare(queue: "TestQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                };
                channel.BasicConsume(queue: "TestQueue",
                                 autoAck: true,
                                 consumer: consumer);
                MailHandler.SentMail(message);
                Console.ReadLine();
            }


        }
    }

    public static class MailHandler
    {
        public static void SentMail(string message)
        {
            using (MailMessage mailMessage = new MailMessage())
            using (SmtpClient smtpClient = new SmtpClient())
            {
                mailMessage.To.Add(new MailAddress("carlsen57@gmail.com"));
                mailMessage.From = new MailAddress("risopew852@whecode.com");      //Create fake email and test with. 
                mailMessage.Subject = "From the mailService!";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = message; 
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.EnableSsl = true;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("","");  //Change to the new email address. 
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.Send(mailMessage);
            }
        }
    }
}
