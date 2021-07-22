using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private Animator playerAnim;
    private AudioSource playerAudio;
    public ParticleSystem explosionParticle;
    public ParticleSystem dirtParticle;
    public AudioClip jumpSound;
    public AudioClip crashSound;
    public float jumpForce;
    public float gravityModifier;
    public bool isOnGround = true;
    public int jumpTimes = 0;
    public bool gameOver = true;
    public float gameSpeed = 1;


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        Physics.gravity *= gravityModifier;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && jumpTimes < 2 && !gameOver)
        {
            playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerAnim.SetTrigger("Jump_trig");
            playerAudio.PlayOneShot(jumpSound, 1.0f);
            dirtParticle.Stop();
            isOnGround = false;
            jumpTimes += 1;
        }
    }

    public void OnSpeedUp(InputAction.CallbackContext context)
    {
        if (context.started && !gameOver)
        {
            gameSpeed = 2f;
            playerAnim.SetFloat("Speed_f", gameSpeed);
        }
        else if (context.canceled)
        {
            gameSpeed = 1f;
            playerAnim.SetFloat("Speed_f", gameSpeed);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (playerAnim.GetFloat("Speed_f") > 0.25f)
            {
                dirtParticle.Play();
            }
            isOnGround = true;
            jumpTimes = 0;
        }
        else if (collision.gameObject.CompareTag("Obstacle"))
        {
            gameOver = true;
            playerAnim.SetBool("Death_b", true);
            playerAnim.SetInteger("DeathType_int", 1);
            dirtParticle.Stop();
            explosionParticle.Play();
            playerAudio.PlayOneShot(crashSound, 1.0f);
            Debug.Log("Game Over");
        }
    }
}
