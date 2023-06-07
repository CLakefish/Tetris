using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    //https://www.youtube.com/watch?v=T5P8ohdxDjo&t=28s

    [Header("Parameters")]
    public static int height = 21;
    public static int width = 10;

    [Header("Variables")]
    [SerializeField] public Vector3 rotationPoint;
    [SerializeField] public float fallSpeed = 1f;
    internal float previousTime = 0f;
    internal float previousTimePressed;
    internal float delayPressTime = 0.1f;
    internal bool isPlaced = false;

    int pointCount = 0;

    private static Transform[,] grid = new Transform[width, height];

    // Update is called once per frame
    void Update()
    {
        if (!canMove())
        {
            FindObjectOfType<RoundHandler>().EndGame();
            Debug.Log("done");
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            transform.position += new Vector3(-1f, 0f, 0f);

            if (!canMove())
            {
                transform.position -= new Vector3(-1f, 0f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(1f, 0f, 0f);

            if (!canMove())
            {
                transform.position -= new Vector3(1f, 0f, 0f);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0f, 0f, 1f), 90);

            if (!canMove())
            {
                transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0f, 0f, 1f), -90);
            }
        }

        if (Time.time - previousTime > ((Mathf.Sign(Input.GetAxisRaw("Vertical")) == -1 && Input.GetAxisRaw("Vertical") != 0) ? fallSpeed / 10 : fallSpeed))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!canMove())
            {
                transform.position += new Vector3(0f, 1f, 0f);

                isPlaced = true;

                AddToGrid();

                CheckLine();

                FindObjectOfType<RoundHandler>().SpawnNew();

                enabled = false;
            }

            previousTime = Time.time;
        }
    }

    void CheckLine()
    {
        for (int i = height - 1; i >= 0; i--)
        {
            if (HasLine(i))
            {
                FindObjectOfType<RoundHandler>().lines += 1;
                pointCount += 1;
                DeleteLine(i);
                FixRow(i);
            }
        }

        RoundHandler rounds = FindObjectOfType<RoundHandler>();

        switch (pointCount)
        {
            case 1:
                rounds.points += 40;
                break;

            case 2:
                rounds.points += 100;
                break;

            case 3:
                rounds.points += 300;
                break;

            case 4:
                rounds.points += 1200;
                break;
        }

        pointCount = 0;
    }

    void FixRow(int i)
    {
        for (int h = i; h < height; h++)
        {
            for (int w = 0; w < width; w++)
            {
                if (grid[w, h] != null)
                {
                    grid[w, h - 1] = grid[w, h];
                    grid[w, h] = null;
                    grid[w, h - 1].transform.position -= new Vector3(0, 1, 0);
                }
            }
        }
    }

    void DeleteLine(int i)
    {
        for (int w = 0; w < width; w++)
        {
            Destroy(grid[w, i].gameObject);
            grid[w, i] = null;

        }
    }

    bool HasLine(int j)
    {
        for (int i = 0; i < width; i++)
        {
            if (grid[i, j] == null)
            {
                return  false;
            }
        }

        return true;
    }

    void AddToGrid()
    {
        FindObjectOfType<RoundHandler>().canSwap = true;

        foreach (Transform blockPiece in transform)
        {
            int roundedX = Mathf.RoundToInt(blockPiece.transform.position.x);
            int roundedY = Mathf.RoundToInt(blockPiece.transform.position.y);

            grid[roundedX, roundedY] = blockPiece;
        }
    }

    bool canMove()
    {
        foreach (Transform blockPiece in transform)
        {
            int roundedX = Mathf.RoundToInt(blockPiece.transform.position.x);
            int roundedY = Mathf.RoundToInt(blockPiece.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height || grid[roundedX, roundedY] != null)
            {
                return false;
            }
        }

        return true;
    }

    private void OnDrawGizmos()
    {



        Gizmos.DrawWireSphere(transform.TransformPoint(rotationPoint), 0.5f);
    }
}
