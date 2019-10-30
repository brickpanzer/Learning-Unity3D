using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
		public GameObject weapon_prefab;
		GameObject weapon;

    // Start is called before the first frame update
    void Start()
    {
			weapon = Instantiate(weapon_prefab, transform.position, transform.rotation, this.gameObject.transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
