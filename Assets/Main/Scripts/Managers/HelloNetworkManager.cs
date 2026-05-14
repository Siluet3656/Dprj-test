using Main.Scripts.Messages;
using Main.Scripts.Services;
using Mirror;
using Zenject;

namespace Main.Scripts.Managers
{
    public class HelloNetworkManager : NetworkManager
    {
        private NetworkMessageService _messageService;

        [Inject]
        private void Construct(NetworkMessageService messageService)
        {
            _messageService = messageService;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            _messageService.RegisterServerHandlers();
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            _messageService.RegisterClientHandlers();
        }

        public override void OnClientConnect()
        {
            base.OnClientConnect();

            _messageService.SendSubscribeMessage(nameof(HelloMessage));
        }
    }
}