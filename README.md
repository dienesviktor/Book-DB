# Book-DB

A simple application to store books and authors in a Microsoft SQL Server, using SqlClient package.

Searching for records in a massive set of massive books is time-consuming, for example if a local librarian wants to digitalize his good old book register.

### Description:

1. A BookDbManager class exists, and its ConnectionString property returns a connection string from the app.config file. Its TestConnection() method validate the connectivity to the database.
2. There is an AuthorDao and BookDao classes in the Model directory. Each class accepts the connection string as a constructor parameter. Instances of both of these DAOs are managed by BookDbManager.
3. There is an UserInterface method, which call from BookDbManager, using a main menu to submenu workflow.
4. The user can add, update, and list all entries from the database.

### Instruction:

Database setup:
```
Create a new database in Microsoft SQL Server and import the schema and the data from the books.sql file.
```

To start:
```
Open the BookDb.sln in Visual Studio then press Ctrl+F5 or select the green Start button from the top menu.
```

Navigate:
```
The main menu with options a - Authors, b - Books and q - Quit navigates to the selected submenu and is functional.
The resources menu with options l - List, a - Add, e - Edit and b - Back to main menu is functional on the selected entities.
```
