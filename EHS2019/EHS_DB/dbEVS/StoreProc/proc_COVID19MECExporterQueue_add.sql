
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_COVID19MECExporterQueue_add]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_COVID19MECExporterQueue_add];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			CRE20-023-73 (Upload COVID19MEC to Central DB)
-- Modified by:		Winnie SUEN
-- Modified date:	24 Jan 2022
-- Description:		Upload COVID19MEC Record for COVID19MECExporter to COVID19MECExporterQueue
--					(Copied from "proc_COVID19ExporterQueue_add")
-- =============================================

--exec proc_COVID19MECExporterQueue_add @Period_From='2010-01-07 00:00:00',@Period_To='2021-01-07 00:00:00'
CREATE PROCEDURE [dbo].[proc_COVID19MECExporterQueue_add]	@Period_From DATETIME = NULL, 
															@Period_To   DATETIME = NULL
AS
    BEGIN

		EXEC [proc_SymmetricKey_open];

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
        
		--scheme 
        DECLARE @SchemeCodeCOVID19MEC AS VARCHAR(10)= 'COVID19MEC';
        DECLARE @SubsidizeItemCode AS VARCHAR(10)= 'MEC';

		--fixed value
		DECLARE @Institution_ID AS VARCHAR(10) = '9406167287' -- Provided by eHR
		DECLARE @UploadSourceSystem AS VARCHAR(10) = 'EHS'
		DECLARE @Language AS VARCHAR(10) = 'TC'

        DECLARE @ValidAccountType AS TINYINT= 1;
        DECLARE @TempAccountType AS TINYINT= 2;

		DECLARE @EmptyStringVarbinary AS VARBINARY(MAX) = ENCRYPTBYKEY(KEY_GUID('sym_Key'), N'')
		-----------------------------------------------------------------
		    
-- ================================================================
-- Part 1: Retrieve Transaction List to be sent to Central DB
-- ================================================================

        --get TX within the period (MEC only)
        SELECT DISTINCT 
               vt.Transaction_ID
        INTO #ResultsWithTx
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = @SubsidizeItemCode
        WHERE vt.Create_Dtm < @In_Period_To
              AND ((vt.Transaction_Dtm >= @In_Period_From
                    AND vt.Transaction_Dtm < @In_Period_To)
                   OR (vt.Update_Dtm >= @In_Period_From
                       AND vt.Update_Dtm < @In_Period_To));


        --get TX from COVID19MECExporterForceSendList
		INSERT INTO #ResultsWithTx(Transaction_ID)
        SELECT DISTINCT Transaction_ID
        FROM COVID19MECExporterForceSendList WITH(NOLOCK)
		

		-- get Account ID which personal info has been updated        
        SELECT account.Doc_Code, 
               account.Encrypt_Field1,
			   account.Voucher_Acc_ID,
			   account.Account_Type
		INTO #ResultsUpdatedAccount
        FROM
            (
                SELECT tpinfo.Doc_Code, 
                       tpinfo.Encrypt_Field1,
					   tpinfo.Voucher_Acc_ID,
					   @TempAccountType AS [Account_Type]
                FROM TempPersonalInformation AS tpinfo WITH(NOLOCK)
                WHERE(tpinfo.Update_Dtm >= @In_Period_From
                      AND tpinfo.Update_Dtm < @In_Period_To)
                UNION
                SELECT pinfo.Doc_Code, 
                       pinfo.Encrypt_Field1,
					   pinfo.Voucher_Acc_ID,
					   @ValidAccountType AS [Account_Type]
                FROM PersonalInformation AS pinfo WITH(NOLOCK)
                WHERE(pinfo.Update_Dtm >= @In_Period_From
                      AND pinfo.Update_Dtm < @In_Period_To)
             ) AS account;


		-- get transaction with updated account (MEC only)
        INSERT INTO #ResultsWithTx
        SELECT vt.Transaction_ID
        FROM VoucherTransaction AS vt
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = @SubsidizeItemCode
             INNER JOIN #ResultsUpdatedAccount AS rua
             ON vt.Doc_Code = rua.Doc_Code
                AND vt.Temp_Voucher_Acc_ID = rua.Voucher_Acc_ID
				AND rua.Account_Type = @TempAccountType

        UNION
        SELECT vt.Transaction_ID
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = @SubsidizeItemCode
             INNER JOIN #ResultsUpdatedAccount AS rua
             ON vt.Doc_Code = rua.Doc_Code
                AND vt.Voucher_Acc_ID = rua.Voucher_Acc_ID
				AND rua.Account_Type = @ValidAccountType


        --[Start]get Transaction_ID with account info to query--
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
        INTO #ResultsTxWithAccID
        FROM #ResultsWithTx AS r
             INNER JOIN VoucherTransaction AS vt WITH(NOLOCK)
             ON r.Transaction_ID = vt.Transaction_id
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_id
                AND td.Subsidize_Item_Code = @SubsidizeItemCode
             LEFT JOIN PersonalInformation AS pin WITH(NOLOCK)
             ON pin.Voucher_Acc_ID = vt.Voucher_Acc_ID
                AND pin.Doc_Code = vt.Doc_Code
             LEFT JOIN TempPersonalInformation AS tpin WITH(NOLOCK)
             ON tpin.Voucher_Acc_ID = vt.Temp_Voucher_Acc_ID
                AND vt.Voucher_Acc_ID = ''
        WHERE 
			vt.Create_Dtm < @In_Period_To;

        --[End] get active COVID19MEC Transaction_ID to query--


-- ================================================================
-- Part 2: Get Details of data content
-- ================================================================
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
							WHEN pinfo.Doc_Code = 'Doc/I'
							THEN '22'
							WHEN pinfo.Doc_Code = 'ID235B'
							THEN '23'
							WHEN pinfo.Doc_Code = 'ADOPC'
							THEN '24'
							WHEN pinfo.Doc_Code = 'REPMT'
							THEN '25'
							WHEN pinfo.Doc_Code = 'VISA'
							THEN '26'
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
							WHEN tpi.Doc_Code = 'Doc/I'
							THEN '22'
							WHEN tpi.Doc_Code = 'ID235B'
							THEN '23'
							WHEN tpi.Doc_Code = 'ADOPC'
							THEN '24'
							WHEN tpi.Doc_Code = 'REPMT'
							THEN '25'
							WHEN tpi.Doc_Code = 'VISA'
							THEN '26'
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
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(
									LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field11)),''))) + 
									LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field1)))), @VBar, @VBarWithQuote)))
                        END
                   ELSE CASE tpi.Doc_Code
                            WHEN 'HKIC'
                            THEN NULL
                            WHEN 'EC'
                            THEN ENCRYPTBYKEY(KEY_GUID('sym_Key'), REPLACE(LTRIM(RTRIM(tpi.EC_Reference_No)), @VBar, @VBarWithQuote))
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(
									LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field11)),''))) + 
									LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field1)))), @VBar, @VBarWithQuote)))
                        END
               END AS 'otherDocID',
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN REPLACE(LTRIM(RTRIM(ISNULL(pinfo.EC_Serial_No, ''))), @VBar, @VBarWithQuote)
                   ELSE REPLACE(LTRIM(RTRIM(ISNULL(tpi.EC_Serial_No, ''))), @VBar, @VBarWithQuote)
               END AS 'EC_SerialNo',

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
                                  END, ''))) AS 'PASS_Issue_Region',
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
			   @Language AS 'Language',
               REPLACE(LTRIM(RTRIM(ISNULL(ContactNo.AdditionalFieldValueCode, ''))), @VBar, @VBarWithQuote) AS 'Phone_no', 
               CASE
                   WHEN rwt.AccountType = @ValidAccountType
                   THEN pinfo.Encrypt_Field3
                   ELSE tpi.Encrypt_Field3
               END AS 'Encrypted_CName',
			   LTRIM(RTRIM(ISNULL(vt.Create_By_SmartID, 'N'))) AS 'Smart_ID_Indicator', -- Create by Smart IC: Y/N

               REPLACE(LTRIM(RTRIM(vt.Transaction_ID)), @VBar, @VBarWithQuote) AS 'record_key', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), @CurrentTime, 120))) AS 'Transaction_datetime',	
               CASE
                   WHEN vt.Record_Status NOT IN('I', 'D')
                        AND ISNULL(vt.Invalidation, '') <> 'I'
                   THEN @Insert
                   ELSE @Delete
               END AS 'Transaction_Type', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), vt.Update_Dtm, 120))) AS 'Record_last_update_datetime', 
               @UploadSourceSystem AS 'Upload_Source_System', 
			   LTRIM(RTRIM(FORMAT(vt.Service_Receive_Dtm, 'yyyy-MM-dd'))) AS 'Issue_Date',
			   LTRIM(RTRIM(FORMAT(cast(ValidUntil.AdditionalFieldValueCode as DATE), 'yyyy-MM-dd'))) AS 'Valid_Til_Date',
               LTRIM(RTRIM(ISNULL(pro.Registration_Code, ''))) AS 'Profession_RegNo', 
			   REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(sp.Encrypt_Field2)))), @VBar, @VBarWithQuote) AS 'SP_Name_Eng',
			   REPLACE(LTRIM(RTRIM(CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(sp.Encrypt_Field3)))), @VBar, @VBarWithQuote) AS 'SP_Name_Chi',
			   @Institution_ID AS 'Institution_identifier', -- Fixed value provided by eHR

               REPLACE(LTRIM(RTRIM(ISNULL(p.Practice_Name, ''))), @VBar, @VBarWithQuote) AS 'Institution_Name_Eng',
               REPLACE(LTRIM(RTRIM(ISNULL(p.Practice_Name_Chi, ''))), @VBar, @VBarWithQuote) AS 'Institution_Name_Chi',

               LTRIM(RTRIM(ISNULL(PreExisting.AdditionalFieldValueCode, ''))) AS 'PreExisting',
               REPLACE(REPLACE(LTRIM(RTRIM(ISNULL(MedicalTreatment.AdditionalFieldValueDesc, ''))), @DoubleQuote, @DoubleQuoteWithBackslash), @VBar, @VBarWithQuote) AS 'Medical_Treatment',
               '' AS 'PreExisting_Remark1', 
               '' AS 'PreExisting_Remark2', 

			   LTRIM(RTRIM(ISNULL(ContraindBioNTech.AdditionalFieldValueCode, ''))) AS 'Contraindications1',
			   LTRIM(RTRIM(ISNULL(ContraindSinovac.AdditionalFieldValueCode, ''))) AS 'Contraindications2',
			   '' AS 'Contraindications3',
			   '' AS 'Contraindications4',
			   '' AS 'Contraindications5',
			   '' AS 'Contraindications_Remark1',
			   '' AS 'Contraindications_Remark2',
			   '' AS 'Contraindications_Remark3',
			   '' AS 'Contraindications_Remark4',
			   '' AS 'Contraindications_Remark5',
               CASE
                   WHEN LTRIM(RTRIM(ISNULL(JoinEHRSS.AdditionalFieldValueCode, 'N'))) = 'Y'
                   THEN 'Y'
                   ELSE 'N'
               END AS 'eHRSS_Consent', 
			   '' AS 'Reserved_field_1',
			   '' AS 'Reserved_field_2',
			   '' AS 'Reserved_field_3',
			   '' AS 'Reserved_field_4',
			   '' AS 'Reserved_field_5',
			   '' AS 'Reserved_field_6',
			   '' AS 'Reserved_field_7',
			   '' AS 'Reserved_field_8',
			   '' AS 'Reserved_field_9',
			   '' AS 'Reserved_field_10',
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
        FROM #ResultsTxWithAccID AS rwt
             INNER JOIN VoucherTransaction AS vt WITH(NOLOCK)
             ON rwt.Transaction_ID = vt.Transaction_ID
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = @SubsidizeItemCode
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
             LEFT OUTER JOIN TransactionAdditionalField AS ValidUntil WITH(NOLOCK)
             ON vt.Transaction_ID = ValidUntil.Transaction_ID
                AND ValidUntil.AdditionalFieldID = 'ValidUntil'
             LEFT OUTER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON vt.SP_ID = sp.SP_ID
             LEFT OUTER JOIN Practice AS p WITH(NOLOCK)
             ON vt.SP_ID = p.SP_ID
                AND vt.Practice_Display_Seq = p.Display_Seq
             LEFT OUTER JOIN Professional AS pro WITH(NOLOCK)
             ON vt.SP_ID = pro.SP_ID
                AND p.Professional_Seq = pro.Professional_Seq
             LEFT OUTER JOIN TransactionAdditionalField AS PreExisting WITH(NOLOCK)
             ON vt.Transaction_ID = PreExisting.Transaction_ID
                AND PreExisting.AdditionalFieldID = 'Pre-Existing'
             LEFT OUTER JOIN TransactionAdditionalField AS MedicalTreatment WITH(NOLOCK)
             ON vt.Transaction_ID = MedicalTreatment.Transaction_ID
                AND MedicalTreatment.AdditionalFieldID = 'Pre_A2_Remark'
             LEFT OUTER JOIN TransactionAdditionalField AS ContraindBioNTech WITH(NOLOCK)
             ON vt.Transaction_ID = ContraindBioNTech.Transaction_ID
                AND ContraindBioNTech.AdditionalFieldID = 'Contraind_BioNTech'
             LEFT OUTER JOIN TransactionAdditionalField AS ContraindSinovac WITH(NOLOCK)
             ON vt.Transaction_ID = ContraindSinovac.Transaction_ID
                AND ContraindSinovac.AdditionalFieldID = 'Contraind_Sinovac'
             LEFT OUTER JOIN TransactionAdditionalField AS JoinEHRSS WITH(NOLOCK)
             ON vt.Transaction_ID = JoinEHRSS.Transaction_ID
                AND JoinEHRSS.AdditionalFieldID = 'JoinEHRSS'


-- ================================================================
-- Part 3: Determine whether the record should be sent to Central DB
-- ================================================================

        --get the latest transaction record sent to central DB by transaction id from queue
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
               ceq.PASS_Issue_Region,
			   ceq.Encrypt_Field10
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
					   c.Encrypt_Field10,
                       ROW_NUMBER() OVER(PARTITION BY c.Transaction_ID
                       ORDER BY c.Update_Dtm DESC) AS rn
                FROM COVID19MECExporterQueue AS c WITH(NOLOCK)--which is using a filtered index "IX_COVID19MECExporterQueue"
                WHERE c.Record_Status = 'C'
             ) AS ceq
            INNER JOIN #Results AS r
            ON r.Transaction_ID = ceq.Transaction_ID
        WHERE ceq.rn = 1;




        --handle Update case
        --Only send the 'Update' record, when the latest record:
        --the Record_Status is complete(C) and
        --personal information is different

		-- Mark status as 'Skipped' (S) if personal info has not been changed
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
										-- Not to check Chinese name for Old Record that not yet include Chinese Name to Central DB
										-- and document with CCCode e.g. HKIC & ROP140)
									   AND ((ctecq.Encrypt_Field10 IS NULL AND (r.Doc_Type = '0' OR r.Doc_Type = '3'))
											OR (ISNULL(r.Encrypted_CName, @EmptyStringVarbinary) = ISNULL(ctecq.Encrypt_Field10, @EmptyStringVarbinary)))
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
        --The transactions are skipped to send in COVID19MECExporterException table
        UPDATE r
          SET Record_Status = @SkipedStatus
        FROM #Results r
             INNER JOIN COVID19MECExporterException cee WITH(NOLOCK)
             ON r.Transaction_ID = cee.Transaction_ID;

        --The transactions must be send in COVID19MECExporterForceSendList table.
        --If the transaction exists both tables(COVID19MECExporterForceSendList and COVID19MECExporterException), it will be skipped to send.

        UPDATE r
          SET Record_Status = @PendingStatus
        FROM #Results r
             INNER JOIN COVID19MECExporterForceSendList cefsl WITH(NOLOCK)
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19MECExporterException AS cee WITH(NOLOCK)
                            WHERE r.Transaction_ID = cee.Transaction_ID
                         );
	
		-- Clear ForceSendList
        DELETE cefsl
        FROM #Results r
             INNER JOIN COVID19MECExporterForceSendList cefsl WITH(NOLOCK)
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19MECExporterException AS cee WITH(NOLOCK)
                            WHERE r.Transaction_ID = cee.Transaction_ID
                         );

-- ================================================================
-- Part 4: Construct data and insert to queue
-- ================================================================

        INSERT INTO COVID19MECExporterQueue
               (Batch_ID, 
                Transaction_ID, 
                Record_File_ID, 
                Encrypt_Field1,
				Encrypt_Field1_Uni,
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
                PASS_Issue_Region,
				Encrypt_Field10
               )
        SELECT CONVERT(VARCHAR(6), @CurrentTime, 12) + REPLACE(CONVERT(VARCHAR(8), @CurrentTime, 108), ':', ''), 
               Transaction_ID, 
               NULL, 
			   @EmptyStringVarbinary,	-- For new record, no longer look at this column as it uses VARCHAR for decryption			   
               ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(NVARCHAR(MAX), LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(HKICDocID)), ''))) +
               @VBar + Doc_Type + @VBar + LTRIM(RTRIM(ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(otherDocID)), ''))) + @VBar + EC_SerialNo + 
			   @VBar + PASS_Issue_Region + @VBar + Sex + @VBar + DOB + @VBar + Exact_DOB + 
			   @VBar + Surname + @VBar + given_name + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(full_name)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc1)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc2)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc3)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc4)))) +
               @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc5)))) + @VBar + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(ccc6)))) +
			   @VBar + [Language] + @VBar + Phone_no + 
			   @VBar + REPLACE(LTRIM(RTRIM(ISNULL(CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(Encrypted_CName)),''))), @VBar, @VBarWithQuote) +
			   @VBar + Smart_ID_Indicator + @VBar + record_key + @VBar + Transaction_datetime + @VBar + transaction_Type + 
			   @VBar + Record_last_update_datetime + @VBar + Upload_Source_System + 
			   @VBar + Issue_Date + @VBar + Valid_Til_Date + @VBar + Profession_RegNo + @VBar + SP_Name_Eng + @VBar + SP_Name_Chi +
			   @VBar + Institution_identifier + @VBar + Institution_Name_Eng + @VBar + Institution_Name_Chi +
			   @VBar + PreExisting + @VBar + Medical_Treatment + @VBar + PreExisting_Remark1 + @VBar + PreExisting_Remark2 +
			   @VBar + Contraindications1 + @VBar + Contraindications2 + @VBar + Contraindications3 + @VBar + Contraindications_Remark4 + @VBar + Contraindications5 +
			   @VBar + Contraindications_Remark1 + @VBar + Contraindications_Remark2 + @VBar + Contraindications_Remark3 +
			   @VBar + Contraindications_Remark4 + @VBar + Contraindications_Remark5 + @VBar + eHRSS_Consent +
			   @VBar + Reserved_field_1 + @VBar + Reserved_field_2 + @VBar + Reserved_field_3 + @VBar + Reserved_field_4 + @VBar + Reserved_field_5 +
			   @VBar + Reserved_field_6 + @VBar + Reserved_field_7 + @VBar + Reserved_field_8 + @VBar + Reserved_field_9 + @VBar + Reserved_field_10 +
			   @VBar + Reserved_field_11 + @VBar + Reserved_field_12 + @VBar + Reserved_field_13 + @VBar + Reserved_field_14 + @VBar + Reserved_field_15 +
			   @VBar + Reserved_field_16 + @VBar + Reserved_field_17 + @VBar + Reserved_field_18 + @VBar + Reserved_field_19 + @VBar + Reserved_field_20)), 

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
               IIF(PASS_Issue_Region = '', NULL, PASS_Issue_Region),
			   Encrypted_CName
        FROM #Results;


        EXEC [proc_SymmetricKey_close];
        IF OBJECT_ID('tempdb..#ResultsWithTx') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWithTx;
            END;

        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            BEGIN
                DROP TABLE #Results;
            END;

        IF OBJECT_ID('tempdb..#ResultsLatestByTransaction') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsLatestByTransaction;
            END;

        IF OBJECT_ID('tempdb..#ResultsTxWithAccID') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsTxWithAccID;
            END;

        IF OBJECT_ID('tempdb..#ResultsUpdatedAccount') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsUpdatedAccount;
            END;
    END;

GO

GRANT EXECUTE ON [dbo].[proc_COVID19MECExporterQueue_add] TO HCVU;
GO