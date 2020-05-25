using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraControll : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 uptilt;
        

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    void LateUpdate()
    {

        offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * smoothSpeed, Vector3.up) * offset;
        offset = Quaternion.AngleAxis(-Input.GetAxis("Mouse Y") * smoothSpeed, Vector3.right) * offset;
        transform.position = target.position + offset;
        transform.LookAt(target.position + uptilt);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
