using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun_AM_SAR : MonoBehaviour
{
    System.Random rand = new System.Random();

    public AudioSource audioSource;
    AudioClip[] Fire_SFX = new AudioClip[3];
    public AudioClip Fire_1;
    public AudioClip Fire_2;
    public AudioClip Fire_3;

    public float fire_rate = 0.3f;
    public float overheat = 0f;
    bool is_overheated = false;
    float interval_timer = 0f;

    public GameObject Bullet_prefab;
    GameObject Bullet_Obj;
    // Start is called before the first frame update
    void Start()
    {
        Fire_SFX[0] = Fire_1;
        Fire_SFX[1] = Fire_2;
        Fire_SFX[2] = Fire_3;
    }

    void FireBullet(){
        Bullet_Obj = Instantiate(Bullet_prefab, transform.position, transform.rotation);
        Rigidbody rb = Bullet_Obj.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 2000f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && interval_timer >= fire_rate){
          interval_timer = 0f;
          FireBullet();
          audioSource.PlayOneShot(Fire_SFX[rand.Next(0,3)],0.5f);
        }
        if(interval_timer <= fire_rate){
          interval_timer += Time.fixedDeltaTime;
        }
    }
}
