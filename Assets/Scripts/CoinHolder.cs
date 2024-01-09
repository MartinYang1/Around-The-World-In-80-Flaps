using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinHolder : MonoBehaviour
{
    [SerializeField]
    private GameObject[] coinReference;
    private List<Coin> coins = new List<Coin>();

    public IEnumerator Spawner()
    {
        while (!GameLogic.instance.gameEnd)
        {
            yield return new WaitForSeconds(5);
            
            GameObject coinObj = Instantiate(coinReference[0]);
            Coin coin = coinObj.GetComponent<Coin>();
            coins.Add(coin);

            int topCoin = Random.Range(0, 2);
            List<Pipe> pipes = GameLogic.instance.gameUI.pipeHolder.GetPipes();
            
            int pipeIndex = pipes.Count - 2 + topCoin;
            float coinX = pipes[pipeIndex].transform.position.x + 2.5f;
            float coinY;
            if (topCoin == 1) coinY = pipes[pipeIndex].transform.position.y - pipes[pipeIndex].GetHeight()/2 + 1.5f;
            else coinY = pipes[pipeIndex].transform.position.y + pipes[pipeIndex].GetHeight()/2 - 1;

            coin.SpawnCoin(coinX, coinY);  
        }
    }

    public void Move()
    {
        if (coins.Count > 0 && coins[0].transform.position.x < -3.355776 - GameLogic.instance.gameUI.Bg.GetBgWidth())
        {
            Destroy(coins[0].gameObject);
            coins.RemoveAt(0);
        }

        foreach (Coin coin in coins)
        {
            coin.Move();
        }
    }

    public List<Coin> GetCoins()
    {
        return coins;
    }
}
