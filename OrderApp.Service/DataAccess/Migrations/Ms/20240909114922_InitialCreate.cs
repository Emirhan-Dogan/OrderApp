﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations.Ms
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ColorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColorCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupClaims",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupClaims", x => new { x.GroupId, x.ClaimId });
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageTemplate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MobileLogins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ExternalUserId = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Code = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    SendDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSend = table.Column<bool>(type: "bit", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MobileLogins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OperationClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationClaims", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Translates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LangId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Translates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ClaimId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => new { x.ClaimId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => new { x.GroupId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CitizenId = table.Column<long>(type: "bigint", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobilePhones = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    RecordDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UpdateContactDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CityId = table.Column<int>(type: "int", nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    AddressDetail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfReceipt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Addresses_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CustomerCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "Storages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressId = table.Column<int>(type: "int", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Storages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Storages_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false),
                    DateOfReceipt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveryStatus = table.Column<int>(type: "int", nullable: false),
                    DeliveryNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    SizeCode = table.Column<int>(type: "int", nullable: false),
                    ColorId = table.Column<int>(type: "int", nullable: false),
                    StockCount = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    StorageId = table.Column<int>(type: "int", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Colors_ColorId",
                        column: x => x.ColorId,
                        principalTable: "Colors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_Storages_StorageId",
                        column: x => x.StorageId,
                        principalTable: "Storages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Piece = table.Column<int>(type: "int", nullable: false),
                    UnitInPrice = table.Column<double>(type: "float", nullable: false),
                    CreatedUserId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastUpdatedUserId = table.Column<int>(type: "int", nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOrders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductOrders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "tr-TR", "Türkçe" },
                    { 2, "en-US", "English" }
                });

            migrationBuilder.InsertData(
                table: "Translates",
                columns: new[] { "Id", "Code", "LangId", "Value" },
                values: new object[,]
                {
                    { 1, "Login", 1, "Giriş" },
                    { 2, "Email", 1, "E posta" },
                    { 3, "Password", 1, "Parola" },
                    { 4, "Update", 1, "Güncelle" },
                    { 5, "Delete", 1, "Sil" },
                    { 6, "UsersGroups", 1, "Kullanıcının Grupları" },
                    { 7, "UsersClaims", 1, "Kullanıcının Yetkileri" },
                    { 8, "Create", 1, "Yeni" },
                    { 9, "Users", 1, "Kullanıcılar" },
                    { 10, "Groups", 1, "Gruplar" },
                    { 11, "Login", 2, "Login" },
                    { 12, "Email", 2, "Email" },
                    { 13, "Password", 2, "Password" },
                    { 14, "Update", 2, "Update" },
                    { 15, "Delete", 2, "Delete" },
                    { 16, "UsersGroups", 2, "User's Groups" },
                    { 17, "UsersClaims", 2, "User's Claims" },
                    { 18, "Create", 2, "Create" },
                    { 19, "Users", 2, "Users" },
                    { 20, "Groups", 2, "Groups" },
                    { 21, "OperationClaim", 1, "Operasyon Yetkileri" },
                    { 22, "OperationClaim", 2, "Operation Claim" },
                    { 23, "Languages", 1, "Diller" },
                    { 24, "Languages", 2, "Languages" },
                    { 25, "TranslateWords", 1, "Dil Çevirileri" },
                    { 26, "TranslateWords", 2, "Translate Words" },
                    { 27, "Management", 1, "Yönetim" },
                    { 28, "Management", 2, "Management" },
                    { 29, "AppMenu", 1, "Uygulama" },
                    { 30, "AppMenu", 2, "Application" },
                    { 31, "Added", 1, "Başarıyla Eklendi." },
                    { 32, "Added", 2, "Successfully Added." },
                    { 33, "Updated", 1, "Başarıyla Güncellendi." },
                    { 34, "Updated", 2, "Successfully Updated." },
                    { 35, "Deleted", 1, "Başarıyla Silindi." },
                    { 36, "Deleted", 2, "Successfully Deleted." },
                    { 37, "OperationClaimExists", 1, "Bu operasyon izni zaten mevcut." },
                    { 38, "OperationClaimExists", 2, "This operation permit already exists." },
                    { 39, "StringLengthMustBeGreaterThanThree", 1, "Lütfen En Az 3 Karakterden Oluşan Bir İfade Girin." },
                    { 40, "StringLengthMustBeGreaterThanThree", 2, "Please Enter A Phrase Of At Least 3 Characters." },
                    { 41, "CouldNotBeVerifyCid", 1, "Kimlik No Doğrulanamadı." },
                    { 42, "CouldNotBeVerifyCid", 2, "Could not be verify Citizen Id" },
                    { 43, "VerifyCid", 1, "Kimlik No Doğrulandı." },
                    { 44, "VerifyCid", 2, "Verify Citizen Id" },
                    { 45, "AuthorizationsDenied", 1, "Yetkiniz olmayan bir alana girmeye çalıştığınız tespit edildi." },
                    { 46, "AuthorizationsDenied", 2, "It has been detected that you are trying to enter an area that you do not have authorization." },
                    { 47, "UserNotFound", 1, "Kimlik Bilgileri Doğrulanamadı. Lütfen Yeni Kayıt Ekranını kullanın." },
                    { 48, "UserNotFound", 2, "Credentials Could Not Verify. Please use the New Registration Screen." },
                    { 49, "PasswordError", 1, "Kimlik Bilgileri Doğrulanamadı, Kullanıcı adı ve/veya parola hatalı." },
                    { 50, "PasswordError", 2, "Credentials Failed to Authenticate, Username and / or password incorrect." },
                    { 51, "SuccessfulLogin", 1, "Sisteme giriş başarılı." },
                    { 52, "SuccessfulLogin", 2, "Login to the system is successful." },
                    { 53, "SendMobileCode", 1, "Lütfen Size SMS Olarak Gönderilen Kodu Girin!" },
                    { 54, "SendMobileCode", 2, "Please Enter The Code Sent To You By SMS!" },
                    { 55, "NameAlreadyExist", 1, "Oluşturmaya Çalıştığınız Nesne Zaten Var." },
                    { 56, "NameAlreadyExist", 2, "The Object You Are Trying To Create Already Exists." },
                    { 57, "WrongCID", 1, "Vatandaşlık No Sistemimizde Bulunamadı. Lütfen Yeni Kayıt Oluşturun!" },
                    { 58, "WrongCID", 2, "Citizenship Number Not Found In Our System. Please Create New Registration!" },
                    { 59, "CID", 1, "Vatandaşlık No" },
                    { 60, "CID", 2, "Citizenship Number" },
                    { 61, "PasswordEmpty", 1, "Parola boş olamaz!" },
                    { 62, "PasswordEmpty", 2, "Password can not be empty!" },
                    { 63, "PasswordLength", 1, "Minimum 8 Karakter Uzunluğunda Olmalıdır!" },
                    { 64, "PasswordLength", 2, "Must be at least 8 characters long! " },
                    { 65, "PasswordUppercaseLetter", 1, "En Az 1 Büyük Harf İçermelidir!" },
                    { 66, "PasswordUppercaseLetter", 2, "Must Contain At Least 1 Capital Letter!" },
                    { 67, "PasswordLowercaseLetter", 1, "En Az 1 Küçük Harf İçermelidir!" },
                    { 68, "PasswordLowercaseLetter", 2, "Must Contain At Least 1 Lowercase Letter!" },
                    { 69, "PasswordDigit", 1, "En Az 1 Rakam İçermelidir!" },
                    { 70, "PasswordDigit", 2, "It Must Contain At Least 1 Digit!" },
                    { 71, "PasswordSpecialCharacter", 1, "En Az 1 Simge İçermelidir!" },
                    { 72, "PasswordSpecialCharacter", 2, "Must Contain At Least 1 Symbol!" },
                    { 73, "SendPassword", 1, "Yeni Parolanız E-Posta Adresinize Gönderildi." },
                    { 74, "SendPassword", 2, "Your new password has been sent to your e-mail address." },
                    { 75, "InvalidCode", 1, "Geçersiz Bir Kod Girdiniz!" },
                    { 76, "InvalidCode", 2, "You Entered An Invalid Code!" },
                    { 77, "SmsServiceNotFound", 1, "SMS Servisine Ulaşılamıyor." },
                    { 78, "SmsServiceNotFound", 2, "Unable to Reach SMS Service." },
                    { 79, "TrueButCellPhone", 1, "Bilgiler doğru. Cep telefonu gerekiyor." },
                    { 80, "TrueButCellPhone", 2, "The information is correct. Cell phone is required." },
                    { 81, "TokenProviderException", 1, "Token Provider boş olamaz!" },
                    { 82, "TokenProviderException", 2, "Token Provider cannot be empty!" },
                    { 83, "Unknown", 1, "Bilinmiyor!" },
                    { 84, "Unknown", 2, "Unknown!" },
                    { 85, "NewPassword", 1, "Yeni Parola:" },
                    { 86, "NewPassword", 2, "New Password:" },
                    { 87, "ChangePassword", 1, "Parola Değiştir" },
                    { 88, "ChangePassword", 2, "Change Password" },
                    { 89, "Save", 1, "Kaydet" },
                    { 90, "Save", 2, "Save" },
                    { 91, "GroupName", 1, "Grup Adı" },
                    { 92, "GroupName", 2, "Group Name" },
                    { 93, "FullName", 1, "Tam Adı" },
                    { 94, "FullName", 2, "Full Name" },
                    { 95, "Address", 1, "Adres" },
                    { 96, "Address", 2, "Address" },
                    { 97, "Notes", 1, "Notlar" },
                    { 98, "Notes", 2, "Notes" },
                    { 99, "ConfirmPassword", 1, "Parolayı Doğrula" },
                    { 100, "ConfirmPassword", 2, "Confirm Password" },
                    { 101, "Code", 1, "Kod" },
                    { 102, "Code", 2, "Code" },
                    { 103, "Alias", 1, "Görünen Ad" },
                    { 104, "Alias", 2, "Alias" },
                    { 105, "Description", 1, "Açıklama" },
                    { 106, "Description", 2, "Description" },
                    { 107, "Value", 1, "Değer" },
                    { 108, "Value", 2, "Value" },
                    { 109, "LangCode", 1, "Dil Kodu" },
                    { 110, "LangCode", 2, "Lang Code" },
                    { 111, "Name", 1, "Adı" },
                    { 112, "Name", 2, "Name" },
                    { 113, "MobilePhones", 1, "Cep Telefonu" },
                    { 114, "MobilePhones", 2, "Mobile Phone" },
                    { 115, "NoRecordsFound", 1, "Kayıt Bulunamadı" },
                    { 116, "NoRecordsFound", 2, "No Records Found" },
                    { 117, "Required", 1, "Bu alan zorunludur!" },
                    { 118, "Required", 2, "This field is required!" },
                    { 119, "Permissions", 1, "Permissions" },
                    { 120, "Permissions", 2, "İzinler" },
                    { 121, "GroupList", 1, "Grup Listesi" },
                    { 122, "GroupList", 2, "Group List" },
                    { 123, "GrupPermissions", 1, "Grup Yetkileri" },
                    { 124, "GrupPermissions", 2, "Grup Permissions" },
                    { 125, "Add", 1, "Ekle" },
                    { 126, "Add", 2, "Add" },
                    { 127, "UserList", 1, "Kullanıcı Listesi" },
                    { 128, "UserList", 2, "User List" },
                    { 129, "OperationClaimList", 1, "Yetki Listesi" },
                    { 130, "OperationClaimList", 2, "OperationClaim List" },
                    { 131, "LanguageList", 1, "Dil Listesi" },
                    { 132, "LanguageList", 2, "Language List" },
                    { 133, "TranslateList", 1, "Dil Çeviri Listesi" },
                    { 134, "TranslateList", 2, "Translate List" },
                    { 135, "LogList", 1, "İşlem Kütüğü" },
                    { 136, "LogList", 2, "LogList" },
                    { 137, "DeleteConfirm", 1, "Emin misiniz?" },
                    { 138, "DeleteConfirm", 2, "Are you sure?" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CityId",
                table: "Addresses",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_AddressId",
                table: "Customers",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MobileLogins_ExternalUserId_Provider",
                table: "MobileLogins",
                columns: new[] { "ExternalUserId", "Provider" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_OrderId",
                table: "ProductOrders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_ProductId",
                table: "ProductOrders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ColorId",
                table: "Products",
                column: "ColorId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StorageId",
                table: "Products",
                column: "StorageId");

            migrationBuilder.CreateIndex(
                name: "IX_Storages_AddressId",
                table: "Storages",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CitizenId",
                table: "Users",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_MobilePhones",
                table: "Users",
                column: "MobilePhones");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupClaims");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Logs");

            migrationBuilder.DropTable(
                name: "MobileLogins");

            migrationBuilder.DropTable(
                name: "OperationClaims");

            migrationBuilder.DropTable(
                name: "ProductOrders");

            migrationBuilder.DropTable(
                name: "Translates");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Storages");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
