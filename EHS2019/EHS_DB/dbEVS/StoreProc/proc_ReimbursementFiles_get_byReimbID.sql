
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementFiles_get_byReimbID]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [proc_ReimbursementFiles_get_byReimbID];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
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
-- Modified by:		Winnie SUEN
-- Modified date:	18 Nov 2020
-- CR No.:			CRE20-016 (To update the format of the Pre Authorization Checking File)
-- Description:		Add [No.] and [Selected] column on Practice Basis (02)
-- ============================================= 
-- =============================================
-- Modification History
-- Modified by:		Chris YIM
-- Modified date:	16 Nov 2020
-- CR No.:			INT20-0050
-- Description:		Fix temp table column for "Display_Code_For_Claim" 
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	21 Oct 2020
-- CR No.:			CRE20-015 (HA Scheme)
-- Description:		Enlarge size of [Scheme_Claim].[Scheme_Desc] from 100 -> 200
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	17 Aug 2020
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
--					1. Handle the released case
--					
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Dickson Law
-- Modified date:	26 February 2018
-- CR No.:			CRE17-004
-- Description:		Generate a new DPAR on EHCP basis
--					1. Reorganize temp table creation
--					2. Rename sheet 01 to 03 and add sheet 01,02 for HCVSCHN and sheet 01,02 for other scheme 
-- =============================================
-- =============================================  
-- Modification History  
-- Modified by:  Chris YIM  
-- Modified date: 27 Feb 2018  
-- CR No.:   CRE17-013  
-- Description:  Extend bank account name to 300 chars  
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

CREATE PROCEDURE [dbo].[proc_ReimbursementFiles_get_byReimbID] @reimburse_id    CHAR(15), 
                                                               @cutoff_Date_str CHAR(11), 
                                                               @scheme_code     CHAR(10), 
                                                               @file_id         VARCHAR(50)
AS
    BEGIN

        -- =============================================
        -- Declaration
        -- =============================================
        DECLARE @Reimbursement_Currency VARCHAR(10);
        DECLARE @Verification_Case_Available CHAR(1);
        DECLARE @display_code CHAR(25);
        DECLARE @subsidize_type_details CHAR(20);
        DECLARE @strGenDtm VARCHAR(50);
        DECLARE @strReleasedMsg VARCHAR(50);

        DECLARE @tempLegend TABLE
        (Parameter VARCHAR(1000), 
         Value     VARCHAR(1000)
        );

        DECLARE @EffectiveScheme TABLE
        (Scheme_Code CHAR(10), 
         Scheme_Seq  SMALLINT
        );

        DECLARE @tempContent TABLE
        (Result_Value1 VARCHAR(100), 
         Result_Value2 VARCHAR(100)
        );

        DECLARE @tempSummary TABLE
        (Parameter NVARCHAR(100), 
         Value     VARCHAR(100)
        );

        -- Temp summary table down to subsidize
        --CREATE Table #SP_Practice_Summary          
        --(        
        --	SP_ID CHAR(8),        
        --	Practice_Display_Seq smallint,
        --	Scheme_Seq smallint,
        --	Subsidize_Code CHAR(10),
        --	SubsidizeUnit BIGINT,
        --	SubsidizeTotalAmount MONEY,
        --	SubsidizeTotalAmount_RMB MONEY,
        --	Per_Unit_Value MONEY,
        --	TotalNoofTx BIGINT
        --
        --)

        CREATE TABLE #initialTransaction
        (   
        --Transaction_Item_No smallint,      
        SP_ID_Practice         VARCHAR(100), 
        SP_Name                VARCHAR(40), 
        SP_Name_Chi            NVARCHAR(40), 
        Practice_Name          NVARCHAR(100), 
        Practice_Name_Chi      NVARCHAR(100), 
        Bank_Acc_Holder        NVARCHAR(300), 
        Bank_Account_No        VARCHAR(30), 
        Transaction_ID         CHAR(20), 
        SubsidizeUnit          BIGINT, 
        Total_Amount           MONEY, 
        Total_Amount_RMB       MONEY, 
        Per_Unit_Value         MONEY, 
        ExchangeRate_Value     DECIMAL(10, 3), 
        Transaction_Dtm        DATETIME, 
        Display_Code           CHAR(25), 
        Subsidize_Display_Code CHAR(25), 
        Subsidize_Code         CHAR(10), 
        Display_Code_For_Claim VARCHAR(25), 
        Display_Seq            SMALLINT, 
        Subsidize_Type         CHAR(20), 
        SP_ID                  CHAR(8), 
        SP_Item_No             SMALLINT, 
        SP_Verification_Case   VARCHAR(1), 
        Practice_Display_Seq   SMALLINT,
        Scheme_Seq             SMALLINT,
        --TRAN_Verification_Case   CHAR(1),     
        RCH_Code               CHAR(10), 
        RCH_Type               CHAR(5)
        );

        CREATE TABLE #temp01
        (SP_Item_No           SMALLINT, 
         SP_ID                CHAR(8), 
         SP_Name              VARCHAR(40), 
         SP_Name_Chi          NVARCHAR(40), 
         SP_Verification_Case VARCHAR(1), 
         Total_Transaction    SMALLINT, 
         Total_Amount         MONEY, 
         Total_Amount_RMB     MONEY, 
         Scheme_Code          CHAR(10)
        );

        CREATE TABLE #temp02
        (SP_ID_Practice       VARCHAR(100), 
         SP_Name              VARCHAR(40), 
         SP_Name_Chi          NVARCHAR(40), 
         Practice_Name        NVARCHAR(100), 
         Practice_Name_Chi    NVARCHAR(100), 
         Bank_Acc_Holder      NVARCHAR(300), 
         Bank_Account_No      VARCHAR(30), 
         Total_Amount         MONEY, 
         Total_Amount_RMB     MONEY, 
         Scheme_Code          CHAR(10), 
         Practice_Display_Seq SMALLINT, 
         SP_ID                CHAR(8),
		 SP_Verification_Case VARCHAR(1)
        );
        CREATE TABLE #temp03
        (SP_Item_No             SMALLINT,  
         --Transaction_Item_No smallint,    
         SP_ID_Practice         VARCHAR(100), 
         SP_Name                VARCHAR(40), 
         SP_Name_Chi            NVARCHAR(40), 
         Practice_Name          NVARCHAR(100), 
         Practice_Name_Chi      NVARCHAR(100), 
         Bank_Acc_Holder        NVARCHAR(300), 
         Bank_Account_No        VARCHAR(30), 
         Transaction_ID         CHAR(20), 
         Total_Amount           MONEY, 
         Total_Amount_RMB       MONEY, 
         ExchangeRate_Value     DECIMAL(10, 3), 
         Transaction_Dtm        DATETIME, 
         Display_Code           CHAR(25), 
         Subsidize_Display_Code CHAR(25), 
         Subsidize_Type         CHAR(20), 
         SP_ID                  CHAR(8), 
         Practice_Display_Seq   SMALLINT, 
         RCH_Code               CHAR(10), 
         RCH_Type               CHAR(5)
        );

        -- =============================================
        -- Initialization
        -- =============================================
        SELECT @Reimbursement_Currency = Reimbursement_Currency
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

        SELECT @Verification_Case_Available = Verification_Case_Available
        FROM ReimbursementAuthorisation
        WHERE Reimburse_ID = @reimburse_id
              AND Scheme_Code = @scheme_code
              AND Authorised_Status = 'P'
              AND Record_Status = 'A';

        INSERT INTO @EffectiveScheme
               (Scheme_Code, 
                Scheme_Seq
               )
        SELECT Scheme_Code, 
               MAX(Scheme_Seq)
        FROM SchemeClaim WITH(NOLOCK)
        WHERE GETDATE() >= Effective_Dtm
        GROUP BY Scheme_Code;

        SET @strReleasedMsg = 'The record is released';
        -----------------------------------    
        -- initial Transaction Pool
        -----------------------------------       
        --DECLARE @txn_count int        
        --DECLARE @maxrow int        

        EXEC [proc_SymmetricKey_open]

        INSERT INTO #initialTransaction
        SELECT
        --DENSE_RANK() OVER(ORDER BY RAT.SP_ID ASC , RAT.practice_display_seq ASC, RAT.Transaction_ID ASC , VT.transaction_dtm ASC ) AS Transaction_Item_No, 
        CAST(VT.SP_ID AS VARCHAR) + ' (' + CAST(VT.practice_display_seq AS VARCHAR) + ')' AS [SP_ID_Practice], 
        CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.[Encrypt_Field2])) AS [SP_Name], 
        CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.[Encrypt_Field3])) AS [SP_Name_Chi], 
        P.Practice_Name, 
        P.Practice_Name_Chi, 
        B.Bank_Acc_Holder, 
        B.Bank_Account_No, 
        RAT.Transaction_ID, 
        TD.Unit, 
        TD.Total_Amount, 
        TD.Total_Amount_RMB, 
        TD.Per_Unit_Value, 
        TD.ExchangeRate_Value, 
        VT.Transaction_Dtm, 
        SC.Display_Code, 
        SS.Display_Code, 
        TD.Subsidize_Code, 
        SGC.Display_Code_For_Claim, 
        SGC.Display_Seq, 
        SI.Subsidize_Type, 
        VT.SP_ID, 
        RSP.Seq_No, 
        RSP.Verification_Case, 
        VT.practice_display_seq, 
        TD.Scheme_Seq,
        --RAT.Verification_Case,
        CASE
            WHEN @scheme_code = 'RVP'
            THEN TAF.AdditionalFieldValueCode
            ELSE NULL
        END AS [RCH_Code],
        CASE
            WHEN @scheme_code = 'RVP'
            THEN RHL.Type
            ELSE NULL
        END AS [RCH_Type]
        FROM ReimbursementAuthTran AS RAT
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON RAT.Transaction_ID = VT.Transaction_ID
             INNER JOIN ServiceProvider AS SP WITH(NOLOCK)
             ON VT.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P WITH(NOLOCK)
             ON VT.SP_ID = P.SP_ID
                AND VT.Practice_Display_Seq = P.display_seq
             INNER JOIN BankAccount AS B WITH(NOLOCK)
             ON VT.SP_ID = B.SP_ID
                AND VT.Practice_Display_Seq = B.SP_Practice_Display_Seq
             INNER JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON RAT.Transaction_ID = TD.Transaction_ID
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON RAT.Scheme_Code = SC.Scheme_Code
             INNER JOIN Subsidize AS SS WITH(NOLOCK)
             ON TD.Subsidize_Code = SS.Subsidize_Code
                AND TD.Subsidize_Item_Code = SS.Subsidize_Item_Code
             INNER JOIN SubsidizeItem AS SI WITH(NOLOCK)
             ON TD.Subsidize_Item_Code = SI.Subsidize_Item_Code
             INNER JOIN SubsidizeGroupClaim AS SGC WITH(NOLOCK)
             ON TD.Subsidize_Code = SGC.Subsidize_Code
                AND RAT.Scheme_Code = SGC.Scheme_Code
                AND TD.Scheme_Seq = SGC.Scheme_Seq
             LEFT JOIN ReimbursementServiceProvider AS RSP WITH(NOLOCK)
             ON VT.SP_ID = RSP.SP_ID
                AND RAT.Reimburse_ID = RSP.Reimburse_ID
                AND RAT.Scheme_Code = RSP.Scheme_Code
             LEFT JOIN TransactionAdditionalField AS TAF WITH(NOLOCK)
             ON RAT.Transaction_ID = TAF.Transaction_ID
                AND TAF.AdditionalFieldID = 'RHCCode'
             LEFT JOIN RVPHomeList AS RHL WITH(NOLOCK)
             ON TAF.AdditionalFieldValueCode = RHL.RCH_code
        WHERE RAT.Reimburse_ID = @reimburse_id
              AND RAT.Scheme_Code = @scheme_code
        ORDER BY VT.SP_ID ASC, 
                 VT.practice_display_seq ASC, 
                 RAT.Transaction_ID ASC, 
                 VT.transaction_dtm ASC;
        EXEC [proc_SymmetricKey_close]

        SELECT @subsidize_type_details =
        (
            SELECT TOP 1 Subsidize_Type
            FROM #initialTransaction
        );

        -----------------------------------    
        -- Result Table 1: Content    
        -----------------------------------     

        SET @strGenDtm = CONVERT(VARCHAR(11), GETDATE(), 111) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108);
        SET @strGenDtm = LEFT(@strGenDtm, LEN(@strGenDtm) - 3);

        --INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('Sub Report ID','Sub Report Name')        
        --INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('eHSM0001-01','Pre Authorization Checking File')        
        --INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('','')        
        --INSERT INTO @tempContent (Result_Value1, Result_Value2) Values ('','')   

        IF @file_id = 'PreAuthorizationCheck'
            BEGIN
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0001-01', 
                       'Pre Authorization Checking File (on EHCP Basis)'
                      );
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0001-02', 
                       'Pre Authorization Checking File (on Practice Basis)'
                      );
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0001-03', 
                       'Pre Authorization Checking File (on Transaction Basis)'
                      );
            END;
            ELSE
            BEGIN
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0002-01', 
                       'Reimbursement Data File (on EHCP Basis)'
                      );
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0002-02', 
                       'Reimbursement Data File (on Practice Basis)'
                      );
                INSERT INTO @tempContent
                       (Result_Value1, 
                        Result_Value2
                       )
                VALUES
                      ('eHS(S)M0002-03', 
                       'Reimbursement Data File (on Transaction Basis)'
                      );
            END;

        INSERT INTO @tempContent
               (Result_Value1, 
                Result_Value2
               )
        VALUES
              ('', 
               ''
              );
        INSERT INTO @tempContent
               (Result_Value1, 
                Result_Value2
               )
        VALUES
              ('', 
               ''
              );
        INSERT INTO @tempContent
               (Result_Value1, 
                Result_Value2
               )
        VALUES
              ('Report Generation Time:' + @strGenDtm, 
               ''
              );

        -----------------------------------    
        -- Result Table 2: Summary   
        -----------------------------------  
        ----
        --INSERT INTO #SP_Practice_Summary(
        --	SP_ID ,        
        --	Practice_Display_Seq ,
        --	Scheme_Seq ,
        --	Subsidize_Code,
        --	SubsidizeUnit ,
        --	SubsidizeTotalAmount ,
        --	SubsidizeTotalAmount_RMB ,
        --	Per_Unit_Value,
        --	TotalNoofTx
        --)
        --SELECT 
        --	rat.SP_ID,
        --	rat.Practice_Display_Seq,
        --	td.Scheme_Seq,
        --	td.Subsidize_Code,
        --	SUM(td.Unit) AS [SubsidizeUnit],
        --	SUM(td.Total_Amount) AS [SubsidizeTotalAmount], 
        --	SUM(ISNULL(td.Total_Amount_RMB,0)) AS [SubsidizeTotalAmount_RMB], 
        --	td.Per_Unit_Value,
        --	--si.subsidize_type  
        --	COUNT(rat.Transaction_ID)
        --FROM  
        --	ReimbursementAuthTran rat WITH (NOLOCK), 
        --	TransactionDetail td WITH (NOLOCK)
        --WHERE          
        --	rat.Scheme_Code = @scheme_code      
        --	AND rat.transaction_id = td.transaction_id      
        --	AND rat.Scheme_code = td.scheme_code
        --	AND rat.Reimburse_ID = @reimburse_id
        --GROUP by rat.SP_ID,rat.Practice_Display_Seq,
        --	td.Scheme_Seq,
        --	td.Subsidize_Code,
        --	td.Per_Unit_Value
        --        
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
              );        
        --INSERT INTO @tempSummary (Parameter, Value) Values ('Generation Date',CONVERT(VARCHAR(11), GETDATE(), 106) + ' ' + CONVERT(VARCHAR(8), GETDATE(), 108))        
        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('Generation Date', 
               @strGenDtm
              );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('Cutoff Date', 
               @cutoff_Date_str
              );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('Reimbursement ID', 
               RTRIM(LTRIM(@reimburse_id))
              );

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT 'Total No. of Transaction', 
               COUNT(DISTINCT Transaction_ID)
        FROM #initialTransaction; 
        --SELECT 'Total No. of Transaction', COUNT(1) FROM ReimbursementAuthTran WHERE Reimburse_ID = @reimburse_id AND Scheme_Code = @scheme_code              
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

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT 'Total No. of Service Provider ID', 
               COUNT(DISTINCT SP_ID)
        FROM #initialTransaction;     
        --SELECT 'Total No. of Service Provider ID', Count(Distinct SP_ID) FROM #SP_Practice_Summary  

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT 'Total No. of Service Provider ID (Practice No.)', 
               COUNT(DISTINCT SP_ID_Practice)
        FROM #initialTransaction;        
        --SELECT 'Total No. of Service Provider ID (Practice No.)', Count(Distinct  cast(SP_ID as varchar)+' ('+cast(practice_display_seq as varchar)+')' ) FROM #SP_Practice_Summary
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

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
              );

        DECLARE @Scheme_Seq INT;
        DECLARE @Display_Seq INT;
        DECLARE @avail_subsidizeCode CHAR(25);
        DECLARE @subsidize_count INT;
        DECLARE @subsidize_amount INT;
        DECLARE @subsidize_value INT;
        DECLARE @subsidize_type CHAR(20);        
        --DECLARE @display_seq smallint        
        DECLARE @skip_breakdown CHAR(1);
        SET @skip_breakdown = 'Y';

        DECLARE avail_cursor CURSOR         
        --FOR SELECT s.display_code as [subsidize_Code], sum(td.Unit) as [SubsidizeUnit], sum(td.Total_Amount) as [SubsidizeTotalAmount], sgc.Subsidize_Value, si.subsidize_type,      
        --sgc.Display_Seq        
        --FOR SELECT sgc.Display_Code_For_Claim as [subsidize_Code], sum(td.Unit) as [SubsidizeUnit], sum(td.Total_Amount) as [SubsidizeTotalAmount], sgc.Subsidize_Value, si.subsidize_type  
        FOR SELECT IT.Scheme_Seq, 
                   IT.Display_Seq, 
                   IT.Display_Code_For_Claim AS [subsidize_Code], 
                   SUM(IT.SubsidizeUnit) AS [SubsidizeUnit], 
                   SUM(IT.Total_Amount) AS [SubsidizeTotalAmount], 
                   IT.Per_Unit_Value, 
                   IT.Subsidize_Type
            FROM #initialTransaction AS IT
            GROUP BY IT.Scheme_Seq, 
                     IT.Display_Seq, 
                     IT.Display_Code_For_Claim, 
                     IT.Per_Unit_Value, 
                     IT.Subsidize_Type
            ORDER BY IT.Scheme_Seq, 
                     IT.Display_Seq; 
        --SELECT 
        --	summary.Scheme_Seq,
        --	SGC.Display_Seq,
        --	SGC.Display_Code_For_Claim AS [subsidize_Code],
        --	SUM(summary.SubsidizeUnit) AS [SubsidizeUnit] ,
        --	SUM(summary.SubsidizeTotalAmount) AS [SubsidizeTotalAmount], 
        --	summary.Per_Unit_Value,
        --	si.Subsidize_Type
        --FROM #SP_Practice_Summary summary
        --	INNER JOIN SubsidizeGroupClaim SGC WITH (NOLOCK)
        --	ON summary.Subsidize_Code = SGC.Subsidize_Code
        --		AND SGC.Scheme_Code = @scheme_code
        --		AND SGC.Scheme_Seq = summary.Scheme_Seq
        --	INNER JOIN Subsidize s WITH (NOLOCK)
        --	ON summary.Subsidize_Code = s.Subsidize_Code
        --	INNER JOIN SubsidizeItem si WITH (NOLOCK)
        --	ON s.Subsidize_Item_Code = si.Subsidize_Item_Code
        --GROUP BY summary.Scheme_Seq,
        --	SGC.Display_Seq,
        --	SGC.Display_Code_For_Claim,
        --	summary.Per_Unit_Value,
        --	si.Subsidize_Type
        --ORDER BY 
        --	summary.Scheme_Seq,
        --	SGC.Display_Seq
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

        OPEN avail_cursor;         
        --FETCH next FROM avail_cursor INTO @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type, @display_seq        
        FETCH NEXT FROM avail_cursor INTO @Scheme_Seq, @Display_Seq, @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value,
        @subsidize_type;
        WHILE @@Fetch_status = 0
            BEGIN
                IF @subsidize_type = 'VACCINE'
                    BEGIN
                        SET @skip_breakdown = 'N';
                        INSERT INTO @tempSummary
                               (Parameter, 
                                Value
                               )
                        VALUES
                              ('Amount of Claims for ' + RTRIM(LTRIM(@avail_subsidizeCode)) + ' ($' + CAST(@subsidize_value AS VARCHAR(20)) +
                              ' subsidy)', 
                               @subsidize_amount
                              );
                        INSERT INTO @tempSummary
                               (Parameter, 
                                Value
                               )
                        VALUES
                              ('', 
                               ''
                              );
                    END;

                --FETCH next FROM avail_cursor INTO @avail_subsidizeCode, @subsidize_count, @subsidize_amount, @subsidize_value, @subsidize_type, @display_seq        
                FETCH NEXT FROM avail_cursor INTO @Scheme_Seq, @Display_Seq, @avail_subsidizeCode, @subsidize_count, @subsidize_amount,
                @subsidize_value, @subsidize_type;
            END;
        CLOSE avail_cursor;
        DEALLOCATE avail_cursor;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        SELECT 'Total Amount Claimed ($)', 
               ISNULL(SUM(Total_Amount), 0)
        FROM #initialTransaction;  
        --SELECT 'Total Amount Claimed ($)', ISNULL(SUM(SubsidizeTotalAmount),0) FROM #SP_Practice_Summary     
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

        IF @Reimbursement_Currency = 'HKDRMB'
            BEGIN
                INSERT INTO @tempSummary
                       (Parameter, 
                        Value
                       )
                SELECT N'Total Amount Claimed (¥)', 
                       ISNULL(SUM(Total_Amount_RMB), 0)
                FROM #initialTransaction;     
                --SELECT N'Total Amount Claimed (¥)', ISNULL(SUM(SubsidizeTotalAmount_RMB),0) FROM #SP_Practice_Summary         
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
            END;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
              );
        SELECT @display_code = SC.Display_Code
        FROM @EffectiveScheme AS ES
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON ES.Scheme_Code = SC.Scheme_Code
                AND ES.Scheme_Seq = SC.Scheme_Seq
        WHERE ES.Scheme_Code = @scheme_code;

        INSERT INTO @tempSummary
               (Parameter, 
                Value
               )
        VALUES
              ('Scheme', 
               RTRIM(LTRIM(@display_code))
              );

        -----------------------------------    
        -- Result Table 3:  Sheet 01
        -----------------------------------

        INSERT INTO #temp01
               (SP_ID, 
                SP_Item_No, 
                SP_Name, 
                SP_Name_Chi, 
                SP_Verification_Case, 
                Scheme_Code
               )
        SELECT DISTINCT 
               IT.SP_ID, 
               IT.SP_Item_No, 
               IT.SP_Name, 
               IT.SP_Name_Chi,
               CASE
                   WHEN IT.SP_Verification_Case IS NULL
                   THEN ''
                   ELSE 'Y'
               END, 
               Display_Code
        FROM #initialTransaction AS IT;

        UPDATE #temp01
          SET Total_Transaction = totalTran, 
              Total_Amount = totalAmount, 
              Total_Amount_RMB = totalAmountRMB
        FROM
        (
            SELECT SP_ID, 
                   COUNT(DISTINCT Transaction_ID) AS totalTran, 
                   SUM(Total_Amount) AS totalAmount, 
                   SUM(Total_Amount_RMB) AS totalAmountRMB
            FROM #initialTransaction
            GROUP BY SP_ID
        ) T1
        WHERE #temp01.SP_ID = T1.SP_ID;

        -----------------------------------    
        -- Result Table 4:  Sheet 02 
        ----------------------------------- 
        INSERT INTO #temp02
               (SP_ID_Practice, 
                SP_Name, 
                SP_Name_Chi, 
                Practice_Name, 
                Practice_Name_Chi, 
                Bank_Acc_Holder, 
                Bank_Account_No, 
                Total_Amount, 
                Total_Amount_RMB, 
                Scheme_Code, 
                practice_display_seq, 
                SP_ID,
				SP_Verification_Case
               )
        SELECT SP_ID_Practice, 
               SP_Name, 
               SP_Name_Chi, 
               Practice_Name, 
               Practice_Name_Chi, 
               Bank_Acc_Holder, 
               Bank_Account_No, 
               SUM(Total_Amount), 
               SUM(Total_Amount_RMB), 
               Display_Code, 
               practice_display_seq, 
               SP_ID,
			   IIF(ISNULL(SP_Verification_Case, '') = '', 'N', 'Y')
        FROM #initialTransaction
        GROUP BY SP_ID_Practice, 
                 SP_Name, 
                 SP_Name_Chi, 
                 Practice_Name, 
                 Practice_Name_Chi, 
                 Bank_Acc_Holder, 
                 Bank_Account_No, 
                 Display_Code, 
                 practice_display_seq, 
                 SP_ID,
				 IIF(ISNULL(SP_Verification_Case, '') = '', 'N', 'Y');

        -----------------------------------    
        -- Result Table 5:  Sheet 03 
        -----------------------------------       
        --DECLARE @txn_count int        
        --DECLARE @maxrow int        

        INSERT INTO #temp03
        SELECT
        --Transaction_Item_No,
        SP_Item_No, 
        SP_ID_Practice, 
        SP_Name, 
        SP_Name_Chi, 
        Practice_Name, 
        Practice_Name_Chi, 
        Bank_Acc_Holder, 
        Bank_Account_No, 
        Transaction_ID, 
        Total_Amount, 
        Total_Amount_RMB, 
        ExchangeRate_Value, 
        Transaction_Dtm, 
        Display_Code, 
        Display_Code_For_Claim, 
        Subsidize_Type, 
        SP_ID, 
        practice_display_seq, 
        RCH_Code, 
        RCH_Type
        FROM #initialTransaction AS IT
        ORDER BY SP_Item_No ASC;

        --IF @Scheme_Code = 'RVP'      
        --	BEGIN       
        --	INSERT INTO @tempDetails        
        --		SELECT        
        --			cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')' as [SP_ID_Practice],        
        --			convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as [SP_Name],        
        --			convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])) as [SP_Name_Chi],        
        --			p.Practice_name,        
        --			p.Practice_name_Chi,        
        --			b.bank_acc_holder,         
        --			t.bank_account_no,        
        --			t.transaction_id,     
        --			td.Total_Amount,        
        --			td.Total_Amount_RMB,        
        --			td.ExchangeRate_Value,
        --			t.transaction_dtm,        
        --			sc.Display_Code,        
        --			sgc.Display_Code_For_Claim as subsidize_display_Code,        
        --			si.Subsidize_Type,        
        --			t.SP_ID,        
        --			t.Practice_Display_Seq,       
        --			rvp.RCH_Code as RCH_Code,       
        --			rvp.Type as RCH_Type      
        --		FROM    VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat,         
        --				TransactionDetail td, SchemeClaim sc, SubsidizeGroupClaim sgc, Subsidize s, SubsidizeItem si,       
        --				TransactionAdditionalField tad, RVPHomeList rvp      
        --		WHERE	t.SP_ID = sp.SP_ID         
        --			AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
        --			AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
        --			AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
        --			AND b.sp_practice_display_seq = p.display_seq        
        --			AND t.transaction_id=rat.transaction_id        
        --			AND rat.Reimburse_ID = @reimburse_id        
        --			AND rat.Scheme_Code = @scheme_code        
        --			AND t.transaction_id = td.transaction_id          
        --			AND t.Scheme_code = td.scheme_code           
        --			AND t.Scheme_code = sc.scheme_code           
        --			AND t.Scheme_Code = sgc.Scheme_Code            
        --			AND td.Scheme_Code = SC.Scheme_Code              
        --			--  AND td.Scheme_Seq = SC.Scheme_Seq    
        --			AND sc.Scheme_Code = sgc.Scheme_Code           
        --			--  AND sc.Scheme_Seq = sgc.Scheme_Seq           
        --			AND td.Scheme_Seq = sgc.Scheme_Seq    
        --			AND td.Subsidize_Code = sgc.Subsidize_Code          
        --			AND sgc.Subsidize_Code = s.Subsidize_Code          
        --			AND s.Subsidize_Item_Code = si.Subsidize_Item_Code         
        --			AND t.transaction_id = tad.transaction_id      
        --			AND tad.AdditionalFieldID = 'RHCCode'       
        --			AND tad.AdditionalFieldValueCode  = rvp.RCH_code      
        --		ORDER BY t.SP_ID asc , t.practice_display_seq asc, t.transaction_dtm ASC        
        --	END      
        --ELSE      
        --	BEGIN      
        --	INSERT INTO @tempDetails        
        --		SELECT        
        --		   cast(t.SP_ID as varchar)+' ('+cast(t.practice_display_seq as varchar)+')' as [SP_ID_Practice],        
        --		   convert(varchar(40), DecryptByKey(sp.[Encrypt_Field2])) as [SP_Name],        
        --		   convert(nvarchar(40), DecryptByKey(sp.[Encrypt_Field3])) as [SP_Name_Chi],        
        --		   p.Practice_name,        
        --		   p.Practice_name_Chi,        
        --		   b.bank_acc_holder,         
        --		   t.bank_account_no,        
        --		   t.transaction_id,        
        --		   td.Total_Amount,        
        --		   td.Total_Amount_RMB,        
        --		   td.ExchangeRate_Value,
        --		   t.transaction_dtm,        
        --		   sc.Display_Code,        
        --		   sgc.Display_Code_For_Claim as subsidize_display_Code,        
        --		   si.Subsidize_Type,        
        --		   t.SP_ID,        
        --		   t.Practice_Display_Seq,       
        --		   null,       
        --		   null      
        --		FROM    VoucherTransaction t, ServiceProvider sp, practice p, bankaccount b , ReimbursementAuthTran rat,         
        --				TransactionDetail td, SchemeClaim sc, SubsidizeGroupClaim sgc, Subsidize s, SubsidizeItem si    
        --		WHERE	t.SP_ID = sp.SP_ID         
        --			AND t.sp_id = p.sp_id AND t.practice_display_seq = p.display_seq AND t.sp_id = sp.sp_id        
        --			AND t.sp_id = b.sp_id AND t.bank_acc_display_seq = b.display_seq        
        --			AND sp.sp_id = p.sp_id AND b.sp_id = p.sp_id        
        --			AND b.sp_practice_display_seq = p.display_seq        
        --			AND t.transaction_id=rat.transaction_id        
        --			AND rat.Reimburse_ID = @reimburse_id        
        --			AND rat.Scheme_Code = @scheme_code        
        --			AND t.transaction_id = td.transaction_id          
        --			AND t.Scheme_code = td.scheme_code           
        --			AND t.Scheme_code = sc.scheme_code           
        --			AND t.Scheme_Code = sgc.Scheme_Code            
        --			AND td.Scheme_Code = SC.Scheme_Code              
        --			--  AND td.Scheme_Seq = SC.Scheme_Seq    
        --			AND sc.Scheme_Code = sgc.Scheme_Code           
        --			--  AND sc.Scheme_Seq = sgc.Scheme_Seq           
        --			AND td.Scheme_Seq = sgc.Scheme_Seq    
        --			AND td.Subsidize_Code = sgc.Subsidize_Code          
        --			AND sgc.Subsidize_Code = s.Subsidize_Code          
        --			AND s.Subsidize_Item_Code = si.Subsidize_Item_Code             
        --	ORDER BY t.SP_ID asc , t.practice_display_seq asc, t.transaction_dtm ASC        
        --END  
        -----------------------------------    
        -- Result Table 6: Legend    
        -----------------------------------      

        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        VALUES
              ('Scheme Legend:', 
               ''
              );
        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        SELECT RTRIM(display_Code), 
               scheme_desc
        FROM SchemeClaim
        GROUP BY display_Code, 
                 scheme_desc, 
                 display_seq
        ORDER BY display_seq;

        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        VALUES
              ('', 
               ''
              );

        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        VALUES
              ('Subsidy Legend:', 
               ''
              );

        INSERT INTO @tempLegend
               (Parameter, 
                Value
               )
        SELECT DISTINCT
               (display_Code_for_Claim), 
               Legend_Desc_For_Claim
        FROM SubsidizeGroupClaim
        WHERE SubsidizeGroupClaim.Subsidize_Code IN
        (
            SELECT Subsidize.Subsidize_Code
            FROM Subsidize, 
                 SubsidizeItem
            WHERE subsidize_type = 'VACCINE'
                  AND Subsidize.subsidize_item_code = SubsidizeItem.subsidize_item_code
        )
        ORDER BY display_Code_for_Claim, 
                 Legend_Desc_For_Claim;

        -- =============================================        
        -- Return results        
        -- =============================================  
        -----------------------------------    
        --  Result Table 1: Content
        -----------------------------------  
        SELECT Result_Value1, 
               Result_Value2
        FROM @tempContent;

        -----------------------------------    
        -- Result Table 2:  Summary
        -----------------------------------  

        SELECT Parameter AS [Summary], 
               Value AS [ ]
        FROM @tempSummary;

        -----------------------------------    
        -- Result Table 3:  sheet 01
        -----------------------------------
        IF @Verification_Case_Available = 'Y'
            BEGIN
                IF @Reimbursement_Currency = 'HKDRMB'
                    BEGIN
                        SELECT SP_Item_No, 
                               SP_ID,
                               CASE ISNULL(SP_Name_Chi, '')
                                   WHEN ''
                                   THEN SP_Name
                                   ELSE SP_Name_Chi
                               END AS [Service Provider Name], 
                               Total_Transaction, 
                               Total_Amount, 
                               Total_Amount_RMB, 
                               RTRIM(Scheme_Code)
                        FROM #temp01
                        ORDER BY SP_Item_No ASC;
                    END;
                    ELSE
                    BEGIN
                        SELECT SP_Item_No, 
                               SP_ID, 
                               SP_Name, 
                               SP_Verification_Case, 
                               Total_Transaction, 
                               Total_Amount, 
                               RTRIM(Scheme_Code)
                        FROM #temp01
                        ORDER BY SP_Item_No ASC;
                    END;
            END;
            ELSE
            BEGIN
                SELECT @strReleasedMsg;
            END;

        -----------------------------------    
        -- Result Table 4:  sheet 02
        ----------------------------------- 
        IF @Verification_Case_Available = 'Y'
            BEGIN
                IF @Reimbursement_Currency = 'HKDRMB'
                    BEGIN
                        SELECT SP_ID_Practice,
                               CASE ISNULL(SP_Name_Chi, '')
                                   WHEN ''
                                   THEN SP_Name
                                   ELSE SP_Name_Chi
                               END AS [Service Provider Name],
                               CASE ISNULL(Practice_Name_Chi, '')
                                   WHEN ''
                                   THEN Practice_name
                                   ELSE Practice_Name_Chi
                               END AS [Practice Name], 
                               Bank_Acc_Holder, 
                               Bank_Account_No AS [Bank Account No.], 
                               Total_Amount, 
                               Total_Amount_RMB, 
                               RTRIM(Scheme_Code)
                        FROM #temp02
                        ORDER BY SP_ID ASC, 
                                 practice_display_seq ASC;
                    END;
                    ELSE
                    BEGIN
                        SELECT ROW_NUMBER() OVER(ORDER BY SP_ID, practice_display_seq ASC) AS [SP_Practice_Item_No],
							   SP_ID_Practice, 
                               SP_Name, 
                               Practice_Name, 
                               Bank_Acc_Holder, 
                               Bank_Account_No AS [Bank Account No.], 
                               Total_Amount, 
                               RTRIM(Scheme_Code),
							   SP_Verification_Case
                        FROM #temp02
                        ORDER BY SP_ID ASC, 
                                 practice_display_seq ASC;
                    END;
            END;
            ELSE
            BEGIN
                SELECT @strReleasedMsg;
            END;
        -----------------------------------    
        -- Result Table 5:  sheet 03
        -----------------------------------  
        IF @Verification_Case_Available = 'Y'
            BEGIN
                IF @Reimbursement_Currency = 'HKDRMB'
                    BEGIN
                        IF @subsidize_type_details = 'VACCINE'
                            BEGIN
                                IF @file_id = 'PreAuthorizationCheck'
                                    BEGIN
                                        SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)],
                                               CASE ISNULL(SP_Name_Chi, '')
                                                   WHEN ''
                                                   THEN SP_Name
                                                   ELSE SP_Name_Chi
                                               END AS [Service Provider Name],
                                               CASE ISNULL(Practice_Name_Chi, '')
                                                   WHEN ''
                                                   THEN Practice_name
                                                   ELSE Practice_Name_Chi
                                               END AS [Practice Name], 
                                               bank_acc_holder AS [Bank Account Name], 
                                               bank_account_no AS [Bank Account No.], 
                                               transaction_id AS [Transaction ID], 
                                               Total_Amount AS [Amount Claimed($)], 
                                               Total_Amount_RMB AS [Amount Claimed(RMB)], 
                                               ExchangeRate_Value AS [Exchange Rate], 
                                               transaction_dtm AS [Transaction Date], 
                                               RTRIM(Display_Code) AS [Scheme], 
                                               RTRIM(subsidize_display_Code) AS [Subsidy], -- display when @subsidize_type_details = 'vaccine'      
                                               RTRIM(RCH_Code) AS [RCH Code], -- display when @Scheme_Code = 'RVP'       
                                               RTRIM(RCH_Type) AS [RCH Type]    -- display when @Scheme_Code = 'RVP'       
                                        FROM #temp03
                                        ORDER BY SP_ID ASC, 
                                                 Practice_Display_Seq ASC, 
                                                 Transaction_dtm ASC;
                                    END;
                                    ELSE
                                    BEGIN
                                        SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)],
                                               CASE ISNULL(SP_Name_Chi, '')
                                                   WHEN ''
                                                   THEN SP_Name
                                                   ELSE SP_Name_Chi
                                               END AS [Service Provider Name],
                                               CASE ISNULL(Practice_Name_Chi, '')
                                                   WHEN ''
                                                   THEN Practice_name
                                                   ELSE Practice_Name_Chi
                                               END AS [Practice Name], 
                                               bank_acc_holder AS [Bank Account Name], 
                                               bank_account_no AS [Bank Account No.], 
                                               transaction_id AS [Transaction ID], 
                                               Total_Amount AS [Amount Claimed($)], 
                                               Total_Amount_RMB AS [Amount Claimed(RMB)], 
                                               ExchangeRate_Value AS [Exchange Rate], 
                                               transaction_dtm AS [Transaction Date], 
                                               RTRIM(Display_Code) AS [Scheme], 
                                               RTRIM(subsidize_display_Code) AS [Subsidy]  -- display when @subsidize_type_details = 'vaccine'      
                                        FROM #temp03
                                        ORDER BY SP_ID ASC, 
                                                 Practice_Display_Seq ASC, 
                                                 Transaction_dtm ASC;
                                    END;
                            END;
                            ELSE
                            BEGIN
                                SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)],
                                       CASE ISNULL(SP_Name_Chi, '')
                                           WHEN ''
                                           THEN SP_Name
                                           ELSE SP_Name_Chi
                                       END AS [Service Provider Name],
                                       CASE ISNULL(Practice_Name_Chi, '')
                                           WHEN ''
                                           THEN Practice_name
                                           ELSE Practice_Name_Chi
                                       END AS [Practice Name], 
                                       bank_acc_holder AS [Bank Account Name], 
                                       bank_account_no AS [Bank Account No.], 
                                       transaction_id AS [Transaction ID], 
                                       Total_Amount AS [Amount Claimed($)], 
                                       Total_Amount_RMB AS [Amount Claimed(RMB)], 
                                       ExchangeRate_Value AS [Exchange Rate], 
                                       transaction_dtm AS [Transaction Date], 
                                       RTRIM(Display_Code) AS [Scheme]
                                FROM #temp03
                                ORDER BY SP_ID ASC, 
                                         Practice_Display_Seq ASC, 
                                         Transaction_dtm ASC;
                            END;
                    END;
                    ELSE
                    BEGIN
                        IF @subsidize_type_details = 'VACCINE'
                            BEGIN
                                IF @file_id = 'PreAuthorizationCheck'
                                    BEGIN
                                        SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)], 
                                               SP_Name AS [Service Provider Name], 
                                               Practice_name AS [Practice Name], 
                                               bank_acc_holder AS [Bank Account Name], 
                                               bank_account_no AS [Bank Account No.], 
                                               transaction_id AS [Transaction ID], 
                                               Total_Amount AS [Amount Claimed($)], 
                                               transaction_dtm AS [Transaction Date], 
                                               RTRIM(Display_Code) AS [Scheme], 
                                               RTRIM(subsidize_display_Code) AS [Subsidy], -- display when @subsidize_type_details = 'vaccine'      
                                               RTRIM(RCH_Code) AS [RCH Code], -- display when @Scheme_Code = 'RVP'       
                                               RTRIM(RCH_Type) AS [RCH Type]    -- display when @Scheme_Code = 'RVP'       
                                        FROM #temp03
                                        ORDER BY SP_ID ASC, 
                                                 Practice_Display_Seq ASC, 
                                                 Transaction_dtm ASC;
                                    END;
                                    ELSE
                                    BEGIN
                                        SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)], 
                                               SP_Name AS [Service Provider Name], 
                                               Practice_name AS [Practice Name], 
                                               bank_acc_holder AS [Bank Account Name], 
                                               bank_account_no AS [Bank Account No.], 
                                               transaction_id AS [Transaction ID], 
                                               Total_Amount AS [Amount Claimed($)], 
                                               transaction_dtm AS [Transaction Date], 
                                               RTRIM(Display_Code) AS [Scheme], 
                                               RTRIM(subsidize_display_Code) AS [Subsidy]  -- display when @subsidize_type_details = 'vaccine'      
                                        FROM #temp03
                                        ORDER BY SP_ID ASC, 
                                                 Practice_Display_Seq ASC, 
                                                 Transaction_dtm ASC;
                                    END;
                            END;
                            ELSE
                            BEGIN
                                SELECT SP_ID_Practice AS [Service Provider ID (Practice Number)], 
                                       SP_Name AS [Service Provider Name], 
                                       Practice_name AS [Practice Name], 
                                       bank_acc_holder AS [Bank Account Name], 
                                       bank_account_no AS [Bank Account No.], 
                                       transaction_id AS [Transaction ID], 
                                       Total_Amount AS [Amount Claimed($)], 
                                       transaction_dtm AS [Transaction Date], 
                                       RTRIM(Display_Code) AS [Scheme]
                                FROM #temp03
                                ORDER BY SP_ID ASC, 
                                         Practice_Display_Seq ASC, 
                                         Transaction_dtm ASC;
                            END;
                    END;
            END;
            ELSE
            BEGIN
                SELECT @strReleasedMsg;
            END;

        -----------------------------------    
        -- Result Table 6: Legend    
        -----------------------------------      

        SELECT Parameter AS [Legend], 
               Value AS [ ]
        FROM @tempLegend;

        -- =============================================        
        -- House Keeping        
        -- =============================================     
        DROP TABLE #initialTransaction;
        DROP TABLE #temp01;
        DROP TABLE #temp02;
        DROP TABLE #temp03;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementFiles_get_byReimbID] TO HCVU;
GO