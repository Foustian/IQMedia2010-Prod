﻿ALTER TABLE [dbo].[PMGSearchLog]
    ADD CONSTRAINT [PK_SearchLog] PRIMARY KEY CLUSTERED ([PMGSearchLogKey] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

