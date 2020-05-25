using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMaker : MonoBehaviour {

    public GameObject trackPiece;
    public GameObject centerOfMass;
    public Vector3 offset;
    public int count = 0;

    private int counter = 0;

    private List<GameObject> track = new List<GameObject>();

    // Start is called before the first frame update
    void Start(){
        //trackPiece.GetComponent<Rigidbody>().centerOfMass = centerOfMass.transform.position;
        CreateTrack();
        //setUpPiece(trackPiece, track.Count - 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void CreateTrack(){
        track.Add(trackPiece);
        Vector3 angle = new Vector3(0, 0, 0);
        createLenght(count, offset, angle);
        /*angle.x = -90;

        offset.y = 1;
        offset.z = offset.z - 0.35f;
        createLenght(1, offset, angle);

        offset.y = offset.z;
 
        offset.z = 0;
        createLenght(1, offset, angle);

       /* for (int i = 0; i < count; i++){
            GameObject activePiece = Instantiate(trackPiece, track[track.Count - 1].transform.position + offset, Quaternion.identity);
            setUpPiece(activePiece, i);
            activePiece.transform.SetParent(centerOfMass.transform);
            track.Add(activePiece);
        }*/
    }

    void setUpPiece(GameObject activeObject, int counter){
        HingeJoint hj = activeObject.GetComponent<HingeJoint>();
        hj.connectedBody = track[counter].GetComponent<Rigidbody>();
    }

    void createLenght(int limit, Vector3 actoffset, Vector3 rotation){
        for (int i = 0; i < limit; i++)
        {
            GameObject activePiece = Instantiate(trackPiece, track[track.Count - 1].transform.position + actoffset, Quaternion.Euler(rotation));
            setUpPiece(activePiece, counter);
            activePiece.transform.SetParent(centerOfMass.transform);
            track.Add(activePiece);
            counter++;
        }
    }
}
