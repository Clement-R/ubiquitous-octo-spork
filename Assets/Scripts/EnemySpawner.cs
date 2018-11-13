using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject enemyPrefab;

	void Start () {
        StartCoroutine(SpawnEnemy());
	}
	
    IEnumerator SpawnEnemy()
    {
        Vector2 position = RandomPointOnUnitCircle(30f);
        // TODO : Check if there is already an enemy at this position
        Instantiate(enemyPrefab, new Vector3(position.x, 1f, position.y), Quaternion.identity);

        yield return new WaitForSeconds(1f);
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
