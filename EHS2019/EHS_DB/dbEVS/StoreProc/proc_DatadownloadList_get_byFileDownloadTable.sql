IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DatadownloadList_get_byFileDownloadTable]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DatadownloadList_get_byFileDownloadTable]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	04 Oct 2018
-- CR No.:			INT18-0014
-- Description:		If [FileGeneration].[Is_SelfAccess] = 'Y', allow the user to dnload if exists in [FileDownload] or the user is requester
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	25 May 2016
-- CR No.:			CRE15-016
-- Description:		Exclude FileGenerationQueue Status "T" (Terminated)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Get Datadownload list join the FileDownload (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	04 Oct 2008
-- Description:		Filter out the download files when the day is over 180 days (parameter)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_DatadownloadList_get_byFileDownloadTable]
	 @create_by			varchar(20)	
	,@download_status	char(1)						

as
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare @day as int
select @day=parm_value1 from SystemParameters
where [Parameter_Name] = 'DatadownloadKeepDay'and [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'

declare @recycle_bin_day as int
select @recycle_bin_day=parm_value1 from SystemParameters
where [Parameter_Name] = 'RecycleBinKeepDay'and [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'
		
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
OPEN SYMMETRIC KEY sym_Key 
	DECRYPTION BY ASYMMETRIC KEY asym_Key
-- =============================================
-- Return results
-- =============================================


select distinct f.[File_ID] as reportNum
		, f.Display_Code
		, q.status
		, convert(varchar,DecryptByKey(q.[File_Password])) as syspassword
		, q.[Output_File] as SysEncryptedFileName
		, f.[File_name] as reportName
		, q.[request_Dtm] as submissionDate
		, q.[request_by] as submittedBy
		, f.[File_Type] as FileType
		, q.[Output_File] as OutputFile
		, q.[Generation_id] as GenerationID
		, fd.[Download_Status] as DownloadStatus
		, fd.[User_ID] as Recipient
		, q.[File_Description] as FileDescription
from FileGeneration f, FileGenerationQueue q, RoleTypeFileGeneration g, UserRole ur, FileDownload fd
where f.[File_ID] = q.[File_ID]

and ((f.[Is_SelfAccess]='N' and g.Access_Type in ('A','D')) OR (f.[Is_SelfAccess]='Y' and (q.request_by = @create_by OR fd.User_ID = @create_by)))

and g.[File_ID] = f.[File_ID]
and g.[Role_type] = ur.[Role_type]
and q.[Generation_id]=fd.[Generation_id]

AND ur.[User_ID] = @create_by
AND fd.[User_ID] = @create_by

--and ur.[User_ID] = q.request_by
--and fd.[user_id]=@create_by

and ((@download_status = 'I' and fd.[Download_Status]='I') or (@download_status<>'I' and fd.[Download_Status]in('N','D')))
--and ((q.[Complete_dtm] is not null and DATEDIFF(hh, q.[Complete_dtm], getdate()) <= @hr) or q.[Complete_dtm] is null)
and (
		(
			q.[Complete_dtm] is not null and 
			(
				(fd.[Download_Status]='I' AND DATEDIFF(dd, fd.[Update_dtm], getdate()) <= @recycle_bin_day AND DATEDIFF(dd, q.[request_Dtm], getdate()) <= @day)
				or (fd.[Download_Status]in('N','D') AND DATEDIFF(dd, q.[request_Dtm], getdate()) <= @day)
			)
		) 	
		or q.[Complete_dtm] is null
	)
and q.status NOT IN ('I', 'T')   --q.status <> 'I'

order by q.[request_Dtm] desc

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_DatadownloadList_get_byFileDownloadTable] TO HCVU
GO
