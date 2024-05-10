using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Player
{
    public string address { get; set; }
    public int id { get; set; }
    public int jump { get; set; }
    public float dirX { get; set; }
    public float[] position { get; set; }
    public Player(string address, int id)
    {
        this.address = address;
        this.id = id;
        jump = 0;
        dirX = 0;
        position = new float[2] { 0, 0 };
    }
}
