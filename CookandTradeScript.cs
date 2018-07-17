using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CookandTradeScript : MonoBehaviour
{

	// Game objects and variables CookScript needs access to
	GameObject Controller;
	GameController gameController;
	PropertiesScript propertiesScript;
	float handSpacing;
	Vector3 posHand;

	// Initialize variables
	private void Start()
	{

		Controller = GameObject.Find("Controller");
		gameController = Controller.GetComponent<GameController>();
		handSpacing = gameController.handSpacing;
		posHand = gameController.posHand;

	}


	/*                     *                     */


	// Cooking switch
	public void OnClickButton(string buttonName)
	{

		// Script must be enabled
		if (!enabled)
		{
			return;
		}

		// Cooking
		if (buttonName == "Cook Button")
		{

			// Check that cooking action is valid
			if (ValidCook())
			{

				// Score the cook (decrements pan count if no pan was used)
				ScoreCook();

				// Delete selected cards from hand
				foreach (GameObject card in gameController.SelectedCards)
				{
					gameController.Hand.Remove(card);
					Destroy(card);
                    
				}

				// Turn ending action
				gameController.turnended = true;

			}

			// If cook or trade action is invalid, reset size of cards.
			else
			{

				foreach (GameObject card in gameController.SelectedCards)
				{
					card.transform.localScale /= 1.2f;

				}
			}

		}

		// Trading
		else if (buttonName == "Stick Button")
		{

			// Check that cooking action is valid
			if (ValidTrade())
			{

				// Add appropriate amount of sticks to player
				GiveSticks();

				// Delete selected cards from hand
				foreach (GameObject card in gameController.SelectedCards)
				{
					gameController.Hand.Remove(card);
					Destroy(card);

				}


				// Turn ending action
				gameController.turnended = true;

			}


			// If trade action is invalid, reset size of cards.
			else
			{

				foreach (GameObject card in gameController.SelectedCards)
				{
					card.transform.localScale /= 1.2f;

				}
			}

		}


		// Reposition remaining cards
		gameController.Reposition(gameController.Hand, handSpacing, posHand);

		// Clear selected cards list
		gameController.SelectedCards.Clear();

	}

	/*                     *                     */

	private void ScoreCook()
	{
		/* Function to tally score of a cook */

		int cookpoints = 0;

		foreach (GameObject card in gameController.SelectedCards)
		{

			int cardPoints = card.GetComponent<PropertiesScript>().points;

			// Night cards are worth double the points
			if (card.tag == "Night")
			{
				cardPoints *= 2;
			}

			cookpoints += cardPoints;

		}

		if (gameController.turn)
		{
			gameController.score1 += cookpoints;
		}
		else
		{
			gameController.score2 += cookpoints;
		}

		// Decrement panCount variable if no pan was used
		bool containsPan = false;
		foreach (GameObject card in gameController.SelectedCards)
		{
			if (card.name == "Pan")
			{
				containsPan = true;
				break;
			}
		}
		if (!containsPan)
		{
			gameController.panCount -= 1;
		}


	}

	/*                     *                     */

	private bool ValidCook()
	{
		/* Function to determine if a given cook-action follows the game rules.
           Returns a boolean */

		// NOTE: Order of some of these rules is important. 

		// Construct new list containing names of all cards in SelectedCards
		// Also checks for "cookable" property - will return false immediately 
		// if a card contained in CardsToCook is not cookable.
		List<string> CardsToCookNames = new List<string>();
		foreach (GameObject card in gameController.SelectedCards)
		{
			// Check for cookable
			propertiesScript = card.GetComponent<PropertiesScript>();
			if (!propertiesScript.cookable)
			{
				return false;
			}

			// Add to list 
			CardsToCookNames.Add(card.name);

			// Night cards count twice
			if (card.tag == "Night")
			{
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
		if (CardsToCookNames.Contains("Butter"))
		{
			if (CardsToCookNames.Count < 4)
			{
				return false;
			}

			if (CardsToCookNames.Count(name => name == "Butter") > 1)
			{
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

			if (CardsToCookNames.Count(name => name == "Cider") > 1)
			{
				return false;
			}

			// Remove cider from list - makes later rule simpler.
			CardsToCookNames.Remove("Cider");

		}

		// Cannot cook with multiple pan cards
		if (CardsToCookNames.Contains("Pan"))
		{
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
		if (CardsToCookNames.Distinct().Skip(1).Any())
		{

			return false;

		}

		return true;

	}

	public void GiveSticks()
	{
		/* Function to determine how many sticks a given trade is worth, and to
        add those sticks to the players stickCount variable*/

		int sticksgained = 0;

		foreach (GameObject card in gameController.SelectedCards)
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


	}

	private bool ValidTrade()
	{
		/* Function to determine if a given trade-action follows the game rules.
           Returns a boolean */

		// Construct new list containing names of all cards in SelectedCards
		// Also checks for "tradeable" property - will return false immediately 
		// if a card contained in cardstotrade is not tradeable.
		List<string> CardsToTradeNames = new List<string>();
		foreach (GameObject card in gameController.SelectedCards)
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
		if (CardsToTradeNames.Count < 2)
		{
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
