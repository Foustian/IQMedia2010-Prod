﻿ALTER TABLE [dbo].[NM_PublicationCategory] ADD  CONSTRAINT [DF_NB_PublicationCategory_IsActive]  DEFAULT ((1)) FOR [IsActive]