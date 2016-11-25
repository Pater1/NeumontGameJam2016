using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	public float triggerRange = 2, transitionSpeed = 2;
	private bool triggered = false;
	private Die player;
	
	public GameObject flag;	
	public GameObject pole;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Die>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, player.gameObject.transform.position) < triggerRange && !triggered){
			triggered = true;
			player.spawn = transform.position;
			StartCoroutine(raiseTheFlag());
		}
	}
	
	private IEnumerator raiseTheFlag(){
		float rotCutoff = 5f, distCutoff = .01f;
		while(true){
			bool rotDone = false;
			if(Mathf.Abs(pole.transform.localRotation.eulerAngles.z) < rotCutoff){
				pole.transform.localRotation = Quaternion.Euler(new Vector3());
				rotDone = true;
			}else{
				pole.transform.localRotation = Quaternion.Slerp(pole.transform.localRotation, Quaternion.identity, transitionSpeed * Time.deltaTime);
			}
			
			if(Vector3.Distance(pole.transform.position, transform.position) < distCutoff){
				pole.transform.position = transform.position;
				if(rotDone) break;
			}else{
				pole.transform.position = Vector3.Lerp(pole.transform.position, transform.position, transitionSpeed * Time.deltaTime);
			}
			
			yield return null;
		}
		
		while(true){
			bool rotDone = false;
			if(Mathf.Abs(flag.transform.localRotation.eulerAngles.z) < rotCutoff){
				flag.transform.localRotation = Quaternion.Euler(new Vector3());
				rotDone = true;
			}else{
				flag.transform.localRotation = Quaternion.Slerp(flag.transform.localRotation, Quaternion.identity, transitionSpeed * Time.deltaTime);
			}
			
			if(Vector3.Distance(flag.transform.position, transform.position) < distCutoff){
				flag.transform.position = transform.position;
				if(rotDone) break;
			}else{
				flag.transform.position = Vector3.Lerp(flag.transform.position, transform.position, transitionSpeed * Time.deltaTime);
			}
			
			yield return null;
		}
	}
	
	void OnDrawGizmos(){
		Gizmos.DrawSphere(transform.position, triggerRange);
	}
}
