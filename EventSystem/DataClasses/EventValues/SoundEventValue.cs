[System.Serializable]
public class SoundEventValue
{
    public int id { get; set; }
    public bool fade { get; set; }
    public float fade_time { get; set; }
    public bool activate { get; set; }
    public float end_volume { get; set; }

    public SoundEventValue(int id, bool fade, float fade_time, bool activate, float end_volume)
    {
        this.id = id;
        this.fade = fade;
        this.fade_time = fade_time;
        this.activate = activate;
        this.end_volume = end_volume;
    }
}