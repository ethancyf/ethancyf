IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_DeathRecordMatchResult_Recheck]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_DeathRecordMatchResult_Recheck]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		18 May 2011
-- CR No.:			CRE11-007
-- Description:		Recheck the [With_Claim] and [With_Suspicious_Claim] in [DeathRecordMatchResult]
-- Note:			Parameters @EHA_Acc_Type and @DOD are optional, you can supply to speed up the processing of this stored procedure.
--					If they are not supplied, they will still be retrieved from tables.
--					Notice PLEASE supply ZERO or BOTH the two optional parameters, do not supply one only.
--					You will know why in the Initialization section.
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- CR No.:			
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_DeathRecordMatchResult_Recheck]
	@EHA_Acc_ID				char(15),
	@EHA_Doc_Code			char(20),
	@User_ID				varchar(20),
	@EHA_Acc_Type			char(1) = NULL,
	@DOD					datetime = NULL
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Declaration
-- =============================================
	DECLARE @Tran_Count			smallint
	DECLARE @S_Tran_Count		smallint
	
	
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	IF @EHA_Acc_Type IS NULL BEGIN
		SELECT
			@EHA_Acc_Type = R.EHA_Acc_Type,
			@DOD = E.DOD
		FROM
			DeathRecordMatchResult R
				INNER JOIN DeathRecordEntry E
					ON R.Encrypt_Field1 = E.Encrypt_Field1
						AND R.Death_Record_File_ID = E.Death_Record_File_ID
		WHERE
			R.EHA_Acc_ID = @EHA_Acc_ID
				AND R.EHA_Doc_Code = @EHA_Doc_Code
	END	

	
-- =============================================
-- Process
-- =============================================
	SET @Tran_Count = 0
	SET @S_Tran_Count = 0
	
	IF @EHA_Acc_Type = 'V' BEGIN
		SELECT
			@Tran_Count = COUNT(1)
		FROM
			VoucherTransaction WITH(NOLOCK)
		WHERE
			Voucher_Acc_ID = @EHA_Acc_ID
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
				
		SELECT
			@S_Tran_Count = COUNT(1)
		FROM
			VoucherTransaction WITH(NOLOCK)
		WHERE
			Voucher_Acc_ID = @EHA_Acc_ID
				AND Service_Receive_Dtm > @DOD
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
	
	END ELSE IF @EHA_Acc_Type = 'T' BEGIN
		SELECT
			@Tran_Count = COUNT(1)
		FROM
			VoucherTransaction WITH(NOLOCK)
		WHERE
			Temp_Voucher_Acc_ID = @EHA_Acc_ID
				AND Voucher_Acc_ID = ''
				AND Special_Acc_ID IS NULL
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
				
		SELECT
			@S_Tran_Count = COUNT(1)
		FROM
			VoucherTransaction WITH(NOLOCK)
		WHERE
			Temp_Voucher_Acc_ID = @EHA_Acc_ID
				AND Voucher_Acc_ID = ''
				AND Special_Acc_ID IS NULL
				AND Service_Receive_Dtm > @DOD
				AND Record_Status NOT IN ('I', 'D')
				AND ISNULL(Invalidation, '') <> 'I'
		
	END
	
	UPDATE
		DeathRecordMatchResult
	SET
		With_Claim = 
			CASE
				WHEN @Tran_Count = 0 THEN 'N'
				ELSE 'Y'
			END,
		With_Suspicious_Claim = 
			CASE
				WHEN @S_Tran_Count = 0 THEN 'N'
				ELSE 'Y'
			END,
		Match_Dtm = GETDATE(),
		Match_By = @User_ID
	WHERE
		EHA_Acc_ID = @EHA_Acc_ID
			AND EHA_Doc_Code = @EHA_Doc_Code


END
GO

GRANT EXECUTE ON [dbo].[proc_DeathRecordMatchResult_Recheck] TO HCVU
GO
