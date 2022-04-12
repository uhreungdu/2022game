using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public int _howTeamSpawn { get; private set; }

    public void SetSpawnTeam(int team)
    {
        _howTeamSpawn = team;
    }
}
