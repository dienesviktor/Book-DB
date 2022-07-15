using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;

namespace Codecool.BookDb.Model;

public class AuthorDao : IAuthorDao
{
    private readonly string _connectionString;
    private const string DateFormat = "yyyy-MM-dd";

    public AuthorDao(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Add(Author author)
    {
        const string insertCommand = @"INSERT INTO author (first_name, last_name, birth_date)
                        VALUES (@first_name, @last_name, @birth_date)
                        SELECT SCOPE_IDENTITY();";

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(insertCommand, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@first_name", author.FirstName);
                cmd.Parameters.AddWithValue("@last_name", author.LastName);
                cmd.Parameters.AddWithValue("@birth_date", author.BirthDate.ToString(DateFormat));

                author.Id = Convert.ToInt32(cmd.ExecuteScalar());
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void Update(Author author)
    {
        const string updateCommand = @"UPDATE author SET first_name = @firstName,
                                                    last_name = @lastName,
                                                    birth_date = @birthDate
                                    WHERE id = @id";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(updateCommand, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@firstName", author.FirstName);
                cmd.Parameters.AddWithValue("@lastName", author.LastName);
                cmd.Parameters.AddWithValue("@birthDate", author.BirthDate.ToString(DateFormat));
                cmd.Parameters.AddWithValue("@id", author.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public Author Get(int id)
    {
        const string cmdText = @"SELECT first_name,
                                    last_name,
                                    birth_date
                            FROM author
                            WHERE id = @Id";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(cmdText, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@id", id);

                var reader = cmd.ExecuteReader();
                if (!reader.Read()) // first row was not found == no data was returned by the query
                {
                    return null;
                }

                var firstName = reader.GetString("first_name");
                var lastName = reader.GetString("last_name");
                var birthDate = DateOnly.FromDateTime(reader.GetDateTime("birth_date"));

                var author = new Author(firstName, lastName, birthDate) { Id = id };

                connection.Close();
                return author;
            }
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public List<Author> GetAll()
    {
        const string cmdText = @"SELECT id,
                                    first_name,
                                    last_name,
                                    birth_date
                            FROM author";
        try
        {
            var results = new List<Author>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(cmdText, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                var reader = cmd.ExecuteReader();

                if (!reader.HasRows)
                    return results;

                while (reader.Read())
                {
                    var firstName = reader["first_name"] as string;
                    var lastName = reader["last_name"] as string;
                    var birthDate = DateOnly.FromDateTime((DateTime)reader["birth_date"]);
                    var author = new Author(firstName, lastName, birthDate) { Id = (int)reader["Id"] };
                    results.Add(author);
                }
                connection.Close();
            }

            return results;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }
}