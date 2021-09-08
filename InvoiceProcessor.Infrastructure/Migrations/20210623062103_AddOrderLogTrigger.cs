using Microsoft.EntityFrameworkCore.Migrations;

namespace InvoiceProcessor.Infrastructure.Migrations
{
    public partial class AddOrderLogTrigger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                        CREATE TRIGGER TR_OutboxLog
                           ON Outbox
                           AFTER UPDATE,INSERT
                        AS 
                        BEGIN
	                        SET NOCOUNT ON;
	                        
	                        INSERT INTO OutBoxLog (OutBoxId, Status, CreatedOn)
	                        SELECT d.Guid, d.Status, d.ModifiedOn FROM inserted d

                        END
                        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER TR_OutboxLog");
        }
    }
}