using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float speed = 10.0f;
    public float y_invert_modifier = -1.0f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      float vert_rot = Input.GetAxis("Mouse Y") * speed * y_invert_modifier;

      transform.Rotate(vert_rot,0,0);
    }
}
