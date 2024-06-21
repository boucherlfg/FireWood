using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForesterScript : MonoBehaviour
{
    [SerializeField]
    private bool debug = false;
    [SerializeField]
    private LightScript _light;
    public float CurrentRange => _light.CurrentRange;
    public float distanceThatIsConsideredTarget = 0.6f;
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float followPlayerSpeedMultiplier = 3;
    [SerializeField]
    private float pauseBetweenTasks = 1;
    private float counter;
    #region [AI stuff]
    private ForesterState _state = ForesterState.NextWaypoint;
    private WoodScript _currentEmptyWood;
    private LightScript _currentLight;

    private int index = 0;
    private bool SeesPlayer => Vector2.Distance(transform.position, _player.transform.position) < CurrentRange + _player.Light.CurrentRange 
        && _player.Light.IsLit;
    private WaypointScript CurrentWaypoint
    {
        get
        {
            return Waypoints[index];
        }
    }

    public int _maxWoodOnTheMap = 100;
    #endregion

    public enum ForesterState 
    {
        NextWaypoint = 0,
        RefillWood = 1,
        Extinguish = 2,
        FollowPlayer = 3
    }
    private PlayerScript _player;
    private List<WaypointScript> Waypoints { get; set; } = new();
    private List<WoodScript> Woods { get; set; } = new();
    private List<LightScript> Lights { get; set; } = new();
    List<LightScript> AllOpenLightsInZone => Lights.FindAll(x => x.IsLit && x.transform.parent == CurrentWaypoint.transform.parent);
    
    private AIPath _aiPath;

    private void ChangeState(ForesterState state)
    {
        _state = state;
        ServiceManager.Instance.Get<OnForesterStateChanged>().Invoke(state);
    }
    private void Start()
    {
        _aiPath = GetComponent<AIPath>();
        _player = FindObjectOfType<PlayerScript>(true);
        Waypoints = FindObjectsOfType<WaypointScript>().ToList();
        Woods = FindObjectsOfType<WoodScript>(true).ToList();
        Lights = FindObjectsOfType<LightScript>(true).Where(x => !x.Exclude).ToList();

    }

    private void Update()
    {
        _aiPath.maxSpeed = _speed * (_state == ForesterState.FollowPlayer ? followPlayerSpeedMultiplier : 1);
        var state = _state;
        // peu importe ce qu'on fait, si le joueur est in range, on se met à le suivre
        if (_state != ForesterState.FollowPlayer && SeesPlayer && !Lights.Exists(x => x.Covers(_player.transform.position)))
        {
            ChangeState(ForesterState.FollowPlayer);
        }

        switch (_state)
        {
            case ForesterState.NextWaypoint:
                NextWaypoint();
                break;
            case ForesterState.Extinguish:
                Extinguish();
                break;
            case ForesterState.RefillWood:
                RefillWood();
                break;
            case ForesterState.FollowPlayer:
                FollowPlayer();
                break;
        }

        if (_state != state && debug)
        {
            Debug.Log("forester state : " + _state);
        }
    }

    void FollowPlayer()
    {
        if (!SeesPlayer || Lights.Exists(x => x.Covers(_player.transform.position)))
        {
            ChangeState(ForesterState.NextWaypoint);
            return;
        }

        // si on est assez proche du joueur, on éteint le joueur
        if (Vector2.Distance(transform.position, _player.transform.position) < distanceThatIsConsideredTarget)
        {
            _player.LightFuel.Extinguish();
            return;
        }

        _aiPath.destination = _player.transform.position;
    }

    void RefillWood()
    {
        // trouver les bois non-activés qui sont dans la zone
        var emptyWood = Woods.FindAll(x => !x.gameObject.activeSelf && x.transform.parent == CurrentWaypoint.transform.parent);
        if (emptyWood.Count <= 0)
        {
            counter += Time.deltaTime;
            if (counter < pauseBetweenTasks) return;
            counter = 0;

            ChangeState(ForesterState.NextWaypoint);
            index = Enumerable.Range(0, Waypoints.Count).Where(x => x != index).GetRandom();
            return;
        }

        // si on a pas de target pile de bois
        if (!_currentEmptyWood)
        {
            _currentEmptyWood = emptyWood.GetRandom();
            return;
        }

        // si on est assez proche du target wood pile
        if (Vector2.Distance(transform.position, _currentEmptyWood.transform.position) < distanceThatIsConsideredTarget)
        {
            _aiPath.destination = transform.position;
            counter += Time.deltaTime;
            if (counter < pauseBetweenTasks) return;
            counter = 0;

            _currentEmptyWood.Refill();
            _currentEmptyWood = null; 
            ChangeState(ForesterState.NextWaypoint);

            index = Enumerable.Range(0, Waypoints.Count).Where(x => x != index).GetRandom();
            return;
        }

        _aiPath.destination = _currentEmptyWood.transform.position;
    }

    void Extinguish()
    {
        // trouver les feux allumés qui sont dans la zone
        var lights = AllOpenLightsInZone;
        if (lights.Count <= 0)
        {
            _aiPath.destination = transform.position;
            counter += Time.deltaTime;
            if (counter < pauseBetweenTasks) return;
            counter = 0;
            ChangeState(ForesterState.RefillWood);
            return;
        }

        if (_currentLight == null)
        {
            _currentLight = lights.GetRandom();
            return;
        }

        // si on est assez proche du target wood pile
        if (Vector2.Distance(transform.position, _currentLight.transform.position) < distanceThatIsConsideredTarget)
        {
            _currentLight.GetComponent<InteractableLightFuel>().Extinguish();
            _currentLight = null;
            return;
        }

        _aiPath.destination = _currentLight.transform.position;
    }

    void NextWaypoint()
    {
        if (Vector2.Distance(CurrentWaypoint.transform.position, transform.position) < distanceThatIsConsideredTarget)
        {
            _aiPath.destination = transform.position;
            counter += Time.deltaTime;
            if (counter < pauseBetweenTasks) return;
            counter = 0;

            ChangeState(ForesterState.Extinguish);
            return;
        }
        _aiPath.destination = CurrentWaypoint.transform.position;
    }
}