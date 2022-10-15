using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameInfo
{
    public bool more_lifes_unlocked { get; set; }
    public uint xp { get; set; }
    public ushort lifes { get; set; }
    public ushort lvl_reached { get; set;}
    public List<double> time_needed { get; set; }
    public uint deaths { get; set; }
}