using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Services.NotificationSevice.cs
{
    public static class WebSocketManager
    {
        private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        // Add a WebSocket connection
        public static void AddSocket(string id, WebSocket socket)
        {
            _sockets.TryAdd(id, socket);
        }

        // Remove a WebSocket connection
        public static void RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out var socket);
        }

        // Broadcast a message to all WebSocket clients
        public static async Task BroadcastMessage(string title, string message)
        {
            var notification = new { Title = title, Message = message };
            var json = JsonSerializer.Serialize(notification);

            foreach (var socket in _sockets.Values)
            {
                if (socket.State == WebSocketState.Open)
                {
                    var buffer = Encoding.UTF8.GetBytes(json);
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }
}