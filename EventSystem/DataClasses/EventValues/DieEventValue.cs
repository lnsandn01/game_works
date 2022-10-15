using UnityEngine;

[System.Serializable]
class DieEventValue
{
    // true dead, false newly born
    public bool dead_or_alive { get; set; }
    public UnityEngine.GameObject mob {get;set;}

    public DieEventValue(bool dead_or_alive, GameObject mob)
    {
        this.dead_or_alive = dead_or_alive;
        this.mob = mob;
    }
}
