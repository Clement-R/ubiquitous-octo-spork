using UnityEngine;
using System.Linq;

public class CharacterController : MonoBehaviour {
    public float cameraXSensitivity = 10f;
    public float cameraYSensitivity = 1f;

    public int startHealth = 5;
	public float speed = 2f;

    [HeaderAttribute("Lightning")]
    public float cooldown = 1.5f;
    public float lightningTravelingSpeed = 1f;

    public GameObject projectilePrefab;

    private float _rotationY = 0f;
    private float _lastShot = -10f;
	private GameObject _activeProjectile;
	private Rigidbody _rb;
	private int _markersLeft = 0;
    private int _health = 5;

	private void Start() {
        _health = startHealth;
        _rb = GetComponent<Rigidbody>();
		_markersLeft = 5;

		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update() {
		// Camera rotation
		float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * cameraXSensitivity;
		transform.localEulerAngles = new Vector3(0, rotationX, 0);

		// TODO : Gyroscope support

		if (Input.GetMouseButtonDown(0) && Time.time > _lastShot + cooldown && _activeProjectile == null) {
			Shoot();
		}

		if (Input.GetMouseButtonDown(1)) {
			Mark();
		}

		_rb.velocity = Vector3.zero;
		if (Input.GetKey(KeyCode.Z)) {
			_rb.velocity += transform.forward * speed;
		} else if (Input.GetKey(KeyCode.S)) {
			_rb.velocity -= transform.forward * speed;
		}

		if (Input.GetKey(KeyCode.D)) {
			_rb.velocity += transform.right * speed;
		} else if (Input.GetKey(KeyCode.Q)) {
			_rb.velocity -= transform.right * speed;
		}
	}

	void Shoot()
    {
        _lastShot = Time.time;

	    _activeProjectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
	    _activeProjectile.GetComponent<Rigidbody>().velocity = transform.forward * _activeProjectile.GetComponent<ProjectileBehaviour>().speed;
	}

	void Mark()
	{
		if (_markersLeft > 0) {
			// Cast a ray in forward direction and filter to only get enemies
			RaycastHit hit = GetHittedEnemy();
			if (hit.collider != null) {
				hit.collider.gameObject.GetComponent<EnemyBehaviour>().SetAsMarked();
				_markersLeft--;
				Debug.Log(_markersLeft);
			}
		}
	}

	RaycastHit GetHittedEnemy() {
		// Cast a ray in forward direction and filter to only get enemies
		RaycastHit hit;
		Debug.DrawRay(transform.position, transform.forward * 31f, Color.red);
		if (Physics.Raycast(transform.position, transform.forward, out hit, 31f, LayerMask.GetMask("Enemy"))) {
			Debug.DrawRay(transform.position, transform.forward * 31f, Color.red);
			Debug.Log(hit.collider.gameObject.name);
		}

		return hit;
	}

	public void AddMarker() {
		_markersLeft++;
	}

    public void Hit()
    {
        _health--;
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }
}
