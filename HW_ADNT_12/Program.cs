using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace HW_ADNT_12
{

    internal class Program
    {
        static void Main(string[] args)
        {
            string ConnectionString = "Data Source=DESKTOP-V5OB79V;Initial Catalog=FruitsNVegetables;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connected sucess");

                    string que = "";
                    string choise = "";
                    while (true)
                    {
                        Console.WriteLine("Choose:\n1 - Out all\n2 - Names all\n3 - Colors all\n4 - Max cal\n5 - Min cal\n6 - Avg cal" +
                            "\n7 - Count vegetables\n8 - Count fruits\n9 - Veg and fruits by color\n10 - Count veg and fruits\n" +
                            "11 - Cal > num\n12 - Cal < num\n13 - Cal in range\n14 - show like yellow or orange color" +
                            "\n15 - Dupper functions" +
                            "\n0 - Exit");
                        choise = Console.ReadLine();
                        if (choise == "0")
                        {
                            break;
                        }
                        else if (choise == "1")
                        {
                            que = "SELECT * FROM FnV";
                        }
                        else if (choise == "2")
                        {
                            que = "SELECT Name FROM FnV";
                        }
                        else if (choise == "3")
                        {
                            que = "SELECT Color FROM FnV";
                        }
                        else if (choise == "4")
                        {
                            que = "SELECT MAX(Caloricity) FROM FnV";
                        }
                        else if (choise == "5")
                        {
                            que = "SELECT MIN(Caloricity) FROM FnV";
                        }
                        else if (choise == "6")
                        {
                            que = "SELECT AVG(Caloricity) FROM FnV";
                        }
                        else if (choise == "7")
                        {
                            que = "SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Type LIKE 'Vegetable     '";
                        }
                        else if (choise == "8")
                        {
                            que = "SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Type LIKE 'Fruit         '";
                        }
                        else if (choise == "9")
                        {
                            Console.WriteLine("Enter color:\n");
                            string color = Console.ReadLine();
                            que = $"SELECT COUNT(*) FROM FnV\r\nWHERE FnV.Color LIKE '{color}'";
                        }
                        else if (choise == "10")
                        {
                            que = "SELECT Color, COUNT(*) FROM FnV\r\nGROUP BY Color";
                        }
                        else if (choise == "11")
                        {
                            Console.WriteLine("Enter cal top line:\n");
                            string cal = Console.ReadLine();
                            que = $"SELECT * FROM FnV\r\nWHERE Caloricity < {cal}";
                        }
                        else if (choise == "12")
                        {
                            Console.WriteLine("Enter cal bottom line:\n");
                            string cal = Console.ReadLine();
                            que = $"SELECT * FROM FnV\r\nWHERE Caloricity > {cal}";
                        }
                        else if (choise == "13")
                        {
                            Console.WriteLine("Enter cal bottom line:\n");
                            string cal_bot = Console.ReadLine();
                            Console.WriteLine("Enter cal top line:\n");
                            string cal_top = Console.ReadLine();
                            que = $"SELECT * FROM FnV\r\nWHERE Caloricity < {cal_top} AND Caloricity > {cal_bot}";
                        }
                        else if (choise == "14")
                        {
                            que = "SELECT * FROM FnV\r\nWHERE Color LIKE 'Yellow' OR Color LIKE 'RED'";
                        }
                        else if (choise == "15")
                        {
                            while(true)
                            {
                                Console.WriteLine("1 - Search by name\n2 - Show all\n3 - Update an element" +
                                    "\n4 - Delete and element\n5 - AVG caloricity" +
                                    "\n0 - Exit");
                                choise = Console.ReadLine();
                                if (choise == "1")
                                {
                                    Console.WriteLine("Enter name of product");
                                    string name = Console.ReadLine();
                                    Product product = connection.QueryFirstOrDefault<Product>("GetByName", new {Name = name}, 
                                        commandType: System.Data.CommandType.StoredProcedure);
                                    if (product != null)
                                    {
                                        Console.WriteLine($"ID: {product.ID}\t Name: {product.Name}\tType: {product.Type}\tColor: {product.Color}" +
                                            $"\tCaloricity: {product.Caloricity}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("ERROR: Not found product");
                                    }
                                }
                                else if (choise == "2")
                                {
                                    IEnumerable<Product> products = connection.Query<Product>("GetAll", null, commandType: System.Data.CommandType.StoredProcedure);
                                    foreach(Product product in products)
                                    {
                                        Console.WriteLine($"ID: {product.ID}\t Name: {product.Name}\tType: {product.Type}\tColor: {product.Color}" +
                                            $"\tCaloricity: {product.Caloricity}");
                                    }
                                }
                                else if (choise == "3")
                                {
                                    Console.WriteLine("Enter name:\n");
                                    string name = Console.ReadLine();
                                    Console.WriteLine("Enter type:\n");
                                    string type = Console.ReadLine();
                                    Console.WriteLine("Enter Color:\n");
                                    string color = Console.ReadLine();
                                    Console.WriteLine("Enter caloricity:\n");
                                    string caloricity = Console.ReadLine();
                                    Console.WriteLine("Enter ID:\n");
                                    string ID = Console.ReadLine();
                                    connection.Execute("UpdateAnElement", new { name, type, color, caloricity, ID }, commandType: System.Data.CommandType.StoredProcedure);
                                }
                                else if (choise == "4")
                                {
                                    Console.WriteLine("Enter ID:\n");
                                    string ID = Console.ReadLine();
                                    connection.Execute("DelAnElem", new {ID}, commandType: System.Data.CommandType.StoredProcedure);
                                }
                                else if (choise == "5")
                                {
                                    IEnumerable<int> avg = connection.Query<int>("CaloricityAVG", null, commandType: System.Data.CommandType.StoredProcedure);
                                    Console.WriteLine(avg.First());
                                }
                                else if (choise == "0")
                                {
                                    break;
                                }
                                else
                                {
                                    Console.Write("ERROR: Invalid option");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Invalid option!");
                        }
                        if (choise != "0")
                        {
                            using (SqlCommand command = new SqlCommand(que, connection))
                            {
                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        for (int i = 0; i < reader.FieldCount; i++)
                                        {
                                            Console.Write(reader[i] + " ");
                                        }
                                        Console.WriteLine('\n');
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex}");
                }
            }
        }
    }
}
