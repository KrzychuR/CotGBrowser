CREATE TABLE [dbo].[AlianceScoreHistory]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AlianceName] NVARCHAR(200) NULL, 
    [Score] INT NOT NULL, 
    [CitiesNo] INT NOT NULL, 
    [CreateDT] DATETIME NOT NULL DEFAULT GetDate(), 
    [Continent] INT NOT NULL, 
    [Players] INT NOT NULL, 
    [Rank] INT NOT NULL
)

GO

CREATE INDEX [IX_AlianceScoreHistory] ON [dbo].[AlianceScoreHistory] ([Continent], [AlianceName])
