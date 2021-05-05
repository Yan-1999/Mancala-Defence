/// <summary>
/// Author: MjuTeX
/// Project: Mancala Defence
/// File: Map.cs
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Cell[] Cells;
    public Arrow arrow;

    void Awake()
    {
        Cells = new Cell[transform.childCount];
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Cells[i] = transform.GetChild(i).gameObject.GetComponent<Cell>();
        }
        Arrow.StartFirst(arrow);
    }
    public void MancalaDirection()
    {
        Arrow.StartFirst(arrow);
    }
}
