USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_UserRoleAssociate] Script Date: 2016-10-26 19:32:08 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_UserRoleAssociate] (
    [UserRoleAssociateID] INT IDENTITY (1, 1) NOT NULL,
    [UserID]              INT NOT NULL,
    [RoleID]              INT NOT NULL
);


