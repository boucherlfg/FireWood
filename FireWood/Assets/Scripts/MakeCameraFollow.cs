using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeCameraFollow : MonoBehaviour
{
    [SerializeField]
    private float followSpeed = 3;
    private Camera _mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        var pos = _mainCamera.transform.position;
        var z = pos.z;
        pos = Vector2.Lerp(pos, transform.position, followSpeed * Time.deltaTime);
        pos.z = z;
        _mainCamera.transform.position = pos;
    }
}
