using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text countText;
	public Text winText;
	public Text timeText;
	public Text resetText;

	public int secondsLeft;
	public bool takingAway = false;

	private Rigidbody rb;
	private int count;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		count = 0;
		SetCountText ();
		winText.text = "";
		resetText.text = "";
		timeText.GetComponent<Text> ().text = "0:" + secondsLeft;
	}

	void Update ()
	{
		if (takingAway == false && secondsLeft > 0)
		{
			StartCoroutine (TimerTake());
		}

		if (secondsLeft <= 0)
		{
			winText.text = "You Lose!";
			resetText.text = "Press <b>SPACE</b> to restart";
			Time.timeScale = 0;
		}

		if (Input.GetKeyDown (KeyCode.Space))
		{
			Time.timeScale = 1;
			Restart ();
		}
	}

	IEnumerator TimerTake ()
	{
		takingAway = true;
		yield return new WaitForSeconds (1);
		secondsLeft -= 1;
		if (secondsLeft < 10)
		{
			timeText.GetComponent<Text> ().text = "0:0" + secondsLeft;
		}
		else
		{
			timeText.GetComponent<Text> ().text = "0:" + secondsLeft;
		}

		takingAway = false;
	}

	void SetCountText ()
	{
		countText.text = "Collect: " + count.ToString ();
		if (count >= 12) 
		{
			winText.text = "You Win!";
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
			secondsLeft = secondsLeft + 6;
			count = count + 1;
			SetCountText ();
		}
	}

	void Restart ()
	{
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}
