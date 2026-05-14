using Main.Scripts.Services;
using Zenject;

namespace Main.Scripts.Installers
{
    public class HelloInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<NetworkMessageService>().AsSingle();
            Container.BindInterfacesAndSelfTo<HelloMessageDisplay>().FromComponentInHierarchy().AsSingle();
        }
    }
}