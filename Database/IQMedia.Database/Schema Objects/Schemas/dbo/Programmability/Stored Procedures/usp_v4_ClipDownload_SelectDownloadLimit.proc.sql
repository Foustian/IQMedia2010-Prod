-- =============================================
-- Author:		<Author,,Name>
-- Create date: 19 June 2013
-- Description:	Return no of download count by customer
-- =============================================
CREATE PROCEDURE [dbo].[usp_v4_ClipDownload_SelectDownloadLimit]
	@CustomerGUID	UNIQUEIDENTIFIER
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	COUNT(*) AS DownloadCount FROM [dbo].[ClipDownload]
	WHERE	CustomerGUID = @CustomerGUID
	AND		ClipDownloadStatus NOT IN (0,4)
	AND     IsActive = 1
END
