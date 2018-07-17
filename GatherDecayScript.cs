using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherDecayScript : MonoBehaviour {

    Canvas maincanvas;
    SceneChangeScript sceneChangeScript;
    GameObject controller;
    GameController gameController;

    private void Awake()
    {

        controller = GameObject.Find("Controller");
        gameController = controller.GetComponent<GameController>();
        maincanvas = gameController.canvasclone;
        sceneChangeScript = maincanvas.transform.Find("DecayButton").GetComponent<SceneChangeScript>();
        
    }
    
	public void OnClick() {

		List<GameObject> decayCards = new List<GameObject>();
		List<GameObject> decayBaskets = new List<GameObject>();

        // Separate the baskets in the decay from the non-baskets
		foreach (GameObject card in gameController.Decay) {

			if (card.name == "Basket") {
				decayBaskets.Add(card);
			}

			else {
				decayCards.Add(card);
			}


		}

        // Is the decay populated?
		if (gameController.Decay.Count == 0) {
			return;
		}

        // Check that the player's hand has space for the decay
        // Add to hand if possible
		if (decayCards.Count + gameController.Hand.Count <= gameController.handLimit + 2*decayBaskets.Count) {

			gameController.handLimit += 2 * decayBaskets.Count;

			foreach (GameObject card in decayBaskets) {
				gameController.Decay.Remove(card);
				Destroy(card);
			}

			foreach (GameObject card in decayCards) {
				gameController.Decay.Remove(card);
				gameController.Hand.Add(card);
			}

			// Housekeeping
			sceneChangeScript.ChangeScene("Main");
			gameController.Reposition(gameController.Hand, gameController.handSpacing, gameController.posHand);

			gameController.turnended = true;
		}

	}

}
