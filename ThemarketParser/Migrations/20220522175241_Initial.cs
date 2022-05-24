using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThemarketParser.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryAbstract",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    parentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sizesName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryAbstract", x => x.id);
                    table.ForeignKey(
                        name: "FK_CategoryAbstract_CategoryAbstract_parentCategoryId",
                        column: x => x.parentCategoryId,
                        principalTable: "CategoryAbstract",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Condition",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    trnslation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Condition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    eur = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    us = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Sizes_CategoryAbstract_categoryId",
                        column: x => x.categoryId,
                        principalTable: "CategoryAbstract",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    userID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    addedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    prettyPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    likesCount = table.Column<int>(type: "int", nullable: false),
                    deliveryPrice = table.Column<int>(type: "int", nullable: false),
                    sexCategoryId = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    concreteCategoryId = table.Column<int>(type: "int", nullable: false),
                    sizeId = table.Column<int>(type: "int", nullable: false),
                    conditionId = table.Column<int>(type: "int", nullable: false),
                    cityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.id);
                    table.ForeignKey(
                        name: "FK_Items_CategoryAbstract_categoryId",
                        column: x => x.categoryId,
                        principalTable: "CategoryAbstract",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Items_CategoryAbstract_concreteCategoryId",
                        column: x => x.concreteCategoryId,
                        principalTable: "CategoryAbstract",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Items_CategoryAbstract_sexCategoryId",
                        column: x => x.sexCategoryId,
                        principalTable: "CategoryAbstract",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_Items_Cities_cityId",
                        column: x => x.cityId,
                        principalTable: "Cities",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Condition_conditionId",
                        column: x => x.conditionId,
                        principalTable: "Condition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Items_Sizes_sizeId",
                        column: x => x.sizeId,
                        principalTable: "Sizes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BrandItem",
                columns: table => new
                {
                    brandsid = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    itemsid = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandItem", x => new { x.brandsid, x.itemsid });
                    table.ForeignKey(
                        name: "FK_BrandItem_Brands_brandsid",
                        column: x => x.brandsid,
                        principalTable: "Brands",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandItem_Items_itemsid",
                        column: x => x.itemsid,
                        principalTable: "Items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    itemId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.id);
                    table.ForeignKey(
                        name: "FK_Images_Items_itemId",
                        column: x => x.itemId,
                        principalTable: "Items",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandItem_itemsid",
                table: "BrandItem",
                column: "itemsid");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryAbstract_parentCategoryId",
                table: "CategoryAbstract",
                column: "parentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_itemId",
                table: "Images",
                column: "itemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_categoryId",
                table: "Items",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_cityId",
                table: "Items",
                column: "cityId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_concreteCategoryId",
                table: "Items",
                column: "concreteCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_conditionId",
                table: "Items",
                column: "conditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_sexCategoryId",
                table: "Items",
                column: "sexCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_sizeId",
                table: "Items",
                column: "sizeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sizes_categoryId",
                table: "Sizes",
                column: "categoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandItem");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Condition");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "CategoryAbstract");
        }
    }
}
