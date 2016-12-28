-- =============================================
-- Author:		<Author,,Name>
-- Create date: 19 June 2013
-- Description:	Select By Customer
-- =============================================
CREATE PROCEDURE [dbo].[usp_v4_ClipDownload_SelectByCustomer]
	@CustomerGUID	UNIQUEIDENTIFIER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT	
			ArchiveClip.ClipTitle,
			ClipDownload.IQ_ClipDownload_Key,
			ClipDownload.CustomerGUID,
			ClipDownload.ClipDownloadStatus,
			ClipDownload.ClipFileLocation,
			ClipDownload.ClipDLFormat,
			ClipDownload.ClipDLRequestDateTime,
			ClipDownload.ClipDownLoadedDateTime,
			ClipDownload.IsActive
			
	FROM	ClipDownload
	
	INNER JOIN	ArchiveClip
	ON		ArchiveClip.ClipID = ClipDownload.ClipID
	AND		ClipDownload.CustomerGUID = @CustomerGUID
	AND		ArchiveClip.IsActive = 1
	
	WHERE	ClipDownload.CustomerGUID = @CustomerGUID
	AND		ClipDownload.IsActive = 1
	AND		ClipDownload.ClipDownloadStatus IN (1,2,3)
    
END
