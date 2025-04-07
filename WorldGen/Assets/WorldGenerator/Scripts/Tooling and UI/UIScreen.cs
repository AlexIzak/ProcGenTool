using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScreen : MonoBehaviour
{
    //public UndergroundGen algorithm;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Canvas>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            gameObject.GetComponent<Canvas>().enabled = true;

            //algorithm.gameObject.SetActive(false);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Close()
    {
        gameObject.GetComponent<Canvas>().enabled = false;

        //algorithm.gameObject.SetActive(true);

        //algorithm.ClearMap();

        //algorithm.Generate();
    }
}
