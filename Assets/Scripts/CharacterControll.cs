using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.AI;
using nightfury34;

namespace nightfury34{
    public class CharacterControll : MonoBehaviour
    {

        public GameObject mainframe;
        public float movementSpeed = 0f;

        private Camera camera;

        private TankController tankController;

        private CharacterController characterController;

        public PlayerInputs inputs {set; private get;}

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;

        private float gravity = 0f;

        private Boolean isInTank = false;

        private Boolean isGrounded;

        private Vector3 velocity;

        private NavMeshAgent navMeshAgent;

        private Boolean isEntering = false;


        // Start is called before the first frame update
        void Start(){
            camera = GetComponentInChildren<Camera>();
            characterController = GetComponent<CharacterController>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        // Update is called once per frame
        void Update(){
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if(isGrounded && velocity.y < 0){
                velocity.y = -2f;
            }
        }

        void FixedUpdate() { 
            if(!(inputs == null)){
                Controll();
            }

            velocity += Physics.gravity * Time.deltaTime;
        }

        /* 
        *  External Controll 
        */
        public void Controll(float forwardmovement, float turninput, Boolean interact, Boolean fire, Boolean enter){
            if(!isInTank){
                Movement(turninput, forwardmovement);
                //
                //Move(forwardmovement);
                //Rotate(turninput, forwardmovement);
            }
            TankUpdate(enter);
        }


        /*
        * Controll via PlayerInput
        */
        private void Controll(){
            if(!isInTank){
                //Move(inputs.PowerAxis);
                //Rotate(inputs.SteerAxis, inputs.PowerAxis);
                Movement(inputs.SteerAxis, inputs.PowerAxis);
            }
            TankUpdate(inputs.enterButton);
        }

        private void Movement(float x, float z){
            if(!isEntering){
                Vector3 move = transform.right * x + transform.forward * z;
                characterController.Move(move * movementSpeed * Time.deltaTime);
                characterController.Move(velocity * Time.deltaTime);
            }
        }

        /*private void Move(float movementInputValue){
            Vector3 movement = transform.forward * movementInputValue * movementSpeed * 1000 * Time.deltaTime;
            movement.y -= gravity * Time.deltaTime;
            //rb.velocity = movement;
        }*/

        private void Rotate(float turnInputValue, float movementInputValue){
            if (movementInputValue >= 0){
                float turn = turnInputValue * movementSpeed * Time.deltaTime;
                Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
                //rb.MoveRotation(rb.rotation * turnRotation);
            }else{
                float turn = -turnInputValue * movementSpeed  * Time.deltaTime;
                Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
                //rb.MoveRotation(rb.rotation * turnRotation);
            }
        }


        /*
        *  Get Tank nearby for entering
        */
        void OnTriggerEnter(Collider other){
            if(other.name.Contains("hitbox")){
            //print(other.transform.parent.parent.gameObject.name);
                TankController tc = other.transform.parent.parent.parent.gameObject.GetComponent<TankController>();
                if(!(tc == null)){
                    tankController = tc;
                }
            }
        }

        void OnTriggerExit(Collider other){
            if(!isInTank && !isEntering){
            tankController = null;
            }
        }



        /*
        *  Check if player wants to enter tank
        */
        private void TankUpdate(Boolean enter){
            if (!(tankController == null) && !(inputs == null)){ //if tankcontroller is set and inputs are set
                if(isInTank){ //if inside Tank
                    tankController.Controll(inputs.PowerAxis, inputs.SteerAxis, inputs.interactButton, inputs.fireButton, inputs.aimButton);
                }
                if (enter){
                    if(isInTank){
                        //GetComponentInChildren<MeshRenderer>().enabled = true;
                        camera.enabled = true;
                        tankController.Disable();
                        isInTank = false;
                    }else{
                        navMeshAgent.Warp(groundCheck.position);
                        navMeshAgent.destination = tankController.animStartFront.position;
                        isEntering = true;
                    }
                }

            }
            if(Vector3.Distance(groundCheck.position, navMeshAgent.destination) < 6 && isEntering){
                camera.enabled = false;
                tankController.Enable();
                //GetComponentInChildren<MeshRenderer>().enabled = false;
                isInTank = true;
                isEntering = false;
            }
            if(isEntering){
                navMeshAgent.destination = tankController.animStartFront.position;
            }
        }


    }


}

