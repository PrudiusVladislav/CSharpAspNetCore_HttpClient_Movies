
using System.Net.Http.Json;
using CSharpClassLib.SharedModels;
using CSharpClassLib.SharedModels.Dtos;

namespace CSharpAspNet.Client;

public sealed class Client
{
    private readonly HttpClient _httpClient;
    
    public Client(string baseUrl)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };
    }

    public async Task<string> CreateMovieAsync(CreateMovieDto createMovieDto)
    {
        var content = JsonContent.Create(createMovieDto);
        
        var response = await _httpClient.PostAsync("/api/movies", content);
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var movieDto = await response.Content.ReadFromJsonAsync<MovieDto>();
        return $"Movie with id {movieDto?.Id} was created";
    }
    
    public async Task<string> CreateCategoryAsync(CreateCategoryDto createCategoryDto)
    {
        var content = JsonContent.Create(createCategoryDto);
        
        var response = await _httpClient.PostAsync("/api/movies/categories", content);
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var categoryDto = await response.Content.ReadFromJsonAsync<MovieCategoryDto>();
        return $"Movie category with id {categoryDto?.Id} was added";
    }
    
    public async Task<string> GetMovieByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/movies/{id}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var movie = await response.Content.ReadFromJsonAsync<MovieDto>();
        return $"Movie with id {movie?.Id}\n\tTitle: {movie?.Title}, \n\tDirector: {movie?.Director}," +
               $" \n\tRelease Date: {movie?.ReleaseDate}, \n\tCategories: {movie?.Categories.Values.Select(c => $"{c}; ")}) was found";
    }

    public async Task<string> GetMoviesByCategoryAsync(string categoryName)
    {
        var response = await _httpClient.GetAsync($"/api/movies/search?categoryName={Uri.EscapeDataString(categoryName)}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
    
        var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
    
        if (movies == null || movies.Count == 0)
            return $"No movies found in category: {categoryName}";

        var result = movies.Aggregate($"Movies in category '{categoryName}':", (current, movieDto) =>
            current + $"\nMovie with id {movieDto.Id}\n\tTitle: {movieDto.Title}, \n\tDirector: {movieDto.Director}," +
            $"\n\tRelease Date: {movieDto.ReleaseDate}, \n\tCategories: {string.Join(", ", movieDto.Categories.Values)}");
    
        return result;
    }
    
    public async Task<string> GetCategoryByIdAsync(int id)
    {
        var response = await _httpClient.GetAsync($"/api/movies/categories/{id}");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var category = await response.Content.ReadFromJsonAsync<MovieCategoryDto>();
        return $"Category with id {category?.Id} and Name: {category?.CategoryName} was found";
    }
    
    public async Task<string> GetAllMoviesAsync()
    {
        var response = await _httpClient.GetAsync("/api/movies");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var movies = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
        var result = movies?.Aggregate("Movies list:", (current, movieDto) => 
            current + $"\nMovie with id {movieDto?.Id}\n\tTitle: {movieDto?.Title}, \n\tDirector: {movieDto?.Director}," +
            $"\n\tRelease Date: {movieDto?.ReleaseDate}, \n\t{movieDto?.Categories.Values.Aggregate("Categories: ", (current, movieCategory) => current + $"{movieCategory}; ")}");
        return result ?? "There are no movies added yet";
    }

    public async Task<string> GetAllCategoriesAsync()
    {
        var response = await _httpClient.GetAsync("/api/movies/categories");
        if (!response.IsSuccessStatusCode)
            return $"Your request failed with status code {response.StatusCode}";
        
        var categories = await response.Content.ReadFromJsonAsync<List<MovieCategoryDto>>();
        var result = categories?.Aggregate("Movie Categories list:", (current, categoryDto) => 
            current + $"\nCategory with id {categoryDto?.Id} and Name: {categoryDto?.CategoryName}");
        return result ?? "There are no movie categories added yet";
    }
    
    public async Task<string> UpdateMovieAsync(int id, MovieDto movieDtoToUpdate)
    {
        var content = JsonContent.Create(movieDtoToUpdate);
        
        var response = await _httpClient.PutAsync($"/api/movies/{id}", content);
        return response.IsSuccessStatusCode
            ? $"Movie with id {id} was successfully updated"
            : $"Your request failed with status code {response.StatusCode}";
    }
    
    public async Task<string> UpdateCategoryAsync(int id, MovieCategoryDto categoryDtoToUpdate)
    {
        var content = JsonContent.Create(categoryDtoToUpdate);
        
        var response = await _httpClient.PutAsync($"/api/movies/categories/{id}", content);
        return response.IsSuccessStatusCode
            ? $"Movie Category with id {id} was successfully updated"
            : $"Your request failed with status code {response.StatusCode}";
    }
    
    public async Task<string> DeleteMovieAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/movies/{id}");
        return response.IsSuccessStatusCode
            ? $"Movie with id {id} has been deleted deleted"
            : $"Your request failed with status code {response.StatusCode}";
    }

    public async Task<string> DeleteCategoryAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"/api/movies/categories/{id}");
        return response.IsSuccessStatusCode
            ? $"Movie category with id {id} has been deleted"
            : $"Your request failed with status code {response.StatusCode}";
    }
    
    
}
