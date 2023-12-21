using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] float transitionTime = 1f;
    [SerializeField] Image fadePanel;
    [SerializeField] AudioSource bgm;
    AudioSource sfx;
    [SerializeField] AudioClip confirmSound;
    [SerializeField] AudioClip hoverSound;

    bool isRunning = false;

    // Start is called before the first frame update
    void Start()
    {
        sfx = GetComponent<AudioSource>();
        fadePanel.fillAmount = 1f;
        fadePanel.DOFillAmount(0f, transitionTime);
        bgm.volume = 0f;
        bgm.DOFade(0.3f, transitionTime);

        Cursor.visible = true;
    }

    public void StartGame()
    {
        if (!isRunning)
        {
            StartCoroutine(ChangeScene(1, transitionTime));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void HoverSound()
    {
        sfx.PlayOneShot(hoverSound, 0.3f);
    }

    IEnumerator ChangeScene(int sceneNum, float fadeTime)
    {
        // Fade out, wait, load new scene based on parameters
        isRunning = true;
        sfx.PlayOneShot(confirmSound, 0.5f);
        fadePanel.DOFillAmount(1f, fadeTime);
        bgm.DOFade(0f, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        DOTween.KillAll();
        SceneManager.LoadScene(sceneNum);
    }

}
