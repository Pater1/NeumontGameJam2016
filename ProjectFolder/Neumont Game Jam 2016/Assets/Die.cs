using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Die : MonoBehaviour {
	public int explosionCount = 10;
	public float watchDeathTime = 2;
	public Vector2 explosionRange = new Vector2(1,5);
	public GameObject pixel;
	
	public Vector3 spawn = new Vector3(), deathLocal;
	
	public void MakeDie(){
		gameObject.GetComponent<SpriteRenderer>().enabled = false;
		if(gameObject.GetComponent<PlatfomerMotor>())	gameObject.GetComponent<PlatfomerMotor>().followCam = false;
		for(int i = 0; i < explosionCount; i++){
			GameObject go = GameObject.Instantiate(pixel);
			go.transform.position = gameObject.transform.position;
			PixelObject px = go.GetComponent<PixelObject>();
			px.StartCoroutine(px.LerpOut(transform.position + dirAtAngle(Random.Range(-180.0f,180.0f),Random.Range(explosionRange.x,explosionRange.y)),
									Random.Range(2.0f,5.0f), 0.01f));
		}
		deathLocal = transform.position;
		StartCoroutine(Dieing());
	}
	
	private IEnumerator Dieing(){
		yield return new WaitForSeconds(watchDeathTime);
		Reset();
	}
	
	private void Reset(){
		transform.position = spawn;
		gameObject.GetComponent<SpriteRenderer>().enabled = true;
		if(gameObject.GetComponent<PlatfomerMotor>()) gameObject.GetComponent<PlatfomerMotor>().followCam = true;
	}
	
	private Vector3 dirAtAngle(float angle, float dist){
		float rads = Mathf.Deg2Rad * angle;
		Vector3 vec = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0);
		return (vec * dist);
	}
}
