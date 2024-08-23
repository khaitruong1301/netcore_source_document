using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    alias = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, computedColumnSql: "(lower(replace([categoryName],' ','-')))", stored: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__categori__3213E83FFB90EEAF", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    alias = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, computedColumnSql: "(lower(replace([username],' ','-')))", stored: false),
                    passwordHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    fullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__users__3213E83F57CDB3F3", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    alias = table.Column<string>(type: "varchar(25)", unicode: false, maxLength: 25, nullable: true, computedColumnSql: "(lower(concat('notification-',[id])))", stored: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isRead = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__notifica__3213E83FB31D760F", x => x.id);
                    table.ForeignKey(
                        name: "FK__notificat__userI__01142BA1",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    buyerId = table.Column<int>(type: "int", nullable: false),
                    alias = table.Column<string>(type: "varchar(18)", unicode: false, maxLength: 18, nullable: true, computedColumnSql: "(lower(concat('order-',[id])))", stored: false),
                    orderDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    totalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orders__3213E83FA509831D", x => x.id);
                    table.ForeignKey(
                        name: "FK__orders__buyerId__6FE99F9F",
                        column: x => x.buyerId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    alias = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true, computedColumnSql: "(lower(replace([productName],' ','-')))", stored: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    stock = table.Column<int>(type: "int", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: false),
                    sellerId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false),
                    image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__products__3213E83F2147A48A", x => x.id);
                    table.ForeignKey(
                        name: "FK__products__catego__693CA210",
                        column: x => x.categoryId,
                        principalTable: "categories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__products__seller__6A30C649",
                        column: x => x.sellerId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "orderItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    alias = table.Column<string>(type: "varchar(22)", unicode: false, maxLength: 22, nullable: true, computedColumnSql: "(lower(concat('orderItem-',[id])))", stored: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__orderIte__3213E83F32FC5BC5", x => x.id);
                    table.ForeignKey(
                        name: "FK__orderItem__order__74AE54BC",
                        column: x => x.orderId,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__orderItem__produ__75A278F5",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    productId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    alias = table.Column<string>(type: "varchar(19)", unicode: false, maxLength: 19, nullable: true, computedColumnSql: "(lower(concat('review-',[id])))", stored: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    updatedAt = table.Column<DateTime>(type: "datetime", nullable: true),
                    deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__reviews__3213E83F5AE88A26", x => x.id);
                    table.ForeignKey(
                        name: "FK__reviews__product__7A672E12",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK__reviews__userId__7B5B524B",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "UQ__categori__37077ABDC5ABC664",
                table: "categories",
                column: "categoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_notifications_userId",
                table: "notifications",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_orderId",
                table: "orderItems",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_productId",
                table: "orderItems",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_buyerId",
                table: "orders",
                column: "buyerId");

            migrationBuilder.CreateIndex(
                name: "IX_products_categoryId",
                table: "products",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_products_sellerId",
                table: "products",
                column: "sellerId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_productId",
                table: "reviews",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_reviews_userId",
                table: "reviews",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "UQ__users__AB6E6164402ED065",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__users__F3DBC572A882FA44",
                table: "users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "orderItems");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "categories");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
