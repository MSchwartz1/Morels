using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class OnClickCardScript : MonoBehaviour
{

    // References to other game objects, scripts, variables
    GameObject daydeck;
    GameObject nightdeck;
    GameObject Controller;
    GameController gameController;
    DealScript dealScript;
    DealScript nightScript;
    float handSpacing;
    float forestSpacing;
    Vector3 posForest;
    Vector3 posHand;


    private void Start()
    {

        // Scripts and objects
        daydeck = GameObject.Find("DayDeck(Clone)");
        nightdeck = GameObject.Find("NightDeck(Clone)");
        Controller = GameObject.Find("Controller");
        gameController = Controller.GetComponent<GameController>();
        dealScript = daydeck.GetComponent<DealScript>();
        nightScript = nightdeck.GetComponent<DealScript>();

        // Get access to variables: hand spacing, forest spacing, posForest, and posHand
        handSpacing = gameController.handSpacing;
        forestSpacing = gameController.forestSpacing;
        posForest = gameController.posForest;
        posHand = gameController.posHand;

    }


    // When a card is clicked on...
    void OnMouseDown()
    {
        // Script must be enabled
        if (!enabled)
        {
            return;
        }
                 
        // Move card to hand if card is in Forest
        if (gameController.Forest.Contains(gameObject))
        {

            // Convert index of card in forest from left-to-right to right-to-left
            int cardIdx = -1 * (gameController.Forest.IndexOf(gameObject) - 8);

            // Cannot take a card's too far into the forest
            if (cardIdx - gameController.stickCount <= 2)
            {

                // If the card is a basket
                if (gameObject.name == "Basket")
                {

                    // Remove card from Forest
                    gameController.Forest.Remove(gameObject);

                    // Update basketCount and handLimit
                    gameController.basketCount += 1;
                    gameController.handLimit += 2;

                    Destroy(gameObject);

                    // Deal new card
                    dealScript.MakeCard("Forest");

                }

                // For any non-basket card, can only add to hand if 
                // cards in hand does not exceed the hand limit.


                // If the card is a moon
                else if (gameObject.name == "Moon")
                {

                    if (gameController.handLimit > gameController.Hand.Count)
                    {

                        // Remove card from Forest
                        gameController.Forest.Remove(gameObject);

                        // Make a night card and put it in Hand
                        nightScript.MakeCard("Hand");

                        // Destroy the moon game object
                        Destroy(gameObject);

                        // Deal new card
                        dealScript.MakeCard("Forest");
                    }

                }

                // Any other card:
                else
                {
                    if (gameController.handLimit > gameController.Hand.Count)
                    {

                        // Remove card from Forest
                        gameController.Forest.Remove(gameObject);

                        // Add the card to the Hand list
                        gameController.Hand.Add(gameObject);

                        // Deal new card
                        dealScript.MakeCard("Forest");

                    }

                }

                // Decrement stickCount if necessary
                if (cardIdx > 2)
                {
                    gameController.stickCount -= (cardIdx - 2);
                }
            
                // Reposition cards in Forest and Hand
                gameController.Reposition(gameController.Forest, forestSpacing, posForest);
                gameController.Reposition(gameController.Hand, handSpacing, posHand);

                // Finally, end turn
                gameController.turnended = true;
            }

        }

        // If card in hand is a moon card or basket, take appropriate action
        else if (gameController.Hand.Contains(gameObject))
        {

            if (gameObject.name == "Pan")
            {

                // Remove card from hand and destroy object
                gameController.Hand.Remove(gameObject);
                Destroy(gameObject);

                // Reposition hand
                gameController.Reposition(gameController.Hand, handSpacing, posHand);

                // Update panCount
                gameController.panCount += 1;

                // End turn
                gameController.turnended = true;
            }

            // This would only ever happen if you are dealt a moon card from the get-go
            else if (gameObject.name == "Moon")
            {

                // Add a night card to hand
                nightScript.MakeCard("Hand");

                // Switch positions of night card and moon card
                int idx = gameController.Hand.IndexOf(gameObject);
                GameObject temp = gameController.Hand[idx];
                gameController.Hand[idx] = gameController.Hand[gameController.Hand.Count - 1];
                gameController.Hand[gameController.Hand.Count - 1] = temp;

                // Remove moon card from hand and destroy
                gameController.Hand.Remove(gameObject);
                Destroy(gameObject);

                // Reposition Hand
                gameController.Reposition(gameController.Hand, handSpacing, posHand);
            }

            // This would only ever happen if you are dealt a basket card from the get-go
            else if (gameObject.name == "Basket")
            {

                // Increment basket count variable
                gameController.basketCount += 1;
                gameController.handLimit += 2;

                // Remove from hand and destroy the basket
                gameController.Hand.Remove(gameObject);
                Destroy(gameObject);

                // Reposition Hand and update text
                gameController.Reposition(gameController.Hand, handSpacing, posHand);
                gameController.SetHandText();

            }


			else if (gameObject.name == "Destroying Angel") {
				return;
			}

			// For any other card, add to Selected Cards list and magnify        
			else {

                // If that card is not already in the list
				if (!gameController.SelectedCards.Contains(gameObject)) {
					gameController.SelectedCards.Add(gameObject);
					gameObject.transform.localScale *= 1.2f;

				}

			}



        }


    }

}



