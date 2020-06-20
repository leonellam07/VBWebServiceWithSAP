Imports SAPbobsCOM

Public Class ApplicationContext
    Implements IDisposable

    Public Property SBOCompany As Company

    Private Property DistribucionSQL As String 'Verificar que tipo base de datos
    Private Property LicenseServer As String 'Servidor de Licencias
    Private Property Server As String 'Verifica el host donde se encuentra alojado SAP
    Private Property CompanyDB As String 'El nombre de la base de datos
    Private Property UserName As String 'Usuario de SAP Ej. manager
    Private Property Password As String 'Password de SAP 
    Private Property TrustedFlag As String 'Colocar servidor de Confianza

    Private _ErrorLog As String

    Public Function GetErrorLog() As String
        Return String.Format("Error ({0}): {1}", SBOCompany.GetLastErrorCode, SBOCompany.GetLastErrorDescription)
    End Function



    Public Sub New()
        DistribucionSQL = ConfigurationManager.AppSettings("DistributationSQL")
        Server = ConfigurationManager.AppSettings("Server")
        CompanyDB = ConfigurationManager.AppSettings("Database")
        UserName = ConfigurationManager.AppSettings("UserName")
        Password = ConfigurationManager.AppSettings("Password")
        TrustedFlag = ConfigurationManager.AppSettings("Trusted")

        Open()
    End Sub

    Private Sub Open()

        Dim trusted As Boolean = False
        Boolean.TryParse(TrustedFlag, trusted)


        SBOCompany = New Company With {
            .Server = Server,
            .UserName = UserName,
            .Password = Password,
            .CompanyDB = CompanyDB,
            .UseTrusted = trusted
        }

        Select Case DistribucionSQL
            Case "HANA" : SBOCompany.DbServerType = BoDataServerTypes.dst_HANADB
            Case "MSSQL2014" : SBOCompany.DbServerType = BoDataServerTypes.dst_MSSQL2014
            Case "MSSQL2016" : SBOCompany.DbServerType = BoDataServerTypes.dst_MSSQL2016
        End Select

        If (SBOCompany.Connect() <> 0) Then
            Throw New Exception(SBOCompany.GetLastErrorDescription)
        End If
    End Sub

    Private Sub DisconectService()
        If (IsNothing(SBOCompany) <> False) Then
            If (SBOCompany.Connected) Then
                SBOCompany.Disconnect()
                SBOCompany = Nothing
            End If
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        DisconectService()
        GC.SuppressFinalize(Me)
    End Sub

End Class
