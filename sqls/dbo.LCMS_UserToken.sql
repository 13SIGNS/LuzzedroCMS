USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_UserToken] Script Date: 2016-10-26 19:32:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_UserToken] (
    [UserTokenID] INT            IDENTITY (1, 1) NOT NULL,
    [UserID]      INT            NOT NULL,
    [Token]       NVARCHAR (MAX) NULL,
    [ExpiryDate]  DATETIME       NOT NULL
);


