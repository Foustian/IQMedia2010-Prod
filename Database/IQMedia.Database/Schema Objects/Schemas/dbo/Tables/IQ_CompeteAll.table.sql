﻿CREATE TABLE [dbo].[IQ_CompeteAll](
	[ID] [bigint] NOT NULL,
	[WebSiteURL] [varchar](max) NULL,
	[CompeteURL] [varchar](100) NULL,
	[DocumentCount] [int] NULL,
	[Period] [varchar](7) NULL,
	[c_rank] [int] NULL,
	[c_uniq_visitor] [bigint] NULL,
	[c_visits] [bigint] NULL,
	[c_page_views] [bigint] NULL,
	[c_avg_stay] [float] NULL,
	[c_visit_person] [float] NULL,
	[c_page_visit] [float] NULL,
	[c_att] [float] NULL,
	[c_reach_daily] [float] NULL,
	[c_att_daily] [float] NULL,
	[c_gender_male] [float] NULL,
	[c_gender_female] [float] NULL,
	[c_age_18_24] [float] NULL,
	[c_age_25_34] [float] NULL,
	[c_age_35_44] [float] NULL,
	[c_age_45_54] [float] NULL,
	[c_age_55_64] [float] NULL,
	[c_age_65_plus] [float] NULL,
	[c_inc_30k] [float] NULL,
	[c_inc_30_60k] [float] NULL,
	[c_inc_60_100k] [float] NULL,
	[c_inc_100k_plus] [float] NULL,
	[c_top_keyword] [int] NULL,
	[results] [char](1) NULL,
	[MediaType] [char](2) NULL,
	[source_category] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]