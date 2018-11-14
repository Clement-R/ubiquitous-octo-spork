using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour {
	public float speed = 5f;
	public float afterBounceSpeed = 15f;
	public int numberOfBounce = 5;
	public float timeToLive = 3f;

	public bool firstHit = true;

	private int _remainingBounces;
	private Rigidbody _rb;
	private GameObject _target;
	private float _startTime;
    private int _combo = 1;

	private void Start() {
		_remainingBounces = numberOfBounce;
		_rb = GetComponent<Rigidbody>();
		_startTime = Time.time;
	}

	private void Update() {
		// If _target is null then we've never hit an enemy
		if (Time.time >= _startTime + timeToLive && _target == null) {
			Destroy(gameObject);
		}
	}

	private void FixedUpdate() {
		if (_target != null) {
			transform.LookAt(_target.transform, Vector3.up);
			Vector3 direction = (_target.transform.position - transform.position).normalized;
			direction.y = 0f;
			_rb.velocity = direction * afterBounceSpeed;
		}
	}

	public bool CanBounce() {
		_remainingBounces--;
		return _remainingBounces > 0;
	}

    public int GetCombo()
    {
        return _combo;
    }

	public void SetNextTarget(GameObject target) {
        _combo++;
        this._target = target;
	}
}
