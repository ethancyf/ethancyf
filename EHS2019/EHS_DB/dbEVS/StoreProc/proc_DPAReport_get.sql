
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_DPAReport_get]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_DPAReport_get];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	03 Nov 2020
-- CR No.:			CRE20-015 
-- Description:		special Support Scheme
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
-- Modification History
-- Modified by:		Lawrence TSANG
-- Modified date:	24 March 2015
-- CR No.:			INT15-0002
-- Description:		Set the stored procedure to recompile each time
-- =============================================
-- =============================================
-- Author:			Clark Yip
-- Create date:		29 Jul 2008
-- Description:		Get DPA report source (ych480:Encryption)
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   02 Dec 2008
-- Description:	    Total amount will be calculated based on the Claim_Amount field
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   26 Feb 2009
-- Description:	    The bank account will be selected from transaction table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   14 Aug 2009
-- Description:	    Add the Scheme code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Clark Yip
-- Modified date:   21 Aug 2009
-- Description:	    Output the display_code instead of Scheme_code
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   4 September 2009
-- Description:	    (1) Reformat the code
--					(2) Pre-calculate the total unit and total amount in @TransactionSum
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   23 September 2009
-- Description:	    Retrieve [SchemeClaim].[Scheme_Code] in second table
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Lawrence TSANG
-- Modified date:   29 September 2009
-- Description:	    Handle expired scheme
-- =============================================
-- =============================================
-- Modification History
-- Modified by:	    Tommy LAM
-- Modified date:   25 Oct 2012
-- Description:	    Add 2 Columns - "[Profession] and [total_trans]" to Output
-- =============================================

CREATE PROCEDURE [dbo].[proc_DPAReport_get] @reimburse_id    CHAR(15), 
                                            @cutoff_Date_str CHAR(11), 
                                            @scheme_code     CHAR(10), 
                                            @User_ID         CHAR(20)
WITH RECOMPILE
AS
    BEGIN
        -- =============================================
        -- Declaration
        -- =============================================
        DECLARE @TransactionSum AS TABLE
        (Transaction_ID   CHAR(20), 
         Total_Unit       INT, 
         Total_Amount     MONEY, 
         Total_Amount_RMB MONEY, 
         Total_SupportFee MONEY
        );

        DECLARE @EffectiveScheme TABLE
        (Scheme_Code CHAR(10), 
         Scheme_Seq  SMALLINT
        );

        DECLARE @lsUser_ID CHAR(20)= @User_ID;
        DECLARE @ReimbursementCurrency VARCHAR(10);
        DECLARE @UserName VARCHAR(80);

        -- =============================================
        -- Validation 
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================
        SELECT @ReimbursementCurrency = Reimbursement_Currency
        FROM SchemeClaim
        WHERE Scheme_Code = @scheme_code;

        EXEC [proc_SymmetricKey_open]

        SELECT @UserName = CONVERT(VARCHAR(MAX), DECRYPTBYKEY(Encrypt_Field2))
        FROM HCVUUserAC
        WHERE User_ID = @lsUser_ID;

        INSERT INTO @TransactionSum
               (Transaction_ID, 
                Total_Unit, 
                Total_Amount, 
                Total_Amount_RMB, 
                Total_SupportFee
               )
        SELECT VT.Transaction_ID, 
               SUM(TD.Unit), 
               SUM(TD.Total_Amount), 
               SUM(TD.Total_Amount_RMB), 
               SUM((ISNULL(CAST(TAF.AdditionalFieldValueCode AS MONEY), 0)))
        FROM VoucherTransaction AS VT
             INNER JOIN TransactionDetail AS TD
             ON VT.Transaction_ID = TD.Transaction_ID
             INNER JOIN ReimbursementAuthTran AS RAT
             ON VT.Transaction_ID = RAT.Transaction_ID
             INNER JOIN ReimbursementAuthorisation AS RA
             ON RAT.Reimburse_ID = RA.Reimburse_ID
                AND RAT.Scheme_Code = RA.Scheme_Code
                AND RAT.Authorised_Status = RA.Authorised_Status
             LEFT OUTER JOIN TransactionAdditionalField AS TAF
             ON vt.Transaction_ID = taf.Transaction_ID
                AND taf.AdditionalFieldID = 'TotalSupportFee'
        WHERE RAT.Reimburse_ID = @reimburse_id
              AND RAT.Scheme_Code = @scheme_code
              AND RA.Record_Status = 'A'
        GROUP BY VT.Transaction_ID;

        INSERT INTO @EffectiveScheme
               (Scheme_Code, 
                Scheme_Seq
               )
        SELECT Scheme_Code, 
               MAX(Scheme_Seq)
        FROM SchemeClaim
        WHERE GETDATE() >= Effective_Dtm
        GROUP BY Scheme_Code;
        -- =============================================
        -- Return results
        -- =============================================

        SELECT CAST(B.SP_ID AS VARCHAR) + ' (' + CAST(B.SP_Practice_Display_Seq AS VARCHAR) + ')' AS [SP_ID_PRACTICE], 
               B.Display_Seq AS [Bank_Acc_Display_Seq],
               CASE @ReimbursementCurrency
                   WHEN 'HKDRMB'
                   THEN CASE ISNULL(CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3)), '')
                            WHEN ''
                            THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                            ELSE CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3))
                        END
                   WHEN 'RMB'
                   THEN CASE ISNULL(CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3)), '')
                            WHEN ''
                            THEN CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
                            ELSE CONVERT(NVARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field3))
                        END
                   ELSE CONVERT(VARCHAR(40), DECRYPTBYKEY(SP.Encrypt_Field2))
               END AS [SP_Name], 
               PRO.Service_Category_Code AS [Profession], 
               B.Bank_Account_No, 
               B.Bank_Acc_Holder, 
               COUNT(TS.Transaction_ID) AS [total_trans], 
               SUM(TS.Total_Unit) AS [voucher_claim], 
               SUM(TS.Total_Amount) AS [total_amount], 
               SUM(TS.Total_Amount_RMB) AS [total_amount_rmb], 
               SUM(TS.Total_SupportFee) AS [total_support_fee], 
               SUM(TS.Total_SupportFee) - SUM(TS.Total_Amount_RMB) AS [total_reduction_fee], 
               RA.Authorised_Dtm AS [Report_Date], 
               p.Practice_Name_Chi
        FROM @TransactionSum AS TS
             INNER JOIN VoucherTransaction AS T
             ON TS.Transaction_ID = T.Transaction_ID
             INNER JOIN ServiceProvider AS SP
             ON T.SP_ID = SP.SP_ID
             INNER JOIN Practice AS P
             ON T.SP_ID = P.SP_ID
                AND T.Practice_Display_Seq = P.Display_Seq
             INNER JOIN Professional AS PRO
             ON P.SP_ID = PRO.SP_ID
                AND P.Professional_Seq = PRO.Professional_Seq
             INNER JOIN BankAccount AS B
             ON T.SP_ID = B.SP_ID
                AND T.Practice_Display_Seq = B.SP_Practice_Display_Seq
                AND T.Bank_Acc_Display_Seq = B.Display_Seq
             INNER JOIN ReimbursementAuthTran AS RT
             ON T.Transaction_ID = RT.Transaction_ID
             INNER JOIN ReimbursementAuthorisation AS RA
             ON RT.Reimburse_ID = RA.Reimburse_ID
                AND RT.Scheme_Code = RA.Scheme_Code
                AND RT.Authorised_Status = RA.Authorised_Status
        WHERE RA.Record_Status = 'A'
        GROUP BY B.SP_ID, 
                 B.SP_Practice_Display_Seq, 
                 B.Display_Seq, 
                 SP.Encrypt_Field2, 
                 SP.Encrypt_Field3, 
                 PRO.Service_Category_Code, 
                 B.Bank_Account_No, 
                 B.Bank_Acc_Holder, 
                 RA.Authorised_Dtm, 
                 p.Practice_Name_Chi
        ORDER BY B.SP_ID ASC, 
                 B.SP_Practice_Display_Seq ASC, 
                 B.Display_Seq ASC;

        EXEC [proc_SymmetricKey_close]

        SELECT @cutoff_Date_str AS [CutoffDate], 
               @reimburse_id AS [ReimburseID], 
               GETDATE() AS [GenerateDate], 
               SC.Display_Code AS [SchemeCode], 
               ES.Scheme_Code, 
               @UserName AS UserName
        FROM @EffectiveScheme AS ES
             INNER JOIN SchemeClaim AS SC
             ON ES.Scheme_Code = SC.Scheme_Code
                AND ES.Scheme_Seq = SC.Scheme_Seq
        WHERE ES.Scheme_Code = @scheme_code;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_DPAReport_get] TO HCVU;
GO