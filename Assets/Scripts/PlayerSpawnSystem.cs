using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using nightfury34;

public class PlayerSpawnSystem : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab = null;

    [SerializeField] List<Transform> spawnPointsTeamA = new List<Transform>();
    [SerializeField] List<Transform> spawnPointsTeamB = new List<Transform>();
    
    //List<Transform>[] teamspawns = new List<Transform>[1];
    
    int teamARot = 0;
    int teamBRot = 0;

    public void SpawnPlayerA(){
        Instantiate(playerPrefab, spawnPointsTeamA[teamARot].transform.position, spawnPointsTeamA[teamARot].transform.rotation, this.transform);
        //active Bot behavoir
        teamARot++;
    }


    public void SpawnPlayerB(){
        Instantiate(playerPrefab, spawnPointsTeamB[teamBRot].transform.position, spawnPointsTeamB[teamBRot].transform.rotation, this.transform);
        //active Bot behavoir;
        teamBRot++;
    }

    public void SpawnPlayerA(PlayerInputs inputs){
        GameObject player = Instantiate(playerPrefab, spawnPointsTeamA[teamARot].transform.position, spawnPointsTeamA[teamARot].transform.rotation, this.transform);
        CharacterControll cc = player.gameObject.GetComponent<CharacterControll>();
        cc.inputs = inputs; 
        teamARot++;
    }


    public void SpawnPlayerB(PlayerInputs inputs){
        GameObject player = Instantiate(playerPrefab, spawnPointsTeamB[teamBRot].transform.position, spawnPointsTeamB[teamBRot].transform.rotation, this.transform);
        CharacterControll cc = player.gameObject.GetComponent<CharacterControll>();
        cc.inputs = inputs;
        teamBRot++;
    }




}
