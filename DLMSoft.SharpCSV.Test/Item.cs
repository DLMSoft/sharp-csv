namespace DLMSoft.SharpCSV.Test;

class Item {
    [CSVField("ITEM_ID")]
    public string ItemID { get; set; } = null!;
    
    [CSVField("ICON_INDEX")]
    public int IconIndex { get; set; }
    
    [CSVField("TYPE")]
    public string Type { get; set; } = null!;
    
    [CSVField("RARE_LEVEL")]
    public int RareLevel { get; set; }
    
    [CSVField("PRICE")]
    public int Price { get; set; }
    
    [CSVField("SCORE")]
    public int Score { get; set; }
    
    [CSVField("EFFECT_IDS")]
    [CSVConverter(typeof(EffectIDsConverter))]
    public string[] EffectIDs { get; set; } = [];
}