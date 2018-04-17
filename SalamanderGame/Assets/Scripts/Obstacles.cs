using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacles : MonoBehaviour
{

    public int objHealth=2;
    //sound effect
    public AudioSource boxSound;
    // Use this for initialization
    void Start()
    {



    }

    // Update is called once per frame
    void Update()
    {

        if (objHealth <= 0)
        {
            Destroy(gameObject);
            boxSound.Play();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        //if enemy collides with weapon 
        if (other.tag == "Weapon")
        {
            //loses -1 health
            objHealth--;
        }

    }


}