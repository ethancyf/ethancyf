
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (Immu record)
-- Modified by:		Martin Tang	
-- Modified date:	25 May 2021
-- Description:		performance tuning
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023 (Immu record)
-- Modified by:		Winnie SUEN
-- Modified date:	11 Mar 2021
-- Description:		Retrieve the list pending for COVID19BatchConfirm
-- =============================================

CREATE PROCEDURE [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm] @Transaction_Dtm DATETIME
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  
        DECLARE @Performance_Start_Dtm DATETIME;
        SET @Performance_Start_Dtm = GETDATE();

        DECLARE @intMaxRecods AS INT;
        SELECT @intMaxRecods = Parm_Value1
        FROM SystemParameters
        WHERE Parameter_Name = 'COVID19BatchConfirmSize';

        DECLARE @In_Transaction_Dtm DATETIME;

        SET @In_Transaction_Dtm = @Transaction_Dtm;

        -- 
        CREATE TABLE #VoucherAccTransaction
        (Transaction_ID        CHAR(20), 
         Transaction_Dtm       DATETIME, 
         SP_ID                 VARCHAR(8), 
         Voucher_Acc_ID        CHAR(15), 
         Temp_Voucher_Acc_ID   CHAR(15), 
         Record_Status         CHAR(1), 
         TSMP                  BINARY(8), 
         Voucher_Acc_TSMP      BINARY(8), 
         Scheme_Code           CHAR(25), 
         Doc_Code              CHAR(20), 
         DOB                   DATETIME, 
         Exact_DOB             CHAR(1), 
         Date_Of_Issue         DATETIME,

         -- For Temp Account
         original_amend_acc_id CHAR(15), 
         original_TSMP         BINARY(8), 
         Validated_Acc_ID      CHAR(15), 
         original_DOI          DATETIME, 
         original_DOB          DATETIME, 
         original_Exact_DOB    CHAR(1), 
         Send_To_ImmD          CHAR(1), 
         Account_Purpose       CHAR(1)
        );

        -- =============================================  
        -- Initization  
        -- =============================================  

        SELECT @In_Transaction_Dtm = DATEADD(day, 1, @In_Transaction_Dtm);

        -- Handle Temp Account Firstly

        INSERT INTO #VoucherAccTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                SP_ID, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Record_Status, 
                TSMP, 
                Voucher_Acc_TSMP, 
                Scheme_Code, 
                Doc_Code, 
                DOB, 
                Exact_DOB, 
                Date_Of_Issue, 
                original_amend_acc_id, 
                Validated_Acc_ID, 
                Account_Purpose, 
                Send_To_ImmD
               )
        SELECT v.Transaction_ID, 
               v.Transaction_Dtm, 
               v.SP_ID, 
               '', 
               v.Temp_Voucher_Acc_ID, 
               v.Record_Status, 
               v.tsmp, 
               t.tsmp, 
               v.scheme_code, 
               p.doc_code, 
               p.DOB, 
               p.Exact_DOB, 
               p.Date_Of_Issue, 
               t.original_amend_acc_id, 
               t.Validated_Acc_ID, 
               t.Account_Purpose,
               CASE
                   WHEN original_amend_acc_id IS NULL
                   THEN 'X'
                   ELSE NULL
               END
        FROM VoucherTransaction AS v
             INNER JOIN TempPersonalInformation AS p
             ON v.Temp_Voucher_Acc_ID = p.Voucher_Acc_ID
             INNER JOIN TempVoucherAccount AS t
             ON v.Temp_Voucher_Acc_ID = t.Voucher_Acc_ID
        WHERE v.Scheme_Code = 'COVID19CVC'
              AND v.Record_Status = 'P'	-- Pending For Confirmation              
              AND v.Voucher_Acc_ID = '';

        UPDATE #VoucherAccTransaction
          SET original_TSMP = t.TSMP, 
              original_DOI = p.date_of_issue, 
              original_DOB = p.DOB, 
              original_Exact_DOB = p.exact_dob
        FROM #VoucherAccTransaction vt
             INNER JOIN tempvoucheraccount t
             ON vt.original_amend_acc_id = t.voucher_acc_id
                AND t.account_purpose = 'O'
             INNER JOIN temppersonalinformation p
             ON t.voucher_acc_id = p.voucher_acc_id
        WHERE vt.original_amend_acc_id IS NOT NULL;

        --UPDATE #VoucherAccTransaction
        --  SET Send_To_ImmD = 'X'
        --WHERE original_amend_acc_id IS NULL;

        UPDATE #VoucherAccTransaction
          SET Send_To_ImmD = 'N'
        WHERE Date_Of_Issue = Original_DOI
              AND DOB = Original_DOB
              AND Exact_DOB = original_Exact_DOB
              AND Original_DOI IS NOT NULL
              AND Original_DOB IS NOT NULL
              AND original_Exact_DOB IS NOT NULL
              AND original_amend_acc_id IS NOT NULL;

        UPDATE #VoucherAccTransaction
          SET Send_To_ImmD = 'Y'
        WHERE Send_To_ImmD IS NULL;

        -- Validated Account

        INSERT INTO #VoucherAccTransaction
               (Transaction_ID, 
                Transaction_Dtm, 
                SP_ID, 
                Voucher_Acc_ID, 
                Temp_Voucher_Acc_ID, 
                Record_Status, 
                TSMP, 
                Voucher_Acc_TSMP, 
                Scheme_Code, 
                Doc_Code, 
                Send_To_ImmD
               )
        SELECT v.Transaction_ID, 
               v.Transaction_Dtm, 
               v.SP_ID, 
               v.Voucher_Acc_ID, 
               '', 
               v.Record_Status, 
               v.tsmp, 
               NULL, 
               v.scheme_code, 
               p.doc_code, 
               'X'
        FROM VoucherTransaction AS v
             INNER JOIN PersonalInformation AS p
             ON v.Voucher_Acc_ID = p.Voucher_Acc_ID
                AND v.doc_code = p.doc_code
        WHERE v.Scheme_Code = 'COVID19CVC'
              AND v.Record_Status = 'P';	-- Pending For Confirmation
        -- =============================================  
        -- Return results  
        -- =============================================  
        SELECT TOP (@intMaxRecods) v.Transaction_ID, 
                                   v.Transaction_Dtm, 
                                   v.SP_ID, 
                                   v.Voucher_Acc_ID, 
                                   v.Temp_Voucher_Acc_ID, 
                                   v.Record_Status, 
                                   v.TSMP, 
                                   v.Voucher_Acc_TSMP, 
                                   v.Doc_Code, 
                                   v.original_amend_acc_id, 
                                   v.original_TSMP, 
                                   v.Validated_Acc_ID, 
                                   v.Send_To_ImmD, 
                                   v.Account_Purpose
        FROM #VoucherAccTransaction AS v
        WHERE v.Transaction_Dtm < @In_Transaction_Dtm
        ORDER BY 
        -- Priority: Amended Account > Other account
        CASE v.Account_Purpose
            WHEN 'A'
            THEN 1
            ELSE 99
        END ASC, 
        v.Transaction_Dtm ASC;

        --

        IF
          (
              SELECT Parm_Value1
              FROM SystemParameters
              WHERE Parameter_Name = 'EnableSProcPerformCapture'
                    AND Scheme_Code = 'ALL'
           ) = 'Y'
            BEGIN
                DECLARE @Performance_End_Dtm DATETIME;
                SET @Performance_End_Dtm = GETDATE();
                DECLARE @Parameter VARCHAR(255);
                SET @Parameter = ISNULL(CONVERT(VARCHAR, @In_Transaction_Dtm, 120), '');

                EXEC proc_SProcPerformance_add 'proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm', 
                                               @Parameter, 
                                               @Performance_Start_Dtm, 
                                               @Performance_End_Dtm;
            END;

		-- 
        IF OBJECT_ID('tempdb..#VoucherAccTransaction') IS NOT NULL
            BEGIN
                DROP TABLE #VoucherAccTransaction;
            END;

    END;
GO

GRANT EXECUTE ON [dbo].[proc_VoucherTransactionConfirm_get_For_COVID19BatchConfirm] TO HCSP, HCVU, WSEXT;
GO