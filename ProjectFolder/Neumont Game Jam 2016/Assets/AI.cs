using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {

	public float walkSpeed = 1, rayCastOffset = .2f, rayCastRange = .5f, gravity = 1, fallCastOffset = .4f, fallCastRange = .01f;
	private float face = -1;
	
	// Update is called once per frame
	void Update () {
		Wall();
		Walk();
		Fall();
	}
	
	private bool isTouchingGround(){
		RaycastHit2D hit = Physics2D.Raycast (transform.position - transform.up * fallCastOffset, -transform.up, fallCastRange);
		if((bool)hit && !hit.collider.isTrigger){
			return true;
		}
		return false;
	}
	
	private void Wall(){
		RaycastHit2D ryo = Physics2D.Raycast(transform.position + face * transform.right * rayCastOffset, 
													face * transform.right, 
													rayCastRange);
		if((bool)ryo && !ryo.collider.gameObject.GetComponent<Die>() && !ryo.collider.isTrigger){
			face = -face;
		}
		
		transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(face,1,1), Time.deltaTime * 8);
	}
	
	private void Walk(){
		transform.position += transform.right * face * walkSpeed * Time.deltaTime;
	}
	
	private float fallSpeed;
	private void Fall(){
		if(!isTouchingGround()){
			fallSpeed += gravity * Time.deltaTime;
		}else{
			fallSpeed = 0;
		}
		transform.position -= transform.up * fallSpeed;
	}
}
