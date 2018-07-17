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
    // Main canvas and associated texts objects and buttons
    Canvas maincanvas;   
    Text cookButtonText;
    Text stickButtonText;
	Text scoreText;
    Transform decayTrans;
	Transform cookTrans;
	Transform stickTrans;
    // Day Deck
    GameObject dayDeck;
    SpriteRenderer dayDeckRend;
    OnClickCardScript onClickCardScript;
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
   
        // Toggle main scene objects
		CompletelyOnOff(isEnabled, true, true, false);

	}

	private void DecayScene(bool isEnabled) 
	{
      

		// Toggle decay scene objects
        CompletelyOnOff(isEnabled, false, false, true);

	}
    
	public void ToggleInteract(bool isEnabled, bool toggleForest, bool toggleHand, bool toggleDecay)
	{
		/*  Toggles interactable components of game objects (scripts / buttons) */
		cookTrans.GetComponent<CookandTradeScript>().enabled = isEnabled;
		stickTrans.GetComponent<CookandTradeScript>().enabled = isEnabled;

		if (toggleForest)
		{
            

			decayTrans.GetComponent<Button>().enabled = isEnabled;
			cookTrans.GetComponent<Button>().enabled = isEnabled;
			stickTrans.GetComponent<Button>().enabled = isEnabled;

			foreach (GameObject card in gameController.Forest)
			{

				// Get scripts and disable
				onClickCardScript = card.GetComponent<OnClickCardScript>();
				onClickCardScript.enabled = isEnabled;


			}
		}
        
		if (toggleHand)
        {

            foreach (GameObject card in gameController.Hand)
            {

                // Get scripts and disable
                onClickCardScript = card.GetComponent<OnClickCardScript>();
                onClickCardScript.enabled = isEnabled;

            }
        }
              
		if (toggleDecay) {
        
			foreach (GameObject card in gameController.Decay)
            {

                // Get scripts and disable
                onClickCardScript = card.GetComponent<OnClickCardScript>();
                onClickCardScript.enabled = isEnabled;

            }


		}

	}

	public void ToggleView(bool isEnabled, bool toggleForest, bool toggleHand, bool toggleDecay)
	{
		/* Toggles viewable components of game objects (renderers / text / etc.) */
        
		if (toggleForest) 
		{
                 
            dayDeckRend.enabled = isEnabled;
            cookButtonText.enabled = isEnabled;
            stickButtonText.enabled = isEnabled;
			scoreText.enabled = isEnabled;
			decayTrans.GetComponent<Image>().enabled = isEnabled;
            decayTrans.Find("Text").GetComponent<Text>().enabled = isEnabled;
			cookTrans.GetComponent<Image>().enabled = isEnabled;
			stickTrans.GetComponent<Image>().enabled = isEnabled;

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
        maincanvas = gameController.canvasclone;
        cookButtonText = maincanvas.transform.Find("Cook Text").GetComponent<Text>();
        stickButtonText = maincanvas.transform.Find("Stick Text").GetComponent<Text>();
		scoreText = maincanvas.transform.Find("Score Text").GetComponent<Text>();
		decayTrans = maincanvas.transform.Find("DecayButton");
		cookTrans = maincanvas.transform.Find("Cook Button");
		stickTrans = maincanvas.transform.Find("Stick Button");
		dayDeck = gameController.daydeckclone;
        dayDeckRend = dayDeck.GetComponent<SpriteRenderer>();

    }

}



