using System.Collections;
using System.Collections.Generic; using UnityEngine.SceneManagement;
using UnityEngine;

public class RoundHandler : MonoBehaviour
{
    [Header("Blocks")]
    public List<GameObject> tetrisBlocks;
    GameObject currentObject,
               nextObject;
    public GameObject nextHolder;
    public GameObject heldObjectHolder;
    GameObject heldObject;

    bool isActive = true;
    
    public bool canSwap = true;
    bool particles = true;

    [SerializeField] TMPro.TMP_Text pointsCount,
                                    linesCount;

    [SerializeField] GameObject panel;
    [SerializeField] TMPro.TMP_Text panelText;
    [SerializeField] TMPro.TMP_Text previousBestText;

    [Header("Particles")]
    [SerializeField] GameObject particleObj;

    internal int points, lines;

    // Start is called before the first frame update
    void Start()
    {
        currentObject = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], transform.position, Quaternion.identity);

        nextObject = Instantiate(tetrisBlocks[Random.Range(0, tetrisBlocks.Count)], nextHolder.transform.position, Quaternion.identity);
        nextObject.GetComponent<TetrisBlock>().enabled = false;

        previousBestText.text = "Previous Best: \n Lines : " + PlayerPrefs.GetInt("bestLines").ToString() + "\nPoints : " + PlayerPrefs.GetInt("bestPoints").ToString();
    }

    private void Update()
    {
        pointsCount.text = "Points : " + points.ToString();
        linesCount.text = "Lines : " + lines.ToString();

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canSwap)
        {
            if (heldObject == null)
            {
                heldObject = Instantiate(currentObject, heldObjectHolder.transform.position, Quaternion.identity);
                heldObject.GetComponent<TetrisBlock>().enabled = false;

                Destroy(currentObject);

                currentObject = Instantiate(nextObject, transform.position, Quaternion.identity);
                currentObject.GetComponent<TetrisBlock>().enabled = true;

                Destroy(nextObject);

                GameObject obj = tetrisBlocks[Random.Range(0, tetrisBlocks.Count)];

                nextObject = Instantiate(obj, nextHolder.transform.position, obj.transform.rotation);
                nextObject.GetComponent<TetrisBlock>().enabled = false;
            }
            else
            {
                GameObject temp;

                temp = currentObject;

                currentObject = Instantiate(heldObject, transform.position, Quaternion.identity);
                currentObject.GetComponent<TetrisBlock>().enabled = true;

                Destroy(heldObject);

                heldObject = Instantiate(temp, heldObjectHolder.transform.position, Quaternion.identity);
                heldObject.GetComponent<TetrisBlock>().enabled = false;

                Destroy(temp);
            }

            canSwap = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            particles = !particles;
            particleObj.SetActive(particles);
        }
    }

    public void SpawnNew()
    {
        if (!isActive) return;

        currentObject = Instantiate(nextObject, transform.position, Quaternion.identity);
        currentObject.GetComponent<TetrisBlock>().enabled = true;

        Destroy(nextObject);

        GameObject obj = tetrisBlocks[Random.Range(0, tetrisBlocks.Count)];

        nextObject = Instantiate(obj, nextHolder.transform.position, obj.transform.rotation);
        nextObject.GetComponent<TetrisBlock>().enabled = false;
    }

    public void EndGame()
    {
        panel.gameObject.SetActive(true);
        panelText.text = "Points : " + points.ToString() + "\nLines : " + lines.ToString();

        PlayerPrefs.SetInt("bestPoints", Mathf.Max(PlayerPrefs.GetInt("bestPoints"), points));
        PlayerPrefs.SetInt("bestLines", Mathf.Max(PlayerPrefs.GetInt("bestLines"), lines));

        isActive = false;

        Destroy(currentObject);
        Destroy(nextObject);
    }

    public void StopTime(bool stop)
    {
        Time.timeScale = (stop ? 0 : 1);
    }
}
