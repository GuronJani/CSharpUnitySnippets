using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Image fadePanel;

    // Start is called before the first frame update
    void Start()
    {
        fadePanel.color = Color.black;
        fadePanel.DOFade(0f, 1f);
    }

    public void StartGame()
    {
        StartCoroutine(SmoothLoadLevel(1));
    }

    IEnumerator SmoothLoadLevel(int i)
    {
        fadePanel.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(i);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReturnToMenu()
    {
        StartCoroutine(SmoothLoadLevel(0));
    }

}
