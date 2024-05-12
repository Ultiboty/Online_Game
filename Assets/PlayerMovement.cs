using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    Player player;
    bool connected = false;
    bool is_host = false;
    //network vars
    public string ip = "";
    UdpClient udpc;
    IPEndPoint ep;
    byte[] send;
    byte[] rdata;
    string bp;

    // Start is called before the first frame update
    void Start()
    {

        udpc = new UdpClient("127.0.0.1", 7878);
        ep = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (!connected) 
        {
            if (udpc.Available > 0)
            {
                // received Data
                rdata = udpc.Receive(ref ep);
                player = Deserialize(rdata);
                connected = true;
            }
            else
            {
                send = Encoding.ASCII.GetBytes("hello");
                udpc.Send(send, send.Length);
            }
        } 
        else
        {
            // get movement info
            player.dirX = Input.GetAxis("Horizontal");
            player.jump = 0;
            if (Input.GetButtonDown("Jump"))
            {
                player.jump = 1;
            }

            // Data to send    
            send = Serialize(player);
            udpc.Send(send, send.Length);
            if (udpc.Available > 0)
            {
                // received Data
                rdata = udpc.Receive(ref ep);
                player = Deserialize(rdata);
            }
        }

    }
    static byte[] Serialize(object obj)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(memoryStream, obj);
            return memoryStream.ToArray();
        }
    }
    // Deserialize a byte array into an object
    static Player Deserialize(byte[] data)
    {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            IFormatter formatter = new BinaryFormatter();
            return (Player)formatter.Deserialize(memoryStream);
        }
    }
}