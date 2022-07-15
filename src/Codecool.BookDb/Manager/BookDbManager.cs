using Codecool.BookDb.Model;
using Codecool.BookDb.View;
using Microsoft.Data.SqlClient;
using System;
using System.Configuration;

namespace Codecool.BookDb.Manager;

public class BookDbManager
{
    private readonly IAuthorDao _authorDao;
    private readonly IBookDao _bookDao;
    private readonly UserInterface _ui;

    public BookDbManager(UserInterface ui)
    {
        _ui = ui ?? throw new ArgumentNullException(nameof(ui));
        EnsureConnectionSuccessful();
        _authorDao = new AuthorDao(ConnectionString);
        _bookDao = new BookDao(ConnectionString, _authorDao);
    }

    public void Run()
    {
        var running = true;

        while (running)
        {
            _ui.Clear();
            _ui.PrintTitle("Main Menu");
            _ui.PrintOption('a', "Authors");
            _ui.PrintOption('b', "Books");
            _ui.PrintOption('q', "Quit");
            switch (_ui.Choice("abq"))
            {
                case 'a':
                    new AuthorManager(_ui, _authorDao).Run();
                    break;
                case 'b':
                    new BookManager(_ui, _bookDao, _authorDao).Run();
                    break;
                case 'q':
                    running = false;
                    break;
            }
        }
    }

    private void EnsureConnectionSuccessful()
    {
        if (!TestConnection())
        {
            _ui.PrintLn("Connection failed, exit!");
            Environment.Exit(1);
        }

        _ui.PrintLn("Connection successful!");
    }

    public string ConnectionString => ConfigurationManager.AppSettings["connectionString"];

    public bool TestConnection()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}