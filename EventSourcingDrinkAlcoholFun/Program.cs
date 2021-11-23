




var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");

var configuration = builder.Build();
var services = new ServiceCollection();

//services.AddEFServices(configuration);
services.AddDapperServices(configuration);
services.AddEventSourcing(configuration);

var serviceProvider = services.BuildServiceProvider();

var drinkRepository = serviceProvider
    .GetService<IDrinkRepository>();

var ingredientRepository = serviceProvider
    .GetService<IIngredientRepository>();

var tableManager = serviceProvider
    .GetService<ITableManager>();

var eventRepository = serviceProvider
    .GetService<IEventRepository>();

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
    WriteLine("9. Travel in time with a specific drink");
    WriteLine("0. Do an Audit of the event stream on the drinks");

    ForegroundColor = ConsoleColor.Gray;
    WriteLine("d. Demo of Aggreggates");
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
    else if (command == '7')
        await ShowEventsOnTheDrinks();
    else if (command == '8')
        await RestoreDataBaseFromEvents();
    else if (command == '9')
        await TimeTravel();
    else if (command == '0')
    {

    }
    else if (command == 'd')
    {
        var ingidients = await ingredientRepository.GetAllAsync();


        var newdrink = new Drink();
        DrinkAggregate drinkAggregate = new(newdrink);
        drinkAggregate.AddedIngredient(newdrink.UniqueId, ingidients[0]);
        drinkAggregate.RemovedIngredient(newdrink.UniqueId, ingidients[0]);
        drinkAggregate.AddedIngredient(newdrink.UniqueId, ingidients[1]);
        drinkAggregate.ToWhoChanged(newdrink.UniqueId, "Cezary", "");
        drinkAggregate.ToWhoChanged(newdrink.UniqueId, "Daniel", "Cezary");
        drinkAggregate.GlassDone(newdrink.UniqueId);

        var result = await drinkRepository.AddAsync(drinkAggregate.Drink);

        await eventRepository.Save<DrinkAggregate>(drinkAggregate);
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
    var newdrink = new Drink();
    DrinkAggregate drinkAggregate = new(newdrink);

    var result = await repostiory.AddAsync(new Drink());

    await eventRepository.Save<DrinkAggregate>(drinkAggregate);

    WriteLine("You start to create a drink with ID: ");
    //WriteLine(result.Id);
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
            WriteLine("You can't add the same ingredient twice");
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

async Task ShowEventsOnTheDrinks()
{
    var rawevents = await eventRepository.GetRawAllEvents();

    var grouped = rawevents.GroupBy(k => k.Key_StreamId);

    foreach (var g in grouped)
    {
        WriteLine(g.Key);
        WriteLine("======================");
        WriteLine("");
        foreach (var eventTemp in g)
        {
            WriteLine(eventTemp.Value_Data);
        }
        WriteLine("");
        WriteLine("");
        WriteLine("");
    }
    ReadKey();
}


async Task RestoreDataBaseFromEvents()
{
    var rawevents = await eventRepository.GetRawAllEvents();
    var aggregateKeys = rawevents.Distinct().OrderByDescending(a => a.Id).Select
        (k => AggregateKey.FromGuid(Guid.Parse(k.Key_StreamId)));

    foreach (var key in aggregateKeys)
    {
        var drinkAgg = await eventRepository.Get<DrinkAggregate>(key);

        var check = await drinkRepository.GetByUniqueIdAsync(drinkAgg.Drink.UniqueId);

        if (check == null)
        {
            try
            {
                var ing = await ingredientRepository.GetAllAsync();

                List<Ingredient> ingredients = new List<Ingredient>();
                foreach (var item in drinkAgg.Drink.Ingredients)
                {
                    var finded = ing.FirstOrDefault(k => k.Id == item.Id);
                    if (finded != null)
                        ingredients.Add(finded);
                }

                drinkAgg.Drink.Ingredients = ingredients;
                await drinkRepository.AddAsync(drinkAgg.Drink);
                WriteLine($"Added projection {drinkAgg.Drink.UniqueId}");
            }
            catch (Exception ex)
            {
                WriteLine($"Error  {drinkAgg.Drink.UniqueId}");

            }
  
        }
        else
        {
            //WriteLine("Press 'y' if you want to do updated base on projection");
            //if (ReadKey().KeyChar == 'y')
            //{
            //    check.Ingredients = drinkAgg.Drink.Ingredients;
            //    check.ToWho = drinkAgg.Drink.ToWho;
            //    check.Status = drinkAgg.Drink.Status;

            //    await drinkRepository.UpdateAsync(drinkAgg.Drink);
            //    WriteLine($"Updated projection {drinkAgg.Drink.UniqueId}");
            //}
        }
    }
    ReadKey();

}

async Task TimeTravel()
{
    var rawevents = await eventRepository.GetRawAllEvents();
    var aggregateKeys = rawevents.Distinct().Select
        (k => AggregateKey.FromGuid(Guid.Parse(k.Key_StreamId))).ToList();

    WriteLine($"Choose Key");

    for (int i = 0; i < aggregateKeys.Count(); i++)
    {
        WriteLine($"{i} {aggregateKeys[i].Id}");
    }

    string r = ReadLine();

    if (int.TryParse(r, out int choosenIndex))
    {
        var key = aggregateKeys[choosenIndex];

        var drinkAgg =
            await eventRepository.Get<DrinkAggregate>(key);
        WriteLine("======================");
        WriteLine("How many steps do you want to go back");
        WriteLine("");

        int AA = 0;

        WriteLine($"You have {drinkAgg.Changes.Count()} event");
        WriteLine("");

        for (int i = drinkAgg.Changes.Count - 1; i >= 0; i--)
        {
            AA = AA + 1;
            var change = drinkAgg.Changes[i];
            WriteLine($"Step {AA} to remove {change.GetType().Name} : SerialNoVersion -> {change.Version_SerialNumber}");
        }

        string r2 = ReadLine();

        if (int.TryParse(r2, out int choosenNumberOfSteps))
        {
            drinkAgg.ReverseEventTimeTravel(choosenNumberOfSteps);

            var check = await drinkRepository.GetByUniqueIdAsync(drinkAgg.Drink.UniqueId);

            if (check == null)
            {
                await drinkRepository.AddAsync(drinkAgg.Drink);
                WriteLine($"Added projection {drinkAgg.Drink.UniqueId}");
            }
            else
            {
                try
                {
                    check.Ingredients = drinkAgg.Drink.Ingredients;
                    check.ToWho = drinkAgg.Drink.ToWho;
                    check.Status = drinkAgg.Drink.Status;

                    await drinkRepository.UpdateAsync(check);
                    WriteLine($"Updated projection {drinkAgg.Drink.UniqueId}");

                    await eventRepository.Save(drinkAgg);
                    WriteLine($"EventRepository Saved");
                    ReadKey();
                }
                catch (Exception ex)
                {
                    ForegroundColor = ConsoleColor.Red;
                    WriteLine($"{ex.Message}");
                    ForegroundColor = ConsoleColor.Yellow;
                }

            }
        }
        else
            WritePareError();
    }
    else
        WritePareError();


}


void WritePareError()
{
    WriteLine("This isn't a number");
    ReadKey();
}



