using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerSpawner : MonoBehaviour {

	public GameObject markPrefab;

	void Start() {
		StartCoroutine(SpawnMark());
	}

	IEnumerator SpawnMark() {
		Vector2 position = Random.insideUnitCircle * 30f;
		
		Instantiate(markPrefab, new Vector3(position.x, 1f, position.y), Quaternion.identity);
		
		yield return new WaitForSeconds(6f);
		StartCoroutine(SpawnMark());
	}
}