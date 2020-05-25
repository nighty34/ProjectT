using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nightfury34;

namespace nightfury34
{

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public List<PlayerInputs> playerInput = new List<PlayerInputs>();

        public PlayerSpawnSystem spawnSystem;

        void Awake()
        {
            Instance = this;
            playerInput.Add(GetComponentInChildren<PlayerInputs>());
            spawnSystem = GetComponentInChildren<PlayerSpawnSystem>();

        }

        void Start(){
        }

        void Update()
        {

        }

    }
}

