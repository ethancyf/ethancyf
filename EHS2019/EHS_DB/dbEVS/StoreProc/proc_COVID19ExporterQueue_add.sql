
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
        DECLARE @VBarWithQuote AS CHAR(1)= '"|"';
        DECLARE @PendingStatus AS CHAR(1)= 'P';
        DECLARE @SkipedStatus AS CHAR(1)= 'S';
        DECLARE @CompleteStatus AS CHAR(1)= 'C';
        DECLARE @Insert AS CHAR(1)= 'I';
        DECLARE @Update AS CHAR(1)= 'U';
        DECLARE @Delete AS CHAR(1)= 'D';
        DECLARE @CurrentTime AS DATETIME= GETDATE();
        DECLARE @BcCutOffDate DATETIME= '2003-06-23 00:00:00';
        --scheme code		
        DECLARE @SchemeCodeCOVID19CVC AS VARCHAR(10)= 'COVID19CVC';
        DECLARE @SchemeCodeCOVID19CBD AS VARCHAR(10)= 'COVID19CBD';
        DECLARE @SchemeCodeCOVID19RVP AS VARCHAR(10)= 'COVID19RVP';
        DECLARE @SchemeCodeRVP AS VARCHAR(10)= 'RVP';
        DECLARE @SchemeCodeVSS AS VARCHAR(10)= 'VSS';

        EXEC [proc_SymmetricKey_open];

        SELECT CASE
                   WHEN vt.Voucher_Acc_ID <> ''
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
                   WHEN vt.Voucher_Acc_ID <> ''
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
                            ELSE '8'
                        END
               END AS 'Doc_Type',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN CASE pinfo.Doc_Code
                            WHEN 'HKIC'
                            THEN NULL
							WHEN 'EC'
							THEN ENCRYPTBYKEY(KEY_GUID('sym_Key'),REPLACE(LTRIM(RTRIM(pinfo.EC_Reference_No)), @VBar, @VBarWithQuote))
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo
                            .Encrypt_Field1))) + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field11))))), @VBar, @VBarWithQuote)))
                        END
                   ELSE CASE tpi.Doc_Code
                            WHEN 'HKIC'
                            THEN NULL
							WHEN 'EC'
							THEN ENCRYPTBYKEY(KEY_GUID('sym_Key'),REPLACE(LTRIM(RTRIM(tpi.EC_Reference_No)), @VBar, @VBarWithQuote))
                            ELSE ENCRYPTBYKEY(KEY_GUID('sym_Key'), CONVERT(VARCHAR(MAX), REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.
                            Encrypt_Field1))) + LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field11))))), @VBar, @VBarWithQuote)))
                        END
               END AS 'otherDocID',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN LTRIM(RTRIM(pinfo.Sex))
                   ELSE LTRIM(RTRIM(tpi.Sex))
               END AS 'Sex',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
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
                   WHEN vt.Voucher_Acc_ID <> ''
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
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field2)), 'S')
                   ELSE [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field2)), 'S')
               END AS 'Surname',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(pinfo.Encrypt_Field2)), 'G')
                   ELSE [dbo].[func_split_EngName](CONVERT(VARCHAR(MAX), DECRYPTBYKEY(tpi.Encrypt_Field2)), 'G')
               END AS 'given_name',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field2
                   ELSE tpi.Encrypt_Field2
               END AS 'full_name',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field4
                   ELSE tpi.Encrypt_Field4
               END AS 'ccc1',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field5
                   ELSE tpi.Encrypt_Field5
               END AS 'ccc2',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field6
                   ELSE tpi.Encrypt_Field6
               END AS 'ccc3',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field7
                   ELSE tpi.Encrypt_Field7
               END AS 'ccc4',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field8
                   ELSE tpi.Encrypt_Field8
               END AS 'ccc5',
               CASE
                   WHEN vt.Voucher_Acc_ID <> ''
                   THEN pinfo.Encrypt_Field9
                   ELSE tpi.Encrypt_Field9
               END AS 'ccc6', 
               REPLACE(LTRIM(RTRIM(ISNULL(ContactNo.AdditionalFieldValueCode, ''))), @VBar, @VBarWithQuote) AS 'Phone_no', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN CC.Category_Name
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN CC.Category_Name
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN ''
                                              WHEN @SchemeCodeVSS
                                              THEN CC.Category_Name
                                              WHEN @SchemeCodeCOVID19CBD
                                              THEN CC.Category_Name
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'category', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN CASE rvpl.Type
                                                       WHEN 'C'
                                                       THEN 'Residential Child Care Centre (RCHC)'
                                                       WHEN 'D'
                                                       THEN 'Residential care home for disabilities (RCHD)'
                                                       WHEN 'E'
                                                       THEN 'Residential care home for elderly (RCHE) '
                                                       WHEN 'I'
                                                       THEN 'Institutions serving PID (RCHI)'
                                                       ELSE ''
                                                   END
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN CASE rvpl.Type
                                                       WHEN 'C'
                                                       THEN 'Residential Child Care Centre (RCHC)'
                                                       WHEN 'D'
                                                       THEN 'Residential care home for disabilities (RCHD)'
                                                       WHEN 'E'
                                                       THEN 'Residential care home for elderly (RCHE) '
                                                       WHEN 'I'
                                                       THEN 'Institutions serving PID (RCHI)'
                                                       ELSE ''
                                                   END
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN ''
                                              WHEN @SchemeCodeVSS
                                              THEN 'Private'
                                              WHEN @SchemeCodeCOVID19CBD
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
                   THEN 'DH'
                   WHEN @SchemeCodeCOVID19RVP
                   THEN 'DH'
                   WHEN @SchemeCodeCOVID19CVC
                   THEN 'Other'
                   WHEN @SchemeCodeVSS
                   THEN 'Private'
                   WHEN @SchemeCodeCOVID19CBD
                   THEN 'Other'
                   ELSE ''
               END AS 'Vaccination_provider_code',
               CASE vt.Scheme_Code
                   WHEN @SchemeCodeRVP
                   THEN 'DH clinic'
                   WHEN @SchemeCodeCOVID19RVP
                   THEN 'DH clinic'
                   WHEN @SchemeCodeCOVID19CVC
                   THEN 'Other vaccination provider'
                   WHEN @SchemeCodeVSS
                   THEN 'Private hospital /clinic'
                   WHEN @SchemeCodeCOVID19CBD
                   THEN 'Other vaccination provider'
                   ELSE ''
               END AS 'Vaccination_provider_description', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN 'Department of Health'
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN 'Department of Health'
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN 'Department of Health COVID-19 Vaccination Programme'
                                              WHEN @SchemeCodeVSS
                                              THEN p.Practice_Name
                                              WHEN @SchemeCodeCOVID19CBD
                                              THEN 'The University of Hong Kong - Shenzhen Hospital'
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'Vaccination_provider_local_description', 
               REPLACE(LTRIM(RTRIM(ISNULL(CASE vt.Scheme_Code
                                              WHEN @SchemeCodeRVP
                                              THEN rvpl.Homename_Eng
                                              WHEN @SchemeCodeCOVID19RVP
                                              THEN rvpl.Homename_Eng
                                              WHEN @SchemeCodeCOVID19CVC
                                              THEN vc.Centre_Name
                                              WHEN @SchemeCodeVSS
                                              THEN p.Practice_Name
                                              WHEN @SchemeCodeCOVID19CBD
                                              THEN 'Shenzhen Bay Port'
                                              ELSE ''
                                          END, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_administration_premises', 
               REPLACE(LTRIM(RTRIM(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(sp.Encrypt_Field2)))), @VBar, @VBarWithQuote) AS 'Health_care_staff_name', --sp name
               LTRIM(RTRIM(ISNULL(vcsm.Booth, ''))) AS 'Booth', 
               REPLACE(LTRIM(RTRIM(ISNULL(Remarks.AdditionalFieldValueDesc, ''))), @VBar, @VBarWithQuote) AS 'Vaccine_administration_remark', --b 
               '' AS 'Contraindication', --b 
               '' AS 'Side_effect', --b 
               '' AS 'Route_of_administration_local_description', --b 
               '' AS 'Site_of_administration_local_description', --b 
               LTRIM(RTRIM(ISNULL(JoinEHRSS.AdditionalFieldValueCode, ''))) AS 'eHRSS_Consent', 
               LTRIM(RTRIM(CONVERT(VARCHAR(19), vt.Create_Dtm, 120))) AS 'Source_record_create_datetime', 
               LTRIM(RTRIM(ISNULL(vt.Create_By_SmartID, 'N'))) AS 'Reserved_field_1', 
               '' AS 'Reserved_field_2', 
               '' AS 'Reserved_field_3', 
               '' AS 'Reserved_field_4', 
               '' AS 'Reserved_field_5', 
               @PendingStatus AS 'Record_Status', 
               vt.Transaction_ID
        INTO #Results
        FROM VoucherTransaction AS vt WITH(NOLOCK)
             INNER JOIN TransactionDetail AS td WITH(NOLOCK)
             ON vt.Transaction_ID = td.Transaction_ID
                AND td.Subsidize_Item_Code = 'C19'
             LEFT OUTER JOIN PersonalInformation AS pinfo WITH(NOLOCK)
             ON vt.Voucher_Acc_ID = pinfo.Voucher_Acc_ID
                AND vt.Doc_Code = pinfo.Doc_Code
             LEFT OUTER JOIN tempPersonalInformation AS tpi WITH(NOLOCK)
             ON vt.Temp_Voucher_Acc_ID = tpi.Voucher_Acc_ID
                AND vt.Doc_Code = tpi.Doc_Code
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
             LEFT OUTER JOIN RVPHomeList AS rvpl
             ON rvpl.RCH_code = RCHCode.AdditionalFieldValueCode
             LEFT OUTER JOIN COVID19VaccineBrandDetail AS cvbd WITH(NOLOCK)
             ON VaccineBrand.AdditionalFieldValueCode = cvbd.Brand_ID
             LEFT OUTER JOIN COVID19VaccineLotDetail AS CVLD WITH(NOLOCK)
             ON VaccineLotNo.AdditionalFieldValueCode = CVLD.Vaccine_Lot_No
             LEFT OUTER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON vt.SP_ID = sp.SP_ID
             LEFT OUTER JOIN Practice AS p WITH(NOLOCK)
             ON vt.SP_ID = p.SP_ID
                AND vt.Practice_Display_Seq = p.Display_Seq
             LEFT OUTER JOIN VaccineCodeMapping AS vcm WITH(NOLOCK)
             ON vcm.Vaccine_Code_Source = LTRIM(RTRIM(td.Scheme_Code)) + @VBar + CONVERT(VARCHAR, td.Scheme_Seq) + @VBar + td.Subsidize_Code
                AND vcm.Target_System = 'CMS'
                AND vcm.Vaccine_Brand_ID_Source = VaccineBrand.AdditionalFieldValueCode
             LEFT OUTER JOIN ClaimCategory AS CC WITH(NOLOCK)
             ON CC.Category_Code = VT.Category_Code
             LEFT OUTER JOIN VaccineCentreSPMapping AS vcsm
             ON vt.SP_ID = vcsm.SP_ID
                AND vt.Practice_Display_Seq = vcsm.Practice_Display_Seq
             LEFT OUTER JOIN VaccineCentre AS vc
             ON vcsm.Centre_ID = vc.Centre_ID
        WHERE(pinfo.Update_Dtm >= @In_Period_From
              AND pinfo.Update_Dtm < @In_Period_To)
             OR (tpi.Update_Dtm >= @In_Period_From
                 AND tpi.Update_Dtm < @In_Period_To)
             OR (vt.Transaction_Dtm >= @In_Period_From
                 AND vt.Transaction_Dtm < @In_Period_To)
             OR (vt.Update_Dtm >= @In_Period_From
                 AND vt.Update_Dtm < @In_Period_To)
             OR EXISTS
                      (
                          SELECT 1
                          FROM COVID19ExporterForceSendList AS cefsl
                          WHERE vt.Transaction_ID = cefsl.Transaction_ID
                       );

        --handle Update case
        --Only send the 'Update' record, when the latest record:
        --the Record_Status is complete(C) and
        --personal information is different
        WITH cteCOVID19Queue -- the view of the lastest transaction
             AS (SELECT Transaction_ID, 
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
                        ROW_NUMBER() OVER(PARTITION BY Transaction_ID
                        ORDER BY Update_Dtm DESC) AS rn
                 FROM COVID19ExporterQueue
                 WHERE Record_Status = @CompleteStatus)
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
                                       THEN @SkipedStatus
                                       ELSE @PendingStatus
                                   END
             FROM #Results r
                  INNER JOIN cteCOVID19Queue ctecq
                  ON r.Transaction_ID = ctecq.Transaction_ID
                     AND ctecq.rn = 1
             WHERE r.transaction_Type = @Insert;

        --handle Del case
        --Only send the 'Del' record, when the latest record:
        --the Record_Status is complete(C) and
        --transaction type is Insert(I) or Update(U) 
        WITH cteCOVID19Queue -- the view of the lastest transaction
             AS (SELECT Transaction_ID, 
                        transaction_Type, 
                        ROW_NUMBER() OVER(PARTITION BY Transaction_ID
                        ORDER BY Update_Dtm DESC) AS rn
                 FROM COVID19ExporterQueue
                 WHERE Record_Status = @CompleteStatus)
             UPDATE r
               SET Record_Status = CASE
                                       WHEN ctecq.transaction_Type IN(@Insert, @Update)
                                       THEN @PendingStatus
                                       ELSE @SkipedStatus
                                   END
             FROM #Results r
                  LEFT OUTER JOIN cteCOVID19Queue ctecq
                  ON r.Transaction_ID = ctecq.Transaction_ID
                     AND ctecq.rn = 1
             WHERE r.transaction_Type = @Delete;
        -------------------------------------------------------------
        --The transactions are skipped to send in COVID19ExporterException table
        UPDATE r
          SET Record_Status = @SkipedStatus
        FROM #Results r
             INNER JOIN COVID19ExporterException cee
             ON r.Transaction_ID = cee.Transaction_ID;

        --The transactions must be send in COVID19ExporterForceSendList table.
		--If the transaction exists both tables(COVID19ExporterForceSendList and COVID19ExporterException), it will be skipped to send.

        UPDATE r
          SET Record_Status = @PendingStatus
        FROM #Results r
             INNER JOIN COVID19ExporterForceSendList cefsl
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19ExporterException AS cee
                            WHERE r.Transaction_ID = cee.Transaction_ID
                         );

        DELETE cefsl
        FROM #Results r
             INNER JOIN COVID19ExporterForceSendList cefsl
             ON r.Transaction_ID = cefsl.Transaction_ID
        WHERE NOT EXISTS
                        (
                            SELECT 1
                            FROM COVID19ExporterException AS cee
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
                To_Dtm
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
               Reserved_field_4 + @VBar + Reserved_field_5)), 
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
               @In_Period_To
        FROM #Results;

        EXEC [proc_SymmetricKey_close];

        IF OBJECT_ID('tempdb..#Results') IS NOT NULL
            BEGIN
                DROP TABLE #Results;
            END;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_COVID19ExporterQueue_add] TO HCVU;
GO