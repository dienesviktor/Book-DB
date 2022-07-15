using Codecool.BookDb.Model;
using Codecool.BookDb.View;

namespace Codecool.BookDb.Manager;

public class BookManager : BaseManager
{
    private readonly IBookDao _bookDao;
    private readonly IAuthorDao _authorDao;

    public BookManager(UserInterface ui, IBookDao bookDao, IAuthorDao authorDao)
        : base(ui)
    {
        _bookDao = bookDao;
        _authorDao = authorDao;
    }

    protected override void Add()
    {
        ListAuthors();
        var authorId = _ui.ReadInt("Author ID", 0);
        var title = _ui.ReadString("Title", "Z");

        var author = _authorDao.Get(authorId);
        if (author == null)
        {
            _ui.PrintLn("Author was not found!");
            return;
        }
        var newBook = new Book(author, title);
        _bookDao.Add(newBook);
    }

    protected override void Edit()
    {
        List();
        var id = _ui.ReadInt("Book ID", 0);
        var book = _bookDao.Get(id);

        if (book == null)
        {
            _ui.PrintLn("Book not found");
            return;
        }
        _ui.PrintLn(book);
        ListAuthors();

        var authorId = _ui.ReadInt("Author ID", book.Author.Id);
        var title = _ui.ReadString("Title", book.Title);

        var author = _authorDao.Get(authorId);
        if (author == null)
        {
            _ui.PrintLn("Author not found!");
            return;
        }
        book.Author = author;
        book.Title = title;
        _bookDao.Update(book);
    }

    private void ListAuthors()
    {
        _ui.PrintLn("Registered authors:");
        foreach (var item in _authorDao.GetAll())
        {
            _ui.PrintLn(item);
        }
    }

    protected override string GetName()
    {
        return "Book Manager";
    }

    protected override void List()
    {
        _ui.PrintLn("Registeresd books:");
        foreach (var item in _bookDao.GetAll())
        {
            _ui.PrintLn(item);
        }
    }
}