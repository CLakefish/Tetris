using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [Header("Blocks")]
    public List<GameObject> tetrisBlocks;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], transform.position, Quaternion.identity);
    }

    public void SpawnNew()
    {
        Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], transform.position, Quaternion.identity);
    }
}
