Imports Microsoft.SqlServer
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Windows
Imports System.Windows.Forms.DataFormats
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Data.OleDb
Imports MySql.Data
Public Class frmConfigSincro

    Private configSincro As datosSincronizador
    Private configFSincro As datosAutoFac
    Private filenum As Integer
    Private recordLen As String
    Private currentRecord As Long
    Private lastRecord As Long
    Private Sub frmConfigSincro_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION) Then

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION, OpenMode.Random, OpenAccess.ReadWrite)

            recordLen = Len(configSincro)

            FileGet(filenum, configSincro, 1)

            ipserver = Trim(configSincro.ipr)
            database = Trim(configSincro.baser)
            userbd = Trim(configSincro.usuarior)
            passbd = Trim(configSincro.passr)
            If IsNumeric(Trim(configSincro.sucursalr)) Then
                susursalr = Trim(configSincro.sucursalr)
            End If

            txt_servidor.Text = Trim(configSincro.ipr)
            txt_base.Text = Trim(configSincro.baser)
            txt_usuario.Text = Trim(configSincro.usuarior)
            txt_contrasena.Text = Trim(configSincro.passr)
            llena_sucursales()

            If IsNumeric(Trim(configSincro.sucursalr)) Then
                cbosucursal.SelectedValue = Trim(configSincro.sucursalr)
            End If
            llena_sucursales()
            FileClose()
        Else

            cbosucursal.Enabled = False

        End If

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION_F) Then

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION_F, OpenMode.Random, OpenAccess.ReadWrite)

            recordLen = Len(configFSincro)

            FileGet(filenum, configFSincro, 1)

            ipserverF = Trim(configFSincro.ipr)
            databaseF = Trim(configFSincro.baser)
            userbdF = Trim(configFSincro.usuarior)
            passbdF = Trim(configFSincro.passr)

            txtServidorF.Text = Trim(configFSincro.ipr)
            txtBaseF.Text = Trim(configFSincro.baser)
            txtUsuarioF.Text = Trim(configFSincro.usuarior)
            txtContraF.Text = Trim(configFSincro.passr)

            FileClose()

        End If
    End Sub

    Private Sub llena_sucursales()
        Dim sInfo As String = ""
        Dim cnn As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection
        Dim sSQL As String = "SELECT * FROM sucursales"

        Dim odata As New ToolKitSQL.myssql
        With odata

            If odata.dbOpen(cnn, sTargetdSincro, sInfo) Then

                Dim ds As New DataSet

                If odata.getDs(cnn, ds, sSQL, "edos", sInfo) Then
                    With cbosucursal
                        .DataSource = ds.Tables("edos")
                        .ValueMember = "id"
                        .DisplayMember = "nombre"
                    End With
                Else
                    '   MessageBox.Show("Error al conectar con los Datos")
                End If
                cnn.Close()
            Else
                ' MessageBox.Show("Error al conectar con los Datos")
            End If
        End With
        If cbosucursal.Items.Count > 0 Then
            cbosucursal.Enabled = True
        End If
    End Sub

    Public Sub salva_datos()

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION) Then
            IO.File.Delete(ARCHIVO_DE_CONFIGURACION)
        End If

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION_F) Then
            IO.File.Delete(ARCHIVO_DE_CONFIGURACION_F)
        End If

        Try

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION, OpenMode.Random, OpenAccess.ReadWrite)

            recordLen = Len(configSincro)

            FileGet(filenum, configSincro, 1)

            configSincro.ipr = txt_servidor.Text
            configSincro.baser = txt_base.Text
            configSincro.usuarior = txt_usuario.Text
            configSincro.passr = txt_contrasena.Text

            If cbosucursal.Text <> "" Then
                configSincro.sucursalr = cbosucursal.SelectedValue
            Else

            End If

            ipserver = Trim(configSincro.ipr)
            database = Trim(configSincro.baser)
            userbd = Trim(configSincro.usuarior)
            passbd = Trim(configSincro.passr)
            If cbosucursal.Items.Count > 0 Then
                susursalr = Trim(configSincro.sucursalr)
            End If

            FilePut(filenum, configSincro, 1)

            FileClose()

            sTargetdSincro = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=30"

            If cbosucursal.Items.Count > 0 Then
                llena_sucursales()
                cbosucursal.SelectedValue = configSincro.sucursalr
                MsgBox("guadado correctamente")

                Dim cias As OleDb.OleDbConnection = New OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & My.Application.Info.DirectoryPath & "\CIAS.mdb;")
                Dim coma As OleDbCommand = New OleDbCommand
                Dim lect As OleDbDataReader = Nothing

                cias.Close()
                cias.Open()
                coma = cias.CreateCommand
                coma.CommandText = "Update Server set Zink=1"
                coma.ExecuteNonQuery()
            Else
                MsgBox("Seleccione Alguna Sucursal")
                llena_sucursales()
            End If

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION_F, OpenMode.Random, OpenAccess.ReadWrite)

            recordLen = Len(configFSincro)

            FileGet(filenum, configFSincro, 1)

            configFSincro.ipr = txtServidorF.Text
            configFSincro.baser = txtBaseF.Text
            configFSincro.usuarior = txtUsuarioF.Text
            configFSincro.passr = txtContraF.Text

            ipserverF = Trim(configFSincro.ipr)
            databaseF = Trim(configFSincro.baser)
            userbdF = Trim(configFSincro.usuarior)
            passbdF = Trim(configFSincro.passr)

            FilePut(filenum, configFSincro, 1)

            FileClose()

            sTargetdAutoFac = "server=" & ipserverF & ";uid=" & userbdF & ";password=" & passbdF & ";database=" & databaseF & ";persist security info=false;connect timeout=300"

        Catch ex As Exception
            MsgBox(ex.ToString)
        End Try

    End Sub

    Private Sub btnGuardaFormatos_Click(sender As Object, e As EventArgs) Handles btnGuardaFormatos.Click
        salva_datos()
    End Sub

    Private Sub btnout_Click(sender As Object, e As EventArgs) Handles btnout.Click
        frmSincro.Show()
        frmSincro.Enabled = True
        frmSincro.Timer_datos.Start()
        frmSincro.Timer_reconecta.Start()
        Me.Close()
    End Sub

End Class