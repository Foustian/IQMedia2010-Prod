﻿ALTER TABLE [dbo].[IQTrack_PlaySummary]
    ADD CONSTRAINT [PK_IQTrack_PlaySummary] PRIMARY KEY CLUSTERED ([_AssetGuid] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
