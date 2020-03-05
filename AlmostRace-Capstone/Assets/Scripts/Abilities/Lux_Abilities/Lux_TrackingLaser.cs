﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lux_TrackingLaser : MonoBehaviour
{

    private GameObject _target;

    float _laserDamageRate,_laserDamage;
    float _laserSpeed;
    private GameObject _immunePlayer;
    private List<CarHealthBehavior> _damagedCars;
    private Interactable hitInteractable;

    // Start is called before the first frame update
    void Start()
    {
        _damagedCars = new List<CarHealthBehavior>();
    }

    public void GiveInfo( float laserDamageRate, float laserDamage, GameObject immunePlayer)
    {
        _laserDamageRate = laserDamageRate;
        _laserDamage = laserDamage;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.GetComponent<CarHealthBehavior>() != null && other.gameObject != _immunePlayer)
        {
            _damagedCars.Add(other.gameObject.GetComponent<CarHealthBehavior>());

            if (_damagedCars.Count == 1)
            {
                InvokeRepeating("DamageCars", 0, _laserDamageRate);
            }
        }
        else if (other.gameObject.GetComponent<Interactable>() != null)
        {
            hitInteractable = other.gameObject.GetComponent<Interactable>();
            hitInteractable.DamageInteractable(hitInteractable.interactableHealth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CarHealthBehavior>() != null && _damagedCars.Contains(other.gameObject.GetComponent<CarHealthBehavior>()))
        {
            _damagedCars.Remove(other.gameObject.GetComponent<CarHealthBehavior>());
        }
        
    }

    private void DamageCars()
    {
        if (_damagedCars.Count != 0)
        {
            foreach (CarHealthBehavior car in _damagedCars)
            {
                if (!car.isDead)
                {
                    car.DamageCar(_laserDamage);

                    if (_target != null)
                    {
                        if (!_target.Equals(car.gameObject))
                        {

                        }
                        if (car.healthCurrent <= 0)
                        {
                            _damagedCars.Remove(car);
                            Destroy(gameObject);
                        }
                    }
                }
            }
        }
        else
        {
            CancelInvoke("DamageCars");
        }
    }
}
