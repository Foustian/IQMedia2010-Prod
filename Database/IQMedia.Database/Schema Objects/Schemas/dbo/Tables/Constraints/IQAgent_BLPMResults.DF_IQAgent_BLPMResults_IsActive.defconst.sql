﻿ALTER TABLE [dbo].[IQAgent_BLPMResults] ADD  CONSTRAINT [DF_IQAgent_BLPMResults_IsActive]  DEFAULT ((1)) FOR [IsActive]