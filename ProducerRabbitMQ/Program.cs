using EmailLibrary;
using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "email",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

do
{
    var email = new Email("szymon.j.jedrzejewski@gmail.com", "Militaria.pl - zadanie rekrutacyjne email", "Przykładowe ciało maila.");

    var message = email.EmailToJSON();

    var body = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish(exchange: string.Empty,
                         routingKey: "email",
                         basicProperties: null,
                         body: body);

    Console.WriteLine("Wysłano zlecenie wysłania maila.");

    Console.WriteLine("Wciśnij [Enter] aby ponownie wysłać zlecenie.");
}while(Console.ReadKey(true).Key == ConsoleKey.Enter);