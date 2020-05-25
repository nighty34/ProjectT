using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

using Debug = UnityEngine.Debug;

public class Shell : MonoBehaviour


{
    // Start is called before the first frame update
    public AnimationCurve penetration = AnimationCurve.Linear(0, 1000, 1000, 0);
    private Rigidbody rb;

    private Transform obj1;
    private Transform obj2;

    private Vector3 pos1;
    private Vector3 pos2;
    private Vector3 pos3;
    private float armor;
    private Vector3 direction;

    private bool drawLine = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        pos1 = transform.position;
        direction = this.transform.forward;
    }

    void Update(){
        //Debug.DrawRay(pos2, direction * 1000, Color.white);

    }

    void FixedUpdate(){
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 10)){       //if shell is about to hit collider
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            obj1 = hit.transform;
            pos2 = hit.point;
            //direction = this.transform.forward;
            calcThinkness(hit.point);
        }else{
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 25, Color.white); 
        }

        if(drawLine){
            Debug.DrawRay(pos2, transform.TransformDirection(Vector3.forward) * armor * 1000000, Color.red);
            print(armor);
        }
    }


    /*
     * CalculateArmorThicness Method
     * calculates armorvalue
     */
    private float calcThinkness(Vector3 enty){
        
        RaycastHit hit;
        if (Physics.Raycast(enty, direction, out hit, Mathf.Infinity))
        {   
            armor = hit.distance;
            obj2 = hit.transform;
            if(obj1.name == obj2.name){
                if(doesPen(hit.distance)){
                    print("Pened: " + hit.transform.name + " Distance: " + Vector3.Distance(pos1, pos2) + " Armor: " + hit.distance * 10000000 + "mm Possible: " + penetration.Evaluate(Vector3.Distance(pos1, pos2)) );
                    rb.isKinematic = true;
                    drawLine = true;
                }else{
                    print("NoPen: " + hit.transform.name + " Distance: " + Vector3.Distance(pos1, pos2) + " Armor: " + hit.distance * 10000000 + "mm Possible: " + penetration.Evaluate(Vector3.Distance(pos1, pos2)));
                    rb.isKinematic = true;
                    drawLine = true;
                }
            }
        }
        return 0f;
    }


    /*
     * Test if penetrades armor;
     *
     */
    private bool doesPen(float armor){
        float flownDistance = Vector3.Distance(pos1, pos2);
        if(penetration.Evaluate(flownDistance)>=armor * 10000000){
            return true;
        }
        return false;
    }
}
