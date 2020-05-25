using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCreator : MonoBehaviour {

    public Transform trackPiece;
    public int count;
    public Vector3 offset;

    private float width;
    private float lenght;

    private List<Transform> trackpieces = new List<Transform>();

    // Start is called before the first frame update
    void Start() {
        trackpieces.Add(trackPiece);
        createTrack();
        //connectJoints(trackpieces[0]);
    }

    // Update is called once per frame
    void Update() {

    }


    void createTrack() {
        for (int i = 0; i < count; i++) {
            Transform result = Instantiate(trackPiece, trackPiece.position + offset, Quaternion.identity);
            offset = offset + offset;
            //Vector3 newRotation = new Vector3(trackPiece.eulerAngles.x, trackPiece.eulerAngles.x, trackPiece.eulerAngles.z);
            //result.rotation = trackpieces[trackpieces.Count - 1].rotation;
            //result.eulerAngles = newRotation;
            setUp(result);
            trackpieces.Add(result);
        }
    }

    void setUp(Transform transform){
        //transform.gameObject.AddComponent<Rigidbody>();
    }

    void connectJoints(Transform transform){
        HingeJoint hj = transform.GetComponent<HingeJoint>();
        hj.connectedBody = trackpieces[trackpieces.Count - 1].GetComponent<Rigidbody>();
    }
}
