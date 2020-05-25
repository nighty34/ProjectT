using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading;
using UnityEngine;

using Debug = UnityEngine.Debug;


public class TankController : MonoBehaviour
{

    [Header("Generall")]

    public GameObject mainObject; 
    public GameObject targetedObject;
    public Camera thirdPersonCamera;
    public Camera cannonCamera;
    public Boolean isActive = false; //If is controlled by Player

    //Start positions for entering the tank
    public Transform animStartFront;
    public Transform animStartRear;

    [Header("Driving")]
    public float speed;
    public float rotationSpeed;

    //WheelCollider
    public List<WheelCollider> wheelsRight = new List<WheelCollider>();
    public List<WheelCollider> wheelsLeft = new List<WheelCollider>();

    //WheelTransforms
    public List<Transform> wheelTransformR = new List<Transform>();
    public List<Transform> wheelTransformL = new List<Transform>();


    //Engine Particle System
    public List<ParticleSystem> engineParticles = new List<ParticleSystem>();

    [Header("Turret")]
    public GameObject turretObject;
    public float turretRotation;
    [Header("Limits")]
    public Boolean limitElevation;
    [Range(0.0f, 180.0f)]
    public float up_limit;
    [Range(0.0f, 180.0f)]
    public float down_limit;
    public Boolean limitRotation;
    [Range(0.0f, 180.0f)]
    public float right_limit;
    [Range(0.0f, 180.0f)]
    public float left_limit;



    [Header("Cannon")]
    public GameObject cannonEnding;
    public GameObject cannonObject;
    public ParticleSystem muzzleflash;

    public GameObject shell;

    public float fireDelta = 0.5F;

    private float nextFire = 0.5F;
    private float myTime = 0.0f;
    private Rigidbody m_Rigidbody;

    private float motorTorque = 100f;



    private float inputFire = 0.5F;
    private float inputTime = 0.0f;




    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        muzzleflash.Stop();
    }


    void OnEnable()
    {
        m_Rigidbody.isKinematic = false;
    }
    // Start is called before the first frame update
    void Start(){
    }

    void Update(){

    }

    void FixedUpdate(){
        if (isActive) { //if is controlled by Player

            myTime = myTime + Time.deltaTime;   //Cannon Cooldown
            inputTime = inputTime + Time.deltaTime; //ChangeView Cooldown
        }


        if(wheelTransformL.Count>0 && wheelTransformR.Count>0){ //If wheelcolliders and wheeltransfroms are set
        UpdateWheels();
        }
        rotateTurret(targetedObject.transform.position, turretRotation); //Hull rotation


        rotateCannon(targetedObject.transform.position, turretRotation); //Cannon elevation

    }

    /*
     * Controll Method
     * is used by the characterControll to move the activ tank
     */
    public void Controll(float forwardmovement, float turninput, Boolean interact, Boolean fire, Boolean aim){
        Move(forwardmovement);
        Turn(turninput, forwardmovement);
        ShootCannon(fire);
        ChangeCamera(aim);
        
    }


    /*
     * Move Method
     * used to move the tank forward
     */
    private void Move(float movementInputValue){
        if (movementInputValue > 0.001){                        //If driving forwards
            foreach (WheelCollider coll in wheelsRight){
                coll.brakeTorque = 0f * Time.deltaTime;
            }
            foreach (WheelCollider coll in wheelsLeft){
                coll.brakeTorque = 0f * Time.deltaTime;
            }

            foreach (WheelCollider coll in wheelsRight){
                coll.motorTorque = movementInputValue * motorTorque * speed * Time.deltaTime;
            }
            foreach (WheelCollider coll in wheelsLeft){
                coll.motorTorque = movementInputValue * motorTorque * speed * Time.deltaTime;
            }
        }else if (movementInputValue < -0.001){                 //if driving backwards
            foreach (WheelCollider coll in wheelsRight){
                coll.brakeTorque = 0f * Time.deltaTime;
            }
            foreach (WheelCollider coll in wheelsLeft){
                coll.brakeTorque = 0f * Time.deltaTime;
            }

            foreach (WheelCollider coll in wheelsRight){
                coll.motorTorque = movementInputValue * motorTorque * speed * Time.deltaTime;
            }
            foreach (WheelCollider coll in wheelsLeft){
                coll.motorTorque = movementInputValue * motorTorque * speed * Time.deltaTime;
            }
        }else{                                                  //If noInput (Breaking)
            foreach (WheelCollider coll in wheelsRight){
                coll.brakeTorque = 3000000f * Time.deltaTime;
            }
            foreach (WheelCollider coll in wheelsLeft){
                coll.brakeTorque = 3000000f * Time.deltaTime;
            }
        }
    

         foreach (ParticleSystem part in engineParticles){      //DrivingparticleSystem controll

            part.startSpeed = 5 + (movementInputValue * 10);
         }

    }

    /*
     * Turn Method
     * used to turn the tank
     * Current rotation is achived by changing the velocity
     */
    private void Turn(float turnInputValue, float movementInputValue){
    
        if (movementInputValue >= 0) {                                          //If is driving forward
            float turn = turnInputValue * rotationSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }else{                                                                  //If is driving backwards
            float turn = -turnInputValue * rotationSpeed * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }


    /*
     * TurretRotation Method
     * used to turn the turretobj and all its childes
     *
     * TODO:
     * The current turret rotation is clamped to the local rotation. Because of that the turret turns in relation to the hull instead of the world. PLS FIX
     */
    private void rotateTurret(Vector3 target, float rotationSpeed){
        Vector3 localTargetPos = (target - turretObject.transform.position).normalized;
        localTargetPos.y = 0.0f;

        Vector3 clampedLocalVec2Target = localTargetPos;
        if (limitRotation){
            if (localTargetPos.x >= 0.0f)
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * right_limit, float.MaxValue);
            else
                clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * left_limit, float.MaxValue);
        }

        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
        Quaternion newRotation = Quaternion.RotateTowards(turretObject.transform.localRotation, rotationGoal, turretRotation * Time.deltaTime);

        turretObject.transform.localRotation = newRotation;
    }


    /*
     * GunElevation Method
     * used to elevate the gun
     * 
     * Same issue as with the turret rotation
     */
    private void rotateCannon(Vector3 target, float rotationSpeed){ //Elevate Cannon


        Vector3 localTargetPos = (target - cannonObject.transform.position).normalized;
        localTargetPos.x = 0.0f;


        Vector3 clampedLocalVec2Target = localTargetPos;
        if (localTargetPos.y >= 0.0f)
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * up_limit, float.MaxValue);
        else
            clampedLocalVec2Target = Vector3.RotateTowards(Vector3.forward, localTargetPos, Mathf.Deg2Rad * down_limit, float.MaxValue);

        // Create new rotation towards the target in local space.
        Quaternion rotationGoal = Quaternion.LookRotation(clampedLocalVec2Target);
        Quaternion newRotation = Quaternion.RotateTowards(cannonObject.transform.localRotation, rotationGoal, 2.0f * turretRotation * Time.deltaTime);

        // Set the new rotation of the base.
        cannonObject.transform.localRotation = newRotation;
    }


    /*
     * Shooting Method
     */
    private void ShootCannon(Boolean shootInput){
        if (shootInput && myTime > nextFire){                   //if reloaded
            nextFire = myTime + fireDelta;
            GameObject bullet = Instantiate(shell, cannonEnding.transform.position, cannonEnding.transform.rotation);
            muzzleflash.Play();
            bullet.GetComponent<Rigidbody>().AddForce(cannonEnding.transform.forward * 2000000);
            nextFire = nextFire - myTime;
            myTime = 0.0F;
        }

    }


    /*
     * ChangeCamera Method
     * called if player wants to switch to gunsight
     */
    private void ChangeCamera(Boolean aimInput){
        if (aimInput && inputTime > inputFire){
            inputFire = inputTime + 0.2f;

            if (thirdPersonCamera.enabled){
                thirdPersonCamera.enabled = false;
                cannonCamera.enabled = true;
            }else{
                cannonCamera.enabled = false;
                thirdPersonCamera.enabled = true;
            }
            inputFire = inputFire - inputTime;
            inputTime = 0f;
        }
    }


    /*
     * UpdateWheel Function
     * syncs wheelcollider and wheeltranforms
     */
    private void UpdateWheels(){
        var pos = Vector3.zero;
        var rot = Quaternion.identity;

        for(int i = 0; i < wheelsLeft.Count; i++){
            if(wheelsLeft[i] != null && wheelTransformL != null){
                wheelsLeft[i].GetWorldPose(out pos, out rot);
                wheelTransformL[i].position = pos;
                wheelTransformL[i].rotation = rot;
            }
        }

        
        for(int i = 0; i < wheelsRight.Count; i++){
            if(wheelsRight[i] != null && wheelTransformR != null){
                wheelsRight[i].GetWorldPose(out pos, out rot);
                wheelTransformR[i].position = pos;
                wheelTransformR[i].rotation = rot;
            }
        }
    }


    /*
     * Enable Method
     * called if character enters tank
     */
    public void Enable(){
        thirdPersonCamera.GetComponent<CameraControll>().enabled = true;
        cannonCamera.enabled = false;
        thirdPersonCamera.enabled = true;
        isActive = true;
    }

    /*
     * Disable Method
     * called when character leaves the tank
     */
    public void Disable(){
        isActive = false;
        thirdPersonCamera.GetComponent<CameraControll>().enabled = false;
        cannonCamera.enabled = false;
        thirdPersonCamera.enabled = false;
    }


    void calcDistance(){


        /*float distance;
        RaycastHit hit;
        Ray lookingRay = new Ray(cannonEnding.transform.position, Vector3.forward);

        movementInputValue = Input.GetAxis(m_MovementAxisName);
        turnInputValue = Input.GetAxis(m_TurnAxisName);


        /*if (Physics.Raycast(transform.position, transform.forward, out hit)) { 
            Debug.DrawRay(transform.position, transform.forward, Color.green);; 
        }

        //Debug.DrawRay(cannonEnding.transform.position, cannonEnding.transform.forward, Color.green);

        /*if (Physics.Raycast(lookingRay, out hit)){

            Debug.Log("Distance: " + hit.distance);
        }else{
            Debug.Log("Nothing");
        }*/

    }
}
