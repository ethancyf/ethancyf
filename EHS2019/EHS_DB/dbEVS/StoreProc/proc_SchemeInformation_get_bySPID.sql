IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeInformation_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeInformation_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			CRE15-004
-- Modified by:		Lawrence TSANG
-- Modified date:	21 July 2015
-- Description:		Change the SProc to automatically read the latest SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE12-001
-- Modified by:		Tony FUNG
-- Modified date:	07 Feb 2012
-- Description:		1. Grant permission to WSINT for PCDInterface
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================
-- =============================================
-- Author:		Kathy LEE
-- Create date: 26 July 2008
-- Description:	Retrieve the Scheme Information from Table
--				SchemeInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Kathy LEE
-- Modified date: 7 May 2009
-- Description:	1. Remove "Service Fee From", "Service Fee To"
--				2. Add "Delist Status", "Delist Dtm", "Effective_Dtm" and "Logo_Return_Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	12 May 2009
-- Description:		1. Add a comma (,) after the field "Delist_Dtm"
--					2. Change the typing mistake field "Logo_Retunr_Dtm" to "Logo_Return_Dtm"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	  Lawrence TSANG
-- Modified date: 18 May 2009
-- Description:	  Inner join an additional table "SPAccountMaintenance"
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 June 2009
-- Description:		Inner join MasterScheme to get Sequence_No
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 August 2009
-- Description:		Revise the [Record_Status] logic
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	5 August 2009
-- Description:		Inner join SchemeBackOffice
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 August 2009
-- Description:		Check today is between [SchemeBackOffice].[Effective_Dtm] and [SchemeBackOffice].[Expiry_Dtm]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	Derek LEUNG
-- Modified date:	09 Nov 2010
-- Description:	Grant execute permission to WSEXT	
-- =============================================

CREATE PROCEDURE proc_SchemeInformation_get_bySPID
	@sp_id	char(8)
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @EffectiveScheme table (  
		Scheme_Code	char(10),  
		Scheme_Seq	int  
	)  

-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	INSERT INTO @EffectiveScheme  
	SELECT  
		Scheme_Code,  
		MAX(Scheme_Seq)  
	FROM  
		SchemeBackOffice  
	WHERE  
		Record_Status = 'A'  
			AND Effective_Dtm < GETDATE()  
	GROUP BY
		Scheme_Code   

-- =============================================
-- Return results
-- =============================================

/*********************************************** 
	[Record_Status] reference:
		Active						A  
		Suspended					W  
		DelistedVoluntary			V  
		DelistedInvoluntary			I  
		ActivePendingSuspend		S  
		ActivePendingDelist			D  
		SuspendedPendingDelist		X  
		SuspendedPendingReactivate	Y  
 **********************************************/

	SELECT		S.SP_ID,
				S.Scheme_Code,
				CASE S.Record_Status
					WHEN 'A' THEN
						ISNULL(M.Upd_Type, 'A')
					WHEN 'S' THEN
						CASE ISNUll(M.Upd_Type, '')
							WHEN 'R' THEN
								'Y'
							WHEN 'D' THEN
								'X'
							ELSE 'W'
						END
					WHEN 'D' THEN
						ISNUll(S.Delist_Status, '')
				END [Record_Status],
				CASE S.Record_Status
					WHEN 'A' THEN 
						CASE ISNUll(M.Upd_Type, '')
							WHEN '' THEN
								''
							ELSE ISNUll(M.Remark, '')
						END
					WHEN 'S' THEN
						CASE ISNULL(M.Upd_Type, '')
							WHEN 'D' THEN
								ISNULL(M.Remark, '')
							ELSE ISNUll(S.Remark, '')
						END
					WHEN 'D' THEN 
						ISNUll(S.Remark, '')
				END [Remark],
				S.Delist_Status,
				S.Effective_Dtm,
				S.Delist_Dtm,
				S.Logo_Return_Dtm,
				S.Create_Dtm,
				S.Create_By,
				S.Update_Dtm,
				S.Update_By,
				S.TSMP,
				B.Display_Seq
				
	FROM		SchemeInformation S
					LEFT JOIN SPAccountMaintenance M
						ON S.SP_ID = M.SP_ID
							AND S.Scheme_Code = M.Scheme_Code
							AND M.Record_Status = 'A'
							AND M.SP_Practice_Display_Seq IS NULL
					INNER JOIN @EffectiveScheme E  
						ON S.Scheme_Code = E.Scheme_Code  
					INNER JOIN SchemeBackOffice B   
						ON E.Scheme_Code = B.Scheme_Code
							AND E.Scheme_Seq = B.Scheme_Seq  
					
	WHERE		S.SP_ID = @sp_id
	
	ORDER BY	M.System_Dtm DESC
	
END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_get_bySPID] TO WSEXT
GO

GRANT EXECUTE ON [dbo].[proc_SchemeInformation_get_bySPID] TO WSINT
GO