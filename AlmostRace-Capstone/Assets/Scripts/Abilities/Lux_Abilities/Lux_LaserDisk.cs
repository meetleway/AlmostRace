﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Eddie Borissov
 Code that handles the functionality of the laser disks fired by the 
 Lux's offensive ability */

public class Lux_LaserDisk : Projectile
{
    private float _laserHype;

    public Transform laserEmitterLeft;
    public Transform laserEmitterRight;

    private float _laserDamage;
    private float _laserPulseRate;

    private RaycastHit rayHit;

    private void Start()
    {
        GiveSpeed();
        InvokeRepeating("PulseLasers", 0, _laserPulseRate);
    }

    /// <summary>
    /// This function is called frequently, many times a second, in order to spawn the laser code and damage anything it hits.
    /// This is done in lue of DoT lists, such as the ones used with DoT colliders. Raycasts don't have an OnTriggerExit, after all.
    /// </summary>
    public void PulseLasers()
    {
        //Left laser code
        if (Physics.Raycast(laserEmitterLeft.position, laserEmitterLeft.TransformDirection(Vector3.forward), out rayHit, Mathf.Infinity))
        {
            //if it hit a car
            if (rayHit.collider.gameObject.GetComponent<CarHealthBehavior>() != null)
            {//hit a car
                if(rayHit.collider.gameObject != _immunePlayer)
                {
                    Debug.Log("Car found by laserEmitterLeft");
                    rayHit.collider.gameObject.GetComponent<CarHealthBehavior>().DamageCar(_laserDamage);
                }
           
            }

            //if it hit an interactable
            if (rayHit.collider.gameObject.GetComponent<Interactable>() != null)
            {//hit an interactable
                Debug.Log("Interactable found by laserEmitterLeft");
                rayHit.collider.gameObject.GetComponent<Interactable>().DamageInteractable(_laserDamage);
            }

            Debug.DrawRay(laserEmitterLeft.position, laserEmitterLeft.TransformDirection(Vector3.forward) * rayHit.distance, Color.red);
        }

        //Right laser code
        if (Physics.Raycast(laserEmitterRight.position, laserEmitterRight.TransformDirection(Vector3.forward), out rayHit, Mathf.Infinity))
        {
            //if it hit a car
            if (rayHit.collider.gameObject.GetComponent<CarHealthBehavior>() != null)
            {//hit a car
                if (rayHit.collider.gameObject != _immunePlayer)
                {
                    Debug.Log("Car found by laserEmitterRight");
                    rayHit.collider.gameObject.GetComponent<CarHealthBehavior>().DamageCar(_laserDamage);
                }
            }

            //if it hit an interactable
            if (rayHit.collider.gameObject.GetComponent<Interactable>() != null)
            {//hit an interactable
                Debug.Log("Interactable found by laserEmitterRight");
                rayHit.collider.gameObject.GetComponent<Interactable>().DamageInteractable(_laserDamage);
            }

            Debug.DrawRay(laserEmitterRight.position, laserEmitterRight.TransformDirection(Vector3.forward) * rayHit.distance, Color.red);

        }

    }

    /// <summary>
    /// might seem a bit extra just for one variable, but it makes more sense than making it 
    /// public and potentially having a designer accidentally fill it out here, or worse yet adding it to Projectile,
    /// which would make 0 sense, since most projectiles only have 1 hype variable (currently).
    /// </summary>
    /// <param name="diskHype"> The amount of hype you want to gain from scoring a direct hit </param>
    public void SetDiskInfo(float laserDamage, float laserDamageRate, float laserHype)
    {
        _laserDamage = laserDamage;
        _laserHype = laserHype;
        _laserPulseRate = laserDamageRate;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //oops
        if(collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }

        if(collision.gameObject.GetComponent<CarHealthBehavior>() != null)
        {
            if(collision.gameObject != _immunePlayer)
            {
                collision.gameObject.GetComponent<CarHealthBehavior>().DamageCar(_projectileDamage);
                Destroy(gameObject);
            }

        }

        if(collision.gameObject.GetComponent<Interactable>() != null)
        {
            collision.gameObject.GetComponent<Interactable>().DamageInteractable(_projectileDamage);
            Destroy(gameObject);
        }


    }


}
