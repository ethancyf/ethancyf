IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_CMSVaccinationRecordTransaction_Stat_Write_ByDtm]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordTransaction_Stat_Write_ByDtm]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Lawrence TSANG
-- Create date:		10 November 2010
-- Description:		Statistics for CMS Vaccination Record
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_CMSVaccinationRecordTransaction_Stat_Write_ByDtm]
	@Start_Dtm	datetime,
	@End_Dtm	datetime
AS BEGIN

	SET NOCOUNT ON;

-- =============================================
-- Temporary tables
-- =============================================
	DECLARE @TransactionExtRefStatus table (
		Status_Title			varchar(30),
		Status_Code				char(3),
		Status_Count			int
	)

	DECLARE @TimeResult table (
		Function_Code			char(6),
		Start_Dtm				datetime,
		End_Dtm					datetime,
		Compare_Time			int,
		Session_ID				varchar(MAX)
	)
	
	DECLARE @ResultTable table (
		Result_Function			varchar(30),
		Result_Log_ID			char(5),
		Result_Title			varchar(30),
		Result_Value1			varchar(30),
		Result_Value2			varchar(30),
		Result_Value3			varchar(30)
	)


-- =============================================
-- Initialization
-- =============================================
	
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('DocumentNotAccept', '_DN')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('FullMatchWithRecord', '_FY')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('FullMatchNoRecord', '_FN')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('PatientNotFound', '_NN')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('DemographicsNotMatch', '_PN')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('ConnectionFail', '_CN')
	INSERT INTO @TransactionExtRefStatus(Status_Title, Status_Code) VALUES ('Suspend', '_UN')


-- =============================================
-- Process data
-- =============================================

-- Retrieve count

	UPDATE
		@TransactionExtRefStatus
	SET
		Status_Count = (
			SELECT
				COUNT(1)
			FROM
				VoucherTransaction
			WHERE
				Scheme_Code <> 'HCVS'
					AND Transaction_Dtm BETWEEN @Start_Dtm AND @End_Dtm
					AND SUBSTRING(Ext_Ref_Status, 1, 3) LIKE Status_Code
		)
		
-- Retrieve total transaction

	INSERT INTO @TransactionExtRefStatus(
		Status_Title,
		Status_Count
	)
	SELECT
		'All',
		COUNT(1)
	FROM
		VoucherTransaction
	WHERE
		Scheme_Code <> 'HCVS'
			AND Transaction_Dtm BETWEEN @Start_Dtm AND @End_Dtm


-- =============================================
-- Store into tables
-- =============================================

-- Clear today's record if any

	DELETE FROM
		_CMSVaccinationRecordStat
	WHERE
		Report_Dtm = CONVERT(varchar(10), @Start_Dtm, 20)  -- To yyyy-mm-dd
			AND Result_Function IN ('Transaction')

-- Insert record

	INSERT INTO _CMSVaccinationRecordStat (
		System_Dtm,
		Report_Dtm,
		Result_Function,
		Result_Title,
		Result_Value1
	)
	SELECT
		GETDATE() AS [System_Dtm],
		CONVERT(varchar(10), @Start_Dtm, 20) AS [Report_Dtm],  -- To yyyy-mm-dd
		'Transaction' AS [Result_Function],
		Status_Title AS [Result_Title],
		Status_Count AS [Result_Value1]
	FROM
		@TransactionExtRefStatus
		

END
GO

GRANT EXECUTE ON [dbo].[proc_EHS_CMSVaccinationRecordTransaction_Stat_Write_ByDtm] TO HCVU
GO

