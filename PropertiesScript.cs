using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertiesScript : MonoBehaviour {

	public int points;
	public int sticks;
	public bool cookable;
	public bool tradeable;

	// This function is used to assign a card's point-value, stick number, and more.
	public void SetProperties(string cardName)
    {
        if (cardName.Contains("chanterelle"))
        {
			points = 4;
			sticks = 2;
			name = "Chanterelle";
			cookable = true;
			tradeable = true;
        }
        
		else if (cardName.Contains("fairyring"))
        {
            points = 3;
			sticks = 2;
			name = "Fairy Ring";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("henofthewoods"))
        {
            points = 3;
			sticks = 1;
			name = "Hen of the Woods";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("honeyfungus"))
        {
            points = 1;
			sticks = 1;
			name = "Honey Fungus";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("lawyerswig"))
        {
            points = 2;
			sticks = 1;
			name = "Lawyers Wig";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("porcini"))
        {
            points = 3;
			sticks = 3;
			name = "Porcini";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("shitake"))
        {
            points = 2;
			sticks = 2;
			name = "Shitake";
			cookable = true;
            tradeable = true;
        }

		else if (cardName.Contains("morels"))
		{
			points = 6;
			sticks = 4;
			name = "Morels";
			cookable = true;
            tradeable = true;
		}

		else if (cardName.Contains("treeear"))
		{
			points = 1;
			sticks = 2;
			name = "Tree Ear";
			cookable = true;
            tradeable = true;
		}

		else if (cardName.Contains("basket")){
			points = 0;
			sticks = 0;
			name = "Basket";
			cookable = false;
            tradeable = false;
		}

		else if (cardName.Contains("moon"))
        {
            points = 0;
            sticks = 0;
            name = "Moon";
			cookable = false;
            tradeable = false;
        }

		else if (cardName.Contains("destroyingangel"))
        {
            points = 0;
            sticks = 0;
            name = "Destroying Angel";
			cookable = false;
            tradeable = false;
        }

		else if (cardName.Contains("pan"))
        {
            points = 0;
            sticks = 0;
			name = "Pan";
			cookable = true;
            tradeable = false;
        }

		else if (cardName.Contains("cider"))
        {
            points = 5;
            sticks = 0;
            name = "Cider";
			cookable = true;
            tradeable = false;
        }
        
		else if (cardName.Contains("butter"))
        {
            points = 3;
            sticks = 0;
            name = "Butter";
			cookable = true;
            tradeable = false;
        }

    }

}
