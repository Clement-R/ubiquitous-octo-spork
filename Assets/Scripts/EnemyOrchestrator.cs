using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using pkm.EventManager;

public class EnemyOrchestrator : MonoBehaviour {
    
    public static GameObject playerReference;
    public static Vector3 playerPosition;

	public float maxDistanceBetweenTargets = 10f;

	private List<EnemyBehaviour> enemies = new List<EnemyBehaviour>();

	void Start () {
        playerReference = GameObject.FindGameObjectWithTag("Player");
        playerPosition = playerReference.transform.position;
	}

	public void AddEnemy(GameObject enemy) {
		enemies.Add(enemy.GetComponent<EnemyBehaviour>());
	}

	public void RemoveEnemy(GameObject enemy) {
        EventManager.TriggerEvent("EnemyKill", new { });
		enemies.Remove(enemies.Find(e => e.gameObject == enemy));
	}

	public GameObject FindClosestEnemyInRange(GameObject enemy) {
		GameObject closest = null;
		float minDistance = 1000f;
		float distance = 0f;

		foreach (EnemyBehaviour target in enemies) {
			if (target.gameObject != enemy && target.IsMarked) {
				distance = SimpleDistance(enemy.transform.position, target.transform.position);
				if (distance < minDistance) {
					if (Vector3.Distance(enemy.transform.position, target.transform.position) <= maxDistanceBetweenTargets) {
						minDistance = distance;
						closest = target.gameObject;
					}
				}
			}
		}

		return closest;
	}

	float SimpleDistance(Vector3 a, Vector3 b) {
		return (Mathf.Pow((b.x - a.x), 2) + Mathf.Pow((b.z - a.z), 2)) ;
	}
}
