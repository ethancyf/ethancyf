IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_PracticeSchemeInformation_get_bySPID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_PracticeSchemeInformation_get_bySPID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-015-09
-- Created by:		Chris YIM
-- Created date:	02 Dec 2020
-- Description:		Get all practice scheme info
-- =============================================

CREATE PROCEDURE [dbo].[proc_PracticeSchemeInformation_get_bySPID]
	@sp_id			CHAR(8)
AS
BEGIN

	SET NOCOUNT ON;
-- =============================================
-- Declaration
-- =============================================
	CREATE Table #temp_practice(	
		sp_id CHAR(8),
		practice_display_seq SMALLINT,
		scheme_code CHAR(10),
		service_fee SMALLINT,
		record_status CHAR(2),
		delist_status CHAR(1),
		remark NVARCHAR(255),
		effective_dtm DATETIME,
		delist_dtm DATETIME,
		create_by VARCHAR(20),
		create_dtm DATETIME,
		update_by VARCHAR(20),
		update_dtm DATETIME,								
		subsidize_code CHAR(10),
		provideservicefee CHAR(1),
		scheme_display_seq SMALLINT,
		subsidize_display_seq SMALLINT,
		scheme_seq SMALLINT,
		subsidize_record_status CHAR(1),
		provide_service CHAR(1),
		Clinic_Type CHAR(1)
	)
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
-- =============================================
-- Return results
-- =============================================

/***********************************************
	[Record_Status] reference:
		Active						A  
		Suspended					W  
		DelistedVoluntary			V  
		DelistedInvoluntary			I  
		ActivePendingSuspend		SP 
		ActivePendingDelist			DP 
		SuspendedPendingDelist		XP 
		SuspendedPendingReactivate	YP 
 ***********************************************/

	INSERT INTO #temp_practice
	(
		sp_id,
		practice_display_seq,
		scheme_code,
		service_fee,
		record_status,
		delist_status,
		remark,
		effective_dtm,
		delist_dtm,
		create_by,
		create_dtm,
		update_by,
		update_dtm,
		subsidize_code,
		provideservicefee,
		scheme_display_seq,
		scheme_seq,
		provide_service,
		Clinic_Type
	)
	SELECT	P.SP_ID,
			P.Practice_Display_Seq,
			P.Scheme_Code,
			P.Service_Fee,
			CASE P.Record_Status
				WHEN 'A' THEN ISNULL(M.Upd_Type, 'A')		-- 3 outputs: A, SP, DP
				WHEN 'S' THEN 
					CASE ISNULL(M.Upd_Type, '')
						WHEN 'RP' THEN 'YP'
						WHEN 'DP' THEN 'XP'
						ELSE 'W'
					END										-- 3 outputs: W, YP, XP
				WHEN 'D' THEN P.Delist_Status				-- 2 outputs: V, I
			END AS [Record_Status],
			P.Delist_Status,
			CASE P.Record_Status
				WHEN 'A' THEN
					CASE ISNULL(M.Upd_Type, '')
						WHEN '' THEN ''
						ELSE ISNULL(M.Remark, '')
					END
				WHEN 'S' THEN ISNULL(P.Remark, '')
				WHEN 'D' THEN ISNULL(P.Remark, '')
			END AS [Remark],
			P.Effective_dtm,
			P.Delist_Dtm,
			P.Create_By,
			P.Create_Dtm,
			P.Update_By,
			P.Update_Dtm,
			--P.TSMP,
			P.subsidize_code,
			P.ProvideServiceFee,
			s.display_seq,
			s.scheme_seq,
			P.Provide_Service,
			P.Clinic_Type
	FROM	PracticeSchemeInfo P
				LEFT JOIN SPAccountMaintenance M
					ON P.SP_ID = M.SP_ID 
						AND M.Record_Status = 'A' 
						AND P.Practice_Display_Seq = M.SP_Practice_Display_Seq 
						AND P.Scheme_Code = M.Scheme_Code
				Inner join schemebackoffice s
					on  p.scheme_code = s.scheme_code and
						getdate() between s.effective_dtm and s.expiry_dtm
						
	WHERE	P.SP_ID = @sp_id 
				--AND P.Practice_Display_Seq = @display_seq 
				
	UPDATE #temp_practice
	SET subsidize_display_seq  = sg.display_seq,
		subsidize_record_status = sg.record_status
	FROM #temp_practice t, subsidizegroupbackoffice sg
	WHERE t.scheme_code = sg.scheme_code and
		  t.subsidize_code = sg.subsidize_code and
		  t.scheme_seq = sg.scheme_seq
				
	
	SELECT	t.sp_id,
			t.practice_display_seq,
			t.scheme_code,
			t.service_fee,
			t.record_status,
			t.delist_status,
			t.remark,
			t.effective_dtm,
			t.delist_dtm,
			t.create_by,
			t.create_dtm,
			t.update_by,
			t.update_dtm,
			t.subsidize_code,
			t.provideservicefee,
			t.scheme_display_seq,
			t.subsidize_display_seq,
			p.tsmp,
			t.provide_service,
			t.Clinic_Type
	FROM	#temp_practice t, PracticeSchemeInfo P
	WHERE	t.sp_id = p.sp_id and
			t.Practice_Display_Seq = p.Practice_Display_Seq and
			t.scheme_code = p.scheme_code and
			t.subsidize_code = p.subsidize_code	and
			t.subsidize_record_status <> 'I'

	DROP TABLE #temp_practice
END
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPID] TO HCSP
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPID] TO HCVU
GO

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPID] TO WSEXT
Go

GRANT EXECUTE ON [dbo].[proc_PracticeSchemeInformation_get_bySPID] TO WSINT
GO

