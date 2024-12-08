using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

[ExecuteInEditMode]
public class SceneBuilder : MonoBehaviour
{
    private MeshRenderer[] walls;
    private Camera main_camera;

	void Awake()
    {
		main_camera = Camera.main;
		main_camera.transform.position = new(5, 40, -5);
		main_camera.transform.rotation = Quaternion.Euler(75, 0, 0);
		walls = GameObject.Find("Wall").GetComponentsInChildren<MeshRenderer>();

        Assert.IsNotNull(walls);
        Assert.IsTrue(walls.Length == 12);

		for (int i = 0; i < walls.Length; i++)
        {
            var wall = walls[i].transform;
            if (i < 6)
            {
				wall.rotation = Quaternion.identity;
				wall.position = new Vector3((i % 3) * 10 - 5, 1, (i / 3) * 10);
            }
            else
            {
				wall.rotation = Quaternion.Euler(0, 90, 0);
				wall.position = new Vector3(((i - 6) / 3) * 10, 1, ((i - 6) % 3) * 10 - 5);
			}

		}


	}
}
