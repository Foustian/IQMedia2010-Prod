﻿CREATE NONCLUSTERED INDEX [idx_IQ_Five_Min_Staging_IQ_CC_Key] ON [dbo].[IQ_Five_Min_Staging] 
(
	[IQ_CC_Key] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


