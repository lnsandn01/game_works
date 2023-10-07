[System.Serializable]
public class StringIntEventValue
{
    public string stringvalue { get; set; }
    public int intvalue { get; set; }


    public StringIntEventValue(string stringvalue, int intvalue)
    {
        this.stringvalue = stringvalue;
        this.intvalue = intvalue;
    }
}