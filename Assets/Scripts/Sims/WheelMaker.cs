using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMaker : MonoBehaviour
{

    public int gearAmount = 0;
    public float height = 1f;
    public GameObject activeObject;
    public GameObject template;
    public float radius = 5f;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        generateWheel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void createWheel(){
        float gearAngle = 360 / gearAmount;
        for (int i = 0; i < gearAmount; i++){
            CapsuleCollider cc = activeObject.AddComponent<CapsuleCollider>();
            cc.height = height;
        }
    }

    void generateWheel(){
        for (int i = 0; i < gearAmount; i++){
            float angle = i * Mathf.PI * 2 / gearAmount;
            float y = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(0, y, z);
            float angleDegrees = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(angleDegrees, 0, 0);
            Instantiate(template, pos + offset, rot).transform.SetParent(activeObject.transform);
        }

        for (int i = 0; i < gearAmount; i++){
            float angle = i * Mathf.PI * 2 / gearAmount;
            float y = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(0, y, z);
            float angleDegrees = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(angleDegrees, 0, 0);
            Instantiate(template, pos + -offset, rot).transform.SetParent(activeObject.transform);
        }
    }
}
