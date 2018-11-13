using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOrchestrator : MonoBehaviour {
    
    public static GameObject playerReference;
    public static Vector3 playerPosition;
	
	void Start () {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerPosition = playerReference.transform.position;
    }
}
