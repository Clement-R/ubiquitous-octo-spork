using UnityEngine;
using System.Linq;

public class CharacterController : MonoBehaviour {
    public float cameraXSensitivity = 10f;
    public float cameraYSensitivity = 1f;

    [HeaderAttribute("Lightning")]
    public float cooldown = 1.5f;
    public float lightningTravelingSpeed = 1f;

    public GameObject projectilePrefab;

    private float _rotationY = 0f;
    private float _lastShot = -10f;
    
    void Update () {
        // Camera rotation
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * cameraXSensitivity;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);

        // TODO : Gyroscope support

        if(Input.GetMouseButtonDown(0) && Time.time > _lastShot + cooldown)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        _lastShot = Time.time;

        GameObject projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
        projectile.GetComponent<Rigidbody>().velocity = transform.forward * 5f;

        // Cast a ray in forward direction and filter to only get
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.forward * 31f, Color.red);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 31f, LayerMask.GetMask("Enemy"))) {
            Debug.DrawRay(transform.position, transform.forward * 31f, Color.red);
            Debug.Log(hit.collider.gameObject.name);
        }

        // TODO : can't shoot while a lightning is growing
        // TODO : hit detect enemy
        // TODO : if no hit stop growing after a given max length
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
