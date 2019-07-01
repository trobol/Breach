using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public PlayerInput input;


	Rigidbody2D rb;
	SpriteRenderer sr;
	Animator animator;
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		sr = GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();
	}

	public float jumpForce = 1;
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.R))
		{
			transform.position = Vector3.zero;
		}
		input.Update();
		FlipSprite();

		UpdateMovement();
		animator.SetBool("wall_slide", wallSliding);
	}
	public float movementCooldown = 0;
	public float movementSpeed = 1;
	public float movementChangeRate = 1;
	float movement = 0;
	void UpdateMovement()
	{
		if (movementCooldown > 0)
		{
			movementCooldown -= Time.deltaTime;
		}
		else if (movement != input.axis.x)
		{
			if (input.axis.x == 0)
			{
				movement = 0;
			}
			else
			{
				movement += input.axis.x * movementChangeRate;
				Mathf.Clamp(movement, 0, input.axis.x);
			}
		}
	}
	public bool grounded;
	public bool wallSliding;
	void FlipSprite()
	{
		if (wallSliding)
		{
			sr.flipX = movement > 0;
			return;
		}
		if (sr.flipX)
		{
			if (movement > 0)
			{

				sr.flipX = false;


			}

		}
		else
		{
			if (movement < 0)
			{

				sr.flipX = true;

			}
		}
	}
	public bool canJump = true;
	public LayerMask groundMask;
	public float slideSpeed = 0;
	void FixedUpdate()
	{

		Vector2 v = rb.velocity;

		v.x = movement * movementSpeed;



		grounded = Physics2D.CapsuleCast(transform.position, new Vector2(1, 2), CapsuleDirection2D.Vertical, 0, Vector2.down, 0.1f, groundMask);

		if (input.axis.x != 0)
		{
			if (Physics2D.BoxCast(new Vector3(0, 0.5f, 0) + transform.position, new Vector2(0.5f, 0.5f), 0, new Vector2(movement, 0), 0.5f, groundMask))
			{
				v.x = 0;
				if (input.axis.y > 0 && canJump)
				{
					movement = input.axis.x * -1f;
					movementCooldown = 0.3f;
					rb.AddForce(Vector2.up * jumpForce);
					canJump = false;
					wallSliding = false;

				}
				else
				{
					wallSliding = true;
					v.y = slideSpeed;
				}

				grounded = true;
			}
			else
			{
				wallSliding = false;
			}
		}
		if (!canJump && grounded && input.axis.y <= 0)
			canJump = true;
		if (input.axis.y > 0 && canJump)
		{
			rb.AddForce(Vector2.up * jumpForce);
			canJump = false;
		}

		input.axis.y = 0;

		rb.velocity = v;
	}

}


[System.Serializable]
public class PlayerInput
{
	public KeyCode left, right, jump;

	public Vector2 axis;

	public void Update()
	{


		if (Input.GetKeyDown(right))
		{
			axis.x = 1;
		}
		else if (Input.GetKeyDown(left))
		{
			axis.x = -1;
		}

		if (Input.GetKeyUp(right))
		{
			if (Input.GetKey(left))
			{
				axis.x = -1;
			}
			else
			{
				axis.x = 0;
			}
		}
		if (Input.GetKeyUp(left))
		{
			if (Input.GetKey(right))
			{
				axis.x = 1;
			}
			else
			{
				axis.x = 0;
			}
		}



		// JUMP
		if (Input.GetKeyDown(jump) || Input.GetKey(jump))
		{
			axis.y = 1;
		}

	}

}