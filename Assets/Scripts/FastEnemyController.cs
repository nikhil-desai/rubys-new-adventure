﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastEnemyController : MonoBehaviour {
    // Movement
    public float speed;
    public bool vertical;
    Rigidbody2D rigidbody2D;
    // Timer
    public float changeTime = 3.0f;
    float timer;
    int direction = 1;
    // Animation
    Animator animator;
    // State Change
    bool broken = true;
    // Particles
    public ParticleSystem smokeEffect;
    // Sound
    public AudioClip collectedClip;
    AudioSource audioSource;
    // Count Fixed Robots
    public RubyController ruby;

    // Start is called before the first frame update
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D> ();
        timer = changeTime;
        // Animation
        animator = GetComponent<Animator> ();
    }

    public void PlaySound (AudioClip clip) {
        audioSource.PlayOneShot (clip);
    }

    void Update () {
        if (!broken) {
            return;
        }
        timer -= Time.deltaTime;

        animator.SetFloat ("Move X", 0);
        animator.SetFloat ("Move Y", direction);

        if (timer < 0) {
            int randomMovement = Random.Range (0, 4);
            switch (randomMovement) {
                case 0:
                    vertical = true;
                    direction = direction;
                    break;
                case 1:
                    vertical = true;
                    direction = -direction;
                    break;
                case 2:
                    vertical = false;
                    direction = direction;
                    break;
                case 3:
                    vertical = false;
                    direction = -direction;
                    break;
            }
            timer = changeTime;
        }
    }

    void FixedUpdate () {
        if (!broken) {
            return;
        }
        Vector2 position = rigidbody2D.position;
        if (vertical) {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat ("Move X", 0);
            animator.SetFloat ("Move Y", direction);
        } else {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat ("Move X", direction);
            animator.SetFloat ("Move Y", 0);
        }

        rigidbody2D.MovePosition (position);
    }

    void OnCollisionEnter2D (Collision2D other) {
        RubyController player = other.gameObject.GetComponent<RubyController> ();

        if (player != null) {
            player.ChangeHealth (-2);
            player.PlaySound (collectedClip);
        }
    }

    public void Fix () {
        broken = false;
        rigidbody2D.simulated = false;
        animator.SetTrigger ("Fixed");
        ruby.CountRobotFixed ();
        smokeEffect.Stop ();
        // Destroy(smokeEffect.gameObject);
    }
}