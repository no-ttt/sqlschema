using System;
using System.Collections.Generic;

namespace WebAPI
{
    public class AssessmentDetail
    {
        public class termFormat
        {
            public string ID { get; set; }
            public string Check { get; set; }
            public string Category { get; set; }
            public string Risk { get; set; }
            public string Des { get; set; }
            public string Impact { get; set; }
            public string Query { get; set; }
            public string Remediation { get; set; }
            public string RemediationScript { get; set; }
        }

        public class Term
        {
            public List<termFormat> allTerms { get; set; } = new List<termFormat>()
            {
                //new termFormat()
                //{
                //    ID = "VA1258",
                //    Check = "Database owners are as expected",
                //    Category = "Auditing And Logging",
                //    Risk = "High",
                //    Des = "Database owners can perform all configuration and maintenance activities on the database, and can also drop databases in SQL Server. Tracking database owners is important to avoid having excessive permission for some principals. Create a baseline which defines the expected database owners for the database. This rule checks whether the database owners are as defined in the baseline.",
                //    Impact = "Keeping track of database owners is important to avoid granting excessive permissions.",
                //    Query = @"SELECT USER_NAME(member_principal_id) AS [Owner]
                //                FROM sys.database_role_members
                //                WHERE USER_NAME(role_principal_id) = 'db_owner'
                //                    AND USER_NAME(member_principal_id) != 'dbo'",
                //    Remediation = "Keep track of database owners. Remove unnecessary database owners to avoid granting excessive permissions or update the baseline to reflect the approved list of owners.",
                //    RemediationScript = "ALTER ROLE db_owner DROP MEMBER [$0]"
                //},
                new termFormat()
                {
                    ID = "VA1265",
                    Check = "Auditing of both successful and failed login attempts for contained DB authentication should be enabled",
                    Category = "Auditing And Logging",
                    Risk = "Medium",
                    Des = "SQL Server auditing configuration enables adminstrators to track users logging to SQL Server instances that they’re responsible for. This rules checks that auditing is enabled for both successful and failed login attempts for contained DB authentication.",
                    Impact = "Logging successful & failed login attempts provides information that can be used to detect brute-force based password attacks against the system as well as forensic information.",
                    Query = @"DECLARE @check_results INT = 0;
DECLARE @violation INT = 1;

SELECT @check_results = containment
FROM   sys.databases
WHERE name = db_name();

PRINT @check_results

IF( @check_results != 0 )
BEGIN
    DECLARE @success_logon_event INT = 0;
    DECLARE @fail_logon_event INT = 0;

    SELECT @success_logon_event = Count(*)
    FROM   sys.server_audits adts,
            sys.server_audit_specifications srvadtspecs,
            sys.server_audit_specification_details srvadtspecdtls
    WHERE  adts.audit_guid = srvadtspecs.audit_guid
            AND adts.is_state_enabled = 1
            AND srvadtspecs.is_state_enabled = 1
            AND srvadtspecdtls.audited_result = 'SUCCESS AND FAILURE'
            AND srvadtspecdtls.audit_action_id = 'DAGS';

    SELECT @fail_logon_event = Count(*)
    FROM   sys.server_audits adts,
            sys.server_audit_specifications srvadtspecs,
            sys.server_audit_specification_details srvadtspecdtls
    WHERE  adts.audit_guid = srvadtspecs.audit_guid
            AND adts.is_state_enabled = 1
            AND srvadtspecs.is_state_enabled = 1
            AND srvadtspecdtls.audited_result = 'SUCCESS AND FAILURE'
            AND srvadtspecdtls.audit_action_id = 'DAGF';

    DECLARE @db_success_logon_event INT = 0;
    DECLARE @db_fail_logon_event INT = 0;

    SELECT @db_success_logon_event = Count(*)
    FROM   sys.server_audits adts,
            sys.database_audit_specifications dbadtspecs,
            sys.database_audit_specification_details dbadtspecdtls
    WHERE  adts.audit_guid = dbadtspecs.audit_guid
            AND adts.is_state_enabled = 1
            AND dbadtspecs.is_state_enabled = 1
            AND dbadtspecdtls.audited_result = 'SUCCESS AND FAILURE'
            AND dbadtspecdtls.audit_action_id = 'DAGS';

    SELECT @db_fail_logon_event = Count(*)
    FROM   sys.server_audits adts,
            sys.database_audit_specifications dbadtspecs,
            sys.database_audit_specification_details dbadtspecdtls
    WHERE  adts.audit_guid = dbadtspecs.audit_guid
            AND adts.is_state_enabled = 1
            AND dbadtspecs.is_state_enabled = 1
            AND dbadtspecdtls.audited_result = 'SUCCESS AND FAILURE'
            AND dbadtspecdtls.audit_action_id = 'DAGF';

    IF( ( @success_logon_event
        + @db_success_logon_event ) > 0
        AND ( @fail_logon_event + @db_fail_logon_event ) > 0 )
    SET @violation = 0;
END
ELSE
BEGIN
    SET @violation = 0; -- ignore if DB is not contained
END

SELECT @violation AS [Violation]",
                    Remediation = "Create and enable a new SERVER AUDIT SPECIFICATION that will audit FAILED_LOGIN_GROUP & SUCCESSFUL_LOGIN_GROUP events. This SERVER AUDIT SPECIFICATION must target a valid SERVER AUDIT object that is enabled.",
                    RemediationScript = @"CREATE DATABASE AUDIT SPECIFICATION [DbAuditSpec_db_logon_information_failed_succeessful]
                                                FOR SERVER AUDIT []
                                            ADD (FAILED_DATABASE_AUTHENTICATION_GROUP),
                                            ADD (SUCCESSFUL_DATABASE_AUTHENTICATION_GROUP)
                                            WITH (STATE = ON)"
                },
                new termFormat()
                {
                    ID = "VA1281",
                    Check = "All memberships for user-defined roles should be intended",
                    Category = "Auditing And Logging",
                    Risk = "Medium",
                    Des = "User-defined roles are security principals defined by the user to group principals to easily manage permissions. Monitoring these roles is important to avoid having excessive permissions. Create a baseline which defines expected membership for each user-defined role. This rule checks whether all memberships for user-defined roles are as defined in the baseline.",
                    Impact = "Keeping track of role memberships is important to avoid granting excessive permissions.",
                    Query = @"SELECT user_name(role_principal_id) AS [Role], user_name(member_principal_id) AS [Member]
FROM sys.database_role_members
WHERE role_principal_id NOT IN (16384,16385,16386,16387,16389,16390,16391,16392,16393)",
                    Remediation = "Keep track of role membership and remove unnecessary members from roles to avoid granting excessive permissions or update baseline to comply with new changes.",
                    RemediationScript = "ALTER ROLE [$0] DROP MEMBER [$1]"
                },
                new termFormat()
                {
                    ID = "VA1020",
                    Check = "Database user GUEST should not be a member of any role",
                    Category = "Authentication And Authorization",
                    Risk = "High",
                    Des = "The guest user permits access to a database for any logins that are not mapped to a specific database user. This rule checks that no database roles are assigned to the Guest user.",
                    Impact = "Database Roles are the basic building block at the heart of separation of duties and the principle of least permission. Granting the Guest user membership to specific roles defeats this purpose.",
                    Query = @"SELECT name as [Role]
FROM sys.database_role_members AS drms
INNER JOIN sys.database_principals AS dps
    ON drms.role_principal_id = dps.principal_id
WHERE member_principal_id = DATABASE_PRINCIPAL_ID('guest')",
                    Remediation = "Remove the special user GUEST from all roles.",
                    RemediationScript = "ALTER ROLE [$0] DROP MEMBER GUEST"
                },
                new termFormat()
                {
                    ID = "VA1048",
                    Check = "Database principals should not be mapped to the sa account",
                    Category = "Authentication And Authorization",
                    Risk = "High",
                    Des = "A database principal that is mapped to the sa account can be exploited by an attacker to elevate permissions to sysadmin.",
                    Impact = "This enables privileged principals on a database to perform operations on other databases that have ownership chaining enabled – specifically msdb. An attacker can then exploit msdb to become sysadmin.",
                    Query = @"SELECT name AS [Principal]
    , SUSER_SNAME(sid) AS [Login]
FROM sys.database_principals
WHERE sid = 0x01
    AND principal_id != 1",
                    Remediation = "Change the login associated with the offending user and investigate why this user exists.",
                    RemediationScript = @"DECLARE @newlogin sysname = 'investigate_VA1048_' + convert(nvarchar(50),newid()); DECLARE @cmd nvarchar(max); SET @cmd = 'CREATE LOGIN ' + quotename(@newlogin) + ' WITH PASSWORD = ''' + convert(nvarchar(50),newid()) + ''', CHECK_POLICY = OFF;'; SET @cmd = @cmd + 'ALTER LOGIN ' + quotename(@newlogin) + ' DISABLE;'; SET @cmd = @cmd + 'ALTER USER [$0] WITH LOGIN = ' + quotename(@newlogin) + ';' EXEC( @cmd )"
                },
                new termFormat()
                {
                    ID = "VA2020",
                    Check = "Minimal set of principals should be granted ALTER or ALTER ANY USER database-scoped permissions",
                    Category = "Authentication And Authorization",
                    Risk = "High",
                    Des = "Every SQL Server securable has permissions associated with it that can be granted to principals. Permissions can be scoped at the server level (assigned to logins and server roles) or at the database level (assigned to database users and database roles). These rules check that only a minimal set of principals are granted ALTER or ALTER ANY USER database-scoped permissions.",
                    Impact = "Developing an application using a least-privileged user account (LUA) approach is an important part of a defensive, in-depth strategy for countering security threats. The LUA approach ensures that users follow the principle of least privilege and always log on with limited user accounts. Administrative tasks are broken out using fixed server roles, and the use of the sysadmin fixed server role is severely restricted. Always follow the principle of least privilege when granting permissions to database users. Grant the minimum permissions necessary to a user or role to accomplish a given task. See https://msdn.microsoft.com/en-us/library/bb669084(v=vs.110).aspx.",
                    Query = @"SELECT perms.class_desc AS [Permission_Class]
    ,perms.permission_name AS [Permission]
    ,type_desc AS [Principal_Type]
    ,prin.name AS [Principal]
FROM sys.database_permissions AS perms
INNER JOIN sys.database_principals AS prin ON perms.grantee_principal_id = prin.principal_id
WHERE permission_name IN (
        'ALTER'
        ,'ALTER ANY USER'
        )
    AND user_name(grantee_principal_id) NOT IN (
        'guest'
        ,'public'
        )
    AND perms.class = 0
    AND [state] IN ('G','W')
    AND NOT (
        prin.type = 'S'
        AND prin.name = 'dbo'
        AND prin.authentication_type = 1
        AND prin.owning_principal_id IS NULL
        )",
                    Remediation = "Revoke permissions from principals where not needed. It is recommended to have at most 1 principal granted a specific permission.",
                    RemediationScript = "REVOKE $1 FROM [$3]"
                },
                new termFormat()
                {
                    ID = "VA2108",
                    Check = "Minimal set of principals should be members of fixed high impact database roles",
                    Category = "Authentication And Authorization",
                    Risk = "High",
                    Des = "SQL Server provides roles to help manage the permissions. Roles are security principals that group other principals. Database-level roles are database-wide in their permission scope. This rule checks that a minimal set of principals are members of the fixed database roles.",
                    Impact = "Fixed database roles may have administrative permissions on the system. Following the principle of least privilege, it is important to minimize membership in fixed database roles and keep a baseline of these memberships. See https://docs.microsoft.com/en-us/sql/relational-databases/security/authentication-access/database-level-roles for additional information on database roles.",
                    Query = @"SELECT user_name(sr.member_principal_id) AS [Principal]
    ,user_name(sr.role_principal_id) AS [Role]
    ,type_desc AS [Principal_Type]
FROM sys.database_role_members AS sr
INNER JOIN sys.database_principals sp ON sp.principal_id = sr.member_principal_id
WHERE sr.role_principal_id IN (
        user_id('bulkadmin'),
        user_id('db_accessadmin'),
        user_id('db_securityadmin'),
        user_id('db_ddladmin'),
        user_id('db_backupoperator'))
    OR (sr.role_principal_id = user_id('db_owner')
        AND sr.member_principal_id <> user_id('dbo'))",
                    Remediation = "Remove members who should not have access to the database role.",
                    RemediationScript = "ALTER ROLE [$1] DROP MEMBER [$0]"
                },
                new termFormat()
                {
                    ID = "VA2129",
                    Check = "Changes to signed modules should be authorized",
                    Category = "Authentication And Authorization",
                    Risk = "High",
                    Des = "You can sign a stored procedure, function or trigger with a certificate or an asymmetric key. This is designed for scenarios when permissions cannot be inherited through ownership chaining or when the ownership chain is broken, such as dynamic SQL. This rule checks for changes made to signed modules which could be an indication of malicious use.",
                    Impact = "Changes made to the contents of a signed module or to the certificate or asymmetric key that is used to sign it, as well as the introduction of new signed modules could be an an indication of an attack. Setting the known signed modules as a baseline allows you to easily detect changes made, and to evaluate whether the changes are intended.",
                    Query = @"SELECT 
    QUOTENAME(sc.name) + '.' + QUOTENAME(oj.name) AS [Module]
    ,IIF(ct.certificate_id IS NOT NULL, ct.name, ak.name) AS [Signing_Object]
    ,dp.name AS [Signing_Object_Owner]
    ,cp.thumbprint AS [Signing_Object_Thumbprint]
    ,oj.modify_date AS [Last_Definition_Modify_Date]
    ,IIF(ct.certificate_id IS NOT NULL, 'CERTIFICATE', 'ASYMMETRIC KEY') AS [Signing_Object_Type]
    -- For debbuging, uncomment following lines:
    -- ,IIF(ct.principal_id IS NOT NULL, SUSER_NAME(ct.principal_id), SUSER_NAME(ak.principal_id)) AS [Owner_Name]
    -- ,oj.type_desc
    -- ,crypt_type
    -- ,md.DEFINITION 
    -- ,IIF(ct.subject IS NOT NULL, ct.subject, 'N/A') AS [Certificate Subject]
    -- ,IIF(ct.certificate_id IS NOT NULL, IS_OBJECTSIGNED('OBJECT', oj.object_id, 'certificate', cp.thumbprint), IS_OBJECTSIGNED('OBJECT', oj.object_id, 'asymmetric key', cp.thumbprint)) AS [Is Object Signed]
FROM 
    sys.crypt_properties AS cp
    INNER JOIN sys.objects AS oj ON cp.major_id = oj.object_id
    INNER JOIN sys.schemas AS sc ON oj.schema_id = sc.schema_id
    INNER JOIN sys.sql_modules AS md ON md.object_id = cp.major_id
    LEFT OUTER JOIN sys.certificates AS ct ON cp.thumbprint = ct.thumbprint
    LEFT OUTER JOIN sys.asymmetric_keys AS ak ON cp.thumbprint = ak.thumbprint
    LEFT OUTER JOIN sys.database_principals AS dp ON (ct.sid = dp.sid OR ak.sid = dp.sid)
WHERE 
    oj.type IN ('P','FN','TR')
    AND cp.class_desc = 'OBJECT_OR_COLUMN'",
                    Remediation = "Baseline or remove the signature from the modules.",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1043",
                    Check = "Principal GUEST should not have access to any user database",
                    Category = "Authentication And Authorization",
                    Risk = "Medium",
                    Des = "The guest user permits access to a database for any logins that are not mapped to a specific database user. This rule checks that the guest user cannot connect to any database.",
                    Impact = "The special user GUEST is used to map any login that otherwise has no access to the database. This can result in principals gaining access to a database without having been explicitly granted permission to do so.",
                    Query = @"SELECT CASE
WHEN EXISTS (
    SELECT *
    FROM sys.database_permissions AS perms
    INNER JOIN sys.database_principals AS usrs ON grantee_principal_id = principal_id
        WHERE grantee_principal_id = Database_principal_id('guest')
            AND perms.type = 'CO'
            AND [state] IN ('G', 'W')
)
THEN 1
ELSE 0
END AS [Violation]",
                    Remediation = "Remove all permissions granted to GUEST, especially the connect permission.",
                    RemediationScript = "REVOKE CONNECT FROM [GUEST]"
                },
                new termFormat()
                {
                    ID = "VA1095",
                    Check = "Excessive permissions should not be granted to PUBLIC role",
                    Category = "Authentication And Authorization",
                    Risk = "Medium",
                    Des = "Every SQL Server login belongs to the public server role. When a server principal has not been granted or denied specific permissions on a securable object, the user inherits the permissions granted to public on that object. This displays a list of all permissions that are granted to the PUBLIC role.",
                    Impact = "Database Roles are the basic building block at the heart of separation of duties and the principle of least permission. Granting permissions to principals through the default PUBLIC role defeats this purpose.",
                    Query = @"SELECT REPLACE(perms.class_desc, '_', ' ') AS [Permission_Class]
    ,CASE
        WHEN perms.class = 0
            THEN db_name() -- database
        WHEN perms.class = 3
            THEN schema_name(major_id) -- schema
        WHEN perms.class = 4
            THEN printarget.NAME -- principal
        WHEN perms.class = 5
            THEN asm.NAME -- assembly
        WHEN perms.class = 6
            THEN type_name(major_id) -- type
        WHEN perms.class = 10
            THEN xmlsc.NAME -- xml schema
        WHEN perms.class = 15
            THEN msgt.NAME COLLATE DATABASE_DEFAULT -- message types
        WHEN perms.class = 16
            THEN svcc.NAME COLLATE DATABASE_DEFAULT -- service contracts
        WHEN perms.class = 17
            THEN svcs.NAME COLLATE DATABASE_DEFAULT -- services
        WHEN perms.class = 18
            THEN rsb.NAME COLLATE DATABASE_DEFAULT -- remote service bindings
        WHEN perms.class = 19
            THEN rts.NAME COLLATE DATABASE_DEFAULT -- routes
        WHEN perms.class = 23
            THEN ftc.NAME -- full text catalog
        WHEN perms.class = 24
            THEN sym.NAME -- symmetric key
        WHEN perms.class = 25
            THEN crt.NAME -- certificate
        WHEN perms.class = 26
            THEN asym.NAME -- assymetric key
        END AS [Object]
    ,perms.permission_name AS [Permission]
FROM sys.database_permissions AS perms
LEFT JOIN sys.database_principals AS prin ON perms.grantee_principal_id = prin.principal_id
LEFT JOIN sys.assemblies AS asm ON perms.major_id = asm.assembly_id
LEFT JOIN sys.xml_schema_collections AS xmlsc ON perms.major_id = xmlsc.xml_collection_id
LEFT JOIN sys.service_message_types AS msgt ON perms.major_id = msgt.message_type_id
LEFT JOIN sys.service_contracts AS svcc ON perms.major_id = svcc.service_contract_id
LEFT JOIN sys.services AS svcs ON perms.major_id = svcs.service_id
LEFT JOIN sys.remote_service_bindings AS rsb ON perms.major_id = rsb.remote_service_binding_id
LEFT JOIN sys.routes AS rts ON perms.major_id = rts.route_id
LEFT JOIN sys.database_principals AS printarget ON perms.major_id = printarget.principal_id
LEFT JOIN sys.symmetric_keys AS sym ON perms.major_id = sym.symmetric_key_id
LEFT JOIN sys.asymmetric_keys AS asym ON perms.major_id = asym.asymmetric_key_id
LEFT JOIN sys.certificates AS crt ON perms.major_id = crt.certificate_id
LEFT JOIN sys.fulltext_catalogs AS ftc ON perms.major_id = ftc.fulltext_catalog_id
WHERE perms.grantee_principal_id = DATABASE_PRINCIPAL_ID('public')
    AND class != 1 -- Object or Columns (class = 1) are handled by VA1054 and have different remediation syntax
    AND [state] IN ('G','W')
    AND NOT (
        perms.class = 0
        AND prin.NAME = 'public'
        AND perms.major_id = 0
        AND perms.minor_id = 0
        AND permission_name IN (
            'VIEW ANY COLUMN ENCRYPTION KEY DEFINITION'
            ,'VIEW ANY COLUMN MASTER KEY DEFINITION'
            )
        )",
                    Remediation = "Revoke any unnecessary permissions granted to PUBLIC, but avoid changing permissions granted out of the box.",
                    RemediationScript = "Revoke any unnecessary permissions granted to PUBLIC, but avoid changing permissions granted out of the box."
                },
                new termFormat()
                {
                    ID = "VA1248",
                    Check = "User-defined database roles should not be members of fixed roles",
                    Category = "Authentication And Authorization",
                    Risk = "Medium",
                    Des = "To easily manage the permissions in your databases, SQL Server provides several roles which are security principals that group other principals. They are like groups in the Microsoft Windows operating system. Database accounts and other SQL Server roles can be added into database-level roles. Each member of a fixed-database role can add other users to that same role. This rule checks that no user-defined roles are members of fixed roles.",
                    Impact = "Adding user defined database roles as members of fixed roles could enable unintended privilege escalation, also finding any metadata indicating that the fixed roles have been modified is typically a sign of data corruption or signs of somebody corrupting the metadata in order to hide unusual activity.",
                    Query = @"SELECT user_name(roles.role_principal_id) as [Role], user_name(roles.member_principal_id) as [Member]
FROM sys.database_role_members AS roles, sys.database_principals users
WHERE roles.member_principal_id = users.principal_id
AND ( roles.role_principal_id >= 16384 AND roles.role_principal_id <= 16393)
AND users.type = 'R'",
                    Remediation = "Remove user defined roles from fixed-database roles.",
                    RemediationScript = "ALTER ROLE [$0] DROP MEMBER [$1]"
                },
                new termFormat()
                {
                    ID = "VA1267",
                    Check = "Contained users should use Windows Authentication",
                    Category = "Authentication And Authorization",
                    Risk = "Medium",
                    Des = "Contained users are users that exist within the database, and do not require a login mapping. This rule checks that contained users use Windows Authentication.",
                    Impact = "Authentication must be centrally managed in order to enforce the domain password policies.",
                    Query = @"SELECT NAME AS [Principal]
FROM   sys.database_principals
WHERE  authentication_type = 2",
                    Remediation = "Remove all contained users with password. All affected applications will have to switch to Windows authentication, or create logins with password policy enforcement in order to connect to the DB.",
                    RemediationScript = "DROP USER [$0];"
                },
                new termFormat()
                {
                    ID = "VA1054",
                    Check = "Excessive permissions should not be granted to PUBLIC role on objects or columns",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Every SQL Server login belongs to the public server role. When a server principal has not been granted or denied specific permissions on a securable object, the user inherits the permissions granted to public on that object. This rule displays a list of all securable objects or columns that are accessible to all users through the PUBLIC role.",
                    Impact = "Database Roles are the basic building block at the heart of separation of duties and the principle of least permission. Granting permissions to principals through the default PUBLIC role defeats this purpose.",
                    Query = @"SELECT permission_name AS [Permission]
,schema_name AS [Schema]
,object_name AS [Object]
FROM (
SELECT objs.TYPE COLLATE database_default AS object_type
    ,schema_name(schema_id) COLLATE database_default AS schema_name
    ,objs.name COLLATE database_default AS object_name
    ,user_name(grantor_principal_id) COLLATE database_default AS grantor_principal_name
    ,permission_name COLLATE database_default AS permission_name
    ,perms.TYPE COLLATE database_default AS TYPE
    ,STATE COLLATE database_default AS STATE
FROM sys.database_permissions AS perms
INNER JOIN sys.objects AS objs
ON objs.object_id = perms.major_id
WHERE perms.class = 1 -- objects or columns. Other cases are handled by VA1095 which has different remediation syntax
AND grantee_principal_id = DATABASE_PRINCIPAL_ID('public')
AND [state] IN (
'G'
,'W'
)
AND NOT (
-- These permissions are granted by default to public
permission_name = 'EXECUTE'
AND schema_name(schema_id) = 'dbo'
AND STATE = 'G'
AND objs.name IN (
'fn_sysdac_is_dac_creator'
,'fn_sysdac_is_currentuser_sa'
,'fn_sysdac_is_login_creator'
,'fn_sysdac_get_username'
,'sp_sysdac_ensure_dac_creator'
,'sp_sysdac_add_instance'
,'sp_sysdac_add_history_entry'
,'sp_sysdac_delete_instance'
,'sp_sysdac_upgrade_instance'
,'sp_sysdac_drop_database'
,'sp_sysdac_rename_database'
,'sp_sysdac_setreadonly_database'
,'sp_sysdac_rollback_committed_step'
,'sp_sysdac_update_history_entry'
,'sp_sysdac_resolve_pending_entry'
,'sp_sysdac_rollback_pending_object'
,'sp_sysdac_rollback_all_pending_objects'
,'fn_sysdac_get_currentusername'
)
OR permission_name = 'SELECT'
AND schema_name(schema_id) = 'sys'
AND STATE = 'G'
AND objs.name IN (
'firewall_rules'
,'database_firewall_rules'
,'ipv6_database_firewall_rules'
,'bandwidth_usage'
,'database_usage'
,'external_library_setup_errors'
,'sql_feature_restrictions'
,'resource_stats'
,'elastic_pool_resource_stats'
,'dm_database_copies'
,'geo_replication_links'
,'database_error_stats'
,'event_log'
,'database_connection_stats'
)
OR permission_name = 'SELECT'
AND schema_name(schema_id) = 'dbo'
AND STATE = 'G'
AND objs.name IN (
'sysdac_instances_internal'
,'sysdac_history_internal'
,'sysdac_instances'
)
)

) t",
                    Remediation = "Revoke unnecessary permissions granted to PUBLIC",
                    RemediationScript = "REVOKE $0 ON [$1].[$2] FROM PUBLIC"
                },
                new termFormat()
                {
                    ID = "VA1070",
                    Check = "Database users shouldn’t share the same name as a server login",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Database users may share the same name as a server login. This rule validates that there are no such users to avoid confusion.",
                    Impact = "Logins are created at the server level, while users are created at the database level. There are different types of users. Users with login are mapped to server level logins. Users with passwords can be created on a specific database, but are not mapped to server logins. This rule checks that users with passwords do not have the same name as any SQL login, as that can lead to false assessments of access rights.",
                    Query = @"SELECT dp.NAME AS [Principal]
FROM   sys.database_principals AS dp
JOIN   sys.server_principals AS sp
        ON dp.NAME = sp.NAME COLLATE database_default
WHERE  dp.sid != sp.sid
        AND dp.authentication_type = 2",
                    Remediation = "You should rename the affected users or logins to avoid the confusion. This requires updating all applications which are using those as credentials as well.",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1094",
                    Check = "Database permissions shouldn’t be granted directly to principals",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Permissions are rules associated with a securable object to regulate which users can gain access to the object. This rule checks that there are no DB permissions granted directly to users.",
                    Impact = "Individuals change organizations & job descriptions over time. It is highly recommended to use a centralized access control management through AD group membership.",
                    Query = @"SELECT permission_name                  AS [Permission],
        ISNULL(Schema_name(objs.schema_id), Replace(dp.class_desc, '_', ' ')) AS [Permission_Class],
        CASE 
            WHEN Schema_name(objs.schema_id) IS NULL  THEN '::' ELSE '.' End AS [Class_Separator],
        CASE
            WHEN dp.class = 0 THEN Db_name() -- database
            WHEN dp.class = 1 THEN objs.NAME -- Object or Column (VA1286)
            WHEN dp.class = 3 THEN Schema_name(major_id) -- schema
            WHEN dp.class = 4 THEN printarget.NAME -- principal
            WHEN dp.class = 5 THEN asm.NAME -- assembly
            WHEN dp.class = 6 THEN Type_name(major_id) -- type
            WHEN dp.class = 10 THEN xmlsc.NAME -- xml schema
            WHEN dp.class = 15 THEN msgt.NAME COLLATE database_default -- message types
            WHEN dp.class = 16 THEN svcc.NAME COLLATE database_default -- service contracts
            WHEN dp.class = 17 THEN svcs.NAME COLLATE database_default -- services
            WHEN dp.class = 18 THEN rsb.NAME COLLATE database_default -- remote service bindings
            WHEN dp.class = 19 THEN rts.NAME COLLATE database_default -- routes
            WHEN dp.class = 23 THEN ftc.NAME -- full text catalog
            WHEN dp.class = 24 THEN sym.NAME -- symmetric key
            WHEN dp.class = 25 THEN crt.NAME -- certificate
            WHEN dp.class = 26 THEN asym.NAME -- assymetric key
        END                              AS [Object],
        prin.NAME                        AS [Principal]
FROM sys.database_permissions AS dp
LEFT JOIN sys.all_objects AS objs ON objs.object_id = dp.major_id
LEFT JOIN sys.database_principals AS prin ON dp.grantee_principal_id = prin.principal_id
LEFT JOIN sys.assemblies AS asm ON dp.major_id = asm.assembly_id
LEFT JOIN sys.xml_schema_collections AS xmlsc ON dp.major_id = xmlsc.xml_collection_id
LEFT JOIN sys.service_message_types AS msgt ON dp.major_id = msgt.message_type_id
LEFT JOIN sys.service_contracts AS svcc ON dp.major_id = svcc.service_contract_id
LEFT JOIN sys.services AS svcs ON dp.major_id = svcs.service_id
LEFT JOIN sys.remote_service_bindings AS rsb ON dp.major_id = rsb.remote_service_binding_id
LEFT JOIN sys.routes AS rts ON dp.major_id = rts.route_id
LEFT JOIN sys.database_principals AS printarget ON dp.major_id = printarget.principal_id
LEFT JOIN sys.symmetric_keys AS sym ON dp.major_id = sym.symmetric_key_id
LEFT JOIN sys.asymmetric_keys AS asym ON dp.major_id = asym.asymmetric_key_id
LEFT JOIN sys.certificates AS crt ON dp.major_id = crt.certificate_id
LEFT JOIN sys.fulltext_catalogs AS ftc ON dp.major_id = ftc.fulltext_catalog_id
WHERE (prin.type = 'S' OR prin.type = 'W')
AND dp.type != 'CO'
AND prin.NAME NOT IN ( '##MS_PolicyEventProcessingLogin##',
                        '##MS_PolicyTsqlExecutionLogin##' )
AND [state] IN ('G','W')",
                    Remediation = "Revoke permissions granted to users directly. Instead use Windows groups or server roles to grant permissions, and manage role memberships instead.",
                    RemediationScript = "REVOKE $0 ON $1$2[$3] FROM [$4]"
                },
                new termFormat()
                {
                    ID = "VA1096",
                    Check = "Principal GUEST should not be granted permissions in the database",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Each database includes a user called GUEST. Permissions granted to GUEST are inherited by users who have access to the database, but who do not have a user account in the database. This rule checks that all permissions have been revoked from the GUEST user.",
                    Impact = "The special user GUEST is used to map any logins that are not mapped to a specific database user. This can result in principals gaining access to a database without having been explicitly granted permission to do so.",
                    Query = @"SELECT perms.permission_name AS [Permission]
FROM sys.database_permissions AS perms
INNER JOIN sys.database_principals AS prin ON perms.grantee_principal_id = prin.principal_id
WHERE prin.[name] = 'guest'
    AND perms.class = 0
    AND [state] IN ('G', 'W')",
                    Remediation = "Revoke any unnecessary permissions granted to the special account GUEST.",
                    RemediationScript = "REVOKE $0 FROM GUEST"
                },
                new termFormat()
                {
                    ID = "VA1097",
                    Check = "Principal GUEST should not be granted permissions on objects or columns",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Each database includes a user called GUEST. Permissions granted to GUEST are inherited by users who have access to the database, but who do not have a user account in the database. This rule checks that all permissions have been revoked from the GUEST user.",
                    Impact = "The special user GUEST is used to map any logins that are not mapped to a specific database user. This can result in principals gaining access to a database without having been explicitly granted permission to do so.",
                    Query = @"SELECT object_schema_name(major_id) AS [Schema]
    ,object_name(major_id) AS [Object]
    ,perms.permission_name AS [Permission]
FROM sys.database_permissions AS perms
INNER JOIN sys.database_principals AS prin ON perms.grantee_principal_id = prin.principal_id
WHERE grantee_principal_id = DATABASE_PRINCIPAL_ID('guest')
    AND perms.class = 1
    AND [state] IN ('G','W')",
                    Remediation = "Revoke any unnecessary permissions granted to the special account GUEST",
                    RemediationScript = "REVOKE $2 ON [$0].[$1] FROM GUEST"
                },
                new termFormat()
                {
                    ID = "VA1099",
                    Check = "GUEST user should not be granted permissions on database securables",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Each database includes a user called GUEST. Permissions granted to GUEST are inherited by users who have access to the database, but who do not have a user account in the database. This rule checks that all permissions have been revoked from the GUEST user.",
                    Impact = "The special user GUEST is used to map any logins that are not mapped to a specific database user. This can result in principals gaining access to a database without having been explicitly granted permission to do so.",
                    Query = @"SELECT REPLACE(perms.class_desc, '_', ' ') AS [Permission_Class],
    CASE
        WHEN perms.class=3 THEN schema_name(major_id) -- schema
        WHEN perms.class=4 THEN printarget.name -- principal
        WHEN perms.class=5 THEN asm.name -- assembly
        WHEN perms.class=5 THEN asm.name -- assembly
        WHEN perms.class=6 THEN type_name(major_id) -- type
        WHEN perms.class=10 THEN xmlsc.name -- xml schema
        WHEN perms.class=15 THEN msgt.name COLLATE DATABASE_DEFAULT -- message types
        WHEN perms.class=16 THEN svcc.name COLLATE DATABASE_DEFAULT -- service contracts
        WHEN perms.class=17 THEN svcs.name COLLATE DATABASE_DEFAULT -- services
        WHEN perms.class=18 THEN rsb.name COLLATE DATABASE_DEFAULT -- remote service bindings
        WHEN perms.class=19 THEN rts.name COLLATE DATABASE_DEFAULT -- routes
        WHEN perms.class=23 THEN ftc.name -- full text catalog
        WHEN perms.class=24 then sym.name -- symmetric key
        WHEN perms.class=25 then crt.name -- certificate
        WHEN perms.class=26 then asym.name -- assymetric key
    END AS [Object],
    perms.permission_name AS [Permission]
FROM sys.database_permissions AS perms
LEFT JOIN
    sys.database_principals AS prin
    ON perms.grantee_principal_id = prin.principal_id
LEFT JOIN
    sys.assemblies AS asm
    ON perms.major_id = asm.assembly_id
LEFT JOIN
    sys.xml_schema_collections AS xmlsc
    ON perms.major_id = xmlsc.xml_collection_id
LEFT JOIN
    sys.service_message_types AS msgt
    ON perms.major_id = msgt.message_type_id
LEFT JOIN
    sys.service_contracts AS svcc
    ON perms.major_id = svcc.service_contract_id
LEFT JOIN
    sys.services AS svcs
    ON perms.major_id = svcs.service_id
LEFT JOIN
    sys.remote_service_bindings AS rsb
    ON perms.major_id = rsb.remote_service_binding_id
LEFT JOIN
    sys.routes AS rts
    ON perms.major_id = rts.route_id
LEFT JOIN
    sys.database_principals AS printarget
    ON perms.major_id = printarget.principal_id
LEFT JOIN
    sys.symmetric_keys AS sym
    On perms.major_id = sym.symmetric_key_id
LEFT JOIN
    sys.asymmetric_keys AS asym
    ON perms.major_id = asym.asymmetric_key_id
    LEFT JOIN
    sys.certificates AS crt
    ON perms.major_id = crt.certificate_id
LEFT JOIN
    sys.fulltext_catalogs AS ftc
    ON perms.major_id = ftc.fulltext_catalog_id
WHERE
    grantee_principal_id = DATABASE_PRINCIPAL_ID('guest')
    AND class in (3,4,5,6,10,15,16,17,18,19,23,24,25,26)
    AND [state] IN ('G','W')",
                    Remediation = "Revoke any unnecessary permissions granted to the special account GUEST.",
                    RemediationScript = "REVOKE $2 ON $0::[$1] FROM GUEST"
                },
                new termFormat()
                {
                    ID = "VA1246",
                    Check = "Application roles should not be used",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "An application role is a database principal that enables an application to run with its own user-like permissions. Application roles enable that only users connecting through a particular application can access specific data. Application roles are password-based (which applications typically hardcode) and not permission based, which exposes the database to approle impersonation by password-guessing. This rule checks that no application roles are defined in the database.",
                    Impact = "It is important to limit the possibility of acquiring user-like permissions to the database, and since application roles are password based, they can lead to impersonation of the application role via password-guessing.",
                    Query = @"SELECT name AS [Role]
FROM sys.database_principals
WHERE type  = 'A'",
                    Remediation = "Remove the application roles. Use users without login as a permission-based replacement.",
                    RemediationScript = "DROP APPLICATION ROLE [$0]"
                },
                new termFormat()
                {
                    ID = "VA1282",
                    Check = "Orphan database roles should be removed",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "Orphan database roles are user-defined roles that have no members. It is recommended to eliminate orphaned roles as they are not needed on the system. This rule checks whether there are any orphan roles.",
                    Impact = "Reduce the attack surface area by eliminating unnecessary database roles in the system.",
                    Query = @"SELECT name AS [Role]
FROM sys.database_principals
WHERE type = 'R'
AND principal_id not in (0,16384,16385,16386,16387,16389,16390,16391,16392,16393)
AND principal_id not in ( 
    SELECT distinct role_principal_id
    FROM sys.database_role_members
    )",
                    Remediation = "Drop the unnecessary database roles.",
                    RemediationScript = "DROP ROLE [$0]"
                },
                new termFormat()
                {
                    ID = "VA2033",
                    Check = "Minimal set of principals should be granted EXECUTE permission on objects or columns",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "This rule checks which principals are granted EXECUTE permission on objects or columns to ensure this permission is granted to a minimal set of principals. Every SQL Server securable has permissions associated with it that can be granted to principals. Permissions can be scoped at the server level (assigned to logins and server roles) or at the database level (assigned to database users, database roles or application roles). The EXECUTE permission applies to both stored procedures and scalar functions, which can be used in computed columns.",
                    Impact = "Developing an application using a least-privileged user account (LUA) approach is an important part of a defensive, in-depth strategy for countering security threats. The LUA approach ensures that users follow the principle of least privilege and always log on with limited user accounts. Administrative tasks are broken out using fixed server roles, and the use of the sysadmin fixed server role is severely restricted. Always follow the principle of least privilege when granting permissions to database users. Grant the minimum permissions necessary to a user or role to accomplish a given task. See https://msdn.microsoft.com/en-us/library/bb669084(v=vs.110).aspx.",
                    Query = @"IF object_id('tempdb.dbo.#entries_to_exclude_va2033', 'U') IS NOT NULL
    DROP TABLE #entries_to_exclude_va2033;

CREATE TABLE #entries_to_exclude_va2033 (
                object_name VARCHAR(64),
                state_desc VARCHAR(24),
                prin_name VARCHAR(64),
                user_name VARCHAR(20),
                prin_type CHAR(1)
            )
     
INSERT INTO #entries_to_exclude_va2033 (object_name, state_desc, prin_name, user_name, prin_type)
    VALUES ('sp_add_job', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_add_jobschedule', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_add_jobserver', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_add_jobstep', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_addtask', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_delete_job', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_delete_jobschedule', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_delete_jobserver', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_delete_jobstep', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_droptask', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_post_msx_operation', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_start_job', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_stop_job', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_update_job', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_update_jobschedule', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_update_jobstep', 'DENY', 'TargetServersRole', 'dbo', 'R')
        ,('sp_syspolicy_events_reader', 'GRANT', '##MS_PolicyEventProcessingLogin##', 'dbo', 'S')
        ,('sp_syspolicy_execute_policy', 'GRANT', '##MS_PolicyEventProcessingLogin##', 'dbo', 'S')
        ,('fn_cColvEntries_80', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_check_parameters', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_decrement_lsn', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_get_column_ordinal', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_get_max_lsn', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_get_min_lsn', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_has_column_changed', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_hexstrtobin', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_increment_lsn', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_is_bit_set', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_map_lsn_to_time', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_cdc_map_time_to_lsn', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_fIsColTracked', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_GetCurrentPrincipal', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_GetRowsetIdFromRowDump', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_hadr_backup_is_preferred_replica', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_hadr_is_primary_replica', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_hadr_is_same_replica', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_IsBitSetInBitmask', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_isrolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MapSchemaType', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSdayasnumber', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSgeneration_downloadonly', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSget_dynamic_filter_login', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSorbitmaps', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSrepl_map_resolver_clsid', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MStestbit', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_MSvector_downloadonly', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_numberOf1InBinaryAfterLoc', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_numberOf1InVarBinary', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_PhysLocFormatter', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_repl_hash_binary', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_repladjustcolumnmap', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_repldecryptver4', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replformatdatetime', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replgetparsedddlcmd', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replp2pversiontotranid', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replreplacesinglequote', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replreplacesinglequoteplusprotectstring', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_repluniquename', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_replvarbintoint', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sqlvarbasetostr', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sysdac_get_currentusername', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sysdac_get_username', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sysdac_is_currentuser_sa', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sysdac_is_dac_creator', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_sysdac_is_login_creator', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_syspolicy_is_automation_enabled', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_varbintohexstr', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_varbintohexsubstring', 'GRANT', 'public', 'dbo', 'R')
        ,('fn_yukonsecuritymodelrequired', 'GRANT', 'public', 'dbo', 'R')
        ,('GeographyCollectionAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeographyConvexHullAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeographyEnvelopeAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeographyUnionAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeometryCollectionAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeometryConvexHullAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeometryEnvelopeAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('GeometryUnionAggregate', 'GRANT', 'public', 'dbo', 'R')
        ,('ORMask', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_agent_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_log_shipping_alert_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_log_shipping_primary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_log_shipping_primary_secondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_log_shipping_secondary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_add_log_shipping_secondary_primary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addapprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addarticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddatatype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddatatypemapping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddistpublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddistributiondb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adddynamicsnapshot_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addextendedproperty', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_AddFunctionalUnitToComponent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addlinkedserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addlinkedsrvlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addlogreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergealternatepublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergefilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergelogsettings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergepartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergepullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergepushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addmessage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addpublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addpublication_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addpullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addpullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addpushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addqreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addqueued_artinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addremotelogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addrole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addrolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addscriptexec', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsrvrolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsubscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsubscriber_schedule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsynctriggers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addsynctriggerscore', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addtabletocontents', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addtype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_addumpdevice', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adduser', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_adjustpublisheridentityrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_altermessage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_approlepassword', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_article_validation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_articlecolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_articlefilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_articleview', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assemblies_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assemblies_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assemblies_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assembly_dependencies_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assembly_dependencies_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_assembly_dependencies_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_attach_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_attach_single_file_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_attachsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_audit_write', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_autostats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_availability_group_command_internal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_bcp_dbcmptlevel', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_begin_parallel_nested_tran', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_bindefault', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_bindrule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_bindsession', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_browsemergesnapshotfolder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_browsereplcmds', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_browsesnapshotfolder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_can_tlog_be_applied', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_catalogs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_catalogs_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_catalogs_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_catalogs_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_add_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_change_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_cleanup_change_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_dbsnapshotLSN', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_disable_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_disable_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_drop_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_enable_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_enable_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_generate_wrapper_function', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_get_captured_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_get_ddl_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_help_change_data_capture', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_help_jobs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_restoredb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_scan', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_start_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_stop_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_vupgrade', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cdc_vupgrade_databases', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_agent_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_log_shipping_primary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_log_shipping_secondary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_log_shipping_secondary_primary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_subscription_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_tracking_waitforchanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_change_users_login', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changearticlecolumndatatype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedbowner', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedistpublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedistributiondb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedistributor_password', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedistributor_property', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changedynamicsnapshot_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changelogreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergefilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergelogsettings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changemergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changeobjectowner', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changepublication_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changeqreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changereplicationserverpasswords', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changesubscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changesubscriber_schedule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changesubscriptiondtsinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_changesubstatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_constbytable_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_constbytable_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_constraints_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_constraints_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_dynamic_filters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_for_sync_trigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_join_filter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_log_shipping_monitor_alert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_publication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_subset_filter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_check_sync_trigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_checkinvalidivarticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_checkOraclepackageversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_clean_db_file_free_space', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_clean_db_free_space', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cleanmergelogfiles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cleanup_log_shipping_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cleanup_temporal_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cleanupdbreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_column_privileges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_column_privileges_ex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_column_privileges_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_column_privileges_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_column_privileges_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_100_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_90_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_90_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_90_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_ex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_ex_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_ex_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_managed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_columns_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_commit_parallel_nested_tran', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_configure', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_configure_peerconflictdetection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_constr_col_usage_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_constr_col_usage_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_control_dbmasterkey_password', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_control_plan_guide', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_copymergesnapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_copysnapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_copysubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_create_plan_guide', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_create_plan_guide_from_handle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_createmergepalrole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_createorphan', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_createstats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_createtranpalrole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursor_list', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorclose', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorexecute', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorfetch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursoropen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursoroption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorprepare', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorprepexec', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_cursorunprepare', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_databases', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_datatype_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_datatype_info_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_datatype_info_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_db_ebcdic277_2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_db_increased_partitions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_db_selective_xml_index', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_db_vardecimal_storage_format', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbcmptlevel', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbfixedrolepermission', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitoraddmonitoring', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorchangealert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorchangemonitoring', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitordropalert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitordropmonitoring', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorhelpalert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorhelpmonitoring', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorresults', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dbmmonitorupdate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_ddopen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_defaultdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_defaultlanguage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_backup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_backup_file_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_http_namespace_reservation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_log_shipping_alert_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_log_shipping_primary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_log_shipping_primary_secondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_log_shipping_secondary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_delete_log_shipping_secondary_primary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_deletemergeconflictrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_deletepeerrequesthistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_deletetracertokenhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_denylogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_depends', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_cursor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_cursor_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_cursor_tables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_first_result_set', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_parameter_encryption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_describe_undeclared_parameters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_detach_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_disableagentoffload', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_distcounters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_drop_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_drop_agent_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropanonymousagent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropanonymoussubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropapprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droparticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdatatypemapping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdevice', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdistpublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdistributiondb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropdynamicsnapshot_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropextendedproperty', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droplinkedsrvlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droplogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergealternatepublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergefilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergelogsettings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergepartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropmessage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droporphans', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droppublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droppublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droppullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropremotelogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropreplsymmetrickey', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droprolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropsrvrolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropsubscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_droptype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dropuser', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_dsninfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enable_heterogeneous_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enableagentoffload', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enum_oledb_providers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumcustomresolvers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumdsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumeratependingschemachanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumerrorlogs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumfullsubscribers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_enumoledbdatasources', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_estimate_data_compression_savings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_estimated_rowsize_reduction_for_vardecimal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_execute', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_execute_external_script', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_executesql', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_expired_subscription_cleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_filestream_force_garbage_collection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_filestream_recalculate_container_size', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_firstonly_bitmap', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fkeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_flush_commit_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_flush_commit_table_on_demand', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_flush_CT_internal_table_on_demand', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_flush_log', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_foreign_keys_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_foreign_keys_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_foreign_keys_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_foreign_keys_rowset3', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_foreignkeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_catalog', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_column', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_keymappings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_load_thesaurus_file', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_pendingchanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_recycle_crawl_log', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_semantic_register_language_statistics_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_semantic_unregister_language_statistics_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_service', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_fulltext_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_FuzzyLookupTableMaintenanceInstall', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_FuzzyLookupTableMaintenanceInvoke', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_FuzzyLookupTableMaintenanceUninstall', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_generate_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_generatefilters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_database_scoped_credential', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_distributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_job_status_mergesubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_mergepublishedarticleproperties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_Oracle_publisher_metadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_query_template', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_get_redirected_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getagentparameterlist', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getbindtoken', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getdefaultdatatypemapping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getmergedeletetype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getProcessorUsage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getpublisherlink', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getqueuedarticlesynctraninfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getqueuedrows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getschemalock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getsqlqueueversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getsubscription_status_hsnapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getsubscriptiondtspackagename', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_gettopologyinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_getVolumeFreeSpace', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_grant_publication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_grantdbaccess', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_grantlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_agent_default', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_agent_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_datatype_mapping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_catalog_components', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_catalogs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_catalogs_cursor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_columns_cursor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_system_components', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_tables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_fulltext_tables_cursor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_alert_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_monitor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_monitor_primary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_monitor_secondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_primary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_primary_secondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_secondary_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_log_shipping_secondary_primary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_peerconflictdetection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_publication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geography_histogram', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geography_index', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geography_index_xml', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geometry_histogram', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geometry_index', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_help_spatial_geometry_index_xml', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpallowmerge_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helparticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helparticlecolumns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helparticledts', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpconstraint', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdatatypemap', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdbfixedrole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdevice', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdistpublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdistributiondb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdistributor_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpdynamicsnapshot_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpextendedproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpfile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpfilegroup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpindex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helplanguage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helplinkedsrvlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helplogins', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helplogreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergealternatepublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergearticlecolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergearticleconflicts', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergeconflictrows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergedeleteconflictrows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergefilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergelogfiles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergelogfileswithdata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergelogsettings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergepartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpmergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpntgroup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppeerrequests', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppeerresponses', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppublication_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppublicationsync', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helppullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpqreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpremotelogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpreplfailovermode', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpreplicationdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpreplicationdboption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpreplicationoption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helprolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helprotect', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsort', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsrvrole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsrvrolemember', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpstats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsubscriberinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsubscription_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpsubscriptionerrors', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helptext', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helptracertokenhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helptracertokens', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helptrigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpuser', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_helpxactsetjob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_http_generate_wsdl_complex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_http_generate_wsdl_defaultcomplexorsimple', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_http_generate_wsdl_defaultsimpleorcomplex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_http_generate_wsdl_simple', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_identitycolumnforreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IH_LR_GetCacheData', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHadd_sync_command', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHarticlecolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHget_loopback_detection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHScriptIdxFile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHScriptSchFile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHValidateRowFilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_IHXactSetJob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexcolumns_managed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_100_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_90_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_90_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_90_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_managed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexes_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_indexoption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_invalidate_textptr', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_is_makegeneration_needed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_ivindexhasnullcols', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_kill_filestream_non_transacted_handles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_kill_oldest_transaction_on_secondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_lightweightmergemetadataretentioncleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_link_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_linkedservers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_linkedservers_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_linkedservers_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_lock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_logshippinginstallmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_lookupcustomresolver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mapdown_bitmap', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_markpendingschemachange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_marksubscriptionvalidation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_memory_optimized_cs_migration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergearticlecolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergecleanupmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergedummyupdate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergemetadataretentioncleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergesubscription_cleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_mergesubscriptionsummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_migrate_user_to_contained', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MS_replication_installed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSacquireHeadofQueueLock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSacquireserverresourcefordynamicsnapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSacquireSlotLock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSacquiresnapshotdeliverysessionlock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSactivate_auto_sub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSactivatelogbasedarticleobject', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSactivateprocedureexecutionarticleobject', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_anonymous_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_article', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_compensating_cmd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_distribution_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_distribution_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_dynamic_snapshot_location', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_filteringcolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_log_shipping_error_detail', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_log_shipping_history_detail', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_logreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_logreader_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_merge_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_merge_anonymous_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_merge_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_merge_history90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_merge_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_mergereplcommand', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_mergesubentry_indistdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_qreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_qreader_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_repl_alert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_repl_command', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_repl_commands27hp', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_repl_error', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_replcmds_mcit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_replmergealert', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_snapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_snapshot_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_subscriber_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_subscriber_schedule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_subscription_3rd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_tracer_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadd_tracer_token', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddanonymousreplica', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadddynamicsnapshotjobatdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddguidcolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddguidindex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddinitialarticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddinitialpublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddinitialschemaarticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddinitialsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddlightweightmergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddmergedynamicsnapshotjob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddmergetriggers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddmergetriggers_from_template', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddmergetriggers_internal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddpeerlsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSaddsubscriptionarticles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSadjust_pub_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSagent_retry_stethoscope', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSagent_stethoscope', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSallocate_new_identity_range', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSalreadyhavegeneration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSanonymous_status', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSarticlecleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSbrowsesnapshotfolder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScache_agent_parameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScdc_capture_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScdc_cleanup_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScdc_db_ddl_event', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScdc_ddl_event', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScdc_logddl', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_article', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_distribution_agent_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_logreader_agent_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_merge_agent_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_mergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_mergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_originatorid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_priority', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_retention', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_retention_period_unit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_snapshot_agent_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchange_subscription_dts_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchangearticleresolver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchangedynamicsnapshotjobatdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchangedynsnaplocationatdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchangeobjectowner', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_agent_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_dropobject', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_Jet_Subscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_logicalrecord_metadatamatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_merge_subscription_count', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_pub_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_pull_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_snapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_subscription_expiry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_subscription_partition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheck_tran_retention', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheckexistsgeneration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheckexistsrecguid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheckfailedprevioussync', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheckidentityrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScheckIsPubOfSub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchecksharedagentforpublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSchecksnapshotstatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanup_agent_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanup_conflict', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanup_publication_ADinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanup_subscription_distside_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanupdynamicsnapshotfolder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanupdynsnapshotvws', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSCleanupForPullReinit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScleanupmergepublisher_internal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSclear_dynamic_snapshot_location', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSclearresetpartialsnapshotprogressbit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScomputelastsentgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScomputemergearticlescreationorder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScomputemergeunresolvedrefs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSconflicttableexists', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_all_article_repl_views', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_article_repl_views', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_dist_tables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_logical_record_views', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_sub_tables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreate_tempgenhistorytable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatedisabledmltrigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatedummygeneration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreateglobalreplica', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatelightweightinsertproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatelightweightmultipurposeproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatelightweightprocstriggersconstraints', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatelightweightupdateproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreatemergedynamicsnapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MScreateretry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdbuseraccess', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdbuserpriv', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdefer_check', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelete_tracer_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdeletefoldercontents', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdeletemetadataactionrequest', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdeletepeerconflictrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdeleteretry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdeletetranconflictrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelgenzero', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelrowsbatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelrowsbatch_downloadonly', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelsubrows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdelsubrowsbatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdependencies', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdetect_nonlogged_shutdown', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdetectinvalidpeerconfiguration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdetectinvalidpeersubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdist_activate_auto_sub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdist_adjust_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdistpublisher_cleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdistribution_counters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdistributoravailable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdodatabasesnapshotinitiation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdopartialdatabasesnapshotinitiation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_6x_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_6x_replication_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_anonymous_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_article', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_distribution_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_distribution_agentid_dbowner_proxy', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_dynamic_snapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_logreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_merge_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_merge_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_qreader_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_snapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_snapshot_dirs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_subscriber_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_subscription_3rd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdrop_tempgenhistorytable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdroparticleconstraints', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdroparticletombstones', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropconstraints', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropdynsnapshotvws', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropfkreferencingarticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropmergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropmergedynamicsnapshotjob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropobsoletearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdropretry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdroptemptable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdummyupdate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdummyupdate_logicalrecord', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdummyupdate90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdummyupdatelightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSdynamicsnapshotjobexistsatdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenable_publication_for_het_sub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSensure_single_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_distribution', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_distribution_s', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_distribution_sd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_logicalrecord_changes', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_logreader', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_logreader_s', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_logreader_sd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_agent_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_s', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_sd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_subscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_subscriptions_90_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_merge_subscriptions_90_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_metadataaction_requests', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_qreader', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_qreader_s', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_qreader_sd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_replication_agents', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_replication_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_replqueues', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_replsqlqueues', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_snapshot_s', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_snapshot_sd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenum_subscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumallpublications', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumallsubscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumarticleslightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumchanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumchanges_belongtopartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumchanges_notbelongtopartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumchangesdirect', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumchangeslightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumcolumns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumcolumnslightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumdeletes_forpartition', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumdeleteslightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumdeletesmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumdistributionagentproperties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumerate_PAL', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumgenerations', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumgenerations90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumpartialchanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumpartialchangesdirect', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumpartialdeletes', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumpubreferences', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumreplicas', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumreplicas90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumretries', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumschemachange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumsubscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSenumthirdpartypublicationvendornames', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSestimatemergesnapshotworkload', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSestimatesnapshotworkload', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSevalsubscriberinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSevaluate_change_membership_for_all_articles_in_pubid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSevaluate_change_membership_for_pubid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSevaluate_change_membership_for_row', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSexecwithlsnoutput', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfast_delete_trans', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfetchAdjustidentityrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfetchidentityrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfillupmissingcols', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfilterclause', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfix_6x_tasks', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfixlineageversions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSFixSubColumnBitmaps', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSfixupbeforeimagetables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSflush_access_cache', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSforce_drop_distribution_jobs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSforcereenumeration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSforeach_worker', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSforeachdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSforeachtable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgenerateexpandproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_agent_names', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_attach_state', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_DDL_after_regular_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_dynamic_snapshot_location', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_identity_range_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_jobstate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_last_transaction', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_latest_peerlsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_load_hint', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_log_shipping_new_sessionid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_logicalrecord_lineage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_max_used_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_min_seqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_MSmerge_rowtrack_colinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_new_xact_seqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_oledbinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_partitionid_eval_proc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_publication_from_taskname', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_publisher_rpc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_repl_cmds_anonymous', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_repl_commands', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_repl_error', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_session_statistics', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_shared_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_snapshot_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_subscriber_partition_id', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_subscription_dts_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_subscription_guid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_synctran_commands', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSget_type_wrapper', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetagentoffloadinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetalternaterecgens', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetarticlereinitvalue', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetchangecount', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetconflictinsertproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetconflicttablename', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSGetCurrentPrincipal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetdatametadatabatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetdbversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetdynamicsnapshotapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetdynsnapvalidationtoken', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetgenstatus4rows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetisvalidwindowsloginfromdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetlastrecgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetlastsentgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetlastsentrecgens', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetlastupdatedtime', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetlightweightmetadatabatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmakegenerationapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmakegenerationapplock_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmaxbcpgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmaxsnapshottimestamp', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmergeadminapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmetadata_changedlogicalrecordmembers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmetadatabatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmetadatabatch90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetmetadatabatch90new', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetonerow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetonerowlightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetpeerconflictrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetpeerlsns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetpeertopeercommands', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetpeerwinnerrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetpubinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetreplicainfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetreplicastate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetrowmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetrowmetadatalightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSGetServerProperties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetsetupbelong_cost', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetsubscriberinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetsupportabilitysettings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgettrancftsrcrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgettranconflictrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgetversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSgrantconnectreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShaschangeslightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShasdbaccess', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_article', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_distdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_distribution_agentid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_identity_property', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_logreader_agentid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_merge_agentid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_profilecache', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_publication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_repl_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_replication_status', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_replication_table', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_snapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_snapshot_agentid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_subscriber_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelp_subscription_status', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpcolumns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpconflictpublications', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpcreatebeforetable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpdestowner', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpdynamicsnapshotjobatdistributor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpfulltextindex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpfulltextscript', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpindex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelplogreader_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpmergearticles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpmergeconflictcounts', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpmergedynamicsnapshotjob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpmergeidentity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpmergeschemaarticles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpobjectpublications', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpreplicationtriggers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpsnapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpsummarypublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelptracertokenhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelptracertokens', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelptranconflictcounts', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelptype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MShelpvalidationdate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSIfExistsSubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSindexspace', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinit_publication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinit_subscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinitdynamicsubscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsert_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsertdeleteconflict', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinserterrorlineage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsertgenerationschemachanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsertgenhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsertlightweightschemachange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinsertschemachange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSinvalidate_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSisnonpkukupdateinconflict', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSispeertopeeragent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSispkupdateinconflict', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSispublicationqueued', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSisreplmergeagent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSissnapshotitemapplied', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSkilldb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSlock_auto_sub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSlock_distribution_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSlocktable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSloginmappings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakearticleprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakebatchinsertproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakebatchupdateproc', 'GRANT', 'public', 'dbo', 'R')
        
INSERT INTO #entries_to_exclude_va2033 (object_name, state_desc, prin_name, user_name, prin_type)
    VALUES ('sp_MSmakeconflictinsertproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakectsview', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakedeleteproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakedynsnapshotvws', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakeexpandproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakegeneration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakeinsertproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakemetadataselectproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakeselectproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakesystableviews', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmakeupdateproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmap_partitionid_to_generations', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmarkreinit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmatchkey', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_alterschemaonly', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_altertrigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_alterview', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_ddldispatcher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_getgencount', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_getgencur_public', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_is_snapshot_required', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_log_identity_range_allocations', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_parsegenlist', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmerge_upgrade_subscriber', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmergesubscribedb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSmergeupdatelastsyncinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSneedmergemetadataretentioncleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSNonSQLDDL', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSNonSQLDDLForSchemaDDL', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSobjectprivs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerapplyresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerapplytopologyinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerconflictdetection_statuscollection_applyresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerconflictdetection_statuscollection_sendresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerconflictdetection_topology_applyresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeerdbinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeersendresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeersendtopologyinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpeertopeerfwdingexec', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpost_auto_proc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpostapplyscript_forsubscriberprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSprep_exclusive', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSprepare_mergearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSprofile_in_use', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSproxiedmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSproxiedmetadatabatch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSproxiedmetadatalightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpub_adjust_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpublication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpublicationcleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSpublicationview', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSquery_syncstates', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSquerysubtype', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrecordsnapshotdeliveryprogress', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreenable_check', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrefresh_anonymous', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrefresh_publisher_idrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSregenerate_mergetriggersprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSregisterdynsnapseqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSregistermergesnappubid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSregistersubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreinit_failed_subscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreinit_hub', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreinit_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreinitoverlappingmergepublications', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreleasedynamicsnapshotapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreleasemakegenerationapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreleasemergeadminapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreleaseSlotLock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreleasesnapshotdeliverysessionlock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSremove_mergereplcommand', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSremoveoffloadparameter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_agentstatussummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_backup_complete', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_backup_start', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_check_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_createdatatypemappings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_distributionagentstatussummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_dropdatatypemappings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_enumarticlecolumninfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_enumpublications', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_enumpublishertables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_enumsubscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_enumtablecolumninfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_FixPALRole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_getdistributorinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_getpkfkrelation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_gettype_mappings', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_helparticlermo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_init_backup_lsns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_isdbowner', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_IsLastPubInSharedSubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_IsUserInAnyPAL', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_linkedservers_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_mergeagentstatussummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_PAL_rolecheck', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_raiserror', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_schema', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_setNFR', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_snapshot_helparticlecolumns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_snapshot_helppublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_startup_internal', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_subscription_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_testadminconnection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrepl_testconnection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplagentjobexists', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcheck_permission', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcheck_pull', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcheck_subscribe', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcheck_subscribe_withddladmin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcheckoffloadserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplcopyscriptfile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplraiserror', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplremoveuncdir', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreplupdateschema', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrequestreenumeration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrequestreenumeration_lightweight', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_attach_state', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_queued_reinit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_subscription_seqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_synctran_bit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSreset_transaction', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSresetsnapshotdeliveryprogress', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSrestoresavedforeignkeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSretrieve_publication_attributes', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_article_view', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_dri', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_pub_upd_trig', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_del_proc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_del_trig', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_ins_proc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_ins_trig', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_upd_proc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscript_sync_upd_trig', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptcustomdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptcustominsproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptcustomupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptdatabase', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptdb_worker', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptforeignkeyrestore', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptsubscriberprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSscriptviewproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsendtosqlqueue', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_dynamic_filter_options', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_logicalrecord_metadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_new_identity_range', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_oledb_prop', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_snapshot_xact_seqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_sub_guid', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSset_subscription_properties', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetaccesslist', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetartprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetbit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetconflictscript', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetconflicttable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetcontext_bypasswholeddleventbit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetcontext_replagent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetgentozero', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetlastrecgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetlastsentgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetreplicainfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetreplicaschemaversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetreplicastatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetrowmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetsubscriberinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetup_identity_range', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetup_partition_groups', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetup_use_partition_groups', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetupbelongs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetupnosyncsubwithlsnatdist', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetupnosyncsubwithlsnatdist_cleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsetupnosyncsubwithlsnatdist_helper', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSharedFixedDisk', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSQLDMO70_version', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSQLDMO80_version', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSQLDMO90_version', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSQLOLE_version', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSSQLOLE65_version', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstartdistribution_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstartmerge_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstartsnapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstopdistribution_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstopmerge_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSstopsnapshot_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsub_check_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsub_set_identity', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsubscription_status', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSsubscriptionvalidated', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStablechecks', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStablekeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStablerefs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStablespace', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStestbit', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStran_ddlrepl', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStran_is_snapshot_required', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MStrypurgingoldsnapshotdeliveryprogress', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSuniquename', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSunmarkifneeded', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSunmarkreplinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSunmarkschemaobject', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSunregistersubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_agenttype_default', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_singlelogicalrecordmetadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_subscriber_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_subscriber_schedule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_subscriber_tracer_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdate_tracer_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdatecachedpeerlsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdategenerations_afterbcp', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdategenhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdateinitiallightweightsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdatelastsyncinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdatepeerlsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdaterecgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdatereplicastate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSupdatesysmergearticles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSuplineageversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSuploadsupportabilitydata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSuselightweightreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSvalidate_dest_recgen', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSvalidate_subscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSvalidate_wellpartitioned_articles', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSvalidatearticle', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_MSwritemergeperfcounter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_new_parallel_nested_tran_id', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_objectfilegroup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledb_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledb_defdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledb_deflang', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledb_language', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledb_ro_usrname', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_oledbinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_ORbitmap', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_password', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_peerconflictdetection_tableaug', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_pkeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_polybase_join_group', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_polybase_leave_group', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_posttracertoken', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_prepare', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_prepexec', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_prepexecrpc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_primary_keys_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_primary_keys_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_primary_keys_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_primarykeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_100_managed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_100_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_90_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_90_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_managed', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedure_params_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedures_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procedures_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_processlogshippingmonitorhistory', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_processlogshippingmonitorprimary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_processlogshippingmonitorsecondary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_processlogshippingretentioncleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_procoption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_prop_oledb_provider', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_provider_types_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_provider_types_90_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_provider_types_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_publication_validation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_publicationsummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_publishdb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_publisherproperty', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_flush_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_force_plan', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_remove_plan', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_remove_query', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_reset_exec_stats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_query_store_unforce_plan', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_deauthorize_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_get_rpo_duration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_reauthorize_db', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_reconcile_batch', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_reconcile_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_reconcile_indexes', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_set_query_mode', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_set_rpo_duration', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rda_test_connection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_readerrorlog', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_recompile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_redirect_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refresh_heterogeneous_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refresh_log_shipping_monitor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refresh_parameter_encryption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refreshsqlmodule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refreshsubscriptions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_refreshview', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_register_custom_scripting', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_registercustomresolver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reinitmergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reinitmergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reinitpullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reinitsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_releaseapplock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_releaseschemalock', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_remote_data_archive_event', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_remoteoption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_removedbreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_removedistpublisherdbreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_removesrvreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rename', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_renamedb', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repl_generate_subscriber_event', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repl_generateevent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repladdcolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replcleanupccsprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replcmds', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replcounters', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replddlparser', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repldeletequeuedtran', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repldone', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repldropcolumn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replflush', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replgetparsedddlcmd', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replhelp', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replica', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replication_agent_checkup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replicationdboption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replincrementlsn', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorchangepublicationthreshold', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelpmergesession', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelpmergesessiondetail', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelpmergesubscriptionmoreinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelppublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelppublicationthresholds', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelppublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorhelpsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorrefreshjob', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replmonitorsubscriptionpendingcmds', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replpostsyncstatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replqueuemonitor', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replrestart', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replrethrow', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replsendtoqueue', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replsetoriginator', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replsetsyncstatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replshowcmds', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replsqlqgetrows', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replsync', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_repltrans', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_replwritetovarbin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_requestpeerresponse', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_requestpeertopologyinfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reserve_http_namespace', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reset_connection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_reset_session_context', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resetsnapshotdeliveryprogress', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resign_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_restoredbreplication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_restoremergeidentityrange', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resyncexecute', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resyncexecutesql', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resyncmergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resyncprepare', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_resyncuniquetable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_revoke_publication_access', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_revokedbaccess', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_revokelogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_rollback_parallel_nested_tran', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_schemafilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_schemata_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_reconciliation_delproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_reconciliation_insproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_reconciliation_sinsproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_reconciliation_vdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_reconciliation_xdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_script_synctran_commands', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptdynamicupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptinsproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptmappedupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptpublicationcustomprocs', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptsinsproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptsubconflicttable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptsupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptvdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptvupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptxdelproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_scriptxupdproc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sequence_get_range', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_server_diagnostics', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_server_info', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_serveroption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_set_session_context', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setapprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_SetAutoSAPasswordAndDisable', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setdefaultdatatypemapping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setnetname', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_SetOBDCertificate', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setOraclepackageversion', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setreplfailovermode', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setsubscriptionxactseqno', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_settriggerorder', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_setuserbylogin', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_showcolv', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_showlineage', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_showmemo_xml', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_showpendingchanges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_showrowreplicainfo', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sm_detach', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_spaceused', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_spaceused_remote_data_archive', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sparse_columns_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_special_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_special_columns_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_special_columns_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sproc_columns', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sproc_columns_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sproc_columns_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_add_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_add_jobstep', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_delete_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_help_jobstep', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_log_job_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_start_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_stop_job', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_verify_database_context', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlagent_write_jobstep_log', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sqlexec', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_srvrolepermission', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_start_user_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_startmergepullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_startmergepushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_startpublication_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_startpullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_startpushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_statistics', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_statistics_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_statistics_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_statistics_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stopmergepullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stopmergepushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stoppublication_snapshot', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stoppullsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stoppushsubscription_agent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_stored_procedures', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_subscribe', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_subscription_cleanup', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_subscriptionsummary', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_add_history_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_add_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_delete_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_delete_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_drop_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_ensure_dac_creator', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_rename_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_resolve_pending_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_rollback_all_pending_objects', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_rollback_committed_step', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_rollback_pending_object', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_setreadonly_database', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_update_history_entry', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_upgrade_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_syspolicy_subscribe_to_policy_category', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_syspolicy_unsubscribe_from_policy_category', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_syspolicy_update_ddl_trigger', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_syspolicy_update_event_notification', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_sysdac_update_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_constraints_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_constraints_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_privileges', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_privileges_ex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_privileges_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_privileges_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_privileges_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_statistics_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_statistics2_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_type_columns_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_type_columns_100_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_type_pkeys', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_type_primary_keys_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_types', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_types_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_table_validation', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tablecollations', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tablecollations_100', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tablecollations_90', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tableoption', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_ex', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_90_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_90_rowset_64', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_90_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_90_rowset2_64', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_rowset_64', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_info_rowset2_64', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tables_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_tableswc', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_testlinkedserver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_create', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_generateevent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_getdata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_setevent', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_setfilter', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_trace_setstatus', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_try_set_session_context', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unbindefault', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unbindrule', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unprepare', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unregister_custom_scripting', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unregistercustomresolver', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unsetapprole', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_unsubscribe', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_update_agent_profile', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_update_user_instance', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_updateextendedproperty', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_updatestats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_upgrade_log_shipping', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter1', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter10', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter3', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter4', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter5', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter6', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter7', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter8', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_user_counter9', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_usertypes_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_usertypes_rowset_rmt', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_usertypes_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validate_redirected_publisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validate_replica_hosts_as_publishers', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validatecache', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validatelogins', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validatemergepublication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validatemergepullsubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validatemergesubscription', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validlang', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_validname', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_verifypublisher', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_views_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_views_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_vupgrade_mergeobjects', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_vupgrade_mergetables', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_vupgrade_replication', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_vupgrade_replsecurity_metadata', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_who', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_who2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xml_preparedocument', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xml_removedocument', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xml_schema_rowset', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xml_schema_rowset2', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_bind_db_resource_pool', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_checkpoint_force_garbage_collection', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_control_proc_exec_stats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_control_query_exec_stats', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_flush_temporal_history', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_kill_active_transactions', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_merge_checkpoint_files', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_objects_present', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_set_memory_quota', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_slo_can_downgrade', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_slo_downgrade_finished', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_slo_prepare_to_downgrade', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_xtp_unbind_db_resource_pool', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_dirtree', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_fileexist', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_fixeddrives', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_getnetname', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_grantlogin', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_instance_regread', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_msver', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_qv', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_regread', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_repl_convert_encrypt_sysadmin_wrapper', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_replposteor', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_revokelogin', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_sprintf', 'GRANT', 'public', 'dbo', 'R')
        ,('xp_sscanf', 'GRANT', 'public', 'dbo', 'R')
        ,('sp_send_dbmail', 'GRANT', 'DatabaseMailUserRole', 'dbo', 'R')
        ,('sysmail_delete_mailitems_sp', 'GRANT', 'DatabaseMailUserRole', 'dbo', 'R')
        ,('sysmail_help_status_sp', 'GRANT', 'DatabaseMailUserRole', 'dbo', 'R')
        ,('sp_ssis_addfolder', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_addlogentry', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_checkexists', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_deletefolder', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_deletepackage', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_getfolder', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_getpackage', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_getpackageroles', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_listfolders', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_listpackages', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_putpackage', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_renamefolder', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_setpackageroles', 'GRANT', 'db_ssisadmin', 'dbo', 'R')
        ,('sp_ssis_addfolder', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_addlogentry', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_checkexists', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_deletefolder', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_deletepackage', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_getfolder', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_getpackage', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_getpackageroles', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_listfolders', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_listpackages', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_putpackage', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_renamefolder', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_setpackageroles', 'GRANT', 'db_ssisltduser', 'dbo', 'R')
        ,('sp_ssis_checkexists', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_deletepackage', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_getfolder', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_getpackage', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_listfolders', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_listpackages', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('sp_ssis_putpackage', 'GRANT', 'db_ssisoperator', 'dbo', 'R')
        ,('fn_syscollector_highest_incompatible_mdw_version', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_cleanup_collector', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_create_collection_item', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_create_collection_set', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_create_collector_type', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_delete_collection_item', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_delete_collection_set', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_delete_collector_type', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_set_cache_directory', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_set_cache_window', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_set_warehouse_database_name', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('sp_syscollector_set_warehouse_instance_name', 'GRANT', 'dc_admin', 'dbo', 'R')
        ,('fn_syscollector_find_collection_set_root', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_create_tsql_query_collector', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_delete_execution_log_tree', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_disable_collector', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_enable_collector', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_get_tsql_query_collector_package_ids', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_run_collection_set', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_start_collection_set', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_stop_collection_set', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_update_collection_item', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_update_collection_set', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_syscollector_upload_collection_set', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('sp_verify_subsystems', 'GRANT', 'dc_operator', 'dbo', 'R')
        ,('fn_syscollector_highest_incompatible_mdw_version', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_create_tsql_query_collector', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_oncollectionbegin', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_oncollectionend', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_onerror', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_onpackagebegin', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_onpackageend', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_onpackageupdate', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_event_onstatsupdate', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_get_tsql_query_collector_package_ids', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_get_warehouse_connection_string', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_snapshot_dm_exec_query_stats', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syscollector_snapshot_dm_exec_requests', 'GRANT', 'dc_proxy', 'dbo', 'R')
        ,('sp_syspolicy_add_condition', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_object_set', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_policy', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_policy_category', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_policy_category_subscription', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_target_set', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_add_target_set_level', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_configure', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_create_purge_job', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_delete_condition', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_delete_object_set', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_delete_policy', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_delete_policy_category', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_delete_policy_category_subscription', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_dispatch_event', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_log_policy_execution_detail', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_log_policy_execution_end', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_log_policy_execution_start', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_purge_health_state', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_purge_history', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_rename_condition', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_rename_policy', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_rename_policy_category', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_repair_policy_automation', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_set_config_enabled', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_set_config_history_retention', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_set_log_on_success', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_condition', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_policy', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_policy_category', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_policy_category_subscription', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_target_set', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_update_target_set_level', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_syspolicy_verify_object_set_identifiers', 'GRANT', 'PolicyAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_add_shared_registered_server', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_add_shared_server_group', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_delete_shared_registered_server', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_delete_shared_server_group', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_move_shared_registered_server', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_move_shared_server_group', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_rename_shared_registered_server', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_rename_shared_server_group', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_update_shared_registered_server', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_sysmanagement_update_shared_server_group', 'GRANT', 'ServerGroupAdministratorRole', 'dbo', 'R')
        ,('sp_enum_login_for_proxy', 'GRANT', 'SQLAgentOperatorRole', 'dbo', 'R')
        ,('sp_help_alert', 'GRANT', 'SQLAgentOperatorRole', 'dbo', 'R')
        ,('sp_help_notification', 'GRANT', 'SQLAgentOperatorRole', 'dbo', 'R')
        ,('sp_help_targetserver', 'GRANT', 'SQLAgentOperatorRole', 'dbo', 'R')
        ,('sp_purge_jobhistory', 'GRANT', 'SQLAgentOperatorRole', 'dbo', 'R')
        ,('sp_add_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_add_jobschedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_add_jobserver', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_add_jobstep', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_add_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_addtask', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_agent_get_jobstep', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_attach_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_check_for_owned_jobs', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_check_for_owned_jobsteps', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_jobschedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_jobserver', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_jobstep', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_jobsteplog', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_delete_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_detach_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_droptask', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_enum_sqlagent_subsystems', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_get_job_alerts', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_get_jobstep_db_username', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_get_sqlagent_properties', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_category', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobactivity', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobcount', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobhistory', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobhistory_full', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobhistory_sem', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobhistory_summary', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobs_in_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobschedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobserver', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobstep', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_jobsteplog', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_operator', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_proxy', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_help_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_maintplan_subplans_by_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_notify_operator', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_start_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_stop_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_uniquetaskname', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_update_job', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_update_jobschedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_update_jobstep', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_update_schedule', 'GRANT', 'SQLAgentUserRole', 'dbo', 'R')
        ,('sp_agent_get_jobstep', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_downloaded_row_limiter', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_enlist_tsx', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_help_jobschedule', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_help_jobstep', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_maintplan_subplans_by_job', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_sqlagent_check_msx_version', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_sqlagent_probe_msx', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('sp_sqlagent_refresh_job', 'GRANT', 'TargetServersRole', 'dbo', 'R')
        ,('fn_encode_sqlname_for_powershell', 'GRANT', 'UtilityCMRReader', 'dbo', 'R')
        ,('fn_sysutility_get_is_instance_ucp', 'GRANT', 'UtilityCMRReader', 'dbo', 'R')
        ,('fn_sysutility_ucp_get_aggregated_failure_count', 'GRANT', 'UtilityCMRReader', 'dbo', 'R')
        ,('fn_sysutility_ucp_get_applicable_policy', 'GRANT', 'UtilityCMRReader', 'dbo', 'R')
        ,('fn_sysutility_ucp_get_global_health_policy', 'GRANT', 'UtilityCMRReader', 'dbo', 'R')
        ,('fn_sysutility_get_culture_invariant_conversion_style_internal', 'GRANT', 'UtilityIMRReader', 'dbo', 'R')
        ,('fn_sysutility_mi_get_cpu_architecture_name', 'GRANT', 'UtilityIMRReader', 'dbo', 'R')
        ,('fn_sysutility_mi_get_cpu_family_name', 'GRANT', 'UtilityIMRReader', 'dbo', 'R')
        ,('sp_sysutility_mi_collect_dac_execution_statistics_internal', 'GRANT', 'UtilityIMRWriter', 'dbo', 'R')
        ,('sp_sysutility_mi_get_dac_execution_statistics_internal', 'GRANT', 'UtilityIMRWriter', 'dbo', 'R')

SELECT [Permission_Class]
        ,[Schema]
        ,rules.object_name AS [Object]
        ,[Permission]
        ,[Principal_Type]
        ,rules.prin_name AS [Principal]
FROM (
    SELECT  perms.class_desc COLLATE DATABASE_DEFAULT AS [Permission_Class]
            ,object_schema_name(major_id) COLLATE DATABASE_DEFAULT AS [Schema]
            ,object_name(major_id) COLLATE DATABASE_DEFAULT AS object_name
            ,perms.permission_name COLLATE DATABASE_DEFAULT AS [Permission]
            ,type_desc COLLATE DATABASE_DEFAULT AS [Principal_Type]
            ,prin.name COLLATE DATABASE_DEFAULT AS prin_name
            ,state_desc COLLATE DATABASE_DEFAULT AS state_desc
            ,prin.type COLLATE DATABASE_DEFAULT AS prin_type
            ,user_name(grantor_principal_id) COLLATE DATABASE_DEFAULT AS user_name
        FROM sys.database_permissions AS perms
        INNER JOIN sys.database_principals AS prin
        ON perms.grantee_principal_id = prin.principal_id
            WHERE permission_name IN ('EXECUTE')
            AND perms.class = 1
            AND [state] IN ('G','W')
            AND grantee_principal_id NOT IN (DATABASE_PRINCIPAL_ID('guest') ,DATABASE_PRINCIPAL_ID('public'))) AS rules
LEFT JOIN #entries_to_exclude_va2033
    ON rules.object_name = #entries_to_exclude_va2033.object_name
    AND rules.state_desc = #entries_to_exclude_va2033.state_desc
    AND rules.prin_name = #entries_to_exclude_va2033.prin_name
    AND rules.user_name = #entries_to_exclude_va2033.user_name
    AND rules.prin_type = #entries_to_exclude_va2033.prin_type
    WHERE #entries_to_exclude_va2033.object_name IS NULL ",
                    Remediation = "Revoke permissions from principals where not needed. It is recommended to have at most 1 principal granted a specific permission.",
                    RemediationScript = "REVOKE $3 ON [$1].[$2] FROM [$5]"
                },
                new termFormat()
                {
                    ID = "VA2109",
                    Check = "Minimal set of principals should be members of fixed low impact database roles",
                    Category = "Authentication And Authorization",
                    Risk = "Low",
                    Des = "SQL Server provides roles to help manage the permissions. Roles are security principals that group other principals. Database-level roles are database-wide in their permission scope. This rule checks that a minimal set of principals are members of the fixed database roles.",
                    Impact = "Fixed database roles may have administrative permissions on the system. Following the principle of least privilege, it is important to minimize membership in fixed database roles and keep a baseline of these memberships. See https://docs.microsoft.com/en-us/sql/relational-databases/security/authentication-access/database-level-roles for additional information on database roles.",
                    Query = @"SELECT user_name(sr.member_principal_id) as [Principal]
    ,user_name(sr.role_principal_id) as [Role]
    ,type_desc as [Principal Type]
FROM sys.database_role_members AS sr
INNER JOIN sys.database_principals AS sp ON sp.principal_id = sr.member_principal_id
WHERE sr.role_principal_id IN (
        user_id('db_datareader')
        ,user_id('db_datawriter')
        ,user_id('db_denydatareader')
        ,user_id('db_denydatawriter')
        )",
                    Remediation = "Remove members who should not have access to the database role.",
                    RemediationScript = "ALTER ROLE [$1] DROP MEMBER [$0]"
                },
                //new termFormat()
                //{
                //    ID = "VA2130",
                //    Check = "Track all users with access to the database",
                //    Category = "Authentication And Authorization",
                //    Risk = "Low",
                //    Des = "This check tracks all users with access to a database. Make sure that these users are authorized according to their current role in the organization.",
                //    Impact = "Performing a User Access Review helps identify accounts that were added to the server maliciously and dormant accounts.",
                //    Query = @"WITH UsersAndRoles (principal_name, sid, type) AS 
                //                (
                //                    SELECT DISTINCT prin.name, prin.sid, prin.type 
                //                    FROM sys.database_principals prin 
                //                        INNER JOIN ( SELECT *
                //                                     FROM sys.database_permissions
                //                                     WHERE type = 'CO' 
                //                                        AND state IN ('G', 'W')
                //                        ) perm 
                //                            ON perm.grantee_principal_id = prin.principal_id 
                //                        WHERE prin.type IN ('S', 'X', 'R', 'E', 'G')
                //                    UNION ALL
                //                    SELECT 
                //                        user_name(rls.member_principal_id), prin.sid, prin.type
                //                    FROM 
                //                        UsersAndRoles cte
                //                        INNER JOIN sys.database_role_members rls
                //                            ON user_name(rls.role_principal_id) = cte.principal_name
                //                        INNER JOIN sys.database_principals prin
                //                            ON rls.member_principal_id = prin.principal_id
                //                        WHERE cte.type = 'R'
                //                ),
                //                Users (database_user, sid) AS
                //                (
                //                    SELECT principal_name, sid
                //                    FROM UsersAndRoles
                //                    WHERE type IN ('S', 'X', 'E', 'G')
                //                        AND principal_name != 'dbo'
                //                )
                //                SELECT DISTINCT database_user, sid
                //                    FROM Users
                //                    WHERE sid != 0x01",
                //    Remediation = "Revoke unnecessary access granted to users. Add the rest to the baseline.",
                //    RemediationScript = "DROP USER $0"
                //},
                new termFormat()
                {
                    ID = "VA1098",
                    Check = "Any Existing Mirroring or SSB endpoint should require AES encryption",
                    Category = "Data Protection",
                    Risk = "High",
                    Des = "Mirroring endpoints, which are used for Always On Synchronization, as well as Service Broker endpoints support different encryption algorithms, including no encryption. This rule checks that any existing endpoint requires AES encryption.",
                    Impact = "Using a weak encryption algorithm or plaintext in communication protocols can lead to data manipulation including data loss, and/or connection hijacking.",
                    Query = @"SELECT ep.NAME      AS [Endpoint_Name],
        ep.type_desc AS [Endpoint_Type]
FROM   sys.database_mirroring_endpoints AS dme
JOIN   sys.endpoints AS ep
        ON dme.endpoint_id = ep.endpoint_id
WHERE  dme.encryption_algorithm <> 2
        AND ep.type BETWEEN 3 AND 4
UNION
SELECT ep.NAME      AS [Name],
        ep.type_desc AS [Type]
FROM   sys.service_broker_endpoints AS sbe
JOIN   sys.endpoints  AS ep 
        ON sbe.endpoint_id = ep.endpoint_id
WHERE  sbe.encryption_algorithm <> 2
        AND ep.type BETWEEN 3 AND 4",
                    Remediation = "Change the affected endpoints to accept only AES connections.",
                    RemediationScript = "ALTER ENDPOINT [$0] FOR $1 ( ENCRYPTION = REQUIRED ALGORITHM AES )"
                },
                new termFormat()
                {
                    ID = "VA1221",
                    Check = "Database Encryption Symmetric Keys should use AES algorithm",
                    Category = "Data Protection",
                    Risk = "High",
                    Des = "SQL Server uses encryption keys to help secure data, credentials, and connection information that is stored in a server database. SQL Server has two kinds of keys: symmetric and asymmetric. This rule checks that Database Encryption Symmetric Keys use AES algorithm.",
                    Impact = "Weak encryption algorithms may lead to weaknesses in the data-at-rest protection.",
                    Query = @"SELECT db_name(database_id) AS [Database]
    ,encryption_state AS [Encryption_State]
    ,key_algorithm AS [Key_Algorithm]
    ,key_length AS [Key_Length]
    ,encryptor_type AS [Encryptor_Type]
FROM sys.dm_database_encryption_keys
WHERE key_algorithm != 'AES'",
                    Remediation = "Regenerate the DEK using AES",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1222",
                    Check = "Cell-Level Encryption keys should use AES algorithm",
                    Category = "Data Protection",
                    Risk = "High",
                    Des = "Cell-Level Encryption (CLE) allows you to encrypt your data using symmetric and asymmetric keys. This rule checks that Cell-Level Encryption symmetric keys use AES algorithm.",
                    Impact = "Weak encryption algorithms may lead to weaknesses in the data-at-rest protection.",
                    Query = @"SELECT NAME AS [Key_Name],
        algorithm_desc AS [Key_Algorithm]
FROM   sys.symmetric_keys
WHERE  key_algorithm NOT IN ( 'A1', 'A2', 'A3' )",
                    Remediation = "Create AES keys, re-encrypt the data using the new key, and drop the affected keys.",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1223",
                    Check = "Certificate keys should use at least 2048 bits",
                    Category = "Data Protection",
                    Risk = "High",
                    Des = "Certificate keys are used in RSA and other encryption algorithms to protect data. These keys need to be of enough length to secure the user’s data. This rule checks that the key’s length is at least 2048 bits for all certificates.",
                    Impact = "Key length defines the upper-bound on the encryption algorithm’s security. Using short keys in encryption algorithms may lead to weaknesses in data-at-rest protection.",
                    Query = @"SELECT name AS [Certificate_Name], thumbprint AS [Thumbprint]
FROM sys.certificates
WHERE key_length < 2048",
                    Remediation = "Create new certificates, re-encrypt the data/sign-data using the new key, and drop the affected keys.",
                    RemediationScript = ""
                },
                //new termFormat()
                //{
                //    ID = "VA1224",
                //    Check = "Asymmetric keys’ length should be at least 2048 bits",
                //    Category = "Data Protection",
                //    Risk = "High",
                //    Des = "Database asymmetric keys are used in many encryption algorithms, these keys need to be of enough length to secure the encrypted data, this rule checks that all asymmetric keys stored in the database are of length of at least 2048 bits.",
                //    Impact = "Key length defines the upper-bound on the encryption algorithm’s security, using short keys in encryption algorithms may lead to weaknesses in the data-at-rest protection.",
                //    Query = @"SELECT name, pvt_key_encryption_type_desc, algorithm_desc
                //                FROM sys.asymmetric_keys
                //                WHERE key_length < 2048
                //                AND NOT (DB_NAME() = 'master' AND name = 'MS_SQLEnableSystemAssemblyLoadingKey')
                //                ORDER BY name, pvt_key_encryption_type_desc, algorithm_desc",
                //    Remediation = "Create new asymmetric Keys, re-encrypt the data/sign-data using the new key, and drop the affected keys.",
                //    RemediationScript = "DROP ASYMMETRIC KEY [$0]"
                //},
                new termFormat()
                {
                    ID = "VA1219",
                    Check = "Transparent data encryption should be enabled",
                    Category = "Data Protection",
                    Risk = "Medium",
                    Des = "Transparent data encryption (TDE) helps to protect the database files against information disclosure by performing real-time encryption and decryption of the database, associated backups, and transaction log files ‘at rest’, without requiring changes to the application. This rule checks that TDE is enabled on the database.",
                    Impact = "Transparent Data Encryption (TDE) protects data ‘at rest’, meaning the data and log files are encrypted when stored on disk.",
                    Query = @"SELECT CASE WHEN EXISTS
( SELECT *
    FROM sys.databases
    WHERE name = db_name()
    AND is_encrypted = 0)
THEN 1
ELSE 0
END AS [Violation]",
                    Remediation = "Enable TDE on the affected database. Please follow the instructions on https://docs.microsoft.com/en-us/sql/relational-databases/security/encryption/transparent-data-encryption.",
                    RemediationScript = ""
                },
                //new termFormat()
                //{
                //    ID = "VA1288",
                //    Check = "Sensitive data columns should be classified",
                //    Category = "Data Protection",
                //    Risk = "Medium",
                //    Des = "This rule checks if the scanned database has potentially sensitive data that has not been classified.",
                //    Impact = "The data residing in your database can have varying levels of business and privacy sensitivity. It is important to be aware of the location of your most sensitive data elements, so that their access can be monitored and tracked. SQL Data Discovery & Classification enables you to assign a distinct classification label to each database column and persist this information as column metadata within the database. This classification metadata can then be used for tracking and monitoring objectives. In addition, access to sensitive data should be more tightly controlled. Built-in SQL security capabilities like Always Encrypted, Dynamic Data Masking, and Row-Level Security can be used to control access and protect data.",
                //    Query = @"",
                //    Remediation = "Click the remediation link below to classify columns with sensitive data or to dismiss recommendations for columns that do not contain sensitive data (false positives).",
                //    RemediationScript = ""
                //},
                new termFormat()
                {
                    ID = "VA2128",
                    Check = "Vulnerability Assessment is not supported for SQL Server versions lower than SQL Server 2012",
                    Category = "Installation Updates And Patches",
                    Risk = "High",
                    Des = "To run a Vulnerability Assessment scan on your SQL Server, the server needs to be upgraded to SQL Server 2012 or higher.SQL Server 2008 R2 and below are no longer supported by Microsoft. See here: https://www.microsoft.com/en-us/cloud-platform/windows-sql-server-2008.",
                    Impact = "Older versions of SQL server are no longer supported by Microsoft. Windows Server 2008 R2 end-of-life mainstream support ended on January 13, 2015. On January 14, 2020, Microsoft will end all support for Windows Server 2008 R2.",
                    Query = @"SELECT CASE
    WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) LIKE '7.%' THEN '1'
    WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) LIKE '8.%' THEN '1'
    WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) LIKE '9.%' THEN '1'
    WHEN CONVERT(VARCHAR(128), SERVERPROPERTY ('productversion')) LIKE '10.%' THEN '1'
    ELSE '0'
END AS [Violation]",
                    Remediation = "Upgrade your SQL Server version to 2012 or higher.",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1102",
                    Check = "he Trustworthy bit should be disabled on all databases except MSDB",
                    Category = "Surface Area Reduction",
                    Risk = "High",
                    Des = "The TRUSTWORTHY database property is used to indicate whether the instance of SQL Server trusts the database and the contents within it. If this option is enabled, database modules (for example, user-defined functions or stored procedures) that use an impersonation context can access resources outside the database. This rule verifies that the TRUSTWORTHY bit is disabled on all databases, except MSDB.",
                    Impact = "The trustworthy bit (TWbit) is an access control mechanism that enables features that can lead to an elevation of privilege such as CLR and server-scope impersonation. For more information: http://support.microsoft.com/kb/2183687.",
                    Query = @"SELECT CASE
    WHEN EXISTS (SELECT *
                FROM   sys.databases
                WHERE  NAME = Db_name()
                        AND is_trustworthy_on = 1) THEN 1
    ELSE 0
END       AS Violation,
Db_name() AS [Database]",
                    Remediation = "Disable the trustworthy bit (TWbit) from all affected databases. If you need to use functionality that is controlled by the TWbit, it is recommended to use digital signatures to enable the functionality instead of enabling the TWbit on the database.",
                    RemediationScript = "ALTER DATABASE [$1] SET TRUSTWORTHY OFF"
                },
                new termFormat()
                {
                    ID = "VA1245",
                    Check = "The database owner information in the database should match the respective database owner information in the master database",
                    Category = "Surface Area Reduction",
                    Risk = "High",
                    Des = "Database ownership metadata is stored in two locations – in the master database and in the database itself. This stored metadata can sometimes become out of sync. For instance, when a database has been restored from a different server, or when the server principal stored as dbo no longer exists for some reason, the data stored in the database and the data stored in the master database will be out of sync.",
                    Impact = "The metadata about the database owner stored inside the database should match that stored in the master database. This helps avoid potential system problems, for instance permission problems when using some features such as CLR.",
                    Query = @"SELECT CASE
WHEN EXISTS (
        SELECT *
        FROM   sys.database_principals AS dbprs
        INNER JOIN sys.databases AS dbs
        ON  dbprs.sid != dbs.owner_sid
        WHERE dbs.database_id = Db_id()
        AND dbprs.principal_id = 1
        )
    THEN 1
ELSE 0
END AS [Violation]",
                    Remediation = "Use ALTER AUTHORIZATION ON DATABASE DDL-command against the database to specify a new server principal that should be the owner of the database",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1256",
                    Check = "User CLR assemblies should not be defined in the database",
                    Category = "Surface Area Reduction",
                    Risk = "High",
                    Des = "CLR assemblies can be used to execute arbitrary code on SQL Server process. This rule checks that there are no user-defined CLR assemblies in the database.",
                    Impact = "Using CLR assemblies can bring a security flaw to the SQL Server instance and to all other network resources accessible from it.",
                    Query = @"SELECT name AS [Assembly] FROM sys.assemblies WHERE is_user_defined != 0",
                    Remediation = "Drop assemblies from the affected databases",
                    RemediationScript = "DROP ASSEMBLY [$0]"
                },
                new termFormat()
                {
                    ID = "VA1277",
                    Check = "Polybase network encryption should be enabled",
                    Category = "Surface Area Reduction",
                    Risk = "High",
                    Des = "PolyBase is a technology that accesses and combines both non-relational and relational data, all from within SQL Server. Polybase network encryption option configures SQL Server to encrypt control and data channels when using Polybase. This rule verifies that this option is enabled.",
                    Impact = "Having any communication protocol without encryption can lead to multiple security problems, including data loss, data tampering & leak of authentication credentials.",
                    Query = @"SELECT CASE
    WHEN EXISTS (SELECT *
                FROM   sys.configurations
                WHERE  NAME = 'polybase network encryption'
                        AND Cast(value AS INT) = 0) THEN 1
    ELSE 0
END AS [Violation]",
                    Remediation = "Enable polybase network encryption (default).",
                    RemediationScript = @"EXECUTE sp_configure 'show advanced options', 1; RECONFIGURE;
                                            EXECUTE sp_configure 'polybase network encryption', 1; RECONFIGURE;
                                            EXECUTE sp_configure 'show advanced options', 0; RECONFIGURE;"
                },
                //new termFormat()
                //{
                //    ID = "VA2062",
                //    Check = "Database-level firewall rules should not grant excessive access",
                //    Category = "Surface Area Reduction",
                //    Risk = "High",
                //    Des = "The Azure SQL Database-level firewall helps protect your data by preventing all access to your database until you specify which IP addresses have permission. Database-level firewall rules grant access to the specific database based on the originating IP address of each request. Database-level firewall rules for master and user databases can only be created and managed through Transact-SQL (unlike server-level firewall rules which can also be created and managed using the Azure portal or PowerShell). For more details please see: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-firewall-configure This check verifies that each database-level firewall rule does not grant access to more than 255 IP addresses.",
                //    Impact = "Often, administrators add rules that grant excessive access as part of a troubleshooting process – to eliminate the firewall as the source of a problem, they simply create a rule that allows all traffic to pass to the affected database. Granting excessive access using database firewall rules is a clear security concern, as it violates the principle of least privilege by allowing unnecessary access to your database. In fact, it’s the equivalent of placing the database outside of the firewall.",
                //    Query = @"SELECT name
                //                    ,start_ip_address
                //                    ,end_ip_address
                //                FROM sys.database_firewall_rules
                //                WHERE ( 
                //                        (CONVERT(bigint, parsename(end_ip_address, 1)) +
                //                         CONVERT(bigint, parsename(end_ip_address, 2)) * 256 + 
                //                         CONVERT(bigint, parsename(end_ip_address, 3)) * 65536 + 
                //                         CONVERT(bigint, parsename(end_ip_address, 4)) * 16777216 ) 
                //                        - 
                //                        (CONVERT(bigint, parsename(start_ip_address, 1)) +
                //                         CONVERT(bigint, parsename(start_ip_address, 2)) * 256 + 
                //                         CONVERT(bigint, parsename(start_ip_address, 3)) * 65536 + 
                //                         CONVERT(bigint, parsename(start_ip_address, 4)) * 16777216 )
                //                      ) > 255;",
                //    Remediation = "Remove database firewall rules that grant excessive access.",
                //    RemediationScript = "EXECUTE sp_delete_database_firewall_rule N'$0';"
                //},
                //new termFormat()
                //{
                //    ID = "VA2064",
                //    Check = "Database-level firewall rules should be tracked and maintained at a strict minimum",
                //    Category = "Surface Area Reduction",
                //    Risk = "High",
                //    Des = "The Azure SQL Database-level firewall helps protect your data by preventing all access to your database until you specify which IP addresses have permission. Database-level firewall rules grant access to the specific database based on the originating IP address of each request. Database-level firewall rules for master and user databases can only be created and managed through Transact-SQL (unlike server-level firewall rules which can also be created and managed using the Azure portal or PowerShell). For more details please see: https://docs.microsoft.com/en-us/azure/sql-database/sql-database-firewall-configure. This check enumerates all the database-level firewall rules so that any changes made to them can be identified and addressed.",
                //    Impact = "Firewall rules should be strictly configured to allow access only to client computers that have a valid need to connect to the database. Any superfluous entries in the firewall may pose a threat by allowing an unauthorized source access to your database.",
                //    Query = @"SELECT name
                //                    ,start_ip_address
                //                    ,end_ip_address
                //                FROM sys.database_firewall_rules",
                //    Remediation = "Evaluate each of the database-level firewall rules. Remove any rules that grant unnecessary access and set the rest as a baseline. Deviations from the baseline will be identified and brought to your attention in subsequent scans.",
                //    RemediationScript = "EXECUTE sp_delete_database_firewall_rule N'$0';"
                //},
                new termFormat()
                {
                    ID = "VA1044",
                    Check = "Remote Admin Connections should be disabled unless specifically required",
                    Category = "Surface Area Reduction",
                    Risk = "Medium",
                    Des = "This rule checks that remote dedicated admin connections are disabled if they are not being used for clustering to reduce attack surface area. SQL Server provides a dedicated administrator connection (DAC). The DAC lets an administrator access a running server to execute diagnostic functions or Transact-SQL statements, or to troubleshoot problems on the server and it becomes an attractive target to attack when it is enabled remotely.",
                    Impact = "The Dedicated Admin Connection (DAC) is intended to be used by administrators for troubleshooting in scenarios when normal connections are not available due to an abnormal state of the system. For scenarios, other than clusters, the DAC is intended to be used only on the same node, and not remotely, to prevent automated attacks against this entry point.",
                    Query = @"SELECT CASE
WHEN EXISTS (SELECT *
    FROM   sys.configurations
    WHERE  NAME = 'remote admin connections'
        AND Cast(value AS INT) = 1
        AND ISNULL(SERVERPROPERTY('IsClustered'), 0) = 0) THEN 1
ELSE 0
END AS [Violation]",
                    Remediation = "Disable remote dedicated admin connections. A good alternative would be to access box directly and use DAC instead of RDAC.",
                    RemediationScript = @"EXECUTE sp_configure 'show advanced options', 1; RECONFIGURE;
                                            EXECUTE sp_configure 'remote admin connections', 0; RECONFIGURE;
                                            EXECUTE sp_configure 'show advanced options', 0; RECONFIGURE;"
                },
                new termFormat()
                {
                    ID = "VA1051",
                    Check = "AUTO_CLOSE should be disabled on all databases",
                    Category = "Surface Area Reduction",
                    Risk = "Medium",
                    Des = "The AUTO_CLOSE option specifies whether the database shuts down gracefully and frees resources after the last user disconnects. Regardless of its benefits it can cause denial of service by aggressively opening and closing the database, thus it is important to keep this feature disabled. This rule checks that this option is disabled on the current database.",
                    Impact = "Databases marked with AUTO_CLOSE allows the DB to be closed if there are no active connections. In the case of partially contained databases, the authentication of users occurs within the database itself, so the database must be opened every time to authenticate a user. Frequent opening/closing of the database consumes additional resources and may contribute to a denial of service attack.",
                    Query = @"SELECT CASE
WHEN EXISTS (SELECT *
FROM   sys.databases
WHERE  NAME = Db_name()
    AND is_auto_close_on = 1) THEN 1
ELSE 0
END       AS [Violation]
    , Db_name() AS [Database]",
                    Remediation = "Disable the AUTO_CLOSE option on the affected databases.",
                    RemediationScript = "ALTER DATABASE [$1] SET AUTO_CLOSE OFF"
                },
                new termFormat()
                {
                    ID = "VA1143",
                    Check = "‘dbo’ user should not be used for normal service operation",
                    Category = "Surface Area Reduction",
                    Risk = "Medium",
                    Des = "The ‘dbo’, or database owner, is a user account that has implied permissions to perform all activities in the database. Members of the sysadmin fixed server role are automatically mapped to dbo. This rule checks that dbo is not the only account allowed to access this database. Please note that on a newly created clean database this rule will fail until additional roles are created.",
                    Impact = "A compromised service that accesses the database with the ‘dbo’ user account will have full control of the database. To avoid this situation, lower privileged users should be defined for normal service operation, while the ‘dbo’ account should only be used for administrative tasks that require this privilege.",
                    Query = @"IF((SELECT count(*) from sys.database_principals  WHERE principal_id >= 5 AND principal_id < 16384 ) > 0) SELECT 0 AS [Violation]
ELSE SELECT 1 AS [Violation]",
                    Remediation = "Create users with low privileges to access the DB and any data stored in it with the appropriate set of permissions.",
                    RemediationScript = ""
                },
                new termFormat()
                {
                    ID = "VA1244",
                    Check = "Orphaned users should be removed from SQL server databases",
                    Category = "Surface Area Reduction",
                    Risk = "Medium",
                    Des = "A database user that exists on a database, but has no corresponding login in master database or as an external resource (i.e. Windows user) is referred to as an orphaned user and it should either be removed or remapped to a valid login. This rule checks that there are no orphaned users.",
                    Impact = "Orphaned users are typically signs of a misconfiguration. These users create a risk because potential attackers might get access to them and inherit their permissions on the database.",
                    Query = @"SELECT NAME AS [Principal]
FROM sys.database_principals
WHERE sid NOT IN (
        SELECT sid
        FROM sys.server_principals
        )
    AND authentication_type_desc = 'INSTANCE'
    AND type = 'S'
    AND principal_id != 2
    AND DATALENGTH(sid) <= 28",
                    Remediation = "Drop the orphaned users or remap them to a different login.",
                    RemediationScript = "DROP USER [$0]"
                }
            };
        }
    }
}
