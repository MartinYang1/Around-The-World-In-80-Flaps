using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public GameLogic gameLogic;
    public Player player;
    public Background Bg;
    public PipeHolder pipeHolder;
    public CoinHolder coinHolder;
    public Menu menu;
    
    private Text scoreText;
    private Text highScoreText;
    private Text coinText;
    public AudioSource audio;

    // Start is called before the first frame update
    private void Awake()
    {
        GameLogic.instance.gameUI = this;
        scoreText = GameObject.Find("Text").GetComponent<Text>();
        highScoreText = GameObject.Find("Text2").GetComponent<Text>();
        audio = GetComponent<AudioSource>();

        GameLogic.instance.gamesPlayed++;
    }

    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        Bg = GameObject.FindObjectOfType<Background>();
        pipeHolder = GameObject.FindObjectOfType<PipeHolder>();
        coinHolder = GameObject.FindObjectOfType<CoinHolder>();
        gameLogic = GameObject.FindObjectOfType<GameLogic>();
        menu = GameObject.FindObjectOfType<Menu>();

        StartCoroutine(Bg.FasterBg());
        StartCoroutine(pipeHolder.SpawnPipes());
        StartCoroutine(pipeHolder.CloserPipes());
        StartCoroutine(coinHolder.Spawner());
        highScoreText.text = "High Score: " + GameLogic.instance.highScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameLogic.instance.gameEnd) StartCoroutine(GameLogic.instance.GameEnd());
        else
        {
            player.Jump();
            player.AnimatePlayer();
            Bg.Move();
            Bg.PlaySound();
            pipeHolder.Move();
            gameLogic.IncreaseScore();
            coinHolder.Move();

            scoreText.text = GameLogic.instance.score.ToString();
        }
    }
}
