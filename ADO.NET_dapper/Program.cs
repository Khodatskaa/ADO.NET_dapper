using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace ADO.NET_dapper
{
    public class Program
    {
        private static string connectionString = "Server=localhost;Database=mailinglistdb;User ID=root;Password=yourpassword;"; 

        static void Main(string[] args)
        {
            using (IDbConnection db = new MySqlConnection(connectionString)) 
            {
                try
                {
                    db.Open();
                    Console.WriteLine("Successfully connected to the database.");
                    DisplayMenu(db);
                    db.Close();
                    Console.WriteLine("Successfully disconnected from the database.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to the database: " + ex.Message);
                }
            }
        }

        private static void DisplayMenu(IDbConnection db)
        {
            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Display all buyers");
                Console.WriteLine("2. Display the email of all buyers");
                Console.WriteLine("3. Display a list of sections");
                Console.WriteLine("4. Display the list of promotional products");
                Console.WriteLine("5. Display all cities");
                Console.WriteLine("6. Display all countries");
                Console.WriteLine("7. Display all buyers from a particular city");
                Console.WriteLine("8. Display all buyers from a specific country");
                Console.WriteLine("9. Display all shares for a particular country");
                Console.WriteLine("0. Exit");
                Console.Write("Select an option: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DisplayAllBuyers(db);
                        break;
                    case "2":
                        DisplayAllEmails(db);
                        break;
                    case "3":
                        DisplayAllSections(db);
                        break;
                    case "4":
                        DisplayAllPromotionalProducts(db);
                        break;
                    case "5":
                        DisplayAllCities(db);
                        break;
                    case "6":
                        DisplayAllCountries(db);
                        break;
                    case "7":
                        DisplayBuyersByCity(db);
                        break;
                    case "8":
                        DisplayBuyersByCountry(db);
                        break;
                    case "9":
                        DisplaySharesByCountry(db);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Invalid option, please try again.");
                        break;
                }
            }
        }

        private static void DisplayAllBuyers(IDbConnection db)
        {
            var buyers = db.Query<Buyer>("SELECT * FROM Buyers").ToList();
            Console.WriteLine("\nBuyers:");
            foreach (var buyer in buyers)
            {
                Console.WriteLine($"{buyer.BuyerId}: {buyer.Name} - {buyer.Email}");
            }
        }

        private static void DisplayAllEmails(IDbConnection db)
        {
            var emails = db.Query<string>("SELECT Email FROM Buyers").ToList();
            Console.WriteLine("\nEmails:");
            foreach (var email in emails)
            {
                Console.WriteLine(email);
            }
        }

        private static void DisplayAllSections(IDbConnection db)
        {
            var sections = db.Query<Interest>("SELECT * FROM Interests").ToList();
            Console.WriteLine("\nSections:");
            foreach (var section in sections)
            {
                Console.WriteLine($"{section.InterestId}: {section.SectionName}");
            }
        }

        private static void DisplayAllPromotionalProducts(IDbConnection db)
        {
            var promotions = db.Query<Promotion>("SELECT * FROM Promotions").ToList();
            Console.WriteLine("\nPromotional Products:");
            foreach (var promo in promotions)
            {
                Console.WriteLine($"{promo.PromotionId}: {promo.ProductName} - {promo.Country} ({promo.StartDate} to {promo.EndDate})");
            }
        }

        private static void DisplayAllCities(IDbConnection db)
        {
            var cities = db.Query<string>("SELECT DISTINCT City FROM Buyers").ToList();
            Console.WriteLine("\nCities:");
            foreach (var city in cities)
            {
                Console.WriteLine(city);
            }
        }

        private static void DisplayAllCountries(IDbConnection db)
        {
            var countries = db.Query<string>("SELECT DISTINCT Country FROM Buyers").ToList();
            Console.WriteLine("\nCountries:");
            foreach (var country in countries)
            {
                Console.WriteLine(country);
            }
        }

        private static void DisplayBuyersByCity(IDbConnection db)
        {
            Console.Write("Enter city name: ");
            var city = Console.ReadLine();
            var buyers = db.Query<Buyer>("SELECT * FROM Buyers WHERE City = @City", new { City = city }).ToList();
            Console.WriteLine($"\nBuyers from {city}:");
            foreach (var buyer in buyers)
            {
                Console.WriteLine($"{buyer.BuyerId}: {buyer.Name} - {buyer.Email}");
            }
        }

        private static void DisplayBuyersByCountry(IDbConnection db)
        {
            Console.Write("Enter country name: ");
            var country = Console.ReadLine();
            var buyers = db.Query<Buyer>("SELECT * FROM Buyers WHERE Country = @Country", new { Country = country }).ToList();
            Console.WriteLine($"\nBuyers from {country}:");
            foreach (var buyer in buyers)
            {
                Console.WriteLine($"{buyer.BuyerId}: {buyer.Name} - {buyer.Email}");
            }
        }

        private static void DisplaySharesByCountry(IDbConnection db)
        {
            Console.Write("Enter country name: ");
            var country = Console.ReadLine();
            var promotions = db.Query<Promotion>("SELECT * FROM Promotions WHERE Country = @Country", new { Country = country }).ToList();
            Console.WriteLine($"\nShares for {country}:");
            foreach (var promo in promotions)
            {
                Console.WriteLine($"{promo.PromotionId}: {promo.ProductName} - ({promo.StartDate} to {promo.EndDate})");
            }
        }

        public class Buyer
        {
            public int BuyerId { get; set; }
            public string? Name { get; set; }
            public DateTime Dob { get; set; }
            public string? Gender { get; set; }
            public string? Email { get; set; }
            public string? Country { get; set; }
            public string? City { get; set; }
        }

        public class Interest
        {
            public int InterestId { get; set; }
            public string? SectionName { get; set; }
        }

        public class Promotion
        {
            public int PromotionId { get; set; }
            public int SectionId { get; set; }
            public string? ProductName { get; set; }
            public string? Country { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }
    }
}
