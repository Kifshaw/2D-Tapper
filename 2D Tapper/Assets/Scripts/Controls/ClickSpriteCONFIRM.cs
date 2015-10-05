using UnityEngine;
using System.Collections;

public class ClickSpriteCONFIRM : MonoBehaviour {

	public struct GetFrontmostRaycastHitResult
	{
		public RaycastHit2D rayCastHit2D;
		public bool nothingClicked;
	}
	
	public struct Clicked
	{
		public GameObject clicked_object; //null if nothing clicked;
		public bool isClicked;
	}
	
	private GameObject leftClickedObject;
	private GetFrontmostRaycastHitResult frontmostRaycastHit;
	private Clicked clickable;
	private bool showDebug = false;
	private SpriteRenderer spriteRenderer;
	
	public Clicked Click(Vector2 touch) 
	{
		// If the left mouse button is clicked anywhere...
		//if (Input.GetMouseButtonDown(0))
		//{
			// frontmostRaycastHit stores information about the RaycastHit2D that is returned by GetFrontmostRaycastHit()
            frontmostRaycastHit = GetFrontmostRaycastHit(touch);
            if (frontmostRaycastHit.rayCastHit2D.collider != null) {
			    clickable.clicked_object = frontmostRaycastHit.rayCastHit2D.collider.gameObject;
			    clickable.isClicked = !frontmostRaycastHit.nothingClicked;
			}
			else
			clickable.isClicked = false;
			return clickable;
		//}
	}
	
	GetFrontmostRaycastHitResult GetFrontmostRaycastHit(Vector2 touch)
	{
		GetFrontmostRaycastHitResult result = new GetFrontmostRaycastHitResult();
		
		Vector3 screenPosition = new Vector3(touch.x,touch.y,0f);
		
		// Store the point where the user has clicked as a Vector3.
        Camera mainCam = Camera.main;
		//Vector3 clickPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        Vector3 clickPosition = mainCam.ScreenToWorldPoint(screenPosition);
		//Camera cam2 = GameObject.Find("Camera").GetComponent<Camera>();
		//Vector3 clickPosition2 = cam2.ScreenToWorldPoint(screenPosition);
        LayerMask removedLayer = ~LayerMask.NameToLayer("ItemDisplayFrame");
		
		// Retrieve all raycast hits from the click position and store them in an array called "hits".
		RaycastHit2D[] hits = Physics2D.LinecastAll (clickPosition, clickPosition/*, removedLayer*/);
		//RaycastHit2D[] hits2 = Physics2D.LinecastAll(clickPosition2, clickPosition2/*, removedLayer*/);


		if (hits.Length/*+hits2.Length*/ != 0)
		{
			// A variable that will store the frontmost sorting layer that contains an object that has been clicked on as an int.
			int topSortingLayer = 0;
			
			int indexOfTopSortingLayer;
			
			// An array that stores the IDs of all the sorting layers that contain a sprite in the path of the linecast.
			int[] sortingLayerIDArray = new int[hits.Length];//+hits2.Length];
			
			// An array that stores the sorting orders of each sprite that has been hit by the linecast
			int[] sortingOrderArray = new int[hits.Length];//+hits2.Length];
			
			// An array that stores the sorting order number of the frontmost sprite that has been clicked.
			int topSortingOrder = 0;
			
			// A variable that will store the index in the sortingOrderArray where topSortingOrder is. This index used with the hits array will give us our frontmost clicked sprite.
			int indexOfTopSortingOrder = 0;
			
			bool hit = false;
			
			// Loop through the array of raycast hits...
			for (int i = 0; i < hits.Length;/*+hits2.Length;*/ i++)
			{
				// Get the SpriteRenderer from each game object under the click.
				if (i < hits.Length)
				{
					spriteRenderer = hits[i].collider.gameObject.GetComponent<SpriteRenderer>();
				}
				else
				{
					//spriteRenderer = hits2[i-hits.Length].collider.gameObject.GetComponent<SpriteRenderer>();
				}
				
				if (spriteRenderer != null)
				{
					sortingLayerIDArray[i] = spriteRenderer.sortingLayerID;
					sortingOrderArray[i] = spriteRenderer.sortingOrder;
					hit = true;
				}
			}
			
			if (!hit)
			{
				if (showDebug)Debug.Log("Nothing clicked.");
				result.nothingClicked = true;
				return result;
			}
			
			// Loop through the array of sprite sorting layer IDs...
			for (int j = 0; j < sortingLayerIDArray.Length; j++)
			{
				// If the sortingLayerID is higher that the topSortingLayer...
				if (sortingLayerIDArray[j] >= topSortingLayer)
				{
					topSortingLayer = sortingLayerIDArray[j];
					indexOfTopSortingLayer = j;
				}
			}
			
			// Loop through the array of sprite sorting orders...
			for (int k = 0; k < sortingOrderArray.Length; k++)
			{
				// If the sorting order of the sprite is higher than topSortingOrder AND the sprite is on the top sorting layer...
				if (sortingOrderArray[k] >= topSortingOrder && sortingLayerIDArray[k] == topSortingLayer)
				{
					topSortingOrder = sortingOrderArray[k];
					indexOfTopSortingOrder = k; //Set index of sprite that is on top layer
				}
				else
				{
					// Do nothing and continue loop.
				}
			}
			
			// How many sprites with colliders attached are underneath the click?
			//if (showDebug) Debug.Log("How many sprites have been clicked on: " + (hits.Length+hits2.Length).ToString());
			// Which is the sorting layer of the frontmost clicked sprite?
			if (showDebug)Debug.Log("Frontmost sorting layer ID: "+ topSortingLayer);
			// Which is the order in that sorting layer of the frontmost clicked sprite?
			if (showDebug)Debug.Log("Frontmost order in layer: "+ topSortingOrder);
			
			// The indexOfTopSortingOrder will also be the index of the frontmost raycast hit in the array "hits". 
			/*if (indexOfTopSortingOrder >= hits.Length)
			{
				result.rayCastHit2D = hits2[indexOfTopSortingOrder-hits.Length];
			}
			else
			{*/
				result.rayCastHit2D = hits[indexOfTopSortingOrder];
			//}
			result.nothingClicked = false;
			return result;
		}
		else // If the hits array has a length of 0 then nothing has been clicked...
		{
			if (showDebug)Debug.Log("Nothing clicked.");
			result.nothingClicked = true;
			return result;
		}
	}
}