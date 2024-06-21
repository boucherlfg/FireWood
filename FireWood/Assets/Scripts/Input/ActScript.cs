using System.Linq;
using UnityEngine;

public class ActScript : MonoBehaviour
{
    private InputSystem input;
    public float Range => interactionRange;
    [SerializeField]
    private float interactionRange = 2;
    private Interactable choice;

    private void Start()
    {
        input = ServiceManager.Instance.Get<InputSystem>();
        input.Acted += Input_Acted;
    }
    private void OnDestroy()
    {
        input.Acted -= Input_Acted;
    }

    private void Input_Acted()
    {
        if (choice) choice.Interact();
    }

    private void Update()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, interactionRange).Where(x => x.GetComponent<Interactable>());

        if (hits.Count() <= 0)
        {
            if(choice) choice.Hint(false);
            choice = null;
            return;
        }

        var intermediate = hits.OrderBy(x => Vector2.Distance(x.transform.position, transform.position))
                             .FirstOrDefault().GetComponent<Interactable>();

        if (intermediate != choice)
        {
            if (choice)
            {
                choice.Hint(false);
            }
            if (intermediate)
            {
                intermediate.Hint(true);
            }
            choice = intermediate;
        }
    }
}