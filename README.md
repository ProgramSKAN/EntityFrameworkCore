

# Controlling Database Creation and Schema Changes and migrations
1: Install nuget Microsoft.EntityFrameworkCore.Tools
migrations commands
powershell> Microsoft.EntityFrameworkCore.Tools
dotnet CLI> Microsoft.EntityFrameworkCore.Tools.Dotnet

Migrations APIs
Microsoft.EntityFrameworkCore.Design
(insatlled as a dependency of tools)

run>get-help entityframework

2:
run> add-migration init
run> script-migration  >to generate sql script

use 'update-database' for development database
use 'script-migration' and run that script for production database

3:
now if the database is not existed yet
run> update-database -verbose

dbo._EFMigrationsHistory table have a record of migration history with time snapshot

4:
Database Diagram
ALTER DATABASE SamuraiAppData set TRUSTWORTHY ON; 
GO 
EXEC dbo.sp_changedbowner @loginame = N'sa', @map = false 
GO 
sp_configure 'show advanced options', 1; 
GO 
RECONFIGURE; 
GO 
sp_configure 'clr enabled', 1; 
GO 
RECONFIGURE; 
GO

5:efcore migration from existing schema and data
https://cmatskas.com/ef-core-migrations-with-existing-database-schema-and-data/

6:scaffold-dbcontext form the exixting database 
run> scaffold-dbcontext -provider Microsoft.EntityFramework.SqlServer -connection "Data Source= (localdb)\\MSSQLLocalDB; Initial Catalog = SamuraiAppData"

then do Mapping Many-to-Many and One-to-One Relationships between classes
use class diagram for visualize class relation
to use class diagram:: modify VS> class diagram

7:Visualize how EF core sees your model described by dbcontext and classes
IN SamuraiApp.Data::::
modify VS> DGML Editor
in samurai data Manage Extensions> "EF Core Power Tools"
now add nuget>Microsoft.EntityFrameworkCore.Design
and target multiple><TargetFrameworks>netcoreapp3.0;netstandard2.0</TargetFrameworks> 
right click SamuraiApp.Data>EF Core power tools>Add DBContext Model Diagram

8:
after adding changes in db context
run> add-migration newrelationships
run> update-database

