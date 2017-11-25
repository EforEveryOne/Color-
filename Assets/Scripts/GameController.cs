using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {



	int gridX = 8;
	int gridY = 5;
	float turnTime = 2;
	int turnCounter = 0;



	int gameLength;
	static GameObject[,] grid;
	static Color[] myColors; 
	public int x, y;


	void CreateGrid () {

	}


	// Use this for initialization
	void Start () {

		CreateGrid ();

		
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > turnTime * turnCounter) {
			turnCounter++;
		}	
	}
		
	}
