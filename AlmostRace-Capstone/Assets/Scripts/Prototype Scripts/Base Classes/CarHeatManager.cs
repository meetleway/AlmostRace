﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CarHeatManager : MonoBehaviour
{
    public Image heatImage;
    public float heatCurrent = 0f;
    public float heatStallLimit = 100f;
    public float heatExplodeLimit = 120f;
    public float cooldownRate = 1f;
    public float respawnSecs = 3f;
    public float cooldownTime = 0f;
    public float cooldownFrequency = 2f;


    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))//for testing, heats up car
        {
            heatCurrent = 110f;
        }
        else if (Input.GetKeyDown(KeyCode.N)) // for testing, cools down car
        {
            heatCurrent = 0f;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            heatCurrent = 120f;
        }

        if (heatCurrent > heatExplodeLimit)
        {
            heatCurrent = heatExplodeLimit;
        }

        if (heatCurrent >= heatExplodeLimit)
        {
            respawn();
        }

        if (heatCurrent > 0)
        {
            InvokeRepeating("healthCooldown", cooldownTime, cooldownFrequency);
        }
        else if (heatCurrent < 0)
        {
            heatCurrent = 0;
        }


        if (heatCurrent == 0 && gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }

        heatImage.fillAmount = ((heatCurrent * 100) / 120) / 100;

    }


    private void respawn()
    {

        if (gameObject.activeSelf == true)
        {
            Instantiate(Resources.Load("explosion"), gameObject.transform.position, gameObject.transform.rotation);
            gameObject.SetActive(false);

            Invoke("respawn", respawnSecs);
        }
        else if (gameObject.activeSelf != true)
        {
            heatCurrent = 0;
            gameObject.SetActive(true);
            BasicAbility bAbility = GetComponent<BasicAbility>();
            if (bAbility != null) {
                bAbility.DeactivateAbility();
            }
        }

    }

    private void healthCooldown()
    {
        heatCurrent -= cooldownRate;
    }

}

