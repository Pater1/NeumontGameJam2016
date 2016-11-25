using UnityEngine;

public class KillOnceBelow : MonoBehaviour {
	public Die[] dieers;
	void Start(){
		dieers = FindObjectsOfType(typeof(Die)) as Die[];
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < dieers.Length; i++){
			if(transform.position.y > dieers[i].gameObject.transform.position.y){
				dieers[i].MakeDie();
			}
		}
	}
}
