using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameEvent
{
    public ushort tag { get; set; }
    public System.DateTime timestamp { get; set; }
    public int order_id { get; set; }
    public bool update_cache { get; set; }
    public System.Object value { get; set; }

    public GameEvent(ushort tag, System.DateTime timestamp,bool update_cache, System.Object value = null)
    {
        this.tag = tag;
        this.timestamp = timestamp;
        this.order_id = order_id;
        this.update_cache = update_cache;
        this.value = value;
    }
}
