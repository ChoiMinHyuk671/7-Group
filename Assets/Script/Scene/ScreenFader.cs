using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PageType
{
    Loading, GameOver, BackGround
}

public class ScreenFader : MonoBehaviour
{
    public CanvasGroup backCanvasGroup;
    public CanvasGroup loadingCanvasGroup;
    public CanvasGroup gameOverCanvasGroup;
    public float fadeTime = 1f;
    protected static bool IsFade;

    public static ScreenFader Instance
    {
        get
        {
            if (s_Instance != null)
                return s_Instance;

            s_Instance = FindObjectOfType<ScreenFader>();

            if (s_Instance != null)
                return s_Instance;

            return s_Instance;
        }
    }

    protected static ScreenFader s_Instance;

    void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    protected IEnumerator Fade(float finalAlpha, CanvasGroup canvasGroup)
    {
        float fadeSpeed = Mathf.Abs(canvasGroup.alpha - finalAlpha) / fadeTime;
        while (!Mathf.Approximately(canvasGroup.alpha, finalAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);
            yield return null;
        }
        canvasGroup.alpha = finalAlpha;
    }

    public static IEnumerator FadeOut(PageType pageType)
    {

        CanvasGroup canvasGroup = null;

        switch (pageType)
        {
            case PageType.BackGround:
                canvasGroup = Instance.backCanvasGroup;
                break;
            case PageType.Loading:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
            case PageType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            default:
                Debug.Log($"오류 : '{pageType}' 씬 전환 UI가 없습니다");
                break;
        }

        canvasGroup.gameObject.SetActive(true);

        yield return Instance.StartCoroutine(Instance.Fade(1f, canvasGroup));
    }

    public static IEnumerator FadeIn(PageType pageType)
    {
       CanvasGroup canvasGroup = null;

        switch (pageType)
        {
            case PageType.BackGround:
                canvasGroup = Instance.backCanvasGroup;
                break;
            case PageType.Loading:
                canvasGroup = Instance.loadingCanvasGroup;
                break;
            case PageType.GameOver:
                canvasGroup = Instance.gameOverCanvasGroup;
                break;
            default:
                Debug.Log($"오류 : '{pageType}' 씬 전환 UI가 없습니다");
                break;
        }

        yield return Instance.StartCoroutine(Instance.Fade(0f, canvasGroup));
        canvasGroup.gameObject.SetActive(false);
    }
}
