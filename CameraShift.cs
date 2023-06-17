using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShift : MonoBehaviour
{
    public Transform UpperCamera;
    public Transform UpperPlayer;

    public Transform LowerCamera;
    public Transform LowerPlayer;

    public bool inUpperWorld = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            inUpperWorld = !inUpperWorld;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (inUpperWorld)
        {
            UpperCamera.gameObject.SetActive(true);
            UpperPlayer.GetComponent<Player>().MakeDynamic();
            UpperPlayer.GetComponent<Player>().enabled = true;

            LowerCamera.gameObject.SetActive(false);
            LowerPlayer.GetComponent<Player>().MakeStatic();
            LowerPlayer.GetComponent<Player>().enabled = false;

        }
        else {
            UpperCamera.gameObject.SetActive(false);
            UpperPlayer.GetComponent<Player>().MakeStatic();
            UpperPlayer.GetComponent<Player>().enabled = false;

            LowerCamera.gameObject.SetActive(true);
            LowerPlayer.GetComponent<Player>().MakeDynamic();
            LowerPlayer.GetComponent<Player>().enabled = true;
        }
    }
}
