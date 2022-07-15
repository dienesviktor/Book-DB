using Codecool.BookDb.Manager;
using Codecool.BookDb.Model;
using Codecool.BookDb.View;

public class AuthorManager : BaseManager
{
    private readonly IAuthorDao _authorDao;

    public AuthorManager(UserInterface ui, IAuthorDao authorDao)
        : base(ui)
    {
        _authorDao = authorDao;
    }

    protected override void Add()
    {
        var firstName = _ui.ReadString("First name", "John");
        var lastName = _ui.ReadString("Last name", "Doe");
        var defaultDate = new DateOnly(1900, 5, 28);
        var birthDate = _ui.ReadDate("Birth date", defaultDate);
        _authorDao.Add(new Author(firstName, lastName, birthDate));
    }

    protected override void Edit()
    {
        List();
        var id = _ui.ReadInt("Author ID", 0);
        var author = _authorDao.Get(id);
        if (author == null)
        {
            _ui.PrintLn("Author not found!");
            return;
        }
        _ui.PrintLn(author);

        var firstName = _ui.ReadString("First name", author.FirstName);
        var lastName = _ui.ReadString("Last name", author.LastName);
        var birthDate = _ui.ReadDate("Birth date", author.BirthDate);
        author.FirstName = firstName;
        author.LastName = lastName;
        author.BirthDate = birthDate;
        _authorDao.Update(author);
    }

    protected override string GetName()
    {
        return "Author Manager";
    }

    protected override void List()
    {
        _ui.PrintLn("Registered authors:");
        foreach (var author in _authorDao.GetAll())
        {
            _ui.PrintLn(author);
        }
    }
}