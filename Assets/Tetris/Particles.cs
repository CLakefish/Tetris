using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour
{
    public List<ParticleSystem> particleList;
    bool particles = true;

    // Start is called before the first frame update
    void Start()
    {
        particleList[Random.Range(0, particleList.Count - 1)].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            particles = !particles;

            switch (particles)
            {
                case true:

                    particleList[Random.Range(0, particleList.Count - 1)].gameObject.SetActive(true);

                    break;

                case false:

                    foreach (ParticleSystem p in particleList)
                    {
                        p.gameObject.SetActive(false);
                    }

                    break;
            }
        }
    }
}
