using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameInfo
{
    public uint xp { get; set; }
    public ushort lives { get; set; }
    public ushort volume { get; set; }
    public ushort language { get; set; } // 0: English, 1: German, ...
}