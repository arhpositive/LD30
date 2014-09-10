using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This script file is related to the main game loop
// Situations we need to check for in every frame are usually handled here

public class GameLogic : MonoBehaviour {

	//Public Variables
	public GameObject lineRendererPrefab;
	public GameObject topicPrefab;
	public float topicCreationRate;
	public AudioClip tone1;
	public AudioClip twoTone1;
	public AudioClip zapTwoTone2;
	public AudioClip zapThreeToneDown;
	public AudioClip otomataMusic;

	// Private Variables
	UserInterface userInterface;
	CharacterManager characterManager;
	GameObject conversationStartChar;
	Character startCharScript;
	GameObject conversationSecondChar;
	Character secondCharScript;
	GameObject conversationLine;
	Rect playingArea;
	float topicCreationTimer;
	float topicBorderOffset;
	//float conversationTimer;

	void Awake()
	{
		characterManager = gameObject.GetComponent<CharacterManager>();
		userInterface = gameObject.GetComponent<UserInterface>();
		playingArea = new Rect(2.0f, 2.0f, 12.0f, 8.0f);
	}

	// Use this for initialization
	void Start () {
		conversationStartChar = null;
		startCharScript = null;
		conversationSecondChar = null;
		secondCharScript = null;

		conversationLine = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		DisableConversationLine();

		topicCreationTimer = 1.0f;
		topicBorderOffset = 3.0f;

		//conversationTimer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		if (topicCreationTimer > 0.0f)
		{
			topicCreationTimer -= Time.deltaTime;
		}
		else
		{
			InstantiateNewTopic();
			topicCreationTimer = topicCreationRate;
		}

		//mouse click events here
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if (hit && hit.collider && hit.collider.gameObject.tag == "Character")
			{
				// step1: choose closest character that is still in borders
				//TODO consider moving this part to character.cs
				//TODO do not forget to add borders to calculation later

				if (!hit.collider.gameObject.GetComponent<Character>().doesReactToCollision())
				{
					conversationStartChar = hit.collider.gameObject;
					startCharScript = conversationStartChar.GetComponent<Character>();
					
					List<GameObject> characterList = characterManager.GetCharacterList();
					
					float closestDistance = float.MaxValue;
					int closestCharacterId = -1;
					for (int i = 0; i < characterList.Count; i++)
					{
						float distance = Vector2.Distance(characterList[i].transform.position, conversationStartChar.transform.position);
						
						if (!characterList[i].Equals(conversationStartChar) && distance < closestDistance &&
						    !characterList[i].GetComponent<Character>().doesReactToCollision())
						{
							closestDistance = distance;
							closestCharacterId = i;
						}
					}
					
					if (closestCharacterId != -1)
					{
						conversationSecondChar = characterList[closestCharacterId];
						secondCharScript = conversationSecondChar.GetComponent<Character>();
						
						//conversationTimer = Time.time;
						
						EnableConversationLine();
						startCharScript.BeginConversation(conversationSecondChar, true);
						secondCharScript.BeginConversation(conversationStartChar, false);
					}
				}
			}
			else
			{
				ConversationEnded();
			}
		}
		else if (Input.GetMouseButton(0))
		{
			//the mouse was not clicked this frame but it's still being pressed
			if (startCharScript && secondCharScript)
			{
				if (startCharScript.doesReactToCollision() || secondCharScript.doesReactToCollision())
				{
					//if someone in the conversation collide with a character and the other didn't
					//that one has to be notified that the conversation has ended
					if (!startCharScript.doesReactToCollision())
					{
						startCharScript.EndConversation();
					}
					else if (!secondCharScript.doesReactToCollision())
					{
						secondCharScript.EndConversation();
					}
					ConversationEnded();
				}
				else
				{
					UpdateConversationLine();
				}
			}
		}
		else if (conversationStartChar && conversationSecondChar)
		{
			//lose focus of characters
			startCharScript.EndConversation();
			secondCharScript.EndConversation();

			ConversationEnded();
		}
	}

	void ConversationEnded()
	{
		//float conversationLength = Time.time - conversationTimer;
		//playerScore += (int)(conversationLength * 10.0f);
		//print(playerScore);

		DisableConversationLine();
		conversationStartChar = null;
		conversationSecondChar = null;
		startCharScript = null;
		secondCharScript = null;
	}

	void DisableConversationLine()
	{
		conversationLine.renderer.enabled = false;
		conversationLine.GetComponent<LineRenderer>().enabled = false;
		conversationLine.GetComponent<EdgeCollider2D>().enabled = false;
	}

	void EnableConversationLine()
	{
		LineRenderer lineRenderer = conversationLine.GetComponent<LineRenderer>();
		lineRenderer.material.color = Color.red;
		conversationLine.renderer.enabled = true;
		lineRenderer.renderer.enabled = true;
		conversationLine.GetComponent<EdgeCollider2D>().enabled = true;
		UpdateConversationLine();
	}

	void UpdateConversationLine()
	{
		LineRenderer lineRenderer = conversationLine.GetComponent<LineRenderer>();
		lineRenderer.SetPosition(0, conversationStartChar.transform.position);
		lineRenderer.SetPosition(1, conversationSecondChar.transform.position);
		
		EdgeCollider2D edgeCollider = conversationLine.GetComponent<EdgeCollider2D>();

		Vector2[] tempArray = edgeCollider.points;
		tempArray[0] = conversationStartChar.transform.position;
		tempArray[1] = conversationSecondChar.transform.position;
		edgeCollider.points = tempArray;
	}

	void InstantiateNewTopic()
	{
		//instantiate new topic at one edge of the playing area

		int spawnSideRandomizer = Random.Range(0, 4);
		Vector3 randomPos = new Vector3(-1.0f, -1.0f, 0.0f);

		switch(spawnSideRandomizer)
		{
		case 0:
			//left
			randomPos.x = playingArea.xMin - topicBorderOffset;
			randomPos.y = Random.Range(playingArea.yMin - topicBorderOffset, playingArea.yMax + topicBorderOffset);
			break;
		case 1:
			//right
			randomPos.x = playingArea.xMax + topicBorderOffset;
			randomPos.y = Random.Range(playingArea.yMin - topicBorderOffset, playingArea.yMax + topicBorderOffset);
			break;
		case 2:
			//bottom
			randomPos.x = Random.Range(playingArea.xMin - topicBorderOffset, playingArea.xMax + topicBorderOffset);
			randomPos.y = playingArea.yMin - topicBorderOffset;
			break;
		case 3:
			//top
			randomPos.x = Random.Range(playingArea.xMin - topicBorderOffset, playingArea.xMax + topicBorderOffset);
			randomPos.y = playingArea.yMax + topicBorderOffset;
			break;
		default:
			break;
		}

		GameObject newTopic = Instantiate(topicPrefab, randomPos, Quaternion.identity) as GameObject;

	}

	public void TopicCollected()
	{
		LineRenderer lineRenderer = conversationLine.GetComponent<LineRenderer>();
		lineRenderer.material.color = Color.green;

		audio.PlayOneShot(zapTwoTone2, 1.0f);
		//topic collected by conservation line
		userInterface.UpdateScore(10);

		//tell both characters that they're going to be able to stop
		startCharScript.hadGoodConversation();
		secondCharScript.hadGoodConversation();
	}

	public void TopicWasted()
	{
		audio.PlayOneShot(tone1, 1.0f);
		//topic collides with character
		userInterface.UpdateScore(-1);
	}

	public void CharacterClash()
	{
		audio.PlayOneShot(twoTone1, 1.0f);
		//characters bumped onto each other, a serious argument during conservation
		userInterface.UpdateScore(-2);
	}

	public void ConversationIsBroken()
	{
		audio.PlayOneShot(zapThreeToneDown, 1.0f);

		userInterface.UpdateScore(-5);
		//due to an interruption, conversation is broken
		startCharScript.EndConversation();
		secondCharScript.EndConversation();
		
		ConversationEnded();
	}

	public Rect GetPlayingArea()
	{
		return playingArea;
	}

	public void characterRemoved(GameObject go)
	{
		if (go == conversationStartChar || go == conversationSecondChar)
		{
			ConversationEnded();
		}
	}
}
