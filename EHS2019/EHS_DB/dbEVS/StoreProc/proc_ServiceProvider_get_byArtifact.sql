IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_ServiceProvider_get_byArtifact]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_ServiceProvider_get_byArtifact];
    END;
GO
SET ANSI_NULLS ON;
GO
SET QUOTED_IDENTIFIER ON;
GO
-- =============================================
-- Modification History
-- CR No.:			 
-- Modified by:		
-- Modified date:	
-- Description:		
-- =============================================
-- =============================================
-- Modification History
-- CR No.:			CRE20-006 
-- Modified by:		Nichole Ip
-- Modified date:	20 JUL 2020
-- Description:		1. Handle the artifact on eHS(S) login
-- =============================================

CREATE PROCEDURE [dbo].[proc_ServiceProvider_get_byArtifact] 
	@Artifact NVARCHAR(100), 
	@Timeout  NVARCHAR(4)
AS
    BEGIN
        
		-- ============================================================
        -- Declaration
        -- ============================================================
		-- ============================================================
        -- Validation
        -- ============================================================
        -- ============================================================
        -- Initialization
        -- ============================================================ 
		-- counter check of artifact
		UPDATE DHCClaimAccess
          SET 
              Access_ehs_count = ISNULL(Access_ehs_count, 0) + 1
        WHERE artifact = @Artifact;

		-- set the timeout time
        IF LEN(RTRIM(@Timeout)) < 1
            BEGIN
                SET @Timeout = '1440';
            END;
        OPEN SYMMETRIC KEY sym_Key DECRYPTION BY ASYMMETRIC KEY asym_Key;  

        -- ============================================================
        -- Return results
        -- ============================================================

        SELECT P.SP_ID, 
               DCA.Service_Category_Code AS ProfCode, 
               DCA.Registration_Code AS ProfRegNo, 
               Doc_Code AS DocType, 
               HKID AS HKID, 
               HKIC_Symbol AS HKICSymbol, 
               DOBFormat AS DOBFormat, 
               DOB AS DOB, 
               DHCDistrictCode AS DHCDistrictCode, 
               Claim_Amount AS ClaimAmount, 
               CONVERT(VARCHAR, DECRYPTBYKEY(Encrypt_Field1)) AS [SP_HKID],
               CASE
                   WHEN(DATEDIFF(MINUTE, DCA.Create_Dtm, GETDATE()) > @Timeout
                        OR Access_ehs_count > 1)
                   THEN 'Y'
                   ELSE 'N'
               END AS Expired
        FROM DHCClaimAccess DCA WITH(NOLOCK) 
             INNER JOIN Professional P  WITH(NOLOCK) ON DCA.Service_Category_Code = P.Service_Category_Code
                                          AND DCA.Registration_Code = P.Registration_Code
        WHERE artifact = @Artifact;
        CLOSE SYMMETRIC KEY sym_Key;
    END;
GO
GRANT EXECUTE ON [dbo].[proc_ServiceProvider_get_byArtifact] TO HCSP;
GO