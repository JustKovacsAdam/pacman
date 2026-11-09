using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public Text score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Pellet")
        {
            string [] texts = score.text.Split("\n");
            texts[1] = (int.Parse(texts[1]) + 100).ToString("D6");
            score.text = texts[0] + "\n" + texts[1];
        }
    }
}
