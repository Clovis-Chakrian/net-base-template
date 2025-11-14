using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChaCha.Notification.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AlterNotificationSentParamsStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "personalizations",
                schema: "notification",
                table: "notifications_sent");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "notification_template_param",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    notification_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_template_param", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_template_param_notification_templates_notifica",
                        column: x => x.notification_template_id,
                        principalSchema: "notification",
                        principalTable: "notification_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification_sent_param_value",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_sent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_template_param_id = table.Column<Guid>(type: "uuid", nullable: false),
                    value = table.Column<string>(type: "character varying(350)", maxLength: 350, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_sent_param_value", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_sent_param_value_notification_template_param_n",
                        column: x => x.notification_template_param_id,
                        principalSchema: "notification",
                        principalTable: "notification_template_param",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_sent_param_value_notifications_sent_notificati",
                        column: x => x.notification_sent_id,
                        principalSchema: "notification",
                        principalTable: "notifications_sent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(283), new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(420) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1123), new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1123) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1119), new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1120) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1125), new DateTime(2025, 11, 13, 2, 36, 29, 248, DateTimeKind.Utc).AddTicks(1125) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5179), new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5181) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5556), new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5557) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5553), new DateTime(2025, 11, 13, 2, 36, 29, 249, DateTimeKind.Utc).AddTicks(5553) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 263, DateTimeKind.Utc).AddTicks(9723), new DateTime(2025, 11, 13, 2, 36, 29, 263, DateTimeKind.Utc).AddTicks(9727) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 13, 2, 36, 29, 264, DateTimeKind.Utc).AddTicks(24), new DateTime(2025, 11, 13, 2, 36, 29, 264, DateTimeKind.Utc).AddTicks(25) });

            migrationBuilder.CreateIndex(
                name: "ix_notification_sent_param_value_notification_sent_id",
                schema: "notification",
                table: "notification_sent_param_value",
                column: "notification_sent_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_sent_param_value_notification_template_param_id",
                schema: "notification",
                table: "notification_sent_param_value",
                column: "notification_template_param_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_template_param_notification_template_id",
                schema: "notification",
                table: "notification_template_param",
                column: "notification_template_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_sent_param_value",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_template_param",
                schema: "notification");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.AddColumn<Dictionary<string, string>>(
                name: "personalizations",
                schema: "notification",
                table: "notifications_sent",
                type: "hstore",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(4524), new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(4650) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5318), new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5318) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5315), new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5315) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_channels",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5320), new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5320) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8159), new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8161) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8463), new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8463) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_external_providers",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8460), new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8461) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_types",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 897, DateTimeKind.Utc).AddTicks(9896), new DateTime(2025, 11, 11, 2, 14, 30, 897, DateTimeKind.Utc).AddTicks(9900) });

            migrationBuilder.UpdateData(
                schema: "notification",
                table: "notification_types",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 11, 11, 2, 14, 30, 898, DateTimeKind.Utc).AddTicks(195), new DateTime(2025, 11, 11, 2, 14, 30, 898, DateTimeKind.Utc).AddTicks(195) });
        }
    }
}
