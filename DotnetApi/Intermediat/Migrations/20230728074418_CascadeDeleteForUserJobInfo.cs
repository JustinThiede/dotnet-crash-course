using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intermediate.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForUserJobInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TutorialAppSchema");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "TutorialAppSchema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserJobInfo",
                schema: "TutorialAppSchema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJobInfo", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserJobInfo_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "TutorialAppSchema",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSalary",
                schema: "TutorialAppSchema",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSalary", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_UserSalary_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "TutorialAppSchema",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserJobInfo",
                schema: "TutorialAppSchema");

            migrationBuilder.DropTable(
                name: "UserSalary",
                schema: "TutorialAppSchema");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "TutorialAppSchema");
        }
    }
}
