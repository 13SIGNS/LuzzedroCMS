USE [LuzzedroCMS]
GO

/****** Object: Table [dbo].[LCMS_Category] Script Date: 2016-10-26 19:31:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LCMS_Category] (
    [CategoryID] INT           IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [Order]      INT           NOT NULL,
    [Status]     INT           NOT NULL
);


