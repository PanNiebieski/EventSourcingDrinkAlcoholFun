using ConsoleTables;
using EventSourcingDrinkAlcoholFun.Core;
using EventSourcingDrinkAlcoholFun.Domain;
using EventSourcingDrinkAlcoholFun.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Console;

var builder = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json");
var configuration = builder.Build();
var services = new ServiceCollection();

services.AddEventSourcingDrinkAlcoholFunEFServices(configuration);
var serviceProvider = services.BuildServiceProvider();


var repostiory = serviceProvider.GetService<IDrinkRepository>();
var repostioryIng = serviceProvider.GetService<IIngredientRepository>();




char command = 'q';

while (command != '0')
{
    ForegroundColor = ConsoleColor.White;
    WriteLine("1. Zobacz wszystkie Drinki w bazie");
    WriteLine("2. Rozpocznij proces tworzenia Drinka");
    WriteLine("3. Dodaj do Drinka składnik");
    WriteLine("4. Usuń z Drinka składnik");
    WriteLine("5. Skończ proces tworzenia danego drinka");

    ForegroundColor = ConsoleColor.Red;
    WriteLine("6. Wyczyść bazę danych [KAC EFEKT]");
    ForegroundColor = ConsoleColor.Yellow;
    WriteLine("7. Projekcja z strumienia zdarzeń na kielichach");
    WriteLine("8. Zrób Audyt z strumienia zdarzeń na kieluchach");
    WriteLine("9. Podróż w czasie z konkretnym kielichem");
    ForegroundColor = ConsoleColor.Gray;
    WriteLine("0. Wyjście");


    ForegroundColor = ConsoleColor.Yellow;
    command = ReadKey().KeyChar;
    Console.Clear();
    if (command == '1')
    {
        var list = await repostiory.GetAllAsync();

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
            //table.AddRow("", "", "", "", "");
            table.Write(Format.MarkDown);
        }

        ReadKey();
    }
    else if (command == '2')
    {
        var d = await repostiory.AddAsync(new Drink());
        WriteLine("Zaczynasz tworzyć Drinka o ID:");
        WriteLine(d.Id);
        ReadKey();
    }
    else if (command == '3')
    {
        WriteLine("Podaj numer Drinka do modyfikacji");
        string s = Console.ReadLine();
        int id;
        if (int.TryParse(s, out id))
        {
            var d = await repostiory.GetByIdAsync(id);

            if (d != null && d.Status == DrinkStatus.Creating)
            {
                WriteLine("");
                WriteLine("Oto dostępne składniki:");
                var table1 =
                    new ConsoleTable
                    ("Id", "Name", "Volume", "Price");

                var ingidients = await repostioryIng.GetAllAsync();

                foreach (var item in ingidients)
                {
                    table1.AddRow(item.Id, item.Name,
                        item.Volume, item.Price);
                }

                table1.Write();
                WriteLine("");
                WriteLine("Oto składniki, które już są w tym drinku:");

                var table2 = new ConsoleTable("Id", "Name", "Volume", "Price");

                foreach (var item in d.Ingredients)
                {
                    table2.AddRow(item.Id, item.Name,
                        item.Volume, item.Price);
                }

                table2.Write();
                WriteLine("");
                WriteLine("Co chcesz dodać?");
                WriteLine("Wpisz znak alfabetyczny aby anulować");

                string inIdstring = ReadLine();


                int inId;
                if (int.TryParse(inIdstring, out inId))
                {
                    var ing = ingidients.
                        FirstOrDefault(x => x.Id == inId);

                    d.Ingredients.Add(ing);

                    await repostiory.UpdateAsync(d);
                }
            }
            else
            {
                WriteLine("Drink o tym ID nie istnieje");
                WriteLine("lub");
                WriteLine("Drink już został skończony");
                ReadLine();
            }

        }
    }
    else if (command == '4')
    {
        WriteLine("Podaj numer Drinka do modyfikacji");
        string s = Console.ReadLine();
        int id;
        if (int.TryParse(s, out id))
        {
            var d = await repostiory.GetByIdAsync(id);

            if (d != null && d.Status == DrinkStatus.Creating)
            {
                var ingidients = await repostioryIng.GetAllAsync();

                WriteLine("Oto składniki, które już są w tym drinku:");

                var table2 = new ConsoleTable("Id", "Name", "Volume", "Price");

                foreach (var item in d.Ingredients)
                {
                    table2.AddRow(item.Id, item.Name,
                        item.Volume, item.Price);
                }

                table2.Write();
                WriteLine("");
                WriteLine("Co chcesz dodać?");
                WriteLine("Wpisz znak alfabetyczny aby anulować");

                string inIdstring = ReadLine();


                int inId;
                if (int.TryParse(inIdstring, out inId))
                {
                    var ing = ingidients.
                        FirstOrDefault(x => x.Id == inId);

                    d.Ingredients.Remove(ing);

                    await repostiory.UpdateAsync(d);
                }
            }


        }
        else
        {
            WriteLine("Drink o tym ID nie istnieje");
            WriteLine("lub");
            WriteLine("Drink już został skończony");
            ReadLine();
        }
    }
    else if (command == '5')
    {
        WriteLine("Podaj numer Drinka do skończenia");
        string s = Console.ReadLine();
        int id;
        if (int.TryParse(s, out id))
        {
            var d = await repostiory.GetByIdAsync(id);
            d.Status = DrinkStatus.Done;
            await repostiory.UpdateAsync(d);
        }
    }
    else if (command == '6')
    {

    }
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
    Console.Clear();
}





