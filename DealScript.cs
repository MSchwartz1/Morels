using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealScript : MonoBehaviour
{

    // Variables original to DealScript
	int numCards = 0;                            // # cards dealt so far
	List<int> cardlist;                          // List of numbers (0,...,83) representing cards   
	public Sprite[] faces;                       // List of card faces
	int numDeal = 8;                             // Number of cards to deal to forest
    int numHand = 3;                             // Number of cards to deal to hand
   

    // References to other objects / scripts
	public GameObject card;
	GameObject Controller;
	GameController gameController;   
	Vector3 posForest;
	Vector3 posHand;
	float forestSpacing;
	float handSpacing;   
    
    // Use to initialize variables
	private void Awake()
	{

		// Necessary variables from Game Controller
		Controller = GameObject.Find("Controller");
		gameController = Controller.GetComponent<GameController>();      
        posForest = gameController.posForest;
        posHand = gameController.posHand;
        forestSpacing = gameController.forestSpacing;
        handSpacing = gameController.handSpacing;


	}


	/* Code below creates shuffled deck */

	public void Shuffle() {

        // Cards list
        cardlist = new List<int>();

        
		// Creates a list that looks like (0,1,2,....83)
		for (int i = 0; i < faces.Length; i++) {
			cardlist.Add(i);
		}

              
		// Randomize order of list using Fisher-Yates algorithm
		int n = cardlist.Count;
		while (n > 1) {
			n--;
			int k = Random.Range(0,n+1);
			int temp = cardlist[k];
			cardlist[k] = cardlist[n];
			cardlist[n] = temp;
		}

	}
    

	/* Code below is used to make a card */   
	// Instantiate a card object and assign it a random face
	public void MakeCard(string location)
	{
		GameObject cardInstance;
		PropertiesScript propertiesScript;
		SpriteRenderer spriteRenderer;

		// Only if the number of cards dealt so far does not exceed the number in the deck
		if (numCards < faces.Length) {
            
			// Instantiate at deck position
			cardInstance = Instantiate(card, transform.position, transform.rotation);
            
            // Set sprite to random face
            // Note: getcomponentinchildren is a depth-first search
            spriteRenderer = cardInstance.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = faces[cardlist[numCards]];

            // Set card's point value
            propertiesScript = cardInstance.GetComponent<PropertiesScript>();
            propertiesScript.SetProperties(spriteRenderer.sprite.name);
            
			// Set tag: day, night, or special
			if (spriteRenderer.sprite.name.Contains(" n")) {
				cardInstance.tag = "Night";            
			}

			else {
				cardInstance.tag = "Day";
			}
            

			// Add cards to appropriate lists
			if (location == "Hand")
            {
				gameController.Hand.Add(cardInstance);

            }
            
			else if (location == "Forest") {
				gameController.Forest.Insert(0, cardInstance);
            }

			// Update variable representing number of cards dealt
            numCards++;

		}


	}

	/* Code below is used to deal cards */

    // Deals the initial 8 cards and the 3 cards in the player's hand
	public void Deal() {
      
        // Add eight cards to forest
        for (int i = 0; i < numDeal; i++) 
		{
			
            MakeCard("Forest");

        }
      
		// Add three cards to hand1
		for (int j = 0; j < numHand; j++)
		{

			MakeCard("Hand");
            
		}

		// Position cards appropriately
        gameController.Reposition(gameController.Hand, handSpacing, posHand);

		// Switch hands to hand 2
		gameController.Hand1 = gameController.Hand;     // Saving data
		gameController.Hand = gameController.Hand2;     // Switching hands

		// Add three cards to hand2
        for (int j = 0; j < numHand; j++)
        {

            MakeCard("Hand");

        }

             
		// Position cards appropriately
        gameController.Reposition(gameController.Hand, handSpacing, posHand);
        gameController.Reposition(gameController.Forest, forestSpacing, posForest);

		// Deactivate hand2
        foreach (GameObject handcard in gameController.Hand)
        {
            handcard.SetActive(false);
        }

		// Switch hands back to hand 1
        gameController.Hand2 = gameController.Hand;     // Saving data
        gameController.Hand = gameController.Hand1;     // Switching hands


    }

}