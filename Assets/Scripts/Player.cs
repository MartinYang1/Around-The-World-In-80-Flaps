using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private float jumpSpeed;

    [SerializeField]
    private int DIVE_DELAY;

    private float jumpTimeEnd;
    public static int frameCount;
    private bool birdDive;

    private Rigidbody2D myBody;
    private Animator anim;
    private AudioSource audio;
    private string BIRD_ANIMATION = "Bird State";

    void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        birdDive = false;
    }

    public void Jump()
    {
        if (Input.GetButton("Jump"))
        {
            myBody.velocity = new Vector2(myBody.velocity.x, jumpSpeed);

            anim.SetInteger(BIRD_ANIMATION, 0);
            jumpTimeEnd = Time.time + DIVE_DELAY;
            birdDive = false;
        }
    }

    public void AnimatePlayer()
    {
        if (!GameLogic.instance.gameEnd)
        {
            if (!birdDive && Time.time >= jumpTimeEnd)
            {
                anim.SetInteger(BIRD_ANIMATION, 1);
                frameCount++;
                if (frameCount >= 10) birdDive = true;
            }

            else if (birdDive)
            {
                anim.SetInteger(BIRD_ANIMATION, 2);
                frameCount = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!GameLogic.instance.gameEnd) audio.Play();
        foreach (ContactPoint2D hitPos in collision.contacts)
        {
            if (hitPos.normal.y == 1f || hitPos.normal.y == -1f)
            {
                // The bird fell or flew up onto an object
                anim.SetInteger(BIRD_ANIMATION, -1);
            }
            else
            {
                anim.SetInteger(BIRD_ANIMATION, -2);
            }
        }
        GameLogic.instance.gameEnd = true;
    }
    
}
