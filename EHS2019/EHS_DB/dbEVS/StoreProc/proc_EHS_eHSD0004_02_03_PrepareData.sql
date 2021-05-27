IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSD0004_02_03_PrepareData]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_eHSD0004_02_03_PrepareData]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Modified by:		Raiman Chong
-- Modified date:	04 May 2021
-- CR. No.:			CRE20-023
-- Description:		Add Covid-19 Scheme Report
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	16 Jul 2020
-- CR. No			INT20-0025
-- Description:		(1) Add WITH (NOLOCK)
-- =============================================
-- =============================================
-- Modification History
-- Modified by		Chris YIM
-- Modified date	25 June 2019
-- CR No.			CRE19-004 (New initiatives for RVP in 2019-20)
-- Description		Include MMR
-- =============================================
-- =============================================
-- Modified by:		Winnie SUEN
-- Modified date:	28 Sep 2018
-- CR No.:			CRE17-018-07 (New initiatives for VSS AND RVP in 2018-19 - RVP Report)
-- Description:		Remove TIV from 2018/19 season
-- ============================================= 
-- =============================================
-- Modified by:		Marco CHOI	
-- Modified date:	11 Jan 2018
-- CR No.:			CRE16-004	
-- Description:		Add Deceased Status
-- =============================================  
-- =============================================      
-- Author:   		Marco CHOI
-- Create date:		3 August 2017
-- CR No.:			CRE16-026
-- Description:		Add PCV13   
--					SP rename from proc_EHS_RVPAgeReport_Stat  
-- =============================================    
CREATE PROCEDURE [dbo].[proc_EHS_eHSD0004_02_03_PrepareData]      
	@Cutoff_Dtm DATETIME      
AS BEGIN      
      
	SET NOCOUNT ON;    
       
	DECLARE @schemeDate DATETIME
	SET @schemeDate = DATEADD(dd, -1, @Cutoff_Dtm)

	DECLARE @current_scheme_Seq				INT
	DECLARE @Scheme_Start_Dtm				DATETIME
	DECLARE @PCV13_Start_Dtm				DATETIME
	DECLARE @MMR_Start_Dtm					DATETIME
	DECLARE @IV_Current_Season_Start_Dtm	DATETIME
	DECLARE @current_scheme_desc			VARCHAR(20)


	EXEC @current_scheme_Seq = [proc_EHS_GetSchemeSeq_Stat] 'RVP', @schemeDate
	--
	SELECT SG.Scheme_Code,
		SG.Scheme_Seq,
		SGD.Subsidize_Item_Code,
		SG.Claim_Period_From
	INTO #SubsidyDateTT
	FROM SubsidizeGroupClaim  SG WITH (NOLOCK)
	INNER JOIN SubsidizeGroupClaimItemDetails SGD
		ON SG.Scheme_Code = SGD.Scheme_Code
		AND SG.Scheme_Seq = SGD.Scheme_Seq
		AND SG.Subsidize_Code = SGD.Subsidize_Code
	WHERE SG.Scheme_Code = 'RVP' 
	--
	SELECT @Scheme_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV' 	
	SELECT @PCV13_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = '1' AND Subsidize_Item_Code='PV13'
	SELECT @MMR_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = '1' AND Subsidize_Item_Code='MMR'
	SELECT @IV_Current_Season_Start_Dtm = MIN(Claim_Period_From) FROM #SubsidyDateTT WHERE Scheme_Code = 'RVP' AND Scheme_Seq = @current_scheme_Seq
	SELECT @current_scheme_desc = Season_Desc FROM VaccineSeason WITH (NOLOCK) WHERE Scheme_Code = 'RVP' AND Scheme_Seq = @current_scheme_Seq AND Subsidize_Item_Code = 'SIV'
	
-- =============================================      
-- Temporary table      
-- =============================================      
	CREATE TABLE #RVPTransaction (      
		Transaction_ID   		char(20),      
		Voucher_Acc_ID   		char(15),      
		Temp_Voucher_Acc_ID  	char(15),      
		Special_Acc_ID   		char(15),      
		Invalid_Acc_ID   		char(15),      
		Doc_Code    			char(20),      
		Dose      				char(20),      
		Category_Code    		varchar(50),      
		Subsidize_Item_Code   	varchar(255),      
		Subsidize_Code   		varchar(255),      
		Service_Receive_Dtm  	datetime,      
		SP_ID     				char(8),  
		RCH_Type  				char(5)  
	)      
            
	CREATE TABLE #RVPAccount (      
		Transaction_ID   		char(20),
		Subsidize_Item_Code   	varchar(255),    
		Subsidize_Code   		varchar(255),      
		Service_Receive_Dtm  	datetime,      
		SP_ID     				char(8),      
		Doc_Code    			char(20),      
		Encrypt_Field1  		varbinary(100),      
		Age      				INT ,  
		DOB       				datetime,      
		DOB_Adjust 				datetime,      
		Exact_DOB  				char(1)     
	)      
  
	CREATE TABLE #SubsideCount (    
		Transaction_ID   			char(20),   
		RCH_Type  					char(5),   
		Doc_Code  					char(20),   
		Service_Receive_Dtm  		datetime, 
		Encrypt_Field1 				varbinary(100),      
		Age   						INT,    
		Dose  						char(20),     
		LessThan9  					tinyINT,  
		IsHealthCare 				tinyINT,  
		IsResident 					tinyINT,  
		IsINTellectualDisability 	tinyINT,  
		IsPV   						tinyINT,    
		IsPV13   					tinyINT,
		IsMMR  						tinyINT,  
		IsRQIV_RQIVHCW  			tinyINT,
		IsRQIVPID  					tinyINT,
		IsCurrentSeason 			tinyINT,
		IsCovid19					tinyINT,
		SP_ID						varchar(8)
	)    
          
	CREATE TABLE #ResultTable (      
		Result_Seq			smallINT,      
		Result_Value1	    varchar(110) default '',      
		Result_Value2	    varchar(100) default '',      
		Result_Value3	    varchar(100) default '',      
		Result_Value4	    varchar(100) default '',      
		Result_Value5	    varchar(100) default '',      
		Result_Value6	    varchar(100) default '',      
		Result_Value7	    varchar(100) default '',      
		Result_Value8	    varchar(100) default '',      
		Result_Value9	    varchar(100) default '',      
		Result_Value10	    varchar(100) default '',      
		Result_Value11	    varchar(100) default '',      
		Result_Value12	    varchar(100) default '',      
		Result_Value13	    varchar(100) default '',      
		Result_Value14	    varchar(100) default '',      
		Result_Value15	    varchar(100) default '',      
		Result_Value16	    varchar(100) default ''      
	)  

	CREATE TABLE #SubResult (      
		Result_Seq			smallINT,      
		Result_Value1	    varchar(110) default '',      
		Result_Value2	    varchar(100) default '',      
		Result_Value3	    varchar(100) default '',      
		Result_Value4	    varchar(100) default '',      
		Result_Value5	    varchar(100) default '',      
		Result_Value6	    varchar(100) default '',      
		Result_Value7	    varchar(100) default '',      
		Result_Value8	    varchar(100) default '',      
		Result_Value9	    varchar(100) default '',      
		Result_Value10	    varchar(100) default '',      
		Result_Value11	    varchar(100) default '',      
		Result_Value12	    varchar(100) default '',      
		Result_Value13	    varchar(100) default '',      
		Result_Value14	    varchar(100) default '',      
		Result_Value15	    varchar(100) default '',      
		Result_Value16	    varchar(100) default ''      
	)       
              
-- =============================================      
-- Retrieve data      
-- =============================================      
	
	-- RQIV transaction under RVP within current season       
	INSERT INTO #RVPTransaction (      
		Transaction_ID,      
		Voucher_Acc_ID,      
		Temp_Voucher_Acc_ID,      
		Special_Acc_ID,      
		Invalid_Acc_ID,      
		Doc_Code,      
		Dose,      
		Category_Code,   
		Subsidize_Item_Code,      
		Subsidize_Code,      
		Service_Receive_Dtm,      
		SP_ID   ,  
		RCH_Type  
	)      
	SELECT      
		VT.Transaction_ID,      
		ISNULL(VT.Voucher_Acc_ID, ''),      
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),      
		ISNULL(VT.Special_Acc_ID, ''),      
		ISNULL(VT.Invalid_Acc_ID, ''),      
		VT.Doc_Code,      
		D.Available_Item_Code AS [Dose],      
		VT.Category_Code,
		D.[Subsidize_Item_Code] AS [Subsidize_Item_Code],      
		D.[Subsidize_Code] AS [Subsidize_Code],     
		VT.Service_Receive_Dtm,      
		VT.SP_ID,  
		HL.Type AS [RCH_Type]  
	FROM      
		VoucherTransaction VT WITH (NOLOCK) 
			INNER JOIN TransactionDetail D WITH (NOLOCK) 
				ON VT.Transaction_ID = D.Transaction_ID 
			INNER JOIN TransactionAdditionalField TAF2 WITH (NOLOCK)        
				ON VT.Transaction_ID = TAF2.Transaction_ID        
			AND TAF2.AdditionalFieldID = 'RHCCode'        
			INNER JOIN RVPHomeList HL WITH (NOLOCK)        
				ON TAF2.AdditionalFieldValueCode = HL.RCH_Code   
	WHERE      
		VT.Scheme_Code = 'RVP'      
		AND VT.Transaction_Dtm < @Cutoff_Dtm      
		AND VT.Record_Status NOT IN  
			(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
			AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'   
		AND ((Effective_Date is null or Effective_Date <= @Cutoff_Dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date )))     
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In   
				(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
				AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
				AND ((Effective_Date is null or Effective_Date <= @Cutoff_Dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date ))))  
		AND D.subsidize_code in 
			(select subsidize_code from SubsidizeGroupClaimItemDetails WITH (NOLOCK) where   
				scheme_code = VT.Scheme_Code AND Scheme_seq = @current_scheme_Seq AND subsidize_item_code in ('SIV'))      
		AND D.Scheme_Seq = @current_scheme_Seq  

	
	-- 23vPPV, PCV13, MMR & C19 transaction under RVP   
	INSERT INTO #RVPTransaction (      
		Transaction_ID,      
		Voucher_Acc_ID,      
		Temp_Voucher_Acc_ID,      
		Special_Acc_ID,      
		Invalid_Acc_ID,      
		Doc_Code,      
		Dose,      
		Category_Code, 
		Subsidize_Item_Code,  
		Subsidize_Code,      
		Service_Receive_Dtm,      
		SP_ID   ,  
		RCH_Type  
	)       
	SELECT      
		VT.Transaction_ID,      
		ISNULL(VT.Voucher_Acc_ID, ''),      
		ISNULL(VT.Temp_Voucher_Acc_ID, ''),      
		ISNULL(VT.Special_Acc_ID, ''),      
		ISNULL(VT.Invalid_Acc_ID, ''),      
		VT.Doc_Code,   
		D.Available_Item_Code AS [Dose],      
		VT.Category_Code,
		D.[Subsidize_Item_Code] AS [Subsidize_Item_Code],      
		D.[Subsidize_Code] AS [Subsidize_Code],      
		VT.Service_Receive_Dtm,      
		VT.SP_ID,  
		HL.Type AS [RCH_Type]  
	FROM      
		VoucherTransaction VT WITH (NOLOCK) INNER JOIN TransactionDetail D WITH (NOLOCK) ON VT.Transaction_ID = D.Transaction_ID 
			INNER JOIN TransactionAdditionalField TAF2 WITH (NOLOCK)        
				ON VT.Transaction_ID = TAF2.Transaction_ID        
		AND TAF2.AdditionalFieldID = 'RHCCode'        
			INNER JOIN RVPHomeList HL WITH (NOLOCK)        
				ON TAF2.AdditionalFieldValueCode = HL.RCH_Code   
	WHERE      
		VT.Scheme_Code = 'RVP'      
		AND VT.Transaction_Dtm < @Cutoff_Dtm      
		AND VT.Record_Status NOT IN  
			(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
		AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Record_Status'   
			AND ((Effective_Date is null or Effective_Date <= @Cutoff_Dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date)))     
		AND (VT.Invalidation IS NULL OR VT.Invalidation NOT In   
			(SELECT Status_Value FROM StatStatusFilterMapping WITH (NOLOCK) WHERE (report_id = 'ALL' OR report_id = 'eHSD0004')   
		AND Table_Name = 'VoucherTransaction' AND Status_Name = 'Invalidation'  
			AND ((Effective_Date is null or Effective_Date <= @Cutoff_Dtm) AND (Expiry_Date is null or @cutoff_dtm < Expiry_Date))))  
		AND D.subsidize_code in 
			(select subsidize_code from SubsidizeGroupClaimItemDetails WITH (NOLOCK) where   
				scheme_code = VT.Scheme_Code AND (subsidize_item_code = 'PV' or subsidize_item_code = 'PV13' or subsidize_item_code = 'MMR' or subsidize_item_code = 'C19'))  
      
	  update #RVPTransaction set Category_Code = case when taf2.AdditionalFieldValueCode = 'RESIDENT' then 'RESIDENT' else 'HCW' end
	  from #RVPTransaction rvp left outer join TransactionAdditionalField TAF2 on rvp.Transaction_ID = taf2.Transaction_ID and  taf2.AdditionalFieldID = 'RecipientType'
	  where rvp.subsidize_item_code = 'C19' 

	-- Validated account with RVP       
	INSERT INTO #RVPAccount (      
		Transaction_ID,  
		Subsidize_Item_Code,  
		Subsidize_Code,      
		Service_Receive_Dtm,      
		SP_ID,      
		Doc_Code,      
		Encrypt_Field1,      
		Age,  
		DOB,   
		DOB_Adjust,      
		Exact_DOB      
	)      
	SELECT      
		VT.Transaction_ID,  
		VT.Subsidize_Item_Code,  
		VT.Subsidize_Code,      
		VT.Service_Receive_Dtm,      
		VT.SP_ID,      
		VP.Doc_Code,      
		VP.Encrypt_Field1,      
		0 AS [Age],  
		VP.DOB,  
		NULL AS [DOB_Adjust],      
		VP.Exact_DOB      
	FROM      
		#RVPTransaction VT      
			INNER JOIN PersonalInformation VP WITH (NOLOCK)      
				ON VT.Voucher_Acc_ID = VP.Voucher_Acc_ID      
		AND VT.Doc_Code = VP.Doc_Code      
	WHERE      
		VT.Voucher_Acc_ID <> ''      
      
	-- Temporary account with RVP         
	INSERT INTO #RVPAccount (      
		Transaction_ID,  
		Subsidize_Item_Code,      
		Subsidize_Code,      
		Service_Receive_Dtm,      
		SP_ID,      
		Doc_Code,      
		Encrypt_Field1,      
		Age,  
		DOB,   
		DOB_Adjust,      
		Exact_DOB      
	)      
	SELECT      
		VT.Transaction_ID,  
		VT.Subsidize_Item_Code,      
		VT.Subsidize_Code,      
		VT.Service_Receive_Dtm,      
		VT.SP_ID,      
		TP.Doc_Code,      
		TP.Encrypt_Field1,      
		0 AS [Age],  
		TP.DOB,   
		NULL AS [DOB_Adjust],      
		TP.Exact_DOB      
	FROM      
		#RVPTransaction VT      
			INNER JOIN TempPersonalInformation TP WITH (NOLOCK)      
				ON VT.Temp_Voucher_Acc_ID = TP.Voucher_Acc_ID      
	WHERE      
		VT.Voucher_Acc_ID = ''      
		AND VT.Temp_Voucher_Acc_ID <> ''      
		AND VT.Special_Acc_ID = ''      
      
	-- Special account with RVP          
	INSERT INTO #RVPAccount (   
		Transaction_ID,
		Subsidize_Item_Code,      
		Subsidize_Code,      
		Service_Receive_Dtm,      
		SP_ID,      
		Doc_Code,      
		Encrypt_Field1,      
		Age,  
		DOB,    
		DOB_Adjust,      
		Exact_DOB        
	)      
	SELECT       
		VT.Transaction_ID,
		VT.Subsidize_Item_Code,   
		VT.Subsidize_Code,      
		VT.Service_Receive_Dtm,      
		VT.SP_ID,      
		SP.Doc_Code,      
		SP.Encrypt_Field1,      
		0 AS [Age],  
		SP.DOB,    
		NULL AS [DOB_Adjust],      
		SP.Exact_DOB     
	FROM      
		#RVPTransaction VT      
			INNER JOIN SpecialPersonalInformation SP WITH (NOLOCK)      
				ON VT.Special_Acc_ID = SP.Special_Acc_ID      
	WHERE      
		VT.Voucher_Acc_ID = ''      
		AND VT.Special_Acc_ID <> ''      
		AND VT.Invalid_Acc_ID = ''      
      
	-- ---------------------------------------------      
	-- Patch DOB      
	-- ---------------------------------------------      
      
	UPDATE      
		#RVPAccount      
	SET      
		DOB = CONVERT(varchar, YEAR(DOB)) + '-' + CONVERT(varchar, MONTH(DOB)) + '-' + CONVERT(varchar, DAY(DATEADD(d, -DAY(DATEADD(m, 1, DOB)), DATEADD(m, 1, DOB))))      
	WHERE      
		Exact_DOB IN ('M', 'U')      
      
	UPDATE      
		#RVPAccount      
	SET      
		DOB_Adjust = DOB      
      
	UPDATE      
		#RVPAccount      
	SET      
		DOB_Adjust = DATEADD(yyyy, 1, DOB)      
	WHERE      
		MONTH(DOB) > MONTH(Service_Receive_Dtm)      
		OR ( MONTH(DOB) = MONTH(Service_Receive_Dtm) AND DAY(DOB) > DAY(Service_Receive_Dtm) )      
  
	UPDATE      
		#RVPAccount      
	SET      
		Age = DATEDIFF(yy, DOB_Adjust, Service_Receive_Dtm)  
  
	-- ---------------------------------------------      
	-- Patch Doc_Code      
	-- ---------------------------------------------      
      
	UPDATE      
		#RVPAccount      
	SET      
		Doc_Code = 'HKIC'      
	WHERE      
		Doc_Code = 'HKBC'      

-- =============================================      
-- Process data      
-- =============================================          
	INSERT INTO #SubsideCount  (
			Transaction_ID
			,RCH_Type  
			,Doc_Code  
			,Service_Receive_Dtm
			,Encrypt_Field1     
			,Age   
			,Dose   
			,LessThan9  
			,IsHealthCare  
			,IsResident 
			,IsINTellectualDisability
			,IsPV
			,IsPV13   
			,IsMMR
			,IsRQIV_RQIVHCW 
			,IsRQIVPID  
	 		,IsCurrentSeason
			,IsCovid19
	 		,SP_ID)
	SELECT DISTINCT 
		T.Transaction_ID, 
		RCH_TYPE, 
		a.DOC_Code, 
		a.Service_Receive_Dtm,
		a.Encrypt_Field1, 
		a.Age, 
		Dose,  
		CASE   
			WHEN Age >= 9 THEN 0   
			WHEN DATEDIFF(dd, A.DOB, A.Service_Receive_Dtm) >= 182 THEN 1   
		END IsLessThan9,   
		CASE [Category_Code]  
			WHEN 'HCW' THEN 1 ELSE 0 END IsHealthCare,  
		CASE [Category_Code]  
			WHEN 'RESIDENT' THEN 1 ELSE 0 END IsResident,  
		CASE [Category_Code]  
			WHEN 'PID' THEN 1 ELSE 0 END IsINTellectualDisability,  
		CASE 
			WHEN t.Subsidize_Item_Code = 'PV' THEN 1 ELSE 0 END PVCount,
		CASE 
			WHEN t.Subsidize_Item_Code = 'PV13' THEN 1 ELSE 0 END PV13Count,
		CASE 
			WHEN t.Subsidize_Item_Code = 'MMR' THEN 1 ELSE 0 END MMRCount,
		CASE 
			WHEN t.Subsidize_Code like '%RQIV%' THEN 1 
			WHEN t.Subsidize_Code like '%RWQIV%' THEN 1 
			ELSE 0 
		END RQIV_RQIVHCWCount,
		CASE 
			WHEN t.Subsidize_Code like '%RDQIV%' THEN 1 ELSE 0 END RQIVPIDCount,
		CASE 
			WHEN T.Service_Receive_Dtm >= @IV_Current_Season_Start_Dtm THEN 1 ELSE 0 END CurrentSeason,
		CASE 
			WHEN t.Subsidize_Item_Code = 'C19' THEN 1 ELSE 0 END Covid19,
		A.SP_ID
	FROM #RVPTransaction T 
		INNER JOIN #RVPAccount A 
			ON t.transaction_id = a.transaction_id  

      
-- =============================================      
-- Generate Result    
-- =============================================        
-- Insert record for the final output format     

-- ============================================= 
-- Sub-Report 02
-- =============================================      
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES (1, REPLACE('eHS(S)D0004-02: Report on yearly RVP transaction ([YEAR])', '[YEAR]', @current_scheme_desc))  
      
	INSERT INTO #ResultTable (Result_Seq)  
	VALUES (2)  
      
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES (3, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111))  
  
	INSERT INTO #ResultTable (Result_Seq)  
	VALUES (4)  

	-- ===============================================================================================================   
	-- (i) By vaccination (20XX/XX) 
	-- ===============================================================================================================  
  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES (5, REPLACE('(i) By vaccination ([DATE])', '[DATE]', @current_scheme_desc) ) 
  
	INSERT INTO #ResultTable (Result_Seq)  
	VALUES (6)   
  
	Declare @displayCodeRQIV_RQIVHCW nvarchar(255)
	SET @displayCodeRQIV_RQIVHCW = ( select Display_Code_For_Claim + ' + ' from 
							(select Display_Code_For_Claim
								from subsidizegroupclaim sgc WITH (NOLOCK)
									inner join subsidize s WITH (NOLOCK)
										on sgc.subsidize_code = s.subsidize_code
								where 
									sgc.scheme_code = 'RVP' 
									AND sgc.Scheme_Seq = @current_scheme_Seq 
									AND s.subsidize_code in ('RQIV','RWQIV')
									AND s.subsidize_Item_Code = 'SIV') tblDisplayCode
						for xml path('') )

	Declare @displayCodeRQIVPID nvarchar(255)
	SET @displayCodeRQIVPID = ( select Display_Code_For_Claim + ' + ' from 
							(select Display_Code_For_Claim
								from subsidizegroupclaim sgc WITH (NOLOCK)
									inner join subsidize s WITH (NOLOCK)
										on sgc.subsidize_code = s.subsidize_code
								where 
									sgc.scheme_code = 'RVP' 
									AND sgc.Scheme_Seq = @current_scheme_Seq 
									AND s.subsidize_code = 'RDQIV'
									AND s.subsidize_Item_Code = 'SIV') tblDisplayCode
						for xml path('') )

	SET @displayCodeRQIV_RQIVHCW = SUBSTRING(@displayCodeRQIV_RQIVHCW, 1, LEN(@displayCodeRQIV_RQIVHCW)-2)
	SET @displayCodeRQIVPID = SUBSTRING(@displayCodeRQIVPID, 1, LEN(@displayCodeRQIVPID)-2)

	INSERT INTO #ResultTable (Result_Seq, Result_Value1,Result_Value2,Result_Value3,Result_Value4,Result_Value5,Result_Value6,Result_Value7,Result_Value8,Result_Value9)  
	VALUES	(7, '', '23vPPV', 'PCV13','MMR', @displayCodeRQIV_RQIVHCW, @displayCodeRQIVPID,'COVID-19', '', 'No. of SP involved' ) 
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(8,	'RCHD'),  
			(9, 'RCHE'),  
			(10, 'RCCC'), 
			(11, 'IPID'), 
			(12, 'Total'), 
			(13, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsPV = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value3 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value4 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value5 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value6 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE) ,
		Result_Value7 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE) 
	WHERE Result_Seq = 8  

	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsPV = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value5 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value6 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value7 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 9  

	--UPDATE RCCC 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsPV = 1 GROUP BY DOC_Code) as RCCC), 
		Result_Value3 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsPV13 = 1 GROUP BY DOC_Code) as RCCC),
		Result_Value4 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC), 
		Result_Value5 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),
		Result_Value6 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC) ,
		Result_Value7 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC) 
	WHERE Result_Seq = 10  

	--UPDATE IPID 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsPV = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value3 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value4 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1
						AND RCH_Type = 'I' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value5 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),
		Result_Value6 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE) ,
		Result_Value7 = (SELECT ISNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 11  

	--UPDATE Horizontal Total   
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11), 
		Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11),
		Result_Value7 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value7)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 8 AND 11),
		Result_Value9 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1) --No. of SP involved   
	WHERE Result_Seq = 12 
    
	-- ===============================================================================================================   
	-- (ii) (a) By age group (RQIV 20XX/XX + RQIV-HCW 20XX/XX)
	-- ===============================================================================================================   
	INSERT INTO #ResultTable (Result_Seq, Result_Value1) 
	VALUES	(20, '(ii) (a) By age group (' + @displayCodeRQIV_RQIVHCW + ')'),
			(21, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6)  
	VALUES	(22, '', '6 months to <9 years','9 years to <65 years','at 65 years','>65 years','Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(23, 'RCHD'),
			(24, 'RCHE'),
			(25, 'RCCC'),
			(26, 'IPID'),
			(27, 'Total'),  
			(28, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND LessThan9 = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age >= 9 AND age  < 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age = 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age > 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 23  
   
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND LessThan9 = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age >= 9 AND age < 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age = 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age > 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 24  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND LessThan9 = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age >= 9 AND age < 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age = 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age > 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 25 
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND LessThan9 = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age >= 9 AND age < 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age = 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age > 65 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 26 
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 23) 
	WHERE Result_Seq = 23  
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 24) 
	WHERE Result_Seq = 24  
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 25) 
	WHERE Result_Seq = 25  
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 26) 
	WHERE Result_Seq = 26  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 23 AND 26), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 23 AND 26),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 23 AND 26),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 23 AND 26),
		Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 23 AND 26) 
	WHERE Result_Seq = 27 

	-- ===============================================================================================================   
	-- (ii) (b) By age group (RQIV-PID 20XX/XX)
	-- =============================================================================================================== 
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(35, '(ii) (b) By age group (' + @displayCodeRQIVPID + ')'),
			(36, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6)  
	VALUES	(37, '', '6 months to <9 years','9 years to <65 years','at 65 years','>65 years','Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(38, 'RCHD'),  
			(39, 'RCHE'),  
			(40, 'RCCC'),  
			(41, 'IPID'),  
			(42, 'Total'),  
			(43, '')   

	--UPDATE RCHD 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND LessThan9 = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age >= 9 AND age  < 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age = 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD), 
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND age > 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 38  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND LessThan9 = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age >= 9 AND age < 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age = 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND age > 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 39  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND LessThan9 = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age >= 9 AND age < 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age = 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND age > 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 40 
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND LessThan9 = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age >= 9 AND age < 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age = 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT isNULL(isNULL(COUNT (Encrypt_Field1),0),0) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND age > 65 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 41 
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable	WHERE Result_Seq = 38) 
	WHERE Result_Seq = 38  
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 39) 
	WHERE Result_Seq = 39  	
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 40) 
	WHERE Result_Seq = 40  	
	UPDATE #ResultTable 
	SET Result_Value6 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value5) FROM #ResultTable WHERE Result_Seq = 41) 
	WHERE Result_Seq = 41  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 38 AND 41), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 38 AND 41),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 38 AND 41),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 38 AND 41),
		Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 38 AND 41) 
	WHERE Result_Seq = 42 

	-- ===============================================================================================================   
	-- (iii) (a) By dose (RQIV 20XX/XX + RQIV-HCW 20XX/XX) 
	-- ===============================================================================================================  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(50, '(iii) (a) By dose (' + @displayCodeRQIV_RQIVHCW + ')'),
			(51, '')
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES  (52, '', '1st Dose','2nd Dose','Only Dose', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES  (53, 'RCHD'),
			(54, 'RCHE'),  
			(55, 'RCCC'),
			(56, 'IPID'),
			(57, 'Total'),
			(58, '')   
	--===============================================================================================================  
	--UPDATE RCHD 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = '1STDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = '2NDDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = 'ONLYDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 53  
   
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '1STDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '2NDDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = 'ONLYDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 54  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '1STDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '2NDDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = 'ONLYDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 55  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '1STDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '2NDDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = 'ONLYDOSE' AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 56  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 53) 
	WHERE Result_Seq = 53    
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 54) 
	WHERE Result_Seq = 54  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 55) 
	WHERE Result_Seq = 55 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 56) 
	WHERE Result_Seq = 56 

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 53 AND 56), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 53 AND 56) 
	WHERE Result_Seq = 57 
 
	--===============================================================================================================   
	--(iii) (b) By dose (RQIV-PID 2015/16)
	--===============================================================================================================  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(65, '(iii) (b) By dose (' + @displayCodeRQIVPID + ')'),
			(66, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES	(67, '', '1st Dose','2nd Dose','Only Dose', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(68, 'RCHD'),
			(69, 'RCHE'),
			(70, 'RCCC'),
			(71, 'IPID'),
			(72, 'Total'),
			(73, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1
						AND RCH_Type = 'D' AND DOSE = '1STDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = '2NDDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = 'ONLYDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 68  
   
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '1STDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '2NDDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = 'ONLYDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 69  
  
	--UPDATE RCCC 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '1STDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '2NDDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = 'ONLYDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 70  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '1STDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '2NDDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = 'ONLYDOSE' AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 71  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 68) 
	WHERE Result_Seq = 68  
	UPDATE #ResultTable 
	SET	Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 69) 
	WHERE Result_Seq = 69  
	UPDATE #ResultTable 
	SET	Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 70) 
	WHERE Result_Seq = 70 
	UPDATE #ResultTable 
	SET	Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 71) 
	WHERE Result_Seq = 71 

	--UPDATE Horizontal Total 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 68 AND 71), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 68 AND 71),  
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 68 AND 71), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 68 AND 71) 
	WHERE Result_Seq = 72


	-- ===============================================================================================================   
	-- (iv) (a) By category (RQIV 20XX/XX + RQIV-HCW 20XX/XX)
	-- ===============================================================================================================  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(80, '(iv) (a) By category (' + @displayCodeRQIV_RQIVHCW + ')'),
			(81, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES	(82, '', 'Health Care Worker', 'Resident', 'Persons with INTellectual Disability (or related)', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(83, 'RCHD'),
			(84, 'RCHE'),
			(85, 'RCCC'),  	 
			(86, 'IPID'),  
			(87, 'Total'),  
			(88, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsHealthCare = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsResident = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsINTellectualDisability = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHD) 
	WHERE Result_Seq = 83  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET  
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsHealthCare = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsResident = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsINTellectualDisability = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCHE) 
	WHERE Result_Seq = 84  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsHealthCare = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsResident = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsINTellectualDisability = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as RCCC) 
	WHERE Result_Seq = 85  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsHealthCare = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsResident = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsINTellectualDisability = 1 AND IsRQIV_RQIVHCW = 1 GROUP BY DOC_Code) as IPID) 
	WHERE Result_Seq = 86  
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 83) 
	WHERE Result_Seq = 83 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 84) 
	WHERE Result_Seq = 84  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 85) 
	WHERE Result_Seq = 85  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 86) 
	WHERE Result_Seq = 86  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 83 AND 86), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 83 AND 86), 
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 83 AND 86), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 83 AND 86) 
	WHERE Result_Seq = 87 
     
	--===============================================================================================================   
	--(iv) (b) By category (RQIV-PID 20XX/XX)
	--=============================================================================================================== 
	INSERT INTO #ResultTable (Result_Seq, Result_Value1) 
	VALUES	(95, '(iv) (b) By category (' + @displayCodeRQIVPID + ')'),
			(96, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES	(97, '', 'Health Care Worker', 'Resident', 'Persons with INTellectual Disability (or related)', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(98, 'RCHD'),
			(99, 'RCHE'),  
			(100, 'RCCC'),
			(101, 'IPID'),
			(102, 'Total'),
			(103, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsHealthCare = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsResident = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsINTellectualDisability = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHD) 
	WHERE Result_Seq = 98  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsHealthCare = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsResident = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsINTellectualDisability = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCHE) 
	WHERE Result_Seq = 99  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsHealthCare = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsResident = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsINTellectualDisability = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as RCCC) 
	WHERE Result_Seq = 100  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsHealthCare = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsResident = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsINTellectualDisability = 1 AND IsRQIVPID = 1 GROUP BY DOC_Code) as IPID) 
	WHERE Result_Seq = 101  
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 98) 
	WHERE Result_Seq = 98  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 99) 
	WHERE Result_Seq = 99  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 100) 
	WHERE Result_Seq = 100  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 101) 
	WHERE Result_Seq = 101  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 98 AND 101), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 98 AND 101), 
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 98 AND 101),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 98 AND 101) 
	WHERE Result_Seq = 102 

	-- ===============================================================================================================   
	-- (v)(a) By age group (MMR)
	-- ===============================================================================================================   
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
		VALUES	(110, '(v) (a) By age group (MMR)'),
				(111, '')  
	--INSERT INTO #ResultTable (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11,Result_Value12)  
	--	VALUES	(129, '', '<36 years', '', '36 - 45 years', '', '46-52 years', '', '>52 years', '', 'Total', '', 'No. of SP involved')    
	--INSERT INTO #ResultTable (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11)  
	--	VALUES	(130, '', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
		VALUES	(112, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
		VALUES	(113, 'RCHD'),  
				(114, 'RCHE'),        
				(115, 'RCCC'),    
				(116, 'IPID'), 
				(117, 'Total'),
				(118, '')   

	----UPDATE RCHD  
	--UPDATE #ResultTable 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'D' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD) 
	--WHERE Result_Seq = 131  
  
	----UPDATE RCHE  
	--UPDATE #ResultTable 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'E' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE) 
	--WHERE Result_Seq = 132  
  
	----UPDATE RCCC  
	--UPDATE #ResultTable 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'C' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC) 
	--WHERE Result_Seq = 133  
  
	----UPDATE IPID  
	--UPDATE #ResultTable 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
	--					AND RCH_Type = 'I' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID) 
	--WHERE Result_Seq = 134  
  
	----UPDATE Vertical Total
	--UPDATE #ResultTable 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8) 
	--						FROM #ResultTable WHERE Result_Seq = 131),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #ResultTable WHERE Result_Seq = 131) 
	--WHERE Result_Seq = 131  
	--UPDATE #ResultTable 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #ResultTable WHERE Result_Seq = 132),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #ResultTable WHERE Result_Seq = 132) 
	--WHERE Result_Seq = 132  
	--UPDATE #ResultTable 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #ResultTable WHERE Result_Seq = 133),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #ResultTable WHERE Result_Seq = 133) 
	--WHERE Result_Seq = 133  
	--UPDATE #ResultTable 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #ResultTable WHERE Result_Seq = 134),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #ResultTable WHERE Result_Seq = 134) 
	--WHERE Result_Seq = 134 

	----UPDATE Horizontal Total
	--UPDATE #ResultTable 
	--SET 
	--	Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134), 
	--	Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134), 
	--	Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134),
	--	Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134), 
	--	Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134), 
	--	Result_Value7 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value7)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134), 
	--	Result_Value8 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value8)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134),
	--	Result_Value9 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value9)) ,0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134),	
	--	Result_Value10= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value10)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134),
	--	Result_Value11= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value11)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 131 AND 134),
	--	Result_Value12 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1 AND IsMMR = 1) 	
	--WHERE Result_Seq = 135

		--UPDATE RCHD 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age < 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age = 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age > 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 113  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age < 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age = 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age > 65 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 114
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age < 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age = 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age > 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 115
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age < 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age = 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age > 65 AND IsMMR = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 116

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 113) 
	WHERE Result_Seq = 113  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 114) 
	WHERE Result_Seq = 114   
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 115) 
	WHERE Result_Seq = 115 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 116) 
	WHERE Result_Seq = 116  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 113 AND 116),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 113 AND 116),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 113 AND 116), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 113 AND 116),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1 AND IsMMR = 1) 
	WHERE Result_Seq = 117

	--===============================================================================================================   
	--(v) (b) By dose (MMR)
	--===============================================================================================================  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(125, '(v) (b) By dose (MMR)'),
			(126, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)  
	VALUES	(127, '', '1st Dose', '2nd Dose', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(128, 'RCHD'),
			(129, 'RCHE'),
			(130, 'RCCC'),
			(131, 'IPID'),
			(132, 'Total'),
			(133, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1
						AND RCH_Type = 'D' AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 128  
   
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCHE)  
	WHERE Result_Seq = 129  
  
	--UPDATE RCCC 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as RCCC)  
	WHERE Result_Seq = 130  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '1STDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '2NDDOSE' AND IsMMR = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 131  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 128) 
	WHERE Result_Seq = 128  
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 129) 
	WHERE Result_Seq = 129  
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 130) 
	WHERE Result_Seq = 130 
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 131) 
	WHERE Result_Seq = 131 

	--UPDATE Horizontal Total 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 128 AND 131), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 128 AND 131),  
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 128 AND 131)
	WHERE Result_Seq = 132
     
	--===============================================================================================================   
	--(v) (c) By category (MMR)
	--=============================================================================================================== 
	INSERT INTO #ResultTable (Result_Seq, Result_Value1) 
	VALUES	(140, '(v) (c) By category (MMR)'),
			(141, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES	(142, '', 'Health Care Worker', 'Resident', 'Persons with INTellectual Disability (or related)', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(143, 'RCHD'),
			(144, 'RCHE'),  
			(145, 'RCCC'),
			(146, 'IPID'),
			(147, 'Total'),
			(148, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsHealthCare = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsResident = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsINTellectualDisability = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHD) 
	WHERE Result_Seq = 143  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsHealthCare = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsResident = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsINTellectualDisability = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCHE) 
	WHERE Result_Seq = 144  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsHealthCare = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsResident = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsINTellectualDisability = 1 AND IsMMR = 1 GROUP BY DOC_Code) as RCCC) 
	WHERE Result_Seq = 145  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsHealthCare = 1 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsResident = 1 AND IsMMR = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsINTellectualDisability = 1 AND IsMMR = 1 GROUP BY DOC_Code) as IPID) 
	WHERE Result_Seq = 146  
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 143) 
	WHERE Result_Seq = 143  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 144) 
	WHERE Result_Seq = 144  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 145) 
	WHERE Result_Seq = 145  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 146) 
	WHERE Result_Seq = 146  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 143 AND 146), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 143 AND 146), 
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 143 AND 146),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 143 AND 146) 
	WHERE Result_Seq = 147 

	-- ===============================================================================================================   
	-- (vi) By age group (23vPPV)  
	-- ===============================================================================================================
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(155, '(vi) By age group (23vPPV)'),
			(156, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(157, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(158, 'RCHD'),
			(159, 'RCHE'),
			(160, 'RCCC'),
			(161, 'IPID'),
			(162, 'Total'),
			(163, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age < 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age = 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age > 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 158  

	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age < 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age = 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age > 65 AND IsPV = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 159  

	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age < 65 AND IsPV = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age = 65 AND IsPV = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age > 65 AND IsPV = 1 GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 160  

	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
			Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
							AND RCH_Type = 'I' AND Age < 65 AND IsPV = 1 GROUP BY DOC_Code) as IPID),  
			Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
							AND RCH_Type = 'I' AND Age = 65 AND IsPV = 1 GROUP BY DOC_Code) as IPID),  
			Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
							AND RCH_Type = 'I' AND Age > 65 AND IsPV = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 161  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 158) 
	WHERE Result_Seq = 158  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 159) 
	WHERE Result_Seq = 159 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 160) 
	WHERE Result_Seq = 160   
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 161) 
	WHERE Result_Seq = 161  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 158 AND 161),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 158 AND 161),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 158 AND 161),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 158 AND 161),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1 AND IsPV = 1) 
	WHERE Result_Seq = 162 
   
	-- ===============================================================================================================   
	-- (vii) By age group (PCV13)  
	-- ===============================================================================================================     
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(170, '(vii) By age group (PCV13)'),
			(171, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(172, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(173, 'RCHD'),
			(174, 'RCHE'),
			(175, 'RCCC'),
			(176, 'IPID'),  
			(177, 'Total'),
			(178, '')   

	--UPDATE RCHD 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age < 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age = 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age > 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 173  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age < 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age = 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age > 65 AND IsPV13 = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 174 
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age < 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age = 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age > 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 175  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age < 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age = 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age > 65 AND IsPV13 = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 176  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 173) 
	WHERE Result_Seq = 173  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 174) 
	WHERE Result_Seq = 174   
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 175) 
	WHERE Result_Seq = 175 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 176) 
	WHERE Result_Seq = 176  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 173 AND 176),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 173 AND 176),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 173 AND 176), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 173 AND 176),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1 AND IsPV13 = 1) 
	WHERE Result_Seq = 177

	-- ===============================================================================================================   
	-- (viii) (a) By age group (COVID-19)  
	-- ===============================================================================================================     
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(185, '(viii) (a) By age group (COVID-19)'),
			(186, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(187, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(188, 'RCHD'),
			(189, 'RCHE'),
			(190, 'RCCC'),
			(191, 'IPID'),  
			(192, 'Total'),
			(193, '')   

	--UPDATE RCHD 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age < 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age = 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND Age > 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 188  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age < 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age = 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND Age > 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 189 
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age < 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age = 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND Age > 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 190  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age < 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age = 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND Age > 65 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 191  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 188) 
	WHERE Result_Seq = 188  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 189) 
	WHERE Result_Seq = 189   
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 190) 
	WHERE Result_Seq = 190 
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 191) 
	WHERE Result_Seq = 191  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 188 AND 191),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 188 AND 191),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 188 AND 191), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 188 AND 191),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsCurrentSeason = 1 AND IsCovid19 = 1) 
	WHERE Result_Seq = 192

	--===============================================================================================================   
	--(viii) (b) By dose (COVID-19)
	--===============================================================================================================  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(200, '(viii) (b) By dose (COVID-19)'),
			(201, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4)  
	VALUES	(202, '', '1st Dose', '2nd Dose', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(203, 'RCHD'),
			(204, 'RCHE'),
			(205, 'RCCC'),
			(206, 'IPID'),
			(207, 'Total'),
			(208, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1
						AND RCH_Type = 'D' AND DOSE = '1STDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND DOSE = '2NDDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 203  
   
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '1STDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND DOSE = '2NDDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE)  
	WHERE Result_Seq = 204  
  
	--UPDATE RCCC 
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '1STDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND DOSE = '2NDDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC)  
	WHERE Result_Seq = 205  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '1STDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND DOSE = '2NDDOSE' AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 206  

	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 203) 
	WHERE Result_Seq = 203  
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 204) 
	WHERE Result_Seq = 204  
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 205) 
	WHERE Result_Seq = 205 
	UPDATE #ResultTable 
	SET	Result_Value4 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) FROM #ResultTable WHERE Result_Seq = 206) 
	WHERE Result_Seq = 206 

	--UPDATE Horizontal Total 
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 203 AND 206), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 203 AND 206),  
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 203 AND 206)
	WHERE Result_Seq = 207

	--===============================================================================================================   
	--(viii) (c) By category (COVID-19)
	--=============================================================================================================== 
	INSERT INTO #ResultTable (Result_Seq, Result_Value1) 
	VALUES	(215, '(viii) (c) By category (COVID-19)'),
			(216, '')  
	INSERT INTO #ResultTable (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5)  
	VALUES	(217, '', 'Health Care Worker', 'Resident', 'Persons with INTellectual Disability (or related)', 'Total')    
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)        
	VALUES	(218, 'RCHD'),
			(219, 'RCHE'),  
			(220, 'RCCC'),
			(221, 'IPID'),
			(222, 'Total'),
			(223, '')   

	--UPDATE RCHD  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsHealthCare = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsResident = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'D' AND IsINTellectualDisability = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHD) 
	WHERE Result_Seq = 218  
  
	--UPDATE RCHE  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsHealthCare = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsResident = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'E' AND IsINTellectualDisability = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCHE) 
	WHERE Result_Seq = 219  
  
	--UPDATE RCCC  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsHealthCare = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsResident = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'C' AND IsINTellectualDisability = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as RCCC) 
	WHERE Result_Seq = 220  
  
	--UPDATE IPID  
	UPDATE #ResultTable 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsHealthCare = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsResident = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE IsCurrentSeason = 1 
						AND RCH_Type = 'I' AND IsINTellectualDisability = 1 AND IsCovid19 = 1 GROUP BY DOC_Code) as IPID) 
	WHERE Result_Seq = 221  
  
	--UPDATE Vertical Total
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 218) 
	WHERE Result_Seq = 218  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 219) 
	WHERE Result_Seq = 219  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 220) 
	WHERE Result_Seq = 220  
	UPDATE #ResultTable 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #ResultTable WHERE Result_Seq = 221) 
	WHERE Result_Seq = 221  

	--UPDATE Horizontal Total
	UPDATE #ResultTable 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 218 AND 221), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 218 AND 221), 
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 218 AND 221),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #ResultTable WHERE Result_Seq BETWEEN 218 AND 221) 
	WHERE Result_Seq = 222 

	--===============================================================================================================  

	--===============================================================================================================  
	--Remark
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(224, 'Remark:')   
	INSERT INTO #ResultTable (Result_Seq, Result_Value1)  
	VALUES	(225, 'Service date since ' + FORMAT(@IV_Current_Season_Start_Dtm, 'dd MMM yyyy')) 
  
-- ============================================= 
-- Sub-Report 03
-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(1, 'eHS(S)D0004-03: Report on RVP transaction (by cutoff date)'),
			(2, ''),        
			(3, 'Reporting period: as at ' + CONVERT(varchar, DATEADD(dd, -1, @Cutoff_Dtm), 111)),
			(4, '')    

	-- ============================================= 
	-- (A) Cumulative Transaction
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(5, '(A) Cumulative transaction with service date since ' + FORMAT(@Scheme_Start_Dtm, 'dd MMM yyyy')),
			(6, '')    

	-- ============================================= 
	-- (A)(i) 23vPPV
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(7, REPLACE('(i) 23vPPV (start from [DATE])', '[DATE]', FORMAT(@Scheme_Start_Dtm, 'dd MMM yyyy')) )  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(8, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
	VALUES	(9, 'RCHD'),  
			(10, 'RCHE'),        
			(11, 'RCCC'),    
			(12, 'IPID'), 
			(13, 'Total'), 
			(14, '')  

	--UPDATE RCHD  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 9  

	--UPDATE RCHE  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 10  

	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 11  

	--UPDATE IPID  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm	GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm	GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT ISNULL(SUM(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @Scheme_Start_Dtm	GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 12  

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 9)
	WHERE Result_Seq = 9  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 10)
	WHERE Result_Seq = 10 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 11)
	WHERE Result_Seq = 11 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 12)
	WHERE Result_Seq = 12 

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 9 AND 12),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 9 AND 12),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 9 AND 12), 
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 9 AND 12),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV = 1 AND Service_Receive_Dtm >= @Scheme_Start_Dtm) 
	WHERE Result_Seq = 13

	-- ============================================= 
	-- (A)(ii) PCV13
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
		VALUES	(14, REPLACE('(ii) PCV13 (start from [DATE])', '[DATE]', FORMAT(@PCV13_Start_Dtm, 'dd MMM yyyy')) )  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
		VALUES	(15, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
		VALUES	(16, 'RCHD'),  
				(17, 'RCHE'),        
				(18, 'RCCC'),    
				(19, 'IPID'), 
				(20, 'Total'),  
				(21, '')  

	--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 16  
  
	--UPDATE RCHE  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 17  

	--UPDATE RCCC  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 18  
  
	--UPDATE IPID  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PCV13_Start_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 19  

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 16)
	WHERE Result_Seq = 16  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 17)
	WHERE Result_Seq = 17 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 18)
	WHERE Result_Seq = 18 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 19)
	WHERE Result_Seq = 19 
  
	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 16 AND 19),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 16 AND 19),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 16 AND 19),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 16 AND 19),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV13 = 1 AND Service_Receive_Dtm >= @PCV13_Start_Dtm) 
	WHERE Result_Seq = 20

	-- ============================================= 
	-- (A)(iii) MMR
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1)  
	VALUES	(22, REPLACE('(iii) MMR (start from [DATE])', '[DATE]', FORMAT(@MMR_Start_Dtm, 'dd MMM yyyy')) ) 
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11,Result_Value12)  
	--VALUES	(23, '', '<36 years', '', '36 - 45 years', '', '46-52 years', '', '>52 years', '', 'Total', '', 'No. of SP involved')    
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11)  
	--VALUES	(24, '', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose')
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(24, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    	    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
	VALUES	(25, 'RCHD'),  
			(26, 'RCHE'),        
			(27, 'RCCC'),    
			(28, 'IPID'), 
			(29, 'Total'),
			(30, '')   

	----UPDATE RCHD  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD) 
	--WHERE Result_Seq = 25  
  
	----UPDATE RCHE  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' 
	--					AND IsMMR = 1 AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE) 
	--WHERE Result_Seq = 26  
  
	----UPDATE RCCC  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC) 
	--WHERE Result_Seq = 27  
  
	----UPDATE IPID  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID) 
	--WHERE Result_Seq = 28  
  
	----UPDATE Vertical Total VALUES
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 25),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 25) 
	--WHERE Result_Seq = 25  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 26),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 26) 
	--WHERE Result_Seq = 26 
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 27),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 27) 
	--WHERE Result_Seq = 27  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 28),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 28) 
	--WHERE Result_Seq = 28 
  
	----UPDATE Horizontal Total
	--UPDATE #SubResult 
	--SET 
	--	Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28), 
	--	Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28), 
	--	Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
	--	Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28), 
	--	Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28), 
	--	Result_Value7 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value7)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28), 
	--	Result_Value8 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value8)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
	--	Result_Value9 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value9)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),	
	--	Result_Value10= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value10)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
	--	Result_Value11= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value11)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
	--	Result_Value12 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @MMR_Start_Dtm) 	
	--WHERE Result_Seq = 29

		--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 25  
  
	--UPDATE RCHE  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 26  

	--UPDATE RCCC  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 27  
  
	--UPDATE IPID  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @MMR_Start_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 28  

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 25)
	WHERE Result_Seq = 25  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 26)
	WHERE Result_Seq = 26 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 27)
	WHERE Result_Seq = 27 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 28)
	WHERE Result_Seq = 28 
  
	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 25 AND 28),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @MMR_Start_Dtm) 
	WHERE Result_Seq = 29

	-- ============================================================ 
	-- (B) Transaction with service date (From 01-Jan to 31 Jul)
	-- ============================================================ 
	DECLARE @Cutoff_Report_Start_Dtm	DATETIME
	DECLARE @Temp_Cutoff_Dtm			DATETIME

	DECLARE @Current_Year_Jan_1st		DATETIME
	DECLARE @Current_Year_Aug_1st		DATETIME
	DECLARE @Next_Year_Jan_1st			DATETIME

	DECLARE @PartB_Start_Dtm			DATETIME
	DECLARE @PartB_End_Dtm				DATETIME
	DECLARE @PartC_Start_Dtm			DATETIME
	DECLARE @PartC_End_Dtm				DATETIME

	SET @Cutoff_Report_Start_Dtm = '2017-01-01'
	SET @Temp_Cutoff_Dtm = @SchemeDate

	IF @Temp_Cutoff_Dtm < @Cutoff_Report_Start_Dtm 
	BEGIN
		SET @Temp_Cutoff_Dtm = @Cutoff_Report_Start_Dtm 
	END

	SET @Current_Year_Jan_1st = DATEADD(YEAR, DATEDIFF(YEAR, 0, @Temp_Cutoff_Dtm), 0)
	SET @Current_Year_Aug_1st = DATEADD(MONTH, 7, @Current_Year_Jan_1st)
	SET @Next_Year_Jan_1st = DATEADD(YEAR, 1, @Current_Year_Jan_1st)

	SET @PartB_Start_Dtm = @Current_Year_Jan_1st
	SET	@PartB_End_Dtm = DATEADD(DAY, -1, @Current_Year_Aug_1st)
	SET @PartC_Start_Dtm = @Current_Year_Aug_1st
	SET	@PartC_End_Dtm = DATEADD(DAY, -1, @Next_Year_Jan_1st)

	IF @Temp_Cutoff_Dtm <= @Current_Year_Aug_1st AND  DATEADD(YEAR, -1, @Current_Year_Aug_1st) > @Cutoff_Report_Start_Dtm
	BEGIN	
		SET @PartC_Start_Dtm = DATEADD(YEAR, -1, @PartC_Start_Dtm)
		SET	@PartC_End_Dtm = DATEADD(YEAR, -1, @PartC_End_Dtm)
	END 

	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(41, REPLACE('(B) Transaction with service date from 1 Jan [YEAR] to 31 Jul [YEAR]', '[YEAR]', CONVERT(VARCHAR, YEAR(@PartB_Start_Dtm)))),
			(42, '')    

	-- ============================================= 
	-- (B)(i) 23vPPV
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(43, '(i) 23vPPV' )  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(44, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
	VALUES	(45, 'RCHD'),  
			(46, 'RCHE'),        
			(47, 'RCCC'),    
			(48, 'IPID'), 
			(49, 'Total'),
			(50, '')  

	--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 45  

	--UPDATE RCHE  
	UPDATE #SubResult SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 46  
  
	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 47  
  
	--UPDATE IPID  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 48  

	--UPDATE Vertical Total  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 45) 
	WHERE Result_Seq = 45  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 46) 
	WHERE Result_Seq = 46  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 47) 
	WHERE Result_Seq = 47 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 48) 
	WHERE Result_Seq = 48  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 45 AND 48), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 45 AND 48),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 45 AND 48),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 45 AND 48),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV = 1 AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm) 
	WHERE Result_Seq = 49 

	-- ============================================= 
	-- (B)(ii) PCV13
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
		VALUES	(51, '(ii) PCV13' )  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
		VALUES	(52, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
		VALUES	(53, 'RCHD'),  
				(54, 'RCHE'),        
				(55, 'RCCC'),    
				(56, 'IPID'), 
				(57, 'Total'),
				(58, '')  

	--UPDATE RCHD  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 53  
  
	--UPDATE RCHE   
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 54  
  
	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 55  
  
	--UPDATE IPID   
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV13 = 1
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 56 

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 53) 
	WHERE Result_Seq = 53 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 54) 
	WHERE Result_Seq = 54  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 55) 
	WHERE Result_Seq = 55  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 56) 
	WHERE Result_Seq = 56  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 53 AND 56),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV13 = 1 AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm) 
	WHERE Result_Seq = 57

	-- ============================================= 
	-- (B)(iii) MMR
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1)  
		VALUES	(61, '(iii) MMR') 
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11,Result_Value12)  
	--	VALUES	(62, '', '<36 years', '', '36 - 45 years', '', '46-52 years', '', '>52 years', '', 'Total', '', 'No. of SP involved')    
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11)  
	--	VALUES	(63, '', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
		VALUES	(63, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
		VALUES	(64, 'RCHD'),  
				(65, 'RCHE'),        
				(66, 'RCCC'),    
				(67, 'IPID'), 
				(68, 'Total'),
				(69, '')   

	----UPDATE RCHD  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD) 
	--WHERE Result_Seq = 64  
  
	----UPDATE RCHE  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE) 
	--WHERE Result_Seq = 65  
  
	----UPDATE RCCC  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC) 
	--WHERE Result_Seq = 66  
  
	----UPDATE IPID  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID) 
	--WHERE Result_Seq = 67  
  
	----UPDATE Vertical Total
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 64),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 64) 
	--WHERE Result_Seq = 64  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 65),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 65) 
	--WHERE Result_Seq = 65 
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 66),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 66) 
	--WHERE Result_Seq = 66  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 67),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 67) 
	--WHERE Result_Seq = 67 
  
	----UPDATE Horizontal Total
	--UPDATE #SubResult 
	--SET 
	--	Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value7 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value7)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value8 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value8)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value9 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value9)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value10= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value10)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value11= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value11)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
	--	Result_Value12 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm) 	
	--WHERE Result_Seq = 68

	--UPDATE RCHD  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 64  
  
	--UPDATE RCHE   
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 65  
  
	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 66  
  
	--UPDATE IPID   
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsMMR = 1
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 67 

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 64) 
	WHERE Result_Seq = 64 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 65) 
	WHERE Result_Seq = 65  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 66) 
	WHERE Result_Seq = 66  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 67) 
	WHERE Result_Seq = 67  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 64 AND 67),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @PartB_Start_Dtm AND Service_Receive_Dtm <= @PartB_End_Dtm) 
	WHERE Result_Seq = 68

	-- ============================================================ 
	-- (C) Transaction with service date (From 01-Aug to 31 Dec)
	-- ============================================================ 
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(71, REPLACE('(C) Transaction with service date from 1 Aug [YEAR] to 31 Dec [YEAR]', '[YEAR]', CONVERT(VARCHAR, YEAR(@PartC_Start_Dtm)))),
			(72, '')    

	-- ============================================= 
	-- (C)(i) 23vPPV
	-- =============================================
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(73, '(i) 23vPPV' )  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(74, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
	VALUES	(75, 'RCHD'),  
			(76, 'RCHE'),        
			(77, 'RCCC'),    
			(78, 'IPID'), 
			(79, 'Total'),
			(80, '')  

	--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 75  
  
	--UPDATE RCHE  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 76  
  
	--UPDATE RCCC 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 77 

	--UPDATE IPID  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 78  

	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 75) 
	WHERE Result_Seq = 75 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 76) 
	WHERE Result_Seq = 76  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 77) 
	WHERE Result_Seq = 77    
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 78) 
	WHERE Result_Seq = 78  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 75 AND 78),
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 75 AND 78),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 75 AND 78),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 75 AND 78),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV = 1 AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm)
	WHERE Result_Seq = 79 

	-- ============================================= 
	-- (C)(ii) PCV13
	-- =============================================
	INSERT INTO #SubResult (Result_Seq, Result_Value1) 
	VALUES	(81, '(ii) PCV13')  
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
	VALUES	(82, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
	VALUES	(83, 'RCHD'),  
			(84, 'RCHE'),        
			(85, 'RCCC'),    
			(86, 'IPID'), 
			(87, 'Total'),
			(88, '')

	--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 83  

	--UPDATE RCHE  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 84  

	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 85  
  
	--UPDATE IPID  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsPV13 = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 86 
  
  	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 83) 
	WHERE Result_Seq = 83 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 84) 
	WHERE Result_Seq = 84  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 85) 
	WHERE Result_Seq = 85  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 86) 
	WHERE Result_Seq = 86  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 83 AND 86), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 83 AND 86),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 83 AND 86),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 83 AND 86),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsPV13 = 1 AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm) 
	WHERE Result_Seq = 87

	-- ============================================= 
	-- (B)(iii) MMR
	-- ============================================= 
	INSERT INTO #SubResult (Result_Seq, Result_Value1)  
		VALUES	(91, '(iii) MMR') 
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11,Result_Value12)  
	--	VALUES	(92, '', '<36 years', '', '36 - 45 years', '', '46-52 years', '', '>52 years', '', 'Total', '', 'No. of SP involved')    
	--INSERT INTO #SubResult (
	--			Result_Seq, 
	--			Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, 
	--			Result_Value7, Result_Value8, Result_Value9, Result_Value10,Result_Value11)  
	--	VALUES	(93, '', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose', '1st Dose', '2nd Dose')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1, Result_Value2, Result_Value3, Result_Value4, Result_Value5, Result_Value6, Result_Value7)  
		VALUES	(93, '', '<65 years','at 65 years','>65 years', 'Total', '', 'No. of SP involved')    
	INSERT INTO #SubResult (Result_Seq, Result_Value1)        
		VALUES	(94, 'RCHD'),  
				(95, 'RCHE'),        
				(96, 'RCCC'),    
				(97, 'IPID'), 
				(98, 'Total'),
				(99, '')   

	----UPDATE RCHD  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD) 
	--WHERE Result_Seq = 94  
  
	----UPDATE RCHE  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE) 
	--WHERE Result_Seq = 95  
  
	----UPDATE RCCC  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC) 
	--WHERE Result_Seq = 96  
  
	----UPDATE IPID  
	--UPDATE #SubResult 
	--SET   
	--	Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 36 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value5 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 36 AND Age < 46 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value6 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value7 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 46 AND Age < 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID), 
	--	Result_Value8 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '1STDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
	--	Result_Value9 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1) rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age >= 53 AND DOSE = '2NDDOSE' AND IsMMR = 1 
	--					AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID) 
	--WHERE Result_Seq = 97  
  
	----UPDATE Vertical Total
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 94),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 94) 
	--WHERE Result_Seq = 94  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 95),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 95) 
	--WHERE Result_Seq = 95 
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 96),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 96) 
	--WHERE Result_Seq = 96  
	--UPDATE #SubResult 
	--SET 
	--	Result_Value10 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value4) + CONVERT(INT,Result_Value6) + CONVERT(INT,Result_Value8)    
	--						FROM #SubResult WHERE Result_Seq = 97),
	--	Result_Value11 = (SELECT CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value5) + CONVERT(INT,Result_Value7) + CONVERT(INT,Result_Value9)     
	--						FROM #SubResult WHERE Result_Seq = 97) 
	--WHERE Result_Seq = 97 
  
	----UPDATE Horizontal Total
	--UPDATE #SubResult 
	--SET 
	--	Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value6 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value6)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value7 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value7)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value8 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value8)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value9 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value9)) ,0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value10= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value10)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value11= (SELECT ISNULL(SUM(CONVERT(INT,Result_Value11)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
	--	Result_Value12 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm) 	
	--WHERE Result_Seq = 98

	--UPDATE RCHD 
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'D' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHD)
	WHERE Result_Seq = 94  

	--UPDATE RCHE  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'E' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCHE)
	WHERE Result_Seq = 95  

	--UPDATE RCCC  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'C' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as RCCC)
	WHERE Result_Seq = 96  
  
	--UPDATE IPID  
	UPDATE #SubResult 
	SET   
		Result_Value2 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age < 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value3 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age = 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID),  
		Result_Value4 = (SELECT isNULL(sum(rec),0) FROM (SELECT COUNT (Encrypt_Field1)rec FROM #SubsideCount WHERE RCH_Type = 'I' AND Age > 65 AND IsMMR = 1 
						AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm GROUP BY DOC_Code) as IPID)
	WHERE Result_Seq = 97 
  
  	--UPDATE Vertical Total
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 94) 
	WHERE Result_Seq = 94 
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 95) 
	WHERE Result_Seq = 95  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 96) 
	WHERE Result_Seq = 96  
	UPDATE #SubResult 
	SET Result_Value5 = (SELECT CONVERT(INT,Result_Value2) + CONVERT(INT,Result_Value3) + CONVERT(INT,Result_Value4) FROM #SubResult WHERE Result_Seq = 97) 
	WHERE Result_Seq = 97  

	--UPDATE Horizontal Total
	UPDATE #SubResult 
	SET 
		Result_Value2 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value2)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97), 
		Result_Value3 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value3)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
		Result_Value4 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value4)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
		Result_Value5 = (SELECT ISNULL(SUM(CONVERT(INT,Result_Value5)),0) AS TotalCount FROM #SubResult WHERE Result_Seq BETWEEN 94 AND 97),
		Result_Value7 = (SELECT COUNT(DISTINCT SP_ID) FROM #SubsideCount WHERE IsMMR = 1 AND Service_Receive_Dtm >= @PartC_Start_Dtm AND Service_Receive_Dtm <= @PartC_End_Dtm) 
	WHERE Result_Seq = 98

-- =============================================      
-- Return result      
-- =============================================      
      
	DELETE FROM RpteHSD0004_02_RVP_Tx_ByYear    
	DELETE FROM RpteHSD0004_03_RVP_Tx_ByCutoffDate
      
	INSERT INTO RpteHSD0004_02_RVP_Tx_ByYear ( 
		Display_Seq,      
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
		Result_Value12,      
		Result_Value13,      
		Result_Value14,      
		Result_Value15,      
		Result_Value16      
	)      
	SELECT      
		Result_Seq,       
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
		Result_Value12,      
		Result_Value13,      
		Result_Value14,      
		Result_Value15,      
		Result_Value16      
	FROM      
		#ResultTable    
      
	INSERT INTO RpteHSD0004_03_RVP_Tx_ByCutoffDate ( 
		Display_Seq,      
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
		Result_Value12,      
		Result_Value13,      
		Result_Value14,      
		Result_Value15,      
		Result_Value16      
	)    
	SELECT  
		Result_Seq,       
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
		Result_Value12,      
		Result_Value13,      
		Result_Value14,      
		Result_Value15,      
		Result_Value16      
	FROM      
		#SubResult   
          
	DROP TABLE #SubsidyDateTT
	DROP TABLE #RVPTransaction             
	DROP TABLE #RVPAccount   
	DROP TABLE #SubsideCount             
	DROP TABLE #ResultTable 
	DROP TABLE #SubResult 
END         
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSD0004_02_03_PrepareData] TO HCVU
GO

