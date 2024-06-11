using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinZone : MonoBehaviour
{
    public static event Action OnPlayerArrived;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement playerMovement = other.GetComponentInParent<PlayerMovement>();
        if (playerMovement != null)
        {
            OnPlayerArrived?.Invoke();
        }
    }
}
