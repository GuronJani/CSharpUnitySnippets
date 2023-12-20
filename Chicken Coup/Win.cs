using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Win : MonoBehaviour
{
    [SerializeField] Image fadePanel;
    bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Unit>(out Unit u))
        {
            Victory();
        }
    }

    public void Victory()
    {
        if (gameWon) { return; }

        gameWon = true;
        StartCoroutine(SmoothLoadLevel(2));
    }

    IEnumerator SmoothLoadLevel(int i)
    {
        fadePanel.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(i);
    }

}
