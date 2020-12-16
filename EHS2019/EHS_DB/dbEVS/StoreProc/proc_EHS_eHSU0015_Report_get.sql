		IF EXISTS
		(
			SELECT *
			FROM dbo.sysobjects
			WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0015_Report_get]')
				  AND OBJECTPROPERTY(id, N'IsProcedure') = 1
		)
			BEGIN
				DROP PROCEDURE [dbo].[proc_EHS_eHSU0015_Report_get];
			END;
		GO
		SET ANSI_NULLS ON;
		SET QUOTED_IDENTIFIER ON;
		GO
        -- =============================================  
        -- Modification History  
        -- Modified by:       
        -- Modified date:   
        -- CR No.:     
        -- Description:      
        -- =============================================  
        -- =============================================
        -- Author:			Raiman Chong
        -- Create date: 	05 Nov 2020
        -- CR. No			CRE20-015 (HA Scheme)
        -- Description:		Claim_Report_for_Guangdong_Province_HA_Patient
        -- =============================================
        -- exec [proc_EHS_eHSU0015_Report_get]
 CREATE PROCEDURE [dbo].[proc_EHS_eHSU0015_Report_get]  @request_time DATETIME = NULL,
														@From_Date DATETIME = NULL,
														@To_Date DATETIME = NULL 
  AS BEGIN 
		
		-- =============================================  
        -- Report setting  
        -- =============================================  
        DECLARE @In_From_Date DATETIME = @From_Date;

		DECLARE @In_To_Date DATETIME =  DATEADD(day, 1, @To_Date);

		DECLARE @ReportDtm DATETIME = GETDATE();

		-- =============================================  
		-- Declaration  
		-- =============================================  
		-- =============================================  
		-- Declaration For #WS02 Summary
		-- =============================================  
		IF OBJECT_ID('tempdb..#WS02') IS NOT NULL BEGIN DROP TABLE #WS02;
		END;

		CREATE TABLE #WS02
		(
			_display_seq int IDENTITY(1,1),
			_result_value1 VARCHAR(200) DEFAULT '',
			_result_value2 VARCHAR(100) DEFAULT '',
			_result_value3 VARCHAR(100) DEFAULT '',
			_result_value4 nVARCHAR(100) DEFAULT '',
			_result_value5 nVARCHAR(100) DEFAULT '',
			_result_value6 VARCHAR(100) DEFAULT '',
			_result_value7 nVARCHAR(100) DEFAULT '',
			_result_value8 nVARCHAR(100) DEFAULT '',
			_result_value9 nVARCHAR(100) DEFAULT '',
			_result_value10 nVARCHAR(100) DEFAULT '',
			_result_value11 nVARCHAR(100) DEFAULT '',
			_result_value12 nVARCHAR(100) DEFAULT '',
			_result_value13 nVARCHAR(100) DEFAULT '',
			_result_value14 nVARCHAR(100) DEFAULT '',
			_result_value15 nVARCHAR(100) DEFAULT '',
			_result_value16 nVARCHAR(100) DEFAULT '',
			_result_value17 nVARCHAR(100) DEFAULT '',
			_result_value18 nVARCHAR(100) DEFAULT '',
			_result_value19 nVARCHAR(100) DEFAULT '',
			_result_value20 nVARCHAR(100) DEFAULT '',
			_result_value21 nVARCHAR(100) DEFAULT '',
			_result_value22 nVARCHAR(100) DEFAULT '',
			_result_value23 nVARCHAR(100) DEFAULT '',
			_result_value24 nVARCHAR(100) DEFAULT '',
			_result_value25 nVARCHAR(100) DEFAULT '',
			_result_value26 nVARCHAR(100) DEFAULT '',
			_result_value27 nVARCHAR(100) DEFAULT '',
			_result_value28 nVARCHAR(100) DEFAULT '',
			_result_value29 nVARCHAR(100) DEFAULT '',
			_result_value30 nVARCHAR(100) DEFAULT '',
			_result_value31 VARCHAR(100) DEFAULT '',
			_result_value32 VARCHAR(100) DEFAULT ''
		);

    -- =============================================  
    -- Declaration For #WS02 Summary end
    -- =============================================  
    -- ================ Transaction ================

	 -- Number of Claim Transaction
    insert into
        #WS02  (_result_value1) values ('Transaction date period: ' + CONVERT(VARCHAR(10), @From_Date, 111) +' to '+ CONVERT(VARCHAR(10), @To_Date, 111)) ;

	-- Period of Claim Transaction
    insert into
        #WS02  (_result_value1) values ('No. of records : ' + convert( varchar(MAX), (select count(1) from VoucherTransaction vt	where 	vt.scheme_code ='SSSCMC' and vt.Transaction_Dtm>=@In_From_Date and vt.Transaction_Dtm < @In_To_Date))) ;
    insert into
        #WS02 (_result_value1) values ('')--next line

    insert into
        #WS02 (_result_value1, 
        _result_value2,
        _result_value3,
        _result_value4,
        _result_value5,
        _result_value6,
        _result_value7,
        _result_value8,
        _result_value9,
        _result_value10,
        _result_value11,
        _result_value12,
        _result_value13,
        _result_value14,
        _result_value15,
        _result_value16,
        _result_value17,
        _result_value18,
        _result_value19,
        _result_value20,
        _result_value21,
        _result_value22,
        _result_value23,
        _result_value24,
        _result_value25,
        _result_value26,
		_result_value27
    )
    values
        (
            'Transaction ID',
            'Service Date',
            'Practice No.',
            'Practice name',
			'Sub-specialities',
            'Doc. Code',
            'Doc. No.',
            N'全額減免病人',
            N'病人申请费用减免资料不符',
            N'病人自付费用 ¥',
            N'診金 ¥',
            N'藥費 ¥',
            N'檢驗費 ¥',
            N'其他費用 ¥',
            N'其他費用-注明',
            N'总服务费用 ¥ (至角)',
            N'该次诊症户口扣除的金额¥',
            N'计划支付的減免费用 ¥',
            N'计划承担的总额 ¥',
            N'额外支付 ¥',
            N'户口余额承前 ¥',
            N'户口余额 ¥',
            N'不需病人自付费用',
            N'收取费用日期',
            'Transaction date time',
            'Transaction status',
			'Reimbursement Status'
        ) --next line
        OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key
    insert into
        #WS02 (_result_value1, 
        _result_value2,
        _result_value3,
        _result_value4,
        _result_value5,
        _result_value6,
        _result_value7,
        _result_value8,
        _result_value9,
        _result_value10,
        _result_value11,
        _result_value12,
        _result_value13,
        _result_value14,
        _result_value15,
        _result_value16,
        _result_value17,
        _result_value18,
        _result_value19,
        _result_value20,
        _result_value21,
        _result_value22,
        _result_value23,
        _result_value24,
        _result_value25,
		_result_value26,
		_result_value27
    )
	select 
	
	dbo.func_format_system_number(vt.transaction_id) 'Transaction ID'
	--, vt.transaction_id
	, convert(varchar, vt.Service_Receive_Dtm, 106) 'Service Date'
	--, vt.Transaction_Dtm
	, vt.Practice_Display_Seq
	, p.Practice_Name_Chi 'Practice name'
	,subspecialitiesName.Name_CN 'Sub-specialities Name'
	,vt.doc_code 'Doc Code'
	, convert( varchar(MAX), DecryptByKey(isnull(tp.Encrypt_Field1, pp.Encrypt_Field1) )) [Doc. No.]
	-- ,CONVERT(varchar(MAX), DecryptByKey(E_Doc_No)) AS [E_Doc_No]
	, case vd.Subsidize_Code
		when 'HAS_A' then ''
		when 'HAS_B' then N'全額減免'
		else  'EXCEPTION'
		end   '全額減免病人'
	, case InvalidWaiver.AdditionalFieldValueCode
		when 'N' then N'减免资料不符'
		else  ''
		end   '病人申请费用减免资料不符'
	--,isnull(InvalidWaiver.AdditionalFieldValueCode,'')
	,vaSelfFee.AdditionalFieldValueCode '病人自付费用¥'
	,ConsultAndRegFeeRMB.AdditionalFieldValueCode '診金¥'
	,DrugFeeRMB.AdditionalFieldValueCode '藥費¥'
	,InvestigationFeeRMB.AdditionalFieldValueCode  '檢驗費¥'
	,OtherFeeRMB.AdditionalFieldValueCode  '其他費用¥'
	,OtherFeeRMB.AdditionalFieldValueDesc  '其他費用-注明'
	, vd.total_amount_rmb + vaSelfFee.AdditionalFieldValueCode  + vaCopayment.AdditionalFieldValueCode  '总服务费用¥'
	, vd.total_amount_rmb '该次诊症户口扣除的金额¥'
	, vaScheme.AdditionalFieldValueCode - vd.total_amount_rmb '计划支付的減免费用¥'
	, vaScheme.AdditionalFieldValueCode '计划承担的总额¥'
	, vaCopayment.AdditionalFieldValueCode '额外支付¥'
	,vaSubsidyB4.AdditionalFieldValueCode '户口余额承前¥'
	,vaSubsidyAfter.AdditionalFieldValueCode '户口余额¥'

	,case isnull(ExemptRegFee.AdditionalFieldValueCode,'N')
		when 'N'  then ''
		when 'Y' then 'Y'
		else 'EXCEPTION'
		end  '不需病人自付费用'
	,ExemptRegFee.AdditionalFieldValueDesc '收取费用日期'
	--, vt.Transaction_Dtm 
	, convert(varchar, vt.Transaction_Dtm, 20)
	, stat.Status_Description 'Transaction status'
	, stat2.Status_Description 'Reimbursement Status' 
    from
        VoucherTransaction vt with (nolock)
        inner join TransactionDetail vd with (nolock) on vt.Transaction_ID = vd.Transaction_ID
        inner join TransactionAdditionalField vaScheme with (nolock) on vaScheme.Transaction_ID = vt.Transaction_ID
			and vaScheme.AdditionalFieldID = 'TotalSupportFee'
        inner join TransactionAdditionalField vaCopayment with (nolock) on vaCopayment.Transaction_ID = vt.Transaction_ID
			and vaCopayment.AdditionalFieldID = 'CoPaymentFeeRMB'
        inner join TransactionAdditionalField vaSelfFee with (nolock) on vaSelfFee.Transaction_ID = vt.Transaction_ID
			and vaSelfFee.AdditionalFieldID = 'RegistrationFeeRMB'
        inner join TransactionAdditionalField vaSubsidyB4 with (nolock) on vaSubsidyB4.Transaction_ID = vt.Transaction_ID
			and vaSubsidyB4.AdditionalFieldID = 'SubsidyBeforeClaim'
        inner join TransactionAdditionalField vaSubsidyAfter with (nolock) on vaSubsidyAfter.Transaction_ID = vt.Transaction_ID
			and vaSubsidyAfter.AdditionalFieldID = 'SubsidyAfterClaim'
        inner join practice p with (nolock) on vt.sp_id = p.sp_id
			and vt.Practice_Display_Seq = p.Display_Seq
        inner join TransactionAdditionalField ConsultAndRegFeeRMB with (nolock) on ConsultAndRegFeeRMB.Transaction_ID = vt.Transaction_ID
			and ConsultAndRegFeeRMB.AdditionalFieldID = 'ConsultAndRegFeeRMB'
        inner join TransactionAdditionalField DrugFeeRMB with (nolock) on DrugFeeRMB.Transaction_ID = vt.Transaction_ID
			and DrugFeeRMB.AdditionalFieldID = 'DrugFeeRMB'
        inner join TransactionAdditionalField InvestigationFeeRMB with (nolock) on InvestigationFeeRMB.Transaction_ID = vt.Transaction_ID
			and InvestigationFeeRMB.AdditionalFieldID = 'InvestigationFeeRMB'
        inner join TransactionAdditionalField OtherFeeRMB with (nolock) on OtherFeeRMB.Transaction_ID = vt.Transaction_ID
			and OtherFeeRMB.AdditionalFieldID = 'OtherFeeRMB'
        left join TransactionAdditionalField InvalidWaiver with (nolock) on InvalidWaiver.Transaction_ID = vt.Transaction_ID
			and InvalidWaiver.AdditionalFieldID = 'PaymentTypeMatch'
			and InvalidWaiver.AdditionalFieldValueCode = 'N'
        left join TransactionAdditionalField ExemptRegFee with (nolock) on ExemptRegFee.Transaction_ID = vt.Transaction_ID
			and ExemptRegFee.AdditionalFieldID = 'ExemptRegFee'
        left join StatusData stat with (nolock) on vt.Record_Status = stat.Status_Value
			and enum_class = 'ClaimTransStatus'
        left join TempPersonalInformation tp with (nolock) on vt.temp_Voucher_Acc_ID = tp.Voucher_Acc_ID
			and vt.Doc_Code = tp.Doc_Code
			and vt.Voucher_Acc_ID = ''
        left join personalinformation pp with (nolock) on vt.Voucher_Acc_ID = pp.Voucher_Acc_ID
			and vt.Doc_Code = pp.Doc_Code
			and vt.voucher_acc_id <> ''
		left join TransactionAdditionalField subspecialitiesCode with (nolock) on subspecialitiesCode.Transaction_ID = vt.Transaction_ID
			and subspecialitiesCode.Scheme_Code = 'SSSCMC'
			and subspecialitiesCode.AdditionalFieldID = 'SubSpecialities'
		left join HAServiceSubSpecialitiesMapping subspecialitiesName with (nolock) on subspecialitiesCode.AdditionalFieldValueCode = subspecialitiesName.SubSpecialities_Code
		left join ReimbursementAuthTran RAT with (nolock) on  VT.Transaction_ID = RAT.Transaction_ID   
		left join StatusData stat2 with (nolock) on RAT.Authorised_Status = stat2.Status_Value
			and stat2.enum_class = 'ReimbursementStatus'
    where
        vt.scheme_code = 'SSSCMC' --and vt.record_status not in ('I','W','D')
        and vt.Transaction_Dtm >= @In_From_Date
        and vt.Transaction_Dtm < @In_To_Date
		order by vt.Transaction_id;
    --select * from TransactionAdditionalField where Transaction_ID= 'TB20B18000000084'
    CLOSE SYMMETRIC KEY sym_Key -- =============================================  
    --  Validation for #WS02 end
    -- ============================================= 
    -- =============================================  
    -- Initialization  
    -- =============================================  
    -- =============================================  
    -- Initialization for #WS02
    -- =============================================  
    -- =============================================  
    -- Initialization for #WS02 end
    -- =============================================  
    -- ============================================= 
    -- Return result 
    -- ============================================= 
    -- =============================================  
    -- Return results  
    -- =============================================  
    -- --------------------------------------------------    
    -- Content Page  WS01  Content
    -- --------------------------------------------------  
    DECLARE @ContentTable TABLE (
        Display_Seq INT IDENTITY(1, 1),
        Value1 VARCHAR(100),
        Value2 VARCHAR(100)
    );

    INSERT INTO
        @ContentTable(Value1)
    SELECT
        'Report Generation Time: ' + CONVERT(VARCHAR(10), @ReportDtm, 111) + ' ' + CONVERT(VARCHAR(5), @ReportDtm, 114);

    SELECT
        ISNULL(Value1, ''),
        ISNULL(Value2, '')
    FROM
        @ContentTable
    ORDER BY
        Display_Seq;

    -- --------------------------------------------------    
    -- Content Page  WS02  -Summary
    -- --------------------------------------------------  
    SELECT
        --ISNULL(_display_seq, ''), 
        ISNULL(_result_value1, ''),
        ISNULL(_result_value2, ''),
        ISNULL(_result_value3, ''),
        ISNULL(_result_value4, ''),
        ISNULL(_result_value5, ''),
        ISNULL(_result_value6, ''),
        ISNULL(_result_value7, ''),
        ISNULL(_result_value8, ''),
        ISNULL(_result_value9, ''),
        ISNULL(_result_value10, ''),
        ISNULL(_result_value11, ''),
        ISNULL(_result_value12, ''),
        ISNULL(_result_value13, ''),
        ISNULL(_result_value14, ''),
        ISNULL(_result_value15, ''),
        ISNULL(_result_value16, ''),
        ISNULL(_result_value17, ''),
        ISNULL(_result_value18, ''),
        ISNULL(_result_value19, ''),
        ISNULL(_result_value20, ''),
        ISNULL(_result_value21, ''),
        ISNULL(_result_value22, ''),
        ISNULL(_result_value23, ''),
        ISNULL(_result_value24, ''),
        ISNULL(_result_value25, ''),
        ISNULL(_result_value26, ''),
        ISNULL(_result_value27, ''),
        ISNULL(_result_value28, '')
    FROM
        #WS02 WITH(NOLOCK)
    ORDER BY
        _display_seq;

    END;

    GO
        GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0015_Report_get] TO HCVU;

    GO