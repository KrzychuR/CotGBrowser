CREATE TABLE [dbo].[RankingsEmpireScore]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlayerName] NVARCHAR(200) NOT NULL, 
    [Rank] INT NOT NULL, 
    [Score] INT NOT NULL, 
    [AlianceName] NVARCHAR(200) NULL, 
    [CitiesNo] INT NOT NULL, 
    [CreateDT] DATETIME NOT NULL DEFAULT GetDate(), 
    [Continent] INT NOT NULL
)

GO

CREATE INDEX [IX_RankingsEmpireScore] ON [dbo].[RankingsEmpireScore] ([Continent], [PlayerName])
