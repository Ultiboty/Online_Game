using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    //game vars
    private Rigidbody2D rb;
    float dirX;

    //network vars
    IDictionary<string, int> Players;
    int counter;
    string address;
    UdpClient udpc;
    IPEndPoint ep;
    byte[] receivedData;
    string received;
    byte[] sdata;

    void Start()
    {
        //Application.targetFrameRate = 1;
        rb = GetComponentInChildren<Rigidbody2D>();
        Players = new Dictionary<string, int>();
        counter = 1;
        udpc = new UdpClient(7878);
        Debug.Log("Server Started and servicing on port no. 7878");
        ep = null;
    }

    
    void Update()
    {
        try
        {
            if (udpc.Available > 0)
            {
                // get data
                receivedData = udpc.Receive(ref ep);
                address = ep.Address.ToString() + " " + ep.Port.ToString();

                // if from new connection, add to dict
                if (Players.ContainsKey(address) == false)
                {
                    Debug.Log("connection from: " + address + "    assigned player number: " + counter);
                    Players.Add(address, counter);
                    counter++;
                }

                //move based on data
                received = Encoding.ASCII.GetString(receivedData);
                if (received[0] == '1')
                    rb.velocity = new Vector2(rb.velocity.x, 13f);
                try
                {
                    dirX = float.Parse(received.Substring(1, received.Length - 1),CultureInfo.InvariantCulture.NumberFormat);
                }
                catch (FormatException)
                {
                    dirX = 0;
                }
                rb.velocity = new Vector2(dirX * 7f, rb.velocity.y);

                // sign and send back
                Debug.Log("received: " + received + "    from player number: " + Players[address]);
                received = received + " server signed, enjoy player number: " + Players[address];
                sdata = Encoding.ASCII.GetBytes(received);
                udpc.Send(sdata, sdata.Length, ep);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
