using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {
	public GameObject cubePrefab;
	float gameLength = 60;
	int gridX = 8;
	int gridY = 5;
	GameObject[,] grid;
	GameObject nextCube;
	Vector3 cubePos;
	Vector3 nextCubePos = new Vector3 (7, 10, 0);
	float turnTime = 2;
	int turnCounter = 0;
	Color[] myColors = { Color.blue, Color.red, Color.green, Color.yellow, Color.magenta };

	void CreateGrid () {
		grid = new GameObject[gridX, gridY];

		for (int y = 0; y < gridY; y++) {
			for (int x = 0; x < gridX; x++) {
				cubePos = new Vector3 (x * 2, y * 2, 0);
		        Instantiate (cubePrefab, cubePos, Quaternion.identity);
			}
		}
	}



	void CreateNextCube () {
		if (nextCube == null) {
			nextCube = Instantiate (cubePrefab, nextCubePos, Quaternion.identity);
		}
		nextCube.GetComponent<Renderer> ().material.color = myColors [Random.Range (0, myColors.Length)];
	}

	// Use this for initialization
	void Start () {

		CreateGrid ();
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time > turnTime * turnCounter) {
			turnCounter++;


			CreateNextCube ();

		}	
	}
		
	}
