using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NonPlayerCharacter : MonoBehaviour {
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    public GameObject winBox;
    float timerDisplay;
    // Count Fixed Robots
    public RubyController ruby;
    // Start is called before the first frame update
    void Start () {
        dialogBox.SetActive (false);
        winBox.SetActive (false);
        timerDisplay = -1.0f;
    }

    // Update is called once per frame
    void Update () {
        if (timerDisplay >= 0) {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0) {
                dialogBox.SetActive (false);
            }
        }
    }

    public void DisplayDialog () {
        timerDisplay = displayTime;
        if (ruby.robotCount == 5) {
            winBox.SetActive (true);
            StartCoroutine (loadNew ());
        } else {
            dialogBox.SetActive (true);
        }
    }

    // Teleport Scene
    IEnumerator loadNew () {
        // Wait 4 Sec
        yield return new WaitForSeconds (4); 
        // Load Scene
        SceneManager.LoadScene (1);
    }
}