using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float cam_speed;
    private float cam_x;
    private float cam_y;
    private Vector3 velocity = Vector3.zero;
    public PlayerControl stats;
    public DialogueManager manager; 
    private int player_facing;
    [SerializeField] public float zoom;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 1 && !manager.talking)
        {
            player_facing = 1;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 &&¡@!manager.talking)
        {
            player_facing = -1;
        }
    }

    public void Camfollowx()
    {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(cam_x + player_facing * 1.5f, transform.position.y, zoom), ref velocity, cam_speed * Time.deltaTime);
        cam_x = stats.transform.position.x;
    }
    public void Camfollowy(float bottom)
    {
        if(cam_y <= bottom)
        {
            cam_y = bottom;
        }
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(transform.position.x, cam_y, zoom), ref velocity, cam_speed * Time.deltaTime*2);
        cam_y = stats.transform.position.y;
    }

    public void CamPos(Vector3 pos)
    {
        transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, cam_speed * Time.deltaTime);
    }

}
