using UnityEngine;

public class Pipe : MonoBehaviour
{
    private GameObject pipe;
    private float speed;
    private float height;

    public void Constructor(float posX, float posY)
    {
        this.pipe = gameObject;
        pipe.transform.position = new Vector3(posX, posY, 1972.907f);
        height = GetComponent<SpriteRenderer>().bounds.size.y;
    }

    public void Move()
    {
        speed = GameLogic.instance.gameUI.Bg.GetBgSpeed() + 0.5f;
        pipe.transform.position -= new Vector3(speed, 0, 0) * Time.deltaTime;
    }

    public GameObject GetPipe()
    {
        return pipe;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetHeight()
    {
        return height;
    }
}
