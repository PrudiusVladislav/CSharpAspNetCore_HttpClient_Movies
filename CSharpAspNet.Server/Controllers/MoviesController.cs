using CSharpClassLib.SharedModels.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CSharpAspNet.Server.Controllers;

public class MoviesController : Controller
{
    private static readonly IDictionary<int, MovieCategoryDto> Categories = new Dictionary<int, MovieCategoryDto>()
    {
        { 1, new MovieCategoryDto(1, "Action") },
        { 2, new MovieCategoryDto(2, "Comedy") },
        { 3, new MovieCategoryDto(3, "Drama") },
        { 4, new MovieCategoryDto(4, "Adventure") },
        { 5, new MovieCategoryDto(5, "Sci-Fi") },
        { 6, new MovieCategoryDto(6, "Romance") },
        { 7, new MovieCategoryDto(7, "Thriller") },
        { 8, new MovieCategoryDto(8, "Horror") }
    };

    private static readonly IDictionary<int, MovieDto> Movies = new Dictionary<int, MovieDto>()
    {
        {
            1, new MovieDto(1, "The Dark Knight", "Christopher Nolan", new DateOnly(2008, 7, 18), 
                new Dictionary<int, string>(){ {1, Categories[1].CategoryName}, {4, Categories[4].CategoryName}, {7, Categories[7].CategoryName} })
        },
        {
            2, new MovieDto(2, "Inception", "Christopher Nolan", new DateOnly(2010, 7, 16), 
                new Dictionary<int, string> { {1, Categories[1].CategoryName}, {5, Categories[5].CategoryName}, {7, Categories[7].CategoryName} })
        },
        {
            3, new MovieDto(3, "The Shawshank Redemption", "Frank Darabont", new DateOnly(1994, 9, 23), 
                new Dictionary<int, string> { {3, Categories[3].CategoryName}, {6, Categories[6].CategoryName} })
        },
        {
            4, new MovieDto(4, "Pulp Fiction", "Quentin Tarantino", new DateOnly(1994, 10, 14), 
                new Dictionary<int, string> { {2, Categories[2].CategoryName}, {7, Categories[7].CategoryName} })
        },
        {
            5, new MovieDto(5, "Forrest Gump", "Robert Zemeckis", new DateOnly(1994, 7, 6), 
                new Dictionary<int, string> { {6, Categories[6].CategoryName} })
        },
        {
            6, new MovieDto(6, "Avatar", "James Cameron", new DateOnly(2009, 12, 18), 
                new Dictionary<int, string> { {4, Categories[4].CategoryName}, {5, Categories[5].CategoryName} })
        },
        {
            7, new MovieDto(7, "The Silence of the Lambs", "Jonathan Demme", new DateOnly(1991, 2, 14), 
                new Dictionary<int, string> { {5, Categories[5].CategoryName}, {7, Categories[7].CategoryName}, {8, Categories[8].CategoryName}})
        },
        {
            8, new MovieDto(8, "The Exorcist", "William Friedkin", new DateOnly(1973, 12, 26), 
                new Dictionary<int, string> { {8, Categories[8].CategoryName} })
        }
    };
    
    
    public MoviesController(ILogger<MoviesController> logger) : base(logger)
    {
    }
    
    [HttpGet]
    public IActionResult GetAllMovies()
    {
        return Ok(Movies.Values);
    }
    
    [HttpGet("categories")]
    public IActionResult GetAllCategories()
    {
        return Ok(Categories.Values);
    }
    
    [HttpGet("search")]
    public IActionResult GetMoviesByCategory([FromQuery] string categoryName)
    {
        var moviesInCategory = Movies.Values
            .Where(movie => movie.Categories.ContainsValue(categoryName))
            .ToList();

        if (moviesInCategory.Count == 0)
            return NotFound();

        return Ok(moviesInCategory);
    }
    
    [HttpGet("{id:int}")]
    public IActionResult GetMovieById([FromRoute] int id)
    {
        if (!Movies.TryGetValue(id, out var movie))
            return NotFound();

        return Ok(movie);
    }
    
    [HttpGet("categories/{id:int}")]
    public IActionResult GetCategoryById([FromRoute] int id)
    {
        if (!Categories.TryGetValue(id, out var category))
            return NotFound();

        return Ok(category);
    }
    
    [HttpPost]
    public IActionResult CreateMovie([FromBody] CreateMovieDto movie)
    {
        var movieToAdd = new MovieDto(Movies.Keys.Max() + 1, movie.Title, movie.Director, movie.ReleaseDate, movie.Categories);
        Movies.Add(movieToAdd.Id, movieToAdd);
        return Created($"/api/movies/{movieToAdd.Id}", movieToAdd);
    }
    
    [HttpPost("categories")]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto category)
    {
        var categoryToAdd = new MovieCategoryDto(Categories.Keys.Max() + 1, category.CategoryName);
        Categories.Add(categoryToAdd.Id, categoryToAdd);
        return Created($"/api/movies/categories/{categoryToAdd.Id}", categoryToAdd);
    }
    
    [HttpPut("{id:int}")]
    public IActionResult UpdateMovie([FromRoute] int id, [FromBody] MovieDto movie)
    {
        if (!Movies.ContainsKey(id))
            return NotFound();

        Movies[id] = movie;
        return Ok();
    }
    
    [HttpPut("categories/{id:int}")]
    public IActionResult UpdateCategory([FromRoute] int id, [FromBody] MovieCategoryDto category)
    {
        if (!Categories.ContainsKey(id))
            return NotFound();

        foreach (var movie in Movies.Values)
        {
            if (movie.Categories.ContainsKey(id))
                movie.Categories[id] = category.CategoryName;
        }
        Categories[id] = category;
        return Ok();
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult DeleteMovie([FromRoute] int id)
    {
        if (!Movies.ContainsKey(id))
            return NotFound();

        Movies.Remove(id);
        return Ok();
    }
    
    [HttpDelete("categories/{id:int}")]
    public IActionResult DeleteCategory([FromRoute] int id)
    {
        if (!Categories.ContainsKey(id))
            return NotFound();

        foreach (var movie in Movies.Values)
        {
            movie.Categories.Remove(id);
        }

        Categories.Remove(id);
        return Ok();
    }
}