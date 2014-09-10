using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour {

	CharacterManager characterManager;
	GameLogic gameLogic;

	void Awake()
	{
		characterManager = Camera.main.GetComponent<CharacterManager>();
		gameLogic = Camera.main.GetComponent<GameLogic>();
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Character")
		{
			gameLogic.characterRemoved(collider.gameObject);
			characterManager.RemoveFromCharacterList(collider.gameObject);
		}
		//TODO play any related effect here
		Destroy(collider.gameObject);
	}
}
