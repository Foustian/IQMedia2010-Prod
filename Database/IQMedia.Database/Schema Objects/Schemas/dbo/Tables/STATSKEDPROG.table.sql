﻿CREATE TABLE [dbo].[STATSKEDPROG] (
    [StatSkedProgKey]   BIGINT         IDENTITY (1, 1) NOT NULL,
    [SSP_Index_Key]     INT            NULL,
    [Station_Id]        VARCHAR (14)   NULL,
    [rl_format]         VARCHAR (5)    NULL,
    [station_call_sign] VARCHAR (10)   NULL,
    [rl_station_active] FLOAT          NULL,
    [IQ_Time_Zone]      VARCHAR (3)    NULL,
    [IQ_Dma_Name]       VARCHAR (47)   NULL,
    [IQ_Dma_Num]        VARCHAR (3)    NULL,
    [gmt_adj]           FLOAT          NULL,
    [dst_adj]           FLOAT          NULL,
    [iq_cluster]        FLOAT          NULL,
    [Station_Num]       VARCHAR (10)   NULL,
    [Station_Affil]     VARCHAR (40)   NULL,
    [Station_Affil_Num] VARCHAR (3)    NULL,
    [Station_Name]      VARCHAR (30)   NULL,
    [IQ_Time_Zone_Num]  INT            NULL,
    [Database_Key]      VARCHAR (10)   NULL,
    [tf_duration]       VARCHAR (4)    NULL,
    [Title120]          VARCHAR (128)  NULL,
    [tf_category]       VARCHAR (18)   NULL,
    [Desc100]           VARCHAR (440)  NULL,
    [iq_cat]            VARCHAR (8)    NULL,
    [iq_cat_num]        VARCHAR (3)    NULL,
    [iq_duration]       NUMERIC (7, 2) NULL,
    [iq_segs]           INT            NULL,
    [gmt]               DATETIME       NULL,
    [iq_local_air_date] DATETIME       NULL,
    [iq_local_air_time] FLOAT          NULL,
    [iq_gmt_adj]        FLOAT          NULL,
    [gmt_air_time]      FLOAT          NULL,
    [iq_class]          VARCHAR (13)   NULL,
    [iq_class_num]      VARCHAR (3)    NULL,
    [iq_start_point]    FLOAT          NULL,
    [iq_rec_type]       VARCHAR (1)    NULL,
    [IQ_CC_Key]         VARCHAR (28)   NULL,
    [iq_master_key]     VARCHAR (36)   NULL,
    [station_market]    VARCHAR (50)   NULL,
    [createddate]       DATETIME       NULL,
    [modifieddate]      DATETIME       NULL,
    [CreatedBy]         VARCHAR (15)   NULL,
    [ModifiedBy]        VARCHAR (15)   NULL,
    [IsActive]          BIT            NULL
);

