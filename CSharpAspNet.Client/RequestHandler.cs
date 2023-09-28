using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CSharpClassLib.SharedModels.Dtos;

namespace CSharpAspNet.Client
{
    public sealed class RequestHandler
    {
        private readonly Client _client;

        public RequestHandler(Client client)
        {
            this._client = client;
        }

        public async Task<bool> HandleAsync(ClientRequest request)
        {
            switch (request)
            {
                case ClientRequest.CreateMovie:
                    return await CreateMovieAsync();
                case ClientRequest.CreateCategory:
                    return await CreateCategoryAsync();
                case ClientRequest.GetMovie:
                    return await GetMovieAsync();
                case ClientRequest.GetMoviesByCategory:
                    return await GetMoviesByCategoryAsync();
                case ClientRequest.GetCategory:
                    return await GetCategoryAsync();
                case ClientRequest.GetAllMovies:
                    return await GetAllMoviesAsync();
                case ClientRequest.GetAllCategories:
                    return await GetAllCategoriesAsync();
                case ClientRequest.UpdateMovie:
                    return await UpdateMovieAsync();
                case ClientRequest.UpdateCategory:
                    return await UpdateCategoryAsync();
                case ClientRequest.DeleteMovie:
                    return await DeleteMovieAsync();
                case ClientRequest.DeleteCategory:
                    return await DeleteCategoryAsync();
                case ClientRequest.Exit:
                    return false;
                default:
                    Console.WriteLine("Invalid request");
                    return true;
            }
        }

        private async Task<bool> CreateMovieAsync()
        {
            Console.WriteLine("Enter movie details:");
            Console.Write("Title: ");
            var title = Console.ReadLine();
            Console.Write("Director: ");
            var director = Console.ReadLine();
            Console.Write("Release Date (yyyy-MM-dd): ");
            var releaseDateStr = Console.ReadLine();
            if (!DateOnly.TryParseExact(releaseDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDate))
            {
                Console.WriteLine("Invalid release date format");
                return true;
            }

            Console.Write("Categories (comma-separated): ");
            var categoriesInput = Console.ReadLine()?.Split(',');

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(director) || categoriesInput == null || categoriesInput.Length == 0)
            {
                Console.WriteLine("Invalid input");
                return true;
            }

            var categories = categoriesInput.Select(cat => cat.Trim()).ToList();
            Dictionary<int, string> categoriesDictionary = categories
                .Select((value, index) => new { Key = index + 1, Value = value })
                .ToDictionary(item => item.Key, item => item.Value);
            
            var message = await _client.CreateMovieAsync(new CreateMovieDto
            {
                Title = title,
                Director = director,
                ReleaseDate = releaseDate,
                Categories = categoriesDictionary
            });

            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> CreateCategoryAsync()
        {
            Console.Write("Enter category name: ");
            var categoryName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Invalid category name");
                return true;
            }

            var message = await _client.CreateCategoryAsync(new CreateCategoryDto
            {
                CategoryName = categoryName
            });

            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> GetMovieAsync()
        {
            Console.Write("Enter movie id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            var message = await _client.GetMovieByIdAsync(id);
            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> GetMoviesByCategoryAsync()
        {
            Console.Write("Enter category name: ");
            var categoryName = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Invalid category name");
                return false;
            }

            var message = await _client.GetMoviesByCategoryAsync(categoryName);
            Console.WriteLine(message);
            return true;
        }
        
        private async Task<bool> GetCategoryAsync()
        {
            Console.Write("Enter category id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            var message = await _client.GetCategoryByIdAsync(id);
            Console.WriteLine(message);
            return true;
        }
        
        private async Task<bool> GetAllMoviesAsync()
        {
            var message = await _client.GetAllMoviesAsync();
            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> GetAllCategoriesAsync()
        {
            var message = await _client.GetAllCategoriesAsync();
            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> UpdateMovieAsync()
        {
            Console.Write("Enter movie id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            Console.WriteLine("Enter updated movie details:");
            Console.Write("Title: ");
            var title = Console.ReadLine();
            Console.Write("Director: ");
            var director = Console.ReadLine();
            Console.Write("Release Date (yyyy-MM-dd): ");
            var releaseDateStr = Console.ReadLine();
            if (!DateOnly.TryParseExact(releaseDateStr, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var releaseDate))
            {
                Console.WriteLine("Invalid release date format");
                return true;
            }

            Console.Write("Categories (comma-separated): ");
            var categoriesInput = Console.ReadLine()?.Split(',');

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(director) || categoriesInput == null || categoriesInput.Length == 0)
            {
                Console.WriteLine("Invalid input");
                return true;
            }

            var categories = categoriesInput.Select(cat => cat.Trim()).ToList();
            Dictionary<int, string> categoriesDictionary = categories
                .Select((value, index) => new { Key = index + 1, Value = value })
                .ToDictionary(item => item.Key, item => item.Value);
            
            var message = await _client.UpdateMovieAsync(id, new MovieDto(id, title, director, releaseDate, categoriesDictionary));

            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> UpdateCategoryAsync()
        {
            Console.Write("Enter category id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            Console.Write("Enter updated category name: ");
            var categoryName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(categoryName))
            {
                Console.WriteLine("Invalid category name");
                return true;
            }

            var message = await _client.UpdateCategoryAsync(id, new MovieCategoryDto(id, categoryName));

            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> DeleteMovieAsync()
        {
            Console.Write("Enter movie id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            var message = await _client.DeleteMovieAsync(id);
            Console.WriteLine(message);
            return true;
        }

        private async Task<bool> DeleteCategoryAsync()
        {
            Console.Write("Enter category id: ");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid id");
                return true;
            }

            var message = await _client.DeleteCategoryAsync(id);
            Console.WriteLine(message);
            return true;
        }
    }
}
