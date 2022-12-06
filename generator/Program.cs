using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                UserName = "admin",
                Password = "admin",
                HostName = "SI_175132_rabbitmq",
                Port = 5672
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile($"appsettings.json", false, true)
                    .Build();
                List<GenericSensor> sensors = new List<GenericSensor>
                {
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts1"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts2"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts3"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts4"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts5"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts6"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts7"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts8"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts9"),
                    new GenericSensor(channel, config.GetSection("TemperatureSensor"), "ts10"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns1"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns2"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns3"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns4"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns5"),
                    new GenericSensor(channel, config.GetSection("NoiseSensor"), "ns6"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs1"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs2"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs3"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs4"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs5"),
                    new GenericSensor(channel, config.GetSection("MusicVolumeSensor"), "mvs6"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs1"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs2"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs3"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs4"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs5"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs6"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs7"),
                    new GenericSensor(channel, config.GetSection("PeopleCounterSensor"), "pcs8")
                };
                List<Thread> threads = new List<Thread>();

                foreach (var sensor in sensors)
                {
                    Console.WriteLine(sensor);
                    sensor.Started = true;
                    var thread = sensor.publishingThread();
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (var thread in threads)
                {
                    thread.Join();
                }
            }
        }
    }
}
