using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{
    //https://www.youtube.com/watch?v=T5P8ohdxDjo&t=28s

    [Header("Parameters")]
    public static int height = 18;
    public static int width = 10;

    [Header("Variables")]
    [SerializeField] public Vector3 rotationPoint;
    [SerializeField] public float fallSpeed = 0.8f;
    internal float previousTime = 0f;
    internal float previousTimePressed;
    internal float delayPressTime = 0.1f;
    internal bool isPlaced = false;

    // Update is called once per frame
    void Update()
    {
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

        if (Time.time - previousTime > ((Mathf.Sign(Input.GetAxisRaw("Vertical")) == -1) ? fallSpeed / 10 : fallSpeed))
        {
            transform.position += new Vector3(0, -1, 0);

            if (!canMove())
            {
                transform.position += new Vector3(0f, 1f, 0f);
                isPlaced = true;
                FindObjectOfType<RoundHandler>().SpawnNew();
                enabled = false;
            }

            previousTime = Time.time;
        }
    }

    bool canMove()
    {
        foreach (Transform blockPiece in transform)
        {
            int roundedX = Mathf.RoundToInt(blockPiece.transform.position.x);
            int roundedY = Mathf.RoundToInt(blockPiece.transform.position.y);

            if (roundedX < 0 || roundedX >= width || roundedY < 0 || roundedY >= height)
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
