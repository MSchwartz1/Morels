using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CookScript : MonoBehaviour
{
	
	// Game objects and variables CookScript needs access to
	GameObject Controller;
	GameController gameController;
	PropertiesScript propertiesScript;
	float handSpacing;
	Vector3 posHand;

	// Main camera for use with RayCasting (seeing what's clicked)
	Camera MainCamera;
   
    // Flag to indicate cooking action
    public bool cooking = false;

    // Array of cards player selects for "cooking"
    List<GameObject> CardsToCook;


    // Initialize variables
	private void Start()
	{
		MainCamera = Camera.main;
		Controller = GameObject.Find("Controller");
		gameController = Controller.GetComponent<GameController>();
		handSpacing = gameController.handSpacing;
		posHand = gameController.posHand;

	}


    /*                     *                     */

   
    // Cooking switch
    void OnMouseDown()
    {
		// Script must be enabled
        if (!enabled)
        {
            return;
        }

		// If it is the player's turn, switches value of "cooking."
		cooking = !cooking;         
        
        // If button switched on -- to "cooking" -- wipe CardsToCook list.
        // If button switched off -- to "not cooking" -- score the cook and move cards appropriately.
        // Also, magnify button for effect.
		if (cooking) {

			CardsToCook = new List<GameObject>();
			transform.localScale = transform.localScale * 1.2f;
		}

		else {
             
			transform.localScale = transform.localScale / 1.2f;

            // Check that cooking action is valid
			if (ValidCook()) {

				// Score the cook (decrements pan count if no pan was used)
				ScoreCook();

                // Delete selected cards from hand
                foreach (GameObject card in CardsToCook)
                {
                    gameController.Hand.Remove(card);
                    Destroy(card);
                    
                }

				// Reposition remaining cards
                gameController.Reposition(gameController.Hand, handSpacing, posHand);

				gameController.SetHandText();

				// End turn
				gameController.turnended = true;
			}

            // If cook is invalid, reset size of cards.
			else {
				
				foreach (GameObject card in CardsToCook) {
					card.transform.localScale /= 1.2f;

				}
			}
			         
		}
      
    }


	/*                     *                     */


	private void Update()
	{
        // If player is cooking and left clicks
		if (cooking && Input.GetMouseButtonDown(0)) {

            // Variables used to determine what was clicked
			Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			// If an object with a collider component is hit (clicked)
			if (Physics.Raycast(ray, out hit)) {

                // If the object hit is card in hand, add card to CardsToCook array
				if (gameController.Hand.Contains(hit.collider.gameObject)) {

                    // But only if the card is not already in CardsToCook
					if (!CardsToCook.Contains(hit.collider.gameObject)) {
					    
						CardsToCook.Add(hit.collider.gameObject);

                        // And magnify card to show it was selected
						hit.collider.gameObject.transform.localScale *= 1.2f;

					}

				}


			}

		}

	}

	/*                     *                     */

	private void ScoreCook() {      
		/* Function to tally score of a cook */

		int cookpoints = 0;

		foreach (GameObject card in CardsToCook) {

			int cardPoints = card.GetComponent<PropertiesScript>().points;

            // Night cards are worth double the points
			if (card.tag == "Night") {
				cardPoints *= 2;
			}
            
			cookpoints += cardPoints;

		}

		gameController.score += cookpoints;
		Debug.Log("Score: " + gameController.score.ToString());

		// Decrement panCount variable if no pan was used
		bool containsPan = false;
		foreach (GameObject card in CardsToCook) {
			if (card.name == "Pan") {
				containsPan = true;
				break;
			}	
		}      
		if (!containsPan) {
			gameController.panCount -= 1;
		}

        // Update text
        gameController.SetCookButtonText();

	}

	/*                     *                     */
    
	private bool ValidCook() {
		/* Function to determine if a given cook-action follows the game rules.
		   Returns a boolean */ 

		// NOTE: Order of some of these rules is important. 

		// Construct new list containing names of all cards in CardsToCook
        // Also checks for "cookable" property - will return false immediately 
        // if a card contained in CardsToCook is not cookable.
		List<string> CardsToCookNames = new List<string>();
        foreach (GameObject card in CardsToCook)
        {
            // Check for cookable
			propertiesScript = card.GetComponent<PropertiesScript>();
			if (!propertiesScript.cookable) {
				return false;
			}

            // Add to list 
            CardsToCookNames.Add(card.name);

			// Night cards count twice
			if (card.tag == "Night") {
				CardsToCookNames.Add(card.name);            
			}
         
        }      

        // Cannot cook if no pans are available
        if (gameController.panCount == 0 && !CardsToCookNames.Contains("Pan"))
        {
			return false;         
        }

        // Cannot cook fewer than 4 cards if Butter is used
        // Cannot cook multiple Butter cards together
		if (CardsToCookNames.Contains("Butter")) {
			if (CardsToCookNames.Count < 4) {
				return false;
			}

			if (CardsToCookNames.Count(name => name == "Butter") > 1) {
				return false;
			}

			// Remove butter from list - makes later rule simpler.
			CardsToCookNames.Remove("Butter");

		}

        // Cannot cook fewer than 5 cards if Cider is used
        // Cannot cook multiple Cider cards together
        if (CardsToCookNames.Contains("Cider"))
        {
            if (CardsToCookNames.Count < 5)
            {
                return false;
            }

			if (CardsToCookNames.Count(name => name == "Cider") > 1){
				return false;
			}

			// Remove cider from list - makes later rule simpler.
			CardsToCookNames.Remove("Cider");

        }
        
        // Cannot cook with multiple pan cards
		if (CardsToCookNames.Contains("Pan")) {
			if (CardsToCookNames.Count(name => name == "Pan") > 1)
            {
                return false;
            }

			// Remove pan from list - makes next two rules simpler.
            CardsToCookNames.Remove("Pan");
		}

              
        // Cannot cook fewer than three cards (night cards counted twice)
        if (CardsToCookNames.Count < 3)
        {

            return false;

        }

        // Cannot cook cards of different varieties
		if (CardsToCookNames.Distinct().Skip(1).Any()){

			return false;

		}

		return true;

	}

}
