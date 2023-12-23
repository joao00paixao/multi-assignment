BEGIN TRANSACTION
    CREATE TABLE [Marketing].[Events] (
            [Id] INT NOT NULL PRIMARY IDENTITY (1, 0),
            [EventCampaignId] INT NOT NULL FOREIGN KEY REFERENCES [Marketing].[EventCampaigns] (Id),
            [Name] NVARCHAR(500) NOT NULL,
            [StartDate] DATETIME NOT NULL,
            [EndDate] DATETIME NOT NULL,
            [CreatedOn] DATETIME NOT NULL DEFAULT GETDATE(),
            INDEX INDEX_Events_Id_EventCampaignId_Name_StartDate_EndDate CLUSTERED ([Id], [EventCampaignId], [Name], [StartDate], [EndDate])
    )

    GO

    DECLARE @BlackMarketEventCampaignId AS INT = SELECT TOP 1 Id FROM [Marketing].[Events] WHERE Name = 'Black Friday';

    INSERT INTO [Marketing].[Events] ([EventCampaignId], [Name], [StartDate], [EndDate]) VALUES
    (@BlackMarketEventCampaignId, '50% Discount Phase 1', '2023-11-15 00:00:00.000', '2023-11-18 23:59:59.999'),
    (@BlackMarketEventCampaignId, '25% Discount Phase 2', '2023-11-19 00:00:00.000', '2023-11-22 23:59:59.999'),
    (@BlackMarketEventCampaignId, '15% Discount Phase 2', '2023-11-23 00:00:00.000', '2023-11-25 23:59:59.999')

    -- Each event is sequential and represent a different discount phase.

    DECLARE @NewWebsiteLaunchEventCampaignId AS INT = SELECT TOP 1 Id FROM [Marketing].[Events] WHERE Name = 'New Website Launch';

    INSERT INTO [Marketing].[Events] ([EventCampaignId], [Name], [StartDate], [EndDate]) VALUES
    (@NewWebsiteLaunchEventCampaignId, 'New Website Header Showcase', '2023-12-28 00:00:00.000', '2024-01-02 23:59:59.999'),
    (@NewWebsiteLaunchEventCampaignId, 'New Website Landing Page', '2024-01-01 00:00:00.000', '2024-01-02 23:59:59.999'),
    (@NewWebsiteLaunchEventCampaignId, 'New Website Redirection', '2024-01-02 00:00:00.000', '2024-01-02 23:59:59.999')

    -- New Header should show during the entire event campaign.
    -- New Landing Page should show after the new year until the end of the campaign.
    -- New Website Redirection should start on the last 24 hours of the event.


COMMIT TRANSACTION