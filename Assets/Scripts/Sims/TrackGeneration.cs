using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class TrackGeneration : MonoBehaviour
{
    public List<GameObject> wheels = new List<GameObject>();

    public float radius = 0f;
    public Vector3 position;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        calcTrackLenght();
    }


    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    private void calcTrackLenght()
    {
        float angle = 0f;
        float lenght = 0f;
        for (int i = 0; i<wheels.Count; i++)
        {
            int sec = adjustCtr(i-1, wheels.Count - 1);
            lenght = lenght + calcPointsBetweenVectors(wheels[i].transform.position, wheels[sec].transform.position);
            lenght = lenght + calcWheel(radius, calcAngleBetweenVecors(wheels[i].transform.position, wheels[sec].transform.position));
            angle = angle + calcAngleBetweenVecors(wheels[i].transform.position, wheels[sec].transform.position);
        }
        //Debug.Log("Lenght: " + lenght + " Angle: " + angle);
    }

    //
    private float calcPointsBetweenVectors(Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(vec1, vec2);
    }

    private int adjustCtr(int i, int max)
    {
        if ((i - 1) < 0)
        {
            return max;
        } else {
            return i;
        }
    }

    private float calcWheel(float radius, float degree)
    {
        float floatPi = (float)Math.PI;
        return (radius * 2f * floatPi) / 360f * degree;
    }

    private float calcAngleBetweenVecors(Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Angle(vec1, vec2);
    }


    //TODO: Adjust
    /*void generateWheel()
    {
        for (int i = 0; i < gearAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / gearAmount;
            float y = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(0, y, z);
            float angleDegrees = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(angleDegrees, 0, 0);
            Instantiate(template, pos + offset, rot).transform.SetParent(activeObject.transform);
        }

        for (int i = 0; i < gearAmount; i++)
        {
            float angle = i * Mathf.PI * 2 / gearAmount;
            float y = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            Vector3 pos = transform.position + new Vector3(0, y, z);
            float angleDegrees = angle * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.Euler(angleDegrees, 0, 0);
            Instantiate(template, pos + -offset, rot).transform.SetParent(activeObject.transform);
        }
    }*/
}
