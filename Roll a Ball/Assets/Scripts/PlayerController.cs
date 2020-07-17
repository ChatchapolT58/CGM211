using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
    public float timeRemaining;
    public bool timerIsRunning = false;

	public Text countText;
	public Text winLoseText;
	public Text timeText;
	public Text resetText;

	private Rigidbody rb;
	private int count;

    void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winLoseText.text = "";
		resetText.text = "";

        timerIsRunning = true;
	}

	void Update ()
	{
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }

        if (timeRemaining <= 0)
        {
            winLoseText.text = "You Lose!";
            resetText.text = "Press <b>SPACE</b> to restart";
            Time.timeScale = 0;
        }

		if (Input.GetKeyDown (KeyCode.Space))
		{
			Time.timeScale = 1;
			Restart ();
		}
	}

	void SetCountText ()
	{
		countText.text = "Collect: " + count.ToString ();
		if (count >= 12) 
		{
			winLoseText.text = "You Win!";
			resetText.text = "Press <b>SPACE</b> to restart";
			Time.timeScale = 0;
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

		rb.AddForce (movement * speed);
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.gameObject.CompareTag ("Pick Up"))
		{
			other.gameObject.SetActive (false);
			count = count + 1;
			SetCountText ();

            timeRemaining = timeRemaining + 5;  
		}
	}

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    void Restart ()
	{
		SceneManager.LoadScene ("MiniGame");
	}
}
