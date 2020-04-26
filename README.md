## Working with Views and Stored Procedures and Raw SQL::::::
1:VIEWS
create empty migration
run> add-migration SamuraiBattleStats
if not works > add-migration SamuraiBattleStats -Context SamuraiContext

2:
it cantake raw SQL using
migrationBuilder.Sql()
add views,functions,... here

3:
run> update-database
all the view and functions are updated in db

4:STORED PROCS
run> add-migration NewSprocs -Context SamuraiContext
------------------------------------------------------------
## Using EF Core with ASP.NET Core
1.add reference of API project to data and domain project
2.add new API Controller with actions, using ENTITY FRAMEWORK
Model class> samurai
Data context class> samurai context
3.change in csproj from Microsoft.EntityFrameworkCore.Sqlite to Microsoft.EntityFrameworkCore.SqlServer
4.add "Microsoft.EntityFrameworkCore.Database.command": "Information",connectionstrings in appsettings
5.AddDBContext in dependency injection
6.add constructor in Samuraicontext because whenerver program sees samuraicontext at samuraicontroller then it will ask DI AddDBContext to instantiate samuraicontext and use it.but samuraicontext has to constructor.so add one and keep it no tracking.
no tracking since there is no tracking between one request and another in api.so tracking is a waste of resources.so keep notracking
7.ramove logging code in samuraicontext since ASP.NETCore API has inbuilt logger
8.REST CLIENT CALLS
http://localhost:5000/api/Samurais

###

POST http://localhost:5000/api/Samurais HTTP/1.1
content-type: application/json

{
    "name": "Huachao Mao"
}

###

PUT http://localhost:5000/api/Samurais/1 HTTP/1.1
content-type: application/json

{
    "id": 1,
    "name": "Julie"
}

DELETE  http://localhost:5000/api/Samurais/33

DELETE  http://localhost:5000/api/Samurais/sproc/31
