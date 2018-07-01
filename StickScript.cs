using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StickScript : MonoBehaviour {
    
	// Game objects and variables StickScript needs access to
    GameObject Controller;
    GameController gameController;
	PropertiesScript propertiesScript;
	float handSpacing;
    Vector3 posHand;

	// Flag to indicate trading action
	public bool trading = false;
       
    // Main camera for use with RayCasting (seeing what's clicked)
    Camera MainCamera;

	// Array of cards player selects for "trading"
    List<GameObject> CardsToTrade;

	// Initialize variables
    private void Start()
    {
        MainCamera = Camera.main;
        Controller = GameObject.Find("Controller");
        gameController = Controller.GetComponent<GameController>();
		handSpacing = gameController.handSpacing;
        posHand = gameController.posHand;
       
    }

	// Cooking switch
    void OnMouseDown()
    {

		// Script must be enabled
        if (!enabled)
        {
            return;
        }

        // If it is the player's turn, switches value of "cooking."
        if (gameController.turn)
        {
            trading = !trading;
        }

        // If button switched on -- to "cooking" -- wipe CardsToCook list.
        // If button switched off -- to "not cooking" -- score the cook and move cards appropriately.
        // Also, magnify button for effect.
        if (trading)
        {

            CardsToTrade = new List<GameObject>();
            transform.localScale = transform.localScale * 1.2f;
        }

        else
        {

            transform.localScale = transform.localScale / 1.2f;

            // Check that cooking action is valid
            if (ValidTrade())
            {

                // Add appropriate amount of sticks to player
                GiveSticks();
                
                // Delete selected cards from hand
                foreach (GameObject card in CardsToTrade)
                {
                    gameController.Hand.Remove(card);
                    Destroy(card);
                                   
                }
                
				// Reposition remaining cards
                gameController.Reposition(gameController.Hand, handSpacing, posHand);

                // Set text
                gameController.SetHandText();

				// End turn
				gameController.EndTurn();

            }

            // If cook is invalid, reset size of cards.
            else
            {

                foreach (GameObject card in CardsToTrade)
                {
                    card.transform.localScale /= 1.2f;

                }
            }

        }

    }


	/*                     *                     */


	private void Update()
    {
        // If player is cooking and left clicks
        if (trading && Input.GetMouseButtonDown(0))
        {

            // Variables used to determine what was clicked
            Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // If an object with a collider component is hit (clicked)
            if (Physics.Raycast(ray, out hit))
            {

                // If the object hit is card in hand, add card to CardsToCook array
                if (gameController.Hand.Contains(hit.collider.gameObject))
                {

                    // But only if the card is not already in CardsToCook
                    if (!CardsToTrade.Contains(hit.collider.gameObject))
                    {

                        CardsToTrade.Add(hit.collider.gameObject);

                        // And magnify card to show it was selected
                        hit.collider.gameObject.transform.localScale *= 1.2f;

                    }

                }


            }

        }

    }


	/*                     *                     */


	public void GiveSticks() {
        /* Function to determine how many sticks a given trade is worth, and to
        add those sticks to the players stickCount variable*/

		int sticksgained = 0;

		foreach (GameObject card in CardsToTrade)
        {

            int cardSticks = card.GetComponent<PropertiesScript>().sticks;

            // Night cards are worth double the points
            if (card.tag == "Night")
            {
                cardSticks *= 2;
            }

            sticksgained += cardSticks;

        }

        gameController.stickCount += sticksgained;

        // Update stick text      
        gameController.SetStickButtonText();

	}


	/*                     *                     */


	private bool ValidTrade() {
		/* Function to determine if a given trade-action follows the game rules.
           Returns a boolean */ 
        
		// Construct new list containing names of all cards in CardsToTrade
		// Also checks for "tradeable" property - will return false immediately 
        // if a card contained in cardstotrade is not tradeable.
        List<string> CardsToTradeNames = new List<string>();
        foreach (GameObject card in CardsToTrade)
        {

			// Check for tradeable
            propertiesScript = card.GetComponent<PropertiesScript>();
            if (!propertiesScript.tradeable)
            {
                return false;
            }

            // Add to list
            CardsToTradeNames.Add(card.name);

            // Night cards count twice
            if (card.tag == "Night")
            {
                CardsToTradeNames.Add(card.name);
            }

        }

		// Cannot trade fewer than 2 cards for sticks (night cards counted twice)
		if (CardsToTradeNames.Count < 2) {
			return false;
		}
        

		// Cannot trade cards of different varieties
        if (CardsToTradeNames.Distinct().Skip(1).Any())
        {         
            return false;         
        }

		return true;

	}


}