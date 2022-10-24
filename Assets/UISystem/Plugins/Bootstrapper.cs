using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Execute()
    {
        GameObject UISystemGO = Object.Instantiate(Resources.Load("UISystemPrefab")) as GameObject;
        Components.UISystem UISystem = UISystemGO.AddComponent<Components.UISystem>();

        UISystem.hideFlags = HideFlags.HideInHierarchy;
        UISystem.hideFlags = HideFlags.HideInInspector;
    }
}
