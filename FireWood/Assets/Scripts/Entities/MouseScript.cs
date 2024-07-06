using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;
    public enum MouseState
    {
        GoTo,
        Wait,
        Flee,
        Disappear
    }

    [SerializeField]
    private float timeBeforeDisappearing = 30f;
    private float disappearCounter;
    [SerializeField]
    private float minimumDistance = 2;
    [SerializeField]
    private float comfortableDistance = 3;
    [SerializeField]
    private float maximumDistance = 10;
    [SerializeField]
    private float speed = 3;
    
    private MouseState state= MouseState.GoTo;
    private AIPath aiPath;
    private PlayerScript player;
    private Vector2 startPosition;

    private float DistanceFromPlayer => Vector2.Distance(transform.position, player.transform.position);
    private float DistanceFromStart => Vector2.Distance(transform.position, startPosition);

    public bool ShouldDisappear => state == MouseState.Disappear;
    // Start is called before the first frame update
    void Start()
    {
        aiPath = GetComponent<AIPath>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (disappearCounter >= timeBeforeDisappearing)
        {
            this.state = MouseState.Disappear;
        }
        player = player ? player : FindObjectOfType<PlayerScript>();

        var state = this.state;
        switch (state)
        {
            case MouseState.GoTo:
                Goto();
                break;
            case MouseState.Wait:
                Wait();
                break;
            case MouseState.Flee:
                Flee();
                break;
        }
        if (state != this.state && debug) Debug.Log("mouse state : " + this.state); 
    }

    void Goto()
    {
        aiPath.destination = player.transform.position;
        aiPath.maxSpeed = speed;

        if (DistanceFromPlayer < comfortableDistance) state = MouseState.Wait;
    }

    void Wait()
    {
        disappearCounter += Time.deltaTime;
        aiPath.destination = transform.position;
        aiPath.maxSpeed = 0;
        if (DistanceFromPlayer < minimumDistance) state = MouseState.Flee;
        else if (DistanceFromPlayer > maximumDistance) state = MouseState.GoTo;
    }

    void Flee()
    {
        disappearCounter += Time.deltaTime;
        aiPath.destination = startPosition;
        aiPath.maxSpeed = speed;

        if (DistanceFromStart < 1)
        {
            aiPath.destination = transform.position;
            state = MouseState.Disappear;
        }
    }
}
