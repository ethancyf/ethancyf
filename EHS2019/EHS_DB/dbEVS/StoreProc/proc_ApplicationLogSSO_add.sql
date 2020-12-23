
IF EXISTS
         (
             SELECT *
             FROM dbo.sysobjects
             WHERE id = OBJECT_ID(N'[dbo].[proc_ApplicationLogSSO_add]')
                   AND OBJECTPROPERTY(id, N'IsProcedure') = 1
          )
    BEGIN
        DROP PROCEDURE [dbo].[proc_ApplicationLogSSO_add];
    END;
GO

SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

-- =============================================
-- Modification History
-- CR# :			I-CRE20-005
-- Modified by:		Martin Tang
-- Modified date:	10 Dec 2020
-- Description:		Fine tune Performance (Open Key with Dynamic SQL)
-- =============================================
-- =============================================  
-- Author:   Paul Yip  
-- Create date:  14-06-2010  
-- Description:  Add SSO Application Log  
-- =============================================  

CREATE PROCEDURE [dbo].[proc_ApplicationLogSSO_add] @client_ip   VARCHAR(20), 
                                                    @user_id     VARCHAR(20), 
                                                    @description NVARCHAR(1000), 
                                                    @session_id  VARCHAR(40), 
                                                    @browser     VARCHAR(20), 
                                                    @os          VARCHAR(20)
AS
    BEGIN

        -- =============================================  
        -- Declaration  
        -- =============================================  

        DECLARE @application_server VARCHAR(20);
        DECLARE @year VARCHAR(2);

        DECLARE @System_Dtm DATETIME;
        DECLARE @E_Client_IP VARBINARY(60);
        DECLARE @E_User_ID VARBINARY(60);
        DECLARE @E_Description VARBINARY(2100);
        DECLARE @E_Application_Server VARBINARY(60);
        DECLARE @E_Session_ID VARBINARY(100);

        DECLARE @E_Browser VARBINARY(60);
        DECLARE @E_OS VARBINARY(60);

        -- =============================================  
        -- Initialization  
        -- =============================================  

        SELECT @System_Dtm = GETDATE();
        SELECT @application_server = HOST_NAME();

        SELECT @year = CONVERT(VARCHAR(2), @system_dtm, 12);

        EXEC [proc_SymmetricKey_open];
        SELECT @E_Client_IP = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @client_ip);
        SELECT @E_User_ID = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @user_id);
        SELECT @E_Description = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @description);
        SELECT @E_Application_Server = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @application_server);
        SELECT @E_Session_ID = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @session_id);

        SELECT @E_Browser = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @browser);
        SELECT @E_OS = ENCRYPTBYKEY(KEY_GUID('sym_Key'), @os);

        EXEC [proc_SymmetricKey_close];

        -- =============================================  
        -- Insert transaction  
        -- =============================================  

        IF @year = '08'
            BEGIN
                INSERT INTO dbo.AppLogSSO08
                       (System_Dtm, 
                        E_Client_IP, 
                        E_User_ID, 
                        E_Description, 
                        E_Application_Server, 
                        E_Session_ID, 
                        E_Browser, 
                        E_OS
                       )
                VALUES
                      (@System_Dtm, 
                       @E_Client_IP, 
                       @E_User_ID, 
                       @E_Description, 
                       @E_Application_Server, 
                       @E_Session_ID, 
                       @E_Browser, 
                       @E_OS
                       );
            END;
            ELSE
            BEGIN
                IF @year = '09'
                    BEGIN
                        INSERT INTO dbo.AppLogSSO09
                               (System_Dtm, 
                                E_Client_IP, 
                                E_User_ID, 
                                E_Description, 
                                E_Application_Server, 
                                E_Session_ID, 
                                E_Browser, 
                                E_OS
                               )
                        VALUES
                              (@System_Dtm, 
                               @E_Client_IP, 
                               @E_User_ID, 
                               @E_Description, 
                               @E_Application_Server, 
                               @E_Session_ID, 
                               @E_Browser, 
                               @E_OS
                               );
                    END;
                    ELSE
                    BEGIN
                        IF @year = '10'
                            BEGIN
                                INSERT INTO dbo.AppLogSSO10
                                       (System_Dtm, 
                                        E_Client_IP, 
                                        E_User_ID, 
                                        E_Description, 
                                        E_Application_Server, 
                                        E_Session_ID, 
                                        E_Browser, 
                                        E_OS
                                       )
                                VALUES
                                      (@System_Dtm, 
                                       @E_Client_IP, 
                                       @E_User_ID, 
                                       @E_Description, 
                                       @E_Application_Server, 
                                       @E_Session_ID, 
                                       @E_Browser, 
                                       @E_OS
                                       );
                            END;
                            ELSE
                            BEGIN
                                IF @year = '11'
                                    BEGIN
                                        INSERT INTO dbo.AppLogSSO11
                                               (System_Dtm, 
                                                E_Client_IP, 
                                                E_User_ID, 
                                                E_Description, 
                                                E_Application_Server, 
                                                E_Session_ID, 
                                                E_Browser, 
                                                E_OS
                                               )
                                        VALUES
                                              (@System_Dtm, 
                                               @E_Client_IP, 
                                               @E_User_ID, 
                                               @E_Description, 
                                               @E_Application_Server, 
                                               @E_Session_ID, 
                                               @E_Browser, 
                                               @E_OS
                                               );
                                    END;
                                    ELSE
                                    BEGIN
                                        IF @year = '12'
                                            BEGIN
                                                INSERT INTO dbo.AppLogSSO12
                                                       (System_Dtm, 
                                                        E_Client_IP, 
                                                        E_User_ID, 
                                                        E_Description, 
                                                        E_Application_Server, 
                                                        E_Session_ID, 
                                                        E_Browser, 
                                                        E_OS
                                                       )
                                                VALUES
                                                      (@System_Dtm, 
                                                       @E_Client_IP, 
                                                       @E_User_ID, 
                                                       @E_Description, 
                                                       @E_Application_Server, 
                                                       @E_Session_ID, 
                                                       @E_Browser, 
                                                       @E_OS
                                                       );
                                            END;
                                            ELSE
                                            BEGIN
                                                IF @year = '13'
                                                    BEGIN
                                                        INSERT INTO dbo.AppLogSSO13
                                                               (System_Dtm, 
                                                                E_Client_IP, 
                                                                E_User_ID, 
                                                                E_Description, 
                                                                E_Application_Server, 
                                                                E_Session_ID, 
                                                                E_Browser, 
                                                                E_OS
                                                               )
                                                        VALUES
                                                              (@System_Dtm, 
                                                               @E_Client_IP, 
                                                               @E_User_ID, 
                                                               @E_Description, 
                                                               @E_Application_Server, 
                                                               @E_Session_ID, 
                                                               @E_Browser, 
                                                               @E_OS
                                                               );
                                                    END;
                                                    ELSE
                                                    BEGIN
                                                        IF @year = '14'
                                                            BEGIN
                                                                INSERT INTO dbo.AppLogSSO14
                                                                       (System_Dtm, 
                                                                        E_Client_IP, 
                                                                        E_User_ID, 
                                                                        E_Description, 
                                                                        E_Application_Server, 
                                                                        E_Session_ID, 
                                                                        E_Browser, 
                                                                        E_OS
                                                                       )
                                                                VALUES
                                                                      (@System_Dtm, 
                                                                       @E_Client_IP, 
                                                                       @E_User_ID, 
                                                                       @E_Description, 
                                                                       @E_Application_Server, 
                                                                       @E_Session_ID, 
                                                                       @E_Browser, 
                                                                       @E_OS
                                                                       );
                                                            END;
                                                            ELSE
                                                            BEGIN
                                                                IF @year = '15'
                                                                    BEGIN
                                                                        INSERT INTO dbo.AppLogSSO15
                                                                               (System_Dtm, 
                                                                                E_Client_IP, 
                                                                                E_User_ID, 
                                                                                E_Description, 
                                                                                E_Application_Server, 
                                                                                E_Session_ID, 
                                                                                E_Browser, 
                                                                                E_OS
                                                                               )
                                                                        VALUES
                                                                              (@System_Dtm, 
                                                                               @E_Client_IP, 
                                                                               @E_User_ID, 
                                                                               @E_Description, 
                                                                               @E_Application_Server, 
                                                                               @E_Session_ID, 
                                                                               @E_Browser, 
                                                                               @E_OS
                                                                               );
                                                                    END;
                                                                    ELSE
                                                                    BEGIN
                                                                        IF @year = '16'
                                                                            BEGIN
                                                                                INSERT INTO dbo.AppLogSSO16
                                                                                       (System_Dtm, 
                                                                                        E_Client_IP, 
                                                                                        E_User_ID, 
                                                                                        E_Description, 
                                                                                        E_Application_Server, 
                                                                                        E_Session_ID, 
                                                                                        E_Browser, 
                                                                                        E_OS
                                                                                       )
                                                                                VALUES
                                                                                      (@System_Dtm, 
                                                                                       @E_Client_IP, 
                                                                                       @E_User_ID, 
                                                                                       @E_Description, 
                                                                                       @E_Application_Server, 
                                                                                       @E_Session_ID, 
                                                                                       @E_Browser, 
                                                                                       @E_OS
                                                                                       );
                                                                            END;
                                                                            ELSE
                                                                            BEGIN
                                                                                IF @year = '17'
                                                                                    BEGIN
                                                                                        INSERT INTO dbo.AppLogSSO17
                                                                                               (System_Dtm, 
                                                                                                E_Client_IP, 
                                                                                                E_User_ID, 
                                                                                                E_Description, 
                                                                                                E_Application_Server, 
                                                                                                E_Session_ID, 
                                                                                                E_Browser, 
                                                                                                E_OS
                                                                                               )
                                                                                        VALUES
                                                                                              (@System_Dtm, 
                                                                                               @E_Client_IP, 
                                                                                               @E_User_ID, 
                                                                                               @E_Description, 
                                                                                               @E_Application_Server, 
                                                                                               @E_Session_ID, 
                                                                                               @E_Browser, 
                                                                                               @E_OS
                                                                                               );
                                                                                    END;
                                                                                    ELSE
                                                                                    BEGIN
                                                                                        IF @year = '18'
                                                                                            BEGIN
                                                                                                INSERT INTO dbo.AppLogSSO18
                                                                                                       (System_Dtm, 
                                                                                                        E_Client_IP, 
                                                                                                        E_User_ID, 
                                                                                                        E_Description, 
                                                                                                        E_Application_Server, 
                                                                                                        E_Session_ID, 
                                                                                                        E_Browser, 
                                                                                                        E_OS
                                                                                                       )
                                                                                                VALUES
                                                                                                      (@System_Dtm, 
                                                                                                       @E_Client_IP, 
                                                                                                       @E_User_ID, 
                                                                                                       @E_Description, 
                                                                                                       @E_Application_Server, 
                                                                                                       @E_Session_ID, 
                                                                                                       @E_Browser, 
                                                                                                       @E_OS
                                                                                                       );
                                                                                            END;
                                                                                            ELSE
                                                                                            BEGIN
                                                                                                IF @year = '19'
                                                                                                    BEGIN
                                                                                                        INSERT INTO dbo.AppLogSSO19
                                                                                                               (System_Dtm, 
                                                                                                                E_Client_IP, 
                                                                                                                E_User_ID, 
                                                                                                                E_Description, 
                                                                                                                E_Application_Server, 
                                                                                                                E_Session_ID, 
                                                                                                                E_Browser, 
                                                                                                                E_OS
                                                                                                               )
                                                                                                        VALUES
                                                                                                              (@System_Dtm, 
                                                                                                               @E_Client_IP, 
                                                                                                               @E_User_ID, 
                                                                                                               @E_Description, 
                                                                                                               @E_Application_Server, 
                                                                                                               @E_Session_ID, 
                                                                                                               @E_Browser, 
                                                                                                               @E_OS
                                                                                                               );
                                                                                                    END;
                                                                                                    ELSE
                                                                                                    BEGIN
                                                                                                        IF @year = '20'
                                                                                                            BEGIN
                                                                                                                INSERT INTO dbo.AppLogSSO20
                                                                                                                       (System_Dtm, 
                                                                                                                        E_Client_IP, 
                                                                                                                        E_User_ID, 
                                                                                                                        E_Description, 
                                                                                                                        E_Application_Server, 
                                                                                                                        E_Session_ID, 
                                                                                                                        E_Browser, 
                                                                                                                        E_OS
                                                                                                                       )
                                                                                                                VALUES
                                                                                                                      (@System_Dtm, 
                                                                                                                       @E_Client_IP, 
                                                                                                                       @E_User_ID, 
                                                                                                                       @E_Description, 
                                                                                                                       @E_Application_Server, 
                                                                                                                       @E_Session_ID, 
                                                                                                                       @E_Browser, 
                                                                                                                       @E_OS
                                                                                                                       );
                                                                                                            END;
                                                                                                            ELSE
                                                                                                            BEGIN
                                                                                                                IF @year = '21'
                                                                                                                    BEGIN
                                                                                                                        INSERT INTO dbo.AppLogSSO21
                                                                                                                               (System_Dtm, 
                                                                                                                                E_Client_IP, 
                                                                                                                                E_User_ID, 
                                                                                                                                E_Description, 
                                                                                                                                E_Application_Server, 
                                                                                                                                E_Session_ID, 
                                                                                                                                E_Browser, 
                                                                                                                                E_OS
                                                                                                                               )
                                                                                                                        VALUES
                                                                                                                              (@System_Dtm, 
                                                                                                                               @E_Client_IP, 
                                                                                                                               @E_User_ID, 
                                                                                                                               @E_Description, 
                                                                                                                               @E_Application_Server, 
                                                                                                                               @E_Session_ID, 
                                                                                                                               @E_Browser, 
                                                                                                                               @E_OS
                                                                                                                               );
                                                                                                                    END;
                                                                                                                    ELSE
                                                                                                                    BEGIN
                                                                                                                        IF @year = '22'
                                                                                                                            BEGIN
                                                                                                                                INSERT INTO dbo.
                                                                                                                                AppLogSSO22
                                                                                                                                       (System_Dtm, 
                                                                                                                                        E_Client_IP, 
                                                                                                                                        E_User_ID, 
                                                                                                                                        E_Description, 
                                                                                                                                        E_Application_Server, 
                                                                                                                                        E_Session_ID, 
                                                                                                                                        E_Browser, 
                                                                                                                                        E_OS
                                                                                                                                       )
                                                                                                                                VALUES
                                                                                                                                      (@System_Dtm, 
                                                                                                                                       @E_Client_IP, 
                                                                                                                                       @E_User_ID, 
                                                                                                                                       @E_Description, 
                                                                                                                                       @E_Application_Server, 
                                                                                                                                       @E_Session_ID, 
                                                                                                                                       @E_Browser, 
                                                                                                                                       @E_OS
                                                                                                                                       );
                                                                                                                            END;
                                                                                                                            ELSE
                                                                                                                            BEGIN
                                                                                                                                IF @year = '23'
                                                                                                                                    BEGIN
                                                                                                                                        INSERT INTO
                                                                                                                                        dbo.
                                                                                                                                        AppLogSSO23
                                                                                                                                               (
                                                                                                                                               System_Dtm, 
                                                                                                                                               E_Client_IP, 
                                                                                                                                               E_User_ID, 
                                                                                                                                               E_Description, 
                                                                                                                                               E_Application_Server, 
                                                                                                                                               E_Session_ID, 
                                                                                                                                               E_Browser, 
                                                                                                                                               E_OS
                                                                                                                                               )
                                                                                                                                        VALUES
                                                                                                                                              (
                                                                                                                                              @System_Dtm, 
                                                                                                                                              @E_Client_IP, 
                                                                                                                                              @E_User_ID, 
                                                                                                                                              @E_Description, 
                                                                                                                                              @E_Application_Server, 
                                                                                                                                              @E_Session_ID, 
                                                                                                                                              @E_Browser, 
                                                                                                                                              @E_OS
                                                                                                                                               );
                                                                                                                                    END;
                                                                                                                                    ELSE
                                                                                                                                    BEGIN
                                                                                                                                        IF @year =
                                                                                                                                        '24'
                                                                                                                                            BEGIN
                                                                                                                                                INSERT
                                                                                                                                                INTO
                                                                                                                                                dbo
                                                                                                                                                .
                                                                                                                                                AppLogSSO24
                                                                                                                                                       (
                                                                                                                                                       System_Dtm, 
                                                                                                                                                       E_Client_IP, 
                                                                                                                                                       E_User_ID, 
                                                                                                                                                       E_Description, 
                                                                                                                                                       E_Application_Server, 
                                                                                                                                                       E_Session_ID, 
                                                                                                                                                       E_Browser, 
                                                                                                                                                       E_OS
                                                                                                                                                       )
                                                                                                                                                VALUES
                                                                                                                                                      (
                                                                                                                                                      @System_Dtm, 
                                                                                                                                                      @E_Client_IP, 
                                                                                                                                                      @E_User_ID, 
                                                                                                                                                      @E_Description, 
                                                                                                                                                      @E_Application_Server, 
                                                                                                                                                      @E_Session_ID, 
                                                                                                                                                      @E_Browser, 
                                                                                                                                                      @E_OS
                                                                                                                                                       )
                                                                                                                                                ;
                                                                                                                                            END;
                                                                                                                                            ELSE
                                                                                                                                            BEGIN
                                                                                                                                                IF
                                                                                                                                                @year
                                                                                                                                                =
                                                                                                                                                '25'
                                                                                                                                                    BEGIN
                                                                                                                                                        INSERT
                                                                                                                                                        INTO
                                                                                                                                                        dbo
                                                                                                                                                        .
                                                                                                                                                        AppLogSSO25
                                                                                                                                                               (
                                                                                                                                                               System_Dtm, 
                                                                                                                                                               E_Client_IP, 
                                                                                                                                                               E_User_ID, 
                                                                                                                                                               E_Description, 
                                                                                                                                                               E_Application_Server, 
                                                                                                                                                               E_Session_ID, 
                                                                                                                                                               E_Browser, 
                                                                                                                                                               E_OS
                                                                                                                                                               )
                                                                                                                                                        VALUES
                                                                                                                                                              (
                                                                                                                                                              @System_Dtm, 
                                                                                                                                                              @E_Client_IP, 
                                                                                                                                                              @E_User_ID, 
                                                                                                                                                              @E_Description, 
                                                                                                                                                              @E_Application_Server, 
                                                                                                                                                              @E_Session_ID, 
                                                                                                                                                              @E_Browser, 
                                                                                                                                                              @E_OS
                                                                                                                                                               )
                                                                                                                                                        ;
                                                                                                                                                    END
                                                                                                                                                    ;
                                                                                                                                                    ELSE
                                                                                                                                                    BEGIN
                                                                                                                                                        IF
                                                                                                                                                        @year
                                                                                                                                                        =
                                                                                                                                                        '26'
                                                                                                                                                            BEGIN
                                                                                                                                                                INSERT
                                                                                                                                                                INTO
                                                                                                                                                                dbo
                                                                                                                                                                .
                                                                                                                                                                AppLogSSO26
                                                                                                                                                                       (
                                                                                                                                                                       System_Dtm, 
                                                                                                                                                                       E_Client_IP, 
                                                                                                                                                                       E_User_ID, 
                                                                                                                                                                       E_Description, 
                                                                                                                                                                       E_Application_Server, 
                                                                                                                                                                       E_Session_ID, 
                                                                                                                                                                       E_Browser, 
                                                                                                                                                                       E_OS
                                                                                                                                                                       )
                                                                                                                                                                VALUES
                                                                                                                                                                      (
                                                                                                                                                                      @System_Dtm, 
                                                                                                                                                                      @E_Client_IP, 
                                                                                                                                                                      @E_User_ID, 
                                                                                                                                                                      @E_Description, 
                                                                                                                                                                      @E_Application_Server, 
                                                                                                                                                                      @E_Session_ID, 
                                                                                                                                                                      @E_Browser, 
                                                                                                                                                                      @E_OS
                                                                                                                                                                       )
                                                                                                                                                                ;
                                                                                                                                                            END
                                                                                                                                                            ;
                                                                                                                                                            ELSE
                                                                                                                                                            BEGIN
                                                                                                                                                                IF
                                                                                                                                                                @year
                                                                                                                                                                =
                                                                                                                                                                '27'
                                                                                                                                                                    BEGIN
                                                                                                                                                                        INSERT
                                                                                                                                                                        INTO
                                                                                                                                                                        dbo
                                                                                                                                                                        .
                                                                                                                                                                        AppLogSSO27
                                                                                                                                                                               (
                                                                                                                                                                               System_Dtm, 
                                                                                                                                                                               E_Client_IP, 
                                                                                                                                                                               E_User_ID, 
                                                                                                                                                                               E_Description, 
                                                                                                                                                                               E_Application_Server, 
                                                                                                                                                                               E_Session_ID, 
                                                                                                                                                                               E_Browser, 
                                                                                                                                                                               E_OS
                                                                                                                                                                               )
                                                                                                                                                                        VALUES
                                                                                                                                                                              (
                                                                                                                                                                              @System_Dtm, 
                                                                                                                                                                              @E_Client_IP, 
                                                                                                                                                                              @E_User_ID, 
                                                                                                                                                                              @E_Description, 
                                                                                                                                                                              @E_Application_Server, 
                                                                                                                                                                              @E_Session_ID, 
                                                                                                                                                                              @E_Browser, 
                                                                                                                                                                              @E_OS
                                                                                                                                                                               )
                                                                                                                                                                        ;
                                                                                                                                                                    END
                                                                                                                                                                    ;
                                                                                                                                                                    ELSE
                                                                                                                                                                    BEGIN
                                                                                                                                                                        IF
                                                                                                                                                                        @year
                                                                                                                                                                        =
                                                                                                                                                                        '28'
                                                                                                                                                                            BEGIN
                                                                                                                                                                                INSERT
                                                                                                                                                                                INTO
                                                                                                                                                                                dbo
                                                                                                                                                                                .
                                                                                                                                                                                AppLogSSO28
                                                                                                                                                                                       (
                                                                                                                                                                                       System_Dtm, 
                                                                                                                                                                                       E_Client_IP, 
                                                                                                                                                                                       E_User_ID, 
                                                                                                                                                                                       E_Description, 
                                                                                                                                                                                       E_Application_Server, 
                                                                                                                                                                                       E_Session_ID, 
                                                                                                                                                                                       E_Browser, 
                                                                                                                                                                                       E_OS
                                                                                                                                                                                       )
                                                                                                                                                                                VALUES
                                                                                                                                                                                      (
                                                                                                                                                                                      @System_Dtm, 
                                                                                                                                                                                      @E_Client_IP, 
                                                                                                                                                                                      @E_User_ID, 
                                                                                                                                                                                      @E_Description, 
                                                                                                                                                                                      @E_Application_Server, 
                                                                                                                                                                                      @E_Session_ID, 
                                                                                                                                                                                      @E_Browser, 
                                                                                                                                                                                      @E_OS
                                                                                                                                                                                       )
                                                                                                                                                                                ;
                                                                                                                                                                            END
                                                                                                                                                                            ;
                                                                                                                                                                    END
                                                                                                                                                                    ;
                                                                                                                                                            END
                                                                                                                                                            ;
                                                                                                                                                    END
                                                                                                                                                    ;
                                                                                                                                            END;
                                                                                                                                    END;
                                                                                                                            END;
                                                                                                                    END;
                                                                                                            END;
                                                                                                    END;
                                                                                            END;
                                                                                    END;
                                                                            END;
                                                                    END;
                                                            END;
                                                    END;
                                            END;
                                    END;
                            END;
                    END;
            END;

        GRANT EXECUTE ON [dbo].[proc_ApplicationLogSSO_add] TO HCSP, HCVU;
    END;
GO