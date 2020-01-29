IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementFiles_get_byReimbID]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [proc_ReimbursementFiles_get_byReimbID]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	27 Feb 2018
-- CR No.:			CRE17-013
-- Description:		Extend bank account name to 300 chars
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	20 February 2018
-- CR No.:			I-CRE17-007
-- Description:		Performance Tuning
--					1. Add WITH (NOLOCK)
--					2. Use #temp table
--					3. Add summary table to reduce large table join
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	11 August 2015
-- CR No.:			CRE15-008
-- Description:		Simplified Chinese version of HCVSCHN reimbursement file
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	2 January 2015
-- CR No.:			CRE13-019-02
-- Description:		Extend HCVS to China
-- =============================================
-- =============================================        
-- Author:			Chris YIM     
-- Create date:		25 Feb 2014      
-- Description:     Merge two SP "proc_PreAuthorizationCheck_get_byReimbID" and "proc_SuperDownload_get_byReimbID" into this one SP.
-- =============================================    

   
    
CREATE PROCEDURE  [dbo].[proc_ReimbursementFiles_get_byReimbID]        
     @reimburse_id  char(15),         
     @cutoff_Date_str char(11),        
     @scheme_code  char(10), 
	 @file_id varchar(50)       
as        
BEGIN        
    
-- =============================================
-- Declaration
-- =============================================
	DECLARE @Reimbursement_Currency		varchar(10)
	

-- =============================================
-- Initialization
-- =============================================
	SELECT @Reimbursement_Currency = Reimbursement_Currency FROM SchemeClaim WHERE Scheme_Code = @scheme_code


-----------------------------------    
-- Result Table 1: Content    
-----------------------------------     
Declare @strGenDtm varchar(50)    
SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108)    
SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm)-3)    
    
DECLARE @tempContent Table    
(        
 Result_Value1 varchar(100),        
 Result_Value2 varchar(100)         
)        
    
--INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('Sub Report ID','Sub Report Name')        
--INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('eHSM0001-01','Pre Authorization Checking File')        
--INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('','')        
--INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('','')        
INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('Report Generation Time:' + @strGenDtm,'')        
    
SELECT Result_Value1, Result_Value2 From @tempContent    


-----------------------------------    
-- Result Table 2: Summary   
-----------------------------------  
DECLARE @display_code char(25)   
 
DECLARE @EffectiveScheme table (        
	Scheme_Code  char(10),        
	Scheme_Seq  smallint        
) 
                 
DECLARE @tempSummary Table         
(        
	Parameter nvarchar(100),        
	Value varchar(100)         
)

-- Temp summary table down to subsidize
CREATE Table #SP_Practice_Summary          
(        
	SP_ID CHAR(8),        
	Practice_Display_Seq smallint,
	Scheme_Seq smallint,
	Subsidize_Code CHAR(10),
	SubsidizeUnit BIGINT,
	SubsidizeTotalAmount MONEY,
	SubsidizeTotalAmount_RMB MONEY,
	Per_Unit_Value MONEY,
	TotalNoofTx BIGINT

)
-- =============================================        
-- Initialization        
-- =============================================    
INSERT INTO @EffectiveScheme (        
	Scheme_Code,        
	Scheme_Seq        
)        
SELECT        
	Scheme_Code,        
	MAX(Scheme_Seq)        
FROM        
	SchemeClaim WITH (NOLOCK)        
WHERE        
	GETDATE() >= Effective_Dtm        
GROUP BY        
	Scheme_Code      

--
INSERT INTO #SP_Practice_Summary(
	SP_ID ,        
	Practice_Display_Seq ,
	Scheme_Seq ,
	Subsidize_Code,
	SubsidizeUnit ,
	SubsidizeTotalAmount ,
	SubsidizeTotalAmount_RMB ,
	Per_Unit_Value,
	TotalNoofTx
)
SELECT 
	t.SP_ID,
	t.Practice_Display_Seq,
	td.Scheme_Seq,
	td.Subsidize_Code,
	SUM(td.Unit) AS [SubsidizeUnit],
	SUM(td.Total_Amount) AS [SubsidizeTotalAmount], 
	SUM(ISNULL(td.Total_Amount_RMB,0)) AS [SubsidizeTotalAmount_RMB], 
	td.Per_Unit_Value,
	--si.subsidize_type  
	COUNT(t.Transaction_ID)
FROM 
	VoucherTransaction t WITH (NOLOCK), 
	ReimbursementAuthTran rat WITH (NOLOCK), 
	TransactionDetail td WITH (NOLOCK)
WHERE    
	t.transaction_id=rat.transaction_id        
	AND rat.Scheme_Code = @scheme_code      
	AND t.transaction_id = td.transaction_id      
	AND t.Scheme_code = td.scheme_code
	AND rat.Reimburse_ID = @reimburse_id
GROUP by t.SP_ID,t.Practice_Display_Seq,
	td.Scheme_Seq,
	td.Subsidize_Code,
	td.Per_Unit_Value
	       
--        
INSERT INTO @tempSummary (Parameter, Value) Values ('','')        
--INSERT INTO @tempSummary (Parameter, Value) Values ('Generation Date',CONVERT(VARCHAR(11), GETDATE(), 106) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108))        
INSERT INTO @tempSummary (Parameter, Value) Values ('Generation Date', @strGenDtm)        
    
INSERT INTO @tempSummary (Parameter, Value) Values ('Cutoff Date',@cutoff_Date_str)   

INSERT INTO @tempSummary (Parameter, Value) Values ('Reimbursement ID',RTRIM(LTRIM(@reimburse_id)))              
        
INSERT INTO @tempSummary (Parameter, Value)  
	SELECT 'Total No. of Transaction', COUNT(1) FROM ReimbursementAuthTran WITH (NOLOCK) WHERE Reimburse_ID = @reimburse_id AND Scheme_Code = @scheme_code              
	--SELECT 'Total No. of Transaction', Count(1)        
	--FROM	VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat        
	--WHERE	t.SP_ID = sp.SP_ID         
	--AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
	--AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
	--AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
	--AND b.sp_practice_display_seq = p.display_seq        
	--AND t.transaction_id=rat.transaction_id        
	--AND rat.Reimburse_ID = @reimburse_id        
	--AND rat.Scheme_Code = @scheme_code        
        
INSERT INTO @tempSummary (Parameter, Value)         
	SELECT 'Total No. of Service Provider ID (Practice No.)', Count(Distinct  cast(SP_ID as varchar)+' ('+cast(practice_display_seq as varchar)+')' ) FROM #SP_Practice_Summary
	--SELECT 'Total No. of Service Provider ID (Practice No.)', Count(Distinct  cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')' )        
	--FROM	VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat        
	--WHERE	t.SP_ID = sp.SP_ID         
	--AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
	--AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
	--AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
	--AND b.sp_practice_display_seq = p.display_seq        
	--AND t.transaction_id=rat.transaction_id        
	--AND rat.Reimburse_ID = @reimburse_id        
	--AND rat.Scheme_Code = @scheme_code        
        
INSERT INTO @tempSummary (Parameter, Value) Values ('','')        
   
DECLARE @Scheme_Seq INT      
DECLARE @Display_Seq INT 
DECLARE @avail_subsidizeCode char(20)        
DECLARE @subsidize_count int        
DECLARE @subsidize_amount int        
DECLARE @subsidize_value int        
DECLARE @subsidize_type char(20)        
--DECLARE @display_seq smallint        
DECLARE @skip_breakdown char(1)        
Set @skip_breakdown = 'Y'        
        
DECLARE avail_cursor cursor         
--FOR SELECT s.display_code as [subsidize_Code], sum(td.Unit) as [SubsidizeUnit], sum(td.Total_Amount) as [SubsidizeTotalAmount], sgc.Subsidize_Value, si.subsidize_type,      
--sgc.Display_Seq        
--FOR SELECT sgc.Display_Code_For_Claim as [subsidize_Code], sum(td.Unit) as [SubsidizeUnit], sum(td.Total_Amount) as [SubsidizeTotalAmount], sgc.Subsidize_Value, si.subsidize_type  
FOR 
	SELECT 
		summary.Scheme_Seq,
		SGC.Display_Seq,
		SGC.Display_Code_For_Claim AS [subsidize_Code],
		SUM(summary.SubsidizeUnit) AS [SubsidizeUnit] ,
		SUM(summary.SubsidizeTotalAmount) AS [SubsidizeTotalAmount], 
		summary.Per_Unit_Value,
		si.Subsidize_Type
	FROM #SP_Practice_Summary summary
		INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
		ON summary.Subsidize_Code = SGC.Subsidize_Code
			AND SGC.Scheme_Code = @scheme_code
			AND SGC.Scheme_Seq = summary.Scheme_Seq
		INNER JOIN Subsidize s WITH (NOLOCK)
		ON summary.Subsidize_Code = s.Subsidize_Code
		INNER JOIN SubsidizeItem si WITH (NOLOCK)
		ON s.Subsidize_Item_Code = si.Subsidize_Item_Code
	GROUP BY summary.Scheme_Seq,
		SGC.Display_Seq,
		SGC.Display_Code_For_Claim,
		summary.Per_Unit_Value,
		si.Subsidize_Type
	ORDER BY 
		summary.Scheme_Seq,
		SGC.Display_Seq

--	SELECT 
--		sgc.Scheme_Seq,
--		sgc.Display_Seq,
--		sgc.Display_Code_For_Claim as [subsidize_Code],
--		sum(td.Unit) as [SubsidizeUnit],
--		sum(td.Total_Amount) as [SubsidizeTotalAmount], 
--		td.Per_Unit_Value,
--		si.subsidize_type  
--	FROM 
--		VoucherTransaction t, 
--		ServiceProvider sp, 
--		practice p, 
--		bankaccount b , 
--		ReimbursementAuthTran rat, 
--		TransactionDetail td, SchemeClaim sc, 
--		SubsidizeGroupClaim sgc, 
--		Subsidize s, 
--		SubsidizeItem si    
----, @EffectiveScheme ES        
--	WHERE    t.SP_ID = sp.SP_ID         
--		AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
--		AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
--		AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
--		AND b.sp_practice_display_seq = p.display_seq        
--		AND t.transaction_id=rat.transaction_id        
--		AND rat.Reimburse_ID = @reimburse_id    
--		AND rat.Scheme_Code = @scheme_code      
--		AND t.transaction_id = td.transaction_id      
--		AND t.Scheme_code = td.scheme_code    
--		AND t.Scheme_code = sc.scheme_code    
--		--AND t.Confirmed_Dtm BETWEEN sc.Effective_Dtm AND sc.Expiry_Dtm        
--		--AND t.Scheme_Code = ES.Scheme_Code        
--		--AND ES.Scheme_Code = SC.Scheme_Code        
--		AND td.Scheme_Code = SC.Scheme_Code    
--		--AND ES.Scheme_Seq = SC.Scheme_Seq        
--		-- AND td.Scheme_Seq  = SC.Scheme_Seq    
--		AND sc.Scheme_Code = sgc.Scheme_Code        
--		-- AND sc.Scheme_Seq = sgc.Scheme_Seq     
--		AND td.Scheme_Seq = sgc.Scheme_Seq   
--		AND td.Subsidize_Code = sgc.Subsidize_Code        
--		AND sgc.Subsidize_Code = s.Subsidize_Code          
--		AND s.Subsidize_Item_Code = si.Subsidize_Item_Code          
--	--GROUP by s.display_code, sgc.Subsidize_Value, si.Subsidize_Type, sgc.Display_Seq        
--	-- GROUP by sgc.Display_Code_For_Claim, sgc.Subsidize_Value, si.Subsidize_Type
--	GROUP by sgc.Scheme_Seq, sgc.Display_Seq, sgc.Display_Code_For_Claim, td.Per_Unit_Value, si.Subsidize_Type
--	--ORDER by sgc.Display_Seq        
        
OPEN avail_cursor         
--FETCH next FROM avail_cursor INTO @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type, @display_seq        
FETCH next FROM avail_cursor INTO @Scheme_Seq, @Display_Seq, @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type  
WHILE @@Fetch_status = 0        
	BEGIN        
	IF @subsidize_type = 'VACCINE'        
	BEGIN        
		Set @skip_breakdown = 'N'       
		INSERT INTO @tempSummary (Parameter, Value) Values ('Amount of Claims for ' + RTRIM(LTRIM(@avail_subsidizeCode)) + ' ($' + CAST(@subsidize_value as varchar(20)) + ' subsidy)', @subsidize_amount)   
		INSERT INTO @tempSummary (Parameter, Value) Values ('','')        
	END        
        
	--FETCH next FROM avail_cursor INTO @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type, @display_seq        
	FETCH next FROM avail_cursor INTO @Scheme_Seq, @Display_Seq, @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type  
	END        
CLOSE avail_cursor         
DEALLOCATE avail_cursor        
           
INSERT INTO @tempSummary (Parameter, Value)       
	SELECT 'Total Amount Claimed ($)', ISNULL(SUM(SubsidizeTotalAmount),0) FROM #SP_Practice_Summary     
	--SELECT 'Total Amount Claimed ($)', ISNULL(SUM(td.Total_Amount),0)        
	--FROM     VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat, TransactionDetail td        
	--WHERE    t.SP_ID = sp.SP_ID         
	--AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
	--AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
	--AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
	--AND b.sp_practice_display_seq = p.display_seq        
	--AND t.transaction_id=rat.transaction_id        
	--AND rat.Reimburse_ID = @reimburse_id        
	--AND rat.Scheme_Code = @scheme_code        
	--AND t.transaction_id=td.transaction_id         
	--AND t.scheme_Code=td.scheme_code        

IF @Reimbursement_Currency = 'HKDRMB' BEGIN
	INSERT INTO @tempSummary (Parameter, Value)  
		SELECT N'Total Amount Claimed (¥)', ISNULL(SUM(SubsidizeTotalAmount_RMB),0) FROM #SP_Practice_Summary         
	--SELECT
	--	N'Total Amount Claimed (¥)', ISNULL(SUM(td.Total_Amount_RMB),0)        
	--FROM     VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat, TransactionDetail td        
	--WHERE    t.SP_ID = sp.SP_ID         
	--AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
	--AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
	--AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
	--AND b.sp_practice_display_seq = p.display_seq        
	--AND t.transaction_id=rat.transaction_id        
	--AND rat.Reimburse_ID = @reimburse_id        
	--AND rat.Scheme_Code = @scheme_code        
	--AND t.transaction_id=td.transaction_id         
	--AND t.scheme_Code=td.scheme_code       

END
        
INSERT INTO @tempSummary (Parameter, Value) Values ('','')        
	SELECT        
		@display_code = SC.Display_Code        
	FROM        
		@EffectiveScheme ES        
		INNER JOIN SchemeClaim SC        
			ON ES.Scheme_Code = SC.Scheme_Code        
				AND ES.Scheme_Seq = SC.Scheme_Seq        
	WHERE        
		ES.Scheme_Code = @scheme_code        
          
INSERT INTO @tempSummary (Parameter, Value) Values ('Scheme',RTRIM(LTRIM(@display_code)))  

-- =============================================        
-- Return results        
-- =============================================                  
SELECT         
	Parameter as [Summary], Value as [ ]        
FROM @tempSummary        
        

-----------------------------------    
-- Result Table 3: Report Data   
-----------------------------------       
--DECLARE @txn_count int        
--DECLARE @maxrow int        
             
CREATE TABLE #tempDetails         
(        
	SP_ID_Practice varchar(100),        
	SP_Name varchar(40),
	SP_Name_Chi	nvarchar(40),
	Practice_Name nvarchar(100),        
	Practice_Name_Chi nvarchar(100),        
	Bank_Acc_Holder nvarchar(300),        
	Bank_Account_No varchar(30),        
	Transaction_ID char(20),       
	Total_Amount money,        
	Total_Amount_RMB money,        
	ExchangeRate_Value	decimal(10, 3),
	Transaction_Dtm datetime,        
	Display_Code char(25),        
	Subsidize_Display_Code char(25),        
	Subsidize_Type char(20),        
	SP_ID char(8),        
	Practice_Display_Seq smallint,      
	RCH_Code char(10),      
	RCH_Type char(5)      
)        

DECLARE @subsidize_type_details char(20)        
                 
-- =============================================        
-- Initialization        
-- =============================================        

OPEN SYMMETRIC KEY sym_Key         
DECRYPTION BY ASYMMETRIC KEY asym_Key        
             
IF @Scheme_Code = 'RVP'      
	BEGIN       
	INSERT INTO #tempDetails        
		SELECT        
			cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')' as [SP_ID_Practice],        
			convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as [SP_Name],        
			convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])) as [SP_Name_Chi],        
			p.Practice_name,        
			p.Practice_name_Chi,        
			b.bank_acc_holder,         
			t.bank_account_no,        
			t.transaction_id,     
			td.Total_Amount,        
			td.Total_Amount_RMB,        
			td.ExchangeRate_Value,
			t.transaction_dtm,        
			sc.Display_Code,        
			sgc.Display_Code_For_Claim as subsidize_display_Code,        
			si.Subsidize_Type,        
			t.SP_ID,        
			t.Practice_Display_Seq,       
			rvp.RCH_Code as RCH_Code,       
			rvp.Type as RCH_Type      
		FROM    VoucherTransaction t WITH (NOLOCK), ServiceProvider sp WITH (NOLOCK), practice p WITH (NOLOCK), bankaccount b WITH (NOLOCK), ReimbursementAuthTran rat WITH (NOLOCK),         
				TransactionDetail td WITH (NOLOCK), SchemeClaim sc WITH (NOLOCK), SubsidizeGroupClaim sgc WITH (NOLOCK), Subsidize s WITH (NOLOCK), SubsidizeItem si WITH (NOLOCK),       
				TransactionAdditionalField tad WITH (NOLOCK), RVPHomeList rvp WITH (NOLOCK)      
		WHERE	t.SP_ID = sp.SP_ID         
			AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
			AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
			AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
			AND b.sp_practice_display_seq = p.display_seq        
			AND t.transaction_id=rat.transaction_id        
			AND rat.Reimburse_ID = @reimburse_id        
			AND rat.Scheme_Code = @scheme_code        
			AND t.transaction_id = td.transaction_id          
			AND t.Scheme_code = td.scheme_code           
			AND t.Scheme_code = sc.scheme_code           
			AND t.Scheme_Code = sgc.Scheme_Code            
			AND td.Scheme_Code = SC.Scheme_Code              
			--  AND td.Scheme_Seq = SC.Scheme_Seq    
			AND sc.Scheme_Code = sgc.Scheme_Code           
			--  AND sc.Scheme_Seq = sgc.Scheme_Seq           
			AND td.Scheme_Seq = sgc.Scheme_Seq    
			AND td.Subsidize_Code = sgc.Subsidize_Code          
			AND sgc.Subsidize_Code = s.Subsidize_Code          
			AND s.Subsidize_Item_Code = si.Subsidize_Item_Code         
			AND t.transaction_id = tad.transaction_id      
			AND tad.AdditionalFieldID = 'RHCCode'       
			AND tad.AdditionalFieldValueCode  = rvp.RCH_code      
		ORDER BY t.SP_ID asc , t.practice_display_seq asc, t.transaction_dtm ASC        
	END      
ELSE      
	BEGIN      
	INSERT INTO #tempDetails        
		SELECT        
		   cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')' as [SP_ID_Practice],        
		   convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as [SP_Name],        
		   convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])) as [SP_Name_Chi],        
		   p.Practice_name,        
		   p.Practice_name_Chi,        
		   b.bank_acc_holder,         
		   t.bank_account_no,        
		   t.transaction_id,        
		   td.Total_Amount,        
		   td.Total_Amount_RMB,        
		   td.ExchangeRate_Value,
		   t.transaction_dtm,        
		   sc.Display_Code,        
		   sgc.Display_Code_For_Claim as subsidize_display_Code,        
		   si.Subsidize_Type,        
		   t.SP_ID,        
		   t.Practice_Display_Seq,       
		   null,       
		   null      
		FROM    VoucherTransaction t WITH (NOLOCK), ServiceProvider sp WITH (NOLOCK), practice p WITH (NOLOCK), bankaccount b WITH (NOLOCK), ReimbursementAuthTran rat WITH (NOLOCK),         
				TransactionDetail td WITH (NOLOCK), SchemeClaim sc WITH (NOLOCK), SubsidizeGroupClaim sgc WITH (NOLOCK), Subsidize s WITH (NOLOCK), SubsidizeItem si WITH (NOLOCK)  
		WHERE	t.SP_ID = sp.SP_ID         
			AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
			AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
			AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
			AND b.sp_practice_display_seq = p.display_seq        
			AND t.transaction_id=rat.transaction_id        
			AND rat.Reimburse_ID = @reimburse_id        
			AND rat.Scheme_Code = @scheme_code        
			AND t.transaction_id = td.transaction_id          
			AND t.Scheme_code = td.scheme_code           
			AND t.Scheme_code = sc.scheme_code           
			AND t.Scheme_Code = sgc.Scheme_Code            
			AND td.Scheme_Code = SC.Scheme_Code              
			--  AND td.Scheme_Seq = SC.Scheme_Seq    
			AND sc.Scheme_Code = sgc.Scheme_Code           
			--  AND sc.Scheme_Seq = sgc.Scheme_Seq           
			AND td.Scheme_Seq = sgc.Scheme_Seq    
			AND td.Subsidize_Code = sgc.Subsidize_Code          
			AND sgc.Subsidize_Code = s.Subsidize_Code          
			AND s.Subsidize_Item_Code = si.Subsidize_Item_Code             
	ORDER BY t.SP_ID asc , t.practice_display_seq asc, t.transaction_dtm ASC        
END      
CLOSE SYMMETRIC KEY sym_Key        
        
Select @subsidize_type_details=(SELECT TOP 1 Subsidize_Type from #tempDetails) 
       
-- =============================================        
-- Return results        
-- =============================================             
IF @Reimbursement_Currency = 'HKDRMB' BEGIN
	IF @subsidize_type_details = 'VACCINE'        
	BEGIN        
		IF @file_id = 'PreAuthorizationCheck'
			BEGIN      
			SELECT        
				SP_ID_Practice as [Service Provider ID (Practice Number)],        
				SP_Name as [Service Provider Name],        
				Practice_name as [Practice Name],        
				bank_acc_holder as [Bank Account Name],         
				bank_account_no as [Bank Account No.],        
				transaction_id as [Transaction ID],
				Total_Amount as [Amount Claimed($)],        
				Total_Amount_RMB as [Amount Claimed(RMB)],
				ExchangeRate_Value AS [Exchange Rate],
				transaction_dtm as [Transaction Date],        
				RTRIM(Display_Code) as [Scheme],        
				RTRIM(subsidize_display_Code) as [Subsidy],  -- display when @subsidize_type_details = 'vaccine'      
				RTRIM(RCH_Code) as [RCH Code],    -- display when @Scheme_Code = 'RVP'       
				RTRIM(RCH_Type) as [RCH Type]    -- display when @Scheme_Code = 'RVP'       
			FROM        
				#tempDetails        
			ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
			END      
		ELSE      
			BEGIN      
			SELECT        
				SP_ID_Practice as [Service Provider ID (Practice Number)],        
				SP_Name as [Service Provider Name],        
				Practice_name as [Practice Name],        
				bank_acc_holder as [Bank Account Name],         
				bank_account_no as [Bank Account No.],        
				transaction_id as [Transaction ID],      
				Total_Amount as [Amount Claimed($)],        
				Total_Amount_RMB as [Amount Claimed(RMB)],
				ExchangeRate_Value AS [Exchange Rate],
				transaction_dtm as [Transaction Date],        
				RTRIM(Display_Code) as [Scheme],        
				RTRIM(subsidize_display_Code) as [Subsidy]  -- display when @subsidize_type_details = 'vaccine'      
			FROM        
				#tempDetails        
			ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
			END      
		END        
	ELSE       
		BEGIN        
		SELECT        
			SP_ID_Practice as [Service Provider ID (Practice Number)],        
			CASE ISNULL(SP_Name_Chi, '')
				WHEN '' THEN SP_Name
				ELSE SP_Name_Chi
			END AS [Service Provider Name],
			CASE ISNULL(Practice_Name_Chi, '')
				WHEN '' THEN Practice_name
				ELSE Practice_Name_Chi
			END AS [Practice Name],        
			bank_acc_holder as [Bank Account Name],         
			bank_account_no as [Bank Account No.],        
			transaction_id as [Transaction ID],   
			Total_Amount as [Amount Claimed($)],        
			Total_Amount_RMB as [Amount Claimed(RMB)],
			ExchangeRate_Value AS [Exchange Rate],
			transaction_dtm as [Transaction Date],        
			RTRIM(Display_Code) as [Scheme]        
		FROM        
			#tempDetails        
		ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
	END        

END ELSE BEGIN
	IF @subsidize_type_details = 'VACCINE'        
		BEGIN        
		IF @file_id = 'PreAuthorizationCheck'
			BEGIN      
			SELECT        
				SP_ID_Practice as [Service Provider ID (Practice Number)],        
				SP_Name as [Service Provider Name],        
				Practice_name as [Practice Name],        
				bank_acc_holder as [Bank Account Name],         
				bank_account_no as [Bank Account No.],        
				transaction_id as [Transaction ID],
				Total_Amount as [Amount Claimed($)],        
				transaction_dtm as [Transaction Date],        
				RTRIM(Display_Code) as [Scheme],        
				RTRIM(subsidize_display_Code) as [Subsidy],  -- display when @subsidize_type_details = 'vaccine'      
				RTRIM(RCH_Code) as [RCH Code],    -- display when @Scheme_Code = 'RVP'       
				RTRIM(RCH_Type) as [RCH Type]    -- display when @Scheme_Code = 'RVP'       
			FROM        
				#tempDetails        
			ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
			END      
		ELSE      
			BEGIN      
			SELECT        
				SP_ID_Practice as [Service Provider ID (Practice Number)],        
				SP_Name as [Service Provider Name],        
				Practice_name as [Practice Name],        
				bank_acc_holder as [Bank Account Name],         
				bank_account_no as [Bank Account No.],        
				transaction_id as [Transaction ID],      
				Total_Amount as [Amount Claimed($)],        
				transaction_dtm as [Transaction Date],        
				RTRIM(Display_Code) as [Scheme],        
				RTRIM(subsidize_display_Code) as [Subsidy]  -- display when @subsidize_type_details = 'vaccine'      
			FROM        
				#tempDetails        
			ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
			END      
		END        
	ELSE       
		BEGIN        
		SELECT        
			SP_ID_Practice as [Service Provider ID (Practice Number)],        
			SP_Name as [Service Provider Name],        
			Practice_name as [Practice Name],        
			bank_acc_holder as [Bank Account Name],         
			bank_account_no as [Bank Account No.],        
			transaction_id as [Transaction ID],   
			Total_Amount as [Amount Claimed($)],        
			transaction_dtm as [Transaction Date],        
			RTRIM(Display_Code) as [Scheme]        
		FROM        
			#tempDetails        
		ORDER BY SP_ID asc , Practice_Display_Seq asc, Transaction_dtm ASC        
	END        
END        
        
-----------------------------------    
-- Result Table 4: Legend    
-----------------------------------      
DECLARE @tempLegend Table         
(        
	Parameter varchar(100),        
	Value varchar(100)         
)        
-- =============================================        
-- Initialization        
-- =============================================            
INSERT INTO @tempLegend (Parameter, Value) Values ('Scheme Legend:','')        
INSERT INTO @tempLegend (Parameter, Value)        
	select rtrim(display_Code), scheme_desc 
	from SchemeClaim        
	group by display_Code, scheme_desc, display_seq        
	order by display_seq        
    
INSERT INTO @tempLegend (Parameter, Value) Values ('','')        
    
INSERT INTO @tempLegend (Parameter, Value) Values ('Subsidy Legend:','')        
    
INSERT INTO @tempLegend (Parameter, Value)        
	select distinct(display_Code_for_Claim), Legend_Desc_For_Claim 
	from SubsidizeGroupClaim WITH (NOLOCK)
	where SubsidizeGroupClaim.Subsidize_Code in 
		(select Subsidize.Subsidize_Code 
			from Subsidize WITH (NOLOCK), SubsidizeItem WITH (NOLOCK)
			where subsidize_type = 'VACCINE'
			and Subsidize.subsidize_item_code = SubsidizeItem.subsidize_item_code)
	order by display_Code_for_Claim, Legend_Desc_For_Claim  

-- =============================================        
-- Return results        
-- =============================================            
SELECT         
	Parameter as [Legend], Value as [ ]        
FROM @tempLegend        

-- =============================================        
-- House Keeping        
-- =============================================     
DROP TABLE #SP_Practice_Summary
DROP TABLE #tempDetails

END        

GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementFiles_get_byReimbID] TO HCVU
GO

