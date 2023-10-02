IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Accounts] (
    [AccountId] int NOT NULL,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([AccountId])
);
GO

CREATE TABLE [MeterReadings] (
    [AccountId] int NOT NULL,
    [MeterReadingDateTime] datetime2 NOT NULL,
    [MeterReadValue] nvarchar(max) NOT NULL
);
GO

CREATE UNIQUE INDEX [IX_Accounts_AccountId] ON [Accounts] ([AccountId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231001193342_ENSEKDbTables', N'7.0.11');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE OR ALTER PROCEDURE [dbo].[SP_ValidateAndInsert_MeterReading]
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
				END
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20231001225322_ENSEKDbSPs', N'7.0.11');
GO

COMMIT;
GO

