using UnityEngine;
using System.Collections;

public class UserInterface : MonoBehaviour {

	//Public Variables
	public Font mainMenuFont;
	public Font textFont;
	public Texture newGameButtonTexture;
	public Texture howToPlayButtonTexture;
	public Texture creditsButtonTexture;
	public Texture restartButtonTexture;
	public Texture mainMenuButtonTexture;
	public Texture closeButtonTexture;
	public float scoreXCoord;
	public float scoreYCoord;

	//Private Variables
	private static int playerScore = 0;
	private GUIStyle titleStyle;
	private GUIStyle blackStyle;
	private GUIStyle boldStyle;
	private GUIStyle slimStyle;
	private GUIStyle smallStyle;

	// Use this for initialization
	void Start () {
		titleStyle = new GUIStyle();
		titleStyle.font = mainMenuFont;
		titleStyle.fontSize = 80;
		titleStyle.alignment = TextAnchor.MiddleRight;
		titleStyle.normal.textColor = Color.black;
		titleStyle.wordWrap = true;
		slimStyle = new GUIStyle();
		slimStyle.font = textFont;
		slimStyle.fontSize = 30;
		slimStyle.alignment = TextAnchor.UpperCenter;
		slimStyle.normal.textColor = Color.black;
		slimStyle.wordWrap = true;
		blackStyle = new GUIStyle();
		blackStyle.font = mainMenuFont;
		blackStyle.fontSize = 30;
		blackStyle.alignment = TextAnchor.UpperCenter;
		blackStyle.normal.textColor = Color.black;
		blackStyle.wordWrap = true;
		boldStyle = new GUIStyle();
		boldStyle.fontSize = 30;
		boldStyle.alignment = TextAnchor.MiddleLeft;
		boldStyle.normal.textColor = Color.black;
		boldStyle.wordWrap = true;
		smallStyle = new GUIStyle();
		smallStyle.font = textFont;
		smallStyle.fontSize = 18;
		smallStyle.alignment = TextAnchor.UpperLeft;
		smallStyle.normal.textColor = Color.black;
		smallStyle.wordWrap = true;
	}

	void OnGUI()
	{
		GUI.backgroundColor = Color.clear;

		//MAIN MENU
		if (Application.loadedLevel == 0)
		{
			//TODO how to play, enter game, and credits

			GUI.BeginGroup(new Rect(60, 60, (Screen.width / 2) - 60, Screen.height - 120));


			GUI.Label(new Rect(10, 10, (Screen.width / 2) - 80, Screen.height - 140), "CON\nVER\nSA\nTION", titleStyle);
			GUI.EndGroup();

			GUI.BeginGroup(new Rect((Screen.width / 2) + 60, 60, (Screen.width / 2) - 60, Screen.height - 120));

			if (GUI.Button(new Rect(10, 82, 256, 64), newGameButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(1);
			}

			if (GUI.Button(new Rect(10, 178, 256, 64), howToPlayButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(3);
			}
			
			if (GUI.Button(new Rect(10, 274, 256, 64), creditsButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(4);
			}
			GUI.EndGroup();
		}
		else if (Application.loadedLevel == 1)
		{
			//TODO display player score here and an exit button
			GUI.Label(new Rect(scoreXCoord, scoreYCoord, Screen.width - scoreXCoord, 30), "SCORE: " + playerScore, boldStyle);

			if (GUI.Button(new Rect(Screen.width - 72, 8, 64, 64), closeButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(2);
			}
		}
		else if (Application.loadedLevel == 2)
		{
			GUI.BeginGroup(new Rect((Screen.width / 2) - 138, 60, (Screen.width / 2) + 138, Screen.height - 120));

			GUI.Label(new Rect(3, 82, 270, 64), "Score: " + playerScore, blackStyle);
			
			if (GUI.Button(new Rect(10, 178, 256, 64), restartButtonTexture) && !Application.isLoadingLevel)
			{
				playerScore = 0;
				Application.LoadLevel(1);
			}
			
			if (GUI.Button(new Rect(10, 274, 256, 64), mainMenuButtonTexture) && !Application.isLoadingLevel)
			{
				playerScore = 0;
				Application.LoadLevel(0);
			}
			GUI.EndGroup();
		}
		else if (Application.loadedLevel == 3)
		{
			//HOWTO
			if (GUI.Button(new Rect(Screen.width - 72, 8, 64, 64), closeButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(0);
			}
			
			GUI.Label(new Rect(50, 50, 620, 25), "HOW TO PLAY", smallStyle);
			GUI.Label(new Rect(50, 80, 620, 25), "1. There are some people who want to talk to each other.", smallStyle);
			GUI.Label(new Rect(50, 110, 620, 25), "2. Start a conversation with the closest character by clicking on someone.", smallStyle);
			GUI.Label(new Rect(50, 160, 620, 25), "3. There are conversation topics flying around. Main goal is to catch these square markers with your conversation line.", smallStyle);
			GUI.Label(new Rect(50, 210, 620, 25), "4. Succesfully catch the markers with the line and characters stay closer. Fail to catch, and characters will become further apart.", smallStyle);
			GUI.Label(new Rect(50, 260, 620, 25), "5. Crashing onto other characters or collecting conversation markers with characters themselves results in loss of points.", smallStyle);
			GUI.Label(new Rect(50, 310, 620, 25), "6. Characters crashing onto ongoing conversation lines will also lose you points. ", smallStyle);
			GUI.Label(new Rect(50, 360, 620, 25), "7. Game ends when all but two characters are out of the screen or whenever you quit. ", smallStyle);
			GUI.Label(new Rect(50, 460, 620, 25), "Enjoy. At its current state it's a very hard and unbalanced game.", smallStyle);
		}
		else if (Application.loadedLevel == 4)
		{
			//CREDITS
			if (GUI.Button(new Rect(Screen.width - 72, 8, 64, 64), closeButtonTexture) && !Application.isLoadingLevel)
			{
				Application.LoadLevel(0);
			}
			
			GUI.Label(new Rect(100, 100, 520, 40), "CONVERSATION", blackStyle);
			GUI.Label(new Rect(100, 150, 520, 40), "by Arhan \"aRHpositive\" Bakan", blackStyle);
			GUI.Label(new Rect(100, 200, 520, 40), "for Ludum Dare 30", slimStyle);

			GUI.Label(new Rect(100, 300, 520, 40), "Fonts: CODE by Fontfabric", slimStyle);
			GUI.Label(new Rect(100, 350, 520, 40), "SFX: by Kenney", slimStyle);
			GUI.Label(new Rect(100, 400, 520, 40), "Music: Otomata", slimStyle);
		}
	}

	public void UpdateScore(int addition)
	{
		playerScore += addition;
	}
}
