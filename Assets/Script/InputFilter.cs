using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputFilter : MonoBehaviour
{
    private bool HasInput = false;
    private int inputPos = -1;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            HasInput = true;

            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var info))
            {
                //Debug.Log(info.point);

                int x = (int)((info.point.x) / 10 + 1);
                int y = 2 - (int)((info.point.z) / 10 + 1);
                if (x < 0 || x > 2 || y < 0 || y > 2)
                {
					HasInput = false;
					return;
                }

				inputPos = y * 3 + x;
				Debug.Log($"{info.point}: ({x}, {y}): {inputPos}");
			}
		}
	}

    public bool GetInput(out int pos)
    {
        pos = inputPos;
        return HasInput;
    }

	public void Accept()
	{
		HasInput = false;
		inputPos = -1;
	}
}
