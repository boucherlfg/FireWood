using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseManager : MonoBehaviour
{
    private List<WoodScript> woods = new();
    private GameObject instance;
    private float counter;
    [SerializeField]
    private float timeBeforeHelp = 60;
    [SerializeField]
    private GameObject prefab;
    private PlayerScript player;
    private WoodScript wood;
    private GameState gameState;
    private float woodActiveCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerScript>();
        gameState = ServiceManager.Instance.Get<GameState>();
        gameState.Wood.Changed += Wood_Changed;
    }

    private void Wood_Changed(int oldValue, int newValue)
    {
        if (oldValue < newValue)
        {
            counter = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (instance) return;

        // if user didn't find wood, give them another chance
        woodActiveCounter += Time.deltaTime;
        if (woodActiveCounter > timeBeforeHelp)
        {
            woodActiveCounter = 0;
            wood = null;
        }

        if (wood && wood.gameObject.activeSelf) return;

        counter += Time.deltaTime;
        if (counter > timeBeforeHelp)
        {
            var woods = FindObjectsOfType<WoodScript>();
            counter = 0;
            var activeWoods = woods.Where(x => x.gameObject.activeSelf);
            if (activeWoods.Count() <= 0) return;

            wood = activeWoods.OrderBy(x => Vector2.Distance(player.transform.position, x.transform.position)).First();

            instance = Instantiate(prefab, wood.transform.position, Quaternion.identity, wood.transform.parent);
        }
    }
}
