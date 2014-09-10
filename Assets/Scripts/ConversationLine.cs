using UnityEngine;
using System.Collections;

public class ConversationLine : MonoBehaviour {

	GameLogic gameLogicScript;
	
	void Awake()
	{
		gameLogicScript = Camera.main.gameObject.GetComponent<GameLogic>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Topic")
		{
			gameLogicScript.TopicCollected();

			//This conversation line collects a topic
			Destroy(collider.gameObject);
		}
	}
}
