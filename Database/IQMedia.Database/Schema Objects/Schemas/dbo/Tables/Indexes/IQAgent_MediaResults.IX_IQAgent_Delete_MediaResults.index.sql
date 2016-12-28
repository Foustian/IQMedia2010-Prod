﻿CREATE NONCLUSTERED INDEX [IX_IQAgent_Delete_MediaResults] ON [dbo].[IQAgent_MediaResults] 
(
	[_MediaID] ASC,
	[MediaType] ASC,
	[IsActive] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


