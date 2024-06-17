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
    private OnNotification notification;
    private CanvasGroup _currentMessage;

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

    private void ShowMessage(GameObject message) 
    {
        if (_currentMessage) Destroy(_currentMessage.gameObject);
        _currentMessage = Instantiate(message, transform).AddComponent<CanvasGroup>();
        state = NotificationState.Display;
        displayCounter = displayDuration;

        _currentMessage.alpha = 1;
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
        if (displayCounter <= 0)
        {
            state = NotificationState.Fade;
        }
    }
    void Fade()
    {
        if (_currentMessage.alpha < float.Epsilon)
        {
            Destroy(_currentMessage.gameObject);
            state = NotificationState.Hidden;
        }
        _currentMessage.alpha = Mathf.Max(0, _currentMessage.alpha - Time.deltaTime / fadeDuration);
    }
}
