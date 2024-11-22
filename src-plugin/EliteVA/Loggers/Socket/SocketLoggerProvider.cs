using System.Net;
using Microsoft.Extensions.Logging;
using WatsonWebsocket;

namespace EliteVA.Loggers.Socket;

public class SocketLoggerProvider : ILoggerProvider
{
    private readonly WatsonWsServer _server;
    private int _basePort = 51556;
    private int _maxRetries = 10;

    public SocketLoggerProvider()
    {
        for (int attempt = 0; attempt < _maxRetries; attempt++)
            {
                try
                {
                    int currentPort = _basePort + attempt;
                    _server = new WatsonWsServer(IPAddress.Loopback.ToString(), currentPort);
                    _server.Start();
                    Console.WriteLine($"Server started successfully on port {currentPort}");
                    return;
                }
                catch (Exception ex) when (ex is System.Net.Sockets.SocketException || ex is System.IO.IOException)
                {
                    Console.WriteLine($"Port {_basePort + attempt} is in use. Trying next port...");
                }
            }
    }

    
    public void Dispose()
    {
        
    }

    public ILogger CreateLogger(string categoryName)
    {
        return new SocketLogger(_server);
    }
}
