IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[proc_generateChkDgt]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[proc_generateChkDgt]
GO

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

create proc proc_generateChkDgt @strOriNum varchar(15), @chkdigit char(1) output
as
begin
	declare @strNewChkdgt char(1), @strCur char(1)
	declare @intSum int, @intTemp int
	declare @strRes char(1)
	declare @strTenth varchar(15)
	declare @strTemp varchar(15), @strDigital varchar(15)
	declare @Pos int
	set @strNewChkdgt = ''
	set @intSum = 0
	set @strRes = ''
	set @strTenth = ''
	set @strDigital = ''
	set @strTemp = @strOriNum
	select  @Pos=len(@strTemp)
        while @Pos > 0 
	begin 
		set @StrCur = Substring(@strTemp, @Pos, 1)
		if convert(int, ascii(@StrCur )) > 57 
		begin
			set @intTemp = (convert(int, ascii(@StrCur)) - convert(int, ascii('A')))
		end
		else
		begin
			set @intTemp = convert(int, @StrCur) 
		end

		if (@Pos % 2) = 1 
		begin
			set @intTemp = @intTemp * 2
		end

		while @intTemp > 9 begin
			set @strTenth = substring(convert(varchar(3),@intTemp),1,1)
			set @strDigital = substring(convert(varchar(3), @intTemp),2,1)
			set @intTemp = convert(int, @strTenth) + convert(int, @strDigital)
		end

                set @intSum = @intSum + @intTemp

		set @Pos = @Pos - 1 
        end 

	set @strRes = convert(char(1), (@intSum % 10))
	set @chkdigit = @strRes	
end
GO
