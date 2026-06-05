using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Migrations
{
    /// <inheritdoc />
    public partial class all : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tbl_Admin",
                columns: table => new
                {
                    Admin_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Admin_Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Admin", x => x.Admin_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Cart",
                columns: table => new
                {
                    cart_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    prod_id = table.Column<int>(type: "int", nullable: false),
                    cust_id = table.Column<int>(type: "int", nullable: false),
                    product_quantity = table.Column<int>(type: "int", nullable: false),
                    cart_status = table.Column<int>(type: "int", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Cart", x => x.cart_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Category",
                columns: table => new
                {
                    Category_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Category_Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Category", x => x.Category_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Customer",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    customer_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_city = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    customer_image = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Customer", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Faqs",
                columns: table => new
                {
                    faqs_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    faqs_Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    faqs_Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Faqs", x => x.faqs_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Feedback",
                columns: table => new
                {
                    feedback_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    user_message = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Feedback", x => x.feedback_Id);
                });

            migrationBuilder.CreateTable(
                name: "tbl_Product",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    product_image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cat_id = table.Column<int>(type: "int", nullable: false),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Publisher = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_Product", x => x.product_id);
                    table.ForeignKey(
                        name: "FK_tbl_Product_tbl_Category_cat_id",
                        column: x => x.cat_id,
                        principalTable: "tbl_Category",
                        principalColumn: "Category_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_order",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderNumber = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerPhone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    DeliveryCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeliveredDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanCancel = table.Column<bool>(type: "bit", nullable: false),
                    CancelledDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationCharges = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_tbl_order_tbl_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "tbl_Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tbl_orderitem",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tbl_orderitem", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_tbl_orderitem_tbl_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "tbl_Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tbl_orderitem_tbl_order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "tbl_order",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tbl_order_CustomerId",
                table: "tbl_order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_orderitem_OrderId",
                table: "tbl_orderitem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_orderitem_ProductId",
                table: "tbl_orderitem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_Product_cat_id",
                table: "tbl_Product",
                column: "cat_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tbl_Admin");

            migrationBuilder.DropTable(
                name: "tbl_Cart");

            migrationBuilder.DropTable(
                name: "tbl_Faqs");

            migrationBuilder.DropTable(
                name: "tbl_Feedback");

            migrationBuilder.DropTable(
                name: "tbl_orderitem");

            migrationBuilder.DropTable(
                name: "tbl_Product");

            migrationBuilder.DropTable(
                name: "tbl_order");

            migrationBuilder.DropTable(
                name: "tbl_Category");

            migrationBuilder.DropTable(
                name: "tbl_Customer");
        }
    }
}
