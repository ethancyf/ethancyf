
IF TYPE_ID('[dbo].[HAServicePatientTableType]') IS NOT NULL
    BEGIN
        DROP TYPE [dbo].[HAServicePatientTableType];
    END;
GO

CREATE TYPE HAServicePatientTableType AS TABLE
([Serial_No]						[VARCHAR](15)		NOT NULL,
	[Doc_Code]						[VARCHAR](20)		NOT NULL,
	[HKID_Code]						[VARCHAR](9)		NOT NULL,
	[HKIC_Symbol]					[CHAR](1)			NOT NULL,
	[Claimed_Payment_Type_Code]		[VARCHAR](10)		NOT NULL,
	[Claimed_Payment_Type]			[VARCHAR](10)		NOT NULL,
	[Eligibility]					[VARCHAR](3)		NOT NULL,
	[Payment_Type_Result]			[CHAR](1)			NOT NULL,
	[Patient_Type]					[VARCHAR](20)		NOT NULL
);

GRANT EXEC ON TYPE ::[dbo].[HAServicePatientTableType] TO [HCVU];
GO
