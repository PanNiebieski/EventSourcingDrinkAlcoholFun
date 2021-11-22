var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var services = new ServiceCollection();

services.AddEventSourcingDrinkAlcoholFunEFServices(configuration);
var serviceProvider = services.BuildServiceProvider();

var drinkRepository = serviceProvider
    .GetService<IDrinkRepository>();

var ingredientRepository = serviceProvider
    .GetService<IIngredientRepository>();

var tableManager = serviceProvider
    .GetService<ITableManager>();



char command = 'a';

while (command != 'q')
{
    ForegroundColor = ConsoleColor.White;
    WriteLine("1. See all drinks in the database");
    WriteLine("2. Start the process of creating a Drink");
    WriteLine("3. Add the ingredient to the drink");
    WriteLine("4. Remove the ingredient from the drink");
    WriteLine("5. Complete the process of creating a given drink");
    WriteLine("=========================================");
    ForegroundColor = ConsoleColor.Red;
    WriteLine("6. Clear the database [KAC EFEKT]");
    ForegroundColor = ConsoleColor.Yellow;
    WriteLine("=========================================");
    WriteLine("7. Show me events on na drink");
    WriteLine("8. Projection from the stream of events on the drinks");
    WriteLine("9. Do an Audit of the event stream on the drinks");
    WriteLine("0. Travel in time with a specific drink");
    ForegroundColor = ConsoleColor.Gray;
    WriteLine("q. Exit");

    ForegroundColor = ConsoleColor.Yellow;
    command = ReadKey().KeyChar;
    Clear();

    if (command == '1')
        await ShowAllDrinksAsync(drinkRepository);
    else if (command == '2')
        await CreateDrink(drinkRepository);
    else if (command == '3')
        await AddingIn(drinkRepository, ingredientRepository);
    else if (command == '4')
        await RemoveIn(drinkRepository);
    else if (command == '5')
        await EndDrink(drinkRepository);
    else if (command == '6')
        await ClearDatabase(tableManager);
    else if (command == '6')
    {

    }
    else if (command == '7')
    {

    }
    else if (command == '8')
    {

    }
    else if (command == '9')
    {

    }
    Clear();
}


async Task ShowAllDrinksAsync(IDrinkRepository repostiory)
{
    var list = await repostiory.GetAllAsync();

    if (list.Count == 0)
        WriteLine("There are no drinks");

    foreach (var drink in list)
    {
        var table = new ConsoleTable("Id", "Status", "CreatedAt", "IngredientsCount", "PriceSum");

        table.AddRow(drink.Id, drink.Status, drink.CreatedAt,
            drink.Ingredients.Count(),
            drink.Ingredients.Sum(p => p.Price));


        if (drink.Ingredients.Count > 0)
        {
            table.AddRow("", "", "", "", "");
            table.AddRow("Id", "Name", "Volume", "Price", "");
        }

        foreach (var ingredient in drink.Ingredients)
        {
            table.AddRow(ingredient.Id, ingredient.Name, ingredient.Volume, ingredient.Price, "");
        }
        table.Write(Format.MarkDown);
    }

    ReadKey();
}

async Task CreateDrink(IDrinkRepository repostiory)
{
    var d = await repostiory.AddAsync(new Drink());
    WriteLine("You start to create a drink with ID: ");
    WriteLine(d.Id);
    ReadKey();
}

async Task AddingIn(IDrinkRepository drinkRepository
    , IIngredientRepository  ingredientRepository)
{
    WriteLine("Enter the number of the drink to be modified");
    string s = ReadLine();
    int id;
    if (int.TryParse(s, out id))
    {
        var d = await drinkRepository.GetByIdAsync(id);

        if (d != null && d.Status == DrinkStatus.Creating)
        {
            WriteLine("");
            WriteLine("Here are the available ingredients: ");
            var table1 =
                new ConsoleTable
                ("Id", "Name", "Volume", "Price");

            var ingidients = await ingredientRepository.GetAllAsync();

            foreach (var item in ingidients)
            {
                table1.AddRow(item.Id, item.Name,
                    item.Volume, item.Price);
            }

            table1.Write(Format.Alternative);
            WriteLine("");
            WriteLine("Here are the ingredients already in this drink:");
            WriteLine("");
            var table2 = new ConsoleTable("Id", "Name", "Volume", "Price");

            foreach (var item in d.Ingredients)
            {
                table2.AddRow(item.Id, item.Name,
                    item.Volume, item.Price);
            }

            table2.Write(Format.Alternative);
            WriteLine("");
            WriteLine("What do you want to add");
            WriteLine("Enter an alphabetic character to cancel");

            string inIdstring = ReadLine();


            int inId;
            if (int.TryParse(inIdstring, out inId))
            {
                var ing = ingidients.
                    FirstOrDefault(x => x.Id == inId);

                d.Ingredients.Add(ing);

                await drinkRepository.UpdateAsync(d);
            }
        }
        else
        {
            WriteLine("Drink with this ID does not exist");
            WriteLine("OR");
            WriteLine("The drink is already finished");
            ReadLine();
        }

    }
}


async Task RemoveIn(IDrinkRepository drinkRepository)
{
    WriteLine("Enter the number of the drink to be modified");
    string s = ReadLine();
    int id;
    if (int.TryParse(s, out id))
    {
        var d = await drinkRepository.GetByIdAsync(id);

        if (d != null && d.Status == DrinkStatus.Creating)
        {
            var ingidients = await ingredientRepository.GetAllAsync();
            WriteLine("");
            WriteLine("Here are the ingredients already in this drink:");
            WriteLine("");

            var table2 = new ConsoleTable("Id", "Name", "Volume", "Price");

            foreach (var item in d.Ingredients)
            {
                table2.AddRow(item.Id, item.Name,
                    item.Volume, item.Price);
            }

            table2.Write(Format.Alternative);
            WriteLine("");
            WriteLine("What do you want to remove");
            WriteLine("Enter an alphabetic character to cancel");

            string inIdstring = ReadLine();


            int inId;
            if (int.TryParse(inIdstring, out inId))
            {
                var ing = ingidients.
                    FirstOrDefault(x => x.Id == inId);

                d.Ingredients.Remove(ing);

                await drinkRepository.UpdateAsync(d);
            }
        }

    }
    else
    {
        WriteLine("Drink with this ID does not exist");
        WriteLine("OR");
        WriteLine("The drink is already finished");
        ReadLine();
    }
}

async Task EndDrink(IDrinkRepository drinkRepository)
{
    WriteLine("Enter the number of the drink to be finished");
    string s = ReadLine();
    int id;
    if (int.TryParse(s, out id))
    {
        var d = await drinkRepository.GetByIdAsync(id);

        if (d == null)
        {
            WriteLine("Could not find such a drink with this id");
            ReadLine();
        }

        d.Status = DrinkStatus.Done;
        await drinkRepository.UpdateAsync(d);
    }
}


async Task ClearDatabase(ITableManager tableManager)
{
    WriteLine("Press 'y' if you want that");
    if (ReadKey().KeyChar == 'y')
    {
        await tableManager.DeleteAllRecordsInTables();
    }
}



