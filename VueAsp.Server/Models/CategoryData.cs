using VueAsp.Server.Models;

public class CategoryData
{
    public int Id { get; set; }
    public string CategoryName { get; set; }
    public List<int> Articles { get; set; }
}
