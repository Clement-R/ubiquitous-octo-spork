using UnityEngine;

public class CharacterController : MonoBehaviour {
    public float cameraXSensitivity = 10f;
    public float cameraYSensitivity = 1f;

    private float _rotationY = 0f;

    void Start () {
    }
    
    void Update () {
        // Camera rotation
        float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * cameraXSensitivity;
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
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
