using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

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

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:hstore", ",,");

            migrationBuilder.CreateTable(
                name: "notification_channels",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_channels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_external_providers",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_external_providers", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_types",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "notification_templates",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    external_template_id = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: true),
                    subject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    body = table.Column<string>(type: "character varying(3000)", maxLength: 3000, nullable: true),
                    notification_external_provider_id = table.Column<int>(type: "integer", nullable: false),
                    notification_type_id = table.Column<int>(type: "integer", nullable: false),
                    notification_channel_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_templates", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_templates_notification_channels_notification_c",
                        column: x => x.notification_channel_id,
                        principalSchema: "notification",
                        principalTable: "notification_channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_templates_notification_external_providers_noti",
                        column: x => x.notification_external_provider_id,
                        principalSchema: "notification",
                        principalTable: "notification_external_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notification_templates_notification_types_notification_type",
                        column: x => x.notification_type_id,
                        principalSchema: "notification",
                        principalTable: "notification_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notifications_sent",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_template_id = table.Column<Guid>(type: "uuid", nullable: false),
                    personalizations = table.Column<Dictionary<string, string>>(type: "hstore", nullable: true),
                    notification_channel_id = table.Column<int>(type: "integer", nullable: false),
                    notification_external_provider_id = table.Column<int>(type: "integer", nullable: false),
                    notification_type_id = table.Column<int>(type: "integer", nullable: false),
                    notification_external_provider_request_id = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    sent_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    attempts = table.Column<int>(type: "integer", nullable: false),
                    should_retry = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications_sent", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_sent_notification_channels_notification_chann",
                        column: x => x.notification_channel_id,
                        principalSchema: "notification",
                        principalTable: "notification_channels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notifications_sent_notification_external_providers_notifica",
                        column: x => x.notification_external_provider_id,
                        principalSchema: "notification",
                        principalTable: "notification_external_providers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notifications_sent_notification_templates_notification_temp",
                        column: x => x.notification_template_id,
                        principalSchema: "notification",
                        principalTable: "notification_templates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_notifications_sent_notification_types_notification_type_id",
                        column: x => x.notification_type_id,
                        principalSchema: "notification",
                        principalTable: "notification_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification_reasons_not_sent",
                schema: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    notification_sent_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification_reasons_not_sent", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_reasons_not_sent_notifications_sent_notificati",
                        column: x => x.notification_sent_id,
                        principalSchema: "notification",
                        principalTable: "notifications_sent",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "notification",
                table: "notification_channels",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(4524), "Representa o canal de envio de emails.", "Email", new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(4650) },
                    { 2, new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5318), "Representa o canal de envio de SMS's.", "SMS", new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5318) },
                    { 3, new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5315), "Representa o canal de envio de whatsapps.", "WhatsApp", new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5315) },
                    { 4, new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5320), "Representa o canal de envio de notificações push.", "Push", new DateTime(2025, 11, 11, 2, 14, 30, 882, DateTimeKind.Utc).AddTicks(5320) }
                });

            migrationBuilder.InsertData(
                schema: "notification",
                table: "notification_external_providers",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8159), "Provider utilizado para envio de Email's.", "Mailgun", new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8161) },
                    { 2, new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8463), "Provider utilizado para envio de WhatsApp ou SMS.", "Twillio", new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8463) },
                    { 3, new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8460), "Provider utilizado para envio de push notifications.", "Firebase", new DateTime(2025, 11, 11, 2, 14, 30, 883, DateTimeKind.Utc).AddTicks(8461) }
                });

            migrationBuilder.InsertData(
                schema: "notification",
                table: "notification_types",
                columns: new[] { "id", "created_at", "description", "name", "updated_at" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 11, 11, 2, 14, 30, 897, DateTimeKind.Utc).AddTicks(9896), "Notificações relacionadas ao sistema e possíveis falhas que ocorram.", "System", new DateTime(2025, 11, 11, 2, 14, 30, 897, DateTimeKind.Utc).AddTicks(9900) },
                    { 2, new DateTime(2025, 11, 11, 2, 14, 30, 898, DateTimeKind.Utc).AddTicks(195), "Notificações sobre ocorrências do sistema para os usuários.", "Transactional", new DateTime(2025, 11, 11, 2, 14, 30, 898, DateTimeKind.Utc).AddTicks(195) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_notification_reasons_not_sent_notification_sent_id",
                schema: "notification",
                table: "notification_reasons_not_sent",
                column: "notification_sent_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_templates_notification_channel_id",
                schema: "notification",
                table: "notification_templates",
                column: "notification_channel_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_templates_notification_external_provider_id",
                schema: "notification",
                table: "notification_templates",
                column: "notification_external_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_templates_notification_type_id",
                schema: "notification",
                table: "notification_templates",
                column: "notification_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sent_notification_channel_id",
                schema: "notification",
                table: "notifications_sent",
                column: "notification_channel_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sent_notification_external_provider_id",
                schema: "notification",
                table: "notifications_sent",
                column: "notification_external_provider_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sent_notification_template_id",
                schema: "notification",
                table: "notifications_sent",
                column: "notification_template_id");

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sent_notification_type_id",
                schema: "notification",
                table: "notifications_sent",
                column: "notification_type_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notification_reasons_not_sent",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notifications_sent",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_templates",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_channels",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_external_providers",
                schema: "notification");

            migrationBuilder.DropTable(
                name: "notification_types",
                schema: "notification");
        }
    }
}
