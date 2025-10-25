using Zenject;
using UnityEngine;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>()
            .FromComponentInHierarchy()
            .AsSingle();

        Container.Bind<RestartButton>()
            .FromComponentInHierarchy()
            .AsSingle();

        Debug.Log("[Installer] GameManager をバインドしました ✅");
    }
}