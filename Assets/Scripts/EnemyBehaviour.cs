using System.Collections;
using System.Collections.Generic;
using pkm.EventManager;
using UnityEngine;
using UnityEngine.Experimental.LowLevel;

public class EnemyBehaviour : MonoBehaviour {
	public float speed = 5f;
	public GameObject mark;
    public GameObject floatingScore;
    public GameObject sfxPlayer;

    public AudioClip hitSound;
    public AudioClip hurtSound;

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

                GameObject text = Instantiate(floatingScore, transform.position + new Vector3(0f, -0.5f, -1.5f), Quaternion.identity);
                text.GetComponent<TMPro.TextMeshPro>().text = projectile.GetCombo().ToString();
                PlaySound(hitSound, projectile.GetCombo());

                // Search closest enemy and set it as next target for the projectile
                GameObject nextTarget = enemyOrchestrator.FindClosestEnemyInRange(gameObject);
				if (nextTarget != null) {
					projectile.SetNextTarget(nextTarget);
				} else {
					EventManager.TriggerEvent("OnChainEnd", new { valueToAdd = projectile.GetCombo() });
					Destroy(other.gameObject);
				}

				EventManager.TriggerEvent("OnEnemyKill", new { valueToAdd = 1});
				
				enemyOrchestrator.RemoveEnemy(gameObject);
				Destroy(gameObject);
			} else {
				if (projectile.firstHit) {
                    Destroy(other.gameObject);
				}
			}
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			Debug.Log("HIT");
            PlaySound(hurtSound);
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

    private void PlaySound(AudioClip clip, int pitch=1)
    {
        GameObject sfx = Instantiate(sfxPlayer, transform.position, Quaternion.identity);
        AudioSource audio = sfx.GetComponent<AudioSource>();

        pitch = Mathf.Clamp(pitch, 1, 5);
        audio.pitch = pitch;

        audio.PlayOneShot(clip);
        Destroy(sfx, clip.length + 0.25f);
    }
}
