using UIFlipmorris;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        UISystemCallback.OnGameStaretd += () =>
        {
            Debug.Log("game started");
        };

        UISystemCallback.OnGameRestart += () =>
        {
            Debug.Log("GAME RESTARTED");
        };
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            UISystemCallback.OnGameResult.Invoke("YOU WIN");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            UISystemCallback.OnGameResult.Invoke("YOU LOSE");
        }
    }
}
