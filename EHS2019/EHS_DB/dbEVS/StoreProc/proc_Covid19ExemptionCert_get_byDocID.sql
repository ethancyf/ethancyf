
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_Covid19ExemptionCert_get_byDocID]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_Covid19ExemptionCert_get_byDocID];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- Modified by:		Winnie SUEN
-- Modified date:	06 Jan 2022
-- CR No.			CRE20-023-71 (COVID19 Exemption Cert Record) 
-- Description:		Get COVID19 Execption Certification by Doc No.
-- =============================================
--EXEC proc_Covid19ExemptionCert_get_byDocID 'EC','G9460283',NULL
CREATE PROCEDURE [dbo].[proc_Covid19ExemptionCert_get_byDocID]	@doc_code			CHAR(20), 
																@identity_no        VARCHAR(20), 
																@Adoption_Prefix_Num CHAR(7)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @l_doc_code CHAR(20)= @doc_code;
        DECLARE @l_identity_no1 VARCHAR(20)= LTRIM(RTRIM(@identity_no));
        DECLARE @l_identity_no2 VARCHAR(20);
        DECLARE @l_Adoption_Prefix_Num CHAR(7)= @Adoption_Prefix_Num;

        CREATE TABLE #Results
        (Transaction_ID      CHAR(20), 
         Doc_Code            CHAR(20), 
         Service_Receive_Dtm DATETIME, 
         Transaction_Dtm     DATETIME,
		 SP_ID				 CHAR(8),
		 Practice_Display_Seq	SMALLINT
        );

        DECLARE @OtherDoc_Code TABLE(Doc_Code CHAR(20));

        -- =============================================
        -- Initialization
        -- =============================================
        INSERT INTO @OtherDoc_Code(Doc_Code)
        VALUES(@l_doc_code);

        IF LTRIM(RTRIM(@l_doc_code)) IN('HKBC', 'EC', 'CCIC', 'ROP140')
            BEGIN
                INSERT INTO @OtherDoc_Code(Doc_Code)
            VALUES('HKIC');
            END;

        IF LTRIM(RTRIM(@l_doc_code)) IN('HKIC', 'EC', 'CCIC', 'ROP140')
            BEGIN
                INSERT INTO @OtherDoc_Code(Doc_Code)
            VALUES('HKBC');
            END;

        IF LTRIM(RTRIM(@l_doc_code)) IN('HKIC', 'HKBC', 'CCIC', 'ROP140')
            BEGIN
                INSERT INTO @OtherDoc_Code(Doc_Code)
            VALUES('EC');
            END;

        IF LTRIM(RTRIM(@l_doc_code)) IN('HKIC', 'HKBC', 'EC', 'ROP140')
            BEGIN
                INSERT INTO @OtherDoc_Code(Doc_Code)
            VALUES('CCIC');
            END;

        IF LTRIM(RTRIM(@l_doc_code)) IN('HKIC', 'HKBC', 'EC', 'CCIC')
            BEGIN
                INSERT INTO @OtherDoc_Code(Doc_Code)
            VALUES('ROP140');
            END;

        IF @l_identity_no1 IS NULL
            BEGIN
                SET @l_identity_no2 = NULL;
            END;
            ELSE
            BEGIN
                SET @l_identity_no2 = ' ' + @l_identity_no1;
            END;

        -- =============================================
        -- Return results
        -- =============================================
        EXEC [proc_SymmetricKey_open];

        INSERT INTO #Results
        SELECT vt.Transaction_ID, 
               vt.Doc_Code, 
               vt.Service_Receive_Dtm, 
               vt.Transaction_Dtm,
			   vt.SP_ID,
			   vt.Practice_Display_Seq
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'MEC'
             INNER JOIN PersonalInformation AS pinfo WITH(NOLOCK)
             ON vt.Voucher_Acc_ID = pinfo.Voucher_Acc_ID
                AND vt.Doc_Code = pinfo.Doc_Code
                AND (pinfo.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_identity_no1)
                     OR pinfo.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_identity_no2))
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
               vt.Transaction_Dtm,
			   vt.SP_ID,
			   vt.Practice_Display_Seq
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'MEC'
             INNER JOIN tempPersonalInformation AS tpi WITH(NOLOCK)
             ON vt.Temp_Voucher_Acc_ID = tpi.Voucher_Acc_ID AND vt.Voucher_Acc_ID = ''
                AND vt.Doc_Code = tpi.Doc_Code
                AND (tpi.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_identity_no1)
                     OR tpi.Encrypt_Field1 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_identity_no2))
                AND (@l_Adoption_Prefix_Num IS NULL
                     OR tpi.Encrypt_Field11 = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @l_Adoption_Prefix_Num))
             INNER JOIN @OtherDoc_Code AS oc
             ON vt.Doc_Code = oc.Doc_Code
        WHERE vt.Record_Status NOT IN('I', 'D')
             AND ISNULL(vt.Invalidation, '') <> 'I';


        SELECT vt.Transaction_ID, 
               vt.Doc_Code, 
               vt.Service_Receive_Dtm, 
               vt.Transaction_Dtm,
			   vt.SP_ID,			   
			   [SP_Name] = CONVERT(VARCHAR(MAX), DecryptByKey(SP.Encrypt_Field2)),  
			   [SP_Name_Chi] = CONVERT(NVARCHAR(MAX), DecryptByKey(SP.Encrypt_Field3)),  
			   [Practice_ID] = P.[Display_Seq],
			   P.Practice_Name,
			   P.Practice_Name_Chi,
			   [ValidUntil] = ValidUntil.AdditionalFieldValueCode,
			   [JoinEHRSS] = JoinEHRSS.AdditionalFieldValueCode,
			   [ContactNo] = ContactNo.AdditionalFieldValueCode

        FROM #Results vt
		INNER JOIN [ServiceProvider] SP WITH (NOLOCK)  
		ON VT.[SP_ID] = SP.[SP_ID]
		INNER JOIN [Practice] P WITH (NOLOCK)  
		ON VT.[SP_ID] = P.[SP_ID] AND VT.[Practice_Display_Seq] = P.[Display_Seq]  
		INNER JOIN TransactionAdditionalField AS ValidUntil WITH(NOLOCK)
		ON vt.Transaction_ID = ValidUntil.Transaction_ID AND ValidUntil.AdditionalFieldID = 'ValidUntil'
		INNER JOIN TransactionAdditionalField AS JoinEHRSS WITH(NOLOCK)
		ON vt.Transaction_ID = JoinEHRSS.Transaction_ID AND JoinEHRSS.AdditionalFieldID = 'JoinEHRSS'
		INNER JOIN TransactionAdditionalField AS ContactNo WITH(NOLOCK)
		ON vt.Transaction_ID = ContactNo.Transaction_ID AND ContactNo.AdditionalFieldID = 'ContactNo'

        ORDER BY Service_Receive_Dtm DESC, 
                CAST(ValidUntil.AdditionalFieldValueCode AS DATE) DESC,
				Transaction_Dtm DESC;

        EXEC [proc_SymmetricKey_close];

        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            BEGIN
                DROP TABLE #Results;
            END;
    END;  
GO

GRANT EXECUTE ON [dbo].[proc_Covid19ExemptionCert_get_byDocID] TO HCVU;
GRANT EXECUTE ON [dbo].[proc_Covid19ExemptionCert_get_byDocID] TO HCSP;
GO

