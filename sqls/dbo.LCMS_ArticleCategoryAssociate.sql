USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_ArticleCategoryAssociate] Script Date: 2016-10-26 19:30:53 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_ArticleCategoryAssociate] (
    [ArticleCategoryAssociateID] INT IDENTITY (1, 1) NOT NULL,
    [ArticleID]                  INT NOT NULL,
    [CategoryID]                 INT NOT NULL,
    [Status]                     INT NOT NULL
);


