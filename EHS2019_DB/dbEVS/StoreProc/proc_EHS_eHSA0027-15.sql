IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSA0027-15]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSA0027-15]
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

CREATE PROCEDURE [dbo].[proc_EHS_eHSA0027-15]
	@Year	int
AS BEGIN

	SET NOCOUNT ON;
	
-- =============================================
-- Declaration
-- =============================================
	Declare @strYear char(4)
	Declare @seq int
	set @strYear= cast(@Year as char(4))
	
	DECLARE @SP table (
		SP_ID					varchar(8),
		Service_Type			varchar(5),
		Scheme_Effective_Dtm	datetime,
		No_Of_Transaction		int,
		No_Of_Voucher			int,
		Record_Status_D			varchar(100),
		create_by_smartid		char(1),
		sourceapp				varchar(10)
	)
	
	DECLARE @ResultTable table(
		Result_Seq				smallint,
		Result_Value1			varchar(100) DEFAULT '',
		Result_Value2			varchar(40) DEFAULT '',
		Result_Value3			varchar(30) DEFAULT '',
		Result_Value4			varchar(30) DEFAULT '',
		Result_Value5			varchar(30) DEFAULT '0',
		Result_Value6			varchar(30) DEFAULT '0',
		Result_Value7			varchar(30) DEFAULT '0',
		Result_Value8			varchar(30) DEFAULT '0',
		Result_Value9			varchar(30) DEFAULT '0',
		Result_Value10			varchar(30) DEFAULT '0',
		Result_Value11			varchar(30) DEFAULT '0',
		Result_Value12			varchar(30) DEFAULT '0'

		
	)


-- =============================================
-- Retrieve data
-- =============================================

-- ---------------------------------------------
-- Claimed SP
-- ---------------------------------------------

	INSERT INTO @SP (
		SP_ID,
		Service_Type,
		Scheme_Effective_Dtm,
		No_Of_Transaction,
		No_Of_Voucher,
		Record_Status_D,
		create_by_smartid, 
		sourceapp
	)
	SELECT DISTINCT
		VT.SP_ID,
		VT.Service_Type,
		NULL AS [Scheme_Effective_Dtm],
		COUNT(1) AS [No_Of_Transaction],
		SUM(VT.Voucher_Before_Claim - VT.Voucher_After_Claim) AS [No_Of_Voucher],
		SD.Status_Description AS [Record_Status_D],
		isnull(VT.create_by_smartid,'N') as create_by_smartid,
		case VT.sourceapp when 'IVRS' then 'IVRS'
		 when 'externalws' then 'PCS'
			  else 'WEB'  end as SourceApp
	FROM
		VoucherTransaction VT
			INNER JOIN ServiceProvider SP	
				ON VT.SP_ID = SP.SP_ID
			INNER JOIN StatusData SD
				ON SP.Record_Status = SD.Status_Value
					AND SD.Enum_Class = 'ServiceProviderStatus'
	WHERE
		VT.Scheme_Code = 'HCVS'
			AND YEAR(VT.Transaction_Dtm) = @Year
			AND VT.Record_Status NOT IN ('I', 'D')
			AND ISNULL(VT.Invalidation, '') <> 'I'			
	GROUP BY
		VT.SP_ID,
		VT.Service_Type,
		SD.Status_Description,
		isnull(VT.create_by_smartid,'N') ,
		case VT.sourceapp when 'IVRS' then 'IVRS'
		when 'externalws' then 'PCS'
		 else 'WEB'  end
	


-- ---------------------------------------------
-- Patch the Scheme_Effective_Dtm
-- ---------------------------------------------

	UPDATE
		@SP
	SET
		Scheme_Effective_Dtm = SI.Effective_Dtm
	FROM
		@SP S
			INNER JOIN SchemeInformation SI
				ON S.SP_ID = SI.SP_ID
					AND SI.Scheme_Code = 'HCVS'


-- =============================================
-- Build frame
-- =============================================

-- ---------------------------------------------
-- Header
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12) VALUES
	(0, 'eHSA0027-14: Statistics of transactions and vouchers of EHCP in '+@strYear +' broken down by means of input' ,'','','','','','','','' )
	
	INSERT INTO @ResultTable (Result_Seq,Result_Value5, Result_Value7,Result_Value9,Result_Value10,Result_Value11,Result_Value12) VALUES
	(1,'','','','','','')
	INSERT INTO @ResultTable (Result_Seq,  Result_Value5, Result_Value7,Result_Value9,Result_Value10,Result_Value11,Result_Value12) VALUES
	(2, 'Internet (By Manual)','Internet (By Smart IC)', 'IVRS','','PCS','' )

		

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6,Result_Value7, Result_Value8,Result_Value9,Result_Value10,Result_Value11,Result_Value12) VALUES
	(10, 'SPID', 'HCVS enrolment effective date', 'Profession', 'Status', 'No. of transactions', 'No. of vouchers', 'No. of transactions', 'No. of vouchers','No. of transactions', 'No. of vouchers','No. of transactions', 'No. of vouchers')

--	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6) VALUES
--	(10, 'SPID', 'HCVS enrolment effective date', 'Profession', 'Status', 'No. of transactions', 'No. of vouchers')


-- ---------------------------------------------
-- Data for EHCP 
-- ---------------------------------------------

	INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)
	SELECT
		10 + ROW_NUMBER() OVER(ORDER BY SP_ID, Service_Type),
		SP_ID,
		CONVERT(varchar, Scheme_Effective_Dtm, 20) AS [Scheme_Effective_Dtm],
		Service_Type,
		Record_Status_D
	FROM
		@SP 
	group by
		SP_ID,
		[Scheme_Effective_Dtm],
		Service_Type,
		Record_Status_D 
	ORDER BY
		SP_ID,
		Service_Type






---- ---------------------------------------------
---- Data for EHCP by Manual
---- ---------------------------------------------

	update 
	@ResultTable set Result_Value5=No_Of_Transaction,
				    Result_Value6= No_Of_Voucher
		from @SP s, @ResultTable t
where s.create_by_smartid='N'
	and t.Result_Value1=s.sp_id and t.result_value3=s.service_type
	and s.sourceapp='WEB'

	
-- ---------------------------------------------
-- Data for EHCP by Smart IC
-- ---------------------------------------------

	
	update 
	@ResultTable set Result_Value7= No_Of_Transaction,
				    Result_Value8= No_Of_Voucher 
		from @SP s, @ResultTable t 
where s.create_by_smartid='Y'
	and t.Result_Value1=s.sp_id and t.result_value3=s.service_type
		and s.sourceapp='WEB'


-- ---------------------------------------------
-- Data for EHCP by IVRS
-- ---------------------------------------------

	update 
	@ResultTable set Result_Value9= No_Of_Transaction,
				    Result_Value10= No_Of_Voucher 
		from @SP s, @ResultTable t where 
		 t.Result_Value1=s.sp_id and t.result_value3=s.service_type
		and s.sourceapp='IVRS'

-- ---------------------------------------------
-- Data for EHCP by PCS
-- ---------------------------------------------

	update 
	@ResultTable set Result_Value11= No_Of_Transaction,
				    Result_Value12= No_Of_Voucher 
		from @SP s, @ResultTable t where  t.Result_Value1=s.sp_id and t.result_value3=s.service_type
		and s.sourceapp='PCS'

	
--select  @seq = max(Result_Seq) from @ResultTable
--set @seq=@seq +1
--
--INSERT INTO @ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value7, Result_Value8,Result_Value9,Result_Value10)
--	SELECT
--		@seq+ ROW_NUMBER() OVER(ORDER BY SP_ID, Service_Type),
--		SP_ID,
--		CONVERT(varchar, Scheme_Effective_Dtm, 20) AS [Scheme_Effective_Dtm],
--		Service_Type,
--		Record_Status_D,
--		No_Of_Transaction,
--		No_Of_Voucher
--	FROM
--		@SP s where s.create_by_smartid='Y'
--		and not exists ( select Result_Value1,t.Result_Value3 from @ResultTable t where 
--		 t.Result_Value1=s.sp_id and t.result_value3=s.service_type)
--	ORDER BY
--		SP_ID,
--		Service_Type




-- =============================================
-- Return result 
-- =============================================

	SELECT
		Result_Value1,
		Result_Value2,
		Result_Value3,
		Result_Value4,
		Result_Value5,
		Result_Value6,
		Result_Value7,
		Result_Value8,
		Result_Value9,
		Result_Value10,
		Result_Value11,
		Result_Value12


	FROM
		@ResultTable
	ORDER BY
		Result_Seq



set nocount off
END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSA0027-15] TO HCVU
GO

