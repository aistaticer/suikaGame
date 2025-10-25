using UnityEngine;
using Zenject;

public class DebugInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameManager>()
            .FromComponentInHierarchy()
            .AsSingle();
        
        Container.Bind<RestartButton>()
            .FromComponentInHierarchy()
            .AsSingle();

        Debug.Log("[DebugInstaller] 呼ばれた！");
    }
}

