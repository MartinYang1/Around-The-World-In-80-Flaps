using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Transform transform;
    private float speed;
    private AudioSource audio;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        audio = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SpawnCoin(float x, float y)
    {
        if (y < -2.45f) y = -2.45f;
        else if (y > 2.86f) y = 2.86f;
        transform.position = new Vector3(x, y, 1972.907f);
    }

    public void Move()
    {
        speed = GameObject.FindObjectOfType<Pipe>().GetSpeed();
        transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GetComponent<Animator>().SetBool("Coin Death", true);
            StartCoroutine(CoinDeath());
        }
    }

    public IEnumerator CoinDeath()
    {
        GameLogic.instance.score += 5;
        audio.Play();
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(2);
        audio.Stop();
        Destroy(gameObject);
        GameLogic.instance.gameUI.coinHolder.GetCoins().Remove(this);
    }
}
