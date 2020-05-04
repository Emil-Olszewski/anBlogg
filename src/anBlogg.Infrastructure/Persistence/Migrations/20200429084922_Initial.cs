using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace anBlogg.Infrastructure.Persistence.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Contents = table.Column<string>(nullable: true),
                    Tags_Raw = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    AuthorId = table.Column<Guid>(nullable: false),
                    PostId = table.Column<Guid>(nullable: false),
                    Contents = table.Column<string>(nullable: true),
                    Score = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AccountId", "Created", "DisplayName", "Modified" },
                values: new object[] { new Guid("591d79d0-7742-4f9d-b285-c30319036ec0"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juri", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AccountId", "Created", "DisplayName", "Modified" },
                values: new object[] { new Guid("0a6c5317-a564-4e48-8e79-df271e9e72d3"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Vanessa", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "AccountId", "Created", "DisplayName", "Modified" },
                values: new object[] { new Guid("184edd78-aeab-4c20-becc-6d3dc9f1b841"), new Guid("00000000-0000-0000-0000-000000000000"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Matthew", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "AuthorId", "Contents", "Created", "Modified", "Score", "Title", "Tags_Raw" },
                values: new object[,]
                {
                    { new Guid("991861b0-e24f-40d9-88f4-8567de578668"), new Guid("591d79d0-7742-4f9d-b285-c30319036ec0"), @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. 
                                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, 
                                    vitae feugiat erat faucibus et. Donec vitae magna eu ipsum dictum dignissim aliquet sodales arcu. 
                                    Sed eu ex auctor, condimentum ex ut, aliquam lectus. Praesent sollicitudin nibh vitae erat aliquet 
                                    aliquet. Duis porta pharetra augue, commodo lobortis ligula placerat ut. Vivamus auctor facilisis 
                                    felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, 
                                    diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. 
                                    Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. 
                                    Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "PiS is destroying Poland?", "<tag><other-tag>" },
                    { new Guid("1c499376-e8e3-41e9-b545-8a26d1fee602"), new Guid("591d79d0-7742-4f9d-b285-c30319036ec0"), @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. 
                                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, 
                                    vitae feugiat erat faucibus et.</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Opposition is defensing the law?", "<tag><some-other-tag>" },
                    { new Guid("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"), new Guid("0a6c5317-a564-4e48-8e79-df271e9e72d3"), @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. 
                                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, 
                                    vitae feugiat erat faucibus et.</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "What after Corona?", "<tag><other-tag>" },
                    { new Guid("809971e5-e6d1-4d4a-8bf3-959411ecd2f7"), new Guid("184edd78-aeab-4c20-becc-6d3dc9f1b841"), @"<p> Vivamus auctor facilisis Praesent efficitur purus nisi vitae iaculis enim pulvinar vitae.
                                    felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, 
                                    diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. 
                                    Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. 
                                    Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Kebab officially the best food in the world!", "<tag><completely-other-tag>" }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Contents", "Created", "Modified", "PostId", "Score" },
                values: new object[] { new Guid("b56a3616-6b3f-4e78-aae1-04ca960c1a54"), new Guid("184edd78-aeab-4c20-becc-6d3dc9f1b841"), "<p>My first comment! :D</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"), 0 });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "AuthorId", "Contents", "Created", "Modified", "PostId", "Score" },
                values: new object[] { new Guid("0fac4488-8943-11ea-bc55-0242ac130003"), new Guid("591d79d0-7742-4f9d-b285-c30319036ec0"), "<p>Nice work Vanessa !!!</p>", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"), 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_AuthorId",
                table: "Comments",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_AuthorId",
                table: "Posts",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Authors");
        }
    }
}
