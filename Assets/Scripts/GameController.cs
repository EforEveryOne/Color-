using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	public GameObject cubePrefab;
	public Text scoreText;
	public Text nextCubeText;
	public Text timerText;


	//count down timer. 
	static float gameLength = 60;
	int timerCountUp = -1; // This variable is used to count up and keep track of seconds passed. Starts at -1 so timer ends at 0, not 1. This ends up conting 0 as a full second.
	// keeps the timer equal to the length of the game so it accurately reflects what is happening just by changing gameLength.
	float timerCountDown = gameLength;

	int gridX = 8;
	int gridY = 5;
	GameObject[,] grid;
	GameObject nextCube;
	Vector3 cubePos;
	Vector3 nextCubePos = new Vector3 (7, 10, 0);
	float turnTime = 2;
	int turnCounter = 0;
	Color[] myColors = { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta };
	int score = 0;
	GameObject activeCube = null;
	int rainbowPoints = 5;
	int sameColorPoints = 10;
	bool gameOver = false;




	// TO-DO-LIST
		//AUDIO
	//add music to main menu+instructions.
	//add music to loss scene
	//add music to win scene

		//VISUAL 
	//make timer text turn red when it reaches 10 seconds

	// not lost or win scene, same scene as main game but button appears for loss AND/OR win. play again button.

	// add secne for start(main menu). (start button AND instructions button)
	// clickable text? scene needs text at least. (if can make button, can use cubes to be buttons/transition between scenes?)

	// add scene for instructions (back to main menu button)
	// add text to the scene, explain the rules of the game.

	// DONE
	// add cube row numbers to main scene.
	//make scored cubes (the plus shape) grow bigger.
	//add background image!
	//add sound to cube clicks
	//add sound to next cube spawn
	//add sound to black cube place on a row.
	//add music for gameplay


	//background music, main scene
	public AudioClip backgroundMusicSoundFile;
	AudioSource backgroundMusic;

	//activate cube sound
	public AudioClip activateCubeclackMinimalUISound;
    AudioSource activateCubeSound;

	//deactive cube sound
	public AudioClip deactivateCubecoarseClickMinimalUISound;
    AudioSource deactivateCubeSound;

	//active cube move sound
	public AudioClip activeCubeMoveFunnyClickMinimalUIsound;
	AudioSource activeCubeMoveSound;

	//blackcube auto place sound
	public AudioClip nextCubeFailPlacepower_2_a;
    AudioSource nextCubeFailPlaceSound;

	//score sound
	public AudioClip scorePointsSoundCollect_Point_00;
	AudioSource scorePointsSound;	

	//win sound
	public AudioClip winGameSoundJingle_Win_00;
	AudioSource winGameSound;

	//lose sound
	public AudioClip loseGameSoundHero_Death_00;
	AudioSource loseGameSound;






	// public AudioClip ;
	// AudioSource ;



	// code is here, reason is to keep start "cleaner" and easier to read. I don't think it makes a difference at this project scale.
	void Awake () {
		backgroundMusic = GetComponent<AudioSource> ();
		activateCubeSound = GetComponent<AudioSource> ();
		deactivateCubeSound = GetComponent<AudioSource> ();
		activeCubeMoveSound = GetComponent<AudioSource> ();
		nextCubeFailPlaceSound = GetComponent<AudioSource> ();
		scorePointsSound = GetComponent<AudioSource> ();
		winGameSound = GetComponent<AudioSource> ();
		loseGameSound  = GetComponent<AudioSource> ();
	}


	// Use this for initialization
	// In the start method we create the grid of cubes by instantiating them.
	void Start () {

		backgroundMusic.PlayOneShot (backgroundMusicSoundFile, 0.2f);

		CreateGrid ();
	}

	void CreateGrid () {
		grid = new GameObject[gridX, gridY];
	//this is the loop that creates the grid of cubes
		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				cubePos = new Vector3 (x * 2, y * 2, 0);
				grid[x,y] = Instantiate (cubePrefab, cubePos, Quaternion.identity);
				grid [x, y].GetComponent<CubeController> ().myX = x;
				grid [x, y].GetComponent<CubeController> ().myY = y;
			}
		}
	}

	void CreateNextCube () {
			nextCube = Instantiate (cubePrefab, nextCubePos, Quaternion.identity);
	    	nextCube.GetComponent<Renderer> ().material.color = myColors [Random.Range (0, myColors.Length)];
		    nextCube.GetComponent<CubeController> ().nextCube = true;
	}

	// Ends the game, this is called when the end game conditions are met.
	void EndGame (bool win) {
		if (win) {
			// you won

			nextCubeText.text = "You won the game!";
			//have to stop backgroundmusic first otherwise the win sound doesn't play. I don't know why that is.
				backgroundMusic.Stop();

			winGameSound.PlayOneShot (winGameSoundJingle_Win_00, 1);
		
		}
		else {
			// you lost

			nextCubeText.text = "You lost. Please try again!";
			//have to stop backgroundmusic first otherwise the win sound doesn't play. I don't know why that is.
				backgroundMusic.Stop();

			loseGameSound.PlayOneShot (loseGameSoundHero_Death_00, 1);
		}
		// get rid of the next cube
		Destroy (nextCube);
		nextCube = null;

		// disable everything (make it nextCube but nextCube can't be clicked!
		for (int x = 0; x < gridX; x++){
			for (int y = 0; y < gridY; y++){
				grid [x, y].GetComponent<CubeController> ().nextCube = true;
		}
		gameOver = true;
	}
	}
		
	GameObject PickWhiteCube (List<GameObject> whiteCubes) {
		if (whiteCubes.Count == 0) {
			// error value
			return null;
		}
		// pick a random white cube
		return whiteCubes [Random.Range(0, whiteCubes.Count)];
	}


	//go over this, understand it and be able to describe it.
	GameObject FindAvailableCube (int y) {
		List<GameObject> whiteCubes = new List<GameObject> ();
		for (int x = 0; x < gridX; x++) {
			if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) {
				whiteCubes.Add (grid[x, y]);
			}
		}
		return PickWhiteCube (whiteCubes);
	}
	GameObject FindAvailableCube () {
		List<GameObject> whiteCubes = new List<GameObject> ();
		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				if (grid [x, y].GetComponent<Renderer> ().material.color == Color.white) {
					whiteCubes.Add (grid[x, y]);
				}
			}
		}
		return PickWhiteCube (whiteCubes);
	}
		
	//does a lot more than just setting the color
	//set cube color or end game
	void SetCubeColor (GameObject myCube, Color color) {
		//no available cube in that row
		if (myCube == null) {
			
			EndGame (false);
		} else {
			//gives the cube in row the color of the next cube
			myCube.GetComponent<Renderer> ().material.color = color;
			Destroy (nextCube);
			nextCube = null;
		}	
	}
		
	void PlaceNextCube (int y) {
		GameObject whiteCube = FindAvailableCube (y);
		SetCubeColor (whiteCube, nextCube.GetComponent<Renderer> ().material.color);

		activeCubeMoveSound.PlayOneShot (activeCubeMoveFunnyClickMinimalUIsound, 1);
	}

	void AddBlackCube() {
		GameObject whiteCube = FindAvailableCube ();

		// use a color value that is beyond max
		SetCubeColor (whiteCube, Color.black);
// Make the black placed cube from the next cube area be small, the space cannot be interacted with. The play did NOT score this cube.
		whiteCube.transform.localScale /= 1.2f;

		nextCubeFailPlaceSound.PlayOneShot (nextCubeFailPlacepower_2_a, 0.2f);


	}
		
	void ProcessKeyboardInput () {
		int numKeyPressed = 0;

		if (Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) {
			numKeyPressed = 5;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) {
			numKeyPressed = 4;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3) || Input.GetKeyDown (KeyCode.Keypad3)) {
			numKeyPressed = 3;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4) || Input.GetKeyDown (KeyCode.Keypad4)) {
			numKeyPressed = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha5) || Input.GetKeyDown (KeyCode.Keypad5)) {
			numKeyPressed = 1;
		}
		// if there is a cube in the nextcube area
		if (nextCube != null && numKeyPressed != 0) {

			// place it in the designated row,
			//-1 because grid array is 0 base, this way makes more sense to align with x/y value
			PlaceNextCube (numKeyPressed-1);
		}
	}

	public void ProcessClick (GameObject clickedCube, int x, int y, Color cubeColor, bool active) {

		//checks the color of cube, nothing happens if white or black.
		if (cubeColor != Color.white && cubeColor != Color.black) {

			// if the specific cube is an active cube
			if (active) {
				//deactivate it
				clickedCube.transform.localScale /= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = false;
				activeCube = null;
				deactivateCubeSound.PlayOneShot (deactivateCubecoarseClickMinimalUISound, 1);

			}
			// the cube is not an active cube
			else {
				//deactivate previous cube
				if (activeCube != null) {
					activeCube.transform.localScale /= 1.5f;
					activeCube.GetComponent<CubeController> ().active = false;

					deactivateCubeSound.PlayOneShot (deactivateCubecoarseClickMinimalUISound, 1);
				}

				//activate it
				clickedCube.transform.localScale *= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = true;
				activeCube = clickedCube;
				activateCubeSound.PlayOneShot (activateCubeclackMinimalUISound, 1);

			}
		}
		else if (cubeColor == Color.white && activeCube != null) {
			int xDist = clickedCube.GetComponent<CubeController> ().myX - activeCube.GetComponent<CubeController> ().myX;
			int yDist = clickedCube.GetComponent<CubeController> ().myY - activeCube.GetComponent<CubeController> ().myY;

			if (Mathf.Abs (yDist) <= 1 && Mathf.Abs (xDist) <= 1) {
				//set clicked cube active.
				clickedCube.GetComponent<Renderer> ().material.color = activeCube.GetComponent<Renderer> ().material.color;
				clickedCube.transform.localScale *= 1.5f;
				clickedCube.GetComponent<CubeController> ().active = true;


				// set the other cube to not be active
				activeCube.GetComponent<Renderer> ().material.color = Color.white;
				activeCube.transform.localScale /= 1.5f;
				activeCube.GetComponent<CubeController> ().active = false;

				// keeping track of active cube (passing on the torch!)
				activeCube = clickedCube;

				activeCubeMoveSound.PlayOneShot (activeCubeMoveFunnyClickMinimalUIsound, 1);
			}
		}
	}

	bool IsRainbowPlus (int x, int y) {
		Color a = grid [x, y].GetComponent<Renderer> ().material.color;
		Color b = grid [x + 1, y].GetComponent<Renderer> ().material.color;
		Color c = grid [x - 1, y].GetComponent<Renderer> ().material.color;
		Color d = grid [x, y + 1].GetComponent<Renderer> ().material.color;
		Color e = grid [x, y - 1].GetComponent<Renderer> ().material.color;

				//keeps black/white out of the rainbow
		if (a == Color.white || a == Color.black ||
		      b == Color.white || b == Color.black ||
		      c == Color.white || c == Color.black ||
		      d == Color.white || d == Color.black ||
		      e == Color.white || e == Color.black) {
			return false;
		}

				//Ensure the colors are different from eachother
		if (a != b && a != c && a != d && a != e &&
		    b != c && b != d && b != e &&
		    c != d && c != e &&
		    d != e) {
			return true;
		} else {
			return false;
		}
	}

	bool IsSameColorPlus (int x, int y) {
			
		if (grid [x, y].GetComponent<Renderer> ().material.color != Color.white &&
			grid [x, y].GetComponent<Renderer> ().material.color != Color.black &&
			grid [x, y].GetComponent<Renderer> ().material.color ==	grid [x + 1, y].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color ==	grid [x - 1, y].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color ==	grid [x, y + 1].GetComponent<Renderer> ().material.color &&
			grid [x, y].GetComponent<Renderer> ().material.color ==	grid [x, y - 1].GetComponent<Renderer> ().material.color) {
			return true;
		}
		else{
			return false;
		}
	}

	void MakeBlackPlus (int x, int y) {
		//this is an error check to ensure that the x and y aren't on the edge of the grid. (shouldn't show up on the edge regardless but useful practice)
		if (x == 0 || y == 0 || x == gridX -1 || y == gridY -1){
			return;
		}
//turns the cubes at the grid locaiton black.
		grid [x, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x + 1, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x - 1, y].GetComponent<Renderer> ().material.color = Color.black;
		grid [x, y + 1].GetComponent<Renderer> ().material.color = Color.black;
		grid [x, y - 1].GetComponent<Renderer> ().material.color = Color.black;
// Make the scored plus grow big, to destinguish it. The play SCORED these black cubes.
		grid [x, y].transform.localScale *= 1.5f;
		grid [x + 1, y].transform.localScale *= 1.5f;
		grid [x - 1, y].transform.localScale *= 1.5f;
		grid [x, y + 1].transform.localScale *= 1.5f;
		grid [x, y - 1].transform.localScale *= 1.5f;


		//if active cube is involved in the plus
		if (activeCube != null && activeCube.GetComponent<Renderer> ().material.color == Color.black) {
			//deactivate it
			activeCube.transform.localScale /= 1.5f;
			activeCube.GetComponent<CubeController> ().active = false;
			activeCube = null;
		}
	}

	void Score (){
		
		//checks grid for plusses but not the edges.
		for (int x = 1; x < gridX - 1; x++) {
			for (int y = 1; y < gridY - 1; y++){

				if (IsRainbowPlus (x, y)) {
					score += rainbowPoints;
					MakeBlackPlus(x, y);

					scorePointsSound.PlayOneShot (scorePointsSoundCollect_Point_00, 1);
				}
				if (IsSameColorPlus (x, y)) {
					score += sameColorPoints;
					MakeBlackPlus(x, y);

					scorePointsSound.PlayOneShot (scorePointsSoundCollect_Point_00, 1);
		    	}
	    	}
    	}
	}

	// Update is called once per frame
	void Update () {


		// if there is time left in the game
		if (Time.time <= gameLength) { 


			if (!gameOver){
				// if the game isn't over, continue with these functions every update.
				ProcessKeyboardInput ();
				Score ();

				//backgroundMusic.PlayOneShot (backgroundMusicSoundFile, 0.2f);


			//	if (timerCountDown <= 10){
			//		timerText.GetComponent(Text).color = Color.red;	




			//		timerText = new gameobject.GetComponent<Text>();
			//		timerText.color = new Color red;}

			}
			// THIS IS where the bug is? proesskeyboard needs to stop if cube placed in a full row.

			if (Time.time > timerCountUp && !gameOver ) {
				timerCountUp++;
				timerText.text = "Time: " + timerCountDown;
				timerCountDown--;
			}
			//possible if statment saying if gameLength is 0, timer = 0?



			if (Time.time > turnTime * turnCounter && !gameOver) {
				turnCounter++;

				// if we still have an exisiting next cube when the turn passes
				if (nextCube != null) {
					score -= 1;

					//keep score above negative
					if (score < 0) {
						score = 0;
					}
					AddBlackCube ();
				}
				CreateNextCube ();
			}	

			// update the Score UI
			scoreText.text = "Score: " + score;
		}
			
		//game is over, time is up
		else if (!gameOver) {
			// players win if score is greater than 0 when time is up
			if (score > 0) {
				//win? change variable name to be more clear, based on bool though...			
				EndGame (true);
			}
			else {
				//lose? change variable name to be more clear
				EndGame (false);
			}
		}
	}
	}