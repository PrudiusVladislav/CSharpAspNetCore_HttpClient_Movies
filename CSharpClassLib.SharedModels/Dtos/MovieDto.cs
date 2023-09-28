
using System.Text.Json.Serialization;

namespace CSharpClassLib.SharedModels.Dtos;


public sealed record MovieDto(int Id, string Title, string Director, DateOnly ReleaseDate,
    Dictionary<int, string> Categories);

public sealed record MovieCategoryDto(int Id, string CategoryName);

public class CreateMovieDto
{
    public string Title { get; set; } 
    public string Director { get; set; }  
    public DateOnly ReleaseDate { get; set; } 
    public Dictionary<int,string> Categories { get; set; }  
}

public class CreateCategoryDto
{
    public string CategoryName { get; set; }
}