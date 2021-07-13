
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19ExporterQueue_add]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19ExporterQueue_add];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-54
-- Modified by:		Martin Tang
-- Modified date:	07 July 2021
-- Description:		1. Add "DS" doc type
--                  2. Fix Bug for sending the records with the latest personal information
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-53
-- Modified by:		Martin Tang
-- Modified date:	29 Jun 2021
-- Description:		1. Add 9 new doc types
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-50 
-- Modified by:		Martin Tang
-- Modified date:	4 Jun 2021
-- Description:		1. Send the records with the latest personal information
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	1 Jun 2021
-- Description:		1. Revise Reserved field 10 for [Issuing Country/Region]
--					2. add PASS_Issue_Region in COVID19ExporterQueue
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	26 May 2021
-- Description:		1. Handle VSS Outreach Case
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	21 May 2021
-- Description:		1. Fix bug (del case)
--					2. Apply Doc_Type = 9 for Two-Way
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-023-43
-- Modified by:		Winnie SUEN
-- Modified date:	18 May 2021
-- Description:		1. Handle scheme [COVID19SB]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-40 
-- Modified by:		Martin Tang
-- Modified date:	13 May 2021
-- Description:		1. Fix double quote in Remark
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 
-- Modified by:		Martin Tang
-- Modified date:	27 Apr 2021
-- Description:		1. Performance turning 
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT20-0088
-- Modified by:		Winnie SUEN
-- Modified date:	23 Apr 2021
-- Description:		1. Fix duplicate sub category code
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-29 (RVP support staff)
-- Modified by:		Winnie SUEN
-- Modified date:	13 Apr 2021
-- Description:		1. Send [Phone_no] to Central DB for [RVP] scheme
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-27 (COVID19RVP support HCW)
-- Modified by:		Winnie SUEN
-- Modified date:	08 Apr 2021
-- Description:		1. Send [Phone_no] to Central DB for [COVID19RVP] scheme
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-24 (COVID19SR Claim)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Mar 2021
-- Description:		1. Handle scheme [COVID19SR]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-21 (COVID19OR Claim)
-- Modified by:		Winnie SUEN
-- Modified date:	23 Mar 2021
-- Description:		1. Handle scheme [COVID19OR]
--					2. Revise Reserved field 9 for [Outreach Code]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-20 (COVID19 - Carry Forward & COVID19OR Enrolment)
-- Modified by:		Koala CHENG
-- Modified date:	19 Mar 2021
-- Description:		1. Handle Join eHRSS with empty string
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023 (COVID19 - RVP Claim)
-- Modified by:		Winnie SUEN
-- Modified date:	15 Mar 2021
-- Description:		1. Revise field for [COVID19RVP] & [RVP]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT20-0084
-- Modified by:		Winnie SUEN
-- Modified date:	15 Mar 2021
-- Description:		1. Fix [Vaccine_administration_remark]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			INT20-0081 (Fix COVID19Exporter)
-- Modified by:		Martin Tang
-- Modified date:	4 Mar 2021
-- Description:		1. Add one more field filter (VoucherTransaction.Create_Dtm)
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-07a (COVID19 - VSS Claim)
-- Modified by:		Winnie SUEN
-- Modified date:	3 Mar 2021
-- Description:		1. Add Reserved Field up to 20
--					2. Revise field logic [Vaccine_administration_remark], [Reserved_Field_6], [Reserved_Field_7]
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-0023-05 (COVID19 - VSS Claim)
-- Modified by:		Winnie SUEN
-- Modified date:	27 Feb 2021
-- Description:		Add VSS setting
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-00XX (Immu record)
-- Modified by:		Martin Tang
-- Modified date:	31 Dec 2020
-- Description:		Assign COVID19 Record to for COVID19Exporter to COVID19ExporterQueue
-- =============================================
--exec proc_COVID19ExporterQueue_add @Period_From='2010-01-07 00:00:00',@Period_To='2021-01-07 00:00:00'
CREATE PROCEDURE [dbo].[proc_COVID19ExporterQueue_add] @Period_From DATETIME = NULL, 
                                                       @Period_To   DATETIME = NULL
AS
    BEGIN

        DECLARE @In_Period_From DATETIME= @Period_From;
        DECLARE @In_Period_To DATETIME= @Period_To;
        DECLARE @VBar AS CHAR(1)= '|';
        DECLARE @VBarWithQuote AS CHAR(3)= '"|"';
        DECLARE @DoubleQuote AS CHAR(1)= '"';
        DECLARE @DoubleQuoteWithBackslash AS CHAR(2)= '\"';
        DECLARE @PendingStatus AS CHAR(1)= 'P';
        DECLARE @SkipedStatus AS CHAR(1)= 'S';
        --DECLARE @CompleteStatus AS CHAR(1)= 'C';
        DECLARE @Insert AS CHAR(1)= 'I';
        DECLARE @Update AS CHAR(1)= 'U';
        DECLARE @Delete AS CHAR(1)= 'D';
        DECLARE @CurrentTime AS DATETIME= GETDATE();
        DECLARE @BcCutOffDate DATETIME= '2003-06-23 00:00:00';
        --scheme code		
        DECLARE @SchemeCodeCOVID19CVC AS VARCHAR(10)= 'COVID19CVC';
        DECLARE @SchemeCodeCOVID19DH AS VARCHAR(10)= 'COVID19DH';
        DECLARE @SchemeCodeCOVID19RVP AS VARCHAR(10)= 'COVID19RVP';
        DECLARE @SchemeCodeCOVID19OR AS VARCHAR(10)= 'COVID19OR';
        DECLARE @SchemeCodeCOVID19SR AS VARCHAR(10)= 'COVID19SR';
        DECLARE @SchemeCodeCOVID19SB AS VARCHAR(10)= 'COVID19SB';

        DECLARE @SchemeCodeRVP AS VARCHAR(10)= 'RVP';
        DECLARE @SchemeCodeVSS AS VARCHAR(10)= 'VSS';

        DECLARE @ValidAccountType AS TINYINT= 1;
        DECLARE @TempAccountType AS TINYINT= 2;

        EXEC [proc_SymmetricKey_open];

        --check COVID19ExporterForceSendList
        SELECT CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Doc_Code
                   ELSE tpin.Doc_Code
               END AS 'Doc_Code',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Encrypt_Field1
                   ELSE tpin.Encrypt_Field1
               END AS 'Encrypt_Field1'
        INTO #ResultsUpdatedAccount
        FROM COVID19ExporterForceSendList AS cefsl WITH(NOLOCK)
             INNER JOIN VoucherTransaction AS vt WITH(NOLOCK)
             ON vt.Transaction_ID = cefsl.Transaction_ID
             LEFT JOIN PersonalInformation AS pin WITH(NOLOCK)
             ON pin.Voucher_Acc_ID = vt.Voucher_Acc_ID
                AND pin.Doc_Code = vt.Doc_Code
             LEFT JOIN TempPersonalInformation AS tpin WITH(NOLOCK)
             ON tpin.Voucher_Acc_ID = vt.Temp_Voucher_Acc_ID
                AND vt.Voucher_Acc_ID = '';

        --get TX within the period
        SELECT DISTINCT 
               vt.Transaction_ID
        INTO #ResultsWithTx
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
        WHERE vt.Create_Dtm < @In_Period_To
              AND ((vt.Transaction_Dtm >= @In_Period_From
                    AND vt.Transaction_Dtm < @In_Period_To)
                   OR (vt.Update_Dtm >= @In_Period_From
                       AND vt.Update_Dtm < @In_Period_To));

        -- get Doc No. which personal info has been updated
        INSERT INTO #ResultsUpdatedAccount
        SELECT temp.Doc_Code, 
               temp.Encrypt_Field1
        FROM
            (
                SELECT tpinfo.Doc_Code, 
                       tpinfo.Encrypt_Field1
                FROM TempPersonalInformation AS tpinfo WITH(NOLOCK)
                WHERE(tpinfo.Update_Dtm >= @In_Period_From
                      AND tpinfo.Update_Dtm < @In_Period_To)
                UNION
                SELECT pinfo.Doc_Code, 
                       pinfo.Encrypt_Field1
                FROM PersonalInformation AS pinfo WITH(NOLOCK)
                WHERE(pinfo.Update_Dtm >= @In_Period_From
                      AND pinfo.Update_Dtm < @In_Period_To)
             ) AS temp;

        INSERT INTO #ResultsWithTx
        SELECT vt.Transaction_ID
        FROM VoucherTransaction AS vt
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             INNER JOIN TempPersonalInformation AS tpinfo WITH(NOLOCK)
             ON vt.Temp_Voucher_Acc_ID = tpinfo.Voucher_Acc_ID
                AND vt.Voucher_Acc_ID = ''
             INNER JOIN #ResultsUpdatedAccount AS rua
             ON tpinfo.Doc_Code = rua.Doc_Code
                AND tpinfo.Encrypt_Field1 = rua.Encrypt_Field1
        UNION
        SELECT vt.Transaction_ID
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             INNER JOIN PersonalInformation AS pinfo WITH(NOLOCK)
             ON vt.Voucher_Acc_ID = pinfo.Voucher_Acc_ID
                AND vt.Doc_Code = vt.Doc_Code
             INNER JOIN #ResultsUpdatedAccount AS rua
             ON pinfo.Doc_Code = rua.Doc_Code
                AND pinfo.Encrypt_Field1 = rua.Encrypt_Field1;

        --[Start]get active C19  Transaction_ID to query--
        --Find the doc_code and doc_id for all Transactions
        SELECT DISTINCT 
               vt.Transaction_ID,
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Voucher_Acc_ID
                   ELSE tpin.Voucher_Acc_ID
               END AS 'Voucher_Acc_ID',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN 1
                   ELSE 2
               END AS 'AccountType',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Doc_Code
                   ELSE tpin.Doc_Code
               END AS 'Doc_Code',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Encrypt_Field1
                   ELSE tpin.Encrypt_Field1
               END AS 'Encrypt_Field1',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pin.Update_Dtm
                   ELSE tpin.Update_Dtm
               END AS 'Update_Dtm', 
               vt.Record_Status, 
               vt.Invalidation
        INTO #ResultsWithActiveTx
        FROM #ResultsWithTx AS r
             INNER JOIN VoucherTransaction AS vt WITH(NOLOCK)
             ON r.Transaction_ID = vt.Transaction_id
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_id
                AND td.Subsidize_Item_Code = 'C19'
             LEFT JOIN PersonalInformation AS pin WITH(NOLOCK)
             ON pin.Voucher_Acc_ID = vt.Voucher_Acc_ID
                AND pin.Doc_Code = vt.Doc_Code
             LEFT JOIN TempPersonalInformation AS tpin WITH(NOLOCK)
             ON tpin.Voucher_Acc_ID = vt.Temp_Voucher_Acc_ID
                AND vt.Voucher_Acc_ID = ''
        WHERE vt.Create_Dtm < @In_Period_To;

        --Find the latest vouncher acc id (Same Doc No. in VA & TA, latest personal info update_dtm)
        WITH ctePI
             AS (SELECT Voucher_Acc_ID, 
                        Doc_Code, 
                        Encrypt_Field1, 
                        Update_Dtm, 
                        accountType, 
                        ROW_NUMBER() OVER(PARTITION BY Doc_Code, 
                                                       Encrypt_Field1
                        ORDER BY Update_Dtm DESC, 
                                 accountType) AS rn
                 FROM #ResultsWithActiveTx AS RWAT
                 WHERE RWAT.Record_Status NOT IN('I', 'D')
                 AND ISNULL(RWAT.Invalidation, '') <> 'I')
             SELECT RWAT.Transaction_ID, 
                    ISNULL(ctpI.Voucher_Acc_ID, RWAT.Voucher_Acc_ID) AS Voucher_Acc_ID, 
                    ISNULL(ctpI.accountType, RWAT.accountType) AS accountType, 
                    ISNULL(ctpI.Doc_Code, RWAT.Doc_Code) AS Doc_Code
             INTO #ResultsTxWithLatestAccID
             FROM #ResultsWithActiveTx AS RWAT
                  LEFT OUTER JOIN ctePI AS ctpI
                  ON RWAT.Doc_Code = ctpI.Doc_Code
                     AND RWAT.Encrypt_Field1 = ctpI.Encrypt_Field1
                     AND ctpI.rn = 1;

        --[End] get active C19 Transaction_ID to query--
        --get the details
        SELECT CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN CASE pinfo.Doc_Code
                            WHEN 'HKIC'
                            THEN pinfo.Encrypt_Field1
                            ELSE NULL
                        END
                   ELSE CASE tpi.Doc_Code
                            WHEN 'HKIC'
                            THEN tpi.Encrypt_Field1
                            ELSE NULL
                        END
               END AS 'HKICDocID',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN CASE
                            WHEN pinfo.Doc_Code = 'HKIC'
                            THEN '0'
                            WHEN pinfo.Doc_Code = 'HKBC'
                            THEN '1'
                            WHEN pinfo.Doc_Code = 'CCIC'
                            THEN '2'
                            WHEN pinfo.Doc_Code = 'ROP140'
                            THEN '3'
                            WHEN pinfo.Doc_Code = 'EC'
                                 AND pinfo.Date_of_Issue >= @BcCutOffDate
                            THEN '4'
                            WHEN pinfo.Doc_Code = 'EC'
                                 AND pinfo.Date_of_Issue < @BcCutOffDate
                            THEN '5'
                            WHEN pinfo.Doc_Code = 'OW'
                            THEN '6'
                            WHEN pinfo.Doc_Code = 'PASS'
                            THEN '7'
                            WHEN pinfo.Doc_Code = 'TW'
                            THEN '9'
                            WHEN pinfo.Doc_Code = 'MEP'
                            THEN '10'
                            WHEN pinfo.Doc_Code = 'TWMTP'
                            THEN '11'
                            WHEN pinfo.Doc_Code = 'TWPAR'
                            THEN '12'
                            WHEN pinfo.Doc_Code = 'TWVTD'
                            THEN '13'
                            WHEN pinfo.Doc_Code = 'TWNS'
                            THEN '14'
                            WHEN pinfo.Doc_Code = 'MD'
                            THEN '15'
                            WHEN pinfo.Doc_Code = 'MP'
                            THEN '16'
                            WHEN pinfo.Doc_Code = 'TD'
                            THEN '17'
                            WHEN pinfo.Doc_Code = 'CEEP'
                            THEN '18'
                            WHEN pinfo.Doc_Code = 'ET'
                            THEN '19'
                            WHEN pinfo.Doc_Code = 'RFNo8'
                            THEN '20'
                            WHEN pinfo.Doc_Code = 'DS'
                            THEN '21'
                            ELSE '8' --others
                        END
                   ELSE CASE
                            WHEN tpi.Doc_Code = 'HKIC'
                            THEN '0'
                            WHEN tpi.Doc_Code = 'HKBC'
                            THEN '1'
                            WHEN tpi.Doc_Code = 'CCIC'
                            THEN '2'
                            WHEN tpi.Doc_Code = 'ROP140'
                            THEN '3'
                            WHEN tpi.Doc_Code = 'EC'
                                 AND tpi.Date_of_Issue >= @BcCutOffDate
                            THEN '4'
                            WHEN tpi.Doc_Code = 'EC'
                                 AND tpi.Date_of_Issue < @BcCutOffDate
                            THEN '5'
                            WHEN tpi.Doc_Code = 'OW'
                            THEN '6'
                            WHEN tpi.Doc_Code = 'PASS'
                            THEN '7'
                            WHEN tpi.Doc_Code = 'TW'
                            THEN '9'
                            WHEN tpi.Doc_Code = 'MEP'
                            THEN '10'
                            WHEN tpi.Doc_Code = 'TWMTP'
                            THEN '11'
                            WHEN tpi.Doc_Code = 'TWPAR'
                            THEN '12'
                            WHEN tpi.Doc_Code = 'TWVTD'
                            THEN '13'
                            WHEN tpi.Doc_Code = 'TWNS'
                            THEN '14'
                            WHEN tpi.Doc_Code = 'MD'
                            THEN '15'
                            WHEN tpi.Doc_Code = 'MP'
                            THEN '16'
                            WHEN tpi.Doc_Code = 'TD'
                            THEN '17'
                            WHEN tpi.Doc_Code = 'CEEP'
                            THEN '18'
                            WHEN tpi.Doc_Code = 'ET'
                            THEN '19'
                            WHEN tpi.Doc_Code = 'RFNo8'
                            THEN '20'
                            WHEN tpi.Doc_Code = 'DS'
                            THEN '21'
                            ELSE '8' --others
                        END
               END AS 'Doc_Type',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN CASE pinfo.Doc_Code
                            WHEN 'HKIC'
                            THEN NULL
                            WHEN 'EC'
                            THEN ENCRYPTBYKEY(KEY_GUID('sym_Key'), REPLACE(LTRIM(RTRIM(pinfo.EC_Reference_No)), @VBar, @VBarWithQuote))
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo
                            .Encrypt_Field1))) + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field11))))), @VBar, @VBarWithQuote)))
                        END
                   ELSE CASE tpi.Doc_Code
                            WHEN 'HKIC'
                            THEN NULL
                            WHEN 'EC'
                            THEN ENCRYPTBYKEY(KEY_GUID('sym_Key'), REPLACE(LTRIM(RTRIM(tpi.EC_Reference_No)), @VBar, @VBarWithQuote))
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.
                            Encrypt_Field1))) + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field11))))), @VBar, @VBarWithQuote)))
                        END
               END AS 'otherDocID',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN LTRIM(RTRIM(pinfo.Sex))
                   ELSE LTRIM(RTRIM(tpi.Sex))
               END AS 'Sex',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN CASE
                            WHEN RTRIM(pinfo.Exact_DOB) IN('D', 'T')
                            THEN CONVERT(VARCHAR(10), pinfo.DOB, 23)
                            WHEN pinfo.Exact_DOB IN('Y', 'R', 'V', 'A') --YYYY
                            THEN CONVERT(VARCHAR(4), pinfo.DOB, 23)
                            ELSE CONVERT(VARCHAR(7), pinfo.DOB, 23) --YYYY-MM (M,U)
                        END
                   ELSE CASE
                            WHEN tpi.Exact_DOB IN('D', 'T')
                            THEN CONVERT(VARCHAR(10), tpi.DOB, 23)
                            WHEN tpi.Exact_DOB IN('Y', 'R', 'V', 'A') --YYYY
                            THEN CONVERT(VARCHAR(4), tpi.DOB, 23)
                            ELSE CONVERT(VARCHAR(7), tpi.DOB, 23) --YYYY-MM (M,U)
                        END
               END AS 'DOB',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN CASE
                            WHEN pinfo.Exact_DOB IN('D', 'T')
                            THEN 'Y'
                            ELSE 'N'
                        END
                   ELSE CASE
                            WHEN tpi.Exact_DOB IN('D', 'T')
                            THEN 'Y'
                            ELSE 'N'
                        END
               END AS 'Exact_DOB',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field2)), 'S')
                   ELSE [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field2)), 'S')
               END AS 'Surname',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field2)), 'G')
                   ELSE [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field2)), 'G')
               END AS 'given_name',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field2
                   ELSE tpi.Encrypt_Field2
               END AS 'full_name',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field4
                   ELSE tpi.Encrypt_Field4
               END AS 'ccc1',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field5
                   ELSE tpi.Encrypt_Field5
               END AS 'ccc2',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field6
                   ELSE tpi.Encrypt_Field6
               END AS 'ccc3',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field7
                   ELSE tpi.Encrypt_Field7
               END AS 'ccc4',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field8
                   ELSE tpi.Encrypt_Field8
               END AS 'ccc5',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field9
                   ELSE tpi.Encrypt_Field9
               END AS 'ccc6', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE
                                              WHEN vt.Scheme_Code IN(@SchemeCodeVSS, @SchemeCodeCOVID19OR, @SchemeCodeCOVID19RVP, @SchemeCodeRVP)
                                              THEN ContactNo.AdditionalFieldValueCode
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'Phone_no', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN VSSMainCat.Status_Description
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN VSSMainCat.Status_Description
                                              WHEN @SchemeCodeCOVID19OR
                                              THEN VSSMainCat.Status_Description
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN ''
                                              WHEN @SchemeCodeVSS
                                              THEN VSSMainCat.Status_Description
                                              WHEN @SchemeCodeCOVID19DH
                                              THEN ''
                                              WHEN @SchemeCodeCOVID19SR
                                              THEN ''
                                              WHEN @SchemeCodeCOVID19SB
                                              THEN ''
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'category', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN VSSSubCat.Status_Description
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN VSSSubCat.Status_Description
                                              WHEN @SchemeCodeCOVID19OR
                                              THEN VSSSubCat.Status_Description
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN ''
                                              WHEN @SchemeCodeVSS
                                              THEN VSSSubCat.Status_Description
                                              WHEN @SchemeCodeCOVID19DH
                                              THEN ''
                                              WHEN @SchemeCodeCOVID19SR
                                              THEN ''
                                              WHEN @SchemeCodeCOVID19SB
                                              THEN ''
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'subcategory', 
               '' AS 'Eligibility_check', 
               'Y' AS 'eConsent_EHS', 
               REPLACE(LTRIM(RTRIM(vt.Transaction_ID)), @VBar, @VBarWithQuote) + '_' + REPLACE(LTRIM(RTRIM(td.Subsidize_Code)), @VBar, @VBarWithQuote)
               AS 'record_key', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), @CurrentTime, 120))) AS 'Transaction_datetime',
               CASE
                   WHEN vt.Record_Status NOT IN('I', 'D')
                        AND ISNULL(vt.Invalidation, '') <> 'I'
                   THEN @Insert
                   ELSE @Delete
               END AS 'Transaction_Type', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), vt.Update_Dtm, 120))) AS 'Record_last_update_datetime', 
               'EHS' AS 'Upload_Source_System', 
               '' AS 'Episode_number', --b
               '9406167287' AS 'Attendance_institution_identifier', -- Fixed value provided by eHR
               REPLACE(LTRIM(RTRIM(vt.Transaction_ID)), @VBar, @VBarWithQuote) AS 'Immunisation_record_number', 
               '' AS 'Immunisation_record_remark', --b
               LTRIM(RTRIM(CONVERT(VARCHAR(19), vt.Service_Receive_Dtm, 120))) AS 'Vaccine_administration_date', 
               REPLACE(LTRIM(RTRIM(ISNULL(vcm.Vaccine_Code_Common, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_local_code', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.Brand_Trade_Name, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_local_description', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.Brand_Name, ''))), @VBar, @VBarWithQuote) AS 'Brand_name', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.HK_Reg_No, ''))), @VBar, @VBarWithQuote) AS 'Registration_no', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.Manufacturer, ''))), @VBar, @VBarWithQuote) AS 'Manufacturer', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.RT_Name, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_recognised_terminology_name', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.RT_Code, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_identifier_recognised_terminology', 
               REPLACE(LTRIM(RTRIM(ISNULL(cvbd.RT_DESC, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_description_recognised_terminology', 
               REPLACE(LTRIM(RTRIM(ISNULL(VaccineLotNo.AdditionalFieldValueCode, ''))), @VBar, @VBarWithQuote) AS 'Batch_number', 
               LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(26), CVLD.Expiry_Date, 23), ''))) AS 'Expiry_date', 
               '' AS 'Manufacture_date', --b 
               CASE td.Available_Item_Code
                   WHEN '1STDOSE'
                   THEN '1st dose'
                   WHEN '2NDDOSE'
                   THEN '2nd dose'
                   ELSE ''
               END AS 'Vaccine_dose_sequence',
               CASE vt.Scheme_Code
                   WHEN @SchemeCodeRVP
                   THEN 'Other'
                   WHEN @SchemeCodeCOVID19RVP
                   THEN 'Other'
                   WHEN @SchemeCodeCOVID19OR
                   THEN 'Other'
                   WHEN @SchemeCodeCOVID19CVC
                   THEN 'Other'
                   WHEN @SchemeCodeVSS
                   THEN 'Private'
                   WHEN @SchemeCodeCOVID19DH
                   THEN 'DH'
                   WHEN @SchemeCodeCOVID19SR
                   THEN 'Other'
                   WHEN @SchemeCodeCOVID19SB
                   THEN 'Other'
                   ELSE ''
               END AS 'Vaccination_provider_code',
               CASE vt.Scheme_Code
                   WHEN @SchemeCodeRVP
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeCOVID19RVP
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeCOVID19OR
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeCOVID19CVC
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeVSS
                   THEN 'Private hospital /clinic'
                   WHEN @SchemeCodeCOVID19DH
                   THEN 'DH clinic'
                   WHEN @SchemeCodeCOVID19SR
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeCOVID19SB
                   THEN 'Other vaccination provider'
                   ELSE ''
               END AS 'Vaccination_provider_description', 
               'Department of Health COVID-19 Vaccination Programme' AS 'Vaccination_provider_local_description', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN rvpl.Homename_Eng
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN rvpl.Homename_Eng
                                              WHEN @SchemeCodeCOVID19OR
                                              THEN orl.Outreach_Name_Eng
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN vc.Centre_Name
                                              WHEN @SchemeCodeCOVID19DH
                                              THEN vc.Centre_Name
                                              WHEN @SchemeCodeCOVID19SR
                                              THEN vc.Centre_Name
                                              WHEN @SchemeCodeCOVID19SB
                                              THEN vc.Centre_Name
                                              WHEN @SchemeCodeVSS
                                              THEN COALESCE(orl.Outreach_Name_Eng, p.Practice_Name)
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_administration_premises', 
               REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(sp.Encrypt_Field2)))), @VBar, @VBarWithQuote) AS 'Health_care_staff_name', --sp name
               LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                      WHEN @SchemeCodeCOVID19CVC
                                      THEN vcsm.Booth
                                      WHEN @SchemeCodeCOVID19SR
                                      THEN vcsm.Booth
                                      WHEN @SchemeCodeCOVID19SB
                                      THEN vcsm.Booth
                                      ELSE ''
                                  END, ''))) AS 'Booth', 
               REPLACE(REPLACE(LTRIM(RTRIM(ISNULL(Remarks.AdditionalFieldValueDesc, ''))), @DoubleQuote, @DoubleQuoteWithBackslash), @VBar,
               @VBarWithQuote) AS 'Vaccine_administration_remark', --b 
               '' AS 'Contraindication', --b 
               '' AS 'Side_effect', --b 
               '' AS 'Route_of_administration_local_description', --b 
               '' AS 'Site_of_administration_local_description', --b 
               CASE
                   WHEN LTRIM(RTRIM(ISNULL(JoinEHRSS.AdditionalFieldValueCode, 'N'))) = 'Y'
                   THEN 'Y'
                   ELSE 'N'
               END AS 'eHRSS_Consent', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), vt.Create_Dtm, 120))) AS 'Source_record_create_datetime', 
               LTRIM(RTRIM(ISNULL(vt.Create_By_SmartID, 'N'))) AS 'Reserved_field_1', -- Create by Smart IC: Y/N
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeCOVID19CVC, @SchemeCodeCOVID19DH, @SchemeCodeCOVID19RVP, @SchemeCodeCOVID19OR,
                                      @SchemeCodeCOVID19SR, @SchemeCodeCOVID19SB)
                                      THEN vcsm.Centre_ID
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_2', -- Centre Code
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN REPLACE(LTRIM(RTRIM(ISNULL(pinfo.EC_Serial_No, ''))), @VBar, @VBarWithQuote)
                   ELSE REPLACE(LTRIM(RTRIM(ISNULL(tpi.EC_Serial_No, ''))), @VBar, @VBarWithQuote)
               END AS 'Reserved_field_3', -- EC Serial No.
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeVSS, @SchemeCodeRVP, @SchemeCodeCOVID19RVP, @SchemeCodeCOVID19OR)
                                      THEN MainCategory.AdditionalFieldValueCode
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_4', -- Main Category
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeVSS, @SchemeCodeRVP, @SchemeCodeCOVID19RVP, @SchemeCodeCOVID19OR)
                                      THEN SubCategory.AdditionalFieldValueCode
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_5', -- Sub Category
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeVSS, @SchemeCodeRVP)
                                      THEN pro.Registration_Code
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_6', -- Professional Registration no.
               LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                      WHEN @SchemeCodeVSS
                                      THEN CONVERT(VARCHAR(10), vt.Practice_Display_Seq)
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_7', --Practice Display Seq
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeRVP, @SchemeCodeCOVID19RVP)
                                      THEN rvpl.RCH_code
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_8', --RCH Code
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN vt.Scheme_Code IN(@SchemeCodeVSS, @SchemeCodeCOVID19OR)
                                      THEN orl.Outreach_code
                                      ELSE ''
                                  END, ''))) AS 'Reserved_field_9', --Outreach Code
               LTRIM(RTRIM(ISNULL(CASE
                                      WHEN rwt.AccountType = @ValidAccountType
                                      THEN CASE pinfo.Doc_Code
                                               WHEN 'PASS'
                                               THEN pinfo.PASS_Issue_Region
                                               ELSE ''
                                           END
                                      ELSE CASE tpi.Doc_Code
                                               WHEN 'PASS'
                                               THEN tpi.PASS_Issue_Region
                                               ELSE ''
                                           END
                                  END, ''))) AS 'Reserved_field_10', --Issuing Country/Region
               '' AS 'Reserved_field_11', 
               '' AS 'Reserved_field_12', 
               '' AS 'Reserved_field_13', 
               '' AS 'Reserved_field_14', 
               '' AS 'Reserved_field_15', 
               '' AS 'Reserved_field_16', 
               '' AS 'Reserved_field_17', 
               '' AS 'Reserved_field_18', 
               '' AS 'Reserved_field_19', 
               '' AS 'Reserved_field_20', 
               @PendingStatus AS 'Record_Status', 
               vt.Transaction_ID
        INTO #Results
        FROM #ResultsTxWithLatestAccID AS rwt
             INNER JOIN VoucherTransaction AS vt WITH(NOLOCK)
             ON rwt.Transaction_ID = vt.Transaction_ID
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             LEFT OUTER JOIN PersonalInformation AS pinfo WITH(NOLOCK)
             ON rwt.Voucher_Acc_ID = pinfo.Voucher_Acc_ID
                AND rwt.Doc_Code = pinfo.Doc_Code
                AND rwt.accountType = @ValidAccountType
             LEFT OUTER JOIN tempPersonalInformation AS tpi WITH(NOLOCK)
             ON rwt.Voucher_Acc_ID = tpi.Voucher_Acc_ID
                AND rwt.accountType = @TempAccountType
             LEFT OUTER JOIN TransactionAdditionalField AS ContactNo WITH(NOLOCK)
             ON vt.Transaction_ID = ContactNo.Transaction_ID
                AND ContactNo.AdditionalFieldID = 'ContactNo'
             LEFT OUTER JOIN TransactionAdditionalField AS VaccineBrand WITH(NOLOCK)
             ON vt.Transaction_ID = VaccineBrand.Transaction_ID
                AND VaccineBrand.AdditionalFieldID = 'VaccineBrand'
             LEFT OUTER JOIN TransactionAdditionalField AS VaccineLotNo WITH(NOLOCK)
             ON vt.Transaction_ID = VaccineLotNo.Transaction_ID
                AND VaccineLotNo.AdditionalFieldID = 'VaccineLotNo'
             LEFT OUTER JOIN TransactionAdditionalField AS Remarks WITH(NOLOCK)
             ON vt.Transaction_ID = Remarks.Transaction_ID
                AND Remarks.AdditionalFieldID = 'Remarks'
             LEFT OUTER JOIN TransactionAdditionalField AS JoinEHRSS WITH(NOLOCK)
             ON vt.Transaction_ID = JoinEHRSS.Transaction_ID
                AND JoinEHRSS.AdditionalFieldID = 'JoinEHRSS'
             LEFT OUTER JOIN TransactionAdditionalField AS RCHCode WITH(NOLOCK)
             ON vt.Transaction_ID = RCHCode.Transaction_ID
                AND RCHCode.AdditionalFieldID = 'RHCCode'
             LEFT OUTER JOIN TransactionAdditionalField AS ORCode WITH(NOLOCK)
             ON vt.Transaction_ID = ORCode.Transaction_ID
                AND ORCode.AdditionalFieldID = 'OutreachCode'
             LEFT OUTER JOIN RVPHomeList AS rvpl WITH(NOLOCK)
             ON rvpl.RCH_code = RCHCode.AdditionalFieldValueCode
             LEFT OUTER JOIN OutreachList AS orl WITH(NOLOCK)
             ON orl.Outreach_Code = ORCode.AdditionalFieldValueCode
             LEFT OUTER JOIN COVID19VaccineBrandDetail AS cvbd WITH(NOLOCK)
             ON VaccineBrand.AdditionalFieldValueCode = cvbd.Brand_ID
             LEFT OUTER JOIN COVID19VaccineLotDetail AS CVLD WITH(NOLOCK)
             ON VaccineLotNo.AdditionalFieldValueCode = CVLD.Vaccine_Lot_No
             LEFT OUTER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON vt.SP_ID = sp.SP_ID
             LEFT OUTER JOIN Practice AS p WITH(NOLOCK)
             ON vt.SP_ID = p.SP_ID
                AND vt.Practice_Display_Seq = p.Display_Seq
             LEFT OUTER JOIN Professional AS pro WITH(NOLOCK)
             ON vt.SP_ID = pro.SP_ID
                AND p.Professional_Seq = pro.Professional_Seq
             LEFT OUTER JOIN VaccineCodeMapping AS vcm WITH(NOLOCK)
             ON vcm.Vaccine_Code_Source = LTRIM(RTRIM(td.Scheme_Code)) + @VBar + CONVERT(VARCHAR, td.Scheme_Seq) + @VBar + td.Subsidize_Code
                AND vcm.Target_System = 'CMS'
                AND vcm.Vaccine_Brand_ID_Source = VaccineBrand.AdditionalFieldValueCode
             LEFT OUTER JOIN ClaimCategory AS CC WITH(NOLOCK)
             ON CC.Category_Code = VT.Category_Code
             LEFT OUTER JOIN VaccineCentreSPMapping AS vcsm WITH(NOLOCK)
             ON vt.SP_ID = vcsm.SP_ID
                AND vt.Practice_Display_Seq = vcsm.Practice_Display_Seq
             LEFT OUTER JOIN VaccineCentre AS vc WITH(NOLOCK)
             ON vcsm.Centre_ID = vc.Centre_ID
             LEFT OUTER JOIN TransactionAdditionalField AS MainCategory WITH(NOLOCK)
             ON vt.Transaction_ID = MainCategory.Transaction_ID
                AND MainCategory.AdditionalFieldID = 'MainCategory'
             LEFT OUTER JOIN TransactionAdditionalField AS SubCategory WITH(NOLOCK)
             ON vt.Transaction_ID = SubCategory.Transaction_ID
                AND SubCategory.AdditionalFieldID = 'SubCategory'
             LEFT OUTER JOIN StatusData AS VSSMainCat WITH(NOLOCK)
             ON VSSMainCat.Enum_Class = 'VSSC19MainCategory'
                AND VSSMainCat.Column_Name = MainCategory.AdditionalFieldValueCode
                AND VSSMainCat.Status_Value = MainCategory.AdditionalFieldValueCode
             LEFT OUTER JOIN StatusData AS VSSSubCat WITH(NOLOCK)
             ON VSSSubCat.Enum_Class = 'VSSC19SubCategory'
                AND VSSSubCat.Column_Name = MainCategory.AdditionalFieldValueCode
                AND VSSSubCat.Status_Value = SubCategory.AdditionalFieldValueCode;

        --the Table of the lastest transaction
        SELECT ceq.Transaction_ID, 
               ceq.transaction_Type, 
               ceq.Encrypt_Field2, 
               ceq.Doc_Type, 
               ceq.Sex, 
               ceq.DOB, 
               ceq.Exact_DOB, 
               ceq.Encrypt_Field3, 
               ceq.Encrypt_Field4, 
               ceq.Encrypt_Field5, 
               ceq.Encrypt_Field6, 
               ceq.Encrypt_Field7, 
               ceq.Encrypt_Field8, 
               ceq.Encrypt_Field9, 
               ceq.PASS_Issue_Region
        INTO #ResultsLatestByTransaction
        FROM
            (
                SELECT c.Transaction_ID, 
                       c.transaction_Type, 
                       c.Encrypt_Field2, 
                       c.Doc_Type, 
                       c.Sex, 
                       c.DOB, 
                       c.Exact_DOB, 
                       c.Encrypt_Field3, 
                       c.Encrypt_Field4, 
                       c.Encrypt_Field5, 
                       c.Encrypt_Field6, 
                       c.Encrypt_Field7, 
                       c.Encrypt_Field8, 
                       c.Encrypt_Field9, 
                       c.PASS_Issue_Region, 
                       ROW_NUMBER() OVER(PARTITION BY c.Transaction_ID
                       ORDER BY c.Update_Dtm DESC) AS rn
                FROM COVID19ExporterQueue AS c WITH(NOLOCK)--which is using a filtered index "IX_COVID19ExporterQueue"
                WHERE c.Record_Status = 'C'
             ) AS ceq
            INNER JOIN #Results AS r
            ON r.Transaction_ID = ceq.Transaction_ID
        WHERE ceq.rn = 1;

        --handle Update case
        --Only send the 'Update' record, when the latest record:
        --the Record_Status is complete(C) and
        --personal information is different

        UPDATE r
          SET transaction_Type = @Update, 
              Record_Status = CASE
                                  WHEN ISNULL(r.HKICDocID, r.otherDocID) = ctecq.Encrypt_Field2
                                       AND r.Doc_Type = ctecq.Doc_Type
                                       AND r.Sex = ctecq.Sex
                                       AND r.DOB = ctecq.DOB
                                       AND r.Exact_DOB = ctecq.Exact_DOB
                                       AND r.full_name = ctecq.Encrypt_Field3
                                       AND r.ccc1 = ctecq.Encrypt_Field4
                                       AND r.ccc2 = ctecq.Encrypt_Field5
                                       AND r.ccc3 = ctecq.Encrypt_Field6
                                       AND r.ccc4 = ctecq.Encrypt_Field7
                                       AND r.ccc5 = ctecq.Encrypt_Field8
                                       AND r.ccc6 = ctecq.Encrypt_Field9
                                       AND r.Reserved_field_10 = ISNULL(ctecq.PASS_Issue_Region, '')
                                  THEN @SkipedStatus
                                  ELSE @PendingStatus
                              END
        FROM #Results r
             INNER JOIN #ResultsLatestByTransaction ctecq
             ON r.Transaction_ID = ctecq.Transaction_ID
        WHERE r.transaction_Type = @Insert;

        --handle Del case
        --Only send the 'Del' record, when the latest record:
        --the Record_Status is complete(C) and
        --transaction type is Insert(I) or Update(U)        
        UPDATE r
          SET Record_Status = CASE
                                  WHEN ctecq.transaction_Type IN(@Insert, @Update)
                                  THEN @PendingStatus
                                  ELSE @SkipedStatus
                              END
        FROM #Results r
             LEFT OUTER JOIN #ResultsLatestByTransaction ctecq
             ON r.Transaction_ID = ctecq.Transaction_ID
        WHERE r.transaction_Type = @Delete;
        -------------------------------------------------------------
        --The transactions are skipped to send in COVID19ExporterException table
        UPDATE r
          SET Record_Status = @SkipedStatus
        FROM #Results r
             INNER JOIN COVID19ExporterException cee WITH(NOLOCK)
             ON r.Transaction_ID = cee.Transaction_ID;

        --The transactions must be send in COVID19ExporterForceSendList table.
        --If the transaction exists both tables(COVID19ExporterForceSendList and COVID19ExporterException), it will be skipped to send.

        UPDATE r
          SET Record_Status = @PendingStatus
        FROM #Results r
             INNER JOIN COVID19ExporterForceSendList cefsl WITH(NOLOCK)
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19ExporterException AS cee WITH(NOLOCK)
                            WHERE r.Transaction_ID = cee.Transaction_ID
                         );

        DELETE cefsl
        FROM #Results r
             INNER JOIN COVID19ExporterForceSendList cefsl WITH(NOLOCK)
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19ExporterException AS cee WITH(NOLOCK)
                            WHERE r.Transaction_ID = cee.Transaction_ID
                         );
        --------------------------------------------------------------
        INSERT INTO COVID19ExporterQueue
               (Batch_ID, 
                Transaction_ID, 
                Record_File_ID, 
                Encrypt_Field1, 
                Encrypt_Field2, 
                Doc_Type, 
                Sex, 
                DOB, 
                Exact_DOB, 
                Encrypt_Field3, 
                Encrypt_Field4, 
                Encrypt_Field5, 
                Encrypt_Field6, 
                Encrypt_Field7, 
                Encrypt_Field8, 
                Encrypt_Field9, 
                Record_Status, 
                Transaction_Type, 
                Update_Dtm, 
                From_Dtm, 
                To_Dtm, 
                PASS_Issue_Region
               )
        SELECT CONVERT(VARCHAR(6), @CurrentTime, 12) + REPLACE(CONVERT(VARCHAR(8), @CurrentTime, 108), ':', ''), 
               Transaction_ID, 
               NULL, 
               ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(HKICDocID)), ''))) +
               @VBar + Doc_Type + @VBar + LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(otherDocID)), ''))) + @VBar + Sex + @VBar + DOB +
               @VBar + Exact_DOB + @VBar + Surname + @VBar + given_name + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(full_name)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc1)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc2)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc3)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc4)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc5)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc6)))) +
               @VBar + Phone_no + @VBar + category + @VBar + subcategory + @VBar + Eligibility_check + @VBar + eConsent_EHS + @VBar + record_key +
               @VBar + Transaction_datetime + @VBar + transaction_Type + @VBar + Record_last_update_datetime + @VBar + Upload_Source_System + @VBar
               + Episode_number + @VBar + Attendance_institution_identifier + @VBar + Immunisation_record_number + @VBar +
               Immunisation_record_remark + @VBar + Vaccine_administration_date + @VBar + Vaccine_local_code + @VBar + Vaccine_local_description +
               @VBar + Brand_name + @VBar + Registration_no + @VBar + Manufacturer + @VBar + Vaccine_recognised_terminology_name + @VBar +
               Vaccine_identifier_recognised_terminology + @VBar + Vaccine_description_recognised_terminology + @VBar + Batch_number + @VBar +
               Expiry_date + @VBar + Manufacture_date + @VBar + Vaccine_dose_sequence + @VBar + Vaccination_provider_code + @VBar +
               Vaccination_provider_description + @VBar + Vaccination_provider_local_description + @VBar + Vaccine_administration_premises + @VBar
               + Health_care_staff_name + @VBar + Booth + @VBar + Vaccine_administration_remark + @VBar + Contraindication + @VBar + Side_effect +
               @VBar + Route_of_administration_local_description + @VBar + Site_of_administration_local_description + @VBar + eHRSS_Consent + @VBar
               + Source_record_create_datetime + @VBar + Reserved_field_1 + @VBar + Reserved_field_2 + @VBar + Reserved_field_3 + @VBar +
               Reserved_field_4 + @VBar + Reserved_field_5 + @VBar + Reserved_field_6 + @VBar + Reserved_field_7 + @VBar + Reserved_field_8 + @VBar
               + Reserved_field_9 + @VBar + Reserved_field_10 + @VBar + Reserved_field_11 + @VBar + Reserved_field_12 + @VBar + Reserved_field_13 +
               @VBar + Reserved_field_14 + @VBar + Reserved_field_15 + @VBar + Reserved_field_16 + @VBar + Reserved_field_17 + @VBar +
               Reserved_field_18 + @VBar + Reserved_field_19 + @VBar + Reserved_field_20)), 
               ISNULL(HKICDocID, otherDocID), 
               Doc_Type, 
               Sex, 
               DOB, 
               Exact_DOB, 
               full_name, 
               ccc1, 
               ccc2, 
               ccc3, 
               ccc4, 
               ccc5, 
               ccc6, 
               Record_Status, 
               Transaction_Type, 
               GETDATE(), 
               @In_Period_From, 
               @In_Period_To, 
               IIF(Reserved_field_10 = '', NULL, Reserved_field_10)
        FROM #Results;

        EXEC [proc_SymmetricKey_close];
        IF OBJECT_ID('tempdb..#ResultsWithTx') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWithTx;
            END;

        IF OBJECT_ID('tempdb..#ResultsWithActiveTx') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWithActiveTx;
            END;
        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            BEGIN
                DROP TABLE #Results;
            END;

        IF OBJECT_ID('tempdb..#ResultsLatestByTransaction') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsLatestByTransaction;
            END;

        IF OBJECT_ID('tempdb..#ResultsTxWithLatestAccID') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsTxWithLatestAccID;
            END;

        IF OBJECT_ID('tempdb..#ResultsUpdatedAccount') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsUpdatedAccount;
            END;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_COVID19ExporterQueue_add] TO HCVU;
GO