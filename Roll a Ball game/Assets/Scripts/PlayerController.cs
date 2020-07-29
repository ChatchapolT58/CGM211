using System.Collections;
using System.Collections.Generic;
using Unity.MPE;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    public float timeIsRemaining = 20;
    public bool timeIsRunning = false;

    public Transform cam;
    public Text countText;
    public Text scoreText;
    public Text timeText;
    public Text restartText;

    private Rigidbody rb;
    private int count;
    private bool m_cursorIsLocked = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        scoreText.text = "";
        timeIsRunning = true;
        restartText.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (timeIsRunning)
        {
            if (timeIsRemaining > 0)
            {
                timeIsRemaining -= Time.deltaTime;
                DisplayTime(timeIsRemaining);
            }
            else
            {
                timeIsRemaining = 0;
                timeIsRunning = false;
            }
        }

        if (timeIsRemaining <= 0f)
        {
            scoreText.text = "YOU LOSE!";
            restartText.text = "Press <b>SPACE</b> to restart";
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1f;
            RestartScane();
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            m_cursorIsLocked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            m_cursorIsLocked = true;
        }

        if (m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!m_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        if (movement.magnitude >= 0.1f) 
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

            Vector3 moveCam = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            rb.AddForce(moveCam * speed);
        }
    }

    void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count = count + 1;
            countText.text = "Coins: " + count.ToString();
            SetCountText();
            timeIsRemaining = timeIsRemaining + 5;
        }

    }

    void SetCountText () 
    {
        countText.text = "Coins: " + count.ToString();
        if (count >= 2)
        {
            scoreText.text = "YOU WIN!";
            restartText.text = "Press <b>SPACE</b> to restart";
            timeIsRunning = false;
            Time.timeScale = 0.1f;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        float milliseconds = (timeToDisplay % 1) * 100;

        if (timeIsRemaining < 9f)
        {
            timeText.text = string.Format("{0:00}:{1:00}", seconds, milliseconds);
        }
        else
        {
            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }

        if (timeIsRemaining < 0f)
        {
            timeText.text = "0" + seconds + ":00";
        }
    }

    void RestartScane()
    {
        SceneManager.LoadScene("MiniGame");
    }
}
