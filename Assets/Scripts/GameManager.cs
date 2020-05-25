using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nightfury34;

namespace nightfury34
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public List<GameObject> deactiveOnStart = new List<GameObject>();
        public List<PlayerInputs> playerInput = new List<PlayerInputs>();

        public PlayerSpawnSystem spawnSystem;

        void Awake()
        {
            Instance = this;
            playerInput.Add(GetComponentInChildren<PlayerInputs>());
            spawnSystem = GetComponentInChildren<PlayerSpawnSystem>();

        }

        void Start(){
            spawnSystem.SpawnPlayerA(playerInput[0]);
            foreach(GameObject obj in deactiveOnStart){
                obj.SetActive(false);
            }
        }

        void Update()
        {

        }

    }
}

