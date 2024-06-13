using Pathfinding;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ForesterScript : MonoBehaviour
{
    public static event Action<ForesterState> Changed;
    [SerializeField]
    private float flicker = 0.01f;
    [SerializeField]
    private float _currentRange = 10;
    public float CurrentRange => _currentRange / 2;
    [SerializeField]
    private Transform _lightRange;

    public float distanceThatIsConsideredTarget = 0.6f;
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private float followPlayerSpeedMultiplier = 3;

    #region [AI stuff]
    private ForesterState _state;
    private WoodScript _currentEmptyWood;
    private LightScript _currentLight;

    private int index = 0;
    private bool SeesPlayer => Vector2.Distance(transform.position, _player.transform.position) < CurrentRange + _player.CurrentRange && _player.IsLit;
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
    private PlayerLight _player;
    private List<WaypointScript> Waypoints { get; set; } = new();
    private List<WoodScript> Woods { get; set; } = new();
    private List<LightScript> Lights { get; set; } = new();
    List<LightScript> AllOpenLightsInZone => Lights.FindAll(x => x.IsLit && x.transform.parent == CurrentWaypoint.transform.parent);
    
    private AIPath _aiPath;

    private void ChangeState(ForesterState state)
    {
        _state = ForesterState.FollowPlayer;
        Changed?.Invoke(_state);
    }
    private void Start()
    {
        _aiPath = GetComponent<AIPath>();
        _player = FindObjectOfType<PlayerLight>(true);
        Waypoints = FindObjectsOfType<WaypointScript>().ToList();
        Woods = FindObjectsOfType<WoodScript>(true).ToList();
        Lights = FindObjectsOfType<LightScript>(true).ToList();

    }

    private void Update()
    {
        _lightRange.localScale = 2 * CurrentRange * Vector3.one + flicker * UnityEngine.Random.value * Vector3.one;
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

        if (_state != state)
        {
            Debug.Log("state : " + _state);
        }
    }
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, CurrentRange);
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
            _player.Extinguish();
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
            Debug.Log("waypoint : " + CurrentWaypoint.name);
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
            _currentLight.Extinguish();
            _currentLight = null;
            return;
        }

        _aiPath.destination = _currentLight.transform.position;
    }

    void NextWaypoint()
    {
        if (Vector2.Distance(CurrentWaypoint.transform.position, transform.position) < distanceThatIsConsideredTarget)
        {
            ChangeState(ForesterState.Extinguish);
            return;
        }
        _aiPath.destination = CurrentWaypoint.transform.position;
    }
}