IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_HCVRIVRS_Stat_Write_ByReportDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_HCVRIVRS_Stat_Write_ByReportDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	30 December 2009
-- Description:		Handle cross-year 2009 to 2028
-- =============================================
-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		23 October 2009
-- Description:		Generate report for HCVR platform - IVRS
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_HCVRIVRS_Stat_Write_ByReportDtm] 
	@Report_Dtm	datetime
AS BEGIN
-- =============================================
-- Declaration
-- =============================================
	DECLARE @AuditLog table (
		System_Dtm			datetime,
		Function_Code		char(6),
		Log_ID				char(10),
		CallConnectionID	varchar(100)
	)
	
	DECLARE @ResultTable table (
		Display_Seq		smallint,
		Result_Value1	varchar(100),
		Result_Value2	varchar(100),
		Result_Value3	varchar(100)
	)
	
	DECLARE @Year		smallint
-- =============================================
-- Validation 
-- =============================================
-- =============================================
-- Initialization
-- =============================================
	SET @Year = CONVERT(varchar(2), DATEADD(dd, -1, @Report_Dtm), 12)
-- =============================================
-- Return results
-- =============================================

-- ---------------------------------------------
-- Retrieve data
-- ---------------------------------------------

	EXEC [proc_SymmetricKey_open]

	IF @Year = '09' BEGIN

		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR09
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'
					
	END ELSE IF @Year = '10' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR10
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '11' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR11
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '12' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR12
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '13' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR13
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '14' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR14
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '15' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR15
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '16' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR16
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '17' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR17
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '18' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR18
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '19' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR19
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '20' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR20
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '21' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR21
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '22' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR22
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '23' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR23
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '24' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR24
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '25' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR25
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '26' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR26
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '27' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR27
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END ELSE IF @Year = '28' BEGIN
			
		INSERT INTO @AuditLog (
			System_Dtm,
			Function_Code,
			Log_ID,
			CallConnectionID
		)
		SELECT
			System_Dtm,
			CONVERT(char(6), DecryptByKey(E_Function_Code)),
			CONVERT(char(10), DecryptByKey(E_Log_ID)),
			CONVERT(varchar(100), DecryptByKey(E_CallConnectionID))
		FROM
			AuditLogIVRSVR28
		WHERE
			CONVERT(varchar, DecryptByKey(E_Function_Code)) = '990207'
				AND System_Dtm
					BETWEEN CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00' 
					AND		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 23:59:59.999'		
			
	END

	EXEC [proc_SymmetricKey_close]

-- ---------------------------------------------
-- Build data
-- ---------------------------------------------

	INSERT INTO @ResultTable (Display_Seq, Result_Value1, Result_Value2, Result_Value3) 
	VALUES (10, '', '', '')

-- Count of Search

	UPDATE
		@ResultTable
	SET
		Result_Value1 = (
						SELECT
							COUNT(1)
						FROM
							@AuditLog
						WHERE
							Log_ID = '00001'
						)
	WHERE
		Display_Seq = 10

-- Count of Session with Search

	UPDATE
		@ResultTable
	SET
		Result_Value2 = (
						SELECT
							COUNT(DISTINCT CallConnectionID)
						FROM
							@AuditLog
						WHERE
							Log_ID = '00001'
						)
	WHERE
		Display_Seq = 10
		
-- Count of Session

	UPDATE
		@ResultTable
	SET
		Result_Value3 = (
						SELECT
							COUNT(DISTINCT CallConnectionID)
						FROM
							@AuditLog
						)
	WHERE
		Display_Seq = 10
	
-- ---------------------------------------------
-- Insert data to temporary table
-- ---------------------------------------------

	INSERT INTO _EHS_HCVRIVRS_Stat (
		System_Dtm,
		Report_Dtm,
		Display_Seq,
		Result_Value1,
		Result_Value2,
		Result_Value3
	)
	SELECT
		GETDATE(),
		CONVERT(varchar(11), DATEADD(dd, -1, @Report_Dtm), 106) + ' 00:00:00',
		Display_Seq,
		Result_Value1,
		Result_Value2,
		Result_Value3
	FROM
		@ResultTable
	ORDER BY
		Display_Seq
	
END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_HCVRIVRS_Stat_Write_ByReportDtm] TO HCVU
GO
