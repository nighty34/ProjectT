using System.Collections;
using Pixelplacement;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Remoting.Messaging;
using System.Diagnostics;

using Debug = UnityEngine.Debug;
using System.Collections.Specialized;
using System.Dynamic;
using System.Reflection;

public class SplineTrack : MonoBehaviour
{
    public GameObject parentObject;
    public Spline path;
    public float lenght;
    public float wide;
    public GameObject trackPiece;
    public GameObject center;
    public bool inwards;

    private List<GameObject> track = new List<GameObject>();
    //private int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        path.GetPosition(100);
        createPlates();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void createPlates()
    {
        for(int i = 0; i < calcPieceAmount(path); i++)
        {
            Vector3 pos = calcPosition(i);
            Quaternion dir = Quaternion.LookRotation(calcDirection(i), Vector3.up); 
            GameObject activePart = Instantiate(trackPiece, pos, dir);
            track.Add(activePart);
            activePart.transform.SetParent(parentObject.transform);
            /*if (isflipped(activePart)) {
                Vector3 rot = activePart.transform.localScale;
                //rot.x = rot.x + 180;
                rot.y = -rot.y;
                activePart.transform.localScale = rot;
            }*/
            if (i > 0) setUpPiece(activePart, i-1);
            if (i == calcPieceAmount(path) - 1) setUpPiece(track[0], i-1);
        }
    }


    private bool isflipped(GameObject activeObject)
    {
        if (activeObject.transform.rotation.eulerAngles.y > 90) {
            return false;
        }
        else { 
            return true;
        }
    }

    private Vector3 calcPosition(int count)
    {
        float perc = (float)(1f /(path.Length / lenght)) * count;
        return path.GetPosition(perc);
    }

    private Vector3 calcDirection(int count)
    {
        float perc = (float)(1f / (path.Length / lenght)) * count;
        return path.GetDirection(perc);
    }

    private Vector3 calcFacing(int count)
    {
        float perc = (float)(1f / (path.Length / lenght)) * count;
        return path.Right(perc);
    }

    private int calcPieceAmount(Spline spline)
    {
        //Debug.Log(spline.Length);
        return (int)(Mathf.Round((spline.Length / lenght)));
    }

    void setUpPiece(GameObject activeObject, int counter)
    {
        HingeJoint hj = activeObject.GetComponent<HingeJoint>();
        hj.connectedBody = track[counter].GetComponent<Rigidbody>();
    }
}
