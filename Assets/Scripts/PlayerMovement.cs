using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public System.Random rand = new System.Random();

    public AudioSource engines_sfx_src;
		public AudioSource engines_loop_src;
		public AudioSource legs_sfx_src;

    AudioClip[] walking_sfx = new AudioClip[4];
    public AudioClip walking_1;
    public AudioClip walking_2;
    public AudioClip walking_3;
    public AudioClip walking_4;

    AudioClip[] landing_sfx = new AudioClip[2];
    public AudioClip landing_1;
    public AudioClip landing_2;

		public AudioClip engineOff_sfx;
		public AudioClip engineOn_sfx;
		public AudioClip engineStrafe_sfx;
		public AudioClip engineLoop_sfx;

		AudioClip[] slide_sfx = new AudioClip[4];
		public AudioClip slide_floor_1;
		public AudioClip slide_floor_2;
		public AudioClip slide_floor_3;
		public AudioClip slide_floor_4;

    public float fuel = 150f;
    float refuel_wait = 0f;
    Text UI_fuel;

		float walk_timer = 0;

    public float base_speed = 10f;
		public float boost_multiplier = 3f;
		float move_speed = 0f;

    public float jump_force = 0f;

    float to_ground = 0f;
    float z_movement;
    float x_movement;

    Rigidbody PlayerBody;

		bool jump_modifier_1 = false;
		bool jump_modifier_2 = false;
		bool is_moving = false;
		bool is_strafing = false;
		bool is_boosting = false;
		bool is_hovering = false;
		bool is_grounded = true;


    // Start is called before the first frame update
    void Start()
    {
        to_ground = GetComponent<Collider>().bounds.extents.y;

        UI_fuel = GameObject.Find("PlayerFuel").GetComponent<Text>();
        PlayerBody = GameObject.Find("PlayerBody").GetComponent<Rigidbody>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        walking_sfx[0] = walking_1;
        walking_sfx[1] = walking_2;
        walking_sfx[2] = walking_3;
        walking_sfx[3] = walking_4;

				slide_sfx[0] = slide_floor_1;
				slide_sfx[1] = slide_floor_2;
				slide_sfx[2] = slide_floor_3;
				slide_sfx[3] = slide_floor_4;

				landing_sfx[0] = landing_1;
				landing_sfx[1] = landing_2;
    }

    bool IsGrounded(){
        return Physics.Raycast(transform.position, -Vector3.up, to_ground + 0.1f);
    }

    void UpdateUI(){
        UI_fuel.text = "Fuel: " + (int)fuel;
    }

		void GetMovementModifiers(){
			jump_modifier_1 = Input.GetKeyDown("space") || jump_modifier_1;
			jump_modifier_2 = (Input.GetKeyDown("space") || jump_modifier_2) && jump_modifier_1 ? true : false;
			is_hovering = Input.GetKey("space") && jump_modifier_2 ? true : false;
			is_boosting = Input.GetKey(KeyCode.LeftShift) && z_movement > 0 && x_movement == 0 ? true : false;
			is_moving = z_movement != 0 ? true : (x_movement != 0 ? true : false);
			is_strafing = (x_movement != 0 && Input.GetKeyDown(KeyCode.LeftShift)) ? true : false;
			is_grounded = IsGrounded();
		}

    void UpdateMovement(){
				z_movement *= move_speed;
				x_movement *= move_speed;

				z_movement *= Time.deltaTime;
				x_movement *= Time.deltaTime;

				transform.Translate(x_movement,0,z_movement);
    }

		void Jump(){
				jump_force = 20f;
				engines_sfx_src.PlayOneShot(engineOn_sfx,1f);
				PlayerBody.AddForce(Vector3.up * jump_force, ForceMode.VelocityChange);
				fuel -= 15f;
				refuel_wait = 0f;
		}

		void Hover(){
			PlayerBody.AddForce(Vector3.up * 25f, ForceMode.Acceleration);
			fuel -= 30f * Time.deltaTime;
			refuel_wait = 0f;

		}

		void Strafe(){
			float direction = x_movement < 0 ? -20f : 20f;
			PlayerBody.AddForce(Vector3.right * direction, ForceMode.VelocityChange);
			fuel -= 35f;
			refuel_wait = 0f;
			if(is_grounded){
				engines_sfx_src.PlayOneShot(slide_sfx[rand.Next(0,4)],1f);
			}
			else{
				engines_sfx_src.PlayOneShot(engineStrafe_sfx,1f);
			}
		}

    // Update is called once per frame
    void Update(){

				z_movement = Input.GetAxis("Vertical");
				x_movement = Input.GetAxis("Horizontal");

        UpdateUI();
				GetMovementModifiers();
				UpdateMovement();

				// CASE: Player is walking normaly
				// Executed when: Player is not ground strafing, and does not have the boost key held
        if(!is_boosting && !is_hovering && !is_strafing && is_grounded){

						move_speed = base_speed; // Reset players pace

						// After x time on the ground, player starts to refuel
						refuel_wait += Time.deltaTime;
	          if(refuel_wait > 0.5f && fuel <= 150f){
	            fuel += 30f * Time.deltaTime;
	          }

						// Play walk cycle sound on a clock
						if(walk_timer >= 0.6f && is_moving){
	            walk_timer = 0f;
	            legs_sfx_src.PlayOneShot(walking_sfx[rand.Next(0,4)],1f);
	          }
	          else if(is_moving){
	            walk_timer += Time.deltaTime;
	          }
				}

				// CASE: Player is boosting foreward
				// Executed when: Player has boost key selected and is moving forward only
				if(!is_strafing && is_boosting && fuel > 0f){
					move_speed = base_speed * boost_multiplier;
					fuel -= 0.5f;
					if(!engines_loop_src.isPlaying){
						engines_sfx_src.PlayOneShot(engineOn_sfx,1f);
						engines_loop_src.PlayOneShot(engineLoop_sfx,1f);
					}
				}

				// CASE: Player jumps once
				// Executed when: Player has jump key pressed and has not left the ground
				if(jump_modifier_1 && is_grounded && fuel >= 15f){
					Jump();
				}

				// Will automatically reset hover state when fuel runs out
				if(fuel <= 0f || is_grounded){
					jump_modifier_1 = false;
					jump_modifier_2 = false;
				}

				// CASE: Player strafes, either on the ground or in the air
				if(is_strafing && fuel >= 35f){
					Strafe();
				}

				// Stops engine sfx loop if not boosting or hovering
				if(!is_hovering && !is_boosting && engines_loop_src.isPlaying){
					engines_loop_src.Stop();
					engines_sfx_src.PlayOneShot(engineOff_sfx,1f);
				}

    }

		void FixedUpdate(){
			// CASE: Player enters a hover
			// Executed when: Player has jump key selected and has left the ground for some amount of time
			// ~~~ SPECIAL N0TE ~~~
			// Players can move and strafe when hoovering, allowing air strafes
			if(!is_grounded && is_hovering && fuel > 0f){
				Hover();
				if(!engines_loop_src.isPlaying){
					engines_loop_src.PlayOneShot(engineLoop_sfx,1f);
				}
			}

			// I wanted the player to fall faster, but not the projectiles, so here we are
			PlayerBody.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
		}

		void OnCollisionEnter(Collision collision){
        if(collision.relativeVelocity.magnitude > 2){
            legs_sfx_src.PlayOneShot(landing_sfx[rand.Next(0,2)], 1f);
        }
    }
}
