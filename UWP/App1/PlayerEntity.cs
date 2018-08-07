using Microsoft.WindowsAzure.Storage.Table;

public class PlayerEntity : TableEntity
{
    public PlayerEntity(string playerName, string playerPassword)
    {
        this.playerName = playerName;
        this.playerPassword = playerPassword;
        this.PartitionKey = playerName;
        this.RowKey = playerName;
        this.history = "";
        this.score = 0;

    }

    public PlayerEntity() { }

    public string playerName { get; set; }

    public string playerPassword { get; set; }

    public string history { get; set; }

    public int score { get; set; }
}