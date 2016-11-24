using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PixelVacuum : MonoBehaviour {
	public float tickDelay = .75f, vacCastDist = 2, playerWidth = .2f, scanAngle = 15, maxPixels = 10;
	
	public int myPixels = 0;
	private PlatfomerMotor move;
	
	public GameObject refPixel;
	public int maxDustPixels = 100;
	public List<GameObject> inactiveDustPixels = new List<GameObject>(), activeDustPixels = new List<GameObject>();
	
	public void Wipe(){
		for(int i = 0; i < inactiveDustPixels.Count; i++){
			GameObject.Destroy(inactiveDustPixels[i]);
		}
		for(int i = 0; i < activeDustPixels.Count; i++){
			GameObject.Destroy(activeDustPixels[i]);
		}
	}
	
	void Start(){
		move = gameObject.GetComponent<PlatfomerMotor>();
		
		for(int i = 0; i < maxDustPixels; i++){
			GameObject go = GameObject.Instantiate(refPixel);
			go.SetActive(false);
			inactiveDustPixels.Add(go);
		}
	}
	
	public bool vaccing = false, dusting = false;
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Fire") != 0){
			if((Input.GetAxis("Fire") < 0) ? (myPixels >= maxPixels) : (myPixels <= 0)){
				if(!dusting){
					StartCoroutine(SetDustActive(false));
				}
				return;
			}
			
			if(!vaccing) StartCoroutine(Vacuum(Input.GetAxis("Fire") < 0));
			if(!dusting){
				StartCoroutine(SetDustActive(true));
			}
		}else{
			if(!dusting){
				StartCoroutine(SetDustActive(false));
			}
		}
	}
	
	public void DustCallback(PixelObject pxo){
		activeDustPixels.Remove(pxo.gameObject);
		inactiveDustPixels.Add(pxo.gameObject);
		pxo.gameObject.SetActive(false);
	}
	
	public void DustStart(PixelObject pxo){
		Vector3 destination = new Vector3();
		if(Input.GetAxis("Fire") < 0){
			destination = transform.position;
			pxo.gameObject.transform.position = transform.position + dirAtAngle(Random.Range(scanAngle,-scanAngle)*move.FacingRight(), vacCastDist);
		}else{
			destination = transform.position + dirAtAngle(Random.Range(scanAngle,-scanAngle)*move.FacingRight(), vacCastDist);
			pxo.gameObject.transform.position = transform.position;
		}
		pxo.StartCoroutine(pxo.Vacuum(destination,2,.1f,this));
		
		activeDustPixels.Add(pxo.gameObject);
		inactiveDustPixels.Remove(pxo.gameObject);
	}
	
	private IEnumerator SetDustActive(bool active){
		bool canDust = false;
		for(int k = (int)-scanAngle; k < (int)scanAngle; k += (int)scanAngle/5){
			RaycastHit2D ryo = Physics2D.Raycast	(transform.position + dirAtAngle(k*move.FacingRight(), playerWidth), 
													dirAtAngle(k*move.FacingRight(), vacCastDist), 
													vacCastDist);
			if((bool)ryo && ryo.collider.gameObject.GetComponent<PixelReceptical>()){
				canDust = true;
				break;
			} 
		}
		if(!canDust) yield break;
		
		dusting = true;
		int per = 4;
		if(active){
			for(int i = 0; i < inactiveDustPixels.Count-per; i += per){
				if((Input.GetAxis("Fire") == 0) == active || !dusting) break;
				for(int j = i; j < i + per; j++){
					if(j >= inactiveDustPixels.Count)break;
					PixelObject pixo = inactiveDustPixels[j].GetComponent<PixelObject>();
					
					inactiveDustPixels[j].SetActive(true);
					DustStart(pixo);
				}
				yield return new WaitForSeconds(Random.Range(.1f,.01f));
			}
		}else{
			for(int i = 0; i < activeDustPixels.Count; i++){
				PixelObject pixo = activeDustPixels[i].GetComponent<PixelObject>();
				
				pixo.StartCoroutine(pixo.Fall(Random.Range(10.0f,5.0f),Random.Range(.001f,.1f),Mathf.Epsilon,this));
			}
		}
		
		dusting = false;
	}
	
	private Vector3 dirAtAngle(float angle, float dist){
		if(move.FacingRight() < 0) angle += 180;
		float rads = Mathf.Deg2Rad * angle;
		Vector3 vec = new Vector3(Mathf.Cos(rads), Mathf.Sin(rads), 0);
		return (vec * dist);
	}
	
	private IEnumerator Vacuum(bool reverse){
		vaccing = true;
		List<PixelReceptical> hits = new List<PixelReceptical>();
		for(int i = (int)-scanAngle; i < (int)scanAngle; i += (int)scanAngle/5){
			RaycastHit2D ryo = Physics2D.Raycast	(transform.position + dirAtAngle(i*move.FacingRight(), playerWidth), 
													dirAtAngle(i*move.FacingRight(), vacCastDist), 
													vacCastDist);
			
			if((bool)ryo && ryo.collider.gameObject.GetComponent<PixelReceptical>() && ( reverse ? myPixels < maxPixels : myPixels > 0)){
				PixelReceptical pr = ryo.collider.gameObject.GetComponent<PixelReceptical>();
				if(!hits.Contains(pr)){
					myPixels += pr.TransferPixels(reverse ? -1 : 1);
					hits.Add(pr);
				}
			}
		}
		
		yield return new WaitForSeconds(tickDelay);
		
		vaccing = false;
	}
}
