using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBerry : MonoBehaviour {
    // Sound
    public AudioClip collectedClip;

    void OnTriggerEnter2D (Collider2D other) {
        RubyController controller = other.GetComponent<RubyController> ();

        if (controller != null) {
            if (controller.health < controller.maxHealth) {
                controller.ChangeSpeed (1);
                controller.PlaySound (collectedClip);
                Destroy (gameObject);
                
            }
        }
    }
}