-- Crear Base de Datos
CREATE DATABASE XYZDatabase;
GO
USE [XYZDatabase]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[PostId] [int] NULL,
	[Name] [nvarchar](100) NULL,
	[Email] [nvarchar](100) NULL,
	[Body] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FamilyGroup]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FamilyGroup](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[Cedula] [nvarchar](50) NULL,
	[Nombres] [nvarchar](100) NULL,
	[Apellidos] [nvarchar](100) NULL,
	[Genero] [nvarchar](50) NULL,
	[Parentesco] [nvarchar](50) NULL,
	[Edad] [int] NULL,
	[MenorEdad] [bit] NULL,
	[FechaNacimiento] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Posts]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[PostId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[Title] [nvarchar](255) NULL,
	[Body] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[PostId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestLog]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[RequestType] [nvarchar](10) NULL,
	[RequestURL] [nvarchar](255) NULL,
	[RequestDetails] [nvarchar](max) NULL,
	[IsSuccessful] [bit] NULL,
	[FailureReason] [nvarchar](max) NULL,
	[Timestamp] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NULL,
	[Password] [nvarchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[RequestLog] ON 
GO
INSERT [dbo].[RequestLog] ([LogId], [RequestType], [RequestURL], [RequestDetails], [IsSuccessful], [FailureReason], [Timestamp]) VALUES (1, N'GET', N'/swagger/index.html', N'', 1, NULL, CAST(N'2023-12-18T20:34:28.680' AS DateTime))
GO
INSERT [dbo].[RequestLog] ([LogId], [RequestType], [RequestURL], [RequestDetails], [IsSuccessful], [FailureReason], [Timestamp]) VALUES (2, N'GET', N'/swagger/v1/swagger.json', N'', 1, NULL, CAST(N'2023-12-18T20:34:29.507' AS DateTime))
GO
INSERT [dbo].[RequestLog] ([LogId], [RequestType], [RequestURL], [RequestDetails], [IsSuccessful], [FailureReason], [Timestamp]) VALUES (3, N'POST', N'/api/Users/AutoCreate', N'', 0, NULL, CAST(N'2023-12-18T20:36:44.993' AS DateTime))
GO
INSERT [dbo].[RequestLog] ([LogId], [RequestType], [RequestURL], [RequestDetails], [IsSuccessful], [FailureReason], [Timestamp]) VALUES (4, N'POST', N'/api/Users/authenticate', N'{
  "userID": 1,
  "username": "Usuario1",
  "password": "Contrasenna1",
  "email": "email1@ejemplo.com",
  "isActive": true
}', 1, NULL, CAST(N'2023-12-18T20:39:11.780' AS DateTime))
GO
SET IDENTITY_INSERT [dbo].[RequestLog] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 
GO
INSERT [dbo].[Users] ([UserID], [Username], [Password], [Email], [IsActive]) VALUES (1, N'Usuario1', N'Contrasenna1', N'email1@ejemplo.com', 1)
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
ALTER TABLE [dbo].[RequestLog] ADD  DEFAULT (getdate()) FOR [Timestamp]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD FOREIGN KEY([PostId])
REFERENCES [dbo].[Posts] ([PostId])
GO
ALTER TABLE [dbo].[FamilyGroup]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Posts]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserID])
GO
/****** Object:  StoredProcedure [dbo].[InsertComment]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Insertar un Comentario
CREATE PROCEDURE [dbo].[InsertComment]
    @PostId INT,
    @Name NVARCHAR(100),
    @Email NVARCHAR(100),
    @Body NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Comments (PostId, Name, Email, Body)
    VALUES (@PostId, @Name, @Email, @Body)
END
GO
/****** Object:  StoredProcedure [dbo].[InsertFamilyMember]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- Procedimientos Almacenados para la Tabla FamilyGroup
--Insertar un Miembro del Grupo Familiar
--Este procedimiento también calculará si el miembro es menor de edad basándose en la fecha de nacimiento.
CREATE PROCEDURE [dbo].[InsertFamilyMember]
    @UserID INT,
    @Cedula NVARCHAR(50),
    @Nombres NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @Genero NVARCHAR(50),
    @Parentesco NVARCHAR(50),
    @Edad INT,
    @FechaNacimiento DATE
AS
BEGIN
    DECLARE @MenorEdad BIT
    SET @MenorEdad = CASE WHEN DATEDIFF(YEAR, @FechaNacimiento, GETDATE()) < 18 THEN 1 ELSE 0 END

    IF NOT EXISTS (SELECT 1 FROM FamilyGroup WHERE Cedula = @Cedula)
    BEGIN
        INSERT INTO FamilyGroup (UserID, Cedula, Nombres, Apellidos, Genero, Parentesco, Edad, MenorEdad, FechaNacimiento)
        VALUES (@UserID, @Cedula, @Nombres, @Apellidos, @Genero, @Parentesco, @Edad, @MenorEdad, @FechaNacimiento)
    END
    ELSE
    BEGIN
        RAISERROR ('Un miembro del grupo familiar con la misma cédula ya existe', 16, 1)
    END
END
GO
/****** Object:  StoredProcedure [dbo].[InsertPost]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Procedimientos Almacenados para Posts y Comments
-- Insertar un Post
CREATE PROCEDURE [dbo].[InsertPost]
    @UserId INT,
    @Title NVARCHAR(255),
    @Body NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Posts (UserId, Title, Body)
    VALUES (@UserId, @Title, @Body)
END
GO
/****** Object:  StoredProcedure [dbo].[InsertRequestLog]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Procedimientos Almacenados para la Tabla RequestLog
--Registrar una Petición
CREATE PROCEDURE [dbo].[InsertRequestLog]
    @RequestType NVARCHAR(10),
    @RequestURL NVARCHAR(255),
    @RequestDetails NVARCHAR(MAX),
    @IsSuccessful BIT,
    @FailureReason NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO RequestLog (RequestType, RequestURL, RequestDetails, IsSuccessful, FailureReason, Timestamp)
    VALUES (@RequestType, @RequestURL, @RequestDetails, @IsSuccessful, @FailureReason, GETDATE())
END
GO
/****** Object:  StoredProcedure [dbo].[UserAuthentication]    Script Date: 18/12/2023 03:49:59 p. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Procedimientos Almacenados para la Tabla Users
-- Autenticación de Usuario
CREATE PROCEDURE [dbo].[UserAuthentication]
    @Username NVARCHAR(50),
    @Password NVARCHAR(50),
    @Authenticated BIT OUTPUT
AS
BEGIN
    SET @Authenticated = 0
    IF EXISTS (SELECT 1 FROM Users WHERE Username = @Username AND Password = @Password)
        SET @Authenticated = 1
END
GO
