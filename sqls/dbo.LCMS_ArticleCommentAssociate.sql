USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_ArticleCommentAssociate] Script Date: 2016-10-26 19:31:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_ArticleCommentAssociate] (
    [ArticleCommentAssociateID] INT IDENTITY (1, 1) NOT NULL,
    [ArticleID]                 INT NOT NULL,
    [CommentID]                 INT NOT NULL,
    [Status]                    INT NOT NULL
);


