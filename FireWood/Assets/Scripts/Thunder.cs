using System.Collections;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private bool isThundering = false;
    private bool isActive = false;

    private float counter;

    private float timeBeforeThunder;
    private int thunderCount;
    [SerializeField] private float minimumThunderInterval = 3;
    [SerializeField] private float maximumThunderInterval = 10;
    
    [SerializeField] private int minimumThunderCount = 1;
    [SerializeField] private int maximumThunderCount = 5;

    [SerializeField] private float thunderOffTime = 0.05f;
    [SerializeField] private float thunderOnTime = 0.05f;
    [SerializeField] private float thunderRandomFactor = 0.25f;
    private Darkness darkness;

    private Coroutine thunderCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        darkness = GetComponentInChildren<Darkness>();
        var storm = ServiceManager.Instance.Get<OnStormChangedEvent>();
        storm.Subscribe(HandleStormChanged);
    }
    private void OnDestroy()
    {
        var storm = ServiceManager.Instance.Get<OnStormChangedEvent>();
        storm.Unsubscribe(HandleStormChanged);
    }
    void HandleStormChanged(Storm.StormState state) 
    {
        isActive = state == Storm.StormState.Ingoing;

        if (thunderCoroutine != null)
        {
            StopCoroutine(thunderCoroutine);
            thunderCoroutine = null;
        }

        if (isActive)
        {
            SetNextThunder();
        }
        else
        {
            darkness.gameObject.SetActive(true);
        }
    }

    void SetNextThunder()
    {
        counter = 0;
        timeBeforeThunder = Random.Range(minimumThunderInterval, maximumThunderInterval);
        thunderCount = Random.Range(minimumThunderCount, maximumThunderCount);
    }
    // Update is called once per frame
    void Update()
    {
        if (!isActive) return;
        if (isThundering) return;

        counter += Time.deltaTime;
        if (!isThundering && counter < thunderCount) return;
        isThundering = true;
        
        thunderCoroutine = StartCoroutine(DoThunder());
    }
    IEnumerator DoThunder()
    {
        while (thunderCount > 0)
        {
            darkness.gameObject.SetActive(false);
            yield return new WaitForSeconds(thunderOnTime + Random.value * thunderRandomFactor * thunderOnTime);
            darkness.gameObject.SetActive(true);
            yield return new WaitForSeconds(thunderOffTime + Random.value * thunderRandomFactor * thunderOffTime);
            thunderCount--;
        }
        isThundering = false;
        SetNextThunder();
    }
}
