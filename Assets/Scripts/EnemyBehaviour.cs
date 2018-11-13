using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    private Rigidbody _rb;

	void Start () {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(MoveToPosition(EnemyOrchestrator.playerPosition, 5f));
    }

    IEnumerator MoveToPosition(Vector3 position, float timeToMove)
    {
        Vector3 currentPos = transform.position;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            _rb.MovePosition(Vector3.Lerp(currentPos, position, t));
            yield return null;
        }
    }
}
