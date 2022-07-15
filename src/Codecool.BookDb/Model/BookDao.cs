using System.Data;
using System.Runtime.CompilerServices;
using Codecool.BookDb.Model;
using Microsoft.Data.SqlClient;

public class BookDao : IBookDao
{
    private readonly string _connectionString;
    private readonly IAuthorDao _authorDao;

    public BookDao(string connectionString, IAuthorDao authorDao)
    {
        _connectionString = connectionString;
        _authorDao = authorDao;
    }

    public void Add(Book book)
    {
        const string cmdText = @"INSERT INTO book (author_id, title)
                                VALUES (@author_id, @title)
                                SELECT SCOPE_IDENTITY();";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(cmdText, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@author_id", book.Author.Id);
                cmd.Parameters.AddWithValue("@title", book.Title);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public void Update(Book book)
    {
        const string cmdText = @"UPDATE book SET author_id = @author_id,
                                                title = @title
                                    WHERE id = @id";
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(cmdText, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@author_id", book.Author.Id);
                cmd.Parameters.AddWithValue("@title", book.Title);
                cmd.Parameters.AddWithValue("@id", book.Id);
                cmd.ExecuteNonQuery();
                connection.Close();
            }
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public Book Get(int id)
    {
        const string sql = @"SELECT author_id,
                            title
                        FROM book
                        WHERE id = @id";
        try
        {
            int authorId;
            string title;
            using (var connection = new SqlConnection(_connectionString))
            {
                var cmd = new SqlCommand(sql, connection);
                if (connection.State == ConnectionState.Closed)
                    connection.Open();
                cmd.Parameters.AddWithValue("@id", id);
                var reader = cmd.ExecuteReader();
                reader.Read();
                authorId = (int)reader["author_id"];
                title = reader["title"] as string;
                connection.Close();
            }

            var author = _authorDao.Get(authorId);
            var book = new Book(author, title) { Id = id };
            return book;
        }
        catch (SqlException e)
        {
            throw new RuntimeWrappedException(e);
        }
    }

    public List<Book> GetAll()
    {
        const string cmdText = @"SELECT id,
                                    author_id,
                                    title
                            FROM book";
        try
        {
            var results = new List<Book>();
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
                    var bookAuthorId = (int)reader["author_id"];
                    var author = _authorDao.Get(bookAuthorId);
                    var title = reader["title"] as string;

                    var book = new Book(author, title)
                    {
                        Id = (int)reader["id"]
                    };

                    results.Add(book);
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