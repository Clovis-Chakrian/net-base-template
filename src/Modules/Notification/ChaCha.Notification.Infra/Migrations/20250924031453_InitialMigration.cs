using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChaCha.Notification.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "notification");

            migrationBuilder.CreateTable(
                name: "notifications_sent",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    message_template_id = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    sent_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    personalizations = table.Column<string>(type: "jsonb", nullable: false),
                    notification_provider = table.Column<string>(type: "text", nullable: false),
                    notification_method = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications_sent", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "token_types",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    expiration_time = table.Column<int>(type: "integer", nullable: false),
                    max_attempts = table.Column<int>(type: "integer", nullable: false),
                    resent_limit = table.Column<int>(type: "integer", nullable: false),
                    token_length = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_token_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tokens_sent",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    token_type_id = table.Column<int>(type: "integer", nullable: false),
                    notification_sent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    used = table.Column<bool>(type: "boolean", nullable: false),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    salt = table.Column<string>(type: "text", nullable: false),
                    data_check = table.Column<string>(type: "text", nullable: false),
                    used_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tokens_sent", x => x.id);
                    table.ForeignKey(
                        name: "fk_tokens_sent_notifications_sent_notification_sent_id",
                        column: x => x.notification_sent_id,
                        principalSchema: "notification",
                        principalTable: "notifications_sent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_tokens_sent_token_types_token_type_id",
                        column: x => x.token_type_id,
                        principalSchema: "notification",
                        principalTable: "token_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "notification",
                table: "token_types",
                columns: new[] { "id", "created_at", "description", "expiration_time", "max_attempts", "name", "resent_limit", "token_length", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(3101), "Token used to multi factor authentication", 5, 5, "MFA", 5, 6, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(4079) },
                    { 2, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(5204), "One Time Password token", 5, 5, "OTP", 5, 9, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(5205) },
                    { 3, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(5207), "Recover account token", 5, 5, "RecoverAccount", 5, 6, new DateTime(2025, 9, 24, 3, 14, 52, 982, DateTimeKind.Utc).AddTicks(5207) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sent_message_id",
                schema: "notification",
                table: "notifications_sent",
                column: "message_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_token_types_name",
                schema: "notification",
                table: "token_types",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tokens_sent_notification_sent_id",
                schema: "notification",
                table: "tokens_sent",
                column: "notification_sent_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tokens_sent_token",
                schema: "notification",
                table: "tokens_sent",
                column: "token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tokens_sent_token_type_id",
                schema: "notification",
                table: "tokens_sent",
                column: "token_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tokens_sent",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notifications_sent",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "token_types",
                schema: "notification");
        }
    }
}
