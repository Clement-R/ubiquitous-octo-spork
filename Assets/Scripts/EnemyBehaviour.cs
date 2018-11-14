using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

public class EnemyBehaviour : MonoBehaviour {
	public float speed = 5f;
	public GameObject mark;

	public bool IsMarked {
		get { return _marked; }
	}

	[HideInInspector]
	public EnemyOrchestrator enemyOrchestrator;

    private Rigidbody _rb;
	private GameObject _target;
	private bool _marked = false;

	public void SetTarget(GameObject player) {
		_target = player;
	}

	public void SetAsMarked() {
		_marked = true;
		mark.SetActive(true);
	}

	private void Awake() {
		_rb = GetComponent<Rigidbody>();
		mark.SetActive(false);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.collider.CompareTag("Projectile") && IsMarked) {
			ProjectileBehaviour projectile = other.collider.gameObject.GetComponent<ProjectileBehaviour>();

			if (IsMarked) {
				projectile.firstHit = false;

				// Search closest enemy and set it as next target for the projectile
				GameObject nextTarget = enemyOrchestrator.FindClosestEnemyInRange(gameObject);
				if (nextTarget != null) {
					projectile.SetNextTarget(nextTarget);
				} else {
					Destroy(other.gameObject);
				}

				// TODO : Play animation or FX
				enemyOrchestrator.RemoveEnemy(gameObject);
				Destroy(gameObject);
			} else {
				if (projectile.firstHit) {
					Destroy(other.gameObject);
				}
			}
		}
	}

	private void FixedUpdate() {
		if (_target != null) {
			transform.LookAt(_target.transform, Vector3.up);
			Vector3 direction = (_target.transform.position - transform.position).normalized;
			direction.y = 0f;
			_rb.velocity = direction * speed;
		}
	}
}
