using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChargeData : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image[] chargeBars;
    [SerializeField] private Image[] chargeGuide;

    public TMP_Text chargeSkillName;
    public string[] skillName;
    private int currentCharge,prevUseCharge;
    public void UpdateCharge(int charge)
    {
        currentCharge = charge;
        UpdateAlpha(chargeGuide[1].GetComponent<CanvasGroup>(), 1f, 0.5f);
        for (int i = 0; i < chargeBars.Length; i++)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(chargeBars[i]);

            if (i < charge)
            {
                // Activate charge bar
                UpdateAlpha(canvasGroup, 1f, 0.5f);
            }
            else
            {
                // Deactivate charge bar
                UpdateAlpha(canvasGroup, 0f, 0.5f);
            }
        }
    }
    public void UseCharge(int charge)
    {
        UpdateName(charge);
        UpdateAlpha(GetCanvasGroup(chargeGuide[0]), charge > 0 ? 1f : 0f, 0.5f);
        UpdateInteractable(GetCanvasGroup(chargeGuide[0]), charge > 0);
        UpdateAlpha(GetCanvasGroup(chargeGuide[1]), charge == currentCharge ? 0f : 1, 0.5f);
        UpdateInteractable(GetCanvasGroup(chargeGuide[1]), charge != currentCharge);
        for (int i = chargeBars.Length - 1; i >= 0; i--)
        {
            CanvasGroup canvasGroup = GetCanvasGroup(chargeBars[i]);
            int indexDifference = currentCharge - 1 - i;

            if (indexDifference < charge)
            {
                // Animate charge depletion with ping-pong effect
                AnimatePingPong(canvasGroup);
            }
            else if (indexDifference < currentCharge && canvasGroup.alpha != 1f)
            {
                // Restore charge bar to active state
                ResetAlpha(canvasGroup);
            }
        }
        prevUseCharge = charge;
    }
    private void UpdateName(int i) 
    {
        StartCoroutine(UpdateNameRoutine(i));
    }
    private IEnumerator UpdateNameRoutine(int i) 
    {
        if (LeanTween.isTweening(chargeSkillName.gameObject))
        {
            LeanTween.cancel(chargeSkillName.gameObject);
        }
        float target = -800f;
        if (prevUseCharge < i)
            target = -target;
        LeanTween.moveLocalX(chargeSkillName.gameObject, target, 0.5f).setEaseInExpo();
        yield return new WaitForSeconds(0.5f);
        chargeSkillName.text = skillName[i];
        chargeSkillName.gameObject.transform.localPosition = new Vector3(-target, 0, 0);
        LeanTween.moveLocalX(chargeSkillName.gameObject, 0f, 0.5f).setEaseOutExpo();
    }
    private void UpdateAlpha(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            LeanTween.cancel(canvasGroup.gameObject);
            LeanTween.alphaCanvas(canvasGroup, targetAlpha, duration).setEaseInOutSine();
        }
    }
    private void AnimatePingPong(CanvasGroup canvasGroup)
    {
        LeanTween.alphaCanvas(canvasGroup, 0f, 0.5f)
            .setLoopPingPong()
            .setEaseInOutSine();
    }
    private void ResetAlpha(CanvasGroup canvasGroup)
    {
        LeanTween.cancel(canvasGroup.gameObject);
        LeanTween.alphaCanvas(canvasGroup, 1f, 0.5f).setEaseInOutSine();
    }
    private void UpdateInteractable(CanvasGroup canvasGroup, bool interactable)
    {
        canvasGroup.interactable = interactable;
        canvasGroup.blocksRaycasts = interactable;
    }

    private CanvasGroup GetCanvasGroup(Image image)
    {
        return image.GetComponent<CanvasGroup>();
    }
    
}
