using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun_grenadelauncher_2 : MonoBehaviour
{
	System.Random rand = new System.Random();

	public AudioSource audioSource;
	AudioClip[] Grenade_SFX = new AudioClip[3];
	public AudioClip Grenade_Launch_1;
	public AudioClip Grenade_Launch_2;
	public AudioClip Grenade_Launch_3;

	public float reload_timer = 5f; //How much time between shots

	public GameObject Grenade_Prefab; //What prefab to launch
	GameObject Grenade_Obj;

	// Start is called before the first frame update
	void Start()
	{
			Grenade_SFX[0] = Grenade_Launch_1;
			Grenade_SFX[1] = Grenade_Launch_2;
			Grenade_SFX[2] = Grenade_Launch_3;
	}

	void LaunchGrenade(){
			Grenade_Obj = Instantiate(Grenade_Prefab, transform.position, transform.rotation);
			Rigidbody rb = Grenade_Obj.GetComponent<Rigidbody>();
			rb.AddForce(transform.forward * 2000f);
	}

	// Update is called once per frame
	void Update()
	{
			if(Input.GetMouseButtonDown(1) && reload_timer >= 5f){
					audioSource.PlayOneShot(Grenade_SFX[rand.Next(0,3)],1f);
					LaunchGrenade();
					reload_timer = 0f;
			}
			if(reload_timer <= 5f){
				reload_timer += Time.deltaTime;
			}

	}
}
