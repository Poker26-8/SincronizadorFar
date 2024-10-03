
Imports System.IO
Imports System.Net
Imports MySql.Data
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

Structure datosAndroid
    Dim ipr As String
    Dim baser As String
    Dim usuarior As String
    Dim passr As String
End Structure

Structure datosAndroidI
    Dim inventarioA As String
End Structure

Module Module1

    Public sTarget As String = ""

    Public ipserverF As String = ""
    Public databaseF As String = ""
    Public userbdF As String = ""
    Public passbdF As String = ""

    Public ipserverA As String = ""
    Public databaseA As String = ""
    Public userbdA As String = ""
    Public passbdA As String = ""

    Public tipoInventario As Integer = 0

    Public susursalr As Integer = 0

    Public sTargetlocal As String = "server=" & dameIP2() & ";uid=Delsscom;password=jipl22;database=cn1;persist security info=false;connect timeout=300"

    Public ARCHIVO_CONF_FACTURAS = My.Application.Info.DirectoryPath & "\Configurapdv.dat"
    Public ARCHIVO_DE_CONFIGURACION = My.Application.Info.DirectoryPath & "\Configurapdvf.dat"
    Public ARCHIVO_DE_CONFIGURACION_F = My.Application.Info.DirectoryPath & "\Configurapdvfac.dat"
    Public ARCHIVO_DE_CONFIGURACION_A = My.Application.Info.DirectoryPath & "\ConfiguraAndroid.dat"
    Public ARCHIVO_DE_CONFIGURACION_AI = My.Application.Info.DirectoryPath & "\ConfiguraAndroidI.dat"

    Public sTargetdSincro As String = ""
    Public sTargetMYSQL As String = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=300"
    Public sTargetdAutoFac As String = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=300"
    Public sTargetdAndroid As String = "server=;uid=;password=;database=;persist security info=false;connect timeout=300"

    Public ipserver As String = ""
    Public database As String = ""
    Public userbd As String = ""
    Public passbd As String = ""
    Public serie_gen As String = ""
    Public timbres_totales As Integer = 0
    Public timbres_timbrados As Integer = 0
    Public var_cotb As Integer = 1


    Public Function conecta()

        Dim banderanoentro As Integer = 0
        Dim banderasientro As Integer = 0

        Dim sInfo As String = ""
        Dim cnn As MySqlConnection = New MySqlConnection
        Dim dr As DataRow
        Dim odata As New ToolKitSQL.myssql
        Dim tta As Integer = 1
        Dim ssql As String = ""
        With odata

            If sTargetdSincro <> "" Then

                ssql = "Select * from sucursales where id=" & susursalr

                If odata.dbOpen(cnn, sTargetdSincro, sInfo) Then
                    If odata.getDr(cnn, dr, ssql, sInfo) Then
                        banderasientro += 1
                        frmSincro.lbl_nombre.Text = dr("nombre").ToString
                        frmSincro.lbl_direccion.Text = dr("direccion").ToString
                        frmSincro.grid_eventos.Rows.Insert(0, "Conectado a Delsscom", Date.Now)
                    End If
                    cnn.Close()
                Else
                    banderanoentro += 1
                    frmSincro.lbl_nombre.Text = ""
                    frmSincro.lbl_direccion.Text = ""
                    'frmSincro.grid_eventos.Rows.Insert(0, "No se pudo Conectar a Delsscom", Date.Now)
                    'Return False
                End If
            Else
                frmSincro.lbl_nombre.Text = ""
                frmSincro.lbl_direccion.Text = ""
            End If

            If sTargetdAutoFac <> "" Then

                ssql = "Select * from sucursales2"

                If odata.dbOpen(cnn, sTargetdAutoFac, sInfo) Then
                    If odata.getDr(cnn, dr, ssql, sInfo) Then
                        banderasientro += 1
                        'frmSincro.lbl_nombre.Text = dr("nombre").ToString
                        'frmSincro.lbl_direccion.Text = dr("direccion").ToString
                        frmSincro.grid_eventos.Rows.Insert(0, "Conectado a AUTOFACT Delsscom ", Date.Now)

                        Dim fechahoy As Date = FormatDateTime(Date.Now, DateFormat.ShortDate)
                        If fechahoy > CDate(dr("FechaTermino").ToString) Then
                            MsgBox("El tiempo de renta terminó, contacte con su asesor de Delsscom para adquirir la renovación de su sistema")
                            End
                        End If

                    End If
                    cnn.Close()
                Else
                    banderanoentro += 1
                End If

            End If

            If sTargetdAndroid <> "" Then

                ssql = "Select * from sucursales"

                If odata.dbOpen(cnn, sTargetdAndroid, sInfo) Then
                    If odata.getDr(cnn, dr, ssql, sInfo) Then
                        banderasientro += 1
                        'frmSincro.lbl_nombre.Text = dr("nombre").ToString
                        'frmSincro.lbl_direccion.Text = dr("direccion").ToString
                        frmSincro.grid_eventos.Rows.Insert(0, "Conectado a ANDROID Delsscom ", Date.Now)

                        Dim fechahoy As Date = FormatDateTime(Date.Now, DateFormat.ShortDate)
                        If fechahoy > CDate(dr("FechaTermino").ToString) Then
                            MsgBox("El tiempo de renta terminó, contacte con su asesor de Delsscom para adquirir la renovación de su sistema")
                            End
                        End If

                    End If
                    cnn.Close()
                Else
                    banderanoentro += 1
                End If

            End If

        End With

        If banderanoentro > 0 Or banderasientro = 0 Then
            Return False
        Else
            Return True
        End If

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
