using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// This manager script file is placed along with the camera for our game scene
// Every time a new game has begun, this script determines how many characters
// are going to be in the game and their parameters aswell.
// All of this information will be quasi-randomly determined.

public struct CharacterPersonality
{
	string name_;
	Color color_;
	bool characterUsed;

	public CharacterPersonality(string name, Color color)
	{
		name_ = name;
		color_ = color;
		characterUsed = false;
	}

	public string GetName()
	{
		return name_;
	}

	public Color GetColor()
	{
		return color_;
	}

	public bool isCharacterUsed()
	{
		return characterUsed;
	}

	public void setCharacterUsed()
	{
		characterUsed = true;
	}
}

public class CharacterManager : MonoBehaviour {

	CharacterPersonality[] personalities = new CharacterPersonality[8]
	{
		new CharacterPersonality("Steve", Color.red),
		new CharacterPersonality("Sam", Color.yellow),
		new CharacterPersonality("Andy", Color.green),
		new CharacterPersonality("Claire", Color.gray),
		new CharacterPersonality("Terrence", Color.blue),
		new CharacterPersonality("Joan", Color.white),
		new CharacterPersonality("Zoe", Color.magenta),
		new CharacterPersonality("Adrian", Color.cyan)
	};
	
	//Public Variables
	public GameObject charPrefab;
	public AudioClip spaceTrash2;

	//Private Variables
	List<CharacterPersonality> personalityList;
	List<GameObject> characterList;
	float sphereRadius = 2.0f;
	int numberOfCharacters;

	public List<GameObject> GetCharacterList()
	{
		return characterList;
	}

	// Use this for initialization
	void Start () 
	{
		numberOfCharacters = Random.Range(3, 7);
		InitializeCharacterList();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void InitializeCharacterList()
	{
		characterList = new List<GameObject>();
		characterList.Clear();

		InitializePersonalityList();

		Vector3 newCoords = Vector3.zero;
		bool coordsNotEmpty = true;

		for (int i = 0; i < numberOfCharacters; i++)
		{
			//get random position for character, while considering other characters aswell
			newCoords = Vector3.zero;
			coordsNotEmpty = true;
			while(coordsNotEmpty)
			{
				coordsNotEmpty = false;
				Rect playingArea = transform.GetComponent<GameLogic>().GetPlayingArea();
				newCoords.x = Random.Range(playingArea.xMin, playingArea.xMax);
				newCoords.y = Random.Range(playingArea.yMin, playingArea.yMax);

				for (int j = 0; j < characterList.Count; j++)
				{
					if (Vector3.Distance(characterList[j].transform.position, newCoords) <= sphereRadius * 2.0f)
					{
						coordsNotEmpty = true;
						break;
					}
				}
			}

			//TODO get character properties from character database

			//instantiate character
			GameObject curObject = Instantiate(charPrefab, newCoords, Quaternion.identity) as GameObject;
			curObject.renderer.material.color = personalityList[i].GetColor();
			characterList.Add(curObject);
		}
	}

	void InitializePersonalityList()
	{
		personalityList = new List<CharacterPersonality>();
		personalityList.Clear();

		for (int i = 0; i < numberOfCharacters; i++)
		{
			bool personalitySelected = false;
			while (!personalitySelected)
			{
				int index = Random.Range(0, personalities.Length);
				
				if (!personalities[index].isCharacterUsed())
				{
					personalities[index].setCharacterUsed();
					personalityList.Add(personalities[index]);
					personalitySelected = true;
				}
			}
		}
	}

	public void RemoveFromCharacterList(GameObject go)
	{
		//audio.PlayOneShot(spaceTrash2);
		//remove character from the list after it's destroyed
		characterList.Remove(go);

		if (characterList.Count <= 2)
		{
			//TODO when the high score menu opens, make sure to play some music
			//go to high score menu if you're out of characters 

			if (!Application.isLoadingLevel)
			{
				Application.LoadLevel(2);
			}
		}
	}
}
