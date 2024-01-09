using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class Menu : MonoBehaviour
{
    private GameObject[] subMenuReferences;

    public AudioMixer audioMixer;
    private Animator anim;
    public GameObject highScore;
    private Text highScoreText;

    public GameObject Easy;
    public GameObject Normal;
    public GameObject Hard;
    private Slider slider;
    private Toggle fullscreen;
    public GameObject lowRes;
    public GameObject medRes;
    public GameObject highRes;
    private bool flipRight;

    private GameObject birdHolderObj;

    private void Start()
    {
        // needs to happen after GameLogic awake()
        switch (GameLogic.instance.gameDifficulty)
        {
            // the default respawn in "M"
            case "E":
                Easy.GetComponent<Toggle>().isOn = true;
                Normal.GetComponent<Toggle>().isOn = false;
                Hard.GetComponent<Toggle>().isOn = false;
                break;
            case "M":
                Easy.GetComponent<Toggle>().isOn = false;
                Normal.GetComponent<Toggle>().isOn = true;
                Hard.GetComponent<Toggle>().isOn = false;
                break;
            case "H":
                Easy.GetComponent<Toggle>().isOn = false;
                Normal.GetComponent<Toggle>().isOn = false;
                Hard.GetComponent<Toggle>().isOn = true;
                break;
        }

        switch (GameLogic.instance.resolutionQuality)
        {
            case 4:
                lowRes.GetComponent<Toggle>().isOn = false;
                medRes.GetComponent<Toggle>().isOn = true;
                highRes.GetComponent<Toggle>().isOn = false;
                break;
            case 1:
                lowRes.GetComponent<Toggle>().isOn = true;
                medRes.GetComponent<Toggle>().isOn = false;
                highRes.GetComponent<Toggle>().isOn = false;
                break;
            case 5:
                lowRes.GetComponent<Toggle>().isOn = false;
                medRes.GetComponent<Toggle>().isOn = false;
                highRes.GetComponent<Toggle>().isOn = true;
                break;
        }

        BirdHolder birdHolder = GameObject.FindObjectOfType<BirdHolder>();
        GameObject bird = Instantiate(birdHolder.birdReference[birdHolder.index]);
        bird.transform.SetParent(birdHolder.transform);
        bird.transform.position = new Vector3(-6.95f, 1.65f, 8647.78f);
        
        highScore = GameObject.Find("High Score");
        if (GameLogic.instance.gamesPlayed == 0) highScore.SetActive(false);
        else
        {
            highScoreText = GameObject.FindObjectOfType<Text>();
            highScoreText.text = GameLogic.instance.highScore.ToString();
        }

        audioMixer.SetFloat("volume", GameLogic.instance.volume);
        anim = GameObject.Find("Menu Manager").GetComponent<Animator>();

        flipRight = true;
        subMenuReferences = new GameObject[] 
        { GameObject.Find("Menu"), GameObject.Find("HelpMenu"), GameObject.Find("OptionsMenu"), GameObject.Find("Store")};
        birdHolderObj = GameObject.Find("Bird Holder");

        slider = GameObject.FindObjectOfType<Slider>();
        slider.value = GameLogic.instance.volume;
        for (int i=1; i<subMenuReferences.Length; i++) subMenuReferences[i].SetActive(false);
    }

    public void SetVolume(float volume)
    {
        GameLogic.instance.volume = slider.value; // volume is in the slider's units, not decibals
        audioMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat(GameLogic.instance.PLAYER_PREF_VOLUME, GameLogic.instance.volume);
    }

    public void SetQuality(int qualityIndex)
    {
        GameLogic.instance.resolutionQuality = qualityIndex;
        PlayerPrefs.SetInt(GameLogic.instance.PLAYER_PREF_RESOLUTION, qualityIndex);
        QualitySettings.SetQualityLevel(qualityIndex, false);
    }

    public void PlayGame()
    {
        // allows you to go to play scence
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void EasyMode()
    {
        GameLogic.instance.gameDifficulty = "E";
        GameLogic.instance.scoreInc = 1;
        PlayerPrefs.SetString(GameLogic.instance.PLAYER_PREF_DIFFICULTY, "E");
    }

    public void MediumMode()
    {
        GameLogic.instance.gameDifficulty = "M";
        GameLogic.instance.scoreInc = 2;
        PlayerPrefs.SetString(GameLogic.instance.PLAYER_PREF_DIFFICULTY, "M");
    }

    public void HardMode()
    {
        GameLogic.instance.gameDifficulty = "H";
        GameLogic.instance.scoreInc = 3;
        PlayerPrefs.SetString(GameLogic.instance.PLAYER_PREF_DIFFICULTY, "H");
    }

    public void PageFlip(string menuType)
    {
        if (!flipRight)
        {
            anim.SetFloat("Speed", -1);
            anim.SetBool("Page Turn", false);
        }
        else
        {
            anim.SetFloat("Speed", 1);
            anim.SetBool("Page Turn", true);
        }
        flipRight = !flipRight;

        switch (menuType)
        {
            case "HelpMenu":
                anim.SetInteger("Page Num", 0);
                break;
            case "OptionsMenu":
                anim.SetInteger("Page Num", 1);
                break;
            case "Store":
                anim.SetInteger("Page Num", 2);
                break;
            case "Menu":
                anim.SetInteger("Page Num", -1);
                break;
        }
        StartCoroutine(SetButtonsActive(menuType));
    }

    private IEnumerator SetButtonsActive(string menuType)
    {
        if (menuType != "Menu")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }

        yield return new WaitForSeconds(0.45f);
        if (menuType == "Menu")
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
            birdHolderObj.SetActive(true);
            yield break;
        }
        
        foreach (GameObject obj in subMenuReferences)
        {
            if (obj.name == menuType)
            {
                obj.SetActive(true);
                break;
            }
        }
    }
}