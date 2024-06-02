using Dapper;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;

namespace MailingListApp
{
    public class Program
    {
        private static string connectionString = "Server=localhost;Database=mailinglistdb;Uid=root;Pwd=yourpassword;";

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
                Console.WriteLine("10. Insert information about new buyers");
                Console.WriteLine("11. Insert new countries");
                Console.WriteLine("12. Insert new cities");
                Console.WriteLine("13. Insert information about new sections");
                Console.WriteLine("14. Insert information about new promotional products");
                Console.WriteLine("15. Update customer information");
                Console.WriteLine("16. Update country information");
                Console.WriteLine("17. Update city information");
                Console.WriteLine("18. Update section information");
                Console.WriteLine("19. Update information about promotional products");
                Console.WriteLine("20. Delete buyer information");
                Console.WriteLine("21. Delete country information");
                Console.WriteLine("22. Delete city information");
                Console.WriteLine("23. Delete section information");
                Console.WriteLine("24. Delete promotional product information");
                Console.WriteLine("25. Display a list of cities in a particular country");
                Console.WriteLine("26. Display a list of sections of a particular customer");
                Console.WriteLine("27. Display the list of promotional products of a certain section");
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
                    case "10":
                        InsertNewBuyer(db);
                        break;
                    case "11":
                        InsertNewCountry(db);
                        break;
                    case "12":
                        InsertNewCity(db);
                        break;
                    case "13":
                        InsertNewSection(db);
                        break;
                    case "14":
                        InsertNewPromotionalProduct(db);
                        break;
                    case "15":
                        UpdateCustomerInformation(db);
                        break;
                    case "16":
                        UpdateCountryInformation(db);
                        break;
                    case "17":
                        UpdateCityInformation(db);
                        break;
                    case "18":
                        UpdateSectionInformation(db);
                        break;
                    case "19":
                        UpdatePromotionalProductInformation(db);
                        break;
                    case "20":
                        DeleteBuyerInformation(db);
                        break;
                    case "21":
                        DeleteCountryInformation(db);
                        break;
                    case "22":
                        DeleteCityInformation(db);
                        break;
                    case "23":
                        DeleteSectionInformation(db);
                        break;
                    case "24":
                        DeletePromotionalProductInformation(db);
                        break;
                    case "25":
                        DisplayCitiesInCountry(db);
                        break;
                    case "26":
                        DisplaySectionsOfCustomer(db);
                        break;
                    case "27":
                        DisplayPromotionalProductsInSection(db);
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
            var buyers = db.Query<Buyer>("SELECT * FROM Buyers WHERE Country = @Country", new
            {
                Country = country
            }).ToList();
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
                Console.WriteLine($"{promo.PromotionId}: {promo.ProductName} ({promo.StartDate} to {promo.EndDate})");
            }
        }

        private static void InsertNewBuyer(IDbConnection db)
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine();
            Console.Write("Enter date of birth (YYYY-MM-DD): ");
            var dob = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter gender: ");
            var gender = Console.ReadLine();
            Console.Write("Enter email: ");
            var email = Console.ReadLine();
            Console.Write("Enter country: ");
            var country = Console.ReadLine();
            Console.Write("Enter city: ");
            var city = Console.ReadLine();

            var sql = "INSERT INTO Buyers (Name, Dob, Gender, Email, Country, City) VALUES (@Name, @Dob, @Gender, @Email, @Country, @City)";
            db.Execute(sql, new { Name = name, Dob = dob, Gender = gender, Email = email, Country = country, City = city });
            Console.WriteLine("Buyer inserted successfully.");
        }

        private static void InsertNewCountry(IDbConnection db)
        {
            Console.Write("Enter country name: ");
            var country = Console.ReadLine();

            var sql = "INSERT INTO Countries (Name) VALUES (@Country)";
            db.Execute(sql, new { Country = country });
            Console.WriteLine("Country inserted successfully.");
        }

        private static void InsertNewCity(IDbConnection db)
        {
            Console.Write("Enter city name: ");
            var city = Console.ReadLine();

            var sql = "INSERT INTO Cities (Name) VALUES (@City)";
            db.Execute(sql, new { City = city });
            Console.WriteLine("City inserted successfully.");
        }

        private static void InsertNewSection(IDbConnection db)
        {
            Console.Write("Enter section name: ");
            var sectionName = Console.ReadLine();

            var sql = "INSERT INTO Interests (SectionName) VALUES (@SectionName)";
            db.Execute(sql, new { SectionName = sectionName });
            Console.WriteLine("Section inserted successfully.");
        }

        private static void InsertNewPromotionalProduct(IDbConnection db)
        {
            Console.Write("Enter section id: ");
            var sectionId = int.Parse(Console.ReadLine());
            Console.Write("Enter product name: ");
            var productName = Console.ReadLine();
            Console.Write("Enter country: ");
            var country = Console.ReadLine();
            Console.Write("Enter start date (YYYY-MM-DD): ");
            var startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter end date (YYYY-MM-DD): ");
            var endDate = DateTime.Parse(Console.ReadLine());

            var sql = "INSERT INTO Promotions (SectionId, ProductName, Country, StartDate, EndDate) VALUES (@SectionId, @ProductName, @Country, @StartDate, @EndDate)";
            db.Execute(sql, new { SectionId = sectionId, ProductName = productName, Country = country, StartDate = startDate, EndDate = endDate });
            Console.WriteLine("Promotional product inserted successfully.");
        }

        private static void UpdateCustomerInformation(IDbConnection db)
        {
            Console.Write("Enter buyer ID: ");
            var buyerId = int.Parse(Console.ReadLine());
            Console.Write("Enter new name: ");
            var newName = Console.ReadLine();
            Console.Write("Enter new date of birth (YYYY-MM-DD): ");
            var newDob = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new gender: ");
            var newGender = Console.ReadLine();
            Console.Write("Enter new email: ");
            var newEmail = Console.ReadLine();
            Console.Write("Enter new country: ");
            var newCountry = Console.ReadLine();
            Console.Write("Enter new city: ");
            var newCity = Console.ReadLine();

            var sql = "UPDATE Buyers SET Name = @Name, Dob = @Dob, Gender = @Gender, Email = @Email, Country = @Country, City = @City WHERE BuyerId = @BuyerId";
            db.Execute(sql, new { Name = newName, Dob = newDob, Gender = newGender, Email = newEmail, Country = newCountry, City = newCity, BuyerId = buyerId });
            Console.WriteLine("Buyer information updated successfully.");
        }

        private static void UpdateCountryInformation(IDbConnection db)
        {
            Console.Write("Enter existing country name: ");
            var oldCountry = Console.ReadLine();
            Console.Write("Enter new country name: ");
            var newCountry = Console.ReadLine();

            var sql = "UPDATE Buyers SET Country = @NewCountry WHERE Country = @OldCountry";
            db.Execute(sql, new { OldCountry = oldCountry, NewCountry = newCountry });
            Console.WriteLine("Country information updated successfully.");
        }

        private static void UpdateCityInformation(IDbConnection db)
        {
            Console.Write("Enter existing city name: ");
            var oldCity = Console.ReadLine();
            Console.Write("Enter new city name: ");
            var newCity = Console.ReadLine();

            var sql = "UPDATE Buyers SET City = @NewCity WHERE City = @OldCity";
            db.Execute(sql, new { OldCity = oldCity, NewCity = newCity });
            Console.WriteLine("City information updated successfully.");
        }

        private static void UpdateSectionInformation(IDbConnection db)
        {
            Console.Write("Enter existing section name: ");
            var oldSection = Console.ReadLine();
            Console.Write("Enter new section name: ");
            var newSection = Console.ReadLine();

            var sql = "UPDATE Interests SET SectionName = @NewSection WHERE SectionName = @OldSection";
            db.Execute(sql, new { OldSection = oldSection, NewSection = newSection });
            Console.WriteLine("Section information updated successfully.");
        }

        private static void UpdatePromotionalProductInformation(IDbConnection db)
        {
            Console.Write("Enter existing product ID: ");
            var productId = int.Parse(Console.ReadLine());
            Console.Write("Enter new product name: ");
            var newProductName = Console.ReadLine();
            Console.Write("Enter new country: ");
            var newCountry = Console.ReadLine();
            Console.Write("Enter new start date (YYYY-MM-DD): ");
            var newStartDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new end date (YYYY-MM-DD): ");
            var newEndDate = DateTime.Parse(Console.ReadLine());

            var sql = "UPDATE Promotions SET ProductName = @NewProductName, Country = @NewCountry, StartDate = @NewStartDate, EndDate = @NewEndDate WHERE PromotionId = @ProductId";
            db.Execute(sql, new { NewProductName = newProductName, NewCountry = newCountry, NewStartDate = newStartDate, NewEndDate = newEndDate, ProductId = productId });
            Console.WriteLine("Promotional product information updated successfully.");
        }

        private static void DeleteBuyerInformation(IDbConnection db)
        {
            Console.Write("Enter buyer ID to delete: ");
            var buyerId = int.Parse(Console.ReadLine());
            var sql = "DELETE FROM Buyers WHERE BuyerId = @BuyerId";
            db.Execute(sql, new { BuyerId = buyerId });
            Console.WriteLine("Buyer information deleted successfully.");
        }

        private static void DeleteCountryInformation(IDbConnection db)
        {
            Console.Write("Enter country name to delete: ");
            var country = Console.ReadLine();

            var sql = "DELETE FROM Countries WHERE Name = @Country";
            db.Execute(sql, new { Country = country });
            Console.WriteLine("Country information deleted successfully.");
        }

        private static void DeleteCityInformation(IDbConnection db)
        {
            Console.Write("Enter city name to delete: ");
            var city = Console.ReadLine();

            var sql = "DELETE FROM Cities WHERE Name = @City";
            db.Execute(sql, new { City = city });
            Console.WriteLine("City information deleted successfully.");
        }

        private static void DeleteSectionInformation(IDbConnection db)
        {
            Console.Write("Enter section name to delete: ");
            var section = Console.ReadLine();

            var sql = "DELETE FROM Interests WHERE SectionName = @Section";
            db.Execute(sql, new { Section = section });
            Console.WriteLine("Section information deleted successfully.");
        }

        private static void DeletePromotionalProductInformation(IDbConnection db)
        {
            Console.Write("Enter promotional product ID to delete: ");
            var productId = int.Parse(Console.ReadLine());

            var sql = "DELETE FROM Promotions WHERE PromotionId = @ProductId";
            db.Execute(sql, new { ProductId = productId });
            Console.WriteLine("Promotional product information deleted successfully.");
        }

        private static void DisplayCitiesInCountry(IDbConnection db)
        {
            Console.Write("Enter country name: ");
            var country = Console.ReadLine();

            var cities = db.Query<string>("SELECT City FROM Buyers WHERE Country = @Country", new { Country = country }).Distinct().ToList();
            Console.WriteLine($"\nCities in {country}:");
            foreach (var city in cities)
            {
                Console.WriteLine(city);
            }
        }

        private static void DisplaySectionsOfCustomer(IDbConnection db)
        {
            Console.Write("Enter buyer id: ");
            var buyerId = int.Parse(Console.ReadLine());

            var sections = db.Query<string>("SELECT DISTINCT i.SectionName FROM Interests i INNER JOIN BuyerInterests bi ON i.InterestId = bi.InterestId WHERE bi.BuyerId = @BuyerId", new { BuyerId = buyerId }).ToList();
            Console.WriteLine($"\nSections of customer {buyerId}:");
            foreach (var section in sections)
            {
                Console.WriteLine(section);
            }
        }

        private static void DisplayPromotionalProductsInSection(IDbConnection db)
        {
            Console.Write("Enter section name: ");
            var sectionName = Console.ReadLine();

            var products = db.Query<Promotion>("SELECT * FROM Promotions WHERE SectionId = (SELECT InterestId FROM Interests WHERE SectionName = @SectionName)", new { SectionName = sectionName }).ToList();
            Console.WriteLine($"\nPromotional products in section {sectionName}:");
            foreach (var product in products)
            {
                Console.WriteLine($"{product.PromotionId}: {product.ProductName} - {product.Country} ({product.StartDate} to {product.EndDate})");
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