using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VideoRentalWeb.Migrations
{
    public partial class MigrateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetRoles", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_AspNetUsers", x => x.Id); });

            //migrationBuilder.CreateTable(
            //    name: "Clientele",
            //    columns: table => new
            //    {
            //        ClientID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Surname = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Middlename = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
            //        Addres = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
            //        Phone = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
            //        Passport = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Clientel__E67E1A04315A897C", x => x.ClientID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Genres",
            //    columns: table => new
            //    {
            //        GenreID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Genres", x => x.GenreID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Positions",
            //    columns: table => new
            //    {
            //        PositionID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Positions", x => x.PositionID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Producers",
            //    columns: table => new
            //    {
            //        ProduceID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Manufacturer = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Country = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Producer__2EEBECB32A180669", x => x.ProduceID);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Types",
            //    columns: table => new
            //    {
            //        TypeID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Description = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Types", x => x.TypeID);
            //    });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "Staff",
            //    columns: table => new
            //    {
            //        StaffID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Surname = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Name = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        Middlename = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
            //        PositionID = table.Column<int>(type: "int", nullable: true),
            //        DateOfEmployment = table.Column<DateTime>(type: "date", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Staff", x => x.StaffID);
            //        table.ForeignKey(
            //            name: "FK__Staff__PositionI__47DBAE45",
            //            column: x => x.PositionID,
            //            principalTable: "Positions",
            //            principalColumn: "PositionID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Disks",
            //    columns: table => new
            //    {
            //        DiskID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Title = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
            //        CreationYear = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true),
            //        Producer = table.Column<int>(type: "int", nullable: false),
            //        MainActor = table.Column<string>(type: "varchar(90)", unicode: false, maxLength: 90, nullable: false),
            //        Recording = table.Column<DateTime>(type: "date", nullable: false),
            //        GenreID = table.Column<int>(type: "int", nullable: false),
            //        DiskType = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Disks", x => x.DiskID);
            //        table.ForeignKey(
            //            name: "FK__Disks__DiskType__3F466844",
            //            column: x => x.DiskType,
            //            principalTable: "Types",
            //            principalColumn: "TypeID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK__Disks__GenreID__3E52440B",
            //            column: x => x.GenreID,
            //            principalTable: "Genres",
            //            principalColumn: "GenreID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK__Disks__Producer__3D5E1FD2",
            //            column: x => x.Producer,
            //            principalTable: "Producers",
            //            principalColumn: "ProduceID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Pricelist",
            //    columns: table => new
            //    {
            //        PriceID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        DiskID = table.Column<int>(type: "int", nullable: false),
            //        Price = table.Column<decimal>(type: "money", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Pricelis__4957584FC87A6E36", x => x.PriceID);
            //        table.ForeignKey(
            //            name: "FK__Pricelist__DiskI__4316F928",
            //            column: x => x.DiskID,
            //            principalTable: "Disks",
            //            principalColumn: "DiskID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Taking",
            //    columns: table => new
            //    {
            //        TakeID = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ClientID = table.Column<int>(type: "int", nullable: false),
            //        DiskID = table.Column<int>(type: "int", nullable: false),
            //        DateOfCapture = table.Column<DateTime>(type: "date", nullable: false),
            //        ReturnDate = table.Column<DateTime>(type: "date", nullable: false),
            //        PaymentMark = table.Column<bool>(type: "bit", nullable: false),
            //        RefundMark = table.Column<bool>(type: "bit", nullable: false),
            //        StaffID = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK__Taking__AC0C2240A69151F3", x => x.TakeID);
            //        table.ForeignKey(
            //            name: "FK__Taking__ClientID__4D94879B",
            //            column: x => x.ClientID,
            //            principalTable: "Clientele",
            //            principalColumn: "ClientID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK__Taking__DiskID__4E88ABD4",
            //            column: x => x.DiskID,
            //            principalTable: "Disks",
            //            principalColumn: "DiskID",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK__Taking__StaffID__4F7CD00D",
            //            column: x => x.StaffID,
            //            principalTable: "Staff",
            //            principalColumn: "StaffID",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Disks_DiskType",
        //        table: "Disks",
        //        column: "DiskType");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Disks_GenreID",
        //        table: "Disks",
        //        column: "GenreID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Disks_Producer",
        //        table: "Disks",
        //        column: "Producer");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Pricelist_DiskID",
        //        table: "Pricelist",
        //        column: "DiskID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Staff_PositionID",
        //        table: "Staff",
        //        column: "PositionID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Taking_ClientID",
        //        table: "Taking",
        //        column: "ClientID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Taking_DiskID",
        //        table: "Taking",
        //        column: "DiskID");

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Taking_StaffID",
        //        table: "Taking",
        //        column: "StaffID");
        //}

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Pricelist");

            migrationBuilder.DropTable(
                name: "Taking");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Clientele");

            migrationBuilder.DropTable(
                name: "Disks");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "Types");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Producers");

            migrationBuilder.DropTable(
                name: "Positions");
        }
    }
}
