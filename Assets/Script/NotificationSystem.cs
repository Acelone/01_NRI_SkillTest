using System.Collections;
using TMPro;
using UnityEngine;

public class NotificationSystem : MonoBehaviour
{
    public static NotificationSystem Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private TMP_Text notificationText;

    [Header("Notification Settings")]
    [SerializeField] private float offScreenPositionX = 3000f;

    [TextArea(3,5)]
    public string notificationLog;

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void ShowNotification(string text, float duration)
    {
        if (notificationText == null) return;
        notificationLog += text + "\n";
        StartCoroutine(HandleNotification(text,duration));
    }
    private IEnumerator HandleNotification(string text, float duration)
    {
        while (LeanTween.isTweening(this.gameObject)) 
        {
            yield return null;
        }
        notificationText.text = text;
        float halfDuration = duration / 2f;

        // Slide in
        LeanTween.moveLocalX(gameObject, 0f, halfDuration).setEaseOutExpo();
        yield return new WaitForSeconds(halfDuration);

        // Slide out
        LeanTween.moveLocalX(gameObject, -offScreenPositionX, halfDuration).setEaseInExpo();
        yield return new WaitForSeconds(halfDuration);

        // Reset position
        ResetNotificationPosition();
    }
    private void ResetNotificationPosition()
    {
        Vector3 position = transform.localPosition;
        position.x = offScreenPositionX;
        transform.localPosition = position;
    }
}