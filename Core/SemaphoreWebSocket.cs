using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Gracie.Core
{
    public class SemaphoreWebSocket
    {
        private readonly ClientWebSocket webSocket;
        private readonly SemaphoreSlim semaphoreSlim;
        private readonly int bufferSize;
        private readonly int? taskTimeout;
        private readonly List<Task> taskList = new List<Task>();

        public SemaphoreWebSocket(ClientWebSocket webSocket, SemaphoreSlim semaphoreSlim, int bufferSize, int? taskTimeout = null)
        {
            this.webSocket = webSocket;
            this.semaphoreSlim = semaphoreSlim;
            this.bufferSize = bufferSize;
            this.taskTimeout = taskTimeout;
        }

        public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
        {
            await webSocket.ConnectAsync(uri, cancellationToken);
        }

        public async Task CloseAsync(WebSocketCloseStatus webSocketCloseStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await webSocket.CloseAsync(webSocketCloseStatus, statusDescription, cancellationToken);
        }

        public async Task CloseOutputAsync(WebSocketCloseStatus webSocketCloseStatus, string statusDescription, CancellationToken cancellationToken = default)
        {
            await webSocket.CloseOutputAsync(webSocketCloseStatus, statusDescription, cancellationToken);
        }

        public void Abort()
        {
            webSocket.Abort();
        }

        public async Task SendAsync(ArraySegment<byte> buffer, WebSocketMessageType webSocketMessageType, bool endOfMessage, CancellationToken cancellationToken = default)
        {
            await webSocket.SendAsync(buffer, webSocketMessageType, endOfMessage, cancellationToken);
        }

        public async Task Recieve(CancellationToken cancellationToken = default)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var buffer = new byte[bufferSize];
                var segment = new ArraySegment<byte>(buffer);
                var receiveResult = await webSocket.ReceiveAsync(segment, cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.WhenAll(taskList);
                    RemoveCompletedTasks();
                    return;
                }

                if (receiveResult.MessageType == WebSocketMessageType.Close)
                {
                    if (WebSocketClosed != null)
                    {
                        await WebSocketClosed(this, receiveResult, buffer, cancellationToken);
                    }
                    await Task.WhenAll(taskList);
                    RemoveCompletedTasks();
                    return;
                }

                await semaphoreSlim.WaitAsync(cancellationToken);

                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.WhenAll(taskList);
                    RemoveCompletedTasks();
                    return;
                }

                if (MessageReceived == null)
                {
                    semaphoreSlim.Release();
                }
                else
                {
                    var task = Task.Run(async () =>
                    {
                        CancellationToken subToken = cancellationToken;
                        if (taskTimeout.HasValue)
                        {
                            subToken = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationTokenSource(taskTimeout.Value).Token).Token;
                        }
                        try
                        {
                            await MessageReceived(this, receiveResult, buffer, subToken);
                        }
                        finally
                        {
                            semaphoreSlim.Release();
                        }
                    });
                    taskList.Add(task);
                }

                if (cancellationToken.IsCancellationRequested)
                {
                    await Task.WhenAll(taskList);
                    RemoveCompletedTasks();
                    return;
                }

                RemoveCompletedTasks();
            }
        }

        private void RemoveCompletedTasks()
        {
            taskList.RemoveAll(x => x.IsCompleted);
        }

        public delegate Task MessageRecievedHandler(object sender, WebSocketReceiveResult receiveResult, byte[] buffer, CancellationToken cancellation = default);
        public event MessageRecievedHandler MessageReceived;

        public delegate Task WebSocketClosedHandler(object sender, WebSocketReceiveResult receiveResult, byte[] buffer, CancellationToken cancellation = default);
        public event WebSocketClosedHandler WebSocketClosed;
    }
}
