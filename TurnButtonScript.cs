using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnButtonScript : MonoBehaviour {

	bool created = false;
	GameObject maincanvas;
	SceneChangeScript sceneChangeScript;
    GameObject controller;
    GameController gameController;   
	int buttonPresses;

	private void Awake()
	{
		if (!created) {

			created = true;
			buttonPresses = 0;
			maincanvas = GameObject.Find("MainCanvas(Clone)");
			controller = GameObject.Find("Controller");
            gameController = controller.GetComponent<GameController>();
			sceneChangeScript = maincanvas.transform.Find("DecayButton").GetComponent<SceneChangeScript>();

		}

	}


	public void OnClick() {
        
		buttonPresses++;

		if (buttonPresses % 2 == 1) {

			// Toggles Hand off
			sceneChangeScript.CompletelyOnOff(false, false, true, false);

			// Deactivate decay button
			maincanvas.transform.Find("DecayButton").GetComponent<Button>().enabled = false; 
            
			// Change button text
			transform.Find("Text").GetComponent<Text>().text = "Start Turn";         
            
            
		}

		else {
        
			// Start change turn procedure
            gameController.ChangeTurn();

			// Toggle interactability and viewability of all objects back on (except decay)         
			sceneChangeScript.ToggleInteract(true, true, true, false);
			sceneChangeScript.CompletelyOnOff(true, false, true, false);

			// Deactivate button
			gameObject.SetActive(false);

		}
	}

}
