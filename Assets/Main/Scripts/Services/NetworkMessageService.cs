using System;
using System.Collections.Generic;
using Main.Scripts.Messages;
using Mirror;
using UnityEngine;

namespace Main.Scripts.Services
{
    public class NetworkMessageService : IDisposable
    {
        private readonly Dictionary<NetworkConnectionToClient, HashSet<string>> _subscriptions = new();

        public event Action<HelloMessage> OnHelloMessageReceived;

        public void RegisterServerHandlers()
        {
            NetworkServer.RegisterHandler<SubscribeMessage>(OnServerSubscribeMessage);
            NetworkServer.OnDisconnectedEvent += OnServerClientDisconnected;
        }

        public void RegisterClientHandlers()
        {
            NetworkClient.RegisterHandler<HelloMessage>(OnClientHelloMessage);
        }

        private void OnServerSubscribeMessage(NetworkConnectionToClient conn, SubscribeMessage msg)
        {
            if (!_subscriptions.ContainsKey(conn))
            {
                _subscriptions[conn] = new HashSet<string>();
            }

            _subscriptions[conn].Add(msg.MessageType);

            if (_subscriptions[conn].Contains(nameof(HelloMessage)))
            {
                conn.Send(new HelloMessage { Text = "Hello Client!" });
            }
        }

        private void OnClientHelloMessage(HelloMessage msg)
        {
            OnHelloMessageReceived?.Invoke(msg);
        }

        private void OnServerClientDisconnected(NetworkConnectionToClient conn)
        {
            _subscriptions.Remove(conn);
        }

        public void SendSubscribeMessage(string messageType)
        {
            if (!NetworkClient.isConnected)
            {
                Debug.LogWarning("нет клиента");
                return;
            }

            NetworkClient.Send(new SubscribeMessage { MessageType = messageType });
        }

        public void Dispose()
        {
            _subscriptions.Clear();
            NetworkServer.UnregisterHandler<SubscribeMessage>();
            NetworkClient.UnregisterHandler<HelloMessage>();
            NetworkServer.OnDisconnectedEvent -= OnServerClientDisconnected;
        }
    }
}