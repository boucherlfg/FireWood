using System.Linq;
using UnityEngine;

public class ActScript : MonoBehaviour
{
    public float Range => interactionRange;
    [SerializeField]
    private float interactionRange = 2;
    private Interactable choice;
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
            if (choice) choice.Hint(false);
            if (intermediate) intermediate.Hint(true);
            choice = intermediate;
        }

        if (Input.GetKeyUp(KeyCode.Space)) 
        {
            if (choice) choice.Interact();
        }
    }
}