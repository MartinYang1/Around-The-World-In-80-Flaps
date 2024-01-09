using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float BgSpeed;
    public float BgInitialSpeed;
    private float BgSpeedCap, scaleFactor, numLevelIncrease;

    [SerializeField]
    private List<GameObject> backgrounds;
    private float BG_WIDTH;
    private float BG_HEIGHT;
    private float SPAWN_POS;
    private AudioSource audioSource;
    private bool soundPlayed;

    [HideInInspector]
    public int BgCount;

    private void Awake()
    {
        BG_WIDTH = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
        BG_HEIGHT = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;
        SPAWN_POS = BG_WIDTH * (backgrounds.Count - 1) - 5;
        BgCount = 1;
        soundPlayed = false;
        BgInitialSpeed = BgSpeed;

        switch (GameLogic.instance.gameDifficulty)
        {
            case "E":
                BgSpeedCap = 5;
                scaleFactor = 0.01f;
                break;
            case "M":
                BgSpeedCap = 6;
                scaleFactor = 0.015f;
                break;
            case "H":
                BgSpeedCap = 7;
                scaleFactor = 0.02f;
                break;
        }
    }

    public void PlaySound()
    {
        if (soundPlayed) return;

        audioSource = backgrounds[0].GetComponent<AudioSource>();
        if (!audioSource) return;
        audioSource.Play();
        soundPlayed = true;
    }

    public void Move()
    {
        if (backgrounds[0].transform.position.x < -3.355776 - BG_WIDTH)
        {
            GameObject Bg = Instantiate(backgrounds[0].gameObject);
            Destroy(backgrounds[0].gameObject);
            backgrounds.RemoveAt(0);
            Bg.transform.position = new Vector3(SPAWN_POS, 0.19f, 1972.907f);
            backgrounds.Add(Bg);           
            soundPlayed = false;

            if (BgCount >= 15)
            {
                BgCount = 0;
                GameLogic.instance.gameUI.pipeHolder.SetIndex(0);
            }
            else BgCount++;
        }

        for (int i=0; i<backgrounds.Count; i++) backgrounds[i].transform.position -= new Vector3(BgSpeed, 0, 0) * Time.deltaTime;
        
    }

    public IEnumerator FasterBg()
    {
        while (!GameLogic.instance.gameEnd)
        {
            yield return new WaitForSeconds(GameLogic.instance.LEVEL_CHANGE_DELAY);
            if (BgSpeed >= 5) GameLogic.instance.gameUI.pipeHolder.SetRNGRemoved(true);
            if (BgSpeed >= BgSpeedCap) yield break;
            
            BgSpeed = (float)(scaleFactor * Math.Pow(Convert.ToDouble(numLevelIncrease)/3, 2)) + BgInitialSpeed;
            numLevelIncrease++;
        }
    }

    public float GetBgWidth()
    {
        return BG_WIDTH;
    }

    public float GetBgHeight()
    {
        return BG_HEIGHT;
    }

    public float GetBgSpeed()
    {
        return BgSpeed;
    }

    public List<GameObject> GetBgs()
    {
        return backgrounds;
    }
}
