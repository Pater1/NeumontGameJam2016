using UnityEngine;
using System.Collections;

public class KillOnContact : MonoBehaviour {
	
	public float killRange = .2f, threatRange = .5f, offset = .3f;
	public Die[] dieers;
	void Start(){
		dieers = FindObjectsOfType(typeof(Die)) as Die[];
	}
	
	void Update () {
		gameObject.GetComponent<Collider2D>().enabled = false;
		bool threat = false;
		for(int i = 0; i < dieers.Length; i++){
			if(dieers[i].gameObject == gameObject) continue;
			
			if(Vector3.Distance(transform.position, dieers[i].gameObject.transform.position) < killRange){
				dieers[i].MakeDie();
			}
			if(Vector3.Distance(transform.position, dieers[i].gameObject.transform.position) < threatRange){
				threat = true;
			}
		}
		if(gameObject.GetComponent<Animator>()) gameObject.GetComponent<Animator>().SetBool("Threat", threat);
		gameObject.GetComponent<Collider2D>().enabled = true;
	}
}
