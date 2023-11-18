using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Peliculas.Server.Migrations
{
    /// <inheritdoc />
    public partial class RollAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //https://guidgenerator.com/
            migrationBuilder.Sql(@"Insert into AspNetRoles(Id, Name, NormalizedName)
                                    Values('6e1bec27-5ea3-4038-86f3-421c54a19f92', 'admin', 'ADMIN')");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles WHERE Id = '6e1bec27-5ea3-4038-86f3-421c54a19f92'");
        }
    }
}
