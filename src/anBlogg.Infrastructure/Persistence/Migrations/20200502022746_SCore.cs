using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace anBlogg.Infrastructure.Persistence.Migrations
{
    public partial class SCore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Comments");

            migrationBuilder.AddColumn<int>(
                name: "Score_Value",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score_Value",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score_Value",
                table: "Authors",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("1c499376-e8e3-41e9-b545-8a26d1fee602"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { "<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi,  vitae feugiat erat faucibus et.</p>", "Is opposition defensing the law?" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                column: "Contents",
                value: "<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, vitae feugiat erat faucibus et.</p>");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("809971e5-e6d1-4d4a-8bf3-959411ecd2f7"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { "<p>Vivamus auctor facilisis Praesent efficitur purus nisi vitae iaculis enim pulvinar vitae.felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", "Kebab officially became the best food in the world!" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("991861b0-e24f-40d9-88f4-8567de578668"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { "<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam. Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi, vitae feugiat erat faucibus et. Donec vitae magna eu ipsum dictum dignissim aliquet sodales arcu. Sed eu ex auctor, condimentum ex ut, aliquam lectus. Praesent sollicitudin nibh vitae erat aliquet aliquet. Duis porta pharetra augue, commodo lobortis ligula placerat ut. Vivamus auctor facilisis felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar, diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", "Is PiS destroying Poland?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score_Value",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Score_Value",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "Score_Value",
                table: "Authors");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Posts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("1c499376-e8e3-41e9-b545-8a26d1fee602"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam.
                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi,
                    vitae feugiat erat faucibus et.</p>", "Opposition is defensing the law?" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("3eabbb2c-4bbd-4863-9392-674b28aa7dcb"),
                column: "Contents",
                value: @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam.
                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi,
                    vitae feugiat erat faucibus et.</p>");

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("809971e5-e6d1-4d4a-8bf3-959411ecd2f7"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { @"<p> Vivamus auctor facilisis Praesent efficitur purus nisi vitae iaculis enim pulvinar vitae.
                    felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar,
                    diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna.
                    Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.
                    Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", "Kebab officially the best food in the world!" });

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "Id",
                keyValue: new Guid("991861b0-e24f-40d9-88f4-8567de578668"),
                columns: new[] { "Contents", "Title" },
                values: new object[] { @"<p>Donec nec nisi egestas, mollis orci quis, sodales est. Fusce sed luctus diam.
                    Sed posuere egestas arcu, vitae iaculis enim pulvinar vitae. Praesent efficitur purus nisi,
                    vitae feugiat erat faucibus et. Donec vitae magna eu ipsum dictum dignissim aliquet sodales arcu.
                    Sed eu ex auctor, condimentum ex ut, aliquam lectus. Praesent sollicitudin nibh vitae erat aliquet
                    aliquet. Duis porta pharetra augue, commodo lobortis ligula placerat ut. Vivamus auctor facilisis
                    felis vitae consequat. Vivamus dui sem, rhoncus nec condimentum id, porta ac velit. Donec pulvinar,
                    diam quis ultricies pellentesque, purus ligula lobortis ipsum, quis consectetur erat diam sodales urna.
                    Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.
                    Maecenas elit eros, pretium eget dui et, tristique ornare ex.</p>", "PiS is destroying Poland?" });
        }
    }
}