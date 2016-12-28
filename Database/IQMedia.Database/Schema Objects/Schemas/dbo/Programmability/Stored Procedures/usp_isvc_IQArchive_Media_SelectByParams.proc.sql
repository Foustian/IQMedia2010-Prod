﻿CREATE PROCEDURE [dbo].[usp_isvc_IQArchive_Media_SelectByParams]
(
		@PageSize INT,
		@FromDate DATETIME,
		@ToDate DATETIME,
		@CategoryGUID	UNIQUEIDENTIFIER,
		@ClientGUID UNIQUEIDENTIFIER,
		@CustomerGUID UNIQUEIDENTIFIER,
		@SeqID BIGINT,
		@SubMediaType VARCHAR(20)
)
AS
BEGIN
	
	SET NOCOUNT ON;  
  
	 DECLARE	@TotalResults BIGINT,  
				@MaxSinceID BIGINT,				
				@IsRadioAccess	BIT
	   
	SELECT 
			@IsRadioAccess = CASE WHEN  ClientRole.IsAccess = 1 AND CustomerRole.IsAccess = 1 THEN 1 ELSE 0 END
	FROM 
			[ROLE] 
				INNER JOIN CustomerRole 
					ON CustomerRole.RoleID = [ROLE].RoleKey
				INNER JOIN ClientRole
					ON	ClientRole.RoleID = [ROLE].RoleKey
					AND CustomerRole.RoleID =ClientRole.RoleID 
				INNER JOIN Customer ON 
					Customer.CustomerKey = CustomerRole.CustomerID 
				INNER JOIN Client ON
					ClientRole.ClientID=Client.ClientKey
					AND Customer.ClientID=Client.ClientKey
	WHERE
			Customer.CustomerGUID   = @CustomerGUID   
		AND Role.IsActive = 1 AND ClientRole.IsActive = 1 AND CustomerRole.IsActive = 1		
		AND RoleName ='v4TM'
	   
	 CREATE TABLE #ArchiveMedia (ID BIGINT, _ArchiveMediaID BIGINT, CategoryName VARCHAR(150), _ParentID BIGINT, MediaType VARCHAR(2), SubMediaType VARCHAR(50))
	   
	 SELECT 
			@MaxSinceID=ISNULL(MAX(ID),0),  
			@TotalResults = COUNT(ID)  
	 FROM 
			IQArchive_Media WITH(NOLOCK) 
				INNER JOIN CustomCategory  
					ON IQArchive_Media.CategoryGUID = CustomCategory.CategoryGUID  
					AND	IQArchive_Media.ClientGUID = @ClientGUID  
					AND IQArchive_Media.IsActive = 1  
					AND CustomCategory.ClientGUID = @ClientGUID  
					AND CustomCategory.IsActive = 1  
	 WHERE 
			(@IsRadioAccess = 1 OR MediaType != 'TM')		
			AND	(@CategoryGUID IS NULL OR IQArchive_Media.CategoryGUID = @CategoryGUID)  
			AND	((@FromDate IS NULL OR @ToDate IS NULL) OR (IQArchive_Media.MediaDate BETWEEN @FromDate AND @ToDate))   
			AND (@SubMediaType IS NULL OR SubMediaType=@SubMediaType)
	 --AND  MediaType='TV'  
	 
	 INSERT INTO #ArchiveMedia
	 (
		ID,
		_ArchiveMediaID,
		CategoryName,
		_ParentID,
		MediaType,
		SubMediaType
	 )
	 SELECT
			TOP(@PageSize)
			ID,
			_ArchiveMediaID,
			CategoryName,
			_ParentID,
			MediaType,
			SubMediaType
	FROM 
			IQArchive_Media WITH(NOLOCK) 
				INNER JOIN CustomCategory  
				ON IQArchive_Media.CategoryGUID = CustomCategory.CategoryGUID  
				AND IQArchive_Media.ClientGUID=@ClientGUID
				AND	IQArchive_Media.IsActive = 1  
				AND CustomCategory.ClientGUID = @ClientGUID  
				AND CustomCategory.IsActive = 1  
	 WHERE 
			ID>@SeqID
		AND (@IsRadioAccess = 1 OR MediaType != 'TM')		
		AND	(@CategoryGUID IS NULL OR IQArchive_Media.CategoryGUID = @CategoryGUID)  
		AND	((@FromDate IS NULL OR @ToDate IS NULL) OR (IQArchive_Media.MediaDate BETWEEN @FromDate AND @ToDate))
		AND (@SubMediaType IS NULL OR SubMediaType=@SubMediaType)   
				
	 ORDER BY ID ASC  
	             
		-- Fill ArchiveClip table  
	      
		SELECT   
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				ClipTitle AS Title,  
				CategoryGUID,
				Keywords,
				[Description],
				PositiveSentiment,
				NegativeSentiment,
				CONVERT(NVARCHAR(MAX),ClosedCaption) AS CONTENT,  
				GMTDateTime AS MediaDate,  				
				ClipCreationDate AS CreatedDate,  
				ClipID,  
				CASE WHEN Nielsen_Audience >= 0 THEN Nielsen_Audience ELSE 0 END AS Audience,  
				Nielsen_Result AS AudienceResult,
				CASE WHEN IQAdShareValue >= 0 THEN IQAdShareValue ELSE 0 END AS IQAdShareValue,				
				(SELECT Dma_Name,Dma_Num,Station_Affil,IQ_Station_ID,TimeZone,Station_Call_Sign FROM IQ_Station WHERE IQ_Station_ID = SUBSTRING(IQ_CC_Key,0 ,CHARINDEX('_',IQ_CC_Key)) FOR XML PATH(''),ROOT('StationDetail')) AS StationDetail,								
				(SELECT  IQCore_ClipMeta.Field AS [KEY],IQCore_ClipMeta.Value FROM IQCore_ClipMeta WHERE _ClipGuid = ArchiveClip.ClipID   
					AND field NOT IN('FileLocation','FileName','IOSLocation','IOSRootPathID','iQCategory','iqClientid','iQUser','NoOfTimesDownloaded','UGCFileName','UGCFileLocation','SubCategory1GUID','SubCategory2GUID','SubCategory3GUID')  
					FOR XML PATH('Meta'),ROOT('ClipMeta')  
				) AS ClipMeta  
				
		FROM 
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveClip WITH(NOLOCK)  
					ON ArchiveMedia._ArchiveMediaID = ArchiveClip.ArchiveClipKey  
					AND ArchiveMedia.MediaType='TV'  		 
				
		
		SELECT
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				Title,
				CategoryGUID,
				Keywords,
				[Description],
				PositiveSentiment,
				NegativeSentiment,
				ArticleContent AS CONTENT,
				Harvest_Time AS Mediadate,
				CreatedDate,
				Url,
				CASE WHEN Compete_Audience >= 0 THEN Compete_Audience ELSE 0 END  AS Audience,
				CASE WHEN IQAdShareValue >= 0 THEN IQAdShareValue ELSE 0 END AS IQAdShareValue,
				Compete_Result AS AudienceResult,
				Publication,
				IQLicense				
		FROM
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveNM WITH(NOLOCK)
					ON ArchiveMedia._ArchiveMediaID=ArchiveNM.ArchiveNMKey
					AND ArchiveMedia.MediaType='NM'
					
		SELECT
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				Title,
				CategoryGUID,
				Keywords,
				[Description],
				PositiveSentiment,
				NegativeSentiment,
				ArticleContent AS CONTENT,
				Harvest_Time AS Mediadate,
				CreatedDate,
				Url,
				CASE WHEN Compete_Audience >= 0 THEN Compete_Audience ELSE 0 END  AS Audience,
				CASE WHEN IQAdShareValue >= 0 THEN IQAdShareValue ELSE 0 END AS IQAdShareValue,
				Compete_Result AS AudienceResult,
				HomeLink				
		FROM
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveSM WITH(NOLOCK)
					ON ArchiveMedia._ArchiveMediaID=ArchiveSM.ArchiveSMKey
					AND ArchiveMedia.MediaType='SM'
					
		SELECT
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				Title,
				CategoryGUID,
				Keywords,
				[Description],
				PositiveSentiment,
				NegativeSentiment,
				Tweet_Body AS CONTENT,
				Tweet_PostedDateTime AS Mediadate,
				CreatedDate,
				Actor_DisplayName,
				Actor_PreferredUserName,
				Actor_link											
		FROM
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveTweets WITH(NOLOCK)
					ON ArchiveMedia._ArchiveMediaID=ArchiveTweets.ArchiveTweets_Key
					AND ArchiveMedia.MediaType='TW'
		
		SELECT
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				Title,
				CategoryGUID,
				Keywords,
				[Description],
				PositiveSentiment,
				NegativeSentiment,
				Transcript AS CONTENT,
				[UTCDateTime] AS Mediadate,
				CreatedDate,
				Location,
				Market
									
		FROM
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveTVEyes WITH(NOLOCK)
					ON ArchiveMedia._ArchiveMediaID=ArchiveTVEyes.ArchiveTVEyesKey
					AND ArchiveMedia.MediaType='TM'
					
		SELECT
				ID,
				MediaType,
				SubMediaType,
				CategoryName,
				_ParentID AS ParentID,
				Headline AS Title,
				CategoryGUID,
				Keywords,
				[Description],
				[TEXT] AS CONTENT,
				PubDate AS Mediadate,
				CreatedDate,
				FileLocation,
				Circulation,
				Pub_Name												
		FROM
				#ArchiveMedia AS ArchiveMedia
					INNER JOIN ArchiveBLPM WITH(NOLOCK)
					ON ArchiveMedia._ArchiveMediaID=ArchiveBLPM.ArchiveBLPMKey
					AND ArchiveMedia.MediaType='PM'				       
	          
	 SELECT @TotalResults AS TotalResults, @MaxSinceID AS SinceID
   
END