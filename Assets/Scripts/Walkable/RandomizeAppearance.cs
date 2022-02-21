using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeAppearance : MonoBehaviour
{
    [SerializeField] GameObject[] appearances;

    // Start is called before the first frame update
    void Start()
    {
        if(appearances.Length>0)
        {
            for(int i = 0; i < appearances.Length; i++)
            {
                appearances[i].SetActive(false);
            }
            appearances[Random.Range(0,appearances.Length)].SetActive(true);
        }
    }
}
