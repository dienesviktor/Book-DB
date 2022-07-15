using Codecool.BookDb.View;

namespace Codecool.BookDb.Manager;

public abstract class BaseManager
{
    protected readonly UserInterface _ui;
    protected abstract string GetName();
    protected abstract void List();
    protected abstract void Add();
    protected abstract void Edit();

    public BaseManager(UserInterface ui)
    {
        _ui = ui;
    }

    public void Run()
    {
        _ui.Clear();
        var running = true;
        while (running)
        {
            _ui.PrintTitle(GetName());
            _ui.PrintOption('l', "List");
            _ui.PrintOption('a', "Add");
            _ui.PrintOption('e', "Edit");
            _ui.PrintOption('b', "Back to main menu");

            switch (_ui.Choice("laeb"))
            {
                case 'l':
                    List();
                    break;
                case 'a':
                    Add();
                    break;
                case 'e':
                    Edit();
                    break;
                case 'b':
                    running = false;
                    break;
                default:
                    break;
            }
        }
    }
}