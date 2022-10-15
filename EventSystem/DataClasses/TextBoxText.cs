[System.Serializable]
public class TextBoxText
{
    public int id { get; set; }
    public int previous_id { get; set; }
    public int next_id { get; set; }
    public ushort language { get; set; }
    public int trigger_bubble_id { get; set; }
    public bool end_text_box { get; set; }
    public string text { get; set; }

    public TextBoxText(
        int id, 
        int previous_id, 
        int next_id, 
        ushort language, 
        int trigger_bubble_id, 
        bool end_text_box, 
        string text
    )
    {
        this.id = id;
        this.previous_id = previous_id;
        this.next_id = next_id;
        this.language = language;
        this.trigger_bubble_id = trigger_bubble_id;
        this.end_text_box = end_text_box;
        this.text = text;
    }
}