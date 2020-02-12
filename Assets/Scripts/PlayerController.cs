using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Text timerText;
    public Text highscoreText;
    public AudioSource audioSource;
    public bool isDead;

    float windForceMaximumMagnitude = 1;
    float secondsBetweenWindChange = 1;
    int highscore;
    Stopwatch stopwatch = new Stopwatch();
    float secondsSinceLastWindChange;
    float windForce;
    Rigidbody rb;
    Scene scene;
    double timer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        scene = SceneManager.GetActiveScene();
        stopwatch.Start();
        highscore = PlayerPrefs.GetInt("highscore", 0);
        highscoreText.text = highscore.ToString("N0");
    }

    private void Update()
    {
        timer = stopwatch.Elapsed.TotalMilliseconds;
        timerText.text = timer.ToString("N0");
        if (timer > highscore)
        {
            highscoreText.text = timer.ToString("N0");
        }
    }

    private void FixedUpdate()
    {
        // Accumulate the amount of seconds since last FixedUpdate
        secondsSinceLastWindChange += Time.fixedDeltaTime;

        //If it's been long enough, generate a new random wind force
        if (secondsSinceLastWindChange > secondsBetweenWindChange)
        {
            windForce = UnityEngine.Random.Range(-windForceMaximumMagnitude, windForceMaximumMagnitude);
            //windForce = 0;

            // Reset the accumulator
            secondsSinceLastWindChange = 0;
        }

        float horizontalAxisForce = 0;
        if (!isDead)
        {
            // This will always be between -1 and 1
            horizontalAxisForce = Input.GetAxis("Horizontal");
        }

        // The more tilted the player is the stronger their rebalance should be
        // Use this help factor to achieve it
        float boost = 0;

        // In Unity euler angles are
        //     0/360
        //       ↑
        //  90 ←---→ 270
        //       ↓ 
        //      180
        if (0 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 90)
        {
            // Divide the angle by something because a boost of more than 45 is too strong
            //boost = (transform.rotation.eulerAngles.z) / 5;
            boost = (transform.rotation.eulerAngles.z) / 5;
        }
        else if (270 <= transform.rotation.eulerAngles.z && transform.rotation.eulerAngles.z <= 360)
        {
            // Divide the angle by something because a boost of more than 45 is too strong
            //boost = (360 - transform.rotation.eulerAngles.z) / 5;
            boost = (360 - transform.rotation.eulerAngles.z) / 5;
        }

        // Calculate the final vorce vector
        var finalForceVector3 = new Vector3((horizontalAxisForce * boost) + windForce, 0.0f, 0.0f);

        // Actually add the force
        rb.AddRelativeForce(finalForceVector3);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "MapElement")
        {
            StartCoroutine(End());
        }
    }

    IEnumerator End()
    {
        stopwatch.Stop();
        isDead = true;
        PlayerPrefs.SetInt("highscore", (int)timer);
        PlayerPrefs.Save();
        audioSource.Play();
        yield return new WaitForSecondsRealtime(3);
        SceneManager.LoadScene(scene.name);
    }
}
