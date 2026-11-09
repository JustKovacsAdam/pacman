using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPelletsCollected : MonoBehaviour
{
    // Start is called before the first frame update
    List <GameObject> pellets = new List<GameObject>();

    void Start()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            pellets.Add(gameObject.transform.GetChild(i).gameObject);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        int count = 0;
        for (int i = 0; i < pellets.Count; i++)
        {
            if (pellets[i].activeSelf)
            {
                count++;
            }
        }
        if (count == 0)
        {
            Debug.Log("Complete");
        }
    }
}
