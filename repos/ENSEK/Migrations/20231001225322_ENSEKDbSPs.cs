using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ENSEK.Migrations
{
    /// <inheritdoc />
    public partial class ENSEKDbSPs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var validateAndInsertMeterReadSP = @"CREATE OR ALTER PROCEDURE [dbo].[SP_ValidateAndInsert_MeterReading]
				@AccountId int,
				@MeterReadingDateTime datetime2,
				@MeterReadingValue nvarchar(max)
				AS
				BEGIN
					SET NOCOUNT ON;
					DECLARE @Result bit =1, @Message nvarchar(300)='Success.'

					IF EXISTS
					(
						SELECT * FROM Accounts WHERE AccountId = @AccountId
					)
					BEGIN
						IF EXISTS
						(
							SELECT * FROM MeterReadings WHERE AccountId = @AccountId AND MeterReadingDateTime = @MeterReadingDateTime AND MeterReadValue = @MeterReadingValue
						)
						BEGIN
							SET @Result = 0
							SET @Message = 'Duplicate Entry.'
						END
						ELSE
						BEGIN
							IF @MeterReadingValue NOT LIKE '%[^0-9]%' AND LEN(@MeterReadingValue) = 5 
							BEGIN
								IF EXISTS
								(
									SELECT * FROM MeterReadings WHERE AccountId = @AccountId AND MeterReadingDateTime > @MeterReadingDateTime
								)
								BEGIN
									SET @Result = 0
									SET @Message = 'New read cannot be older than an exising read.'
								END
								ELSE
								BEGIN
									INSERT INTO MeterReadings(AccountId,MeterReadingDateTime,MeterReadValue) VALUES(@AccountId,@MeterReadingDateTime,@MeterReadingValue)
								END
							END
							ELSE
							BEGIN
								SET @Result = 0
								SET @Message = 'Invalid Meter Read Value format.'
							END
						END
					END
					ELSE
					BEGIN
						SET @Result = 0
						SET @Message = 'Invalid Account Id.'
					END

					SELECT @Result as Result, @Message as [Message]
				END";
            migrationBuilder.Sql(validateAndInsertMeterReadSP);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessResults");
        }
    }
}
