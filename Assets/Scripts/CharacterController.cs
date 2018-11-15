using UnityEngine;
using System.Linq;
using System.Collections;
using pkm.EventManager;
using System;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {
    public float cameraXSensitivity = 10f;
    public float cameraYSensitivity = 1f;

	public float speed = 2f;

    public GameObject[] lifeUI;
    public CanvasGroup endGamePanel;
    public AudioSource backgroundMusic;

    [HeaderAttribute("Lightning")]
    public float cooldown = 1.5f;
    public float lightningTravelingSpeed = 1f;

    public GameObject projectilePrefab;

    public Text killScoreText;
    public Text chainScoreText;

    private float _rotationY = 0f;
    private float _lastShot = -10f;
	private GameObject _activeProjectile;
	private Rigidbody _rb;
	private int _markersLeft = 0;
    private int _health = 3;
    private int _killScore = 0;
    private int _chainScore = 0;

    private void Start() {
        _rb = GetComponent<Rigidbody>();
		_markersLeft = 5;

		Cursor.lockState = CursorLockMode.Locked;

        EventManager.StartListening("OnEnemyKill", UpdateKillScore);
        EventManager.StartListening("OnChainEnd", UpdateChainScore);

        UpdateUi();

#if UNITY_ANDROID
        Input.gyro.enabled = true;
#endif
    }

    private void UpdateChainScore(dynamic obj)
    {
        if(obj.valueToAdd > _chainScore)
        {
            _chainScore = obj.valueToAdd;
            UpdateUi();
        }
    }

    private void UpdateKillScore(dynamic obj)
    {
        _killScore++;
        UpdateUi();
    }

    void UpdateUi()
    {
        killScoreText.text = _killScore.ToString();
        chainScoreText.text = _chainScore.ToString();
    }

    private static Quaternion GyroToUnity(Quaternion q)
    {
        return new Quaternion(q.x, q.y, -q.z, -q.w);
    }

    void Update() {
		// Camera rotation
        if(Time.timeScale != 0)
        {
            // TODO : Gyroscope support
#if UNITY_ANDROID
            Quaternion gyroRotation = GyroToUnity(Input.gyro.attitude);
            float rotation = -(transform.localEulerAngles.y + (GyroToUnity(Input.gyro.attitude).y / 2f) * cameraXSensitivity);
#else
            float rotation = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * cameraXSensitivity;
#endif
		    transform.localEulerAngles = new Vector3(0, rotation, 0);

            if (Input.GetMouseButtonDown(0) && Time.time > _lastShot + cooldown && _activeProjectile == null)
            {
                Shoot();
            }

            if (Input.GetMouseButtonDown(1))
            {
                Mark();
            }

            _rb.velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.Z))
            {
                _rb.velocity += transform.forward * speed;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                _rb.velocity -= transform.forward * speed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _rb.velocity += transform.right * speed;
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                _rb.velocity -= transform.right * speed;
            }
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
        lifeUI[_health].SetActive(false);

        if(_health == 0)
        {
            Time.timeScale = 0f;
            backgroundMusic.volume = 0.25f;
            StartCoroutine(ShowEndGamePanel());
        }
    }

    private IEnumerator ShowEndGamePanel()
    {
        float t = 0f;
        while (endGamePanel.alpha < 1)
        {
            t += Time.unscaledDeltaTime / 0.5f;
            endGamePanel.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }
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
