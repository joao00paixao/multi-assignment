BEGIN TRANSACTION
    CREATE SCHEMA [Marketing];

    GO

    CREATE TABLE [Marketing].[EventCampaigns] (
        [Id] INT NOT NULL PRIMARY IDENTITY (1, 0),
        [Name] NVARCHAR(500) NOT NULL,
        [StartDate] DATETIME NOT NULL,
        [EndDate] DATETIME NOT NULL,
        [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
        INDEX INDEX_EventCampaigns_Id_Name_StartDate_EndDate CLUSTERED ([Id], [Name], [StartDate], [EndDate])
    )

    GO

    INSERT INTO Events.EventCampaigns (Name, StartDate, EndDate) VALUES 
    ('Black Friday', '2023-11-15 00:00:00.000', '2023-11-25 23:59:59.999'),
    ('New Website Launch', '2023-12-28 00:00:00.000', '2024-01-02 23:59:59.999')
COMMIT TRANSACTION