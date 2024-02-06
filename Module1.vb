
Imports System.IO
Imports System.Net
Imports MySql.Data.MySqlClient

Structure datosSincronizador
    Dim rutar As String
    Dim sucursalr As String
    Dim ipr As String
    Dim baser As String
    Dim usuarior As String
    Dim passr As String
End Structure

Structure datosAutoFac
    Dim rutar As String
    Dim sucursalr As String
    Dim ipr As String
    Dim baser As String
    Dim usuarior As String
    Dim passr As String
End Structure

Module Module1

    Public varnumbase As String = ""
    Public varrutabase As String = ""
    Public sTarget As String = ""
    Public vempresa As Integer = 1
    Public uidcancel As String
    Public refccancel As String
    Public directoriof As String = My.Application.Info.DirectoryPath & "\ARCHIVOSDL" & varnumbase & ""
    Public cadena_pagos1 As String = ""
    Public metodo_p_ac As String = ""
    Public Const ESTATUS_FACTURA = 1
    Public Const ESTATUS_PREFACTURA = 2
    Public Const ESTATUS_ARRENDAMIENTO = 4
    Public Const ESTATUS_HONORARIOS = 5
    Public Const ESTATUS_NOTASCREDITO = 6
    Public Const ESTATUS_FACTURA_ERROR = 3

    Public numero_MAC As String = ""

    Public ipserverF As String = ""
    Public databaseF As String = ""
    Public userbdF As String = ""
    Public passbdF As String = ""

    Public susursalr As Integer = 0

    Public sTargetlocal As String = "server=" & dameIP2() & ";uid=Delsscom;password=jipl22;database=cn1;persist security info=false;connect timeout=300"


    Public ARCHIVO_CONF_FACTURAS = My.Application.Info.DirectoryPath & "\Configurapdv.dat"
    Public ARCHIVO_DE_CONFIGURACION = My.Application.Info.DirectoryPath & "\Configurapdvf.dat"
    Public ARCHIVO_DE_CONFIGURACION_F = My.Application.Info.DirectoryPath & "\Configurapdvfac.dat"

    Public sTargetdSincro As String = ""
    Public sTargetMYSQL As String = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=300"
    Public sTargetdAutoFac As String = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=300"

    Public ipserver As String = ""
    Public database As String = ""
    Public userbd As String = ""
    Public passbd As String = ""
    Public serie_gen As String = ""
    Public timbres_totales As Integer = 0
    Public timbres_timbrados As Integer = 0
    Public var_cotb As Integer = 1


    Public Function conecta()
        Dim sInfo As String = ""
        Dim cnn As MySqlConnection = New MySqlConnection
        Dim dr As DataRow
        Dim odata As New ToolKitSQL.myssql
        Dim tta As Integer = 1
        Dim ssql As String = "Select * from sucursales where id=" & susursalr
        With odata
            If odata.dbOpen(cnn, sTargetdSincro, sInfo) Then
                If odata.getDr(cnn, dr, ssql, sInfo) Then
                    frmSincro.lbl_nombre.Text = dr("nombre").ToString
                    frmSincro.lbl_direccion.Text = dr("direccion").ToString
                    frmSincro.grid_eventos.Rows.Insert(0, "Conectado a Delsscom", Date.Now)
                End If
                cnn.Close()
            Else
                frmSincro.grid_eventos.Rows.Insert(0, "No se pudo Conectar a Delsscom", Date.Now)
                Return False
            End If
        End With

        Return True

    End Function

    Public Function dameIP2() As String
        Dim nombrePC As String
        Dim entradasIP As Net.IPHostEntry
        nombrePC = Dns.GetHostName
#Disable Warning BC40000
        entradasIP = Dns.GetHostByName(nombrePC)
#Enable Warning BC40000
        Dim direccion_Ip As String = entradasIP.AddressList(0).ToString

        Return direccion_Ip
    End Function

End Module
