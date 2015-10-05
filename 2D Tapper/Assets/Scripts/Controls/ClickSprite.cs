using UnityEngine;
using System.Collections;

public class ClickSprite : MonoBehaviour {

	public struct RaycastHitResult {
		public RaycastHit2D ray_hit;
		public bool nothing_clicked;
	}
	
	public struct Clicked {
		public GameObject clicked_object;
		public bool isClicked;
	}
	
	private GameObject clickedObject;
	private RaycastHitResult front_hit;
	private Clicked clickable;
	private bool showDebug = false;
	private SpriteRenderer spriteRenderer;
	
	public Camera[] cameras;
	private Vector3[] click_pos;

	public Clicked Click(Vector2 pos) {
		front_hit = GetFrontHit(pos);
        Clicked obj;
        if (!front_hit.nothing_clicked)
        {
            obj.clicked_object = front_hit.ray_hit.collider.gameObject;
            obj.isClicked = true;
            return obj;
        }
        else
        {
            obj.clicked_object = null;
            obj.isClicked = false;
            return obj;
        }
	}
	
	private RaycastHitResult GetFrontHit(Vector2 pos) {
		RaycastHitResult result = new RaycastHitResult();
		
		Vector3 screenPos = new Vector3(pos.x, pos.y, 0f);
		click_pos = new Vector3[cameras.Length];
		RaycastHit2D[][] hits = new RaycastHit2D[cameras.Length][];
		int hit_number = 0;
		for (int i = 0; i < cameras.Length; i++)
		{
			click_pos[i] = cameras[i].ScreenToWorldPoint(screenPos);
			hits[i] = Physics2D.LinecastAll(click_pos[i], click_pos[i]);
			hit_number += hits[i].Length;
		}
		
		if (hit_number <= 0)
		{
			if (showDebug) Debug.Log("Nothing clicked");
			result.nothing_clicked = true;
			return result;
		}
		
		bool hit = false;
		
		int[][] curSortingLayers = new int[hits.Length][];
		int[][] curSortingOrders = new int[hits.Length][];
		int index_x = 0, index_y = 0;
		int topSortingLayer = 0;
		int topSortingOrder = 0;
		
		for (int i = 0; i < hits.Length; i++)
		{
			curSortingLayers[i] = new int[hits[i].Length];
			curSortingOrders[i] = new int[hits[i].Length];
			for (int j = 0; j < hits[i].Length; j++)
			{
				spriteRenderer = hits[i][j].collider.gameObject.GetComponent<SpriteRenderer>();
				if (spriteRenderer != null)
				{
					hit = true;
					curSortingLayers[i][j] = spriteRenderer.sortingLayerID;
					curSortingOrders[i][j] = spriteRenderer.sortingOrder;
					if (curSortingLayers[i][j] >= topSortingLayer)
						topSortingLayer = curSortingLayers[i][j];
				}
			}
		}
		
		if (!hit)
		{
			if (showDebug) Debug.Log("Nothing clicked");
			result.nothing_clicked = true;
			return result;
		}
	
		for (int i = 0; i < hits.Length; i++)
		{
			for (int j = 0; j < hits[i].Length; j++)
			{
				if (curSortingOrders[i][j] >= topSortingOrder && curSortingLayers[i][j] >= topSortingLayer)
				{
					topSortingOrder = curSortingOrders[i][j];
					index_x = i;
					index_y = j;
				}
			}
		}
		
		result.ray_hit = hits[index_x][index_y];
		result.nothing_clicked = false;
		return result;
	}
}
