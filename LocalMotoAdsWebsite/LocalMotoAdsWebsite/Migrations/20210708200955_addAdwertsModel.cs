using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalMotoAdsWebsite.Migrations
{
    public partial class addAdwertsModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Adverts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    Descritpion = table.Column<string>(nullable: true),
                    VIN = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    CarMileage = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    ImagePath = table.Column<string>(nullable: true),
                    ModelFK = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Adverts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Adverts_Models_ModelFK",
                        column: x => x.ModelFK,
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Adverts_ModelFK",
                table: "Adverts",
                column: "ModelFK");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Adverts");
        }
    }
}
