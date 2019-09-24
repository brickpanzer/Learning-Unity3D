using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : MonoBehaviour
{

    public System.Random rand = new System.Random();
    public AudioSource grenadeLauncher_merged_02b;
    public AudioSource am_sar_fire_01a;
    public AudioSource am_sar_fire_01b;
    public AudioSource am_sar_fire_01c;
    public AudioSource[] am_sar = new AudioSource[3];
    public float reload_timer = 5.0f;
    public float interval_timer = 0.0f;
    public GameObject Grenade_prefab;
    public GameObject Bullet_prefab;
    public GameObject grenade;
    public GameObject bullet;
    public bool grenade_is_fired = false;
    public Vector3 grenade_fire_position;

    // Start is called before the first frame update
    void Start()
    {
      reload_timer = 5f;
      interval_timer = 0.5f;
      am_sar[0] = am_sar_fire_01a;
      am_sar[1] = am_sar_fire_01b;
      am_sar[2] = am_sar_fire_01c;
    }

    void LaunchGrenade(){

      grenade_fire_position = transform.position + new Vector3(0f,1f,0f);
      grenade = Instantiate(Grenade_prefab, grenade_fire_position, transform.rotation);
      Rigidbody rb = grenade.GetComponent<Rigidbody>();
      rb.AddForce(transform.forward * 2000f);
    }

    void FireBullet(){
      grenade_fire_position = transform.position + new Vector3(0f,1f,0f);
      bullet = Instantiate(Bullet_prefab, grenade_fire_position, transform.rotation);
      Rigidbody rb = bullet.GetComponent<Rigidbody>();
      rb.AddForce(transform.forward * 2000f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1) && reload_timer >= 5.0f){
            grenadeLauncher_merged_02b.Play();
            LaunchGrenade();
            grenade_is_fired = true;
            reload_timer = 0.0f;
        }
        if(reload_timer <= 5.0f){
          reload_timer += Time.fixedDeltaTime;
        }
        if(reload_timer >= 5f && grenade_is_fired){
          grenade_is_fired = false;
        }
        if(Input.GetMouseButton(0) && interval_timer >= 0.4f){
            interval_timer = 0.0f;
            FireBullet();
            am_sar[rand.Next(0,3)].Play();
        }
        if(interval_timer <= 0.5f){
          interval_timer += Time.fixedDeltaTime;
        }
    }
}
