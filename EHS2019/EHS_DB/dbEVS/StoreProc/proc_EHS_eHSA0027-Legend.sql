IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-Legend]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-Legend]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Helen Lam
-- Modified date:	27 Jan 2012
-- Description:		eHSA0027 - FHB statistics for 2011 (CRD12-002)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	1 February 2011
-- Description:		eHSA0018 - FHB statistics for 2010
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-Legend]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	Declare @strYear char(4)
	set @strYear=cast(@Year as char(4))
	DECLARE @ResultTable table (
		Result_Seq		smallint,
		Result_Value1	varchar(400) DEFAULT '',
		Result_Value2	varchar(100) DEFAULT ''
	)
	
	DECLARE @Transaction_PendingConfirmation_Count	int
	
	DECLARE @SPExceptionList	varchar(500)


-- =============================================
-- Retrieve data
-- =============================================

	SELECT
		@Transaction_PendingConfirmation_Count = COUNT(1)
	FROM
		VoucherTransaction
	WHERE
		YEAR(Transaction_Dtm) = @Year
			AND Scheme_Code = 'HCVS'
			AND Record_Status = 'P'

--

	SET 
		@SPExceptionList = (
			SELECT
				 LTRIM(RTRIM(SP_ID)) + ', '
			FROM
				SPExceptionList
			ORDER BY
				SP_ID
			FOR
				XML PATH('')
		)
	
	IF @SPExceptionList IS NULL BEGIN
		SET @SPExceptionList = ''
	END
		
	SET @SPExceptionList = RTRIM(@SPExceptionList)
		
	IF LEN(@SPExceptionList) > 1 BEGIN
		SET @SPExceptionList = SUBSTRING(@SPExceptionList, 1, LEN(@SPExceptionList) - 1)
	END


-- =============================================
-- Build frame
-- =============================================

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(0, '(A) Legned')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(1, '1. Health Profession:')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(2, 'ENU', 'Enrolled Nurses')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(3, 'RCM', 'Registered Chinese Medicine Practitioners')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(4, 'RCP', 'Registered Chiropractors')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(5, 'RDT', 'Registered Dentists')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(6, 'RMP', 'Registered Medical Practitioners')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(7, 'RMT', 'Registered Medical Laboratory Technologists')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(8, 'RNU', 'Registered Nurses')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(9, 'ROT', 'Registered Occupational Therapists')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(10, 'RPT', 'Registered Physiotherapists')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(11, 'RRD', 'Registered Radiographers')

	INSERT INTO @ResultTable (Result_Seq) VALUES
	(15)

--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(20, '2. District:')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(21, 'CW', 'Central & West.')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(22, 'EAST', 'Eastern')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(23, 'SOUTH', 'Southern')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(24, 'WC', 'Wan Chai')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(25, 'KC', 'Kowloon City')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(26, 'KT', 'Kwun Tong')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(27, 'SSP', 'Sham Shui Po')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(28, 'WTS', 'Wong Tai Sin')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(29, 'YTM', 'Yau Tsim Mong')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(30, 'ISL', 'Islands')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(31, 'KTS', 'Kwai Tsing')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(32, 'NORTH', 'North')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(33, 'SK', 'Sai Kung')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(34, 'ST', 'Sha Tin')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(35, 'TP', 'Tai Po')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(36, 'TW', 'Tsuen Wan')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(37, 'TM', 'Tuen Mun')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(38, 'YL', 'Yuen Long')

	INSERT INTO @ResultTable (Result_Seq) VALUES
	(39)

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(40, '3. Abbreviation list:')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(41, 'PCS', 'Private Clinic Solution')
	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2) VALUES
	(42, 'IVRS', 'Interactive Voice Response System')

	INSERT INTO @ResultTable (Result_Seq) VALUES
	(43)
--

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(44, '(B) Common Note(s) for the report')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(45, 'a)  All service providers are counted including delisted status and those in the exception list.')

	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(46, '      * SPIDs in the exception list: ' + @SPExceptionList)
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(47, 'b)  "Transactions" refers to claim transactions under HCVS with transaction date in ' + @strYear +' entered by service providers or back office users and have not been voided or invalidated at the time of statistics generation.')
	
	INSERT INTO @ResultTable (Result_Seq, Result_Value1) VALUES
	(48, '      Transactions with pending confirmation status are also included. The no. of transactions with pending confirmation status is ' + CONVERT(varchar, @Transaction_PendingConfirmation_Count) + ' up to the report generation time.')


-- =============================================
-- Return result
-- =============================================

	SELECT 
		Result_Value1,
		Result_Value2
	FROM
		@ResultTable
	ORDER BY
		Result_Seq

set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-Legend] TO HCVU
GO

