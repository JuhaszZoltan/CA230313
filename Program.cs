﻿using MySql.Data.MySqlClient;

Console.WriteLine("---csatlakozás az adatbázishoz---");
/*
Console.Write("uid: ");
string uid = Console.ReadLine();
Console.Write("pwd: ");
string pwd = Console.ReadLine();
*/

string uid = "diak171";
string pwd = "ZDGE0W";

string connectionString =
    "Server=172.16.1.241;" +
    $"Database={uid};" +
    $"Uid={uid};" +
    $"Pwd={pwd};";

using MySqlConnection connection = new(connectionString);
connection.Open();

string cmdText = "SELECT * FROM teszt;";

MySqlCommand cmd = new(cmdText, connection);
MySqlDataReader reader = cmd.ExecuteReader();

while (reader.Read()) Console.WriteLine($"{reader["nev"]}\t{reader["egyenleg"]}");

reader.Close();

Console.WriteLine("--- új adatok rögzítése---");
Console.Write("írjon be egy nevet: ");
string nev = Console.ReadLine();

cmdText = $"SELECT * FROM teszt WHERE nev LIKE '{nev}';";
reader = new MySqlCommand(cmdText, connection).ExecuteReader();
bool benneVan = reader.Read();

if (benneVan)
{
    int modId = reader.GetInt32(0);
    int aktEgy = reader.GetInt32(2);
    reader.Close();

    Console.WriteLine($"{nev} benne van az adatbázisban (ID = {modId})");
    Console.Write($"Szeretné {nev} egyenlegét módosítani? (y/n) ");
    if (Console.ReadKey().KeyChar == 'y')
    {
        Console.Write("\nmennyivel kívánja módsítani az egyenleget? ");
        int egyenlegModositas = int.Parse(Console.ReadLine());
        aktEgy += egyenlegModositas;

        MySqlDataAdapter adapter = new()
        {
            UpdateCommand = new(
                $"UPDATE teszt " +
                $"SET egyenleg = {aktEgy} " +
                $"WHERE id = {modId};",
                connection),
        };
        adapter.UpdateCommand.ExecuteNonQuery();
        Console.WriteLine("---adatok módosítása megtörtént!---");
    }
}
else
{
    //új adat rögzítése lesz
    Console.WriteLine($"{nev} még nincs benne az adatbázisban");
}
