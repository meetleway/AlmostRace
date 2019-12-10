﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    /*
    Author: Jake Velicer
    Purpose: Contains the behavior for vehciles picking up the hot spot bot,
    and the behavior for dropping it.
    */

public class HotSpotVehicleAdministration : MonoBehaviour
{
    public bool holdingTheBot;
    private GameObject HotSpotBotHeld;
    public float initialHypeGain, gradualHypeGain, hypeWaitTime;
    private float hypeTimer;

    // Update is called once per frame
    void Update()
    {
        if (holdingTheBot)
        {
            hypeTimer += Time.deltaTime;
            if(hypeTimer >= hypeWaitTime)
            {
                HypeGain(gradualHypeGain);
                hypeTimer = 0;
            }
        }
    }

    private void HoldTheBot(GameObject theBot)
    {
        holdingTheBot = true;
        HotSpotBotHeld = theBot.transform.parent.gameObject;
        HotSpotBotHeld.GetComponent<HotSpotBotBehavior>().SetBeingHeld(true);
        HotSpotBotHeld.SetActive(false);
        AudioManager.instance.Play("HotSpot Attachment");
    }

    public void DropTheBot()
    {
        holdingTheBot = false;
        HotSpotBotHeld.SetActive(true);
        HotSpotBotHeld.GetComponent<HotSpotBotBehavior>().SetBeingHeld(false);
        StartCoroutine(HotSpotBotHeld.GetComponent<HotSpotBotBehavior>().SetPosition(transform.position));
        HotSpotBotHeld = null;
        AudioManager.instance.Play("HotSpot Lost");
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("HotSpot"))
        {
            if(!other.gameObject.transform.parent.GetComponent<HotSpotBotBehavior>().GetBeingHeld()
            && !holdingTheBot && HotSpotBotHeld == null)
            {
                HoldTheBot(other.gameObject);
                GetComponent<CarHeatManager>().healthCurrent = GetComponent<CarHeatManager>().healthMax;
                HypeGain(initialHypeGain);
                hypeTimer = 0;
            }
        }
    }

    void HypeGain(float added)
    {
        GetComponent<VehicleHypeBehavior>().AddHype(added, "Bot Grabbed!");
    }
}
