CREATE TABLE [dbo].[CurrentAlianceRanking]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [AlianceName] NVARCHAR(200) NULL, 
    [Score] INT NOT NULL, 
    [CitiesNo] INT NOT NULL, 
    [UpdateDT] DATETIME NOT NULL DEFAULT GetDate(), 
    [Continent] INT NOT NULL, 
    [Players] INT NOT NULL, 
    [Rank] INT NOT NULL
)

GO

CREATE INDEX [IX_CurrentAlianceRanking_AlianceName] ON [dbo].[CurrentAlianceRanking] ([AlianceName])

GO

CREATE INDEX [IX_CurrentAlianceRanking_Continent] ON [dbo].[CurrentAlianceRanking] ([Continent])
