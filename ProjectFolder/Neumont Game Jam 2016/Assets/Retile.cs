using UnityEngine;
using System.Collections.Generic;
using Tiled2Unity;

public class Retile : MonoBehaviour {
	
	public TiledMap map;
	public GameObject ground = null, surface = null, rightEdge = null, leftEdge = null;
	public Vector2 startingDencity = new Vector2(6,10);
	
	public List<GameObject> tiles = new List<GameObject>();

	// Use this for initialization
	void Start () {
		map = gameObject.GetComponent<TiledMap>();
		BoxSet(false);
		reTile();
		deMesh(transform);
		BoxSet(true);
	}
	
	private void BoxSet(bool set){
		BoxCollider2D[] boxs = FindObjectsOfType(typeof(BoxCollider2D)) as BoxCollider2D[];
		for(int i = 0; i < boxs.Length; i++){
			boxs[i].enabled = set;
		}
	}
	
	private void deMesh(Transform trans){
		for(int i = 0; i < trans.childCount; i++){
			GameObject go = trans.GetChild(i).gameObject;
			if(go.transform.childCount > 0) deMesh(go.transform);
			if(go.GetComponent<MeshRenderer>() || go.GetComponent<PolygonCollider2D>()) GameObject.Destroy(go);
		}
	}
	
	private void reTile(){
		Vector3 tileTop = new Vector3();
		Rect rec = map.GetMapRectInPixelsScaled();
		float Xp = map.GetMapWidthInPixelsScaled()/map.NumTilesWide;
		float Yp = map.GetMapHeightInPixelsScaled()/map.NumTilesHigh;
		
		for(int x = 0; x < map.NumTilesWide; x++){
			for(int y = 0; y > -map.NumTilesHigh; y--){
				tileTop = transform.position + new Vector3(x * Xp + Xp/2, y * Yp, 0);
				Vector3 tilePlace = tileTop - transform.up * Yp/2;
				RaycastHit2D hit = Physics2D.Raycast(tileTop - .1f * Vector3.up, -Vector3.up, Yp/2);
				if(hit){
					if(hit.collider.gameObject.GetComponent<RetileWith>()){
						RetileWith retw = hit.collider.gameObject.GetComponent<RetileWith>();
						surface = retw.surface;
						rightEdge = retw.rightEdge;
						leftEdge = retw.leftEdge;
						ground = retw.ground;
						startingDencity = retw.startingDencity;
					}
					
					if(surface != null && !Physics2D.Raycast(tileTop + .1f * Vector3.up, Vector3.up, Yp/2)){
						AddObj(GameObject.Instantiate(surface, tilePlace, Quaternion.identity) as GameObject);
					}else if(rightEdge != null && !Physics2D.Raycast(new Vector3((x+1) * Xp + .01f, y * Yp + Yp/2, 0), Vector3.right, Xp/2)){
						AddObj(GameObject.Instantiate(rightEdge, tilePlace, Quaternion.identity) as GameObject);
					}else if(leftEdge != null && !Physics2D.Raycast(new Vector3(x * Xp - .01f, y * Yp + Yp/2, 0), -Vector3.right, Xp/2)){
						AddObj(GameObject.Instantiate(leftEdge, tilePlace, Quaternion.identity) as GameObject);
					}else{
						AddObj(GameObject.Instantiate(ground, tilePlace, Quaternion.identity) as GameObject);
					}
				}
			}
		}
	}
	
	private void AddObj(GameObject toAdd){
		toAdd.transform.parent = gameObject.transform;
		toAdd.GetComponent<PixelReceptical>().Initialize((int)Random.Range((int)startingDencity.x,(int)startingDencity.y));
		tiles.Add(toAdd);
	}
	
	/*void OnDrawGizmos(){
		map = gameObject.GetComponent<TiledMap>();
		
		float Xp = map.GetMapWidthInPixelsScaled()/map.NumTilesWide;
		float Yp = map.GetMapHeightInPixelsScaled()/map.NumTilesHigh;
		
		Vector3 tileTop = new Vector3();
		for(int x = 0; x < map.NumTilesWide; x++){
			for(int y = 0; y > -map.NumTilesHigh; y--){
				tileTop = transform.position + new Vector3(x * Xp + Xp/2, y * Yp, 0);
				Gizmos.DrawSphere(tileTop, .125f);
			}
		}
	}*/
}
