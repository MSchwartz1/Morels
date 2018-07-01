using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

	// Tracks whether game object has been created
	bool created = false;

	void Awake()
	{      
		// Only read code in awake once (even through scene changes)
		if (!created)
		{
			created = true;
			DontDestroyOnLoad(this.gameObject);

		}

	}
}
