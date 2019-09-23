using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{
    public float fuel = 100f;
    public float refuel_wait = 0.0f;
    public float base_speed = 10.0f;
    public float speed = 0.0f;
    public float jump = 100.0f;
    public float x_invert_modifier = 1.0f;
    public float distToGround = 0.0f;
    public Rigidbody capsule;
    public Text fuel_text;

    public AudioSource Mech_Boost_Slide_01;
    public AudioSource Mech_Boost_Hover_Launch_01;
    public AudioSource Mech_Movement_Rotate_YAxis_DirectionChange_01_a;
    public AudioSource Mech_Boost_Hover_Loop_01;
    public AudioSource Mech_Boost_Hover_EngineOff_01;

    // Start is called before the first frame update
    void Start()
    {
        fuel_text = GameObject.Find("Fuel").GetComponent<Text>();
        capsule = GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    bool IsGrounded()
    {
      return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

        fuel_text.text = "Fuel: " + (int)fuel;
        Debug.Log(fuel);

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && fuel >= 0.5f){
          speed = 2 * base_speed;
          fuel -= 0.5f;
          if(!Mech_Boost_Hover_Loop_01.isPlaying){
            Mech_Boost_Hover_Loop_01.Play();
          }
        }
        else{
          speed = base_speed;
          if(Mech_Boost_Hover_Loop_01.isPlaying){
            Mech_Boost_Hover_Loop_01.Stop();
            Mech_Boost_Hover_EngineOff_01.Play();
          }
        }

        float z_trans = Input.GetAxis("Vertical") * speed;
        float x_trans = Input.GetAxis("Horizontal") * speed;

        x_trans *= Time.deltaTime;
        z_trans *= Time.deltaTime;

        float horz_rot = Input.GetAxis("Mouse X") * speed * x_invert_modifier;

        transform.Rotate(0,horz_rot,0);

        transform.Translate(x_trans,0,z_trans);

        if(Input.GetKeyDown("space") && IsGrounded() && fuel >= 15f ){
          capsule.AddForce(transform.up * jump, ForceMode.Impulse);
          fuel -= 15f;
          refuel_wait = 0.0f;
          Mech_Boost_Hover_Launch_01.Play();
        }

        if(Input.GetKey("space") && Input.GetKey(KeyCode.LeftShift) && fuel >= 1f){
          capsule.AddForce(transform.up * 150f);
          refuel_wait = 0.0f;
          fuel -= 1f;
          if(!Mech_Boost_Hover_Loop_01.isPlaying){
            Mech_Boost_Hover_Loop_01.Play();
          }
        }
        if(!Input.GetKey("space") && !Input.GetKey(KeyCode.LeftShift) && Mech_Boost_Hover_Loop_01.isPlaying){
          Mech_Boost_Hover_Loop_01.Stop();
          Mech_Boost_Hover_EngineOff_01.Play();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Horizontal") < 0 && fuel >= 35){
          capsule.AddForce(-transform.right * 100f, ForceMode.Impulse);
          refuel_wait = 0.0f;
          fuel -= 35f;
          Mech_Boost_Slide_01.Play();
        }
        else if(Input.GetKeyDown(KeyCode.LeftShift) && Input.GetAxis("Horizontal") > 0 && fuel >= 35){
          capsule.AddForce(transform.right * 100f, ForceMode.Impulse);
          refuel_wait = 0.0f;
          fuel -= 35f;
          Mech_Boost_Slide_01.Play();
        }

        if(!Input.GetKeyDown(KeyCode.LeftShift)){
          refuel_wait += Time.fixedDeltaTime;
          if(refuel_wait > 0.5f && fuel <= 100f){
            fuel += 10f * Time.fixedDeltaTime;
          }
        }

        capsule.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
    }
}
