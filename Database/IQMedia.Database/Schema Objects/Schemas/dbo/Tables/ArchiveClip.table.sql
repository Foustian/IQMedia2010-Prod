﻿CREATE TABLE [dbo].[ArchiveClip](
	[ArchiveClipKey] [int] IDENTITY(1,1) NOT NULL,
	[ClipID] [uniqueidentifier] NULL,
	[ClipLogo] [varchar](150) NULL,
	[ClipTitle] [varchar](255) NULL,
	[ClipDate] [datetime] NULL,
	[GMTDateTime] [datetime] NULL,
	[FirstName] [varchar](150) NULL,
	[LastName] [varchar](150) NULL,
	[CustomerID] [bigint] NULL,
	[Category] [varchar](50) NULL,
	[Description] [varchar](max) NULL,
	[Keywords] [varchar](max) NULL,
	[ClosedCaption] [xml] NULL,
	[ClipCreationDate] [datetime] NULL,
	[Rating] [tinyint] NULL,
	[CreatedBy] [varchar](50) NULL,
	[ModifiedBy] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NULL,
	[CustomerGUID] [uniqueidentifier] NULL,
	[CategoryGUID] [uniqueidentifier] NULL,
	[ClientGUID] [uniqueidentifier] NULL,
	[SubCategory1GUID] [uniqueidentifier] NULL,
	[SubCategory2GUID] [uniqueidentifier] NULL,
	[SubCategory3GUID] [uniqueidentifier] NULL,
	[ThumbnailImagePath] [varchar](600) NULL,
	[IQ_CC_Key] [varchar](28) NULL,
	[StartOffset] [int] NULL,
	[Nielsen_Audience] [int] NULL,
	[IQAdShareValue] [decimal](18, 2) NULL,
	[Nielsen_Result] [varchar](1) NULL,
	[PositiveSentiment] [tinyint] NULL,
	[NegativeSentiment] [tinyint] NULL,
	[Title120] [varchar](100) NULL,
	[v5SubMediaType] [varchar](50) NULL
) ON [PRIMARY]

GO