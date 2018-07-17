using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	/* This scipt controls and manages the major aspects of game behavior:
	 * shuffling, dealing, repositioning cards, etc., in addition to initializing
	 * useful data structures for the game (lists, booleans, etc). */
    
    
	// Game objects and scripts that GameController will need to access
    // Assigned in editor
	public GameObject daydeck; public GameObject daydeckclone;
	public GameObject nightdeck;
	public Canvas maintextcanvas;
	public Canvas canvasclone;
	Text cookButtonText;
	Text stickButtonText;
	Text scoreText;
	Text cardDisplayText;
	Text handText;
	DealScript dealScript;
	DealScript nightScript;
	SceneChangeScript sceneChangeScript;

    
	// Flags and other variables
	public bool turn;                   // Controls turn
	public bool turnended;
	static bool created = false;        // Tracks whether Controller has been created   
	public Vector3 posForest;           // Positions of forest, hand, and decay
	public Vector3 posHand;
	public Vector3 posDecay;
	public float forestSpacing;         // Spacing of cards in forest, hand, and decay
	public float handSpacing;
	public float decaySpacing;
	public int basketCount;             // Counts number of baskets played
	public int panCount;                // Counts number of pans played (starts at 1)
	public int handLimit;               // Tracks cards allowed in hand
	public int stickCount;              // Counts number of sticks in player's possession

	// Lists (Hand, Forest, Decay, Selected Cards)
	public List<GameObject> Hand;
	public List<GameObject> Forest;
	public List<GameObject> Decay;
	public List<GameObject> SelectedCards;

	// Player-specific variables (never accessed outside of GameController - only used as storage)
	public List<GameObject> Hand1;
	public List<GameObject> Hand2;
	public int score1;
	public int score2;
	public int basketCount1;
	public int basketCount2;
	public int panCount1;
	public int panCount2;   
	public int handLimit1;   
	public int handLimit2;
	public int stickCount1;
	public int stickCount2;


	// Use this for initialization
	void Awake()
	{

		// Only call this part of Awake once (even through scene changes)
		if (!created)
		{
			created = true;

			// Don't destroy object on scene changes         
			DontDestroyOnLoad(this.gameObject);
            
			// Initialize card lists
			Hand1 = new List<GameObject>(); Hand2 = new List<GameObject>();
			Forest = new List<GameObject>(); Decay = new List<GameObject>();
			SelectedCards = new List<GameObject>();
			Hand1.Clear(); Hand2.Clear(); Forest.Clear(); Decay.Clear(); SelectedCards.Clear();
            
			// Initialize variables
			turn = true;
			score1 = 0; score2 = 0;
			basketCount1 = 0; basketCount2 = 0;
			panCount1 = 1; panCount2 = 1;
			handLimit1 = 8; handLimit2 = 8;
			stickCount1 = 0; stickCount2 = 0;

			// Set up variables such that Player 1 starts by default
			SwitchPlayer("Player1");

			// Instantiate a day-deck, shuffle the deck and deal the initial hand.
			daydeckclone = Instantiate(daydeck, daydeck.transform.position, Quaternion.identity);
			dealScript = daydeckclone.GetComponent<DealScript>();         
            dealScript.Shuffle();
            dealScript.Deal();

			// Instantiate a night-deck, shuffle the deck but do NOT deal.
			GameObject nightdeckclone;
			nightdeckclone = Instantiate(nightdeck, nightdeck.transform.position, Quaternion.identity);
			nightScript = nightdeckclone.GetComponent<DealScript>();
			nightScript.Shuffle();

			// Instantiate buttons and text
			canvasclone = Instantiate(maintextcanvas, maintextcanvas.transform.position, Quaternion.identity);         
			cookButtonText = canvasclone.transform.Find("Cook Text").GetComponent<Text>();
			stickButtonText = canvasclone.transform.Find("Stick Text").GetComponent<Text>();
			scoreText = canvasclone.transform.Find("Score Text").GetComponent<Text>();
			cardDisplayText = canvasclone.transform.Find("Card Name").GetComponent<Text>();
			handText = canvasclone.transform.Find("Hand Text").GetComponent<Text>();
			SetCookButtonText();
			SetStickButtonText();
			SetHandText();
			SetCardDisplayText("",0,0);

                     
		}


	}

	private void FixedUpdate()
	{
		if (turnended == true) {
			turnended = false;
			EndTurn();
		}
	}

	public void Reposition(List<GameObject> cardlist,float Spacing,Vector3 Anchor) {
		/* Takes a list of cards (game objects), a positional anchor, and an 
		 * associated spacing to reposition the card game objects. */

		foreach (GameObject card in cardlist)
		{
			card.transform.position = Anchor;
			Anchor.x += Spacing;         
            
         }

	}

	public void EndTurn() {
		/* Initiates sequence of events following end of turn.
           Called whenever a turn-ending action is performed. */

        // Update texts
		SetHandText(); SetCookButtonText(); SetStickButtonText(); SetScoreText();

        // If decay has more than four cards in it, discard.
		if (Decay.Count == 4) {
			List<GameObject> DecayCopy = new List<GameObject>(Decay);
			foreach (GameObject card in DecayCopy) 
			{
				Decay.Remove(card);
				Destroy(card);
			}
		}

		// Move a card to the decay and make sure it's inactive
        GameObject decayCard = Forest[Forest.Count - 1];
        Decay.Add(decayCard);
		decayCard.SetActive(false);

        // Remove card from forest
        Forest.Remove(decayCard);

        // Deal a new card
        dealScript.MakeCard("Forest");

        // Reposition cards in forest and decay
        Reposition(Forest, forestSpacing, posForest);
        Reposition(Decay, decaySpacing, posDecay);
        
		// Get access to SceneChangeScript
		// Make objects un-interactable
		sceneChangeScript = canvasclone.transform.Find("DecayButton").GetComponent<SceneChangeScript>();
		sceneChangeScript.GetObjects();
		sceneChangeScript.ToggleInteract(false, true, true, false);
		//canvasclone.transform.Find("DecayButton").GetComponent<Button>().enabled = true;    // Turn back on button

        // Activate the turn button
        canvasclone.transform.Find("TurnButton").gameObject.SetActive(true);
		// Set text
		canvasclone.transform.Find("TurnButton").transform.Find("Text").GetComponent<Text>().text = "End Turn";

      
	}

	public void ChangeTurn() {
        /* Takes care of all procedures that follow a turn change */  
              
        // Switch turn variable
		turn = !turn;      
      
		if (turn) {

			// Changing TO player 1's turn

			Save("Player2");
			SwitchPlayer("Player1");
         
		}

		else {

			// Changing TO player 2's turn         
   
			Save("Player1");
            SwitchPlayer("Player2");

		}
        
		SetHandText(); SetCookButtonText(); SetStickButtonText(); SetScoreText();

	}
   
    /* Functions below update variables in this script */

	public void SetCookButtonText() 
	{
		cookButtonText.text = "Cook! x" + panCount.ToString();
	}

    public void SetHandText()
    {
		handText.text = "Hand: " + Hand.Count.ToString() + " / " + handLimit;
    }

	public void SetStickButtonText() 
	{
		stickButtonText.text = "Trade! x" + stickCount.ToString();
	}

    public void SetScoreText()
	{
		scoreText.text = "Player One: " + score1.ToString() + "\n" + "Player Two: " + score2.ToString();
	}

	public void SetCardDisplayText(string cardname, int cookpoints, int sticks) {

		if (cardname != "") {
			cardDisplayText.text = cardname + "\n" + "Cook Points: " + cookpoints.ToString() 
				+ "\n" + "Sticks: " + sticks.ToString();         
		}

		else {
			cardDisplayText.text = "";
		}
	}
    
    /* Functions below used in switching turns */

	void Save(string player) {
        /* Saves data before switching players */

		if (player == "Player1") {

			Hand1 = Hand; basketCount1 = basketCount; panCount1 = panCount;
            handLimit1 = handLimit; stickCount1 = stickCount;
			         
		}

		else if (player == "Player2") {

			Hand2 = Hand; basketCount2 = basketCount; panCount2 = panCount;
			handLimit2 = handLimit; stickCount2 = stickCount;

		}

	}

	void SwitchPlayer(string player) {
        /* Switches active hand / corresponding variables */

		if (player == "Player1") {

			Hand = Hand1; basketCount = basketCount1; panCount = panCount1;
            handLimit = handLimit1; stickCount = stickCount1;

		}

		else if (player == "Player2") {

			Hand = Hand2; basketCount = basketCount2; panCount = panCount2;
            handLimit = handLimit2; stickCount = stickCount2;

		}

	}

}
