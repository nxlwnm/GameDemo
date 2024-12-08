using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoundState
{
    Init = 0,
    ReayToPlay,
	WaitForPlayerInput,
    WaitForEnemyInput,
    End,
}


public class GameController : MonoBehaviour
{
    private RoundState round = RoundState.Init;
    public GameObject WhitePrefab = null;
    public GameObject BlackPrefab = null;

    private List<GameObject> _white_cheese = new();
    private List<GameObject> _black_cheese = new();

    private int[] cheeseBoard = new int[9];

    private InputFilter _input_filter = null;
    private GameObject _cheese_container = null;
    private PlayerController _enemy = null;

	private int[] _scores = new int[3];
	private int _curScore = 0;

	void Start()
    {
		_input_filter = this.transform.GetComponent<InputFilter>();

        _cheese_container = GameObject.Find("Cheeses");

        _enemy = this.transform.GetComponent<PlayerController>();
	}

    void Update()
    {
        if (!_input_filter)
            return;

        switch (round)
        {
            case RoundState.Init:
                InitGame();
                round = RoundState.WaitForPlayerInput;
				break;
            case RoundState.ReayToPlay:
                Replay();
				RefreshCheese();
				round = RoundState.WaitForPlayerInput;
                break;
            case RoundState.WaitForPlayerInput:
                if (_input_filter.GetInput(out var pos) && pos >= 0 && cheeseBoard[pos] == 0)
                {
                    cheeseBoard[pos] = 1;
                    RefreshCheese();

					round = RoundState.WaitForEnemyInput;
					_input_filter.Accept();

					CheckGameEnd();
				}
                break;
            case RoundState.WaitForEnemyInput:
                if (_enemy.MakeDecision(cheeseBoard, out var enemy_pos))
                {
					cheeseBoard[enemy_pos] = 2;
					RefreshCheese();

					round = RoundState.WaitForPlayerInput;

					CheckGameEnd();
				}
                else
                {
                    // 失败的情况暂不考虑
                }
				break;
            case RoundState.End:
                break;
        }

    }

    public void InitGame()
    {
		if (WhitePrefab != null && BlackPrefab != null)
		{
			for (int i = 0; i < 5; i++)
			{
				var cheese = GameObject.Instantiate(WhitePrefab, _cheese_container.transform);
				cheese.SetActive(false);
				_white_cheese.Add(cheese);
			}

			for (int i = 0; i < 5; i++)
			{
				var cheese = GameObject.Instantiate(BlackPrefab, _cheese_container.transform);
				cheese.SetActive(false);
				_black_cheese.Add(cheese);
			}

		}

		
        for (int i = 0; i < 9; i++)
        {
            cheeseBoard[i] = 0;
		}

        RefreshCheese();
	}

    public void RefreshCheese()
    {
		for (int i = 0; i < 5; i++)
        {
            _white_cheese[i].SetActive(false);
			_black_cheese[i].SetActive(false);
		}

        int _last_white = 0;
        int _last_black = 0;
		for (int i = 0; i < 9;i++)
        {
            if (cheeseBoard[i] == 1)
            {
                if (_last_black >= _black_cheese.Count)
                    return;
                _black_cheese[_last_black].SetActive(true);
				_black_cheese[_last_black].transform.position = new Vector3((i % 3) * 10 - 5, 1,  (2 - (i / 3)) * 10 - 5);
				_last_black++;

			}
            else if (cheeseBoard[i] == 2)
            {
				if (_last_white >= _white_cheese.Count)
					return;
				_white_cheese[_last_white].SetActive(true);
				_white_cheese[_last_white].transform.position = new Vector3((i % 3) * 10 - 5, 1, (2 - (i / 3)) * 10 - 5);
				_last_white++;

			}
        }
    }

	public void Decide(int score)
	{
		_curScore = score;
		_scores[score]++;

		round = RoundState.End;
	}

    public void CheckGameEnd()
    {
        // 1, 连子
        for (int i = 0; i < 3; i++)
        {
            // 横
			if ((cheeseBoard[i * 3 + 0] == cheeseBoard[i * 3 + 1]) && (cheeseBoard[i * 3 + 0] == cheeseBoard[i * 3 + 2]))
            {
                if (cheeseBoard[i * 3 + 0] == 1)
                {
					Decide(1);

				}
                else if (cheeseBoard[i * 3 + 0] == 2)
                {
					Decide(2);
				}
            }

			// 竖
			if ((cheeseBoard[0 * 3 + i] == cheeseBoard[1 * 3 + i]) && (cheeseBoard[1 * 3 + i] == cheeseBoard[2 * 3 + i]))
			{
				if (cheeseBoard[0 * 3 + i] == 1)
				{
					Decide(1);
				}
				else if (cheeseBoard[0 * 3 + i] == 2)
				{
					Decide(2);
				}
			}
		}

		// 
		if ((cheeseBoard[0] == cheeseBoard[4]) && (cheeseBoard[4] == cheeseBoard[8]))
        {
			if (cheeseBoard[4] == 1)
			{
				Decide(1);
			}
			else if (cheeseBoard[4] == 2)
			{
				Decide(2);
			}
		}

		if ((cheeseBoard[2] == cheeseBoard[4]) && (cheeseBoard[4] == cheeseBoard[6]))
		{
			if (cheeseBoard[4] == 1)
			{
				Decide(1);
			}
			else if (cheeseBoard[4] == 2)
			{
				Decide(2);
			}
		}


		// 2，没有空位
		int validCount = 0;
		for (int i = 0; i < 9; i++)
		{
			if (cheeseBoard[i] == 0)
			{
				validCount++;
			}
		}
		if (validCount == 0)
		{
			Decide(0);
		}
	}

    public void Replay()
    {
		for (int i = 0; i < 9; i++)
		{
			cheeseBoard[i] = 0;
		}

		round = RoundState.ReayToPlay;
	}

	public void ResetGame()
	{
		_curScore = 0;
		_scores = new int[3];

		Replay();
	}

	public void OnGUI()
	{
		if (GUI.Button(new Rect(20, 20, 100, 50), "重新开始"))
        {
            Replay();
		}

		if (GUI.Button(new Rect(150, 20, 100, 50), "重置计数"))
		{
			ResetGame();
		}

		GUI.TextArea(new Rect(20, 120, 100, 50), $"当前分数：胜:{_scores[1]} 平:{_scores[0]} 负:{_scores[2]}");

		if (round == RoundState.End)
		{
			if (_curScore == 0)
			{
				GUI.TextField(new Rect(20, 220, 100, 50), "平局！");
			}
			else if (_curScore == 1)
			{
				GUI.TextField(new Rect(20, 220, 100, 50), "你赢了！");
			}
			else if (_curScore == 2)
			{
				GUI.TextField(new Rect(20, 220, 100, 50), "你输了！");
			}
		}

		

	}
}
