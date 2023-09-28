
using CSharpAspNet.Client;

var requestHandler = new RequestHandler(new Client("https://localhost:7047"));

Console.WriteLine("Instructions:");
Console.WriteLine("1. Create a new movie: createMovie <title> <director> <release date (yyyy-mm-dd)> <movie categories: category1, category2...>");
Console.WriteLine("2. Create a new category: createCategory <category name>");
Console.WriteLine("3. Get an existing movie: getMovie <id>");
Console.WriteLine("4. Get movies By Category: getMovieByCategory <category name>");
Console.WriteLine("5. Get an existing category: getCategory <id>");
Console.WriteLine("6. Get all existing movies: getAllMovies");
Console.WriteLine("7. Get all existing categories: getAllCategories");
Console.WriteLine("8. Update an existing movie: updateMovie <id> <title> <director> <release date (yyyy-mm-dd)> <movie categories: category1, category2...>");
Console.WriteLine("9. Update an existing category: updateCategory <id> <category name>");
Console.WriteLine("10. Delete an existing movie: deleteMovie <id>");
Console.WriteLine("11. Delete an existing category: deleteCategory <id>");
Console.WriteLine("12. Exit: exit");
Console.WriteLine();

while (true)
{
    Console.Write("Enter what you need: ");
    var input = Console.ReadLine();

    if (!int.TryParse(input, out var instruction) || !Enum.IsDefined(typeof(ClientRequest), instruction))
    {
        Console.WriteLine("Invalid input");
        continue;
    }
    
    var result = await requestHandler.HandleAsync((ClientRequest)instruction);
    if (!result)
        break;
}