using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces.NotificationInterface;

public  class WebsocketHandler :IWebsocketHandler
{

    private readonly Dictionary<string, WebSocket> _connections = new();

    public async Task AddConnectionAsync(string connectionId, WebSocket webSocket)
    {
        _connections[connectionId] = webSocket;
    }

   
public async Task SendMessageToAdminAsync(string message)
    {
        foreach (var webSocket in _connections.Values)
        {
            if (webSocket.State == WebSocketState.Open)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                var segment = new ArraySegment<byte>(buffer);

                // Send message to the connected admin client
                await webSocket.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
