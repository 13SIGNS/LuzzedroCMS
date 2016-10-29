USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_UserTemp] Script Date: 2016-10-26 19:32:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_UserTemp] (
    [UserTempID] INT            IDENTITY (1, 1) NOT NULL,
    [Date]       DATETIME       NOT NULL,
    [Email]      NVARCHAR (MAX) NULL,
    [Nick]       NVARCHAR (MAX) NULL,
    [Password]   NVARCHAR (MAX) NULL,
    [Token]      NVARCHAR (MAX) NULL
);


