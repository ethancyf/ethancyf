
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_HAServicePatient_bulkcopy]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_HAServicePatient_bulkcopy]
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR No.:			INT20-0066
-- Modified by:		Chris YIM
-- Modified date:	28 Dec 2020
-- Description:		Upper HKIC
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-015 (HA Scheme) 
-- Modified by:		Raiman Chong
-- Modified date:	20 Oct 2020
-- Description:		insert record for HAServicePatient Table
-- =============================================

CREATE PROCEDURE [dbo].[proc_HAServicePatient_bulkcopy] @PatientTable HASERVICEPATIENTTABLETYPE READONLY
AS
    BEGIN

        SET NOCOUNT ON;
        -- =============================================
        -- Declaration
        -- =============================================
        -- =============================================
        -- Validation 
        -- =============================================
        -- =============================================
        -- Initialization
        -- =============================================
        OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key;
        -- =============================================
        -- Return results
        -- =============================================
        --Clear the record in HAServicePatient table
        DELETE FROM dbo.HAServicePatient;

        INSERT INTO dbo.HAServicePatient
               (Serial_No, 
                Doc_Code, 
                Encrypt_Field1, 
                HKIC_Symbol, 
				Claimed_Payment_Type_Code,
                Claimed_Payment_Type, 
                Eligibility, 
                Payment_Type_Result, 
                Patient_Type, 
                Create_Dtm
               )
        SELECT Serial_No, 
               Doc_Code, 
               ENCRYPTBYKEY(KEY_GUID('sym_Key'), UPPER(HKID_Code)), 
               HKIC_Symbol, 
			   Claimed_Payment_Type_Code,
               Claimed_Payment_Type, 
               Eligibility, 
               Payment_Type_Result, 
               Patient_Type, 
               GETDATE()
        FROM @PatientTable;

        CLOSE SYMMETRIC KEY sym_Key;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_HAServicePatient_bulkcopy] TO HCVU;
GO

