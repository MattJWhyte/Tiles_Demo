using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Element : MonoBehaviour
{
    bool shatter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (this.tag == "Glass")
        {
            shatter = (Random.value > 0.5f);
        }
        else if (this.tag == "Fire")
        {
            SceneManager.LoadScene("End Game", LoadSceneMode.Additive);
            Debug.Log("end");
        }
        if (shatter)
        {
           // Destroy(this.gameObject);
        }
    }
}
