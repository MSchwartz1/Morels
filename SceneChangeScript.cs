using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeScript : MonoBehaviour
{

	// Game Controller
	GameObject controller;
	GameController gameController;
	// Cook Button
    GameObject cookButton;
    CookScript cookScript;
    SpriteRenderer cookRend;
    // Stick Button
    GameObject stickButton;
    StickScript stickScript;
    SpriteRenderer stickRend;
    // Main canvas and associated texts objects and buttons
    Canvas maincanvas;
    Text cookButtonText;
    Text stickButtonText;
    Transform decayTrans;
    // Day Deck
    GameObject dayDeck;
    SpriteRenderer dayDeckRend;
    OnClickScript onClickScript;
	// Cards
	SpriteRenderer[] rends;

	public void ChangeScene(string sceneName)
	{

		GetObjects();

		// (De)activate appropriate game objects / components
		if (sceneName == "Decay")
		{

			DecayScene(true);
			MainScene(false);

		}

		else if (sceneName == "Main")
		{
			DecayScene(false);         
			MainScene(true);
		}

		// Change scenes
		SceneManager.LoadScene(sceneName);
	}

	private void MainScene(bool isEnabled)
	{
    
		// Toggle viewability and interactibility
		//ToggleView(isEnabled, true, true, false);
		//ToggleInteract(isEnabled, true, true, false);

        // Toggle main scene objects
		CompletelyOnOff(isEnabled, true, true, false);

	}

	private void DecayScene(bool isEnabled) 
	{
      
        // Toggle viewability and interactibility
        //ToggleView(isEnabled, false, false, true);
        //ToggleInteract(isEnabled, false, false, true);
        
		// Toggle decay scene objects
        CompletelyOnOff(isEnabled, false, false, true);

	}

	public void ToggleInteract(bool isEnabled, bool toggleForest, bool toggleHand, bool toggleDecay)
	{
		/*  Toggles interactable components of game objects (scripts / buttons) */
                    
		if (toggleForest)
		{

			cookScript.enabled = isEnabled;
            stickScript.enabled = isEnabled;
			decayTrans.GetComponent<Button>().enabled = isEnabled;         
                     
			foreach (GameObject card in gameController.Forest)
			{

				// Get scripts and disable
				onClickScript = card.GetComponent<OnClickScript>();
				onClickScript.enabled = isEnabled;


			}
		}
        
		if (toggleHand)
        {

            foreach (GameObject card in gameController.Hand)
            {

                // Get scripts and disable
                onClickScript = card.GetComponent<OnClickScript>();
                onClickScript.enabled = isEnabled;

            }
        }
              
		if (toggleDecay) {
        
			foreach (GameObject card in gameController.Decay)
            {

                // Get scripts and disable
                onClickScript = card.GetComponent<OnClickScript>();
                onClickScript.enabled = isEnabled;

            }


		}

	}

	public void ToggleView(bool isEnabled, bool toggleForest, bool toggleHand, bool toggleDecay)
	{
		/* Toggles viewable components of game objects (renderers / text / etc.) */
        
		if (toggleForest) 
		{
                 
            cookRend.enabled = isEnabled;
            stickRend.enabled = isEnabled;
            dayDeckRend.enabled = isEnabled;
            cookButtonText.enabled = isEnabled;
            stickButtonText.enabled = isEnabled;
			decayTrans.GetComponent<Image>().enabled = isEnabled;
            decayTrans.Find("Text").GetComponent<Text>().enabled = isEnabled;

			foreach (GameObject card in gameController.Forest) 
			{
                
				// Get sprite renderers of day card (front and back) and disable
                rends = card.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer rend in rends)
                {
                    rend.enabled = isEnabled;

                }


			}

		}

		if (toggleHand)
		{

			foreach (GameObject card in gameController.Hand)
			{

				// Get sprite renderers of day card (front and back) and disable
				rends = card.GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer rend in rends)
				{
					rend.enabled = isEnabled;
                    
				}

			}
            

		}

		if (toggleDecay)
        {
                 
            foreach (GameObject card in gameController.Decay)
            {

                // Get sprite renderers of day card (front and back) and disable
                rends = card.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer rend in rends)
                {
                    rend.enabled = isEnabled;

                }

            }


        }

	}

	public void CompletelyOnOff(bool isEnabled, bool toggleForest, bool toggleHand, bool toggleDecay) {

		/* Completely turns game objects on / off. This is more complete and less
        buggy than turning off the visual AND interactable components of gameobjects */

		if (toggleForest) {

			cookButton.SetActive(isEnabled);
			stickButton.SetActive(isEnabled);
			dayDeck.SetActive(isEnabled); 
			maincanvas.gameObject.SetActive(isEnabled);

			foreach (GameObject card in gameController.Forest)
            {

                card.SetActive(isEnabled);

            }

		}

		if (toggleHand) {

			foreach (GameObject card in gameController.Hand) {

				card.SetActive(isEnabled);

			}

		}

		if (toggleDecay) {

			foreach (GameObject card in gameController.Decay) {

				card.SetActive(isEnabled);

			}

		}

	}

	public void GetObjects()
    {

        // Get access to necessary game objects and their components
        controller = GameObject.Find("Controller");
        gameController = controller.GetComponent<GameController>();
		cookButton = gameController.cookbuttonclone;
        cookScript = cookButton.GetComponent<CookScript>();
        cookRend = cookButton.GetComponent<SpriteRenderer>();
		stickButton = gameController.stickbuttonclone;
        stickScript = stickButton.GetComponent<StickScript>();
        stickRend = stickButton.GetComponent<SpriteRenderer>();
		maincanvas = gameController.canvasclone;
        cookButtonText = maincanvas.transform.Find("Cook Text").GetComponent<Text>();
        stickButtonText = maincanvas.transform.Find("Stick Text").GetComponent<Text>();
        decayTrans = maincanvas.transform.Find("DecayButton");
		dayDeck = gameController.daydeckclone;
        dayDeckRend = dayDeck.GetComponent<SpriteRenderer>();

    }

}



