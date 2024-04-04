using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cam_detect_ver : MonoBehaviour
{
    public MainCamera cam;
    public float bottom;
    [SerializeField] public float zoom;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cam.Camfollowy(bottom);
            cam.zoom = zoom;
        }
    }
}
