﻿using UnityEngine;
using System.Collections;
using UnitySampleAssets.CrossPlatformInput;

public class PlayerShooting : MonoBehaviour {

	//variables para la deteccion del joystick
	//public Joystick rightJoystick;
	float posxr;
	float posyr;
	//...........................


	public int damagePerShot = 20;
	public float timeBetweenBullets = 0.15f;
	public float range = 100f;
	
	
	float timer;
	Ray shootRay;
	RaycastHit shootHit;
	int shootableMask;
	ParticleSystem gunParticles;
	LineRenderer gunLine;
	AudioSource gunAudio;
	Light gunLight;
	float effectsDisplayTime = 0.2f;
	
	
	void Awake ()
	{
		shootableMask = LayerMask.GetMask ("Shootable");
		gunParticles = GetComponent<ParticleSystem> ();
		gunLine = GetComponent <LineRenderer> ();
		gunAudio = GetComponent<AudioSource> ();
		gunLight = GetComponent<Light> ();
	}
	

	void Update ()
	{
		posxr = CrossPlatformInputManager.GetAxis ("HorizontalR");
		posyr = CrossPlatformInputManager.GetAxis ("VerticalR");
		//Debug.Log( posxr != 0 || posyr!=0 ? "Moving" : "Stoped");

		timer += Time.deltaTime;
		
		if((posxr != 0 || posyr!=0) && timer >= timeBetweenBullets && Time.timeScale != 0  /*|| Input.GetButtonDown("Fire1")*/)
		{
			Shoot ();
		}
		
		if(timer >= timeBetweenBullets * effectsDisplayTime)
		{
			DisableEffects ();
		}
	}
	
	
	public void DisableEffects ()
	{
		gunLine.enabled = false;
		gunLight.enabled = false;
	}
	
	
	void Shoot ()
	{
		timer = 0f;
		
		gunAudio.Play ();
		
		gunLight.enabled = true;
		
		gunParticles.Stop ();
		gunParticles.Play ();
		
		gunLine.enabled = true;
		gunLine.SetPosition (0, transform.position);
		
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;
		
		if(Physics.Raycast (shootRay, out shootHit, range/*, shootableMask*/))
		{
			EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();
			if(enemyHealth != null)
			{
				enemyHealth.TakeDamage (damagePerShot, shootHit.point);
			}
			gunLine.SetPosition (1, shootHit.point);
		}
		else
		{
			gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
		}
	}
}
