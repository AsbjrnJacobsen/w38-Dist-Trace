using EasyNetQ;

namespace Helpers;

public static class ConnectionHelper
{
    public static IBus GetRMQConnection()
    {
        return RabbitHutch.CreateBus("host=localhost;port=5672;username=application;password=pepsi");
    }
}