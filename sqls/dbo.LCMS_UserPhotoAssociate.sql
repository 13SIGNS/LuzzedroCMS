USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_UserPhotoAssociate] Script Date: 2016-10-26 19:32:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_UserPhotoAssociate] (
    [UserPhotoAssociateID] INT      IDENTITY (1, 1) NOT NULL,
    [UserID]               INT      NOT NULL,
    [PhotoID]              INT      NOT NULL,
    [Date]                 DATETIME NOT NULL,
    [Status]               INT      NOT NULL
);


