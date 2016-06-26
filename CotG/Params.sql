CREATE TABLE [dbo].[Params]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	PName nvarchar(200),
	PValue nvarchar(400)
)

GO

CREATE UNIQUE INDEX [IX_Params_PName] ON [dbo].[Params] (PName)
