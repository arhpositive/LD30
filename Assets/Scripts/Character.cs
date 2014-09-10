using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

	//Public Variables
	public float initSpeed;
	public float maxSpeed;
	public float maxReverseSpeed;
	public float curSpeed;
	public float rotationSpeed;
	public float afterCollisionSpeed;
	public float rotationDeadZone;
	public float accelerationRate;
	public float decelerationRate;

	//Private Variables
	GameObject conversationChar;
	Vector2 curMoveDirection;
	Vector2 idealMoveDirection;
	bool isDecelerating;
	bool isStartChar_;
	bool reactToCollision;
	bool goodConversation;
	GameLogic gameLogicScript;

	void Awake()
	{
		gameLogicScript = Camera.main.gameObject.GetComponent<GameLogic>();
	}

	// Use this for initialization
	void Start () 
	{
		conversationChar = null;
		curMoveDirection = Vector2.zero;
		idealMoveDirection = Vector2.zero;
		isDecelerating = false;
		isStartChar_ = false;
		reactToCollision = false;
		goodConversation = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (reactToCollision && conversationChar)
		{
			print ("WARNING! Should not both react to a collision and still be in conversation!");
		}

		if (reactToCollision)
		{
			if(Time.fixedTime % 0.5f < 0.2f)
			{
				renderer.enabled = false;
			}
			else
			{
				renderer.enabled = true;
			}
		}

		//if the character does not want to move, we're in the beginning of the game
		if (idealMoveDirection != Vector2.zero || goodConversation)
		{
			//1: if the character is decelerating, reduce speed, if its accelerating, increase speed
			if (isDecelerating)
			{
				curSpeed = curSpeed - (decelerationRate * Time.deltaTime);
				
				if (curSpeed <= 0.0f)
				{
					isDecelerating = false;
					curMoveDirection = idealMoveDirection;
				}
				curSpeed = Mathf.Clamp(curSpeed, initSpeed, maxSpeed);
			}
			else
			{
				curSpeed = curSpeed + (accelerationRate * Time.deltaTime);
			}

			if(conversationChar)
			{
				curSpeed = Mathf.Clamp(curSpeed, initSpeed, maxSpeed);

				idealMoveDirection = conversationChar.transform.position - transform.position;
				idealMoveDirection.Normalize();

				if (!isDecelerating)
				{
					Vector3 rotationVector = DetermineRotationVector();
					if (rotationVector != Vector3.zero)
					{
						Vector3 centerPosition = (conversationChar.transform.position + transform.position) / 2.0f;
						transform.RotateAround(centerPosition, rotationVector, Time.deltaTime * rotationSpeed);
					}

					curMoveDirection = idealMoveDirection;
				}
			}
			else
			{
				if (reactToCollision)
				{
					curSpeed = Mathf.Clamp(curSpeed, initSpeed, afterCollisionSpeed);

					if (curSpeed <= maxReverseSpeed)
					{
						reactToCollision = false;
						renderer.enabled = true;
					}
				}
				else if(!isDecelerating)
				{
					if (goodConversation)
					{
						curSpeed = 0.0f;
					}
					else
					{
						curSpeed = Mathf.Clamp(curSpeed, initSpeed, maxReverseSpeed);
					}
				}
			}

			transform.Translate(curMoveDirection * curSpeed * Time.deltaTime, Space.World);
		}
	}

	Vector3 DetermineRotationVector()
	{
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		float rotationAxis = (conversationChar.transform.position.x - transform.position.x) *
		                      (mousePos.y - transform.position.y) -
		                      (conversationChar.transform.position.y - transform.position.y) *
		                      (mousePos.x - transform.position.x);

		if (rotationAxis < -rotationDeadZone)
		{
			if (isStartChar_)
			{
				return Vector3.forward;
			}
			else
			{
				return Vector3.back;
			}
		}
		else if (rotationAxis > rotationDeadZone)
		{
			if (isStartChar_)
			{
				return Vector3.back;
			}
			else
			{
				return Vector3.forward;
			}
		}
		else
		{
			return Vector3.zero;
		}
	}

	void CollideWithCharacter(GameObject otherChar)
	{
		curSpeed = afterCollisionSpeed;
		isDecelerating = true;
		reactToCollision = true;
		goodConversation = false;
		conversationChar = null;

		curMoveDirection = transform.position - otherChar.transform.position;
		curMoveDirection.Normalize();
		idealMoveDirection = transform.position - otherChar.transform.position;
		idealMoveDirection.Normalize();		
	}

	void BreakConversation()
	{
		//notify gamelogic about the broken conversation
		gameLogicScript.ConversationIsBroken();
	}

	public void BeginConversation(GameObject otherChar, bool isStartChar)
	{
		goodConversation = false;
		conversationChar = otherChar;
		isStartChar_ = isStartChar;
		idealMoveDirection = conversationChar.transform.position - transform.position;
		idealMoveDirection.Normalize();

		if (curMoveDirection != idealMoveDirection)
		{
			isDecelerating = true;
		}
	}

	public void EndConversation()
	{
		if (goodConversation)
		{
			idealMoveDirection = Vector2.zero;
		}
		else
		{
			if (conversationChar) //TODO: there was a crash here
			{
				idealMoveDirection = transform.position - conversationChar.transform.position;
				idealMoveDirection.Normalize();
			}

		}
		conversationChar = null;

		if (curMoveDirection != idealMoveDirection)
		{
			isDecelerating = true;
		}
	}

	public bool doesReactToCollision()
	{
		return reactToCollision;
	}

	public void hadGoodConversation()
	{
		//a character with a good conversation does not repel itself from the last character
		goodConversation = true;
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
		if (coll.gameObject.tag == "Character")
		{
			gameLogicScript.CharacterClash();

			CollideWithCharacter(coll.gameObject);
		}
		else if (coll.gameObject.tag == "Topic")
		{
			gameLogicScript.TopicWasted();
			//TODO you probably need to play some effects here
			
			Destroy(coll.gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Conversation" && !conversationChar)
		{
			// This character touched a conversation line while not in a conversation
			BreakConversation();
		}
	}
}
