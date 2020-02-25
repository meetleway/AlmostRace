﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 *  Mike R 06/11/2019
 *  Functionality for destuctable boulder that the player can shoot at
 *  or collide with. 
 * 
 *  Edited by Eddie B
 *  Made to work with turrets blowing up boulders as well.
 **/

public class DestructableBoulderBehaviour : Interactable
{
    [Tooltip("Amount of damage it does to the vehicle.")]
    public int ramDamage = 20;

    [Tooltip("Amount of hype gained by destroying the boulder.")]
    public float boulderDestroyedHype = 50f;

    [Tooltip("Amount that ramming the boulder slows the player.")]
    public float slowDownFactor = 4;

    [Tooltip("Attach boulder particle effect.")]
    public ParticleSystem boulderParticles;

    public GameObject speedCrystal;
    private SpeedBoostCrystal speedCrystalScript;

    private MeshRenderer rend;
    private Collider coll;

    [Tooltip("A reference to destruction sound.")]
    public AudioClip deathSound;

    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();
        coll = this.GetComponent<Collider>();
        rend.enabled = true;
        coll.enabled = true;

        speedCrystalScript = speedCrystal.GetComponent<SpeedBoostCrystal>();
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.GetComponent<CarHealthBehavior>() != null)
        {

            interactingPlayer = collision.gameObject; // sets the person crashing with the boulder as the interacting player
            TriggerInteractable();
            if (collision.gameObject.GetComponent<VehicleInput>())
            {
                if (collision.gameObject.GetComponent<AimAssistant>().aimCircle.GetComponent<AimCollider>().colliding.Contains(gameObject))
                {
                    collision.gameObject.GetComponent<AimAssistant>().aimCircle.GetComponent<AimCollider>().colliding.Remove(gameObject);
                }
            }
            if (collision.gameObject.GetComponent<AIBehaviour>())
            {
                collision.gameObject.GetComponentInChildren<AIObstacleAvoidance>().turnR = false;
                collision.gameObject.GetComponentInChildren<AIObstacleAvoidance>().turnL = false;
            }

            collision.gameObject.GetComponent<CarHealthBehavior>().DamageCar(ramDamage);
            if (collision.gameObject.GetComponent<SphereCarController>() != null)
            {
                collision.gameObject.GetComponent<SphereCarController>().currentSpeed -=
                    (collision.gameObject.GetComponent<SphereCarController>().currentSpeed / slowDownFactor);
            }

        }
    }

    public override void TriggerInteractable()
    {
        if (interactingPlayer != null)
        {
            if (interactingPlayer.GetComponent<VehicleHypeBehavior>() != null)
            {   //makes sure that non-player agents can destroy the boulders without throwing null references.
                interactingPlayer.GetComponent<VehicleHypeBehavior>().AddHype(boulderDestroyedHype, "Vandal");
            }

            if (interactingPlayer.GetComponent<VehicleInput>())
            {
                if (interactingPlayer.GetComponent<AimAssistant>().target == gameObject)
                {
                    interactingPlayer.GetComponent<AimAssistant>().aimCircle.GetComponent<AimCollider>().colliding.Remove(gameObject);
                    interactingPlayer.GetComponent<AimAssistant>().aimCircle.GetComponent<AimCollider>().interactables.Remove(GetComponent<Collider>());
                }
            }

            if (interactingPlayer.GetComponent<AIBehaviour>())
            {
                interactingPlayer.GetComponentInChildren<AIObstacleAvoidance>().turnR = false;
                interactingPlayer.GetComponentInChildren<AIObstacleAvoidance>().turnL = false;
            }
        }
        AimCollider[] allPlayers = FindObjectsOfType<AimCollider>();

        foreach(AimCollider i in allPlayers)
        {
            i.interactables.Remove(GetComponent<Collider>());
        }

        boulderParticles.Play();
        rend.enabled = false;
        coll.enabled = false;

        AudioManager.instance.Play("RockExplosion", transform);
        speedCrystal.SetActive(true);
        speedCrystalScript.ActivateCrystal();
        Invoke("DestroyInteractable", boulderParticles.main.startLifetime.constant);
    }

    public override void DestroyInteractable()
    {

        if (interactingPlayer != null)
        {
            if (interactingPlayer.GetComponent<AIBehaviour>())
            {
                interactingPlayer.GetComponentInChildren<AIObstacleAvoidance>().turnR = false;
                interactingPlayer.GetComponentInChildren<AIObstacleAvoidance>().turnL = false;
            }
        }

            Destroy(gameObject);
    }

    public override void ResetInteractable()
    {
        //Do not need
    }

    public override void DamageInteractable(float damageNumber)
    {
        interactableHealth -= damageNumber;
        if (interactableHealth <= 0)
        {
            TriggerInteractable();
        }
    }
}
