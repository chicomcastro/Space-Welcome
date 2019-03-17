using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [TextArea(3, 10)]
    public string names;

    public Text rescuedPlayersText;
    public Text welcomingText;

    private Queue playersToRescue = new Queue();

    public static GameController instance;
    public GameObject enemyPrefab;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (string s in names.Split(';'))
            playersToRescue.Enqueue(s);

        InvokeRepeating("SpawnEnemy", 0f, 4f);
    }

    public void RescuePlayer()
    {
        if (playersToRescue.Count > 0)
        {
            rescuedPlayersText.text += playersToRescue.Peek();
            playersToRescue.Dequeue();
        }
    }

    private void SpawnEnemy()
    {
        if (playersToRescue.Count > 0)
            Instantiate(enemyPrefab);
        else
        {
            welcomingText.text = "Welcome to ITABits!";
        }
    }

}
