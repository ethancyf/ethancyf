IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_SchemeBackOffice_GetEligibleProfessional]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_SchemeBackOffice_GetEligibleProfessional]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:			Tommy Lam
-- Create date:		07 Nov 2012
-- Description:		Get the list of eligible professional
-- =============================================

CREATE PROCEDURE [dbo].[proc_SchemeBackOffice_GetEligibleProfessional] 
	@scheme_code		char(10),
	@period_from		datetime,
	@period_to			datetime
AS BEGIN

-- =============================================
-- Declaration
-- =============================================

	DECLARE @delimiter varchar(5)
	DECLARE @eligible_professional varchar(100)

	DECLARE @EligibleProfessionalTemp table (
		Eligible_Professional	varchar(100)
	)

	DECLARE @EligibleProfessional table (
		Eligible_Professional	char(5)
	)

-- =============================================
-- Validation
-- =============================================

	IF @scheme_code IS NULL
		SET @scheme_code = ''

	IF @period_from = ''
		SET @period_from = NULL

	IF @period_to = ''
		SET @period_to = NULL

	IF @period_from > @period_to AND (NOT(@period_from IS NULL)) AND (NOT(@period_to IS NULL))
		RETURN

-- =============================================
-- Initialization
-- =============================================

	SET @delimiter = ','

	IF @scheme_code = 'EVSS'
		SET @scheme_code = 'EVSSHSIVSS'

	INSERT INTO @EligibleProfessionalTemp (
		Eligible_Professional
		)
	SELECT Eligible_Professional
	FROM SchemeBackOffice
	WHERE (@scheme_code = '' OR @scheme_code = Scheme_Code)
		AND ((NOT ((@period_from < Effective_Dtm AND @period_to < Effective_Dtm) OR (@period_from >= Expiry_dtm AND @period_to >= Expiry_dtm)))
			OR (@period_from IS NULL AND @period_to IS NULL)
			OR (@period_from IS NULL AND @period_to >= Effective_Dtm)
			OR (@period_to IS NULL AND @period_from < Expiry_dtm))
	GROUP BY Eligible_Professional

-- =============================================
-- Retrieve Data
-- =============================================

	IF (SELECT COUNT(1)
			FROM @EligibleProfessionalTemp
			WHERE Eligible_Professional = 'ALL') > 0
		BEGIN

			INSERT INTO @EligibleProfessional (
				Eligible_Professional
				)
			SELECT DISTINCT Service_Category_Code FROM Profession

		END

	ELSE
		BEGIN

			DECLARE Eligible_Professional_Cursor CURSOR FOR (
				SELECT Eligible_Professional
				FROM @EligibleProfessionalTemp
				)

			OPEN Eligible_Professional_Cursor

			FETCH NEXT FROM Eligible_Professional_Cursor INTO @eligible_professional

			WHILE (@@FETCH_STATUS = 0)
				BEGIN
					INSERT INTO @EligibleProfessional (
						Eligible_Professional
						)
					SELECT Item FROM func_split_string(@eligible_professional, @delimiter)

					FETCH NEXT FROM Eligible_Professional_Cursor INTO @eligible_professional
				END

			CLOSE Eligible_Professional_Cursor
			DEALLOCATE Eligible_Professional_Cursor

		END

-- =============================================
-- Return results
-- =============================================

	SELECT DISTINCT Eligible_Professional
	FROM @EligibleProfessional
	ORDER BY Eligible_Professional

END
GO

GRANT EXECUTE ON [dbo].[proc_SchemeBackOffice_GetEligibleProfessional] TO HCVU
GO
