using UnityEngine;
using System.Collections;

public class PlatfomerMotor : MonoBehaviour {

	public float jumpPower, jumpImpulse, gravity = .25f, groundMoveSpeed = 1, airMoveSpeed = .25f, downCastDist = .01f, playerHeight = .125f;
	
	public Vector3 move = new Vector3();
	
	public bool followCam = true;
	
	public GameObject cam;
	private Rigidbody2D rigi;
	private bool jumped = false;
	
	void Start(){
		rigi = gameObject.GetComponent<Rigidbody2D>();
	}
	
	private bool isTouchingGround(){
		RaycastHit2D hit = Physics2D.Raycast (transform.position - transform.up * playerHeight, -transform.up, downCastDist);
		if((bool)hit && !hit.collider.isTrigger){
			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {
		Move();
		Jump();
		if(followCam) CamMove();
		
		if(move.x > 0) face = 1;
		if(move.x < 0) face = -1;
		
		transform.position += move;
	}
	
	private int face = 1;
	public int FacingRight(){
		return face;
	}
	
	private void CamMove(){
		cam.transform.position = Vector3.Lerp(cam.transform.position ,transform.position + 10 * move + new Vector3(0,0,-10),Time.deltaTime * 2);
	}
	
	private void Move(){
		float moveSpeed = (isTouchingGround()) ? groundMoveSpeed : airMoveSpeed;
		move.x = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
	}
	
	private float jumpBuild = 0, vertVelocity = 0;
	private void Jump(){
		if(Input.GetAxis("Vertical") != 0 && jumpBuild > 0){
			if(jumped){
				jumpBuild -= Time.deltaTime * 2;
				vertVelocity += jumpPower * Time.deltaTime * jumpBuild;
			}else{
				vertVelocity += jumpImpulse;
			}
			
			jumped = true;
		}else{
			if(isTouchingGround()){
				if(Input.GetAxis("Vertical") == 0){
					jumped = false;
					jumpBuild = 1;
				}
				vertVelocity = 0;
			}else{
				vertVelocity -= Time.deltaTime * gravity;
			}
		}
		
		move.y = vertVelocity;
	}
}
