﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 Eddie B. This code is basically a copy past of Volt_LaserBolt (which will probably get deleted soon).
 I made this script to allow us to edit behavior while keeping a temporary backup.
 This script will no doubt become the final version eventually.
     */

public class Lux_ProjectileBehavior : Projectile
{
    private void OnTriggerEnter(Collider other)
    {   
        if (_isAlive)
        {
            if (other.gameObject != _immunePlayer && other.gameObject.GetComponent<CarHeatManager>() != null)
            {//Checks if the object isn't the immunePlayer and if they are a car.
                other.gameObject.GetComponent<CarHeatManager>().AddHeat(_projectileDamage);
                _immunePlayerScript.AddHype(_projectileHype, "Damage:");
                StartCoroutine(ExplosionEffect());
            }
            else if (other.gameObject != _immunePlayer && other.gameObject.GetComponent<Interactable>() != null)
            {//Checks if the object isn't the immunePlayer and if they are an interactable object.
                if(other.gameObject.GetComponent<Lux_ShieldPanelBehavior>() != null)
                {//checks if about to hit a shield
                    if(other.gameObject.GetComponent<Lux_ShieldPanelBehavior>().GetShieldPlayer() == _immunePlayer)
                    {
                       return;
                    }
                }
                other.gameObject.GetComponent<Interactable>().interactingPlayer = _immunePlayer;
                other.gameObject.GetComponent<Interactable>().DamageInteractable(_projectileDamage);
                StartCoroutine(ExplosionEffect());
            }
            else if (other.gameObject != _immunePlayer)
            {
                StartCoroutine(ExplosionEffect());
            }
        }
    }
}
