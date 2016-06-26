CREATE TABLE [dbo].[PlunderedHistory]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [PlayerName] NVARCHAR(200) NOT NULL, 
    [Score] INT NOT NULL, 
    [Rank] INT NOT NULL, 
    [CreateDT] DATETIME NOT NULL 
)
