using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    float delay = 6f;
    public AudioClip explosion_sfx;
    public AudioClip collision_sfx;
    public GameObject explosion_vfx;
    float countdown;
    bool exploded = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.fixedDeltaTime;
        if(countdown <= 0f && !exploded){
            exploded = true;
            AudioSource.PlayClipAtPoint(explosion_sfx, transform.position, 32f);
            Instantiate(explosion_vfx, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if(Input.GetMouseButtonDown(1) && !exploded){
          exploded = true;
          AudioSource.PlayClipAtPoint(explosion_sfx, transform.position, 32f);
          Instantiate(explosion_vfx, transform.position, transform.rotation);
          Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision){
        AudioSource.PlayClipAtPoint(collision_sfx, transform.position, 64f);
    }
}
