using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RubyController : MonoBehaviour {
    // Movement
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;
    // Health
    public int maxHealth = 5;
    public int health { get { return currentHealth; } }
    int currentHealth;
    // Speed
    public float speed = 3.0f;
    // Invincibility
    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;
    // Animation
    Animator animator;
    Vector2 lookDirection = new Vector2 (1, 0);
    // Particles
    public ParticleSystem collisionEffect;
    public ParticleSystem rewardEffect;
    // Projectile
    public GameObject projectilePrefab;
    // Audio
    AudioSource audioSource;
    public AudioClip projectileSound;
    public GameObject backgroundNoise;
    public GameObject winNoise;
    // Fixed Robots
    public Text robotCountText;
    public int robotCount;
    // Cog Ammo
    public Text ammoCountText;
    private int ammoCount;

    // Start is called before the first frame update
    void Start () {
        rigidbody2d = GetComponent<Rigidbody2D> ();
        animator = GetComponent<Animator> ();
        currentHealth = maxHealth;

        robotCount = 0;
        robotCountText.text = "Robots Fixed: " + robotCount.ToString () + "/5";

        ammoCount = 5;
        ammoCountText.text = ammoCount.ToString ();

        audioSource = GetComponent<AudioSource> ();
    }

    public void PlaySound (AudioClip clip) {
        audioSource.PlayOneShot (clip);
    }

    // Update is called once per frame
    void Update () {
        horizontal = Input.GetAxis ("Horizontal");
        vertical = Input.GetAxis ("Vertical");

        Vector2 move = new Vector2 (horizontal, vertical);
        if (!Mathf.Approximately (move.x, 0.0f) || !Mathf.Approximately (move.y, 0.0f)) {
            lookDirection.Set (move.x, move.y);
            lookDirection.Normalize ();
        }
        animator.SetFloat ("Look X", lookDirection.x);
        animator.SetFloat ("Look Y", lookDirection.y);
        animator.SetFloat ("Speed", move.magnitude);

        if (isInvincible) {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }
        if (Input.GetKeyDown (KeyCode.C)) {
            Launch ();
        }

        // Raycasting
        if (Input.GetKeyDown (KeyCode.X)) {
            RaycastHit2D hit = Physics2D.Raycast (rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask ("NPC"));
            RaycastHit2D box = Physics2D.Raycast (rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask ("BOX"));
            if (hit.collider != null) {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter> ();
                if (character != null) {
                    character.DisplayDialog ();
                }
            }
            if (box.collider != null) {
                ResourceCube boxObject = box.collider.GetComponent<ResourceCube> ();
                if (boxObject != null) {
                    boxObject.spawnPrefabs();
                }
            }
        }
        // Winning
        if (robotCount == 5 && SceneManager.GetActiveScene ().name == "Second") {
            backgroundNoise.SetActive (false);
            winNoise.SetActive (true);
            speed = 0;
        }
        // Death/Game Over
        if (currentHealth <= 0) {
            SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
        }
    }

    void FixedUpdate () {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition (position);
    }
    public void ChangeHealth (int amount) {
        if (amount < 0) {
            if (isInvincible) {
                return;
            }
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger ("Hit");
            Instantiate (collisionEffect, rigidbody2d.transform.position, Quaternion.identity);
            collisionEffect.Play ();
        } else {
            Instantiate (rewardEffect, rigidbody2d.transform.position, Quaternion.identity);
            rewardEffect.Play ();
        }

        currentHealth = Mathf.Clamp (currentHealth + amount, 0, maxHealth);
        UIHealthBar.instance.SetValue (currentHealth / (float) maxHealth);
    }
    public void CountRobotFixed () {
        robotCount++;
        robotCountText.text = "Robots Fixed: " + robotCount.ToString () + "/5";
    }
    public void AddCogs () {
        ammoCount = ammoCount + 3;
        ammoCountText.text = ammoCount.ToString ();
    }
    void Launch () {
        if (ammoCount > 0) {
            GameObject projectileObject = Instantiate (projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile> ();
            projectile.Launch (lookDirection, 300);

            animator.SetTrigger ("Launch");

            PlaySound(projectileSound);

            ammoCount--;
            ammoCountText.text = ammoCount.ToString ();
        }
    }
}