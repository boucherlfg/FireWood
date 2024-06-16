using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationScript : MonoBehaviour
{
    private enum NotificationState
    {
        Display,
        Fade,
        Hidden
    }
    private NotificationState state = NotificationState.Hidden;
    private float displayCounter = 0;
    [SerializeField]
    private float displayDuration = 3;
    [SerializeField]
    private float fadeDuration = 1;
    [SerializeField]
    private TMPro.TMP_Text label;
    private OnNotification notification;
    // Start is called before the first frame update
    void Start()
    {
        notification = ServiceManager.Instance.Get<OnNotification>();
        notification.Subscribe(ShowMessage);
    }
    private void OnDestroy()
    {
        notification.Unsubscribe(ShowMessage);
    }

    private void ShowMessage(string message) 
    {
        label.text = message;
        state = NotificationState.Display;
        displayCounter = displayDuration;

        var color = label.color;
        color.a = 1;
        label.color = color;
    }

    private void Update()
    {
        switch (state)
        {
            case NotificationState.Display:
                Display();
                break;
            case NotificationState.Fade:
                Fade();
                break;
        }
    }
    void Display()
    {
        displayCounter -= Time.deltaTime;
        if (displayCounter > 0)
        {
            state = NotificationState.Fade;
        }
    }
    void Fade()
    {
        var color = label.color;
        if (color.a < float.Epsilon)
        {
            state = NotificationState.Hidden;
        }
        color.a = Mathf.Max(0, color.a - Time.deltaTime / fadeDuration);
        label.color = color;
    }
}
