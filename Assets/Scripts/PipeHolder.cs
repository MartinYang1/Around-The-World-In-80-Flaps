using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pipeReference;
    private float[,] spawnPosY;
    private int index;
    private List<Pipe> pipes = new List<Pipe>();

    [SerializeField]
    private float gapSize;
    private float spawnDelay;
    private int pipeCount = 0;

    private float gapSizeCap, gapSizeIncr;
    private bool RNGRemove;
    private float prevUpperSpawnY;  // prevUpperSpawnY keeps track of the previous obstacle to remove RNG
    [SerializeField]
    private float TOP_RNG_CAP, BOTTOM_RNG_CAP;

    private void Awake()
    {
        switch (GameLogic.instance.gameDifficulty)
        {
            case "E":
                gapSizeCap = 4f;
                gapSizeIncr = 0.005f;
                break;
            case "M":
                gapSizeCap = 3.9f;
                gapSizeIncr = 0.009f;
                break;
            case "H":
                gapSizeCap = 3.8f;
                gapSizeIncr = 0.013f;
                break;
        }

        spawnPosY = new float[,]
        { { 1.51f, 4.477f}, { 1.55f, 4.61f}, { 1.74f, 4.5f}, { 1.57f, 4.7f}, { 1.54f, 4.4f}, { 1.67f, 4.1f} };
        index = 0;
        RNGRemove = false;
        spawnDelay = 1.4f;
    }

    public void Move()
    {
        if (pipes[0].transform.position.x < -3.355776 - GameLogic.instance.gameUI.Bg.GetBgWidth())
        {
            Destroy(pipes[0].gameObject);
            pipes.RemoveAt(0);
            GameLogic.instance.pipeIndex--;
        }

        for (int i = 0; i < pipes.Count; i++) pipes[i].Move();
    }

    private void SpawnPipe()
    {
        if (GameLogic.instance.gameUI.Bg.BgCount % 3 == 0)
        {
            index = GameLogic.instance.gameUI.Bg.BgCount / 3 * 2;
            if (index >= 2) index += 2;     // because forest bg has 2 obstacles
        }

        if (index < 4)
        {
            if (pipeCount % 3 == 0 || pipeCount % 3 == 1)
            {
                index = 0;
            }
            else
            {
                index = 2;
            }
            pipeCount++;
        }

        GameObject upperPipeObj = Instantiate(pipeReference[index+1]);
        Pipe upperPipe = upperPipeObj.GetComponent<Pipe>();

        float upperSpawnY;
        int spawnYIndex = (index + 1) / 2;
        if (spawnYIndex >= spawnPosY.GetLength(0)) spawnYIndex = spawnPosY.GetLength(0) - 1;
        if (!RNGRemove)
        {
            upperSpawnY = Random.Range(spawnPosY[spawnYIndex, 0], spawnPosY[spawnYIndex, 1]);
        }
        else
        {
            if (prevUpperSpawnY - BOTTOM_RNG_CAP < spawnPosY[spawnYIndex, 0])
                upperSpawnY = Random.Range(spawnPosY[spawnYIndex, 0], prevUpperSpawnY + TOP_RNG_CAP);
            else if (prevUpperSpawnY + TOP_RNG_CAP > spawnPosY[spawnYIndex, 1])
                upperSpawnY = Random.Range(prevUpperSpawnY - BOTTOM_RNG_CAP, spawnPosY[spawnYIndex, 1]);
            else upperSpawnY = Random.Range(prevUpperSpawnY-BOTTOM_RNG_CAP, prevUpperSpawnY+TOP_RNG_CAP);
        }
        prevUpperSpawnY = upperSpawnY;

        upperPipe.Constructor(-13.48f + GameLogic.instance.gameUI.Bg.GetBgWidth() + 5, upperSpawnY);
        GameObject bottomPipeObj = Instantiate(pipeReference[index]);
        Pipe bottomPipe = bottomPipeObj.GetComponent<Pipe>();
        float bottomSpawnY;
        
        if (index == 6 || index == 7)
        {
            // desert obstacle. The anchor points is in the middle for every obstacle except for the upright desert one
            bottomSpawnY = upperSpawnY - upperPipeObj.GetComponent<SpriteRenderer>().bounds.size.y / 2 + 0.35f;
        }
        else
        {
            bottomSpawnY = upperSpawnY - upperPipeObj.GetComponent<SpriteRenderer>().bounds.size.y / 2 - gapSize;
        }
        bottomPipe.Constructor(-13.48f + GameLogic.instance.gameUI.Bg.GetBgWidth() + 5, bottomSpawnY);

        pipes.Add(bottomPipe);
        pipes.Add(upperPipe);
    }

    public IEnumerator SpawnPipes()
    {
        SpawnPipe();
        while (!GameLogic.instance.gameEnd)
        {
            yield return new WaitForSeconds(spawnDelay);
            SpawnPipe();
        }
    }

    public IEnumerator CloserPipes()
    {
        while (!GameLogic.instance.gameEnd)
        {
            yield return new WaitForSeconds(GameLogic.instance.LEVEL_CHANGE_DELAY);
            if (RNGRemove && gapSize > gapSizeCap) yield break;  // when RNG is removed, it means the bg speed increase has reached its cap
            spawnDelay = 1.4f / (GameLogic.instance.gameUI.Bg.GetBgSpeed() / GameLogic.instance.gameUI.Bg.BgInitialSpeed * 0.75f);
            if (gapSize > gapSizeCap) gapSize -= gapSizeIncr;
        }
    }

    public List<Pipe> GetPipes()
    {
        return pipes;
    }

    public void SetIndex(int i)
    {
        index = i;
    }

    public void SetRNGRemoved(bool remove)
    {
        RNGRemove = remove; 
    }
}