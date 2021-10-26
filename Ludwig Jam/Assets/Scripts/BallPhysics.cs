using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
public class BallPhysics : MonoBehaviour
{
    public static BallPhysics instance;
    private Rigidbody rb;
    // the minimum speed the ball can be moving before stopping
    private float minBallSpeed = 0.1f;
    // the velocity of the ball on the last frame
    private Vector3 velocityPrev;
    // whether the ball is moving
    public bool rolling { get; private set; }
    // 0 = air, 1 = stone, 2 = grass
    private int groundMaterial;
    // whether the ball was hit this frame
    private bool ballHit;
    // the distance from the ground the ball has to be for it to be affected by friction
    private float groundFrictionHeight = 0.15f;
    // the friction force applied to the ball on grass
    [SerializeField] float grassFriction = 25f;
    [SerializeField] float stoneFriction = 5f;
    [SerializeField] float rotationFactor;
    private float currentFriction = 0;

    private bool onSlope;
    private float radius;

    public int shots { get; private set; }
    [SerializeField] int startingShots;

    private Vector3 initialPosition;

    private AudioSource grassRussle;
    private AudioSource stoneRussle;

    private int wicketsFromLastShot;
    private int wicketsFromThisShot;

    [SerializeField] CameraEffects camShake;

    [SerializeField] BallParticles particles;

    
    
    void Awake()
    {

        SetupSingleton();

        shots = startingShots;
        rb = GetComponent<Rigidbody>();
        rb.maxAngularVelocity = 9999f;
        radius = transform.lossyScale.x / 2;
        initialPosition = transform.position;

        grassRussle = AudioManager.instance.GetSource("grassRussle");
        grassRussle.volume = 0;
        grassRussle.Play();
        grassRussle.Pause();

        stoneRussle = AudioManager.instance.GetSource("stoneRussle");
        stoneRussle.volume = 0;
        stoneRussle.Play();
        stoneRussle.Pause();

        EventHandler.instance.e_Restart += ListenRestart;
        EventHandler.instance.e_Pause += ListenPause;
    }

    void SetupSingleton()
    {
        if (instance != null)
            Destroy(this);
        instance = this;
    }


    void FixedUpdate()
    {
        GetGroundMaterial();
        ApplyFriction();

        if (Vector3.Distance(rb.velocity, velocityPrev) > 10)
		{
            camShake.Amount = 0.4f * (Vector3.one * Vector3.Distance(rb.velocity, velocityPrev) / 20);
            camShake.Speed = 0.4f * (10 * Vector3.Distance(rb.velocity, velocityPrev) / 20);
            camShake.Shake();
        }
        velocityPrev = rb.velocity;
        ballHit = false;
    }

    private void GetGroundMaterial()
    {
        // Casts a ray at the ground under the ball
        float rayLength = radius + groundFrictionHeight;
        Debug.DrawRay(transform.position, Vector3.down * rayLength, Color.red);

        LayerMask groundMask = LayerMask.GetMask("Ground");
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundMask))
        //if (Physics.SphereCast(transform.position, radius, Vector3.down, out hit, rayLength, groundMask))
        {
            string hitTag = hit.transform.gameObject.tag;
            if (hitTag == "Grass" || hitTag == "BrownGrass" || hitTag == "WhiteGrass")
			{
                currentFriction = grassFriction;

                if (!grassRussle.isPlaying)
                    grassRussle.UnPause();
                grassRussle.volume = (Mathf.Min(rb.velocity.magnitude / 10f, 1)) * 0.15f;

                if (stoneRussle.isPlaying)
                    stoneRussle.Pause();
            }
            else
			{
                currentFriction = stoneFriction;

                if (grassRussle.isPlaying)
                    grassRussle.Pause();
                //grassParticles.Stop();

                if (!stoneRussle.isPlaying)
                    stoneRussle.UnPause();
                stoneRussle.volume = (Mathf.Min(rb.velocity.magnitude / 10f, 1));

                
            }

            particles.SetParticles(rb.velocity.magnitude, hitTag);
            onSlope = hit.normal != Vector3.up;
        }
        else
        {
            // The ball is in the air

            if (grassRussle.isPlaying)
                grassRussle.Pause();
            if (stoneRussle.isPlaying)
                stoneRussle.Pause();

            particles.SetParticles(rb.velocity.magnitude, "Air");


            currentFriction = 0;
            onSlope = false;
        }
    }
    

    private void ApplyFriction()
	{
		// If the ball is rolling
		if (Mathf.Abs(rb.velocity.magnitude) > minBallSpeed)
		{
			// Check whether the ball has just started rolling
			if (!rolling)
				OnStartedRolling();

			Vector3 frictionForce = rb.velocity.normalized * -1 * currentFriction;
            if (!onSlope)
			{
                rb.AddForce(frictionForce);
            }
			

			rb.angularVelocity = new Vector3(rb.angularVelocity.x, 0, rb.angularVelocity.z);
			transform.RotateAround(transform.position, Vector3.Cross(Vector3.up, rb.velocity), rotationFactor * rb.velocity.magnitude);
		}
		// If the ball is not rolling and is still flagged as rolling
		else if (rolling && !onSlope && !ballHit)
		{
			float mag = Mathf.Abs(rb.velocity.magnitude);
			rb.velocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
			OnStoppedRolling(mag);
		}
       
    }

    // Called when the ball starts rolling
    private void OnStartedRolling()
	{
        rolling = true;
        grassRussle.UnPause();
    }

    // Called when the ball stops rolling
    private void OnStoppedRolling(float mag)
	{
        rolling = false;
        grassRussle.Pause();
    }

    public void HitBall(Vector3 force)
    {
        rb.AddForce(force);
        ballHit = true;
        AudioSource source = AudioManager.instance.GetSource("malletHit");
        source.volume = 0.2f + force.magnitude / (1500 * 2);
        source.Play();

        shots--;

        wicketsFromLastShot = wicketsFromThisShot;
        wicketsFromThisShot = 0;

        camShake.Amount = 0.5f * (Vector3.one * force.magnitude / 1500f);
        camShake.Speed = 0.5f * (10 * force.magnitude / 1500f);
        camShake.Shake();
        OnStartedRolling();
    }

    public void AddShots(int s)
	{
        if (wicketsFromLastShot >= 2 && wicketsFromThisShot >= 1)
		{
            Debug.Log("skipped a shot");
		}
        else
		{
            shots += s;
        }

        if (s == 1)
        {
            wicketsFromThisShot++;
        }

    }

    public void SetShots(int s)
	{
        shots = s;
	}

    public void ListenRestart()
	{
        shots = startingShots;
        transform.position = initialPosition;
        rb.velocity = Vector3.zero;
	}

	private void OnCollisionEnter(Collision collision)
	{
        
        if (collision.collider.gameObject.tag == "Grass")
        {
            float vel = (Mathf.Abs(Mathf.Min(0, velocityPrev.y)) / 10) - 0.5f;
            AudioSource source = AudioManager.instance.GetSource("grassCollision");
            source.volume = vel;
            source.Play();
        }
        else if (collision.collider.gameObject.tag == "Stone")
        {
            float vel = rb.velocity.magnitude / 10f;
            if (vel >= 1)
            {
                AudioManager.instance.Play("collision1");
            }
            else if (vel >= 0.6f)
            {
                AudioManager.instance.Play("collision2");
            }
            else if (vel >= 0.3f)
            {
                AudioManager.instance.Play("collision3");
            }
        } 
    }

    public void ListenPause()
	{
        if (grassRussle.isPlaying)
            grassRussle.Pause();
        //grassParticles.Stop();
        if (stoneRussle.isPlaying)
            stoneRussle.Pause();
    }
}
