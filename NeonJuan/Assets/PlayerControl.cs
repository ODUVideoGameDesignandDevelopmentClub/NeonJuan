using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float Movespeed = 20f;
	public float jumpVelocity = 50f;
	public bool canJump = true;

	private bool facingLeft = false;

	private Animator anim;
	private AnimatorStateInfo currentBaseState;

	static int idleState = Animator.StringToHash("Base Layer.JuanIdle");
	static int punchOneState = Animator.StringToHash("Base Layer.BasicPunch1");
	static int punchTwoState = Animator.StringToHash("Base Layer.BasicPunch2");

	public float comboWindow = .8f;
	private float comboTime;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		//Get the current state
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0); 

		//Horizontal movement
		float movement = Input.GetAxis("Horizontal");
		this.rigidbody2D.AddForce(new Vector2(Movespeed * movement, 0));

		if(movement < 0 && !facingLeft)
		{
			this.transform.Rotate(new Vector3(0,180,0));
			facingLeft = true;
		}
		else if(movement > 0 && facingLeft)
		{
			this.transform.Rotate(new Vector3(0,180,0));
			facingLeft = false;
		}

		if(Input.GetKeyDown("space"))
		{
			if(canJump)
			{
				this.rigidbody2D.AddForce(new Vector2(0, jumpVelocity));
				canJump = false;
			}
		}

		if(Input.GetButtonDown("Fire1"))
		{
			if(currentBaseState.nameHash ==punchOneState)
				anim.SetBool("Punch2", true);
			else
				anim.SetBool("Punch1", true);


			comboTime = Time.time;
		}
		else if(currentBaseState.nameHash == punchOneState)
		{
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Punch1", false);
			}
		}
		else if(currentBaseState.nameHash == punchTwoState)
		{
			if(!anim.IsInTransition(0))
			{
				anim.SetBool("Punch2", false);
			}
			
			//if(Time.time > comboTime + comboWindow)
			//{
			//	anim.SetBool("Combo", false);
			//6}
		}

	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		canJump = true;
	}
}
