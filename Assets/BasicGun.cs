using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGun : MonoBehaviour
{

    public AudioSource grenadeLauncher_merged_02b;
    public bool is_reloading = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !is_reloading){
            grenadeLauncher_merged_02b.Play();
        }
    }
}
