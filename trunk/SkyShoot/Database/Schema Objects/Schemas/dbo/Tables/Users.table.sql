/* 
 * TABLE: Users 
 */

/* 
 * TABLE: Users 
 */

CREATE TABLE Users(
    Login               nvarchar(100)    NOT NULL,
    Email               email            NOT NULL,
    Password            binary(32)       NOT NULL,
    Salt                binary(8)        NOT NULL,
    FirstName           nvarchar(100)    NULL,
    LastName            nvarchar(100)    NULL,
    AvaUrl              nvarchar(200)    NULL,
    RegistrationDate    datetime2(0)     NOT NULL,
    LastUpdateDate      datetime2(0)     NOT NULL,
    Status              tinyint          CONSTRAINT [DF_Users_Status] DEFAULT 1 NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY NONCLUSTERED (Login)
)





