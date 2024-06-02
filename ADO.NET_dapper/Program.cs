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

        private static void InsertNewBuyer(IDbConnection db)
        {
            Console.Write("Enter name: ");
            string name = Console.ReadLine();
            Console.Write("Enter date of birth (YYYY-MM-DD): ");
            DateTime dob = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter gender: ");
            string gender = Console.ReadLine();
            Console.Write("Enter email: ");
            string email = Console.ReadLine();
            Console.Write("Enter country: ");
            string country = Console.ReadLine();
            Console.Write("Enter city: ");
            string city = Console.ReadLine();

            string sql = "INSERT INTO Buyers (Name, Dob, Gender, Email, Country, City) VALUES (@Name, @Dob, @Gender, @Email, @Country, @City)";
            db.Execute(sql, new { Name = name, Dob = dob, Gender = gender, Email = email, Country = country, City = city });
            Console.WriteLine("Buyer inserted successfully.");
        }

        private static void InsertNewCountry(IDbConnection db)
        {
            Console.Write("Enter country name: ");
            string country = Console.ReadLine();

            string sql = "INSERT INTO Countries (Name) VALUES (@Country)";
            db.Execute(sql, new { Country = country });
            Console.WriteLine("Country inserted successfully.");
        }

        private static void InsertNewCity(IDbConnection db)
        {
            Console.Write("Enter city name: ");
            string city = Console.ReadLine();

            string sql = "INSERT INTO Cities (Name) VALUES (@City)";
            db.Execute(sql, new { City = city });
            Console.WriteLine("City inserted successfully.");
        }

        private static void InsertNewSection(IDbConnection db)
        {
            Console.Write("Enter section name: ");
            string sectionName = Console.ReadLine();

            string sql = "INSERT INTO Interests (SectionName) VALUES (@SectionName)";
            db.Execute(sql, new { SectionName = sectionName });
            Console.WriteLine("Section inserted successfully.");
        }

        private static void InsertNewPromotionalProduct(IDbConnection db)
        {
            Console.Write("Enter section id: ");
            int sectionId = int.Parse(Console.ReadLine());
            Console.Write("Enter product name: ");
            string productName = Console.ReadLine();
            Console.Write("Enter country: ");
            string country = Console.ReadLine();
            Console.Write("Enter start date (YYYY-MM-DD): ");
            DateTime startDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter end date (YYYY-MM-DD): ");
            DateTime endDate = DateTime.Parse(Console.ReadLine());

            string sql = "INSERT INTO Promotions (SectionId, ProductName, Country, StartDate, EndDate) VALUES (@SectionId, @ProductName, @Country, @StartDate, @EndDate)";
            db.Execute(sql, new { SectionId = sectionId, ProductName = productName, Country = country, StartDate = startDate, EndDate = endDate });
            Console.WriteLine("Promotional product inserted successfully.");
        }

        private static void UpdateCustomerInformation(IDbConnection db)
        {
            Console.Write("Enter buyer id: ");
            int buyerId = int.Parse(Console.ReadLine());
            Console.Write("Enter new name (leave empty to keep current): ");
            string name = Console.ReadLine();
            Console.Write("Enter new date of birth (YYYY-MM-DD, leave empty to keep current): ");
            string dobInput = Console.ReadLine();
            DateTime? dob = string.IsNullOrEmpty(dobInput) ? (DateTime?)null : DateTime.Parse(dobInput);
            Console.Write("Enter new gender (leave empty to keep current): ");
            string gender = Console.ReadLine();
            Console.Write("Enter new email (leave empty to keep current): ");
            string email = Console.ReadLine();
            Console.Write("Enter new country (leave empty to keep current): ");
            string country = Console.ReadLine();
            Console.Write("Enter new city (leave empty to keep current): ");
            string city = Console.ReadLine();

            string sql = "UPDATE Buyers SET Name = COALESCE(NULLIF(@Name, ''), Name), Dob = COALESCE(@Dob, Dob), Gender = COALESCE(NULLIF(@Gender, ''), Gender), Email = COALESCE(NULLIF(@Email, ''), Email), Country = COALESCE(NULLIF(@Country, ''), Country), City = COALESCE(NULLIF(@City, ''), City) WHERE BuyerId = @BuyerId";
            db.Execute(sql, new { BuyerId = buyerId, Name = name, Dob = dob, Gender = gender, Email = email, Country = country, City = city });
            Console.WriteLine("Customer information updated successfully.");
        }

        private static void UpdateCountryInformation(IDbConnection db)
        {
            Console.Write("Enter country id: ");
            int countryId = int.Parse(Console.ReadLine());
            Console.Write("Enter new country name (leave empty to keep current): ");
            string country = Console.ReadLine();

            string sql = "UPDATE Countries SET Name = COALESCE(NULLIF(@Country, ''), Name) WHERE CountryId = @CountryId";
            db.Execute(sql, new { CountryId = countryId, Country = country });
            Console.WriteLine("Country information updated successfully.");
        }

        private static void UpdateCityInformation(IDbConnection db)
        {
            Console.Write("Enter city id: ");
            int cityId = int.Parse(Console.ReadLine());
            Console.Write("Enter new city name (leave empty to keep current): ");
            string city = Console.ReadLine();

            string sql = "UPDATE Cities SET Name = COALESCE(NULLIF(@City, ''), Name) WHERE CityId = @CityId";
            db.Execute(sql, new { CityId = cityId, City = city });
            Console.WriteLine("City information updated successfully.");
        }

        private static void UpdateSectionInformation(IDbConnection db)
        {
            Console.Write("Enter section id: ");
            int sectionId = int.Parse(Console.ReadLine());
            Console.Write("Enter new section name (leave empty to keep current): ");
            string sectionName = Console.ReadLine();

            string sql = "UPDATE Interests SET SectionName = COALESCE(NULLIF(@SectionName, ''), SectionName) WHERE InterestId = @SectionId";
            db.Execute(sql, new { SectionId = sectionId, SectionName = sectionName });
            Console.WriteLine("Section information updated successfully.");
        }

        private static void UpdatePromotionalProductInformation(IDbConnection db)
        {
            Console.Write("Enter promotion id: ");
            int promotionId = int.Parse(Console.ReadLine());
            Console.Write("Enter new section id (leave empty to keep current): ");
            string sectionIdInput = Console.ReadLine();
            int? sectionId = string.IsNullOrEmpty(sectionIdInput) ? (int?)null : int.Parse(sectionIdInput);
            Console.Write("Enter new product name (leave empty to keep current): ");
            string productName = Console.ReadLine();
            Console.Write("Enter new country (leave empty to keep current): ");
            string country = Console.ReadLine();
            Console.Write("Enter new start date (YYYY-MM-DD, leave empty to keep current): ");
            string startDateInput = Console.ReadLine();
            DateTime? startDate = string.IsNullOrEmpty(startDateInput) ? (DateTime?)null : DateTime.Parse(startDateInput);
            Console.Write("Enter new end date (YYYY-MM-DD, leave empty to keep current): ");
            string endDateInput = Console.ReadLine();
            DateTime? endDate = string.IsNullOrEmpty(endDateInput) ? (DateTime?)null : DateTime.Parse(endDateInput);

            string sql = "UPDATE Promotions SET SectionId = COALESCE(@SectionId, SectionId), ProductName = COALESCE(NULLIF(@ProductName, ''), ProductName), Country = COALESCE(NULLIF(@Country, ''), Country), StartDate = COALESCE(@StartDate, StartDate), EndDate = COALESCE(@EndDate, EndDate) WHERE PromotionId = @PromotionId";
            db.Execute(sql, new { PromotionId = promotionId, SectionId = sectionId, ProductName = productName, Country = country, StartDate = startDate, EndDate = endDate });
            Console.WriteLine("Promotional product information updated successfully.");
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
