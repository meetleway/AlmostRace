﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//eddie

public class Lux_EnergyBlade : Projectile
{
    public int lifeTime = 7;
    private List<GameObject> _immuneCars;
    private Interactable hitInteractable;
    private float _growthRate;
    private float _growthAmount;
    private float _growthLimit;
    private int _debugInt;
    private CarHealthBehavior carHit;

    void Start()
    {
        _immuneCars = new List<GameObject>();
        GiveSpeed();
        InvokeRepeating("Grow", 0, _growthRate);
        Destroy(gameObject, lifeTime);
    }

    public void GiveInfo(float growthRate, float growthAmount, float growthLimit)
    {
        _growthRate = growthRate;
        _growthAmount = growthAmount;
        _growthLimit = transform.localScale.x + growthLimit;
    }

    public void Grow()
    {
        _debugInt++;
       // Debug.Log("Grow has been called: " + _debugInt + " times!");
        if(transform.localScale.x < _growthLimit)
        {
            transform.localScale += new Vector3(_growthAmount, 0, _growthAmount);
        }
        else
        {
            CancelInvoke("Grow");
        }
     
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Vehicle") && _immunePlayer != null && other.gameObject != _immunePlayer && !_immuneCars.Contains(other.gameObject))
        {
            carHit = other.gameObject.GetComponent<CarHealthBehavior>();
            carHit.DamageCar(_projectileDamage, _immunePlayerScript.playerID);
            _immuneCars.Add(other.gameObject);
            Destroy(gameObject);
        }
        if(other.CompareTag("Interactable"))
        {
            hitInteractable = other.GetComponent<Interactable>();

            hitInteractable.DamageInteractable(hitInteractable.interactableHealth);
        }
    }


}
