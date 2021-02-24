# Thesaurus
This application gets and stores synonyms. Note that only the backend side is implemented.

## The Task 
The main task of the project is to implement the given interface Ithesaurus.cs, the file that is implementing it is 'thesaurus/SynonymApp/Controllers/Thesaurus.cs'.

## Languages, frameworks and tools etc.
1. .NET Core
2. C#
3. Entity Framework
4. xUnit.net

## Requirements before starting
1. .NET Core
2. Any SQL Server

## Getting Started
1. Start the database server
2. Create a database called 'thesaurus'
3. Run 'thesaurus/DatabaseScript.sql' on the database to create the tables
4. Open the file 'thesaurus/SynonymApp/ServerConfig.cs'
5. Add the database configuration here: server, database, user, password
6. Start the terminal
7. Go to the folder 'thesaurus/SynonymApp'
8. Type "dotnet restore"
9. Go to the folder 'thesaurus/Tests'
10. Type "dotnet restore"
11. Go to the root folder 'thesaurus'
12. Type "dotnet test"


