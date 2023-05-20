using EmailLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };
using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

var emailFrom = "szymon.j.jedrzejewski@gmail.com";

channel.QueueDeclare(queue: "email",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

Console.WriteLine("Oczekiwanie na zlecenie...");

var email = new Email();

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine("Otrzymano zlecenie.");

    email.JSONToEmail(message);

    email.Send(emailFrom);

    Console.WriteLine("Otrzymano zlecenie.");
    Console.WriteLine("Oczekiwanie na zlecenie...");
};
channel.BasicConsume(queue: "email",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("Wciśnij [Enter] aby zakończyć.");
Console.ReadLine();
