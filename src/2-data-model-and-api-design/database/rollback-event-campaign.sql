BEGIN TRANSACTION

DROP TABLE [Marketing].[Events]
DROP TABLE [Marketing].[EventCampaigns]

DROP SCHEMA IF EXISTS [Marketing];

COMMIT TRANSACTION