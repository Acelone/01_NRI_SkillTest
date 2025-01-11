using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameConfig : MonoBehaviour
{
    public TMP_Text winNotif,turnText, remainingHp;
    public GameObject dataUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator EndGameRoutine() 
    {
        yield return new WaitForSeconds(3);
        winNotif.text = GameSystem.Instance.win ? "YOU WIN" : "YOU LOSE";
        turnText.text = GameSystem.Instance.turnCount.ToString();
        remainingHp.text = GameSystem.Instance.player.health.ToString();
        CanvasGroup thisCanvasGroup = GetComponent<CanvasGroup>();
        LeanTween.alphaCanvas(thisCanvasGroup, 1, 0.5f).setEaseInOutSine();
        yield return new WaitForSeconds(0.5f);
        LeanTween.moveLocalX(dataUI, 0f, 1f).setEaseOutExpo();
        yield return new WaitForSeconds(0.5f);
        thisCanvasGroup.interactable = true;
        thisCanvasGroup.blocksRaycasts = true;
    }
    public void EndGame() 
    {
        StartCoroutine(EndGameRoutine());
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
}
