using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public float enemySpeed = 10f;

	public EnemyOrchestrator enemyOrchestrator;
    public GameObject enemyPrefab;

	private float _startTime;
	private GameObject _player;

	void Start () {
		_startTime = Time.time;
		_player = GameObject.FindWithTag("Player");
        StartCoroutine(SpawnEnemy());
	}
	
    IEnumerator SpawnEnemy()
    {
        Vector2 position = RandomPointOnUnitCircle(30f);
		// TODO : Check if there is already an enemy at this position
		GameObject spawnedEnemy = Instantiate(enemyPrefab, new Vector3(position.x, 1f, position.y), Quaternion.identity);
	    enemyOrchestrator.AddEnemy(spawnedEnemy);

	    EnemyBehaviour behaviour = spawnedEnemy.GetComponent<EnemyBehaviour>();
	    behaviour.speed = enemySpeed;
	    behaviour.SetTarget(_player);
	    behaviour.enemyOrchestrator = enemyOrchestrator;

		yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnEnemy());
    }

    private Vector2 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        return new Vector2(x, y);
    }
}
