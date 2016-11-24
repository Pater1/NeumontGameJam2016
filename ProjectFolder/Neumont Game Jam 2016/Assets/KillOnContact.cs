using UnityEngine;
using System.Collections;

public class KillOnContact : MonoBehaviour {
	
	public float killRange = .2f, offset = .3f;
	public Die[] dieers;
	void Start(){
		dieers = FindObjectsOfType(typeof(Die)) as Die[];
	}
	
	void Update () {
		gameObject.GetComponent<Collider2D>().enabled = false;
		for(int i = 0; i < dieers.Length; i++){
			if(dieers[i] == gameObject) continue;
			RaycastHit2D ryo = Physics2D.Raycast(transform.position + (transform.position - dieers[i].gameObject.transform.position) * offset, 
													transform.position - dieers[i].gameObject.transform.position, 
													killRange);
			if((bool)ryo && ryo.collider.gameObject.GetComponent<Die>()){
				ryo.collider.gameObject.GetComponent<Die>().MakeDie();
			}
		}
		gameObject.GetComponent<Collider2D>().enabled = true;
	}
}
