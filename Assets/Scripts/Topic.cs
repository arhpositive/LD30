using UnityEngine;
using System.Collections;

public class Topic : MonoBehaviour {

	//Public Variables
	public float moveSpeed;

	//Private Variables
	Vector3 moveDirection_;
	Rect playingArea;
	GameLogic gameLogicScript;

	void Awake()
	{
		gameLogicScript = Camera.main.gameObject.GetComponent<GameLogic>();
	}

	// Use this for initialization
	void Start () {
		playingArea = gameLogicScript.GetPlayingArea();
		RandomizeMoveDirection(); 
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(moveSpeed * moveDirection_ * Time.deltaTime, Space.World);
	}

	public void RandomizeMoveDirection()
	{
		float targetX = Random.Range(playingArea.xMin + 1.0f, playingArea.xMax - 1.0f);
		float targetY = Random.Range(playingArea.yMin + 1.0f, playingArea.yMax - 1.0f);
		Vector3 randomTarget = new Vector3(targetX, targetY, 0.0f);

		moveDirection_ = randomTarget - transform.position;
		moveDirection_.Normalize();
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "Topic")
		{
			//collided with another topic
			
			Vector3 relativePos = coll.gameObject.transform.position - transform.position;

			if (Mathf.Abs(relativePos.x) > Mathf.Abs(relativePos.y))
			{
				//hit came from left or right
				moveDirection_.x = -moveDirection_.x;
			}
			else if (Mathf.Abs(relativePos.x) < Mathf.Abs(relativePos.y))
			{
				//hit came from up or down
				moveDirection_.y = -moveDirection_.y;
			}
			else
			{
				//hit came directly diagonal
				moveDirection_.x = -moveDirection_.x;
				moveDirection_.y = -moveDirection_.y;
			}
		}
	}
}
