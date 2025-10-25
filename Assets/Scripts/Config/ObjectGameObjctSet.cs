using UnityEngine;

public enum ObjectPrefabType
{
    Capsule,
    Circle,
    Square
}

[CreateAssetMenu(menuName = "Config/PlayerGameObjectSet")]
public class ObjectGameObjectSet : ScriptableObject
{
    public GameObject CapsuleGameObject;
    public GameObject CircleGameObject;
    public GameObject SquareGameObject;

    public GameObject GetGameObject(ObjectPrefabType type)
    {
        switch (type)
        {
            case ObjectPrefabType.Capsule: return CapsuleGameObject;
            case ObjectPrefabType.Circle: return CircleGameObject;
            case ObjectPrefabType.Square: return SquareGameObject;
            default: return null;
        }
    }
}