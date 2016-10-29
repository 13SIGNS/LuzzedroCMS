USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Article] Script Date: 2016-10-26 19:30:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Article] (
    [ArticleID]  INT             IDENTITY (1, 1) NOT NULL,
    [UserID]     INT             NOT NULL,
    [CategoryID] INT             NOT NULL,
    [ImageName]  NVARCHAR (300)  NULL,
    [ImageDesc]  NVARCHAR (300)  NOT NULL,
    [DateAdd]    DATETIME        NOT NULL,
    [DatePub]    DATETIME        NOT NULL,
    [DateExp]    DATETIME        NOT NULL,
    [Title]      NVARCHAR (300)  NOT NULL,
    [Url]        NVARCHAR (MAX)  NULL,
    [Lead]       NVARCHAR (1000) NOT NULL,
    [Content]    NVARCHAR (MAX)  NOT NULL,
    [Source]     NVARCHAR (2000) NOT NULL,
    [Status]     INT             NOT NULL
);


