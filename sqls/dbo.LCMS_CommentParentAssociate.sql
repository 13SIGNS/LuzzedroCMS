USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_CommentParentAssociate] Script Date: 2016-10-26 19:31:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_CommentParentAssociate] (
    [CommentParentAssociateID] INT IDENTITY (1, 1) NOT NULL,
    [CommentChild_CommentID]   INT NULL,
    [CommentParent_CommentID]  INT NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_CommentChild_CommentID]
    ON [dbo].[LCMS_CommentParentAssociate]([CommentChild_CommentID] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CommentParent_CommentID]
    ON [dbo].[LCMS_CommentParentAssociate]([CommentParent_CommentID] ASC);


GO
ALTER TABLE [dbo].[LCMS_CommentParentAssociate]
    ADD CONSTRAINT [PK_dbo.LCMS_CommentParentAssociate] PRIMARY KEY CLUSTERED ([CommentParentAssociateID] ASC);


GO
ALTER TABLE [dbo].[LCMS_CommentParentAssociate]
    ADD CONSTRAINT [FK_dbo.LCMS_CommentParentAssociate_dbo.LCMS_Comment_CommentChild_CommentID] FOREIGN KEY ([CommentChild_CommentID]) REFERENCES [dbo].[LCMS_Comment] ([CommentID]);


GO
ALTER TABLE [dbo].[LCMS_CommentParentAssociate]
    ADD CONSTRAINT [FK_dbo.LCMS_CommentParentAssociate_dbo.LCMS_Comment_CommentParent_CommentID] FOREIGN KEY ([CommentParent_CommentID]) REFERENCES [dbo].[LCMS_Comment] ([CommentID]);


