﻿using System;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;
using Service_Solution_Project2_PBA.repositories.redisCache;
using Service_Solution_Project2_PBA.services;
using Service_Solution_Project2_PBA.domain;
using System.Threading.Tasks;

namespace Service_Solution_Project2_PBA
{
    public class RabbitMQRecieve
    {

        //Will keep running with the EventingBasicConsumer event.  
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
                channel.QueueDeclare(queue: "SendFromClient",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    RedisCacheServiceIF rediscacheService = new RedisCacheService();
                    var answer = rediscacheService.RetrieveFromCacheParkingService<string>(message);

                    //var ad = rediscacheService.RetrieveFromCacheAdService<string>(message);



                    //SendWithRabbit(answer.Result, ad.Result, message);

                    //TEST CALL
                    SendWithRabbit(answer.Result, "TEST", message);


                    //Console.WriteLine(" [x] Received {0}", message);

                };
                channel.BasicConsume(queue: "SendFromClient",
                                 autoAck: true,
                                 consumer: consumer);
                Console.ReadLine();
            }
        }
        


        public void SendWithRabbit (string parking, string ad, string message)
        {
            RabbitMQSent send = new RabbitMQSent();
            send.RabbitMQSendParkingAndAd(parking, ad, message);
            

        }


    }
}
