using SQLite;

namespace DailyPoetryM.Library.Model;

public class Poetry
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("name")]
    public string Name { get; set; }
    
    [Column("content")]
    public string Content { get; set; }
    
    [Column("author_name")]
    public string Author { get; set; }
    
    [Column("dynasty")]
    public string Dynasty { get; set; }

    private string _snippet;
    public string Snippet =>
    _snippet = Content.Length>50 ? Content.Split('。')[0] : Content;

}