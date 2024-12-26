using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class WebSocketManager
{
    private static readonly ConcurrentDictionary<string, WebSocket> _sockets = new();

    // Add a WebSocket to the collection
    public static void AddSocket(string id, WebSocket socket)
    {
        _sockets.TryAdd(id, socket);
    }

    // Remove a WebSocket from the collection
    public static void RemoveSocket(string id)
    {
        _sockets.TryRemove(id, out var _);
    }

    // Broadcast a message to all connected clients
    public static async Task BroadcastMessage(string title, string message)
    {
        var fullMessage = $"{title}: {message}";
        var messageBytes = Encoding.UTF8.GetBytes(fullMessage);

        foreach (var socket in _sockets.Values)
        {
            if (socket.State == WebSocketState.Open)
            {
                await socket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
