using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextController : MonoBehaviour
{

    public GameObject textComponent;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.SetActive(false);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
}
