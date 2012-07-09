/* 
 * TABLE: Administrators 
 */

/* 
 * TABLE: Administrators 
 */

CREATE TABLE Administrators(
    Login       varchar(100)    NOT NULL,
    Password    binary(32)      NOT NULL,
    Salt        binary(8)       NOT NULL,
    CONSTRAINT PK_Administrators PRIMARY KEY CLUSTERED (Login)
)





