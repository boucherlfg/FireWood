using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    [SerializeField]
    private float speed = 3;
    private Rigidbody2D _rigidBody;
    private InputSystem input;
    // Start is called before the first frame update
    void Start()
    {
        input = ServiceManager.Instance.Get<InputSystem>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var move = input.Move.normalized;
        _rigidBody.velocity = speed * (Vector3)move;
    }
}
