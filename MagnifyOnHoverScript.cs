using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyOnHoverScript : MonoBehaviour {

	public float magCoeff;

    // Magnify card object when hovering over with mouse
	private void OnMouseEnter(){

		transform.localScale = transform.localScale * magCoeff;

        // This line moves card closer to camera to avoid clipping
		transform.position = new Vector3(transform.position.x, transform.position.y, -3);
	}
    
    // Unmagnify card object when no longer hovering over with mouse
	private void OnMouseExit()
	{

		transform.localScale = transform.localScale / magCoeff;

		// This line moves card back to original transform position
		transform.position = new Vector3(transform.position.x, transform.position.y, -1);
	}
       
   
}
