USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_User] Script Date: 2016-10-26 19:31:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_User] (
    [UserID]   INT            IDENTITY (1, 1) NOT NULL,
    [Date]     DATETIME       NOT NULL,
    [Email]    NVARCHAR (300) NOT NULL,
    [Nick]     NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (300) NOT NULL,
    [PhotoUrl] NVARCHAR (MAX) NULL,
    [Status]   INT            NOT NULL
);


