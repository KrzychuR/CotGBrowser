CREATE TABLE [dbo].[CurrentEmpireRanking]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PlayerName] NVARCHAR(200) NOT NULL, 
    [Rank] INT NULL, 
    [Score] INT NULL, 
    [AlianceName] NVARCHAR(200) NULL, 
    [CitiesNo] INT NULL, 
    [UpdateDT] DATETIME NOT NULL DEFAULT GetDate(), 
    [Continent] INT NOT NULL, 
    [UnitsKills] BIGINT NULL, 
    [UnitsKillsRank] INT NULL, 
    [Caverns] BIGINT NULL, 
    [CavernsRank] INT NULL, 
    [UnitsKillsDiffAvg] FLOAT NULL, 
    [ScoreDiffAvg] FLOAT NULL, 
    [DefReputation] BIGINT NULL, 
    [DefReputationRank] INT NULL, 
    [OffReputation] BIGINT NULL, 
    [OffReputationRank] INT NULL, 
    [DefReputationDiffAvg] FLOAT NULL, 
    [OffReputationDiffAvg] FLOAT NULL, 
    [RankLastChange] INT NULL, 
    [ScoreLastChange] INT NULL, 
    [CitiesNoLastChange] INT NULL, 
    [DefReputationRankLastChange] INT NULL 
)

GO

CREATE INDEX [IX_CurrentEmpireRanking_PlayerName] ON [dbo].[CurrentEmpireRanking] ([PlayerName])
