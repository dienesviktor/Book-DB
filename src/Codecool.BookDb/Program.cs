using Codecool.BookDb.Manager;
using Codecool.BookDb.Model;
using Codecool.BookDb.View;

namespace Codecool.BookDb;

public static class Program
{
    public static void Main()
    {
        var ui = new UserInterface();
        var bookDbManager = new BookDbManager(ui);
        bookDbManager.Run();
    }
}

