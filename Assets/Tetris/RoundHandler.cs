using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [Header("Blocks")]
    public List<GameObject> tetrisBlocks;
    GameObject currentObject,
               nextObject;
    public GameObject nextHolder;

    // Start is called before the first frame update
    void Start()
    {
        currentObject = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], transform.position, Quaternion.identity);

        nextObject = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], nextHolder.transform.position, Quaternion.identity);
        nextObject.GetComponent<TetrisBlock>().enabled = false;
    }

    public void SpawnNew()
    {
        currentObject = Instantiate(nextObject, transform.position, Quaternion.identity);
        currentObject.GetComponent<TetrisBlock>().enabled = true;

        Destroy(nextObject);

        nextObject = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], nextHolder.transform.position, Quaternion.identity);
        nextObject.GetComponent<TetrisBlock>().enabled = false;
    }

    public void EndGame()
    {
        Destroy(currentObject);
        Destroy(nextObject);
    }
}
