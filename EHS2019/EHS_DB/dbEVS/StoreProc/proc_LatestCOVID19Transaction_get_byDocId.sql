
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_LatestCOVID19Transaction_get_byDocId]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_LatestCOVID19Transaction_get_byDocId];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- Modified by:		Martin Tang
-- Modified date:	28 Jan 2021
-- CR No.			CRE20-0XX (Immu record) 
-- Description:		
-- =============================================

CREATE PROCEDURE [dbo].[proc_LatestCOVID19Transaction_get_byDocId] @doc_code            CHAR(20), 
                                                                   @identity_no1        VARCHAR(20), 
                                                                   @Adoption_Prefix_Num CHAR(7)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_doc_code CHAR(20)= @doc_code;
        DECLARE @l_identity_no1 VARCHAR(20)= RTRIM(LTRIM(@identity_no1));
        DECLARE @l_Adoption_Prefix_Num CHAR(7)= @Adoption_Prefix_Num;

        CREATE TABLE #Results
        (Transaction_ID      CHAR(20), 
         Doc_Code            CHAR(20), 
         Service_Receive_Dtm DATETIME, 
         Transaction_Dtm     DATETIME
        );

        DECLARE @OtherDoc_Code TABLE(Doc_Code CHAR(20));

        -- =============================================
        -- Initialization
        -- =============================================
        INSERT INTO @OtherDoc_Code(Doc_Code)
        VALUES(@l_doc_code);
		
		IF LTRIM(RTRIM(@l_doc_code)) IN ('HKBC','EC','CCIC','ROP140')
		BEGIN
			INSERT INTO @OtherDoc_Code(Doc_Code) VALUES('HKIC');
		END;

		IF LTRIM(RTRIM(@l_doc_code)) IN ('HKIC','EC','CCIC','ROP140')
		BEGIN
			INSERT INTO @OtherDoc_Code(Doc_Code) VALUES('HKBC');
		END;

		IF LTRIM(RTRIM(@l_doc_code)) IN ('HKIC','HKBC','CCIC','ROP140')
		BEGIN
			INSERT INTO @OtherDoc_Code(Doc_Code) VALUES('EC');
		END;

		IF LTRIM(RTRIM(@l_doc_code)) IN ('HKIC','HKBC','EC','ROP140')
		BEGIN
			INSERT INTO @OtherDoc_Code(Doc_Code) VALUES('CCIC');
		END;

		IF LTRIM(RTRIM(@l_doc_code)) IN ('HKIC','HKBC','EC','CCIC')
		BEGIN
			INSERT INTO @OtherDoc_Code(Doc_Code) VALUES('ROP140');
		END;


        -- =============================================
        -- Return results
        -- =============================================
        EXEC [proc_SymmetricKey_open];

        INSERT INTO #Results
        SELECT vt.Transaction_ID, 
               vt.Doc_Code, 
               vt.Service_Receive_Dtm, 
               vt.Transaction_Dtm
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             INNER JOIN PersonalInformation AS pinfo WITH(NOLOCK)
             ON vt.Voucher_Acc_ID = pinfo.Voucher_Acc_ID
                AND vt.Doc_Code = pinfo.Doc_Code
                AND RTRIM(LTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field1)))) = @l_identity_no1
                AND (@l_Adoption_Prefix_Num IS NULL
                     OR pinfo.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_Adoption_Prefix_Num))
             INNER JOIN @OtherDoc_Code AS oc
             ON vt.Doc_Code = oc.Doc_Code
        WHERE vt.Record_Status NOT IN('I', 'D')
             AND ISNULL(vt.Invalidation, '') <> 'I';

        INSERT INTO #Results
        SELECT vt.Transaction_ID, 
               vt.Doc_Code, 
               vt.Service_Receive_Dtm, 
               vt.Transaction_Dtm
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             INNER JOIN tempPersonalInformation AS tpi WITH(NOLOCK)
             ON vt.Temp_Voucher_Acc_ID = tpi.Voucher_Acc_ID
                AND vt.Doc_Code = tpi.Doc_Code
                AND RTRIM(LTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field1)))) = @l_identity_no1
                AND (@l_Adoption_Prefix_Num IS NULL
                     OR tpi.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_Adoption_Prefix_Num))
             INNER JOIN @OtherDoc_Code AS oc
             ON vt.Doc_Code = oc.Doc_Code
        WHERE vt.Record_Status NOT IN('I', 'D')
             AND ISNULL(vt.Invalidation, '') <> 'I';

        --return the latest record
        SELECT TOP 1 Transaction_ID, 
                     Doc_Code
        FROM #Results
        ORDER BY Service_Receive_Dtm DESC, 
                 Transaction_Dtm DESC;

        EXEC [proc_SymmetricKey_close];

        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            BEGIN
                DROP TABLE #Results;
            END;
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_LatestCOVID19Transaction_get_byDocId] TO HCVU;
GO