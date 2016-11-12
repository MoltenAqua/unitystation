﻿using UnityEngine;
using System.Collections;
using SS.GameLogic;


namespace SS.PlayGroup{
	
	[RequireComponent (typeof (Rigidbody2D))]

public class PhysicsMove : MonoBehaviour {

		public GameManager gameManager;

		private Rigidbody2D thisRigi;

		public bool isMoving = false;
		public bool lerpA = false;
		public bool isSpaced = false;
		public bool triedToMoveInSpace = false;
		public bool keyDown = false;

		private Vector2 moveDirection;
		public Vector2 _moveDirection { get { return moveDirection; } }
		private Vector3 startPos;
		private Vector3 node;
		private Vector3 toClamp;

		public float lerpSpeed = 7f;
		private float lerpTime = 0f;
		public float clampPos;
		public float moveSpeed;


	// Use this for initialization
	void Start () {
			thisRigi = GetComponent<Rigidbody2D> ();
			GameObject findGM = GameObject.FindGameObjectWithTag ("GameManager");
			if (findGM != null) {
				gameManager = findGM.GetComponent<GameManager> ();
				//TODO error handling
			
			}
	}
	
	// Update is called once per frame
	void Update () {

			//when movekeys are released then take the character to the nearest node
			if (lerpA) {
				LerpToTarget ();
			}
	
	}

	void FixedUpdate(){

			if (isMoving) {
				//Move character via RigidBody
				MoveRigidBody ();

			} 
	}

		//move in direction input
		public void MoveInDirection(Vector2 direction){

			lerpA = false;
		    moveDirection = direction;
			isMoving = true;

			if (direction == Vector2.right || direction == Vector2.left) {
			
				clampPos = transform.position.y;
			
			}

			if (direction == Vector2.down || direction == Vector2.up) {
			
				clampPos = transform.position.x;
			
			}
	
		}



		//movement input stopped
		public void MoveInputReleased(){ 

			startPos = transform.position;
			node = gameManager.GetClosestNode (transform.position, thisRigi.velocity);
			thisRigi.velocity = Vector3.zero;
			lerpTime = 0f;
			lerpA = true;

		}

		//used with LerpA in update
		private void LerpToTarget(){

			lerpTime += Time.deltaTime;
			float t = lerpTime * lerpSpeed;

			transform.position = Vector3.Lerp (startPos, node, t);

			if (moveDirection == Vector2.right && transform.position.x >= node.x) {
				isMoving = false;
				lerpA = false;
				thisRigi.velocity = Vector3.zero;
			}
			if (moveDirection == Vector2.left && transform.position.x <= node.x) {
				isMoving = false;
				lerpA = false;
				thisRigi.velocity = Vector3.zero;
			}
			if (moveDirection == Vector2.up && transform.position.y >= node.y) {
				isMoving = false;
				lerpA = false;
				thisRigi.velocity = Vector3.zero;
			}
			if (moveDirection == Vector2.down && transform.position.y <= node.y) {
				isMoving = false;
				lerpA = false;
				thisRigi.velocity = Vector3.zero;
			}
		}

		//move the character via RigidBody in FixedUpdate
		private void MoveRigidBody(){

			if (!triedToMoveInSpace) {

				if (moveDirection == Vector2.down) {
					thisRigi.velocity = new Vector3 (0f, moveDirection.y, 0).normalized * moveSpeed;
					toClamp = transform.position;
					Mathf.Clamp (toClamp.x, clampPos, clampPos);
					transform.position = toClamp;
					return;
				}
				if (moveDirection == Vector2.up) {
					thisRigi.velocity = new Vector3 (0f, moveDirection.y, 0).normalized * moveSpeed;
					toClamp = transform.position;
					Mathf.Clamp (toClamp.x, clampPos, clampPos);
					transform.position = toClamp;
					return;
				}
				if (moveDirection == Vector2.right) {
					thisRigi.velocity = new Vector3 (moveDirection.x, 0f, 0).normalized * moveSpeed;
					toClamp = transform.position;
					Mathf.Clamp (toClamp.y, clampPos, clampPos);
					transform.position = toClamp;
					return;
				}
				if (moveDirection == Vector2.left) {
					thisRigi.velocity = new Vector3 (moveDirection.x, 0f, 0).normalized * moveSpeed;
					toClamp = transform.position;
					Mathf.Clamp (toClamp.y, clampPos, clampPos);
					transform.position = toClamp;
					return;
				}
			}
			if (isSpaced && !triedToMoveInSpace) {
				triedToMoveInSpace = true;
				thisRigi.mass = 0f;
				thisRigi.drag = 0f;
				thisRigi.angularDrag = 0f;

			}

		}

	

}
}
