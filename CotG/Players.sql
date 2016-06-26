CREATE TABLE [dbo].[Players]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	PlayerName nvarchar(200),
	LastLoginDT datetime NULL, 
    [HasAccess2Reports] INT NULL DEFAULT 0
)

GO

CREATE INDEX [IX_Players_PlayerName] ON [dbo].[Players] (PlayerName)
