USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Comment] Script Date: 2016-10-26 19:31:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Comment] (
    [CommentID]       INT             IDENTITY (1, 1) NOT NULL,
    [ParentCommentID] INT             NOT NULL,
    [ArticleID]       INT             NOT NULL,
    [UserID]          INT             NOT NULL,
    [Date]            DATETIME        NOT NULL,
    [Content]         NVARCHAR (2048) NOT NULL,
    [Status]          INT             NOT NULL
);


