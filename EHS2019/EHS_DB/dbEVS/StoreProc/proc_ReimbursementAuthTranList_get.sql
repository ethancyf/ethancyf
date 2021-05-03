
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ReimbursementAuthTranList_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ReimbursementAuthTranList_get];
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
-- Modified by:		Martin Tang
-- Modified date:	10 Sep 2020
-- CR No.:			CRE20-003 (Break Down of Monthly Statement by Schools)
-- Description:		Add @School_Code
-- =============================================    
-- =============================================
-- Modification History
-- Modified by:		Koala CHENG
-- Modified date:	18 Sep 2018
-- CR No.:			CRE17-018 (New initiatives for VSS and RVP in 2018-19)
-- Description:		Support reimbursed transaction with temp voucher account
-- =============================================    
-- =============================================  
-- Modification History  
-- Modified by:		Koala CHENG
-- Modified date:	15 January 2018
-- CR No.:			I-CRE17-005
-- Description:		Performance Tuning
-- 					1. Add WITH (NOLOCK)
-- ============================================= 
-- =============================================  
-- Modification History  
-- CR No:			CRE11-024-02 HCVS Pilot Extension Part 2
-- Modified by:		Tony FUNG
-- Modified date:	18 November 2011  
-- Description:		Added SchemeCode_TransactionID to result set
-- =============================================  
-- =============================================  
-- Modification History  
-- CR No:			CRE11-024-02 HCVS Pilot Extension Part 2
-- Modified by:		Tony FUNG
-- Modified date:	11 November 2011  
-- Description:		Added DocCode_IdentityNum to result set
-- =============================================  
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	6 May 2010
-- Description:		(1) Fix the INNER JOIN on InvalidPersonalInformation
--					(2) Retrieve [Invalidation]
-- =============================================
-- =============================================
-- Author:			Billy Lam
-- Create date:		31 July 2008
-- Description:		Get the list of ReimbursementAuthTran by Reimburse_ID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark Yip
-- Modified date:	17 Dec 2008
-- Description:		1. Total amount will be calculated based on the Claim_Amount field
--					2. Join the VoucherAccount to make the query match the index
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	26 August 2009
-- Description:		Inner join [SchemeClaim] and [DocType]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	3 September 2009
-- Description:		Remove criteria [Scheme_Code] while inner joining [VoucherAccount]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 September 2009
-- Description:		Pre-calculate the total unit and total amount into the temporary table #TransactionGroupByTransactionID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	14 Sep 2009
-- Description:		Update the default sorting order to transaction_dtm
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	15 September 2009
-- Description:		Add criteria Doc_Code on inner joining PersonalInformation
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	17 September 2009
-- Description:		Add @Scheme_Code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	18 September 2009
-- Description:		Remove the field [Doc_Code] in temporary table #TransactionGroupByTransactionID
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 September 2009
-- Description:		Retrieve [PersonalInformation].[Encrypt_Field11]
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Clark YIP
-- Modified date:	24 September 2009
-- Description:		Handle Special account and invalid account
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	4 October 2009
-- Description:		Handle expired scheme
-- =============================================
CREATE PROCEDURE [dbo].[proc_ReimbursementAuthTranList_get] @SP_ID                CHAR(8), 
                                                            @Practice_Display_Seq SMALLINT, 
                                                            @Reimburse_ID         VARCHAR(20), 
                                                            @Scheme_Code          CHAR(10), 
                                                            @School_Code          VARCHAR(50)
AS
    BEGIN
        -- =============================================
        -- Declaration
        -- =============================================
        CREATE TABLE #TransactionGroupByTransactionID
        (Transaction_ID        CHAR(20), 
         TotalUnit             INT, 
         TotalAmount           MONEY, 
         Authorised_Cutoff_Dtm DATETIME
        );

        CREATE TABLE #TranList
        (Transaction_ID      CHAR(20), 
         Transaction_Dtm     DATETIME, 
         Service_Receive_Dtm DATETIME, 
         Scheme_Code         CHAR(10), 
         Doc_Code            CHAR(10), 
         Encrypt_Field1      VARBINARY(100), 
         Encrypt_Field2      VARBINARY(100), 
         Encrypt_Field3      VARBINARY(100), 
         Encrypt_Field11     VARBINARY(100), 
         TotalUnit           INT, 
         TotalAmount         MONEY, 
         Invalidation        CHAR(1)
        );

        CREATE TABLE #EffectiveScheme
        (Scheme_Code CHAR(10), 
         Scheme_Seq  SMALLINT
        );

        -- =============================================
        -- Initialization
        -- =============================================
		-- Transaction
        INSERT INTO #TransactionGroupByTransactionID
               (Transaction_ID, 
                TotalUnit, 
                TotalAmount, 
                Authorised_Cutoff_Dtm
               )
        SELECT VT.Transaction_ID, 
               SUM(TD.Unit) AS [TotalUnit], 
               SUM(TD.Total_Amount) AS [TotalAmount], 
               MIN(RAT.Authorised_Cutoff_Dtm) AS [Authorised_Cutoff_Dtm]
        FROM VoucherTransaction AS VT WITH(NOLOCK)
             INNER JOIN TransactionDetail AS TD WITH(NOLOCK)
             ON VT.Transaction_ID = TD.Transaction_ID
             INNER JOIN ReimbursementAuthTran AS RAT WITH(NOLOCK)
             ON VT.Transaction_ID = RAT.Transaction_ID
             LEFT OUTER JOIN TransactionAdditionalField AS taf
             ON vt.Transaction_ID = taf.Transaction_ID
                AND taf.AdditionalFieldID = 'SchoolCode'
        WHERE VT.SP_ID = @SP_ID
              AND VT.Practice_Display_Seq = @Practice_Display_Seq
              AND RAT.Reimburse_ID = @Reimburse_ID
              AND VT.Scheme_Code = @Scheme_Code
              AND (@School_Code IS NULL
                   OR taf.AdditionalFieldValueCode = @School_Code)
        GROUP BY VT.Transaction_ID;

		-- Scheme
        INSERT INTO #EffectiveScheme
               (Scheme_Code, 
                Scheme_Seq
               )
        SELECT Scheme_Code, 
               MAX(Scheme_Seq)
        FROM SchemeClaim WITH(NOLOCK)
        WHERE GETDATE() >= Effective_Dtm
        GROUP BY Scheme_Code;

        -- Validated Account
        INSERT INTO #TranList
               (Transaction_ID, 
                Transaction_Dtm, 
                Service_Receive_Dtm, 
                Scheme_Code, 
                Doc_Code, 
                Encrypt_Field1, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                Encrypt_Field11, 
                TotalUnit, 
                TotalAmount, 
                Invalidation
               )
        SELECT T.Transaction_ID, 
               VT.Transaction_Dtm, 
               VT.Service_Receive_Dtm, 
               VT.Scheme_Code, 
               P.Doc_Code, 
               P.Encrypt_Field1, 
               P.Encrypt_Field2, 
               P.Encrypt_Field3, 
               P.Encrypt_Field11, 
               T.TotalUnit AS [Total_Unit], 
               T.TotalAmount AS [Total_Amount], 
               VT.Invalidation
        FROM #TransactionGroupByTransactionID AS T
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON T.Transaction_ID = VT.Transaction_ID
             INNER JOIN VoucherAccount AS VA WITH(NOLOCK)
             ON VT.Voucher_Acc_ID = VA.Voucher_Acc_ID
             INNER JOIN PersonalInformation AS P WITH(NOLOCK)
             ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID
                AND VT.Doc_Code = P.Doc_Code
        WHERE VT.Voucher_Acc_ID <> '';

        -- Temp Account
        INSERT INTO #TranList
               (Transaction_ID, 
                Transaction_Dtm, 
                Service_Receive_Dtm, 
                Scheme_Code, 
                Doc_Code, 
                Encrypt_Field1, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                Encrypt_Field11, 
                TotalUnit, 
                TotalAmount, 
                Invalidation
               )
        SELECT T.Transaction_ID, 
               VT.Transaction_Dtm, 
               VT.Service_Receive_Dtm, 
               VT.Scheme_Code, 
               P.Doc_Code, 
               P.Encrypt_Field1, 
               P.Encrypt_Field2, 
               P.Encrypt_Field3, 
               P.Encrypt_Field11, 
               T.TotalUnit AS [Total_Unit], 
               T.TotalAmount AS [Total_Amount], 
               VT.Invalidation
        FROM #TransactionGroupByTransactionID AS T
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON T.Transaction_ID = VT.Transaction_ID
             INNER JOIN TempVoucherAccount AS VA WITH(NOLOCK)
             ON VT.Temp_Voucher_Acc_ID = VA.Voucher_Acc_ID
             INNER JOIN TempPersonalInformation AS P WITH(NOLOCK)
             ON VA.Voucher_Acc_ID = P.Voucher_Acc_ID
                AND VT.Doc_Code = P.Doc_Code
        WHERE VT.Voucher_Acc_ID = ''
              AND ISNULL(VT.Special_Acc_ID, '') = ''
              AND VT.Invalid_Acc_ID IS NULL;

        -- Special Account
        INSERT INTO #TranList
               (Transaction_ID, 
                Transaction_Dtm, 
                Service_Receive_Dtm, 
                Scheme_Code, 
                Doc_Code, 
                Encrypt_Field1, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                Encrypt_Field11, 
                TotalUnit, 
                TotalAmount, 
                Invalidation
               )
        SELECT T.Transaction_ID, 
               VT.Transaction_Dtm, 
               VT.Service_Receive_Dtm, 
               VT.Scheme_Code, 
               P.Doc_Code, 
               P.Encrypt_Field1, 
               P.Encrypt_Field2, 
               P.Encrypt_Field3, 
               P.Encrypt_Field11, 
               T.TotalUnit AS [Total_Unit], 
               T.TotalAmount AS [Total_Amount], 
               VT.Invalidation
        FROM #TransactionGroupByTransactionID AS T
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON T.Transaction_ID = VT.Transaction_ID
             INNER JOIN SpecialAccount AS VA WITH(NOLOCK)
             ON VT.Special_Acc_ID = VA.Special_Acc_ID
             INNER JOIN SpecialPersonalInformation AS P WITH(NOLOCK)
             ON VA.Special_Acc_ID = P.Special_Acc_ID
                AND VT.Doc_Code = P.Doc_Code
        WHERE VT.Voucher_Acc_ID = ''
              AND VT.Special_Acc_ID IS NOT NULL
              AND VT.Invalid_Acc_ID IS NULL;

        -- Invalid Account
        INSERT INTO #TranList
               (Transaction_ID, 
                Transaction_Dtm, 
                Service_Receive_Dtm, 
                Scheme_Code, 
                Doc_Code, 
                Encrypt_Field1, 
                Encrypt_Field2, 
                Encrypt_Field3, 
                Encrypt_Field11, 
                TotalUnit, 
                TotalAmount, 
                Invalidation
               )
        SELECT T.Transaction_ID, 
               VT.Transaction_Dtm, 
               VT.Service_Receive_Dtm, 
               VT.Scheme_Code, 
               P.Doc_Code, 
               P.Encrypt_Field1, 
               P.Encrypt_Field2, 
               P.Encrypt_Field3, 
               P.Encrypt_Field11, 
               T.TotalUnit AS [Total_Unit], 
               T.TotalAmount AS [Total_Amount], 
               VT.Invalidation
        FROM #TransactionGroupByTransactionID AS T
             INNER JOIN VoucherTransaction AS VT WITH(NOLOCK)
             ON T.Transaction_ID = VT.Transaction_ID
             INNER JOIN InvalidAccount AS VA WITH(NOLOCK)
             ON VT.Invalid_Acc_ID = VA.Invalid_Acc_ID
             INNER JOIN InvalidPersonalInformation AS P WITH(NOLOCK)
             ON VA.Invalid_Acc_ID = P.Invalid_Acc_ID
        WHERE VT.Voucher_Acc_ID = ''
              AND VT.Invalid_Acc_ID IS NOT NULL;

		-- =============================================
        -- Return results
        -- =============================================
        EXEC [proc_SymmetricKey_open]

        SELECT T.Transaction_ID, 
               T.Transaction_Dtm, 
               T.Service_Receive_Dtm, 
               T.Scheme_Code, 
               SC.Display_Code, 
               T.Doc_Code, 
               DT.Doc_Display_Code, 
               CONVERT(CHAR, DECRYPTBYKEY(T.Encrypt_Field1)) AS [IDNo], 
               CONVERT(VARCHAR(100), DECRYPTBYKEY(T.Encrypt_Field2)) AS [Eng_Name], 
               CONVERT(NVARCHAR, DECRYPTBYKEY(T.Encrypt_Field3)) AS [Chi_Name], 
               CONVERT(CHAR, DECRYPTBYKEY(T.Encrypt_Field11)) AS [IDNo2], 
               T.TotalUnit AS [Total_Unit], 
               T.TotalAmount AS [Total_Amount], 
               T.Invalidation, 
               LEFT(DT.doc_display_code + SPACE(20), 20) + CONVERT(VARCHAR, DECRYPTBYKEY(T.Encrypt_Field1)) AS DocCode_IdentityNum, 
               T.Scheme_Code + ' ' + T.Transaction_ID AS SchemeCode_TransactionID		-- CRE11-024-02
        FROM #TranList AS T
             INNER JOIN #EffectiveScheme AS ES
             ON T.Scheme_Code = ES.Scheme_Code
             INNER JOIN SchemeClaim AS SC WITH(NOLOCK)
             ON ES.Scheme_Code = SC.Scheme_Code
                AND ES.Scheme_Seq = SC.Scheme_Seq
             INNER JOIN DocType AS DT WITH(NOLOCK)
             ON T.Doc_Code = DT.Doc_Code
        ORDER BY T.Transaction_Dtm;

        EXEC [proc_SymmetricKey_close]

        DROP TABLE #TranList;
        DROP TABLE #TransactionGroupByTransactionID;
        DROP TABLE #EffectiveScheme;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_ReimbursementAuthTranList_get] TO HCSP;
GO