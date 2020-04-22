IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_EHS_Utilization_Stat_SendMessage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_EHS_Utilization_Stat_SendMessage]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Modification History
-- Modified by:		Tony FUNG
-- Modified date:	19 Sep 2011
-- CR No.:			CRE11-024-01 (Enhancement on HCVS Extension)
-- Description:		- added profession 'ROP' for registered optometrists
--			- only displays the column 'ROP' after its enrolment start date
--			- if @ex_sp is NULL, make it an empty string
--			- modified "Professional Type & Legend" to display data dynamically from the Profession table
--			- modified to send to HCVUUsers who have been assigned Role_type=99 and Scheme_Code='HCVS' and is not yet expired
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Pak Ho LEE
-- Modified date:	10 Oct 2009
-- Description:		Select "Distinct" UserID to recieve Message
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	1 Sept 2009
-- Description:		1. Relationship of MO
--					2. Professional Type
-- =============================================
-- =============================================
-- Modification History
-- Modified by:		Kathy LEE
-- Modified date:	16 Oct 2009
-- Description:		1. Add wording at the end of message
-- =============================================
-- =============================================
-- Author:		Tommy Cheung
-- Report ID:		eHSD0020
-- Report Desc:		eHS_EnrolmentSummary (daily via user inbox)
-- Create date: 03 Oct 2008
-- Description:	Statistics - Send Message to Inbox
-- =============================================
CREATE PROCEDURE [dbo].[proc_EHS_Utilization_Stat_SendMessage]
	-- Add the parameters for the stored procedure here
	--@no_of_days	int,
	--@end_date	varchar(10),
	--@Role_Type smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

declare @temp_date varchar(10)

declare @Solo	int
declare @Partenship	int
declare @Shareholder	int
declare @Director	int
declare @Employee	int
declare @Others	int

-- CRE11-024-01: begin of commented
/*
declare @ENU	int
declare @RCM	int
declare @RCP	int
declare @RDT	int
declare @RMP	int
declare @RMT	int
declare @RNU	int
declare @ROT	int
declare @RPT	int
declare @RRD	int
*/
-- CRE11-024-01: end of commented

select @temp_date = convert(varchar, dateadd(day, -1, getdate()), 120)

select @Solo= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Solo_HCVS'
select @Partenship= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Partenship_HCVS'
select @Shareholder= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Shareholder_HCVS'
select @Director= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Director_HCVS'
select @Employee= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Employee_HCVS'
select @Others= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'Others_HCVS'

-- CRE11-024-01: begin of commented
/*
select @ENU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ENU_HCVS'
select @RCM= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RCM_HCVS'
select @RCP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RCP_HCVS'
select @RDT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RDT_HCVS'
select @RMP= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RMP_HCVS'
select @RMT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RMT_HCVS'
select @RNU= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RNU_HCVS'
select @ROT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'ROT_HCVS'
select @RPT= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RPT_HCVS'
select @RRD= StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = 'RRD_HCVS'
*/
-- CRE11-024-01: end of commented

create table #SPExceptionList
(
	sp_id char(8)
)
insert into #SPExceptionList
(sp_id)
select sp_id from SPExceptionList

declare @ex_sp varchar(255)

select @ex_sp = Substring(t.sp_id, 3, LEN(t.sp_id))
from (
select (SELECT ', ' + ltrim(rtrim(sp_id))
FROM SPExceptionList
FOR XML PATH('') ) AS sp_id
) as t

select @ex_sp=ISNULL(@ex_sp,'')	-- CRE11-024-01: added

Declare @Message_No char(12)
Declare @Message_ID char(12)
Declare @Profile_ID char(10)
Select @Profile_ID = 'IMN'
BEGIN TRANSACTION
exec proc_SystemProfile_get_byProfileID_withOutputParm @Profile_ID, @Message_No output
COMMIT TRANSACTION
Select @Message_No = Right('0000000000' + RTrim(LTrim(@Message_No)), 8)
Select @Message_ID = Right(Year(getdate()), 2) + RTrim(LTrim(@Message_No))


Declare @Subject nvarchar(500)
Select @Subject = 'Enrolment Summary - ' + substring(convert(varchar(20), convert(datetime, @temp_date), 13),0,12)

-- CRE11-024-01: begin of added
Declare @ProfessionAvail Table (
	Service_Category_Code varchar(3),
	Service_Category_Desc varchar(100)
)

Insert into @ProfessionAvail (
	Service_Category_Code,
	Service_Category_Desc)
Select 
	Service_Category_Code,
	Service_Category_Desc
From
	Profession
Where datediff(d,Enrol_Period_From,@temp_date)>=0


Declare @ProfessionalTypeMsg_lbl nvarchar(900)
Declare @ProfessionalTypeMsg_count nvarchar(900)
Select @ProfessionalTypeMsg_lbl=''
Select @ProfessionalTypeMsg_count=''

Declare @ProfessionalTypeLegendMsg_content nvarchar(1500)
Select @ProfessionalTypeLegendMsg_content=''

Declare @temp_count int
Select @temp_count=0

Declare Profession_Cursor Cursor For 
Select Service_Category_Code, Service_Category_Desc 
From @ProfessionAvail
Open Profession_Cursor
Declare @temp_code varchar(3), @temp_desc varchar(100)
Fetch Next From Profession_Cursor into @temp_code, @temp_desc
While (@@Fetch_Status <> -1)
	Begin
		-- each Type label has overhead of 41 characters, excluding the type's label itself
		Select @ProfessionalTypeMsg_lbl=@ProfessionalTypeMsg_lbl+'<td style="background-color: white">'+@temp_code+'</td>'
		Select @temp_count=(
				-- The StatisticType has suffix of 'HCVS': e.g. ENU_HCVS, RCM_HCVS, ROP_HCVS, ROT_HCVS
				select StatisticValue from StatisticTable where (StatisticDate = @temp_date) and StatisticType = @temp_code+'_HCVS'
				)
		-- each Type's count has overhead of 55 characters, excluding the type's count itself
		Select @ProfessionalTypeMsg_count=@ProfessionalTypeMsg_count+'<td style="background-color: white" align="right">' + convert(varchar,isnull(@temp_count,0)) + '</td>'

		-- each Type's legend has overhead of 47 characters, excluding the type's legend itself
		Select @ProfessionalTypeLegendMsg_content=@ProfessionalTypeLegendMsg_content+'<tr><td>'+@temp_code+'</td><td>-</td><td>'+@temp_desc+'</td></tr>'
		Fetch Next From Profession_Cursor Into @temp_code, @temp_desc
	End
Close Profession_Cursor
Deallocate Profession_Cursor

-- CRE11-024-01: end of added


Declare @Message nvarchar(4000)
-- CRE11-024-01: updated
Select @Message = '<div class="headingText">Relationship between Medical Organization</div>
<table id="Table3" border="0" cellpadding="2" cellspacing="1" style="background-color: #666666" width="500">
<tr>
<td style="background-color: #ffffff">
Solo</td>
<td style="background-color: #ffffff">
Partnership</td>
<td style="background-color: #ffffff">
Shareholder</td>
<td style="background-color: #ffffff">
Director</td>
<td style="background-color: #ffffff">
Employee</td>
<td style="background-color: #ffffff">
Others</td>
</tr>
<tr>
<td style="background-color: #ffffff" align="right">' +
convert(varchar,isnull(@Solo,0)) + '</td>
<td style="background-color: #ffffff" align="right">' +
convert(varchar,isnull(@Partenship,0)) + '</td>
<td style="background-color: #ffffff" align="right">' +
convert(varchar,isnull(@Shareholder,0)) + '</td>
<td align="right" style="background-color: #ffffff">' +
convert(varchar,isnull(@Director,0)) + '</td>
<td align="right" style="background-color: #ffffff">' +
convert(varchar,isnull(@Employee,0)) + '</td>
<td align="right" style="background-color: #ffffff">' +
convert(varchar,isnull(@Others,0)) + '</td>
</tr>
</table>
<br />' +

-- CRE11-024-01: begin of added
'<div class="headingText">Professional Type</div>
<table border="0" cellpadding="2" cellspacing="1" style="background-color: #666666" width="500">
<tr>'+@ProfessionalTypeMsg_lbl+'</tr><tr>'+@ProfessionalTypeMsg_count+'</tr></table>' +
'<br />' +
'<u>Professional Type Legend</u><br />
<table cellpadding="5" cellspacing="2">'+@ProfessionalTypeLegendMsg_content+'</table>' +
-- CRE11-024-01: end of added

-- CRE11-024-01: begin of commented
/*
<div class="headingText">Professional Type</div>
<table border="0" cellpadding="2" cellspacing="1" style="background-color: #666666" width="500">
<tr>
<td style="background-color: white">
ENU</td>
<td style="background-color: white">
RCM</td>
<td style="background-color: white">
RCP</td>
<td style="background-color: white">
RDT</td>
<td style="background-color: white">
RMP</td>
<td style="background-color: white">
RMT</td>
<td style="background-color: white">
RNU</td>
<td style="background-color: white">
ROT</td>
<td style="background-color: white">
RPT</td>
<td style="background-color: white">
RRD</td>
</tr>
<tr>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@ENU,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RCM,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RCP,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RDT,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RMP,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RMT,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RNU,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@ROT,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RPT,0)) + '</td>
<td style="background-color: white" align="right">' +
convert(varchar,isnull(@RRD,0)) + '</td>
</tr>
</table>
<br />
<u>Professional Type Legend</u><br />
<table cellpadding="5" cellspacing="2">
<tr>
<td>
ENU</td>
<td>
-</td>
<td>
Enrolled Nurses</td>
</tr>
<tr>
<td>
RCM</td>
<td>
-</td>
<td>
Registered Chinese Medicine Practitioners</td>
</tr>
<tr>
<td>
RCP</td>
<td>
-</td>
<td>
Registered Chiropractors</td>
</tr>
<tr>
<td>
RDT</td>
<td>
-</td>
<td>
Registered Dentists</td>
</tr>
<tr>
<td>
RMP</td>
<td>
-</td>
<td>
Registered Medical Practitioners</td>
</tr>
<tr>
<td>
RMT</td>
<td>
-</td>
<td>
Registered Medical Laboratory Technologists</td>
</tr>
<tr>
<td>
RNU</td>
<td>
-</td>
<td>
Registered Nurses</td>
</tr>
<tr>
<td>
ROT</td>
<td>
-</td>
<td>
Registered Occupational Therapists</td>
</tr>
<tr>
<td>
RPT</td>
<td>
-</td>
<td>
Registered Physiotherapists</td>
</tr>
<tr>
<td>
RRD</td>
<td>
-</td>
<td>
Registered Radiographers</td>
</tr>
</table>'
*/
-- CRE11-024-01: end of commented

'<br/>* The above data include enrolment via electronic only. (Effective Date:' + substring(convert(varchar(20), convert(datetime, @temp_date), 13),0,12) + ')
<br/> The following dummy SP accounts are excluded from the statistic of 19 Oct 2009 or after: ' + @ex_sp

Insert into [Message] (Message_ID,
						Subject,
						Message,
						Create_By,
						Create_Dtm)
values(@Message_ID,@Subject,@Message,'System',getdate())


-- CRE11-024-01: begin of commented
/*
Insert into [MessageReader] 
(Message_ID, Message_Reader, Record_Status, Update_by, Update_dtm)
Values(@Message_ID,'HCVUADM','U','System',getdate())
 

Insert into [MessageReader] 
(Message_ID, Message_Reader, Record_Status, Update_by, Update_dtm)
Values(@Message_ID,'HCVUSEO','U','System',getdate())
*/
-- CRE11-024-01: end of commented

-- CRE11-024-01: begin of added
Insert into [MessageReader] 
(Message_ID, Message_Reader, Record_Status, Update_by, Update_dtm)
Select 
	@Message_ID,
	UR.User_ID,
	'U',
	'System',
	getdate()
From UserRole UR, HCVUUserAC AC where UR.User_ID=AC.User_ID 
	and UR.role_type='99' 
	and UR.Scheme_Code='HCVS'
	and (AC.Expiry_Date is NULL or datediff(d,getDate(),AC.Expiry_Date) > 0)
-- CRE11-024-01: end of added


/*Declare @Message_Reader varchar(20)
DECLARE vendor_cursor CURSOR FOR 
Select Distinct(User_ID) from [dbo].[UserRole]
where Role_Type=@Role_Type

OPEN vendor_cursor

FETCH NEXT FROM vendor_cursor
INTO @Message_Reader
WHILE @@FETCH_STATUS = 0
BEGIN

Insert into [MessageReader] (Message_ID,
							 Message_Reader,
							 Record_Status,
							 Update_by,
							 Update_dtm)
values(@Message_ID,@Message_Reader,'U','System',getdate())




        -- Get the next vendor.
    FETCH NEXT FROM vendor_cursor 
	INTO @Message_Reader
END 
CLOSE vendor_cursor
DEALLOCATE vendor_cursor*/

END 
GO

GRANT EXECUTE ON [dbo].[proc_EHS_Utilization_Stat_SendMessage] TO HCVU
GO
