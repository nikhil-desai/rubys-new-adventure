using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCube : MonoBehaviour {
    // Resource Box
    public GameObject healthItem;
    public GameObject cogItem;

    public RubyController ruby;

    public void spawnPrefabs () {
        var randomNumber = Random.Range(0, 4);
        if (ruby.health == ruby.maxHealth) {
            for (var i = 0; i < randomNumber; i++) {
                GameObject cogObject = Instantiate (cogItem, transform.position + Vector3.down * 1.5f * i, Quaternion.identity);
                GameObject cogObject2 = Instantiate (cogItem, transform.position + Vector3.up * 1.5f * i, Quaternion.identity);
            } 
        } else {
            for (var i = 0; i < randomNumber; i++) {
                GameObject healthObject = Instantiate (healthItem, transform.position + Vector3.down * 1.5f * i, Quaternion.identity);
                GameObject cogObject = Instantiate (cogItem, transform.position + Vector3.up * 1.5f * i, Quaternion.identity);
            }
        }
    }
}