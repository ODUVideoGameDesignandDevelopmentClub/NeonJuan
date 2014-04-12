using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

	public float Movespeed = 20f;
	public float MaxSpeed = 215f;
	public float jumpVelocity = 50f;

	private bool facingLeft = false;
	private Transform groundCheck;
	private bool grounded;
	private bool jump;

	private Animator anim;
	private AnimatorStateInfo currentBaseState;

	private CircleCollider2D leftFist;
	private CircleCollider2D rightFist;

	static int idleState = Animator.StringToHash("Base Layer.JuanIdle");
	static int punchOneState = Animator.StringToHash("Base Layer.BasicPunch1");
	static int punchTwoState = Animator.StringToHash("Base Layer.BasicPunch2");
	static int jumpState = Animator.StringToHash("Base Layer.JuanJump");

	// Use this for initialization
	void Awake() {
		anim = GetComponent<Animator>();
		groundCheck = transform.Find("GroundCheck");
		leftFist = transform.Find("LeftHand/LeftFistSpawner").GetComponentInChildren<CircleCollider2D>();
		rightFist = transform.Find("RightHand/RightFistSpawner").GetComponentInChildren<CircleCollider2D>();

		leftFist.enabled = false;
		rightFist.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

		if(Input.GetKeyDown("space") && grounded)
		{
			jump = true;
		}

		//Get the current state
		currentBaseState = anim.GetCurrentAnimatorStateInfo(0); 

		if(Input.GetButtonDown("Fire1"))
		{
			if(currentBaseState.nameHash == punchOneState)
			{
				anim.SetTrigger("Punch2");
				rightFist.enabled = true;
				//leftFist.enabled = false;
			}
			else if(currentBaseState.nameHash == idleState)
			{
				anim.SetTrigger("Punch1");
				leftFist.enabled = true;
			}
		}
		else if(currentBaseState.nameHash == idleState)
		{
			leftFist.enabled = false;
			rightFist.enabled = false;		
		}
	}

	void FixedUpdate()
	{
		//Horizontal movement
		float movement = Input.GetAxis("Horizontal");
		rigidbody2D.AddForce(new Vector2(Movespeed * movement, 0));

		if( Mathf.Abs(rigidbody2D.velocity.x) > MaxSpeed)
		{
			rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * MaxSpeed, rigidbody2D.velocity.y);
		}

		if(movement < 0 && !facingLeft)
		{
			//this.transform.Rotate(new Vector3(0,180,0));
			transform.localScale = new Vector3(-1,1,1);
			facingLeft = true;
		}
		else if(movement > 0 && facingLeft)
		{
			//this.transform.Rotate(new Vector3(0,180,0));
			transform.localScale = new Vector3(1,1,1);
			facingLeft = false;
		}
		
		if(jump)
		{

			anim.SetTrigger("Jump");
			rigidbody2D.AddForce(new Vector2(0, jumpVelocity));
			jump = false;
		}
	}
}
