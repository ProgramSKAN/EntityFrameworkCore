﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace SamuraiApp.Data.Migrations
{
    public partial class NewSprocs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE PROCEDURE dbo.SamuraiWhoSaidAWord
                    @text VARCHAR(20)
                    AS
                    SELECT Samurais.Id,Samurais.Name,Samurais.ClanId
                    FROM   Samurais
                    INNER JOIN Quotes ON Samurais.Id=Quotes.SamuraiId
                    WHERE (Quotes.Text Like '%'+@text+'%')");
            migrationBuilder.Sql(
                @"CREATE PROCEDURE dbo.DeleteQuotesForSamurai
                    @SamuraiId int
                    AS
                    DELETE FROM Quotes
                    WHERE Quotes.SamuraiId=@samuraiId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE dbo.SamuraisWhoSaidAWord");
            migrationBuilder.Sql("DROP PROCEDURE dbo.DeleteQuotesForSamurai");
        }
    }
}
