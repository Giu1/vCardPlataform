CREATE TABLE [dbo].[Earning]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [EarningPercentege] INT NOT NULL CHECK ([EarningPercentege]>=0 AND [EarningPercentege]<=100), 
    CONSTRAINT [FK_Eearning_ToTable] FOREIGN KEY ([Id]) REFERENCES [Contas]([PhoneNumber])

)
