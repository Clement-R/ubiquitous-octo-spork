using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

public class EnemyBehaviour : MonoBehaviour {
	public float speed = 5f;
	public GameObject mark;
    public GameObject floatingScore;

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
		if (other.collider.CompareTag("Projectile")) {
			ProjectileBehaviour projectile = other.collider.gameObject.GetComponent<ProjectileBehaviour>();

			if (IsMarked) {
				projectile.firstHit = false;
                Debug.Log("Shot hit");

                GameObject text = Instantiate(floatingScore, transform.position + new Vector3(0f, -0.5f, -1.5f), Quaternion.identity);
                text.GetComponent<TMPro.TextMeshPro>().text = projectile.GetCombo().ToString();

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
                    Debug.Log("Shot hit unmarked");
                    Destroy(other.gameObject);
				}
			}
		} else if(other.collider.CompareTag("Player"))
        {
            Debug.Log("HIT");
            other.gameObject.GetComponent<CharacterController>().Hit();
            enemyOrchestrator.RemoveEnemy(gameObject);
            Destroy(gameObject);
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
