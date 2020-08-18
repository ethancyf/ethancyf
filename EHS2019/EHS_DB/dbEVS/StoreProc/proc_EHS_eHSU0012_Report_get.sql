
IF EXISTS
(
    SELECT *
    FROM dbo.sysobjects
    WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_eHSU0012_Report_get]')
          AND OBJECTPROPERTY(id, N'IsProcedure') = 1
)
    DROP PROCEDURE [dbo].[proc_EHS_eHSU0012_Report_get];
GO

-- =============================================
-- Author:			Martin Tang
-- Create date:		08 July 2020
-- CR No.:			CRE19-022 
-- Description:		Get Report for CRE19-022
-- =============================================
--[dbo].[proc_EHS_eHSU0012_Report_get] '','2020-01-01 00:00:00','2020-06-30 00:00:00'

CREATE PROCEDURE [dbo].[proc_EHS_eHSU0012_Report_get] @request_time DATETIME = null, 
                                                      @Period_From    DATETIME= null, 
                                                      @Period_To      DATETIME= null
AS
    BEGIN
-- =============================================  
-- Declaration  
-- =============================================  

DECLARE @current_dtm DATETIME;
DECLARE @DisplayVisitPeriod VARCHAR(60);
DECLARE @seq INT;
DECLARE @In_Period_From DATETIME= @Period_From;
DECLARE @In_Period_To DATETIME= @Period_To;

-- Create Worksheet 01 Result Table  
 DECLARE @WS01 AS TABLE(  
  Seq  INT,  
  Col01 VARCHAR(100) DEFAULT '',  
  Col02 VARCHAR(100) DEFAULT ''  
 )  

 -- Create Worksheet 02 Result Table  
 DECLARE @WS02 AS TABLE(  
  Seq  INT,  
  Col01 VARCHAR(1000) DEFAULT '',  
  Col02 VARCHAR(100) DEFAULT '' 
 )  
  -- Create Worksheet 03 Result Table  
 DECLARE @WS03 AS TABLE(  
  Seq  INT,   
  Col01 VARCHAR(100) DEFAULT '',  
  Col02 VARCHAR(100) DEFAULT '',
  Col03 VARCHAR(100) DEFAULT '' 
 )  

 -- Create Worksheet 04 Result Table  
DECLARE @WS04 AS TABLE
(Seq   INT IDENTITY(1, 1), 
 Col01 VARCHAR(100) DEFAULT '', 
 Col02 VARCHAR(100) DEFAULT '', 
 Col03 VARCHAR(100) DEFAULT '', 
 Col04 VARCHAR(100) DEFAULT '', 
 Col05 VARCHAR(100) DEFAULT '', 
 Col06 VARCHAR(100) DEFAULT '', 
 Col07 VARCHAR(100) DEFAULT '', 
 Col08 VARCHAR(100) DEFAULT '', 
 Col09 VARCHAR(100) DEFAULT '', 
 Col10 VARCHAR(100) DEFAULT '', 
 Col11 VARCHAR(100) DEFAULT '', 
 Col12 VARCHAR(100) DEFAULT '', 
 Col13 VARCHAR(100) DEFAULT '', 
 Col14 VARCHAR(100) DEFAULT '', 
 Col15 VARCHAR(100) DEFAULT '', 
 Col16 VARCHAR(100) DEFAULT ''
); 

-- Create Worksheet 05 Result Table  
DECLARE @WS05 AS TABLE
(Seq   INT IDENTITY(1, 1), 
 Col01 VARCHAR(100) DEFAULT '', 
 Col02 VARCHAR(100) DEFAULT '', 
 Col03 VARCHAR(100) DEFAULT '', 
 Col04 VARCHAR(100) DEFAULT '', 
 Col05 VARCHAR(100) DEFAULT '', 
 Col06 VARCHAR(100) DEFAULT '', 
 Col07 VARCHAR(100) DEFAULT '', 
 Col08 VARCHAR(100) DEFAULT '', 
 Col09 VARCHAR(100) DEFAULT '', 
 Col10 VARCHAR(100) DEFAULT '', 
 Col11 VARCHAR(100) DEFAULT '', 
 Col12 VARCHAR(100) DEFAULT '', 
 Col13 VARCHAR(100) DEFAULT '', 
 Col14 VARCHAR(100) DEFAULT '', 
 Col15 VARCHAR(100) DEFAULT '', 
 Col16 VARCHAR(100) DEFAULT ''
); 

-- Create Worksheet 06 Result Table  
DECLARE @WS06 AS TABLE
(Seq   INT IDENTITY(1, 1), 
 Col01 VARCHAR(100) DEFAULT '', 
 Col02 VARCHAR(100) DEFAULT '', 
 Col03 VARCHAR(100) DEFAULT '', 
 Col04 VARCHAR(100) DEFAULT '', 
 Col05 VARCHAR(100) DEFAULT '', 
 Col06 VARCHAR(100) DEFAULT '', 
 Col07 VARCHAR(100) DEFAULT '', 
 Col08 VARCHAR(100) DEFAULT '', 
 Col09 VARCHAR(100) DEFAULT '', 
 Col10 VARCHAR(100) DEFAULT '', 
 Col11 VARCHAR(100) DEFAULT '', 
 Col12 VARCHAR(100) DEFAULT '', 
 Col13 VARCHAR(100) DEFAULT '', 
 Col14 VARCHAR(100) DEFAULT '', 
 Col15 VARCHAR(100) DEFAULT '', 
 Col16 VARCHAR(100) DEFAULT ''
); 

-- Create Worksheet 07 Remark
DECLARE @Remark AS TABLE
(Seq   INT IDENTITY(1, 1), 
 Col01 VARCHAR(1000) DEFAULT ''
);

--General table
DECLARE @Profession AS TABLE
(Service_Category_Code VARCHAR(3), 
 DisplayOrder          INT IDENTITY(1, 1)
);
 -- =============================================  
-- Initialization  
-- =============================================  

SET @DisplayVisitPeriod = 'Visit Period: ' + CONVERT(CHAR(10), @In_Period_From, 111) + ' to ' + CONVERT(CHAR(10), @In_Period_To, 111);
SET @current_dtm = GETDATE() 

-- Profession 
INSERT INTO @Profession(Service_Category_Code)
VALUES('ENU');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RCM');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RCP');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RDT');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RMP');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RMT');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RNU');
INSERT INTO @Profession(Service_Category_Code)
VALUES('ROP');
INSERT INTO @Profession(Service_Category_Code)
VALUES('ROT');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RPT');
INSERT INTO @Profession(Service_Category_Code)
VALUES('RRD');
INSERT INTO @Profession(Service_Category_Code)
VALUES('DIT');
INSERT INTO @Profession(Service_Category_Code)
VALUES('POD');
INSERT INTO @Profession(Service_Category_Code)
VALUES('SPT');

--result table
SELECT pro.Service_Category_Code, 
       ivi.File_Reference_No, 
       ivi.SP_ID, 
       ivi.Main_Type_Of_Inspection, 
       ivi.Original_Status, 
       ivi.Record_Status, 
       ivi.Inspection_ID, 
       ivi.No_Of_TotalCheck, 
       ivi.No_Of_InOrder, 
       ivi.No_Of_MissingForm, 
       ivi.No_Of_Inconsistent, 
       ivi.Anomalous_Claims, 
       ivi.No_Of_Anomalous_Claims, 
       ivi.Is_OverMajor, 
       ivi.No_Of_Is_OverMajor, 
       ivi.Advisory_Letter_Date, 
       ivi.Warning_Letter_Date, 
       ivi.Delist_Letter_Date, 
       ivi.Suspend_Payment_Letter_Date, 
       ivi.Suspend_EHCP_Account_Letter_Date, 
       ivi.Other_Letter_Date, 
       ivi.BoardAndCouncil_Date, 
       ivi.Police_Date, 
       ivi.Social_Welfare_Department_Date, 
       ivi.HK_Customs_And_Excise_Department_Date, 
       ivi.Immigration_Department_Date, 
       ivi.Labour_Department_Date, 
       ivi.Other_Party_Date, 
       ivi.Suspend_EHCP_Date, 
       ivi.Delist_EHCP_Date, 
       ivi.Payment_RecoverySuspension_Date
INTO #Results
FROM dbo.InspectionVisitInfo AS ivi WITH(NOLOCK)
     INNER JOIN dbo.Practice AS p WITH(NOLOCK)
     ON ivi.SP_ID = p.SP_ID
        AND ivi.Practice_Display_Seq = p.Display_Seq
     INNER JOIN dbo.Professional AS pro WITH(NOLOCK)
     ON p.SP_ID = pro.SP_ID
        AND p.Professional_Seq = pro.Professional_Seq
WHERE ivi.Visit_Date >= @In_Period_From
      AND ivi.Visit_Date < @In_Period_To + 1
      AND ivi.record_status <> 'D';


------For Excel Sheet (04-06) table: (With result)

SELECT *
INTO #ResultsWithResult
FROM #Results AS r
WHERE Record_Status NOT IN('I', 'PV')
AND ISNULL(Original_Status, '') NOT IN('I', 'PV');

------------------------------------------------  
-- For Excel Sheet (01): Summary  
-- ---------------------------------------------  

SET @seq = 0  
  
 INSERT INTO @WS01 (Seq, Col01)  
 VALUES (@seq, @DisplayVisitPeriod)  
  
 SET @seq = @seq + 1      

 INSERT INTO @WS01 (Seq) VALUES (@seq)  
 INSERT INTO @WS01 (Seq) VALUES (@seq) 

INSERT INTO @WS01
SELECT @seq + 1, 
       'No. of Inspections:', 
       COUNT(DISTINCT r.Inspection_ID)
FROM #Results AS r
UNION
SELECT @seq + 2, 
       'No. of Service Providers Involved:', 
       COUNT(DISTINCT r.SP_ID)
FROM #Results AS r
UNION
SELECT @seq + 3, 
       'No. of File Reference:', 
       COUNT(DISTINCT r.File_Reference_No)
FROM #Results AS r
UNION
SELECT @seq + 4, 
       'No. of Claims:', 
       ISNULL(SUM(r.No_Of_TotalCheck),0)
FROM #Results AS r
UNION
SELECT @seq + 5, 
       'No. of Case Closed:', 
       COUNT(DISTINCT r.Inspection_ID)
FROM #Results AS r
WHERE r.record_status = 'C';

------------------------------------------------  
-- For Excel Sheet (02): Type of Inspection  
------------------------------------------------  
  
 SET @seq = 0  
  
 INSERT INTO @WS02 (Seq, Col01)  
 VALUES (@seq, @DisplayVisitPeriod)  

 SET @seq = @seq + 1;      
 INSERT INTO @WS02(Seq)
 VALUES(@seq);  
 INSERT INTO @WS02(Seq)
 VALUES(@seq);
 SET @seq = @seq + 1;
 INSERT INTO @WS02 (Seq, Col01, Col02)  
 VALUES (@seq, 'Main Type of Inspection', 'No. of Inspection (Count by Inspection ID)') 


SELECT sd.Display_Order, 
       sd.Data_Value, 
       COUNT(DISTINCT r.Inspection_ID) AS total
INTO #ResultsWs02
FROM #Results AS r
     RIGHT OUTER JOIN dbo.StaticData AS sd WITH(NOLOCK)
     ON r.Main_Type_Of_Inspection = sd.Item_No
WHERE sd.Column_Name = 'TypeOfInspection'
GROUP BY sd.Data_Value, 
         sd.Display_Order;

		 
INSERT INTO @WS02
SELECT r.Display_Order, 
       r.Data_Value, 
       r.total
FROM #ResultsWs02 AS r;


INSERT INTO @WS02
SELECT 99, 
       'Total', 
       SUM(r.total) 
FROM #ResultsWs02 r

------------------------------------------------  
-- For Excel Sheet (03): Profession  
-----------------------------------------------  
 SET @seq = 0  
  
 INSERT INTO @WS03 (Seq, Col01)  
 VALUES (@seq, @DisplayVisitPeriod)  
  
 SET @seq = @seq + 1      
 INSERT INTO @WS03(Seq)
 VALUES(@seq);  
 INSERT INTO @WS03(Seq)
 VALUES(@seq);

 SET @seq = @seq + 1 
 INSERT INTO @WS03 (Seq, Col01, Col02, Col03)  
 VALUES (@seq, 'Profession', 'No. of Inspection (Count by Inspection ID)', 'No. of SP Involved (Count by SPID)') 

 SET @seq = @seq + 1 

INSERT INTO @WS03 (Seq, Col01, Col02, Col03) 
SELECT p.DisplayOrder+@seq, 
       p.Service_Category_Code, 
       COUNT(DISTINCT  r.Inspection_ID), 
       COUNT(DISTINCT  r.SP_ID)
FROM #Results AS r
     RIGHT OUTER JOIN @Profession AS p
     ON r.Service_Category_Code = p.Service_Category_Code
GROUP BY p.Service_Category_Code,p.DisplayOrder


INSERT INTO @WS03 (Seq, Col01, Col02, Col03) 
SELECT 99,
       'Total',
       COUNT(DISTINCT  r.Inspection_ID), 
       COUNT(DISTINCT  r.SP_ID)
FROM #Results AS r

------------------------------------------------  
-- For Excel Sheet (04): Claims by Prof  
-----------------------------------------------  
 
INSERT INTO @WS04(Col01)
VALUES(@DisplayVisitPeriod);  
INSERT INTO @WS04(Col01)
VALUES('');  
INSERT INTO @WS04(Col01)
VALUES('');

INSERT INTO @WS04(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');

SELECT p.Service_Category_Code, 
       ISNULL(SUM(r.No_Of_InOrder), 0) AS inorder, 
       ISNULL(SUM(r.No_Of_MissingForm), 0) AS MissingForm, 
       ISNULL(SUM(r.No_Of_Inconsistent), 0) AS Inconsistent
INTO #ResultsClaims
FROM #ResultsWithResult AS r
     RIGHT OUTER JOIN @Profession AS p
     ON r.Service_Category_Code = p.Service_Category_Code
GROUP BY p.Service_Category_Code;

--Goal: Transform the data
--original (Assume only 3 Service_Category_Code)
--Service_Category_Code | Inorder  | MissingForm  | Inconsistent 
--ENU					     1         5            1     
--POD					     8         4            3     
--RCM					     0         0            0     

--The inner query with the 'UNPIVOT' performs the same function as the UNION ALL. 
--It takes the list of columns and turns it into rows
--Service_Category_Code | Type       | value
--   ENU                  Inorder	     1
--   ENU                  MissingForm	 5
--   ENU                  Inconsistent   1
--   POD                  Inorder	     8
--   POD                  MissingForm	 4
--   POD                  Inconsistent   3
--   RCM                  Inorder	     0
--   RCM                  MissingForm	 0
--   RCM                  Inconsistent   0

--Then, the 'PIVOT' then performs the final transformation into columns.

--Type        | ENU | POD | RCM
--MissingForm	 5	   4	 0
--Inorder	     1	   8	 0
--Inconsistent	 1	   3	 0

SELECT Claim, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs04
FROM
(
    SELECT Service_Category_Code, 
           Claim, 
           value
    FROM #ResultsClaims AS r UNPIVOT(value FOR Claim IN(inorder, 
                                                  MissingForm, 
                                                  Inconsistent)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv

INSERT INTO @WS04(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs04
WHERE Claim='inorder'

update @WS04
set Col01='In Order'
where Col01='inorder'

INSERT INTO @WS04(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs04
WHERE Claim='MissingForm'

update @WS04
set Col01='Missing Form'
where Col01='MissingForm'

INSERT INTO @WS04(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs04
WHERE Claim='Inconsistent'

INSERT INTO @WS04(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs04;

------------------------------------------------  
-- For Excel Sheet (05): Finding
-----------------------------------------------  
 

INSERT INTO @WS05(Col01)
VALUES(@DisplayVisitPeriod);  
INSERT INTO @WS05(Col01)
VALUES('');  
INSERT INTO @WS05(Col01)
VALUES('');

-- 5(i) Anomalous Cases 
INSERT INTO @WS05(Col01)
VALUES('(i) Anomalous Cases');

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT', 'POD', 'SPT','Total');


SELECT Anomalous, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs051
FROM
(
    SELECT Service_Category_Code, 
           Anomalous, 
           value
    FROM
    (
        SELECT p.Service_Category_Code,
               SUM(CASE
                   WHEN r.Anomalous_Claims = 'N'
                   THEN 1
                   ELSE 0
               END) AS 'No',
               SUM(CASE
                   WHEN r.Anomalous_Claims = 'Y'
                   THEN 1
                   ELSE 0
               END) AS 'Yes'
        FROM #ResultsWithResult AS r
             RIGHT OUTER JOIN @Profession AS p
             ON r.Service_Category_Code = p.Service_Category_Code
        GROUP BY p.Service_Category_Code
    ) AS r UNPIVOT(value FOR Anomalous IN(Yes, 
                                          No)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs051
WHERE Anomalous='Yes'
INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs051
WHERE Anomalous='No'

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs051;


-- 5(ii) >40% Major Irregularities

INSERT INTO @WS05(Col01)
VALUES('');  
INSERT INTO @WS05(Col01)
VALUES('');
INSERT INTO @WS05(Col01)
VALUES('(ii) >40% Major Irregularities');


INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');


SELECT overMajor, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs052
FROM
(
    SELECT Service_Category_Code, 
           overMajor, 
           value
    FROM
    (
        SELECT p.Service_Category_Code,
               SUM(CASE
                   WHEN r.Is_OverMajor = 'N'
                   THEN 1
                   ELSE 0
               END) AS 'No',
               SUM(CASE
                   WHEN r.Is_OverMajor = 'Y'
                   THEN 1
                   ELSE 0
               END) AS 'Yes'
        FROM #ResultsWithResult AS r
             RIGHT OUTER JOIN @Profession AS p
             ON r.Service_Category_Code = p.Service_Category_Code
        GROUP BY p.Service_Category_Code
    ) AS r UNPIVOT(value FOR overMajor IN(Yes, 
                                               No)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs052
WHERE overMajor='Yes'
INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs052
WHERE overMajor='No'

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs052;



-- 5(iii) No. of records

INSERT INTO @WS05(Col01)
VALUES('');  
INSERT INTO @WS05(Col01)
VALUES('');
INSERT INTO @WS05(Col01)
VALUES('(iii) No. of records');


INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');
		 
SELECT Type, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs053
FROM
(
    SELECT Service_Category_Code, 
           Type, 
           value
    FROM
    (
        SELECT p.Service_Category_Code, 
               ISNULL(SUM(No_Of_Anomalous_Claims), 0) AS Anomalous, 
               ISNULL(SUM(No_Of_Is_OverMajor), 0) AS overMajor
        FROM #ResultsWithResult AS r
             RIGHT OUTER JOIN @Profession AS p
             ON r.Service_Category_Code = p.Service_Category_Code
        GROUP BY r.Is_OverMajor, 
                 p.Service_Category_Code
    ) AS r UNPIVOT(value FOR Type IN(Anomalous, 
                                     overMajor)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs053
WHERE Type='Anomalous'

update @WS05
set Col01='Anomalous Cases'
where Col01='Anomalous'

INSERT INTO @WS05(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs053
WHERE Type='overMajor'

update @WS05
set Col01='> 40% Major Irregularities'
where Col01='overMajor'


------------------------------------------------  
-- For Excel Sheet (06): Action
-----------------------------------------------  

--6(i) Issue Letter
 INSERT INTO @WS06(Col01)
VALUES(@DisplayVisitPeriod);  
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01)
VALUES('(i) Issue Letter');
INSERT INTO @WS06(Col01)
VALUES(''); 
INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');

SELECT Type, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs061
FROM
(
    SELECT Service_Category_Code, 
           Type, 
           value
    FROM
    (
        SELECT p.Service_Category_Code, 
               COUNT(r.Advisory_Letter_Date) AS Advisory, 
               COUNT(r.Warning_Letter_Date) AS Warning, 
               COUNT(r.Delist_Letter_Date) AS Delist, 
               COUNT(r.Suspend_Payment_Letter_Date) AS Payment, 
               COUNT(r.Suspend_EHCP_Account_Letter_Date) AS EHCP, 
               COUNT(r.Other_Letter_Date) AS Others, 
               SUM(CASE
                       WHEN(r.Advisory_Letter_Date IS NULL
                            AND r.Warning_Letter_Date IS NULL
                            AND r.Delist_Letter_Date IS NULL
                            AND r.Suspend_EHCP_Account_Letter_Date IS NULL
                            AND r.Suspend_Payment_Letter_Date IS NULL
                            AND r.Other_Letter_Date IS NULL)
                           AND r.Inspection_ID IS NOT NULL
                       THEN 1
                       ELSE 0
                   END) AS withoutLetter, 
               SUM(CASE
                       WHEN(r.Advisory_Letter_Date IS NOT NULL
                            OR r.Warning_Letter_Date IS NOT NULL
                            OR r.Delist_Letter_Date IS NOT NULL
                            OR r.Suspend_EHCP_Account_Letter_Date IS NOT NULL
                            OR r.Suspend_Payment_Letter_Date IS NOT NULL
                            OR r.Other_Letter_Date IS NOT NULL)
                       THEN 1
                       ELSE 0
                   END) AS withLetter
        FROM #ResultsWithResult AS r
             RIGHT OUTER JOIN @Profession AS p
             ON r.Service_Category_Code = p.Service_Category_Code
        GROUP BY p.Service_Category_Code
    ) AS r UNPIVOT(value FOR Type IN(Advisory, 
                                     Warning, 
                                     Delist, 
                                     Payment, 
                                     EHCP, 
                                     Others, 
                                     withoutLetter, 
                                     withLetter)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='Advisory'

update @WS06
set Col01='Advisorty Letter'
where Col01='Advisory'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='Warning'

update @WS06
set Col01='Warning Letter'
where Col01='Warning'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='Delist'

update @WS06
set Col01='Delist Letter'
where Col01='Delist'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='Payment'

update @WS06
set Col01='Suspend Payment Letter'
where Col01='Payment'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='EHCP'

update @WS06
set Col01='Suspend EHCP Account Letter'
where Col01='EHCP'


INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='Others'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Sub-Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs061
WHERE Type = 'Advisory'
      OR Type = 'Warning'
      OR Type = 'Delist'
      OR Type = 'Payment'
      OR Type = 'EHCP'
      OR Type = 'EHCP'
      OR Type = 'Others';



INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='withLetter'

update @WS06
set Col01='No. of case with letter issued'
where Col01='withLetter'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs061
WHERE Type='withoutLetter'

update @WS06
set Col01='No. of case without letter issued'
where Col01='withoutLetter'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs061
WHERE Type = 'withoutLetter'
      OR Type = 'withLetter'
      

-- 6(ii) Refer Party

INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01)
VALUES('(ii) Refer Party');
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');

SELECT Type, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs062
FROM
(
    SELECT Service_Category_Code, 
           Type, 
           value
    FROM
    (
         
	 SELECT p.Service_Category_Code, 
       COUNT(r.BoardAndCouncil_Date) AS BoardAndCouncil, 
       COUNT(r.Police_Date) AS Police, 
       COUNT(r.Social_Welfare_Department_Date) AS Social_Welfare_Department, 
       COUNT(r.HK_Customs_And_Excise_Department_Date) AS HK_Customs_And_Excise_Department, 
       COUNT(r.Immigration_Department_Date) AS Immigration_Department, 
       COUNT(r.Labour_Department_Date) AS Labour_Department, 
       COUNT(r.Other_Party_Date) AS Others, 
       SUM(CASE
               WHEN(r.BoardAndCouncil_Date IS NULL
                    AND r.Police_Date IS NULL
                    AND r.Social_Welfare_Department_Date IS NULL
                    AND r.HK_Customs_And_Excise_Department_Date IS NULL
                    AND r.Immigration_Department_Date IS NULL
                    AND r.Labour_Department_Date IS NULL
                    AND r.Other_Party_Date IS NULL)
                   AND r.Inspection_ID IS NOT NULL
               THEN 1
               ELSE 0
           END) AS withoutRefer, 
       SUM(CASE
               WHEN(r.BoardAndCouncil_Date IS NOT NULL
                    OR r.Police_Date IS NOT NULL
                    OR r.Social_Welfare_Department_Date IS NOT NULL
                    OR r.HK_Customs_And_Excise_Department_Date IS NOT NULL
                    OR r.Immigration_Department_Date IS NOT NULL
                    OR r.Labour_Department_Date IS NOT NULL
                    OR r.Other_Party_Date IS NOT NULL)
               THEN 1
               ELSE 0
           END) AS withRefer
FROM #ResultsWithResult AS r
     RIGHT OUTER JOIN @Profession AS p
     ON r.Service_Category_Code = p.Service_Category_Code
GROUP BY p.Service_Category_Code
    ) AS r UNPIVOT(value FOR Type IN(BoardAndCouncil, 
                                     Police, 
                                     Social_Welfare_Department, 
                                     HK_Customs_And_Excise_Department, 
                                     Immigration_Department, 
                                     Labour_Department, 
                                     Others, 
                                     withoutRefer,
									 withRefer)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;


INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='BoardAndCouncil'

update @WS06
set Col01='Board and Council'
where Col01='BoardAndCouncil'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='Police'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='Social_Welfare_Department'

update @WS06
set Col01='Social Welfare Department'
where Col01='Social_Welfare_Department'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='HK_Customs_And_Excise_Department'

update @WS06
set Col01='Hong Kong Customs and Excise Department'
where Col01='HK_Customs_And_Excise_Department'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='Immigration_Department'

update @WS06
set Col01='Immigration Department'
where Col01='Immigration_Department'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='Labour_Department'

update @WS06
set Col01='Labour Department'
where Col01='Labour_Department'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='Others'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Sub-Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs062
WHERE Type = 'BoardAndCouncil'
      OR Type = 'Police'
      OR Type = 'Social_Welfare_Department'
      OR Type = 'HK_Customs_And_Excise_Department'
      OR Type = 'Immigration_Department'
      OR Type = 'Labour_Department'
      OR Type = 'Others';

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='withRefer'

update @WS06
set Col01='No. of case being referred'
where Col01='withRefer'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs062
WHERE Type='withoutRefer'

update @WS06
set Col01='No. of case without being referred'
where Col01='withoutRefer'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs062
WHERE Type = 'withoutRefer'
      OR Type = 'withRefer'
      
-- 6(iii) Action to EHCP
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01)
VALUES('(iii) Action to EHCP');
INSERT INTO @WS06(Col01)
VALUES('');  
INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
VALUES('', 'ENU', 'RCM', 'RCP', 'RDT', 'RMP', 'RMT', 'RNU', 'ROP', 'ROT', 'RPT', 'RRD', 'DIT',  'POD', 'SPT','Total');

SELECT Type, 
       ENU, 
       RCM, 
       RCP, 
       RDT, 
       RMP, 
       RMT, 
       RNU, 
       ROP, 
       ROT, 
       RPT, 
       RRD, 
       DIT, 
       POD, 
       SPT, 
       ENU + RCM + RCP + RDT + RMP + RMT + RNU + ROP + ROT + RPT + RRD + DIT + POD + SPT AS RowTotal
INTO #ResultsWs063
FROM
(
    SELECT Service_Category_Code, 
           Type, 
           value
    FROM
    (         
	 SELECT p.Service_Category_Code, 
       COUNT(r.Suspend_EHCP_Date) AS Suspend_EHCP, 
       COUNT(r.Delist_EHCP_Date) AS Delist_EHCP, 
       COUNT(r.Payment_RecoverySuspension_Date) AS Payment_RecoverySuspension, 
       SUM(CASE
               WHEN(r.Suspend_EHCP_Date IS NULL
                    AND r.Delist_EHCP_Date IS NULL
                    AND r.Payment_RecoverySuspension_Date IS NULL)
                   AND r.Inspection_ID IS NOT NULL
               THEN 1
               ELSE 0
           END) AS withoutAction, 
       SUM(CASE
               WHEN(r.Suspend_EHCP_Date IS NOT NULL
                    OR r.Delist_EHCP_Date IS NOT NULL
                    OR r.Payment_RecoverySuspension_Date IS NOT NULL)
               THEN 1
               ELSE 0
           END) AS withAction
FROM #ResultsWithResult AS r
     RIGHT OUTER JOIN @Profession AS p
     ON r.Service_Category_Code = p.Service_Category_Code
GROUP BY p.Service_Category_Code
    ) AS r UNPIVOT(value FOR Type IN(Suspend_EHCP, 
                                     Delist_EHCP, 
                                     Payment_RecoverySuspension,                                     
                                     withoutAction,
									 withAction)) AS unpiv
) AS src PIVOT(SUM(value) FOR Service_Category_Code IN(ENU, 
                                                       RCM, 
                                                       RCP, 
                                                       RDT, 
                                                       RMP, 
                                                       RMT, 
                                                       RNU, 
                                                       ROP, 
                                                       ROT, 
                                                       RPT, 
                                                       RRD, 
                                                       DIT, 
                                                       POD, 
                                                       SPT)) piv;


INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs063
WHERE Type='Suspend_EHCP'

update @WS06
set Col01='Suspend the EHCP from HCVS'
where Col01='Suspend_EHCP'


INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs063
WHERE Type='Delist_EHCP'

update @WS06
set Col01='Delist the EHCP from HCVS'
where Col01='Delist_EHCP'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs063
WHERE Type='Payment_RecoverySuspension'

update @WS06
set Col01='Recovery or Suspension Payment'
where Col01='Payment_RecoverySuspension'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Sub-Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs063
WHERE Type = 'Suspend_EHCP'
      OR Type = 'Delist_EHCP'
      OR Type = 'Payment_RecoverySuspension'
  
  
INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs063
WHERE Type='withAction'

update @WS06
set Col01='No. of case required action to EHCP'
where Col01='withAction'

INSERT INTO @WS06 (Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT *
FROM #ResultsWs063
WHERE Type='withoutAction'

update @WS06
set Col01='No. of case without required action'
where Col01='withoutAction'

INSERT INTO @WS06(Col01, Col02, Col03, Col04, Col05, Col06, Col07, Col08, Col09, Col10, Col11, Col12, Col13, Col14, Col15, Col16)
SELECT 'Total', 
       SUM(ENU), 
       SUM(RCM), 
       SUM(RCP), 
       SUM(RDT), 
       SUM(RMP), 
       SUM(RMT), 
       SUM(RNU), 
       SUM(ROP), 
       SUM(ROT), 
       SUM(RPT), 
       SUM(RRD), 
       SUM(DIT), 
       SUM(POD), 
       SUM(SPT), 
       SUM(RowTotal)
FROM #ResultsWs063
WHERE Type = 'withoutAction'
      OR Type = 'withAction'

 -- -----------------------------------------  
 -- Excel worksheet (Remark)  
 -- -----------------------------------------  
  
 INSERT INTO @Remark (Col01) VALUES   
 ('(A) Common Note(s) for the report'),  
 ('1. Exclude Removed inspection record')
  
-- =============================================  
-- Return results  
-- =============================================   

SELECT 'Report Generation Time: ' + CONVERT(VARCHAR(10), @current_dtm, 111) + ' ' + CONVERT(VARCHAR(5), @current_dtm, 114); --The first Sheet

SELECT w.col01, 
       w.Col02
FROM @WS01 AS w
ORDER BY w.Seq;

SELECT w.col01, 
       w.Col02
FROM @WS02 AS w
ORDER BY w.Seq;

SELECT w.Col01, 
       w.Col02, 
       w.Col03
FROM @WS03 AS w
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
FROM @WS04 AS w
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
FROM @WS05 AS w
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
FROM @WS06 AS w
ORDER BY w.Seq;

SELECT r.Col01
FROM @Remark AS r
ORDER BY r.Seq;

IF OBJECT_ID('tempdb..#Results') IS NOT NULL
    DROP TABLE #Results;
IF OBJECT_ID('tempdb..#ResultsWithResult') IS NOT NULL
    DROP TABLE #ResultsWithResult;
IF OBJECT_ID('tempdb..#ResultsClaims') IS NOT NULL
    DROP TABLE #ResultsClaims;
IF OBJECT_ID('tempdb..##ResultsWs02') IS NOT NULL
    DROP TABLE #ResultsWs02;	
IF OBJECT_ID('tempdb..#ResultsWs04') IS NOT NULL
    DROP TABLE #ResultsWs04;
IF OBJECT_ID('tempdb..#ResultsWs051') IS NOT NULL
    DROP TABLE #ResultsWs051;
IF OBJECT_ID('tempdb..#ResultsWs052') IS NOT NULL
    DROP TABLE #ResultsWs052;
IF OBJECT_ID('tempdb..#ResultsWs053') IS NOT NULL
    DROP TABLE #ResultsWs053;
IF OBJECT_ID('tempdb..#ResultsWs061') IS NOT NULL
    DROP TABLE #ResultsWs061;
IF OBJECT_ID('tempdb..#ResultsWs062') IS NOT NULL
    DROP TABLE #ResultsWs062;
IF OBJECT_ID('tempdb..#ResultsWs063') IS NOT NULL
    DROP TABLE #ResultsWs063;
	   	 
    END;
GO
GRANT EXECUTE ON [dbo].[proc_EHS_eHSU0012_Report_get] TO HCVU;
GO