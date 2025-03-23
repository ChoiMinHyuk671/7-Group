using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScreenUICore : MonoBehaviour
{
    protected ScreenUICore previousScreen;
    public static ScreenUICore activeScreen;
    public static void Show(ScreenUICore screen)
    {
        if (screen == activeScreen)
            return;

        if (activeScreen != null)
            activeScreen.Hide();
        screen.previousScreen = activeScreen;
        activeScreen = screen;
        screen.Show();
    }
    public void ShowScreen(ScreenUICore screen) //활성화할 UI 받는곳
    {
        Show(screen);
    }
    public void Show() //실질적인 활성화를 실행
    {
        if (gameObject)
            gameObject.SetActive(true);
    }
    public void Hide() //UI 비활성화를 담당
    {
        if (gameObject)
            gameObject.SetActive(false);
    }
    public void Back() //되돌리기를 실행
    {
        if (previousScreen != null)
        {
            Hide();
            activeScreen = previousScreen;
            activeScreen.Show();
            previousScreen = null;
        }
    }
}
