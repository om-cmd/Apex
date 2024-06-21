using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DomainLayer.Migrations
{
    /// <inheritdoc />
    public partial class INIT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DTbl_Branches",
                columns: table => new
                {
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Branches", x => x.TenantId);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Roles = table.Column<int>(type: "int", nullable: false),
                    TenantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_User_DTbl_Branches_TenantId",
                        column: x => x.TenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_Messages_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_DTbl_Messages_DTbl_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DTbl_Messages_DTbl_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_Notifications_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_DTbl_Notifications_DTbl_User_UserId",
                        column: x => x.UserId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TextContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VideoPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateArchived = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsArchived = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_Posts_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_DTbl_Posts_DTbl_User_UserId",
                        column: x => x.UserId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_FriendRequests_DTbl_User_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FriendRequests_DTbl_User_SenderId",
                        column: x => x.SenderId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_Comments_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_DTbl_Comments_DTbl_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "DTbl_Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DTbl_Comments_DTbl_User_UserId",
                        column: x => x.UserId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DTbl_Likes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BranchTenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DTbl_Likes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DTbl_Likes_DTbl_Branches_BranchTenantId",
                        column: x => x.BranchTenantId,
                        principalTable: "DTbl_Branches",
                        principalColumn: "TenantId");
                    table.ForeignKey(
                        name: "FK_DTbl_Likes_DTbl_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "DTbl_Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DTbl_Likes_DTbl_User_UserId",
                        column: x => x.UserId,
                        principalTable: "DTbl_User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Comments_BranchTenantId",
                table: "DTbl_Comments",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Comments_PostId",
                table: "DTbl_Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Comments_UserId",
                table: "DTbl_Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Likes_BranchTenantId",
                table: "DTbl_Likes",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Likes_PostId",
                table: "DTbl_Likes",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Likes_UserId",
                table: "DTbl_Likes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Messages_BranchTenantId",
                table: "DTbl_Messages",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Messages_ReceiverId",
                table: "DTbl_Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Messages_SenderId",
                table: "DTbl_Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Notifications_BranchTenantId",
                table: "DTbl_Notifications",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Notifications_UserId",
                table: "DTbl_Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Posts_BranchTenantId",
                table: "DTbl_Posts",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_Posts_UserId",
                table: "DTbl_Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DTbl_User_TenantId",
                table: "DTbl_User",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_BranchTenantId",
                table: "FriendRequests",
                column: "BranchTenantId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_SenderId",
                table: "FriendRequests",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DTbl_Comments");

            migrationBuilder.DropTable(
                name: "DTbl_Likes");

            migrationBuilder.DropTable(
                name: "DTbl_Messages");

            migrationBuilder.DropTable(
                name: "DTbl_Notifications");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "DTbl_Posts");

            migrationBuilder.DropTable(
                name: "DTbl_User");

            migrationBuilder.DropTable(
                name: "DTbl_Branches");
        }
    }
}
