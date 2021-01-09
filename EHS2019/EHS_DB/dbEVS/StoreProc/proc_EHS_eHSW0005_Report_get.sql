
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSW0005_Report_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    BEGIN
        DROP PROCEDURE [dbo].[proc_EHS_eHSW0005_Report_get];
    END;
GO

-- =============================================
-- Modification History
-- CR No.:			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================
-- Author:			Martin Tang
-- Create date:		21 July 2020
-- CR No.:			CRE19-022 
-- Description:		Inspection Schedule Weekly Report 
-- =============================================
--[dbo].[proc_EHS_eHSW0005_Report_get] ''

CREATE PROCEDURE [dbo].[proc_EHS_eHSW0005_Report_get] @request_time DATETIME = NULL
AS
    BEGIN
        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @current_dtm DATETIME;
        DECLARE @DisplayVisitPeriod VARCHAR(60);
        DECLARE @DisplayFollowPeriod VARCHAR(60);
        DECLARE @PeriodDays INT;
        DECLARE @In_Period_From DATETIME;
        DECLARE @In_Period_To DATETIME;
        DECLARE @InspectionStatusClosed VARCHAR(2);
        DECLARE @InspectionStatusRemoved VARCHAR(2);
        DECLARE @InspectionStatusIRI VARCHAR(2);
        DECLARE @InspectionStatusClosePA VARCHAR(2);
        -- Create Worksheet 01 Result Table  

        DECLARE @WS01 AS TABLE
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(1000) DEFAULT '', 
         Col02 VARCHAR(1000) DEFAULT '', 
         Col03 VARCHAR(1000) DEFAULT '', 
         Col04 VARCHAR(1000) DEFAULT '', 
         Col05 VARCHAR(2000) DEFAULT '', 
         Col06 VARCHAR(1000) DEFAULT '', 
         Col07 VARCHAR(1000) DEFAULT '', 
         Col08 NVARCHAR(1000) DEFAULT '', 
         Col09 VARCHAR(1000) DEFAULT '', 
         Col10 VARCHAR(1000) DEFAULT '', 
         Col11 NVARCHAR(1000) DEFAULT '', 
         Col12 VARCHAR(1000) DEFAULT '', 
         Col13 VARCHAR(1000) DEFAULT '', 
         Col14 VARCHAR(1000) DEFAULT '', 
         Col15 VARCHAR(1000) DEFAULT '', 
         Col16 VARCHAR(1000) DEFAULT '', 
         Col17 VARCHAR(1000) DEFAULT '', 
         Col18 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 02 Result Table  

        DECLARE @WS02 AS TABLE
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(1000) DEFAULT '', 
         Col02 VARCHAR(1000) DEFAULT '', 
         Col03 VARCHAR(1000) DEFAULT '', 
         Col04 VARCHAR(1000) DEFAULT '', 
         Col05 VARCHAR(1000) DEFAULT '', 
         Col06 VARCHAR(1000) DEFAULT '', 
         Col07 VARCHAR(2000) DEFAULT '', 
         Col08 VARCHAR(1000) DEFAULT '', 
         Col09 VARCHAR(1000) DEFAULT '', 
         Col10 VARCHAR(1000) DEFAULT '', 
         Col11 VARCHAR(1000) DEFAULT '', 
         Col12 VARCHAR(1000) DEFAULT '', 
         Col13 VARCHAR(1000) DEFAULT '', 
         Col14 VARCHAR(1000) DEFAULT '', 
         Col15 VARCHAR(1000) DEFAULT '', 
         Col16 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 03 Result Table  

        DECLARE @WS03 AS TABLE
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(1000) DEFAULT '', 
         Col02 VARCHAR(1000) DEFAULT '', 
         Col03 VARCHAR(2000) DEFAULT '', 
         Col04 VARCHAR(1000) DEFAULT '', 
         Col05 VARCHAR(1000) DEFAULT '', 
         Col06 VARCHAR(1000) DEFAULT '', 
         Col07 VARCHAR(1000) DEFAULT '', 
         Col08 VARCHAR(1000) DEFAULT '', 
         Col09 VARCHAR(1000) DEFAULT '', 
         Col10 VARCHAR(1000) DEFAULT '', 
         Col11 VARCHAR(1000) DEFAULT '', 
         Col12 VARCHAR(1000) DEFAULT '', 
         Col13 VARCHAR(1000) DEFAULT '', 
         Col14 VARCHAR(1000) DEFAULT '', 
         Col15 VARCHAR(1000) DEFAULT '', 
         Col16 VARCHAR(1000) DEFAULT ''
        );

        -- Create Worksheet 05 Remark

        DECLARE @Remark AS TABLE
        (Seq   INT IDENTITY(1, 1), 
         Col01 VARCHAR(1000) DEFAULT ''
        );

        -- =============================================  
        -- Initialization  
        -- =============================================  
        SET @PeriodDays = 14;  -- following 14 days  
        SET @In_Period_From = CAST(CAST(GETDATE() AS DATE) AS DATETIME);
        SET @In_Period_To = CAST(CAST(GETDATE() + @PeriodDays AS DATE) AS DATETIME);

        SET @InspectionStatusClosed = 'C';
        SET @InspectionStatusRemoved = 'D';
        SET @InspectionStatusIRI = 'RI'; --Inspection Result Inputted 
        SET @InspectionStatusClosePA = 'CC ';--Close (Pending Approval)

        SET @DisplayVisitPeriod = 'Visit Date: ' + CONVERT(CHAR(10), @In_Period_From, 111) + ' to ' + CONVERT(CHAR(10), @In_Period_To, 111);
        SET @DisplayFollowPeriod = 'Follow-up Date: ' + CONVERT(CHAR(10), @In_Period_From, 111) + ' to ' + CONVERT(CHAR(10), @In_Period_To, 111);
        SET @current_dtm = GETDATE();
        --OPEN SYMMETRIC KEY
        EXEC [proc_SymmetricKey_open]

        -- -----------------------------------------  
        -- Excel worksheet (Remark)  
        -- -----------------------------------------  

        INSERT INTO @Remark(Col01)
        VALUES('(A) Common Note(s) for the report'), ('1. Inspection Record:'), ('    a. The closed and removed inspection reocrds are excluded');

        ------------------------------------------------  
        -- For Excel Sheet (01): Visit  
        -- ---------------------------------------------  
        SELECT CONVERT(VARCHAR(10), ivi.Visit_Date, 111) AS Visit_Date, 
               CONVERT(VARCHAR(5), ivi.Visit_Begin_Dtm, 114) + ' - ' + CONVERT(VARCHAR(5), ivi.Visit_End_Dtm, 114) AS Visit_Time, 
               ivi.File_Reference_No, 
               ivi.Inspection_ID, 
               std.Data_Value AS Main_Type_Of_Inspection_Value, 
               ivi.SP_ID, 
               CONVERT(VARCHAR(MAX), DECRYPTBYKEY(CASE
                                                      WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                      @InspectionStatusClosePA)
                                                           OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                           @InspectionStatusClosePA)
                                                      THEN ivi.SP_Eng_Name
                                                      ELSE sp.Encrypt_Field2
                                                  END)) AS [SP_Eng_Name], 
               CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(CASE
                                                       WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                       @InspectionStatusClosePA)
                                                            OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                            @InspectionStatusClosePA)
                                                       THEN ivi.SP_Chi_Name
                                                       ELSE sp.Encrypt_Field3
                                                   END)) AS [SP_Chi_Name],
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Display_Seq
                   ELSE p.Display_Seq
               END AS Display_Seq,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name
                   ELSE p.Practice_Name
               END AS Practice_Name,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name_Chi
                   ELSE p.Practice_Name_Chi
               END AS Practice_Name_Chi, 
               pro.Service_Category_Code, 
               d.district_name, 
               d.district_board, 
               da.area_name, 
               sd.Status_Description, 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac1.Encrypt_Field2)), 'N/A') AS [Case_Officer], 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac2.Encrypt_Field2)), 'N/A') AS [Subject_Officer]
        INTO #ResultsWs01
        FROM InspectionVisitInfo AS ivi WITH(NOLOCK)
             INNER JOIN Practice AS p WITH(NOLOCK)
             ON ivi.SP_ID = p.SP_ID
                AND ivi.Practice_Display_Seq = p.Display_Seq
             INNER JOIN Professional AS pro WITH(NOLOCK)
             ON p.SP_ID = pro.SP_ID
                AND p.Professional_Seq = pro.Professional_Seq
             INNER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON ivi.SP_ID = sp.SP_ID
             INNER JOIN district AS d WITH(NOLOCK)
             ON p.District = d.district_code
             INNER JOIN district_area AS da WITH(NOLOCK)
             ON d.district_area = da.area_code
             INNER JOIN StatusData AS sd WITH(NOLOCK)
             ON ivi.Record_Status = sd.Status_Value
                AND sd.Enum_Class = 'InspectionStatus'
             LEFT OUTER JOIN HCVUUserAC AS hac1 WITH(NOLOCK)
             ON ivi.Case_Officer = hac1.User_ID
             LEFT OUTER JOIN HCVUUserAC AS hac2 WITH(NOLOCK)
             ON ivi.Subject_Officer = hac2.User_ID
             INNER JOIN dbo.StaticData AS std WITH(NOLOCK)
             ON ivi.Main_Type_Of_Inspection = std.Item_No
                AND std.Column_Name = 'TypeOfInspection'
        WHERE ivi.Visit_Date >= @In_Period_From
              AND ivi.Visit_Date < @In_Period_To + 1
              AND ivi.Record_Status NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved)
             AND ISNULL(ivi.Original_Status, '') NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved);
        INSERT INTO @WS01(Col01)
        VALUES(@DisplayVisitPeriod);
        INSERT INTO @WS01(Col01)
        VALUES('');
        INSERT INTO @WS01
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16, 
                Col17, 
                Col18
               )
        VALUES
              ('Visit Date', 
               'Visit Time', 
               'File Reference No.', 
               'Inspection Record ID', 
               'Main Type of Inspection', 
               'SPID', 
               'SP Name (English)', 
               'SP Name (Chinese)', 
               'Practice No.', 
               'Practice Name (English)', 
               'Practice Name (Chinese)', 
               'Profession', 
               'District', 
               'District Board', 
               'Area', 
               'Inspection Status', 
               'Case Officer', 
               'Subject Officer'
              );
        INSERT INTO @WS01
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16, 
                Col17, 
                Col18
               )
        SELECT r1.Visit_Date, 
               r1.Visit_Time, 
               r1.File_Reference_No, 
               r1.Inspection_ID, 
               r1.Main_Type_Of_Inspection_Value,
               r1.SP_ID, 
               r1.SP_Eng_Name, 
               r1.SP_Chi_Name, 
               r1.Display_Seq, 
               r1.Practice_Name, 
               r1.Practice_Name_Chi, 
               r1.Service_Category_Code, 
               r1.district_name, 
               r1.district_board, 
               r1.area_name, 
               r1.Status_Description, 
               r1.Case_Officer, 
               r1.Subject_Officer
        FROM #ResultsWs01 AS r1
        ORDER BY r1.Visit_Date, 
                 r1.Visit_Time, 
                 r1.File_Reference_No;

        ------------------------------------------------  
        -- For Excel Sheet (02): Follow-up Action  
        -- ---------------------------------------------  
        SELECT CONVERT(VARCHAR(10), ivi.Visit_Date, 111) AS Visit_Date, 
               CONVERT(VARCHAR(5), ivi.Visit_Begin_Dtm, 114) + ' - ' + CONVERT(VARCHAR(5), ivi.Visit_End_Dtm, 114) AS Visit_Time, 
               ivi.File_Reference_No, 
               ivi.Inspection_ID, 
               std.Data_Value AS Main_Type_Of_Inspection_Value, 
               ivi.SP_ID, 
               CONVERT(VARCHAR(MAX), DECRYPTBYKEY(CASE
                                                      WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                      @InspectionStatusClosePA)
                                                           OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                           @InspectionStatusClosePA)
                                                      THEN ivi.SP_Eng_Name
                                                      ELSE sp.Encrypt_Field2
                                                  END)) AS [SP_Eng_Name], 
               CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(CASE
                                                       WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                       @InspectionStatusClosePA)
                                                            OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                            @InspectionStatusClosePA)
                                                       THEN ivi.SP_Chi_Name
                                                       ELSE sp.Encrypt_Field3
                                                   END)) AS [SP_Chi_Name],
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Display_Seq
                   ELSE p.Display_Seq
               END AS Display_Seq,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name
                   ELSE p.Practice_Name
               END AS Practice_Name,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name_Chi
                   ELSE p.Practice_Name_Chi
               END AS Practice_Name_Chi, 
               sd.Status_Description, 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac1.Encrypt_Field2)), 'N/A') AS [Case_Officer], 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac2.Encrypt_Field2)), 'N/A') AS [Subject_Officer], 
               CONVERT(VARCHAR(10), ifua.Action_Date, 111) AS Action_Date, 
               ifua.Action_Desc
        INTO #ResultsWs02
        FROM InspectionVisitInfo AS ivi WITH(NOLOCK)
             INNER JOIN Practice AS p WITH(NOLOCK)
             ON ivi.SP_ID = p.SP_ID
                AND ivi.Practice_Display_Seq = p.Display_Seq
             INNER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON ivi.SP_ID = sp.SP_ID
             INNER JOIN StatusData AS sd WITH(NOLOCK)
             ON ivi.Record_Status = sd.Status_Value
                AND sd.Enum_Class = 'InspectionStatus'
             LEFT OUTER JOIN HCVUUserAC AS hac1 WITH(NOLOCK)
             ON ivi.Case_Officer = hac1.User_ID
             LEFT OUTER JOIN HCVUUserAC AS hac2 WITH(NOLOCK)
             ON ivi.Subject_Officer = hac2.User_ID
             INNER JOIN dbo.StaticData AS std WITH(NOLOCK)
             ON ivi.Main_Type_Of_Inspection = std.Item_No
                AND std.Column_Name = 'TypeOfInspection'
             INNER JOIN InspectionFollowUpAction AS ifua WITH(NOLOCK)
             ON ivi.Inspection_ID = ifua.Inspection_ID
        WHERE ifua.Action_Date >= @In_Period_From
              AND ifua.Action_Date < @In_Period_To + 1
              AND ivi.Record_Status NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved)
             AND ISNULL(ivi.Original_Status, '') NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved);

        INSERT INTO @WS02(Col01)
        VALUES(@DisplayFollowPeriod);

        INSERT INTO @WS02(Col01)
        VALUES('');

        INSERT INTO @WS02
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16
               )
        VALUES
              ('Case Officer', 
               'Subject Officer', 
               'Follow-up Date', 
               'Follow-up Action', 
               'File Reference No.', 
               'Inspection Record ID', 
               'Main Type of Inspection', 
               'SPID', 
               'SP Name (English)', 
               'SP Name (Chinese)', 
               'Practice No.', 
               'Practice Name (English)', 
               'Practice Name (Chinese)', 
               'Visit Date', 
               'Visit Time', 
               'Inspection Status'
              );
        INSERT INTO @WS02
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16
               )
        SELECT r1.Case_Officer, 
               r1.Subject_Officer, 
               r1.Action_Date, 
               r1.Action_Desc, 
               r1.File_Reference_No, 
               r1.Inspection_ID, 
               r1.Main_Type_Of_Inspection_Value,
               r1.SP_ID, 
               r1.SP_Eng_Name, 
               r1.SP_Chi_Name, 
               r1.Display_Seq, 
               r1.Practice_Name, 
               r1.Practice_Name_Chi, 
               r1.Visit_Date, 
               r1.Visit_Time, 
               r1.Status_Description
        FROM #ResultsWs02 AS r1
        ORDER BY r1.Case_Officer, 
                 r1.Subject_Officer, 
                 r1.Action_Date, 
                 r1.Action_Desc, 
                 r1.File_Reference_No;
        ------------------------------------------------  
        -- For Excel Sheet (03): No Visit Schedule
        -- ---------------------------------------------  

        SELECT ivi.File_Reference_No, 
               ivi.Inspection_ID, 
               std.Data_Value AS Main_Type_Of_Inspection_Value, 
               ivi.SP_ID, 
               CONVERT(VARCHAR(MAX), DECRYPTBYKEY(CASE
                                                      WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                      @InspectionStatusClosePA)
                                                           OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                           @InspectionStatusClosePA)
                                                      THEN ivi.SP_Eng_Name
                                                      ELSE sp.Encrypt_Field2
                                                  END)) AS [SP_Eng_Name], 
               CONVERT(NVARCHAR(MAX), DECRYPTBYKEY(CASE
                                                       WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                       @InspectionStatusClosePA)
                                                            OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI,
                                                            @InspectionStatusClosePA)
                                                       THEN ivi.SP_Chi_Name
                                                       ELSE sp.Encrypt_Field3
                                                   END)) AS [SP_Chi_Name],
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Display_Seq
                   ELSE p.Display_Seq
               END AS Display_Seq,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name
                   ELSE p.Practice_Name
               END AS Practice_Name,
               CASE
                   WHEN ivi.Record_Status IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                        OR ISNULL(ivi.Original_Status, '') IN(@InspectionStatusClosed, @InspectionStatusIRI, @InspectionStatusClosePA)
                   THEN ivi.Practice_Name_Chi
                   ELSE p.Practice_Name_Chi
               END AS Practice_Name_Chi, 
               pro.Service_Category_Code, 
               d.district_name, 
               d.district_board, 
               da.area_name, 
               sd.Status_Description, 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac1.Encrypt_Field2)), 'N/A') AS [Case_Officer], 
               ISNULL(CONVERT(VARCHAR(MAX), DECRYPTBYKEY(hac2.Encrypt_Field2)), 'N/A') AS [Subject_Officer]
        INTO #ResultsWs03
        FROM InspectionVisitInfo AS ivi WITH(NOLOCK)
             INNER JOIN Practice AS p WITH(NOLOCK)
             ON ivi.SP_ID = p.SP_ID
                AND ivi.Practice_Display_Seq = p.Display_Seq
             INNER JOIN Professional AS pro WITH(NOLOCK)
             ON p.SP_ID = pro.SP_ID
                AND p.Professional_Seq = pro.Professional_Seq
             INNER JOIN ServiceProvider AS sp WITH(NOLOCK)
             ON ivi.SP_ID = sp.SP_ID
             INNER JOIN district AS d WITH(NOLOCK)
             ON p.District = d.district_code
             INNER JOIN district_area AS da WITH(NOLOCK)
             ON d.district_area = da.area_code
             INNER JOIN StatusData AS sd WITH(NOLOCK)
             ON ivi.Record_Status = sd.Status_Value
                AND sd.Enum_Class = 'InspectionStatus'
             LEFT OUTER JOIN HCVUUserAC AS hac1 WITH(NOLOCK)
             ON ivi.Case_Officer = hac1.User_ID
             LEFT OUTER JOIN HCVUUserAC AS hac2 WITH(NOLOCK)
             ON ivi.Subject_Officer = hac2.User_ID
             INNER JOIN dbo.StaticData AS std WITH(NOLOCK)
             ON ivi.Main_Type_Of_Inspection = std.Item_No
                AND std.Column_Name = 'TypeOfInspection'
        WHERE ivi.Visit_Date IS NULL
              AND ivi.Record_Status NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved)
             AND ISNULL(ivi.Original_Status, '') NOT IN(@InspectionStatusClosed, @InspectionStatusRemoved)
        ORDER BY ivi.File_Reference_No, 
                 ivi.Inspection_ID;

        INSERT INTO @WS03
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16
               )
        VALUES
              ('File Reference No.', 
               'Inspection Record ID', 
               'Main Type of Inspection', 
               'SPID', 
               'SP Name (English)', 
               'SP Name (Chinese)', 
               'Practice No.', 
               'Practice Name (English)', 
               'Practice Name (Chinese)', 
               'Profession', 
               'District', 
               'District Board', 
               'Area', 
               'Inspection Status', 
               'Case Officer', 
               'Subject Officer'
              );

        INSERT INTO @WS03
               (Col01, 
                Col02, 
                Col03, 
                Col04, 
                Col05, 
                Col06, 
                Col07, 
                Col08, 
                Col09, 
                Col10, 
                Col11, 
                Col12, 
                Col13, 
                Col14, 
                Col15, 
                Col16
               )
        SELECT r1.File_Reference_No, 
               r1.Inspection_ID, 
               r1.Main_Type_Of_Inspection_Value,
               r1.SP_ID, 
               r1.SP_Eng_Name, 
               r1.SP_Chi_Name, 
               r1.Display_Seq, 
               r1.Practice_Name, 
               r1.Practice_Name_Chi, 
               r1.Service_Category_Code, 
               r1.district_name, 
               r1.district_board, 
               r1.area_name, 
               r1.Status_Description, 
               r1.Case_Officer, 
               r1.Subject_Officer
        FROM #ResultsWs03 AS r1
        ORDER BY r1.File_Reference_No, 
                 r1.Inspection_ID;

       EXEC [proc_SymmetricKey_close]

        -- =============================================  
        -- Return results  
        -- =============================================   

        SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), @current_dtm, 111) + ' ' + CONVERT(VARCHAR(5), @current_dtm, 114); --The first Sheet

        SELECT w.Col01, 
               w.Col02, 
               w.Col03, 
               w.Col04, 
               w.Col05, 
               w.Col06, 
               w.Col07, 
               w.Col08, 
               w.Col09, 
               w.Col10, 
               w.Col11, 
               w.Col12, 
               w.Col13, 
               w.Col14, 
               w.Col15, 
               w.Col16, 
               w.Col17, 
               w.Col18
        FROM @WS01 AS w
        ORDER BY w.Seq;

        SELECT w.Col01, 
               w.Col02, 
               w.Col03, 
               w.Col04, 
               w.Col05, 
               w.Col06, 
               w.Col07, 
               w.Col08, 
               w.Col09, 
               w.Col10, 
               w.Col11, 
               w.Col12, 
               w.Col13, 
               w.Col14, 
               w.Col15, 
               w.Col16
        FROM @WS02 AS w
        ORDER BY w.Seq;
        SELECT w.Col01, 
               w.Col02, 
               w.Col03, 
               w.Col04, 
               w.Col05, 
               w.Col06, 
               w.Col07, 
               w.Col08, 
               w.Col09, 
               w.Col10, 
               w.Col11, 
               w.Col12, 
               w.Col13, 
               w.Col14, 
               w.Col15, 
               w.Col16
        FROM @WS03 AS w
        ORDER BY w.Seq;

        SELECT r.Col01
        FROM @Remark AS r
        ORDER BY r.Seq;

        IF OBJECT_ID('tempdb..#ResultsWs01') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWs01;
            END;
        IF OBJECT_ID('tempdb..#ResultsWs02') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWs02;
            END;

        IF OBJECT_ID('tempdb..#ResultsWs03') IS NOT NULL
            BEGIN
                DROP TABLE #ResultsWs03;
            END;
    END;
GO

GRANT EXECUTE ON [dbo].[proc_EHS_eHSW0005_Report_get] TO HCVU;
GO