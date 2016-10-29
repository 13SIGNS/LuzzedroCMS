USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Tag] Script Date: 2016-10-26 19:31:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Tag] (
    [TagID]  INT           IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (50) NOT NULL,
    [Status] INT           NOT NULL
);


