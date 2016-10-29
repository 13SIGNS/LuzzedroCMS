USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_BookmarkUserArticleAssociate] Script Date: 2016-10-26 19:31:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_BookmarkUserArticleAssociate] (
    [BookmarkUserArticleAssociateID] INT      IDENTITY (1, 1) NOT NULL,
    [UserID]                         INT      NOT NULL,
    [ArticleID]                      INT      NOT NULL,
    [Date]                           DATETIME NOT NULL,
    [Status]                         INT      NOT NULL
);


