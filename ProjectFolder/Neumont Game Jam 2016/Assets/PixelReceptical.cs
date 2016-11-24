using UnityEngine;
using System.Collections.Generic;

public class PixelReceptical : MonoBehaviour {
	
	private SpriteRenderer rend;
	private Texture2D tex;
	private Collider2D col;
	private Dictionary<int, List<PixelUnit>> textReff = new Dictionary<int, List<PixelUnit>>();
	
	public int placablePixels = 10;
	
	public int percentTransparent = 10;
	
	void Start () {
		Initialize();
	}
	
	public void Initialize(){
		rend = gameObject.GetComponent<SpriteRenderer>();
		tex = rend.sprite.texture;
		col = gameObject.GetComponent<Collider2D>();
		
		if(percentTransparent < 0) percentTransparent = 0;
		if(percentTransparent > placablePixels) percentTransparent = placablePixels;
		
		BakeTectureReffs();
	}
	
	private void BakeTectureReffs(){
		textReff = new Dictionary<int, List<PixelUnit>>();
		for(int y = 0; y < tex.height; y++){
			for(int x = 0; x < tex.width; x++){
				int rand = (int)Random.Range(0,placablePixels);
				PixelUnit toAdd = PixelUnit._NewPixelUnit(tex.GetPixel(x,y),x,y);
				
				if(!textReff.ContainsKey(rand)){
					textReff.Add(rand, new List<PixelUnit>(){toAdd});
				}else{
					textReff[rand].Add(toAdd);
				}
			}
		}
	}
	
	public int TransferPixels(int amountGiven){
		if(percentTransparent <= 0 && amountGiven < 0){
			return 0;
		}
		if(percentTransparent >= placablePixels && amountGiven > 0){
			return 0;
		}
		
		percentTransparent += amountGiven;
		RefreshTexure();
		
		if(percentTransparent <= 2){
			col.isTrigger = true;
			if(gameObject.GetComponent<Rigidbody2D>()) gameObject.GetComponent<Rigidbody2D>().Sleep();
			if(gameObject.GetComponent<AI>()) gameObject.GetComponent<AI>().enabled = false;
			if(gameObject.GetComponent<KillOnContact>()) gameObject.GetComponent<KillOnContact>().enabled = false;
		}else if(percentTransparent >= 2){
			col.isTrigger = false;
			if(gameObject.GetComponent<Rigidbody2D>()) gameObject.GetComponent<Rigidbody2D>().WakeUp();
			if(gameObject.GetComponent<AI>()) gameObject.GetComponent<AI>().enabled = true;
			if(gameObject.GetComponent<KillOnContact>()) gameObject.GetComponent<KillOnContact>().enabled = false;
		}
		
		return -amountGiven;
	}
	
	private void RefreshTexure(){
		Texture2D tmpTex = new Texture2D(tex.width, tex.height);
		for(int i = 0; i < placablePixels; i++){
			bool trans = (i >= percentTransparent);
			List<PixelUnit> pxs = textReff[i];
			for(int j = 0; j < pxs.Count; j++){
				PixelUnit pxu = pxs[j];
				tmpTex.SetPixel(pxu.x, pxu.y, pxu.GetPixelColor(trans));
			}
		}
		tmpTex.Apply();
		rend.sprite = Sprite.Create(tmpTex, new Rect(0,0,tmpTex.width,tmpTex.height), new Vector2(.5f,.5f));
		tex = rend.sprite.texture;
	}
	
	public class PixelUnit{
		public Color initialColor;
		public int x,y;
		private float maxTrans;
		
		public Color GetPixelColor(bool forceTrans){
			Color pxc = Color.white;
			pxc.r = initialColor.r;
			pxc.g = initialColor.g;
			pxc.b = initialColor.b;
			pxc.a = (forceTrans) ? 0 : maxTrans;
			
			return pxc;
		}
		
		public static PixelUnit _NewPixelUnit(Color init, int mX, int mY){
			PixelUnit pxu = new PixelUnit();
			pxu.initialColor = init;
			pxu.x = mX;
			pxu.y = mY;
			pxu.maxTrans = init.a;
			return pxu;
		}
	}
}
