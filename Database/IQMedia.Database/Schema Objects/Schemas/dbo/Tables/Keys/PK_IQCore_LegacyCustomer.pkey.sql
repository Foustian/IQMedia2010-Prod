﻿ALTER TABLE [dbo].[IQCore_LegacyCustomer]
    ADD CONSTRAINT [PK_IQCore_LegacyCustomer] PRIMARY KEY CLUSTERED 
(
	[Guid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]

