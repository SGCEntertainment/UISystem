using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

namespace UIFlipmorris
{
    public static class UISystemCallback
    {
        public static Action OnGameStaretd { get; set; } = delegate { };
        public static Action OnGameRestart { get; set; } = delegate { };
        public static Action<string> OnGameResult { get; set; } = delegate { };
    }

    class Components
    {
        public class MyBtn : MonoBehaviour, IPointerDownHandler
        {
            public Action OnClickEvent { get; set; }
            public void OnPointerDown(PointerEventData eventData)
            {
                OnClickEvent?.Invoke();
            }
        }

        public class UISystem : MonoBehaviour
        {
            GameObject loaderProgressGo;
            GameObject tapToStartGo;
            GameObject gameUIGo;
            GameObject resultPopupGo;
            GameObject pausePopupGo;

            Text resultText;
            Image filled;
            Scene currentScene;

            private void Awake()
            {
                CacheComponents();
                SetupInit();
            }

            private void Start() => StartCoroutine(nameof(Loading));

            void SetupInit()
            {
                loaderProgressGo.SetActive(true);
                tapToStartGo.SetActive(false);
                gameUIGo.SetActive(false);
                resultPopupGo.SetActive(false);
                pausePopupGo.SetActive(false);
            }

            void CacheComponents()
            {
                currentScene = SceneManager.GetActiveScene();

                loaderProgressGo = FindGameObject("loaderProgressGo");
                tapToStartGo = FindGameObject("tapToStartGo");
                gameUIGo = FindGameObject("gameUIGo");
                resultPopupGo = FindGameObject("resultPopupGo");
                pausePopupGo = FindGameObject("pausePopupGo");

                resultText = FindGameObject("resultText").GetComponent<Text>();
                filled = FindGameObject("filled").GetComponent<Image>();

                MyBtn myBtn = tapToStartGo.AddComponent<MyBtn>();
                myBtn.hideFlags = HideFlags.HideInHierarchy;
                myBtn.hideFlags = HideFlags.HideInInspector;
                myBtn.OnClickEvent += () =>
                {
                    tapToStartGo.SetActive(false);
                    gameUIGo.SetActive(true);
                };

                GameObject pauseGO = FindGameObject("pause");
                MyBtn pauseBtn = pauseGO.AddComponent<MyBtn>();
                pauseBtn.hideFlags = HideFlags.HideInHierarchy;
                pauseBtn.hideFlags = HideFlags.HideInInspector;
                pauseBtn.OnClickEvent += () =>
                {
                    gameUIGo.SetActive(false);
                    pausePopupGo.SetActive(true);
                };

                GameObject unpauseGO = FindGameObject("unpause");
                MyBtn unpauseBtn = unpauseGO.AddComponent<MyBtn>();
                unpauseBtn.hideFlags = HideFlags.HideInHierarchy;
                unpauseBtn.hideFlags = HideFlags.HideInInspector;
                unpauseBtn.OnClickEvent += () =>
                {
                    gameUIGo.SetActive(true);
                    pausePopupGo.SetActive(false);
                };

                GameObject menuGO = FindGameObject("menu");
                MyBtn menuBtn = menuGO.AddComponent<MyBtn>();
                menuBtn.hideFlags = HideFlags.HideInHierarchy;
                menuBtn.hideFlags = HideFlags.HideInInspector;
                menuBtn.OnClickEvent += () =>
                {
                    SetupInit();
                    StartCoroutine(nameof(Loading));
                };

                GameObject restartGO = FindGameObject("restart");
                MyBtn restartBtn = restartGO.AddComponent<MyBtn>();
                restartBtn.hideFlags = HideFlags.HideInHierarchy;
                restartBtn.hideFlags = HideFlags.HideInInspector;
                restartBtn.OnClickEvent += () =>
                {
                    UISystemCallback.OnGameRestart?.Invoke();

                    gameUIGo.SetActive(true);
                    resultPopupGo.SetActive(false);
                };

                UISystemCallback.OnGameResult += (resutlString) =>
                {
                    resultText.text = resutlString;

                    resultPopupGo.SetActive(true);
                    gameUIGo.SetActive(false);
                };
            }

            private void OnDestroy()
            {
                UISystemCallback.OnGameResult = null;
            }

            GameObject FindGameObject(string name)
            {
                Transform[] trs = gameObject.GetComponentsInChildren<Transform>(true);
                foreach (Transform t in trs)
                {
                    if (t.name == name)
                    {
                        return t.gameObject;
                    }
                }

                return null;
            }

            IEnumerator Loading()
            {
                float et = 0.0f;
                float loadingTime = UnityEngine.Random.Range(1.0f, 2.5f);

                filled.fillAmount = 0;

                while (et < loadingTime)
                {
                    float delayTime = UnityEngine.Random.Range(0.01f, 0.25f);
                    et += delayTime;

                    filled.fillAmount = et / loadingTime;
                    yield return new WaitForSeconds(delayTime);
                }

                loaderProgressGo.SetActive(false);
                tapToStartGo.SetActive(true);

                UISystemCallback.OnGameStaretd.Invoke();
            }
        }
    }
}
