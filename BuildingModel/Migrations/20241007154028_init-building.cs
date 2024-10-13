using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuildingModels.Migrations
{
    /// <inheritdoc />
    public partial class initbuilding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contributor",
                columns: table => new
                {
                    Staff_Id = table.Column<int>(type: "int", nullable: true),
                    ThirdParty_Id = table.Column<int>(type: "int", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Finance",
                columns: table => new
                {
                    Finance_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinanceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Maintain = table.Column<double>(type: "float", nullable: true),
                    IncidentalChanges = table.Column<double>(type: "float", nullable: true),
                    serviceFee = table.Column<double>(type: "float", nullable: true),
                    providerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Finance", x => x.Finance_Id);
                });

            migrationBuilder.CreateTable(
                name: "NotifyCategory",
                columns: table => new
                {
                    NewCategory_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotifyCategory", x => x.NewCategory_Id);
                });

            migrationBuilder.CreateTable(
                name: "Postion",
                columns: table => new
                {
                    Code_Position = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CustomerID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Postion", x => x.Code_Position);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Role_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RollName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Role_Id);
                });

            migrationBuilder.CreateTable(
                name: "Salary",
                columns: table => new
                {
                    Staff_Id = table.Column<int>(type: "int", nullable: true),
                    Roll_Id = table.Column<int>(type: "int", nullable: true),
                    AmoutSalary = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Service_Id = table.Column<int>(type: "int", nullable: false),
                    Service_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Service_Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Service_Id);
                });

            migrationBuilder.CreateTable(
                name: "Staff",
                columns: table => new
                {
                    Staff_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Password = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Phone_Number = table.Column<int>(type: "int", nullable: true),
                    HireDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Role_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Staff", x => x.Staff_Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Status_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status_Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Status_Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Task_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Task_Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Task_Id);
                });

            migrationBuilder.CreateTable(
                name: "ThirdParty",
                columns: table => new
                {
                    ThirdParty_Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Type = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Contact = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdParty", x => x.ThirdParty_Id);
                });

            migrationBuilder.CreateTable(
                name: "ThirdPartyContact",
                columns: table => new
                {
                    Staff_Id = table.Column<int>(type: "int", nullable: true),
                    Building_Id = table.Column<int>(type: "int", nullable: true),
                    Start_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    End_Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Building_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Building_Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Number_Floor = table.Column<int>(type: "int", nullable: true),
                    Number_Apartment = table.Column<int>(type: "int", nullable: true),
                    Code_Position = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Building_Id);
                    table.ForeignKey(
                        name: "FK_Building_Postion",
                        column: x => x.Code_Position,
                        principalTable: "Postion",
                        principalColumn: "Code_Position");
                });

            migrationBuilder.CreateTable(
                name: "Assigment",
                columns: table => new
                {
                    Assigment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Staff_Id = table.Column<int>(type: "int", nullable: true),
                    Task_Id = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    ServicePrice = table.Column<double>(type: "float", nullable: true),
                    ServiceFee = table.Column<double>(type: "float", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assigment", x => x.Assigment_Id);
                    table.ForeignKey(
                        name: "FK_Assigment_Task",
                        column: x => x.Task_Id,
                        principalTable: "Task",
                        principalColumn: "Task_Id");
                });

            migrationBuilder.CreateTable(
                name: "Apartment",
                columns: table => new
                {
                    Apartment_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Building_Id = table.Column<int>(type: "int", nullable: false),
                    DepartmentType = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Price = table.Column<double>(type: "float", nullable: true),
                    Floor = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Apartment", x => x.Apartment_Id);
                    table.ForeignKey(
                        name: "FK_Apartment_Building",
                        column: x => x.Building_Id,
                        principalTable: "Building",
                        principalColumn: "Building_Id");
                });

            migrationBuilder.CreateTable(
                name: "Finance_Building",
                columns: table => new
                {
                    Building_Id = table.Column<int>(type: "int", nullable: false),
                    Finance_Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Finance_Building_Building",
                        column: x => x.Building_Id,
                        principalTable: "Building",
                        principalColumn: "Building_Id");
                    table.ForeignKey(
                        name: "FK_Finance_Building_Finance",
                        column: x => x.Finance_Id,
                        principalTable: "Finance",
                        principalColumn: "Finance_Id");
                });

            migrationBuilder.CreateTable(
                name: "Notify",
                columns: table => new
                {
                    NewCategory_Id = table.Column<int>(type: "int", nullable: false),
                    Assigment_Id = table.Column<int>(type: "int", nullable: true),
                    StartTime = table.Column<DateTime>(type: "datetime", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_Notify_Assigment",
                        column: x => x.Assigment_Id,
                        principalTable: "Assigment",
                        principalColumn: "Assigment_Id");
                    table.ForeignKey(
                        name: "FK_Notify_NotifyCategory",
                        column: x => x.NewCategory_Id,
                        principalTable: "NotifyCategory",
                        principalColumn: "NewCategory_Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    Invoice_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apartment_ID = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: true),
                    Issue_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Due_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status_Id = table.Column<int>(type: "int", nullable: false),
                    Transaction_Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.Invoice_Id);
                    table.ForeignKey(
                        name: "FK_Invoice_Apartment",
                        column: x => x.Apartment_ID,
                        principalTable: "Apartment",
                        principalColumn: "Apartment_Id");
                    table.ForeignKey(
                        name: "FK_Invoice_Status",
                        column: x => x.Status_Id,
                        principalTable: "Status",
                        principalColumn: "Status_Id");
                });

            migrationBuilder.CreateTable(
                name: "OwnerShip",
                columns: table => new
                {
                    OwnerShip_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Apartment_Id = table.Column<int>(type: "int", nullable: true),
                    Start_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    End_Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OwnerShip_1", x => x.OwnerShip_Id);
                    table.ForeignKey(
                        name: "FK_OwnerShip_Apartment",
                        column: x => x.Apartment_Id,
                        principalTable: "Apartment",
                        principalColumn: "Apartment_Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceContract",
                columns: table => new
                {
                    Apartment_Id = table.Column<int>(type: "int", nullable: false),
                    Service_Id = table.Column<int>(type: "int", nullable: false),
                    Start_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    End_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_ServiceContract_Apartment",
                        column: x => x.Apartment_Id,
                        principalTable: "Apartment",
                        principalColumn: "Apartment_Id");
                    table.ForeignKey(
                        name: "FK_ServiceContract_Service",
                        column: x => x.Service_Id,
                        principalTable: "Service",
                        principalColumn: "Service_Id");
                });

            migrationBuilder.CreateTable(
                name: "Resident",
                columns: table => new
                {
                    Resident_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Password = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Fullname = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Phone_Number = table.Column<int>(type: "int", nullable: true),
                    OwnerShip_Id = table.Column<int>(type: "int", nullable: true),
                    DOB = table.Column<DateOnly>(type: "date", nullable: true),
                    RegistratorDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resident", x => x.Resident_Id);
                    table.ForeignKey(
                        name: "FK_Resident_OwnerShip",
                        column: x => x.OwnerShip_Id,
                        principalTable: "OwnerShip",
                        principalColumn: "OwnerShip_Id");
                });

            migrationBuilder.CreateTable(
                name: "Living",
                columns: table => new
                {
                    Living_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resident_Id = table.Column<int>(type: "int", nullable: true),
                    Apartment_Id = table.Column<int>(type: "int", nullable: true),
                    Start_Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    End_Date = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Living", x => x.Living_Id);
                    table.ForeignKey(
                        name: "FK_Living_Apartment",
                        column: x => x.Apartment_Id,
                        principalTable: "Apartment",
                        principalColumn: "Apartment_Id");
                    table.ForeignKey(
                        name: "FK_Living_Resident",
                        column: x => x.Resident_Id,
                        principalTable: "Resident",
                        principalColumn: "Resident_Id");
                });

            migrationBuilder.CreateTable(
                name: "RequestComplain",
                columns: table => new
                {
                    Requestion_Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Resident_Id = table.Column<int>(type: "int", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Date_Request = table.Column<DateTime>(type: "datetime", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestComplain", x => x.Requestion_Id);
                    table.ForeignKey(
                        name: "FK_RequestComplain_Resident",
                        column: x => x.Resident_Id,
                        principalTable: "Resident",
                        principalColumn: "Resident_Id");
                });

            migrationBuilder.CreateTable(
                name: "HandleRequest",
                columns: table => new
                {
                    Requestion_Id = table.Column<int>(type: "int", nullable: false),
                    Assigment_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_HandleRequest_Assigment",
                        column: x => x.Assigment_Id,
                        principalTable: "Assigment",
                        principalColumn: "Assigment_Id");
                    table.ForeignKey(
                        name: "FK_HandleRequest_RequestComplain",
                        column: x => x.Requestion_Id,
                        principalTable: "RequestComplain",
                        principalColumn: "Requestion_Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Apartment_Building_Id",
                table: "Apartment",
                column: "Building_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Assigment_Task_Id",
                table: "Assigment",
                column: "Task_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Building_Code_Position",
                table: "Building",
                column: "Code_Position");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Building_Building_Id",
                table: "Finance_Building",
                column: "Building_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Finance_Building_Finance_Id",
                table: "Finance_Building",
                column: "Finance_Id");

            migrationBuilder.CreateIndex(
                name: "IX_HandleRequest_Assigment_Id",
                table: "HandleRequest",
                column: "Assigment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_HandleRequest_Requestion_Id",
                table: "HandleRequest",
                column: "Requestion_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Apartment_ID",
                table: "Invoice",
                column: "Apartment_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_Status_Id",
                table: "Invoice",
                column: "Status_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Living_Apartment_Id",
                table: "Living",
                column: "Apartment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Living_Resident_Id",
                table: "Living",
                column: "Resident_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notify_Assigment_Id",
                table: "Notify",
                column: "Assigment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Notify_NewCategory_Id",
                table: "Notify",
                column: "NewCategory_Id");

            migrationBuilder.CreateIndex(
                name: "IX_OwnerShip_Apartment_Id",
                table: "OwnerShip",
                column: "Apartment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_RequestComplain_Resident_Id",
                table: "RequestComplain",
                column: "Resident_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resident_OwnerShip_Id",
                table: "Resident",
                column: "OwnerShip_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_Apartment_Id",
                table: "ServiceContract",
                column: "Apartment_Id");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceContract_Service_Id",
                table: "ServiceContract",
                column: "Service_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contributor");

            migrationBuilder.DropTable(
                name: "Finance_Building");

            migrationBuilder.DropTable(
                name: "HandleRequest");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Living");

            migrationBuilder.DropTable(
                name: "Notify");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Salary");

            migrationBuilder.DropTable(
                name: "ServiceContract");

            migrationBuilder.DropTable(
                name: "Staff");

            migrationBuilder.DropTable(
                name: "ThirdParty");

            migrationBuilder.DropTable(
                name: "ThirdPartyContact");

            migrationBuilder.DropTable(
                name: "Finance");

            migrationBuilder.DropTable(
                name: "RequestComplain");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Assigment");

            migrationBuilder.DropTable(
                name: "NotifyCategory");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Resident");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "OwnerShip");

            migrationBuilder.DropTable(
                name: "Apartment");

            migrationBuilder.DropTable(
                name: "Building");

            migrationBuilder.DropTable(
                name: "Postion");
        }
    }
}
