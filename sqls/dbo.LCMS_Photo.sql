USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Photo] Script Date: 2016-10-26 19:31:40 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Photo] (
    [PhotoID] INT             IDENTITY (1, 1) NOT NULL,
    [Date]    DATETIME        NOT NULL,
    [Name]    NVARCHAR (MAX)  NULL,
    [Desc]    NVARCHAR (2048) NOT NULL,
    [Status]  INT             NOT NULL
);


