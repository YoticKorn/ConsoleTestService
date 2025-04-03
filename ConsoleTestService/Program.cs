using Test.ClientPackets;
using Test.ServerPackets;
using Korn.Service;
using System;

class Program
{
    static void Main()
    {
        var configuration = new ServerConfiguration("TestServer", new PacketList("Test.ClientPackets"), new PacketList("Test.ServerPackets"));
     
        /* Server */
        var server = new Server(configuration);
        server
        .Register<PingPacket>((connection, packet) =>
        {
            var pong = new PongPacket();
            connection.Callback(packet, pong);
        })
        .Register<LogWritePacket>((connection, packet) => Console.WriteLine(packet.Text));

        /* Client */
        var client = new Client(configuration);
        client.Send(new PingPacket(), callback => Console.WriteLine("ping! pong!"));

        for (var i = 0; i < 10000; i++)
            client.Send(new LogWritePacket($"{i}"));

        Console.ReadLine();
    }
}

namespace Test.ClientPackets
{
    class PingPacket : ClientCallbackablePacket<PongPacket> { }

    class LogWritePacket : ClientPacket
    {
        public LogWritePacket(string text) => Text = text;
        public string Text;
    }
}

namespace Test.ServerPackets
{
    class PongPacket : ServerCallbackPacket { }
}