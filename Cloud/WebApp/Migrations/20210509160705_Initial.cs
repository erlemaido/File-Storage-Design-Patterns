using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cloud");

            migrationBuilder.CreateTable(
                name: "product_state_type",
                schema: "cloud",
                columns: table => new
                {
                    product_state_type_code = table.Column<short>(type: "smallint", nullable: false),
                    title = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_state_type", x => x.product_state_type_code);
                });

            migrationBuilder.CreateTable(
                name: "product",
                schema: "cloud",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "serial", nullable: false),
                    product_code = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "decimal(19,4)", nullable: false),
                    product_state_type_code = table.Column<short>(type: "smallint", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_product_product_state_type_product_state_type_code",
                        column: x => x.product_state_type_code,
                        principalSchema: "cloud",
                        principalTable: "product_state_type",
                        principalColumn: "product_state_type_code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_picture",
                schema: "cloud",
                columns: table => new
                {
                    product_picture_id = table.Column<long>(type: "bigserial", nullable: false),
                    seq_nr = table.Column<short>(type: "smallint", nullable: false),
                    picture_url = table.Column<string>(type: "varchar", nullable: true),
                    product_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_picture", x => x.product_picture_id);
                    table.ForeignKey(
                        name: "FK_product_picture_product_product_id",
                        column: x => x.product_id,
                        principalSchema: "cloud",
                        principalTable: "product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "cloud",
                table: "product_state_type",
                columns: new[] { "product_state_type_code", "title" },
                values: new object[,]
                {
                    { (short)1, "Ootel" },
                    { (short)2, "Aktiivne" },
                    { (short)3, "Mitteaktiivne" },
                    { (short)4, "Lõpetatud" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_product_product_state_type_code",
                schema: "cloud",
                table: "product",
                column: "product_state_type_code");

            migrationBuilder.CreateIndex(
                name: "IX_product_picture_product_id_seq_nr",
                schema: "cloud",
                table: "product_picture",
                columns: new[] { "product_id", "seq_nr" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_state_type_title",
                schema: "cloud",
                table: "product_state_type",
                column: "title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product_picture",
                schema: "cloud");

            migrationBuilder.DropTable(
                name: "product",
                schema: "cloud");

            migrationBuilder.DropTable(
                name: "product_state_type",
                schema: "cloud");
        }
    }
}
