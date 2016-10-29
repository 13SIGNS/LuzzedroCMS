USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_ArticleTagAssociate] Script Date: 2016-10-26 19:31:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_ArticleTagAssociate] (
    [ArticleTagAssociateID] INT IDENTITY (1, 1) NOT NULL,
    [ArticleID]             INT NOT NULL,
    [TagID]                 INT NOT NULL,
    [Status]                INT NOT NULL
);


