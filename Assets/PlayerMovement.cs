using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //game vars
    private Rigidbody2D rb;
    int jump;
    float dirX;

    //network vars
    UdpClient udpc;
    IPEndPoint ep;
    byte[] send;
    byte[] rdata;
    string bp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        udpc = new UdpClient("127.0.0.1", 7878);
        ep = null;
    }

    // Update is called once per frame
    void Update()
    {
        // get movement info
        dirX = Input.GetAxis("Horizontal");
        jump = 0;
        if (Input.GetButtonDown("Jump"))
        {
            jump = 1;
        }
        
        // send data
        try
        {
            // Data to send
            send = Encoding.ASCII.GetBytes(jump.ToString()+dirX.ToString());
            udpc.Send(send, send.Length);
            if (udpc.Available > 0)
            {
                // received Data
                rdata = udpc.Receive(ref ep);
                bp = Encoding.ASCII.GetString(rdata);
                Debug.Log(bp);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
