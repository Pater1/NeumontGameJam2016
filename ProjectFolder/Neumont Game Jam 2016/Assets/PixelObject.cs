using UnityEngine;
using System.Collections;

public class PixelObject : MonoBehaviour {

	public Vector2 pixelSize = new Vector2(2,2);
	public SpriteRenderer rend;

	// Use this for initialization
	void Start () {
		rend = gameObject.GetComponent<SpriteRenderer>();
		ColorInit();
	}
	
	public void ColorInit(){
		Vector2 vec = new Vector2(Random.Range(-Mathf.PI,Mathf.PI),Random.Range(-Mathf.PI,Mathf.PI));
		float sat = .85f;
		Color col = new Color(Mathf.Cos(vec.x) * Mathf.Sin(vec.y) * sat,Mathf.Cos(vec.x) * Mathf.Cos(vec.y) * sat,Mathf.Sin(vec.y) * sat,1);
		
		Texture2D tmpTex = new Texture2D((int)pixelSize.x, (int)pixelSize.y);
		for(int x = 0; x < tmpTex.width; x++){
			for(int y = 0; y < tmpTex.height; y++){
				tmpTex.SetPixel(x, y, col);
			}
		}
		tmpTex.Apply();
		
		rend.sprite = Sprite.Create(tmpTex, new Rect(0,0,tmpTex.width,tmpTex.height), new Vector2(.5f,.5f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(Random.Range(-1,1),Random.Range(-1,1),1), Time.deltaTime);
	}
	
	
	public IEnumerator LerpOut(Vector3 toWhere, float speed, float cutOffDist){
		while(Vector3.Distance(toWhere, transform.position) > cutOffDist){
			transform.position = Vector3.Lerp(transform.position, toWhere, Time.deltaTime * speed);
			yield return null;
		}
		GameObject.Destroy(gameObject);
	}

	private Vector3 inertia = new Vector3();
	private bool vacc = false;
	public IEnumerator Vacuum(Vector3 toWhere, float speed, float cutOffDist, PixelVacuum vac){
		vacc = true;
		while(Vector3.Distance(toWhere, transform.position) > cutOffDist){
			if(!vacc)break;
			Vector3 pos = Vector3.Lerp(transform.position, toWhere, Time.deltaTime * speed);
			inertia = pos - transform.position;
			transform.position = pos;
			yield return null;
		}
		if(vacc) vac.DustCallback(this);
	}
	public IEnumerator Fall(float drag, float gravity, float castDist, PixelVacuum vac){
		Vector3 vel = new Vector3();
		vacc = false;
		while(!Physics2D.Raycast(transform.position, -Vector3.up, castDist)){
			inertia = Vector3.Lerp(inertia, new Vector3(0,0,0), drag * Time.deltaTime);
			vel.y -= gravity * Time.deltaTime;
			transform.position += inertia + vel;
			yield return null;
		}
		
		vac.DustCallback(this);
	}
}
