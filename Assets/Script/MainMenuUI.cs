using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public CanvasGroup title;
    public CanvasGroup[] button;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartRoutine());
    }
    IEnumerator StartRoutine() 
    {
        LeanTween.alphaCanvas(title, 1, 0.5f).setEaseInOutSine();
        yield return new WaitForSeconds(1);
        for (int i = 0; i < button.Length; i++) 
        {
            LeanTween.alphaCanvas(button[i], 1, 0.5f).setEaseInOutSine();
            yield return new WaitForSeconds(0.25f);
        }
            
    }
    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

}
