using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirstPersonMovement : MonoBehaviour
{

    public System.Random rand = new System.Random();

    public float fuel = 150f;
    public float refuel_wait = 0.0f;
    public float base_speed = 10.0f;
    public float speed = 0.0f;
    public float jump = 100.0f;
    public float yaw = 0f;
    public float pitch = 0f;
    public float x_invert_modifier = 1.0f;
    public float distToGround = 0.0f;
    public Rigidbody capsule;
    public Text fuel_text;

    AudioSource basic_audio_source;
    public AudioSource Mech_Boost_Slide_01;
    public AudioSource Mech_Boost_Hover_Launch_01;
    public AudioSource Mech_Movement_Rotate_YAxis_DirectionChange_01_a;
    public AudioSource Mech_Boost_Hover_Loop_01;
    public AudioSource Mech_Boost_Hover_EngineOff_01;
    public AudioClip landing;

    public AudioClip[] walking_sfx = new AudioClip[4];
    public AudioClip walking_1;
    public AudioClip walking_2;
    public AudioClip walking_3;
    public AudioClip walking_4;

    float walk_timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        basic_audio_source = GetComponent<AudioSource>();
        fuel_text = GameObject.Find("PlayerFuel").GetComponent<Text>();
        capsule = GameObject.Find("PlayerBody").GetComponent<Rigidbody>();
        distToGround = GetComponent<Collider>().bounds.extents.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        walking_sfx[0] = walking_1;
        walking_sfx[1] = walking_2;
        walking_sfx[2] = walking_3;
        walking_sfx[3] = walking_4;
    }

    bool IsGrounded()
    {
      return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {

        fuel_text.text = "Fuel: " + (int)fuel;

        float z_trans = Input.GetAxis("Vertical") * speed;
        float x_trans = Input.GetAxis("Horizontal") * speed;

        x_trans *= Time.deltaTime;
        z_trans *= Time.deltaTime;

        transform.Translate(x_trans,0,z_trans);

        if(Input.GetKey(KeyCode.LeftShift) && Input.GetAxis("Vertical") > 0 && fuel >= 0.5f){
          speed = 2 * base_speed;
          fuel -= 0.5f;
          if(!Mech_Boost_Hover_Loop_01.isPlaying){
            Mech_Boost_Hover_Loop_01.Play();
          }
        }
        else{
          speed = base_speed;
          if(Mech_Boost_Hover_Loop_01.isPlaying && !Input.GetKey("space")){
            Mech_Boost_Hover_Loop_01.Stop();
            Mech_Boost_Hover_EngineOff_01.Play();
          }
        }

        if((Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 ) && !Input.GetKey(KeyCode.LeftShift) && IsGrounded()){
          if(walk_timer >= 0.6f){
            walk_timer = 0f;
            basic_audio_source.PlayOneShot(walking_sfx[rand.Next(0,4)],1f);
          }
          else{
            walk_timer += Time.fixedDeltaTime;
          }
        }

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
          if(refuel_wait > 0.5f && fuel <= 150f){
            fuel += 20f * Time.fixedDeltaTime;
          }
        }

        capsule.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.relativeVelocity.magnitude > 2){
            basic_audio_source.PlayOneShot(landing, 0.25f);
        }
    }
}
