﻿ALTER TABLE [dbo].[IQCore_ClipInfo]
    ADD CONSTRAINT [PK_IQCore_ClipInfo] PRIMARY KEY CLUSTERED ([_ClipGuid] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
