IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DatadownloadList_get_byUserID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DatadownloadList_get_byUserID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:			Clark Yip
-- Create date:		19 Jun 2008
-- Description:		Get Datadownload list (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	27 May 2009
-- Description:		System Parameter Add Scheme Code
-- =============================================

CREATE PROCEDURE 	[dbo].[proc_DatadownloadList_get_byUserID] @create_by			varchar(20)							

as
BEGIN
-- =============================================
-- Declaration
-- =============================================
declare @hr as int
select @hr=parm_value1 from SystemParameters
where [Parameter_Name] = 'ActiveInboxIntrayHour'and [Record_Status] = 'A' AND [Scheme_Code] = 'ALL'

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

select f.[File_ID] as reportNum
		, q.status
		, convert(varchar,DecryptByKey(q.[File_Password])) as syspassword
		, q.[Output_File] as SysEncryptedFileName
		, f.[File_name] as reportName
		, q.[request_Dtm] as submissionDate
		, q.[request_by] as submittedBy
		, f.[File_Type] as FileType
		, q.[Output_File] as OutputFile
		, q.[Generation_id] as GenerationID
		, q.[File_Description] as FileDescription
from FileGeneration f, FileGenerationQueue q, RoleTypeFileGeneration g, UserRole ur
where f.[File_ID] = q.[File_ID]
and f.[Is_SelfAccess]='N'
and g.Access_Type in ('A','D')
and g.[File_ID] = f.[File_ID]
and g.[Role_type] = ur.[Role_type]
and ur.[User_ID] = q.request_by
and ((q.[Complete_dtm] is not null and DATEDIFF(hh, q.[Complete_dtm], getdate()) <= @hr) or q.[Complete_dtm] is null)
UNION
select f.[File_ID] as reportNum
		, q.status
		, convert(varchar,DecryptByKey(q.[File_Password])) as syspassword
		, q.[Output_File] as SysEncryptedFileName
		, f.[File_name] as reportName
		, q.[request_Dtm] as submissionDate
		, q.[request_by] as submittedBy
		, f.[File_Type] as FileType
		, q.[Output_File] as OutputFile
		, q.[Generation_id] as GenerationID
		, q.[File_Description] as FileDescription
from FileGeneration f, FileGenerationQueue q
Where f.[File_ID] = q.[File_ID]
and f.[Is_SelfAccess]='Y'
and q.request_by = @create_by
and ((q.[Complete_dtm] is not null and DATEDIFF(hh, q.[Complete_dtm], getdate()) <= @hr) or q.[Complete_dtm] is null)

CLOSE SYMMETRIC KEY sym_Key

END
GO

GRANT EXECUTE ON [dbo].[proc_DatadownloadList_get_byUserID] TO HCVU
GO
