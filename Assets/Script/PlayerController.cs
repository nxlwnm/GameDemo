using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Difficulty
{
    Simple,
    Diffcult,
}


public class PlayerController : MonoBehaviour
{
    public Difficulty difficulty = Difficulty.Simple;

	public bool MakeDecision(int[] cheeseBoard, out int pos)
    {
		if (difficulty != Difficulty.Simple)
		{
			List<int> valid = new List<int>();
			for (int i = 0; i < cheeseBoard.Length; i++)
			{
				if (cheeseBoard[i] == 0)
				{
					valid.Add(i);
				}
			}

			foreach (int candidate in valid)
			{
				int[] new_board = new int[9];

				Array.Copy(cheeseBoard, new_board, 9);

				new_board[candidate] = 2;
				if (CheckIfWin(new_board, 2))
				{
					pos = candidate;
					return true;
				}

				new_board[candidate] = 1;
				if (CheckIfWin(new_board, 1))
				{
					pos = candidate;
					return true;
				}
			}

			if (cheeseBoard[4] == 0)
			{
				pos = 4;
				return true;
			}

			if (cheeseBoard[0] == 0)
			{
				pos = 0;
				return true;
			}

			if (cheeseBoard[2] == 0)
			{
				pos = 2;
				return true;
			}

			if (cheeseBoard[6] == 0)
			{
				pos = 6;
				return true;
			}

			if (cheeseBoard[8] == 0)
			{
				pos = 8;
				return true;
			}

			int index = UnityEngine.Random.Range(0, valid.Count);

			pos = valid[index];
		}
		else
		{
			List<int> valid = new List<int>();
			for (int i = 0; i < cheeseBoard.Length; i++)
			{
				if (cheeseBoard[i] == 0)
				{
					valid.Add(i);
				}
			}

			if (valid.Count == 0)
			{
				pos = -1;
				return false;
			}

			int index = UnityEngine.Random.Range(0, valid.Count);

			pos = valid[index];

		}

        return true;
	}

    private bool CheckIfWin(int[] cheeseBoard, int type)
    {
		for (int i = 0; i < 3; i++)
		{
			if ((cheeseBoard[i * 3 + 0] == cheeseBoard[i * 3 + 1]) && (cheeseBoard[i * 3 + 0] == cheeseBoard[i * 3 + 2]))
			{
				if (cheeseBoard[i * 3 + 0] == type)
				{
					return true;
				}
			}

			if ((cheeseBoard[0 * 3 + i] == cheeseBoard[1 * 3 + i]) && (cheeseBoard[1 * 3 + i] == cheeseBoard[2 * 3 + i]))
			{
				if (cheeseBoard[0 * 3 + i] == type)
				{
					return true;
				}
			}
		}

		if ((cheeseBoard[0] == cheeseBoard[4]) && (cheeseBoard[4] == cheeseBoard[8]))
		{
			if (cheeseBoard[4] == type)
			{
				return true;
			}
		}

		if ((cheeseBoard[2] == cheeseBoard[4]) && (cheeseBoard[4] == cheeseBoard[6]))
		{
			if (cheeseBoard[4] == type)
			{
				return true;
			}
		}

		return false;
	}
}
