using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;
    private int livesValue = 3;
    private bool facingRight = true;

    public int scoreValue = 0;
    public float horizontalValue;
    public float speed;
    public Text score;
    public Text lives;
    public GameObject player;
    public GameObject winTextObject;
    public GameObject loseTextObject;
    public Transform Target;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    public AudioSource musicSource2;
    public float volume = 0.5f;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        {
            musicSource.clip = musicClipOne;
            musicSource.Play();
            musicSource.loop = true;
        }

        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        anim = GetComponent<Animator>();
    }

    void Update()
    {

        move();
        Flip();

        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetInteger("State", 2);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetInteger("State", 0);
        }

        if (scoreValue == 4)
        {
            transform.position = Target.position;
            livesValue = 3;
            lives.text = livesValue.ToString();
        }

        if (scoreValue == 8)
        {
            musicSource.clip = musicClipOne;
            musicSource.Stop();
        }

        if (scoreValue == 8)
        {
            musicSource2.PlayOneShot(musicClipTwo, volume);
            musicSource2.loop = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }

        if (scoreValue == 8)
        {
            // display win text
            winTextObject.SetActive(true);

        }

        if (livesValue == 0)
        {
            // display lose text
            loseTextObject.SetActive(true);

            Destroy(player);

        }
    }

    void Flip()
    {
        if ((horizontalValue < 0 && facingRight) || (horizontalValue > 0 && !facingRight))
        {
            facingRight = !facingRight;
            transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    void move()
    {
        horizontalValue = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        //transform.Translate(new Vector3()
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse); //the 3 in this line of code is the player's "jumpforce," and you change that number to get different jump behaviors.  You can also create a public variable for it and then edit it in the inspector.
            }
        }
    }
}