﻿ALTER TABLE [dbo].[IQDiscovery_SavedSearch] 
	ADD  CONSTRAINT [DF_IQDiscovery_SavedSearch_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]