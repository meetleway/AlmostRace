﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
    Creator and developer of script: Leonardo Caballero
    Purpose: To allow a countdown to start the race or to show the game is almost over.
    Also helps keeps the player in place during the starting countdown. 
*/

public class Countdown : MonoBehaviour
{
    public int timeLeft = 3;
    private TextMeshProUGUI _countText;
    private VehicleInput[] _arrV;
    private HotSpotBotBehavior _botBehaviorScript;
    private bool _startStatus = true;

    void Start()    
    {
        _countText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(countDown(timeLeft));
    }

    public IEnumerator countDown(int seconds)
    {
        int count = seconds;

        while (count > 0)
        {
            _countText.text = count.ToString();
            yield return new WaitForSeconds(1);
            count--;
            AudioManager.instance.Play("Countdown");
        }

        _arrV = FindObjectsOfType<VehicleInput>();
        _botBehaviorScript = FindObjectOfType<HotSpotBotBehavior>();
        turnOff(true);
        _startStatus = false;

        gameObject.SetActive(false);
        AudioManager.instance.Play("Countdown End");
    }

    private void turnOff(bool stat)
    {
        foreach (VehicleInput t in _arrV)
        {
            t.setStatus(stat);
        }
        _botBehaviorScript.SetCanGoForward(stat);
        AIBehaviour[] bots = FindObjectsOfType<AIBehaviour>();
        foreach(AIBehaviour go in bots)
        {
            go.canDrive = true;
        }
    }
}
