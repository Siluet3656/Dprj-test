using System;
using Main.Scripts.Messages;
using Main.Scripts.Services;
using UnityEngine;
using Zenject;

namespace Main.Scripts
{
    public class HelloMessageDisplay : MonoBehaviour, IInitializable, IDisposable
    {
        private NetworkMessageService _messageService;

        [Inject]
        private void Construct(NetworkMessageService messageService)
        {
            _messageService = messageService;
        }

        public void Initialize()
        {
            _messageService.OnHelloMessageReceived += ShowMessage;
        }

        private void ShowMessage(HelloMessage msg)
        {
            Debug.Log(msg.Text);
        }

        public void Dispose()
        {
            if (_messageService != null)
            {
                _messageService.OnHelloMessageReceived -= ShowMessage;
                _messageService = null;
            }
        }
    }
}