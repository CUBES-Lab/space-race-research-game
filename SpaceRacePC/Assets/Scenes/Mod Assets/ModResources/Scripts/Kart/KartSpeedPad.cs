using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KartGame.KartSystems;

public class KartSpeedPad : MonoBehaviour
{

    public MultiplicativeKartModifier boostStats;

    [Range (0, 5)]
    public float duration = 1f;

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            var rb = other.attachedRigidbody;
            if (rb == null) return;
            var kart = rb.GetComponent<KartMovement>();
            kart.StartCoroutine(KartModifier(kart, duration));

            GameObject.Find("GameLogger").GetComponent<GameLogger>().LogEvent("Boost", "Player touched a boost");
        }
    }

    IEnumerator KartModifier(KartGame.KartSystems.KartMovement kart, float lifetime){
        kart.AddKartModifier(boostStats);
        yield return new WaitForSeconds(lifetime);
        kart.RemoveKartModifier(boostStats);
    }

}
