USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Log] Script Date: 2016-10-26 19:31:36 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Log] (
    [LogID]  INT      IDENTITY (1, 1) NOT NULL,
    [UserID] INT      NOT NULL,
    [Date]   DATETIME NOT NULL
);


