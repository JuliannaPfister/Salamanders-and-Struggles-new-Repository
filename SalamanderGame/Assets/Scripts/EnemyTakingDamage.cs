using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTakingDamage : MonoBehaviour
{
	public int currentHealth;
	public int maxHealth;
	public int damage;

	private Rigidbody2D myrigid;

	// Use this for initialization
	void Start () 
	{

		//at spawn, the enemy's current health is whatever it's maximum health is.
		currentHealth = maxHealth;

		myrigid = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		//if the enemy's health is equal to OR less than zero, it will get destroyed- ie die
		if (currentHealth <= 0) 
		{
			Destroy (gameObject);

		}

	}

	//for taking damage, the value of which is in the Attacking script
	public void Damage (int damage)
	{
		currentHealth -= damage;
	}
}
