CREATE TABLE [dbo].[DefReputationHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlayerName] NVARCHAR(200) NOT NULL, 
    [Score] BIGINT NOT NULL, 
    [Rank] INT NOT NULL, 
    [CreateDT] DATETIME NOT NULL DEFAULT GetDate() 
)

GO

CREATE INDEX [IX_DefReputationHistory] ON [dbo].[DefReputationHistory] ([PlayerName])
