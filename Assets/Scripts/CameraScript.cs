using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 10.0f;
    public float y_invert_modifier = -1.0f;
    public float pitch = 0f;
    public float yaw = 0f;
    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
      cam = this.GetComponent<Camera>();
      pitch = transform.eulerAngles.x;
      yaw = transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
      yaw += Input.GetAxis("Mouse X") * speed;
      pitch += Input.GetAxis("Mouse Y") * speed * y_invert_modifier;

      pitch = Mathf.Clamp(pitch,-90f,90f);

      transform.parent.rotation = Quaternion.Euler(0, yaw, 0); // rotation of parent (player body)
      cam.transform.rotation = Quaternion.Euler(pitch, yaw, 0); // rotation of Camera

    }
}
