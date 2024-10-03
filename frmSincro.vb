
Imports MySql
Imports MySql.Data
Imports MySql.Data.MySqlClient
Imports Mysqlx
Imports System.Data.SqlTypes
Imports System.IO
Imports System.Media
Imports System.Text.RegularExpressions
Imports ToolKitSQL

Public Class frmSincro

    Private config As datosSincronizador
    Private configF As datosAutoFac
    Private configA As datosAndroid
    Private configAI As datosAndroidI
    Private filenum As Integer
    Private recordLen As String
    Private currentRecord As Long
    Private lastRecord As Long
    Dim sucu As String = 0
    Dim num_Sucursales As Integer = 0
    Dim es_matriz As Integer = 0
    Dim dt_Sucursales As New DataTable
    Dim dr_Sucursales As DataRow
    Public sucdestino As Integer = 0

    Dim codigopro As String = ""
    Dim productosxd As String = ""

#Region "Minimizar a la bandeja del sistema"

    Private Sub Base_Resize(sender As Object, e As System.EventArgs) Handles Me.Resize
        ' Agregado NotifyIcon1.Visible = False en el load del form
        'Si el estado actual de la ventana es "minimizado"...
        If Me.WindowState = FormWindowState.Minimized Then
            'Ocultamos el formulario
            Me.Visible = False
            'Hacemos visible el icono de la bandeja del sistema
            NotifyIcon1.Visible = True
            Me.ShowInTaskbar = False
        End If
    End Sub

    Private Sub NotifyIcon1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon1.MouseDoubleClick
        Me.Show()
        Me.WindowState = FormWindowState.Normal
        'Ocultamos el icono de la bandeja de sistema
        NotifyIcon1.Visible = False
        Me.ShowInTaskbar = True
    End Sub


    Private Sub NotifyIcon2_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles NotifyIcon2.MouseDoubleClick
        MsgBox("Se han recibido correctamente los productos " & productosxd, vbInformation + vbOKOnly, "Delsscom Sincronizador")
        'Ocultamos el icono de la bandeja de sistema
        NotifyIcon1.Visible = False
        Me.ShowInTaskbar = True
        productosxd = ""
    End Sub
#End Region

    Private Sub btn_configura_Click(sender As Object, e As EventArgs) Handles btn_configura.Click

        frmConfigSincro.Show()
        Timer_datos.Stop()
        Timer_reconecta.Stop()

    End Sub

    Private Sub frmSincro_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim banderaentra As Integer = 0

        sTargetdSincro = ""

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION) Then

            banderaentra = 1

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION, OpenMode.Random, OpenAccess.ReadWrite)

            recordLen = Len(config)

            FileGet(filenum, config, 1)

            ipserver = Trim(config.ipr)
            database = Trim(config.baser)
            userbd = Trim(config.usuarior)
            passbd = Trim(config.passr)
            If IsNumeric(Trim(config.sucursalr)) Then
                susursalr = Trim(config.sucursalr)
            End If

            'susursalr = 1

            If ipserver = "" Or database = "" Or userbd = "" Or passbd = "" Then
                sTargetdSincro = ""
            Else
                sTargetdSincro = "server=" & ipserver & ";uid=" & userbd & ";password=" & passbd & ";database=" & database & ";persist security info=false;connect timeout=300"
            End If

            'sTargetdSincro = "server=72.167.50.81;uid=delsscomdeli1_sincro1;password=Delsscom22;database=delsscomdeli1_Sincronizador;persist security info=false;connect timeout=300"
            'sTargetdSincro = "server=72.167.50.81;uid=delsincro1_pruebas;password=Delsscom22;database=delsincro1_pruebas;persist security info=false;connect timeout=300"
            'sTargetdSincro = "server=72.167.50.81;uid=delsincro1_sincro2;password=Delsscom22;database=delsincro1_TaqLDMonterrey;persist security info=false;connect timeout=300"
            'sTargetdSincro = "server=72.167.50.81;uid=delsincro1_sincro4;password=Delsscom22;database=delsincro1_AjusteFarmacias;persist security info=false;connect timeout=300"

            FileClose()

            If Not IsNumeric(susursalr) Then
                ' MsgBox("Es Necesario Configurar la Sucursal")
                frmConfigSincro.Show()
            Else
            End If

            Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
            Dim sSQL As String = ""
            Dim dt As New DataTable
            Dim dr As DataRow
            Dim sinfo As String = ""
            Dim odata2 As New ToolKitSQL.myssql
            With odata2
                'If .dbOpen(cnn2, sTargetdSincro, sinfo) Then
                '    If .getDr(cnn2, dr, "select FechaTermino from renta", sinfo) Then
                '        Dim fechahoy As Date = FormatDateTime(Date.Now, DateFormat.ShortDate)

                '        If fechahoy > CDate(dr(0).ToString) Then
                '            MsgBox("El tiempo de renta terminó, contacte con su asesor de Delsscom para adquirir la renovación de su sistema")
                '            End
                '        End If
                '    Else
                '        cnn2.Close()
                '        MsgBox("Debe asignar una fecha de inicio de renta")
                '        End
                '    End If
                '    cnn2.Close()
                'Else
                '    'MsgBox("Error de conexión 1")
                '    ' MsgBox(sinfo)
                'End If
            End With

        Else
            'frmConfigSincro.Show()
            'Me.WindowState = FormWindowState.Minimized
        End If

        sTargetdAutoFac = ""

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION_F) Then

            banderaentra = 1

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION_F, OpenMode.Random, OpenAccess.ReadWrite)
            recordLen = Len(configF)
            FileGet(filenum, configF, 1)
            ipserverF = Trim(configF.ipr)
            databaseF = Trim(configF.baser)
            userbdF = Trim(configF.usuarior)
            passbdF = Trim(configF.passr)

            If ipserverF = "" Or databaseF = "" Or userbdF = "" Or passbdF = "" Then
                sTargetdAutoFac = ""
            Else
                sTargetdAutoFac = "server=" & ipserverF & ";uid=" & userbdF & ";password=" & passbdF & ";database=" & databaseF & ";persist security info=false;connect timeout=300"
                Label1.Text = "AutoFact base: " & databaseF
                Label1.Visible = True
            End If

            ' sTargetdAutoFac = "server=72.167.50.81;uid=delsincro1_NewAutoF;password=Delsscom22;database=delsincro1_NewAutoF;persist security info=false;connect timeout=300"
            'sTargetdAutoFac = "server=72.167.50.81;uid=delsincro1_NewAutoF;password=Delsscom22;database=delsincro1_TaqLDMonterreyAF;persist security info=false;connect timeout=300"


            FileClose()
        Else
            ipserverF = ""
            databaseF = ""
            userbdF = ""
            passbdF = ""
            Label1.Visible = False
        End If

        sTargetdAndroid = ""

        If IO.File.Exists(ARCHIVO_DE_CONFIGURACION_A) Then

            banderaentra = 1

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION_A, OpenMode.Random, OpenAccess.ReadWrite)
            recordLen = Len(configA)
            FileGet(filenum, configA, 1)
            ipserverA = Trim(configA.ipr)
            databaseA = Trim(configA.baser)
            userbdA = Trim(configA.usuarior)
            passbdA = Trim(configA.passr)
            If ipserverA = "" Or databaseA = "" Or userbdA = "" Or passbdA = "" Then
                sTargetdAndroid = ""
            Else
                sTargetdAndroid = "server=" & ipserverA & ";uid=" & userbdA & ";password=" & passbdA & ";database=" & databaseA & ";persist security info=false;connect timeout=300"
                Label3.Text = "Android base: " & databaseA
                Label3.Visible = True
            End If

            'sTargetdAutoFac = "server=72.167.50.81;uid=delsincro1_NewAutoF;password=Delsscom22;database=delsincro1_NewAutoF;persist security info=false;connect timeout=300"

            FileClose()

            filenum = FreeFile()
            FileOpen(filenum, ARCHIVO_DE_CONFIGURACION_AI, OpenMode.Random, OpenAccess.ReadWrite)
            recordLen = Len(configAI)
            FileGet(filenum, configAI, 1)
            tipoInventario = configAI.inventarioA
            FileClose()

        Else
            ipserverA = ""
            databaseA = ""
            userbdA = ""
            passbdA = ""

            tipoInventario = 0

            Label3.Visible = False
        End If

        If banderaentra = 0 Then
            frmConfigSincro.Show()
            Me.WindowState = FormWindowState.Minimized
        Else

            If sTargetdAndroid <> "" Or sTargetdAutoFac <> "" Or sTargetdSincro <> "" Then
                If conecta() Then
                    insertarcampos()
                    Timer_datos.Start()
                Else
                    Timer_reconecta.Start()
                End If
            Else
                frmConfigSincro.Show()
                Me.WindowState = FormWindowState.Minimized
            End If

        End If

        NotifyIcon1.BalloonTipText = "Form 2 abierto"
        Me.WindowState = FormWindowState.Minimized

    End Sub

    Private Sub insertarcampos()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim odata As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then

            'Dim connectionString As String = sTargetlocal

            '' Valores para los parámetros del procedimiento almacenado
            'Dim nombreCliente As String = "Cliente ejemplo"
            'Dim montoVenta As Decimal = 100.5

            'Using connection As New MySqlConnection(connectionString)
            '    Using command As New MySqlCommand("ResetAutoIncrement", connection)
            '        command.CommandType = CommandType.StoredProcedure

            '        ' Configurar los parámetros del procedimiento almacenado
            '        command.Parameters.AddWithValue("@pruebitas", 9999)

            '        connection.Open()
            '        command.ExecuteNonQuery()
            '    End Using
            'End Using

            If odata.getDt(cnn, dt, "select Cargado from devoluciones", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE devoluciones ADD COLUMN Cargado Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update devoluciones set Cargado = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select CargadoF from ventas", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE ventas ADD CargadoF Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update ventas set CargadoF = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select CargadoAndroid from usuarios", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE usuarios ADD COLUMN CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update usuarios set CargadoAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select CargadoAndroid from productos", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE productos ADD COLUMN CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update productos set CargadoAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select * from traspasos", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "CREATE TABLE traspasos (Id INT AUTO_INCREMENT PRIMARY KEY, IdTraspasoNube INTEGER DEFAULT 0, Usuario VARCHAR(255), TotalProd Double DEFAULT 0, Fecha VARCHAR(255), Tipo VARCHAR(255), Cargado INTEGER DEFAULT 0, Eliminado INTEGER DEFAULT 0, Sucursal VARCHAR(150))", sinfo)
                    odata.runSp(cnn, "CREATE TABLE traspasosdetalle (Id INT AUTO_INCREMENT PRIMARY KEY, IdTraspaso INTEGER DEFAULT 0, Codigo VARCHAR(255), Descripcion VARCHAR(255), Cantidad Double DEFAULT 0, Lote VARCHAR(255), Caducidad VARCHAR(255))", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select Sucursal from traspasos", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE traspasos ADD COLUMN Sucursal VARCHAR(150)", sinfo)
                    odata.runSp(cnn, "update traspasos set Sucursal = ''", sinfo)
                    sinfo = ""
                End If
            End If



            If odata.getDt(cnn, dt, "select CargadoAndroid from clientes", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE clientes ADD COLUMN CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update clientes set CargadoAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select CargadoAndroid from datosnegocio", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE datosnegocio ADD COLUMN CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update datosnegocio set CargadoAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select FolioAndroid, CargadoAndroid from ventas", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE ventas ADD COLUMN FolioAndroid Integer DEFAULT 0, CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update ventas set CargadoAndroid = 1, FolioAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select CargadoAndroid from abonoi", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE abonoi ADD COLUMN CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update abonoi set CargadoAndroid = 1", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select FolioAndroid, CargadoAndroid from cotped", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE cotped ADD COLUMN FolioAndroid Integer DEFAULT 0, CargadoAndroid Integer DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update cotped set CargadoAndroid = 1, FolioAndroid = 0", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select * from pedidostemporal", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "CREATE TABLE pedidostemporal (Id AUTOINCREMENT, IdNube INTEGER DEFAULT 0 , IdPedido INTEGER DEFAULT 0, IdVenta INTEGER DEFAULT 0, Codigo TEXT(255), Descripcion TEXT(255), Cantidad Double DEFAULT 0, Precio Double DEFAULT 0, FechaHora TEXT(255), Usuario TEXT(255))", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select Usuario from ventasdetalle", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE ventasdetalle ADD COLUMN Usuario TEXT(255)", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select * from clienteeliminado", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "CREATE TABLE clienteeliminado (Id AUTOINCREMENT, Nombre TEXT(255), CargadoAndroid INTEGER DEFAULT 0)", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select * from productoeliminado", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "CREATE TABLE productoeliminado (Id AUTOINCREMENT, Codigo TEXT(255), Nombre TEXT(255), CargadoAndroid INTEGER DEFAULT 0)", sinfo)
                    sinfo = ""
                End If
            End If

            'If odata.getDt(cnn, dt, "select UniMed from RepPedidos", sinfo) Then
            'Else
            '    If sinfo <> "" Then
            '        odata.runSp(cnn, "ALTER TABLE RepPedidos ADD COLUMN UniMed TEXT(255)", sinfo)
            '        sinfo = ""
            '    End If
            'End If

            If odata.getDt(cnn, dt, "select * from pedidoeliminado", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "CREATE TABLE pedidoeliminado (Id AUTOINCREMENT, IdPedido TEXT(20), IdPedidoA TEXT(20), CargadoAndroid INTEGER DEFAULT 0)", sinfo)
                    sinfo = ""
                End If
            End If

            If odata.getDt(cnn, dt, "select FolioAndroid from abonoi", sinfo) Then
            Else
                If sinfo <> "" Then
                    odata.runSp(cnn, "ALTER TABLE abonoi ADD COLUMN FolioAndroid TEXT(255) DEFAULT 0", sinfo)
                    odata.runSp(cnn, "update abonoi set FolioAndroid = '0'", sinfo)
                    sinfo = ""
                End If
            End If


            cnn.Close()
        End If
    End Sub

    Private Sub Timer_reconecta_Tick(sender As Object, e As EventArgs) Handles Timer_reconecta.Tick
        Timer_reconecta.Stop()
        My.Application.DoEvents()
        If conecta() Then
            Timer_reconecta.Stop()
            Timer_datos.Start()
        Else
            Timer_reconecta.Start()
        End If
        My.Application.DoEvents()
    End Sub

    Private Sub Timer_datos_Tick(sender As Object, e As EventArgs) Handles Timer_datos.Tick

        Timer_datos.Stop()

        'grid_eventos.Rows.Insert(0, "Entro21", Date.Now)

        get_sucursales()
        My.Application.DoEvents()

        If Trim(sTargetdSincro) <> "" Then
            subeGrupos()
            My.Application.DoEvents()

            bajaGrupos()
            My.Application.DoEvents()

            subeDepartamentos()
            My.Application.DoEvents()

            bajaDepartamentos()
            My.Application.DoEvents()

            subeEmpleados()
            My.Application.DoEvents()

            bajaEliminarProd()
            My.Application.DoEvents()

            subeProductos()
            My.Application.DoEvents()

            bajaProductos()
            My.Application.DoEvents()

            subeExistencias()
            My.Application.DoEvents()

            bajaExistencias()
            My.Application.DoEvents()

            bajaPrecios()
            My.Application.DoEvents()

            bajaCompra()
            My.Application.DoEvents()

            bajaExitCompra()
            My.Application.DoEvents()

            bajaAbonoCompra()
            My.Application.DoEvents()

            subeCompra()
            My.Application.DoEvents()

            subeAbonoCompra()
            My.Application.DoEvents()

            subeTraspasoSalida()
            My.Application.DoEvents()

            bajaTraspasoSalida()
            My.Application.DoEvents()

            bajaExitTrasSalida()
            My.Application.DoEvents()

            bajaTraspasosEntrada()
            My.Application.DoEvents()

            bajaExitTrasEntrada()
            My.Application.DoEvents()

            buscaDevoluciones()
            My.Application.DoEvents()

            'busca_ventasFranquicia()
            'My.Application.DoEvents()

            busca_ventasl()
            My.Application.DoEvents()

            busca_abonos()
            My.Application.DoEvents()

        End If

        '''If Trim(sTargetdAutoFac) <> "" Then
        '''    My.Application.DoEvents()
        '''    subeVentasF()
        '''End If

        '''If Trim(sTargetdAndroid) <> "" Then

        '''    My.Application.DoEvents()
        '''    subeEmpleadosAndroid()

        '''    My.Application.DoEvents()
        '''    subeProductosAndroid()

        '''    My.Application.DoEvents()
        '''    subeClientesAndroid()

        '''    My.Application.DoEvents()
        '''    subeDatosTicketAndroid()

        '''    My.Application.DoEvents()
        '''    bajaClientesAndroid()

        '''    My.Application.DoEvents()
        '''    bajaVentasAndroid()

        '''    My.Application.DoEvents()
        '''    bajaActuVentasAndroid()

        '''    My.Application.DoEvents()
        '''    bajaAbonosAndroid()

        '''    My.Application.DoEvents()
        '''    subirVentasAndroid()

        '''    My.Application.DoEvents()
        '''    bajaTraspasosEntradaAndroid()

        '''    My.Application.DoEvents()
        '''    bajaPedidosAndroid()

        '''    My.Application.DoEvents()
        '''    eliminarPedidosNewAndroid()

        '''    My.Application.DoEvents()
        '''    eliminarPedidosAndroid()

        '''    My.Application.DoEvents()
        '''    subirPedidosAndroid()

        '''    My.Application.DoEvents()
        '''    eliminarPedidosNubeAndroid()

        '''    My.Application.DoEvents()
        '''    eliminarClienteNubeAndroid()

        '''    My.Application.DoEvents()
        '''    eliminarProductosNubeAndroid()

        '''    My.Application.DoEvents()
        '''    subirTraspasosAndroid()

        '''End If


        'My.Application.DoEvents()
        'grid_eventos.Rows.Insert(0, "Entro21", Date.Now)

        'My.Application.DoEvents()
        'subeProveedores()
        'My.Application.DoEvents()
        ' bajaProveedores()

        'My.Application.DoEvents()
        '  subeClientes()
        'My.Application.DoEvents()
        '  bajaClientes()


        Timer_datos.Start()

    End Sub
    Private Sub subeGrupos()
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from grupo where Cargado=0"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr1, "select * from grupos where Nombre = '" & dr("Nombre").ToString & "'", sinfo) Then
                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO grupos(nombre,cargado,NumSuc) " &
                                              " VALUES ('" & dr("Nombre").ToString & "',1," & susursalr & ")"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update grupo set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "'", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion grupo " & dr("Nombre").ToString, Date.Now)
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub subeDepartamentos()
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Departamentos where Cargado=0"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr1, "select * from departamentos where Nombre = '" & dr("Nombre").ToString & "'", sinfo) Then
                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO departamentos(nombre,cargado,NumSuc) " &
                                              " VALUES ('" & dr("Nombre").ToString & "',1," & susursalr & ")"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update departamentos Set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "'", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion departamento " & dr("Nombre").ToString, Date.Now)
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub subeEmpleados()
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Usuarios where Cargado=0"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr1, "select * from empleados where IdEmpleado = " & dr("idEmpleado").ToString & " and Sucursal = '" & susursalr & "' ", sinfo) Then
                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO empleados(Nombre, Alias, Clave, Sucursal, IdEmpleado) " &
                                              " VALUES ('" & dr("Nombre").ToString & "','" & dr("Alias").ToString & "','" & dr("Clave").ToString & "','" & susursalr &
                                              "','" & dr("idEmpleado").ToString & "')"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update Usuarios set Cargado = 1 where idEmpleado = " & dr("idEmpleado").ToString & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Empleado " & dr("Nombre").ToString, Date.Now)
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub bajaEliminarProd()
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from delprod where NumSuc = " & susursalr & ""
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, "Select * from delprod where NumSuc = " & susursalr & "", sinfo) Then
                        For Each dr In dt.Rows
                            My.Application.DoEvents()

                            If oData.getDr(cnn, dr2, "select Codigo from Productos where Nombre = '" & dr("Descripcion").ToString & "'", sinfo) Then
                                If oData.runSp(cnn, "delete from Productos where Codigo = '" & dr2("Codigo").ToString & "'", sinfo) Then
                                    odata2.runSp(cnn2, "delete from delprod where Id = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Eliminación del producto " & dr("Descripcion").ToString, Date.Now)
                                End If
                            Else
                                'MsgBox(sinfo)
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With
    End Sub

    Private Sub subeProductos()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Codigo,Nombre,IVA,UVenta,PrecioCompra,PorcMay,PorcMM,PorcMin,PorcEsp,Porcentaje,PreMin,PrecioVentaIVA,PreMay," &
                "PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin1,CantMay1,CantMM1,CantEsp1,CantLst1,CantMin2,CantMay2,CantMM2,CantEsp2," &
                "CantLst2,id_tbMoneda,Departamento,Grupo,Existencia,ClaveSat,UnidadSat,ProvPri,MCD,Multiplo,CodBarra,IIEPS,UCompra,Min,Max from Productos where Cargado=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim newExistt As Double = 0
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr2, "select * from productos where Codigo = '" & dr("Codigo").ToString & "' and NumSuc = " & susursalr & "", sinfo) Then

                            If dr("Codigo").ToString = dr2("Codigo").ToString Then

                                If CDec(dr("Multiplo").ToString) > 1 And CDec(dr("Existencia").ToString) > 0 Then
                                    newExistt = FormatNumber(CDec(dr("Existencia").ToString) / CDec(dr("Multiplo").ToString), 2)
                                Else
                                    newExistt = dr("Existencia").ToString
                                End If

                                ssqlinsertal = ""
                                ssqlinsertal = "update productos set exitencia = " & newExistt & ", clavesat = '" & dr("ClaveSat").ToString & "', claveunisat = '" & dr("UnidadSat").ToString & "', proveedor = '" & dr("ProvPri").ToString & "', IVA = '" & dr("IVA").ToString & "',UVenta = '" & dr("UVenta").ToString & "', PrecioCompra = '" & dr("PrecioCompra").ToString &
                                                                  "', PorcentageMin = " & dr("PorcMin").ToString & ", PorMay = " & dr("PorcMay").ToString & ", PorMM = " & dr("PorcMM").ToString &
                                                                  ", PorEsp = " & IIf(dr("PorcEsp").ToString > 0, dr("PorcEsp").ToString, 0) & ", Porcentage = '" & dr("Porcentaje").ToString & "', PecioVentaMinIVA = '" & dr("PreMin").ToString & "', PreMay = " & dr("PreMay").ToString &
                                                                  ", PreMM = " & dr("PreMM").ToString & ", PreEsp = " & dr("PreEsp").ToString & ", PrecioVentaIVA = '" & dr("PrecioVentaIVA").ToString & "', PrecioVenta = '" & dr("PrecioVenta").ToString &
                                                                  "', pres_vol = '" & dr("pres_vol").ToString & "', CantMin = " & dr("CantMin1").ToString & ", CantMay = " & dr("CantMay1").ToString & ", CantMM = " & dr("CantMM1").ToString &
                                                                  ", CantEsp = " & dr("CantEsp1").ToString & ", CantLta = " & dr("CantLst1").ToString & ", CantMin2 = " & dr("CantMin2").ToString & ", CantMay2 = " & dr("CantMay2").ToString &
                                                                  ", CantMM2 = " & dr("CantMM2").ToString & ", CantEsp2 = " & dr("CantEsp2").ToString & ", CantLta2 = " & dr("CantLst2").ToString & ", MCD = " & IIf(IsNumeric(dr("MCD").ToString), dr("MCD").ToString, 1) & ", Multiplo = " & IIf(IsNumeric(dr("Multiplo").ToString), dr("Multiplo").ToString, 1) & ", CodBarra = '" & dr("CodBarra").ToString & "', IIEPS = " & IIf(IsNumeric(dr("IIEPS").ToString), dr("IIEPS").ToString, 0) &
                                                                  ",Depto='" & Replace(dr("Departamento").ToString, "Ã'", "Ñ") & "',Grupo='" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "', UCompra = '" & dr("UCompra").ToString & "', `Min` = " & dr("Min").ToString & ", `Max` = " & dr("Max").ToString & " where Codigo = '" & dr("Codigo").ToString & "' and NumSuc = " & susursalr & ""
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                    '"select * from Formatos where Facturas='ACTSINEXIPRE' AND NotasCred='1'"
                                    If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTSINEXIPRE'", sinfo) Then
                                        If dr2("NotasCred").ToString = "1" Then
                                            ExiteProductoSucACTSINEXIPRE(dr("Codigo").ToString, dr("Nombre").ToString)
                                        End If
                                        'Else
                                        '    MsgBox("no paso")
                                    End If

                                    If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTPROPRE'", sinfo) Then
                                        If dr2("NotasCred").ToString = "1" Then
                                            ACTPROEXISUC(dr("Codigo").ToString, dr("Nombre").ToString)
                                        End If
                                        'Else
                                        '    MsgBox("no paso")
                                    End If

                                    odata.runSp(cnn, "update Productos set Cargado = 1, CargadoInv = 1 where Codigo = '" & dr("Codigo").ToString & "'", sinfo)
                                End If

                                grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Precio " & dr("Nombre").ToString, Date.Now)

                            Else

                                If CDec(dr("Multiplo").ToString) > 1 And CDec(dr("Existencia").ToString) > 0 Then
                                    newExistt = FormatNumber(CDec(dr("Existencia").ToString) / CDec(dr("Multiplo").ToString), 2)
                                Else
                                    newExistt = dr("Existencia").ToString
                                End If

                                ssqlinsertal = ""
                                ssqlinsertal = "update productos set exitencia = " & newExistt & ", clavesat = '" & dr("ClaveSat").ToString & "', claveunisat = '" & dr("UnidadSat").ToString & "', proveedor = '" & dr("ProvPri").ToString & "', IVA = '" & dr("IVA").ToString & "',UVenta = '" & dr("UVenta").ToString & "', PrecioCompra = '" & dr("PrecioCompra").ToString &
                                                                  "', PorcentageMin = '" & dr("PorcMin").ToString & "', PorMay = " & dr("PorMay").ToString & ", PorMM = " & dr("PorMM").ToString &
                                                                  ", PorEsp = " & IIf(dr("PorEsp").ToString > 0, dr("PorEsp").ToString, 0) & ", Porcentage = '" & dr("Porcentaje").ToString & "', PecioVentaMinIVA = '" & dr("PreMin").ToString & "', PreMay = " & dr("PreMay").ToString &
                                                                  ", PreMM = " & dr("PreMM").ToString & ", PreEsp = " & dr("PreEsp").ToString & ", PrecioVentaIVA = '" & dr("PrecioVentaIVA").ToString & "', PrecioVenta = '" & dr("PrecioVenta").ToString &
                                                                  "', pres_vol = '" & dr("pres_vol").ToString & "', CantMin = " & dr("CantMin1").ToString & ", CantMay = " & dr("CantMay1").ToString & ", CantMM = " & dr("CantMM1").ToString &
                                                                  ", CantEsp = " & dr("CantEsp1").ToString & ", CantLta = " & dr("CantLst1").ToString & ", CantMin2 = " & dr("CantMin2").ToString & ", CantMay2 = " & dr("CantMay2").ToString &
                                                                  ", CantMM2 = " & dr("CantMM2").ToString & ", CantEsp2 = " & dr("CantEsp2").ToString & ", CantLta2 = " & dr("CantLst2").ToString & ", MCD = " & IIf(IsNumeric(dr("MCD").ToString), dr("MCD").ToString, 1) & ", Multiplo = " & IIf(IsNumeric(dr("Multiplo").ToString), dr("Multiplo").ToString, 1) & ", CodBarra = '" & dr("CodBarra").ToString & "', IIEPS = " & IIf(IsNumeric(dr("IIEPS").ToString), dr("IIEPS").ToString, 0) &
                                                                  ",Depto='" & Replace(dr("Departamento").ToString, "Ã'", "Ñ") & "',Grupo='" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "', UCompra = '" & dr("UCompra").ToString & "', `Min` = " & dr("Min").ToString & ", `Max` = " & dr("Max").ToString & " where Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & ""
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                    '"select * from Formatos where Facturas='ACTSINEXIPRE' AND NotasCred='1'"
                                    If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTSINEXIPRE'", sinfo) Then
                                        If dr2("NotasCred").ToString = "1" Then
                                            ExiteProductoSucACTSINEXIPRE(dr("Codigo").ToString, dr("Nombre").ToString)
                                        End If
                                        'Else
                                        '    MsgBox("no paso")
                                    End If

                                    If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTPROPRE'", sinfo) Then
                                        If dr2("NotasCred").ToString = "1" Then
                                            ACTPROEXISUC(dr("Codigo").ToString, dr("Nombre").ToString)
                                        End If
                                        'Else
                                        '    MsgBox("no paso")
                                    End If

                                    odata.runSp(cnn, "update Productos set Cargado = 1, CargadoInv = 1 where Codigo = '" & dr("Codigo").ToString & "'", sinfo)
                                End If

                                grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Precio " & dr("Nombre").ToString, Date.Now)

                            End If

                        Else

                            If CDec(dr("Multiplo").ToString) > 1 And CDec(dr("Existencia").ToString) > 0 Then
                                newExistt = FormatNumber(CDec(dr("Existencia").ToString) / CDec(dr("Multiplo").ToString), 2)
                            Else
                                newExistt = dr("Existencia").ToString
                            End If

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO productos(Codigo,Nombre,IVA,UVenta,PrecioCompra,PorMay,PorMM,PorEsp,Porcentage,PecioVentaMinIVA,PreMay," &
                                                            "PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin,CantMay,CantMM,CantEsp,CantLta,CantMin2,CantMay2,CantMM2,CantEsp2," &
                                                            "CantLta2,id_tbMoneda,NumSuc,Depto,Grupo,proveedor,exitencia,clavesat,claveunisat,MCD,Multiplo,CodBarra,IIEPS,UCompra,Min,Max) " &
                                                            " VALUES ('" & dr("Codigo").ToString & "','" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") & "','" & dr("IVA").ToString & "','" & dr("UVenta").ToString &
                                                            "','" & dr("PrecioCompra").ToString & "'," & dr("PorcMay").ToString & "," & dr("PorcMM").ToString &
                                                            "," & dr("PorcEsp").ToString & ",'" & dr("Porcentaje").ToString & "','" & dr("PreMin").ToString & "'," & dr("PreMay").ToString &
                                                            "," & dr("PreMM").ToString & "," & dr("PreEsp").ToString & ",'" & dr("PrecioVentaIVA").ToString & "','" & dr("PrecioVenta").ToString &
                                                            "','" & dr("pres_vol").ToString & "'," & dr("CantMin1").ToString & "," & dr("CantMay1").ToString & "," & dr("CantMM1").ToString &
                                                            "," & dr("CantEsp1").ToString & "," & dr("CantLst1").ToString & "," & dr("CantMin2").ToString & "," & dr("CantMay2").ToString &
                                                            "," & dr("CantMM2").ToString & "," & dr("CantEsp2").ToString & "," & dr("CantLst2").ToString & ",1," & susursalr & ",'" &
                                                            Replace(dr("Departamento").ToString, "Ã'", "Ñ") & "','" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "','" & dr("ProvPri").ToString & "'," & newExistt & ",'" & dr("ClaveSat").ToString & "','" & dr("UnidadSat").ToString &
                                                            "'," & IIf(IsNumeric(dr("MCD").ToString), dr("MCD").ToString, 1) & "," & IIf(IsNumeric(dr("Multiplo").ToString), dr("Multiplo").ToString, 1) & ",'" & dr("CodBarra").ToString & "'," &
                                                            IIf(IsNumeric(dr("IIEPS").ToString), dr("IIEPS").ToString, 0) & ",'" & dr("UCompra").ToString & "'," & dr("Min").ToString & "," & dr("Max").ToString & ")"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                '"select * from Formatos where Facturas='ACTSINEXIPRE' AND NotasCred='1'"
                                If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTSINEXIPRE'", sinfo) Then
                                    If dr2("NotasCred").ToString = "1" Then
                                        ExiteProductoSucACTSINEXIPRE(dr("Codigo").ToString, dr("Nombre").ToString)
                                    End If
                                    'Else
                                    '    MsgBox("no paso")
                                End If

                                If odata.getDr(cnn, dr2, "select NotasCred from Formatos where Facturas='ACTPROPRE'", sinfo) Then
                                    If dr2("NotasCred").ToString = "1" Then
                                        ACTPROEXISUC(dr("Codigo").ToString, dr("Nombre").ToString)
                                    End If
                                    'Else
                                    '    MsgBox("no paso")
                                End If

                                If odata.runSp(cnn, "update Productos set Cargado = 1, CargadoInv = 1 where Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then
                                Else
                                    MsgBox(sinfo)
                                End If
                            Else
                                MsgBox(sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr("Nombre").ToString, Date.Now)


                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaProductos()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from productos where Cargado='1' and NumSuc = " & susursalr & ""

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()
                        If odata.getDr(cnn, dr2, "SELECT * FROM Productos WHERE Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then
                            Dim newExistt As Double = 0
                            If CDec(dr2("Multiplo").ToString) > 1 And CDec(dr("Exitencia").ToString) > 0 Then
                                newExistt = FormatNumber(CDec(dr("Exitencia").ToString) * CDec(dr2("Multiplo").ToString), 2)
                            Else
                                newExistt = dr("Exitencia").ToString
                            End If

                            ssqlinsertal = ""
                            ssqlinsertal = "update productos set Nombre = '" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") & "',Existencia='" & newExistt & "', ProvPri = '" & dr("proveedor").ToString & "', UVenta = '" & dr("UVenta").ToString & "', Departamento = '" & Replace(dr("Depto").ToString, "Ã'", "Ñ") & "', Grupo = '" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "', PrecioCompra = '" & dr("PrecioCompra").ToString & "', PrecioVentaIVA = '" & dr("PrecioVentaIVA").ToString & "', IVA = '" & dr("IVA").ToString & "', ClaveSat = '" & dr("clavesat").ToString & "', UnidadSat = '" & dr("claveunisat").ToString & "',MCD = " & IIf(IsNumeric(dr("MCD").ToString), IIf(dr("MCD").ToString = 0, 1, dr("MCD").ToString), 1) & ", Multiplo = " & IIf(IsNumeric(dr("Multiplo").ToString), IIf(dr("Multiplo").ToString = 0, 1, dr("Multiplo").ToString), 1) & ", CodBarra = '" & dr("CodBarra").ToString & "', IIEPS = " & IIf(IsNumeric(dr("IIEPS").ToString), dr("IIEPS").ToString, 0) & ", PorcMay = " & dr("PorMay").ToString & ", PorcMM = " & dr("PorMM").ToString & ", PorcEsp = " & dr("PorEsp").ToString & ", PreMay = " & dr("PreMay").ToString & ", PreMM = " & dr("PreMM").ToString & ", PreEsp = " & dr("PreEsp").ToString & ", CantMin1 = " & dr("CantMin").ToString & ", CantMay1 = " & dr("CantMay").ToString & ", CantMM1 = " & dr("CantMM").ToString & ", CantEsp1 = " & dr("CantEsp").ToString & ", CantLst1 = " & dr("CantLta").ToString & ", CantMin2 = " & dr("CantMin2").ToString & ", CantMay2 = " & dr("CantMay2").ToString & ", CantMM2 = " & dr("CantMM2").ToString & ", CantEsp2 = " & dr("CantEsp2").ToString & ", CantLst2 = " & dr("CantLta2").ToString & ", PorcMin = " & dr("PorcentageMin").ToString & ", Porcentaje = " & dr("Porcentage").ToString & ",Min = " & dr("Min").ToString & ", Max = " & dr("Max").ToString & ",PreMin = '" & dr("PecioVentaMinIVA").ToString & "', Ubicacion = '" & dr("Ubicacion").ToString & "', Anti = " & dr("Antibiotico").ToString & ", Caduca = " & dr("Caduca").ToString & ", PrecioMaximoPublico = " & dr("PrecioP").ToString & ", Laboratorio = '" & dr("LaboMar").ToString & "', PrincipioActivo = '" & dr("PrincipioAct").ToString & "', Controlado = " & dr("Controlado").ToString & " where Codigo = '" & dr("Codigo").ToString & "'"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update productos set Cargado = 0 where Codigo = '" & dr("Codigo").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            Else
                                MsgBox(sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr("Nombre").ToString, Date.Now)


                        Else

                            ssqlinsertal = "Insert Into Productos(Codigo,Nombre,ProvPri,ProvRes,UCompra,UVenta,UMinima,Departamento,Grupo,PrecioCompra,PorcMin,Porcentaje,PrecioVenta,PrecioVentaIVA,PreMin,IVA,Existencia,id_tbMoneda,PercentIVAret,NombreLargo,isr,ClaveSat,UnidadSat,N_Serie,MCD,Multiplo,CodBarra,IIEPS,almacen3,Porcentaje_Promo,PorcMay,PorcMM,PorcEsp,PreMay,PreMM,PreEsp,CantMin1,CantMay1,CantMM1,CantEsp1,CantLst1,CantMin2,CantMay2,CantMM2,CantEsp2,CantLst2,Min,Max,Fecha_Inicial,Fecha_Final,Fecha,Ubicacion,Anti,Caduca,PrecioMaximoPublico,Laboratorio,PrincipioActivo,Controlado) " &
                              "VALUES('" & dr("Codigo").ToString & "','" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") & "','" & dr("proveedor").ToString & "',0,'" & dr("UCompra").ToString & "','" & dr("UVenta").ToString & "','" & dr("VentaMin").ToString & "','" & Replace(dr("Depto").ToString, "Ã'", "Ñ") & "','" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "','" & dr("PrecioCompra").ToString & "','" & dr("PorcentageMin").ToString & "','" & dr("Porcentage").ToString & "','0','" & dr("PrecioVentaIVA").ToString & "','" & dr("PecioVentaMinIVA").ToString & "','" & dr("IVA").ToString & "'," & dr("exitencia").ToString & ",1,0,'',0,'" & dr("clavesat").ToString & "','" & dr("claveunisat").ToString & "',0, " & IIf(IsNumeric(dr("MCD").ToString), IIf(dr("MCD").ToString = 0, 1, dr("MCD").ToString), 1) & "," & IIf(IsNumeric(dr("Multiplo").ToString), IIf(dr("Multiplo").ToString = 0, 1, dr("Multiplo").ToString), 1) & ",'" & dr("CodBarra").ToString & "'," & IIf(IsNumeric(dr("IIEPS").ToString), dr("IIEPS").ToString, 0) & "," & dr("PrecioCompra").ToString & ",0," & dr("PorMay").ToString & "," & dr("PorMM").ToString & "," & dr("PorEsp").ToString & "," & dr("PreMay").ToString & "," & dr("PreMM").ToString & "," & dr("PreEsp").ToString & "," & dr("CantMin").ToString & "," & dr("CantMay").ToString & "," & dr("CantMM").ToString & "," & dr("CantEsp").ToString & "," & dr("CantLta").ToString & "," & dr("CantMin2").ToString & "," & dr("CantMay2").ToString & "," & dr("CantMM2").ToString & "," & dr("CantEsp2").ToString & "," & dr("CantLta2").ToString & "," & dr("Min").ToString & "," & dr("Max").ToString & ",'" & Format(Date.Now, "yyyy-MM-dd") & "','" & Format(Date.Now, "yyyy-MM-dd") & "','" & Format(Date.Now, "yyyy-MM-dd") & "','" & dr("Ubicacion").ToString & "'," & dr("Antibiotico").ToString & "," & dr("Caduca").ToString & "," & dr("PrecioP").ToString & ",'" & dr("LaboMar").ToString & "','" & dr("PrincipioAct").ToString & "'," & dr("Controlado").ToString & ")"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update productos set Cargado = 0 where Codigo = '" & dr("Codigo").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr("Nombre").ToString, Date.Now)

                        End If
                    Next
                Else

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaGrupos()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from grupos where Cargado=0 and NumSuc = " & susursalr & ""
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows
                        My.Application.DoEvents()
                        If odata.getDr(cnn, dr2, "SELECT * FROM grupo WHERE nombre = '" & dr("Nombre").ToString & "'", sinfo) Then
                            ssqlinsertal = ""
                            ssqlinsertal = "update grupo set Nombre = '" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") & "',Cargado=1 where Nombre = '" & dr("Nombre").ToString & "'"
                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update grupos set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            Else
                                MsgBox(sinfo)
                            End If
                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Grupo " & dr("Nombre").ToString, Date.Now)
                        Else
                            ssqlinsertal = "Insert Into Grupo(Nombre,Cargado) " &
                              "VALUES('" & dr("Nombre").ToString & "',1)"
                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update grupos set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            End If
                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Grupo " & dr("Nombre").ToString, Date.Now)
                        End If
                    Next
                Else
                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub bajaDepartamentos()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from departamentos where Cargado=0 and NumSuc = " & susursalr & ""
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows
                        My.Application.DoEvents()
                        If odata.getDr(cnn, dr2, "SELECT * FROM departamentos WHERE nombre = '" & dr("Nombre").ToString & "'", sinfo) Then
                            ssqlinsertal = ""
                            ssqlinsertal = "update departamentos set Nombre = '" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") & "' where Nombre = '" & dr("Nombre").ToString & "'"
                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update departamentos set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            Else
                                MsgBox(sinfo)
                            End If
                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Departamento " & dr("Nombre").ToString, Date.Now)
                        Else
                            ssqlinsertal = "Insert Into Departamentos(Nombre,Cargado) " &
                              "VALUES('" & dr("Nombre").ToString & "',1)"
                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update departamentos set Cargado = 1 where Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                            End If
                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Departamento " & dr("Nombre").ToString, Date.Now)
                        End If
                    Next
                Else
                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub subeExistencias()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from Productos where CargadoInv ='0'"
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If .getDt(cnn, dt, sSQL, sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()
                            ssql2 = "Select * from productos where NumSuc=" & susursalr & " and Codigo='" & dr("Codigo").ToString & "'"
                            If odata2.getDr(cnn2, dr2, ssql2, sinfo) Then

                                Dim newExistt As Double = 0
                                If CDec(dr("Multiplo").ToString) > 1 And CDec(dr("Existencia").ToString) > 0 Then
                                    newExistt = FormatNumber(CDec(dr("Existencia").ToString) / CDec(dr("Multiplo").ToString), 2)
                                Else
                                    newExistt = dr("Existencia").ToString
                                End If

                                ssql3 = "update productos set exitencia=" & newExistt & " where id=" & dr2("Id").ToString
                                If odata2.runSp(cnn2, ssql3, sinfo) Then
                                    oData.runSp(cnn, "update Productos set CargadoInv = 1 where Codigo ='" & dr("Codigo").ToString & "'", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Inventario " & dr("Nombre").ToString, Date.Now)
                                End If
                            Else
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With
    End Sub

    Private Sub bajaExistencias()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from actuinv where NumSuc = " & susursalr & ""
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, "Select * from actuinv where NumSuc = " & susursalr & "", sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()

                            If oData.getDr(cnn, dr2, "select Codigo,Existencia,Multiplo from Productos where Codigo = '" & dr("Codigo").ToString & "'", "drDOS") Then

                                MyExist = 0
                                If CDec(dr2("Multiplo").ToString) > 1 And CDec(dr2("Existencia").ToString) > 0 Then
                                    MyExist = FormatNumber(CDec(dr2("Existencia").ToString), 2)
                                    MyNewEsist = CDec(MyExist) - CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString))
                                Else
                                    MyExist = dr2("Existencia").ToString
                                    MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                End If

                                If oData.runSp(cnn, "update Productos set Existencia = " & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & " where Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then

                                    'ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cant_Prod,Precio_prod,fecha,Usuario,Existencia,Diferencia,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Ajuste Inventario Nube'," & MyNewEsist & ",'0','" & Now & "','Nube','" & MyExist & "','" & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & "','')"
                                    ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Ajuste Inventario Nube'," & MyNewEsist & ",'0', '" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & "','')"

                                    oData.runSp(cnn, ssql3, sinfo)

                                    odata2.runSp(cnn2, "delete from actuinv where Id = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)
                                End If
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With

    End Sub

    Private Sub bajaPrecios()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from actuprecios where NumSuc = " & susursalr & ""
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim driva As DataRow

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()
                            If oData.getDr(cnn, dr2, "select Codigo from Productos where Codigo = '" & dr("codigo").ToString & "'", "drDOS") Then
                                Dim ope As Double = 0

                                Select Case dr("tipo").ToString
                                    Case 1 'precio compra
                                        If oData.runSp(cnn, "update Productos set PrecioCompra = " & dr("precio").ToString & ", Almacen3 = " & dr("precio").ToString & " where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then
                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                    Case 2 'precio minimo
                                        If oData.runSp(cnn, "update Productos set PreMin = " & dr("precio").ToString & ", CantMin1 = " & dr("min").ToString & ", CantMin2 = " & dr("max").ToString & "  where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then

                                            oData.getDr(cnn, driva, "select * from Productos where Codigo = '" & dr("codigo").ToString & "'", "drUno")
                                            If CDec(driva("IVA").ToString) > 0 Then
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString) * 1.16, 2)
                                            Else
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString), 2)
                                            End If

                                            If CDec(ope) > 0 Then
                                                ope = FormatNumber(CDec(CDec(CDec(driva("PreMin").ToString) * 100) / ope) - 100, 2)
                                            Else
                                                ope = 0
                                            End If

                                            If ope < 0 Then
                                                ope = 0
                                            End If

                                            oData.runSp(cnn, "update Productos set PorcMin = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)

                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                    Case 3 'precio med mayoreo
                                        If oData.runSp(cnn, "update Productos set PreMM = " & dr("precio").ToString & ", CantMM1 = " & dr("min").ToString & ", CantMM2 = " & dr("max").ToString & "  where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then

                                            oData.getDr(cnn, driva, "select * from Productos where Codigo = '" & dr("codigo").ToString & "'", "drUno")
                                            If CDec(driva("IVA").ToString) > 0 Then
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString) * 1.16, 2)
                                            Else
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString), 2)
                                            End If

                                            If CDec(ope) > 0 Then
                                                ope = FormatNumber(CDec(CDec(CDec(driva("PreMM").ToString) * 100) / ope) - 100, 2)
                                            Else
                                                ope = 0
                                            End If

                                            If ope < 0 Then
                                                ope = 0
                                            End If

                                            oData.runSp(cnn, "update Productos set PorcMM = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)

                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                    Case 4 'precio mayoreo
                                        If oData.runSp(cnn, "update Productos set PreMay = " & dr("precio").ToString & ", CantMay1 = " & dr("min").ToString & ", CantMay2 = " & dr("max").ToString & "  where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then

                                            oData.getDr(cnn, driva, "select * from Productos where Codigo = '" & dr("codigo").ToString & "'", "drUno")
                                            If CDec(driva("IVA").ToString) > 0 Then
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString) * 1.16, 2)
                                            Else
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString), 2)
                                            End If

                                            If CDec(ope) > 0 Then
                                                ope = FormatNumber(CDec(CDec(CDec(driva("PreMay").ToString) * 100) / ope) - 100, 2)
                                            Else
                                                ope = 0
                                            End If

                                            If ope < 0 Then
                                                ope = 0
                                            End If

                                            oData.runSp(cnn, "update Productos set PorcMay = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)

                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                    Case 5 'precio kits
                                        If oData.runSp(cnn, "update Productos set PreEsp = " & dr("precio").ToString & ", CantEsp1 = " & dr("min").ToString & ", CantEsp2 = " & dr("max").ToString & "  where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then

                                            oData.getDr(cnn, driva, "select * from Productos where Codigo = '" & dr("codigo").ToString & "'", "drUno")
                                            If CDec(driva("IVA").ToString) > 0 Then
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString) * 1.16, 2)
                                            Else
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString), 2)
                                            End If

                                            If CDec(ope) > 0 Then
                                                ope = FormatNumber(CDec(CDec(CDec(driva("PreEsp").ToString) * 100) / ope) - 100, 2)
                                            Else
                                                ope = 0
                                            End If

                                            If ope < 0 Then
                                                ope = 0
                                            End If

                                            oData.runSp(cnn, "update Productos set PorcEsp = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)

                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                    Case 6 'precio publico general
                                        If oData.runSp(cnn, "update Productos set PrecioVentaIVA = " & dr("precio").ToString & ", CantLst1 = " & dr("min").ToString & ", CantLst2 = " & dr("max").ToString & "  where Codigo = '" & dr("codigo").ToString & "'", sinfo) Then

                                            oData.getDr(cnn, driva, "select * from Productos where Codigo = '" & dr("codigo").ToString & "'", "drUno")
                                            If CDec(driva("IVA").ToString) > 0 Then
                                                ope = FormatNumber(CDec(driva("PrecioVentaIVA").ToString) / 1.16, 2)
                                                oData.runSp(cnn, "update Productos set PrecioVenta = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString) * 1.16, 2)
                                            Else
                                                ope = FormatNumber(CDec(driva("PrecioVentaIVA").ToString) / 1, 2)
                                                oData.runSp(cnn, "update Productos set PrecioVenta = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)
                                                ope = FormatNumber(CDec(driva("PrecioCompra").ToString), 2)
                                            End If

                                            If CDec(ope) > 0 Then
                                                ope = FormatNumber(CDec(CDec(CDec(driva("PrecioVentaIVA").ToString) * 100) / ope) - 100, 2)
                                            Else
                                                ope = 0
                                            End If

                                            If ope < 0 Then
                                                ope = 0
                                            End If

                                            oData.runSp(cnn, "update Productos set Porcentaje = '" & ope & "' where Codigo = '" & dr("codigo").ToString & "'", sinfo)

                                            odata2.runSp(cnn2, "delete from actuprecios where Id = " & dr("Id").ToString & "", sinfo)
                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Precio " & dr("descripcion").ToString, Date.Now)
                                        End If
                                End Select
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With

    End Sub

    Private Sub bajaCompra()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from compras where Bajado=0 and NumSuc = " & susursalr & ""
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdCompra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Bajando Compra folio " & dr("IdCompra").ToString, Date.Now)
                        My.Application.DoEvents()

                        If odata.getDr(cnn, dr3, "select * from Compras where NumRemision = '" & dr("NumRemision").ToString & "' and NumFactura = '" & dr("NumFactura").ToString & "' and Proveedor = '" & dr("Proveedor").ToString & "'", "druno") Then

                            ssqlinsertal = "update compras set Cargado = 1, Acuenta = '" & Replace(dr("Acuenta").ToString, ",", "") & "', Resta = '" & Replace(dr("Resta").ToString, ",", "") & "', Status = '" & dr("Status").ToString & "' where Id = " & dr3("Id").ToString

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                                ssql3 = "update compras set Bajado=1 where IdCompra=" & dr("IdCompra").ToString
                                If odata2.runSp(cnn2, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Compra folio " & dr("IdCompra").ToString, Date.Now)
                                End If

                            End If

                        Else

                            ssqlinsertal = "INSERT INTO compras(NumFactura, NumRemision, Proveedor, Desc1, Sub1, IVA, Total, Desc2, Pagar, Acuenta, Resta," &
                                          " FechaC, FechaP, Status, Usuario,Cargado) VALUES ('" & dr("NumFactura").ToString & "','" & dr("NumRemision").ToString &
                                          "','" & dr("Proveedor").ToString & "','0','" & Replace(dr("Subtotal").ToString, ",", "") & "','" & Replace(dr("IVA").ToString, ",", "") & "','" & Replace(dr("Total").ToString, ",", "") &
                                          "','0','" & Replace(dr("TotalPagar").ToString, ",", "") & "','" & Replace(dr("Acuenta").ToString, ",", "") & "','" & Replace(dr("Resta").ToString, ",", "") &
                                          "','" & dr("FCompra").ToString & "','" & dr("FPago").ToString & "','" & dr("Status").ToString & "','" & dr("Usuario").ToString &
                                          "',1)"

                            If odata.getDr(cnn, dr2, "select Id from Proveedores where NComercial = '" & dr("Proveedor").ToString & "' or Compania = '" & dr("Proveedor").ToString & "'", "druno") Then
                            Else
                                odata.runSp(cnn, "insert into Proveedores(NComercial,Compania) values('" & dr("Proveedor").ToString & "','" & dr("Proveedor").ToString & "')", sinfo)
                            End If

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                                odata.getDr(cnn, dr2, "select max(Id) as XD from Compras", "drdos")

                                maxIdCompra = dr2(0).ToString


                                bajaCompraDetalle(dr("IdCompra").ToString, maxIdCompra)

                                ssql3 = "update compras set Bajado=1 where IdCompra=" & dr("IdCompra").ToString
                                If odata2.runSp(cnn2, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Compra folio " & dr("IdCompra").ToString, Date.Now)

                                    NotifyIcon2.Visible = True
                                    NotifyIcon2.BalloonTipIcon = ToolTipIcon.Info
                                    NotifyIcon2.BalloonTipTitle = "Compra / Venta Franquicia recibida correctamente"
                                    NotifyIcon2.BalloonTipText = "Se han recibido una compra con los productos: " & productosxd & " correctamente"
                                    Dim soundplayer As New SoundPlayer("C:\ControlNegociosPro\sonido.wav")
                                    soundplayer.Play()

                                    NotifyIcon2.ShowBalloonTip(9000)
                                End If
                            Else
                                MsgBox(sinfo)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub bajaCompraDetalle(ByVal Folio As String, ByVal maxId As String)

        Dim cnn3 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn4 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from comprasdetalle where Compra_id=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim d3 As DataRow
        Dim dr4 As DataRow
        Dim sinfo As String = ""
        Dim odata3 As New ToolKitSQL.myssql
        Dim odata4 As New ToolKitSQL.myssql

        If odata3.dbOpen(cnn3, sTargetlocal, sinfo) Then
            If odata4.dbOpen(cnn4, sTargetdSincro, sinfo) Then

                If odata4.getDt(cnn4, dt4, sSQL, sinfo) Then
                    For Each dr4 In dt4.Rows

                        Dim IdCompra As Integer = 0
                        Dim d25 As DataRow
                        If odata3.getDr(cnn3, d25, "select Id from Compras where NumRemision='" & dr4("NumRemision").ToString & "' and Proveedor='" & dr4("Proveedor").ToString & "'", sinfo) Then
                            IdCompra = d25("Id").ToString
                        End If

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO ComprasDet(Id_Compra, Proveedor, NumFactura, NumRemision, Codigo, Nombre, UCompra, Cantidad, Precio, Total, FechaC, Grupo, Depto,Caducidad,Lote,FolioRep,NotaCred)" &
                                        " VALUES (" & IdCompra & ",'" & dr4("Proveedor").ToString & "','" & dr4("NumFactura").ToString & "','" & dr4("NumRemision").ToString & "','" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString &
                                        "','" & dr4("UCompra").ToString & "','" & dr4("Cantidad").ToString & "','" & dr4("Precio").ToString & "','" & dr4("Total").ToString & "','" & dr4("Fecha").ToString & "','" & dr4("Grupo").ToString &
                                        "','" & dr4("Depto").ToString & "','','',0,'')"

                        If odata3.runSp(cnn3, ssqlinsertal, sinfo) Then
                            If productosxd = "" Then
                                productosxd = dr4("Nombre").ToString
                            Else
                                productosxd = productosxd & ", " & dr4("Nombre").ToString
                            End If
                        Else
                            MsgBox(sinfo)
                        End If
                    Next
                End If
                cnn4.Close()
            End If
            cnn3.Close()
        End If

    End Sub

    Private Sub bajaExitCompra()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from actuinvcompras where NumSuc = " & susursalr & ""
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, "Select * from actuinvcompras where NumSuc = " & susursalr & "", sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()

                            If oData.getDr(cnn, dr2, "select Codigo,Existencia,Multiplo from Productos where Codigo = '" & dr("Codigo").ToString & "'", "drDOS") Then

                                MyExist = 0
                                If CDec(dr2("Multiplo").ToString) > 1 And CDec(dr2("Existencia").ToString) > 0 Then
                                    MyExist = FormatNumber(CDec(dr2("Existencia").ToString), 2)
                                    MyNewEsist = CDec(MyExist) + CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString))
                                Else
                                    MyExist = dr2("Existencia").ToString
                                    MyNewEsist = CDec(MyExist) + CDec(dr("Cantidad").ToString)
                                End If

                                If oData.runSp(cnn, "update Productos set CargadoInv = 0, Existencia = Existencia + " & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & " where Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then

                                    ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Ingreso por Compra Nube'," & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"

                                    oData.runSp(cnn, ssql3, sinfo)

                                    odata2.runSp(cnn2, "delete from actuinvcompras where Id = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)
                                End If
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With

    End Sub

    Private Sub bajaAbonoCompra()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from actuAbonoCompras where Bajado=0 and NumSuc = " & susursalr & ""
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdCompra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            Dim idprov As Integer = 0
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()
                        If odata.getDt(cnn, dt2, "select * from Proveedores where Compania ='" & dr("Proveedor").ToString & "' or NComercial ='" & dr("Proveedor").ToString & "'", sinfo) Then
                            For Each dr2 In dt2.Rows
                                idprov = dr2("Id").ToString
                            Next
                        End If
                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Bajando Abono Compra " & dr("IdCompra").ToString, Date.Now)
                        My.Application.DoEvents()


                        If dr("FechaCheque").ToString = "" Then
                            ssqlinsertal = "INSERT INTO AbonoE(NumFactura, NumRemision, IdProv,Proveedor, Concepto, Fecha, Cargo, Abono, Saldo, Efectivo, Usuario,Cargado,Tarjeta,Transfe,Banco" &
                                          ") VALUES ('" & dr("NumFactura").ToString & "','" & dr("NumRemision").ToString & "'," & idprov & ",'" & dr("Proveedor").ToString & "','" & dr("Concepto").ToString & "','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Replace(dr("Cargo").ToString, ",", "") & "','" & Replace(dr("Abono").ToString, ",", "") & "','" & Replace(dr("Saldo").ToString, ",", "") & "','" & Replace(dr("MontoEfec").ToString, ",", "") &
                                          "','NUBE',1,'" & dr("MontoTarjeta").ToString & "','" & dr("MontoTrasferencia").ToString & "','" & dr("Banco").ToString & "')"
                        Else
                            ssqlinsertal = "INSERT INTO AbonoE(NumFactura, NumRemision,IdProv Proveedor, Concepto, Fecha, Cargo, Abono, Saldo, Efectivo, Usuario,Cargado,Tarjeta,Transfe,Banco" &
                                          ") VALUES ('" & dr("NumFactura").ToString & "','" & dr("NumRemision").ToString & "', " & idprov & ",'" & dr("Proveedor").ToString & "','" & dr("Concepto").ToString & "','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Replace(dr("Cargo").ToString, ",", "") & "','" & Replace(dr("Abono").ToString, ",", "") & "','" & Replace(dr("Saldo").ToString, ",", "") & "','" & Replace(dr("MontoEfec").ToString, ",", "") &
                                          "','NUBE',1,'" & dr("MontoTarjeta").ToString & "','" & dr("MontoTrasferencia").ToString & "','" & dr("Banco").ToString & "')"
                        End If

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                            ssql3 = "update actuAbonoCompras set Bajado=1 where Id=" & dr("Id").ToString
                            If odata2.runSp(cnn2, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Abono Compra " & dr("IdCompra").ToString, Date.Now)
                            End If
                        Else
                            MsgBox(sinfo)
                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If


    End Sub

    Private Sub subeCompra()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Compras where Cargado=0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdCompra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata.getDt(cnn, dt, sSQL, "dtdos") Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "subida Compra folio " & dr("Id").ToString, Date.Now)
                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr3, "select * from compras where NumRemision = '" & dr("NumRemision").ToString & "' and NumFactura = '" & dr("NumFactura").ToString & "' and Proveedor = '" & dr("Proveedor").ToString & "' and NumSuc = " & susursalr & "", sinfo) Then

                            ssqlinsertal = "update compras set Acuenta = '" & Replace(dr("Acuenta").ToString, ",", "") & "', Resta = '" & Replace(dr("Resta").ToString, ",", "") & "', Status = '" & dr("Status").ToString & "' where IdCompra = " & dr3("IdCompra").ToString & " and NumSuc = " & susursalr & ""

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                ssql3 = "update Compras set Cargado=1 where id=" & dr("Id").ToString
                                If odata.runSp(cnn, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Compra folio " & dr("Id").ToString, Date.Now)
                                End If
                            End If

                        Else


                            ssqlinsertal = "INSERT INTO compras(id,NumFactura, NumRemision, Proveedor, Descuento1, Subtotal, IVA, Total, Descuento2, TotalPagar, Acuenta, Resta," &
                                          " FCompra, FPago, Status, Usuario, NumSuc, Bajado) VALUES (" & dr("Id").ToString & ",'" & dr("NumFactura").ToString & "','" & dr("NumRemision").ToString &
                                          "','" & dr("Proveedor").ToString & "','0','" & Replace(dr("Sub1").ToString, ",", "") & "','" & Replace(dr("IVA").ToString, ",", "") & "','" & Replace(dr("Total").ToString, ",", "") &
                                          "','0','" & Replace(dr("Pagar").ToString, ",", "") & "','" & Replace(dr("Acuenta").ToString, ",", "") & "','" & Replace(dr("Resta").ToString, ",", "") &
                                          "','" & Format(CDate(dr("FechaC").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaP").ToString), "yyyy-MM-dd") & "','" & dr("Status").ToString & "','" & dr("Usuario").ToString &
                                          "'," & susursalr & ",1)"

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                odata2.getDr(cnn2, dr2, "select max(IdCompra) as XD from compras", sinfo)
                                maxIdCompra = dr2(0).ToString

                                subeCompraDetalle(dr("Id").ToString, maxIdCompra)

                                ssql3 = "update compras set Cargado=1 where Id=" & dr("Id").ToString
                                If odata.runSp(cnn, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Compra folio " & dr("Id").ToString, Date.Now)
                                End If
                            Else
                                MsgBox(sinfo)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If


    End Sub

    Private Sub subeCompraDetalle(ByVal Folio As String, ByVal maxId As String)

        Dim cnn3 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn4 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from ComprasDet where Id_Compra=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim d3 As DataRow
        Dim dr4 As DataRow
        Dim sinfo As String = ""
        Dim odata3 As New ToolKitSQL.myssql
        Dim odata4 As New ToolKitSQL.myssql

        If odata3.dbOpen(cnn3, sTargetlocal, sinfo) Then
            If odata4.dbOpen(cnn4, sTargetdSincro, sinfo) Then

                If odata3.getDt(cnn3, dt4, sSQL, "dtcuatro") Then
                    For Each dr4 In dt4.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO comprasdetalle(Compra_id, Proveedor, NumFactura, NumRemision, Codigo, Nombre, UCompra, Cantidad, Precio, Total, Fecha, Grupo, Depto)" &
                                        " VALUES (" & maxId & ",'" & dr4("Proveedor").ToString & "','" & dr4("NumFactura").ToString & "','" & dr4("NumRemision").ToString & "','" & dr4("Codigo").ToString & "','" & Replace(dr4("Nombre").ToString, "Ã'", "Ñ") &
                                        "','" & dr4("UCompra").ToString & "','" & dr4("Cantidad").ToString & "','" & dr4("Precio").ToString & "','" & dr4("Total").ToString & "','" & Format(CDate(dr4("FechaC").ToString), "yyyy-MM-dd") & "','" & Replace(dr4("Grupo").ToString, "Ã'", "Ñ") &
                                        "','" & Replace(dr4("Depto").ToString, "Ã'", "Ñ") & "')"

                        If odata4.runSp(cnn4, ssqlinsertal, sinfo) Then

                        Else
                            MsgBox(sinfo)
                        End If

                    Next

                End If

                cnn4.Close()
            End If
            cnn3.Close()

        End If

    End Sub

    Private Sub subeAbonoCompra()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from AbonoE where Cargado=0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdCompra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata.getDt(cnn, dt, sSQL, "dtuno") Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Subiendo Abono Compra " & dr("id").ToString, Date.Now)
                        My.Application.DoEvents()

                        Dim idcompra As Integer = 0

                        If CStr(dr("NumFactura").ToString) = "" Then
                            odata.getDr(cnn, dr2, "select Id from Compras where NumRemision = '" & dr("NumRemision").ToString & "' and Proveedor = '" & dr("Proveedor").ToString & "'", "drdos")
                        Else
                            odata.getDr(cnn, dr2, "select Id from Compras where NumFactura = '" & dr("NumFactura").ToString & "' and NumRemision = '" & dr("NumRemision").ToString & "' and Proveedor = '" & dr("Proveedor").ToString & "'", "drdos")
                        End If
                        idcompra = dr2("Id").ToString
                        Dim formadepago As String = ""
                        ssqlinsertal = "INSERT INTO actuabonocompras(IdCompra, NumFactura, NumRemision, Proveedor, Concepto, Fecha, Abono, Saldo, MontoEfec, NumSuc, Bajado, FormaPago, Banco" &
                                        ") VALUES (" & idcompra & ",'" & dr("NumFactura").ToString & "','" & dr("NumRemision").ToString & "','" & dr("Proveedor").ToString & "','" & dr("Concepto").ToString & "','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Replace(dr("Abono").ToString, ",", "") & "','" & Replace(dr("Saldo").ToString, ",", "") & "','" & Replace(dr("Efectivo").ToString, ",", "") &
                                        "','" & susursalr & "',1,'Efectivo ', '" & dr("Banco").ToString & "')"

                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                            ssql3 = "update AbonoE set Cargado=1 where id=" & dr("id").ToString
                            If odata.runSp(cnn, ssql3, sinfo) Then
                                odata.runSp(cnn, "update Compras set Cargado = 0 where Id = " & idcompra & "", sinfo)
                                grid_eventos.Rows.Insert(0, "Finaliza Abono Compra " & dr("id").ToString, Date.Now)
                            End If
                        Else
                            MsgBox(sinfo)
                        End If
                    Next
                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub bajaTraspasoSalida()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select T.*, S.nombre as XD from Traspasos T, sucursales S  where S.id = T.Destino and T.CargadoS=0 and T.Origen = " & susursalr & ""
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdTraspaso As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()
                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Bajando Traspaso Salida folio " & dr("NumTraspasosS").ToString, Date.Now)
                        My.Application.DoEvents()

                        Dim fechapago As Date = dr("Fecha").ToString
                        Dim fechahora As Date = dr("Hora").ToString


                        ssqlinsertal = "INSERT INTO Traslados(Cargado,Nombre,Direccion,Usuario,FVenta,HVenta,FPago,FCancelado,Status,Comisionista,concepto,NUM_TRASLADO) " &
                                                   " VALUES (1,'TRASLADO','0','0','" & Format(fechapago, "yyyy-MM-dd") & "','" & Format(fechahora, "yyyy-MM-dd HH:mm:ss") & "','" & Format(fechapago, "yyyy-MM-dd") & "','" & Format(fechapago, "yyyy-MM-dd") & "','PAGADO','" & dr("XD").ToString & "','SALIDA'," & dr("NumTraspasosS").ToString & ")"

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                            odata.getDr(cnn, dr2, "select max(Folio) as XD from Traslados", "drdos")
                            maxIdTraspaso = dr2(0).ToString

                            bajaTrasDetalle(dr("Id").ToString, maxIdTraspaso, dr("NumTraspasosS").ToString, dr("XD").ToString)

                            ssql3 = "update Traspasos set CargadoS=1 where Id=" & dr("Id").ToString
                            If odata2.runSp(cnn2, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Traspaso Salida folio " & dr("NumTraspasosS").ToString, Date.Now)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If


    End Sub

    Private Sub bajaTrasDetalle(ByVal Folio As String, ByVal maxId As String, ByVal numTras As String, ByVal vardestino As String)

        Dim cnn3 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn4 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from TraspasosDetalle where IdTraspaso=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim d3 As DataRow
        Dim dr4 As DataRow
        Dim sinfo As String = ""
        Dim odata3 As New ToolKitSQL.myssql
        Dim odata4 As New ToolKitSQL.myssql

        If odata3.dbOpen(cnn3, sTargetlocal, sinfo) Then
            If odata4.dbOpen(cnn4, sTargetdSincro, sinfo) Then

                If odata4.getDt(cnn4, dt4, sSQL, sinfo) Then
                    For Each dr4 In dt4.Rows

                        My.Application.DoEvents()

                        'ssqlinsertal = "INSERT INTO TrasladosDet(Folio, Codigo, Nombre, Unidad, Cantidad, Precio, Total, PrecioSinIVA, TotalSinIVA, Fecha, Comisionista, Depto, Grupo, concepto, num_traslado)" &
                        '                " VALUES (" & maxId & ",'" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString & "','" & dr4("UVenta").ToString & "'," & dr4("Cantidad").ToString & "," & dr4("Precio").ToString &
                        '                "," & dr4("Total").ToString & "," & dr4("Precio").ToString & "," & dr4("Total").ToString & ",'" & Format(CDate(dr4("Fecha").ToString), "dd/MM/yyyy") & "','" & vardestino &
                        '                "','" & dr4("Depto").ToString & "','" & dr4("Grupo").ToString & "','SALIDA'," & numTras & ")"
                        ssqlinsertal = ""

                        Dim fechapago As Date = dr4("Fecha").ToString

                        ssqlinsertal = "INSERT INTO TrasladosDet(Folio, Codigo, Nombre, Unidad, Cantidad, Precio, Total, Fecha, Comisionista, Depto, Grupo, concepto, num_traslado)" &
                                        " VALUES (" & maxId & ",'" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString & "','" & dr4("UVenta").ToString & "'," & dr4("Cantidad").ToString & "," & dr4("Precio").ToString &
                                        "," & dr4("Total").ToString & ",'" & Format(fechapago, "yyyy-MM-dd") & "','" & vardestino &
                                        "','" & dr4("Depto").ToString & "','" & dr4("Grupo").ToString & "','SALIDA'," & numTras & ")"

                        odata3.runSp(cnn3, ssqlinsertal, sinfo)

                    Next

                End If

                cnn4.Close()
            End If
            cnn3.Close()

        End If

    End Sub

    Private Sub bajaExitTrasSalida()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from actuinvtraspasos where NumSuc = " & susursalr & " and Tipo = 'SALIDA'"
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()

                            If oData.getDr(cnn, dr2, "select Codigo,Existencia,Multiplo from Productos where Codigo = '" & dr("Codigo").ToString & "'", "drDOS") Then

                                MyExist = 0
                                If CDec(dr2("Multiplo").ToString) > 1 And CDec(dr2("Existencia").ToString) > 0 Then
                                    MyExist = FormatNumber(CDec(dr2("Existencia").ToString), 2)
                                    MyNewEsist = CDec(MyExist) - CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString))
                                Else
                                    MyExist = dr2("Existencia").ToString
                                    MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                End If

                                If oData.runSp(cnn, "update Productos set Existencia = Existencia - " & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ", CargadoInv = 0 where Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then

                                    ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Salida por Traspaso Nube'," & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"

                                    oData.runSp(cnn, ssql3, sinfo)

                                    If Trim(dr("Lote").ToString) <> "" Then
                                        actualizarLoteCad(dr("Codigo").ToString, dr("Lote").ToString, dr("FechaCad").ToString, dr("Cantidad").ToString, 0)
                                    End If

                                    odata2.runSp(cnn2, "delete from actuinvtraspasos where Id = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)
                                End If
                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With

    End Sub

    Private Sub bajaTraspasosEntrada()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select T.*, S.nombre as XD from traspasos T, sucursales S  where S.id = T.Origen and T.CargadoE=0 and T.Destino = " & susursalr & ""
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdTraspaso As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Bajando Traspaso Entrada folio " & dr("NumTraspasosE").ToString, Date.Now)
                        My.Application.DoEvents()

                        Dim fechapago As Date = dr("Fecha").ToString
                        Dim fechahora As Date = dr("Hora").ToString

                        ssqlinsertal = "INSERT INTO Traslados(Cargado,Nombre,Direccion,Usuario,FVenta,HVenta,FPago,FCancelado,Status,Comisionista,concepto,NUM_TRASLADO) " &
                                                   " VALUES (1,'INGRESO','0','0','" & Format(fechapago, "yyyy-MM-dd") & "','" & Format(fechahora, "yyyy-MM-dd HH:mm:ss") & "','" & Format(fechapago, "yyyy-MM-dd") & "','" & Format(fechapago, "yyyy-MM-dd HH:mm:ss") & "','PAGADO','" & dr("XD").ToString & "','ENTRADA'," & dr("NumTraspasosE").ToString & ")"

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                            odata.getDr(cnn, dr2, "select max(Folio) as XD from Traslados", "drdos")
                            maxIdTraspaso = dr2(0).ToString

                            bajaTrasEDetalle(dr("Id").ToString, maxIdTraspaso, dr("NumTraspasosE").ToString, dr("XD").ToString)

                            ssql3 = "update traspasos set CargadoE=1 where Id=" & dr("Id").ToString
                            If odata2.runSp(cnn2, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Traspaso Entrada folio " & dr("NumTraspasosE").ToString, Date.Now)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If


    End Sub

    Private Sub bajaExitTrasEntrada()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim odata2 As New ToolKitSQL.myssql
        Dim sSQL As String = "Select * from actuinvtraspasos where NumSuc = " & susursalr & " and Tipo = 'ENTRADA'"
        Dim ssql2 As String = ""
        Dim ssql3 As String = ""
        Dim sinfo As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        Dim oData As New ToolKitSQL.myssql
        With oData
            If .dbOpen(cnn, sTargetlocal, sinfo) Then
                If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                    If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                        For Each dr In dt.Rows

                            My.Application.DoEvents()

                            If oData.getDr(cnn, dr2, "select Codigo,Existencia,Multiplo from Productos where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'", "drDOS") Then

                                MyExist = 0
                                If CDec(dr2("Multiplo").ToString) > 1 And CDec(dr2("Existencia").ToString) > 0 Then
                                    MyExist = FormatNumber(CDec(dr2("Existencia").ToString), 2)
                                    If Len(dr("Codigo").ToString) > 6 Then
                                        MyNewEsist = CDec(MyExist) + CDec(dr("Cantidad").ToString)
                                    Else
                                        MyNewEsist = CDec(MyExist) + CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString))
                                    End If

                                Else
                                    MyExist = dr2("Existencia").ToString
                                    MyNewEsist = CDec(MyExist) + CDec(dr("Cantidad").ToString)
                                End If

                                Dim sqlnew As String = ""

                                If Len(dr("Codigo").ToString) > 6 Then
                                    sqlnew = "update Productos set Existencia = Existencia + " & CDec(dr("Cantidad").ToString) & ", CargadoInv = 0  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'"
                                Else
                                    sqlnew = "update Productos set Existencia = Existencia + " & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ", CargadoInv = 0  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'"
                                End If

                                If oData.runSp(cnn, sqlnew, sinfo) Then

                                    If Len(dr("Codigo").ToString) > 6 Then
                                        ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Entrada por Traspaso Nube'," & CDec(dr("Cantidad").ToString) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"
                                    Else
                                        ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Entrada por Traspaso Nube'," & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"
                                    End If

                                    'ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cant_Prod,Precio_prod,fecha,Usuario,Existencia,Diferencia,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Entrada por Traspaso Nube'," & CDec(CDec(dr("Cantidad").ToString) * CDec(dr2("Multiplo").ToString)) & ",'0','" & Now & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"

                                    oData.runSp(cnn, ssql3, sinfo)

                                    If Trim(dr("Lote").ToString) <> "" Then
                                        actualizarLoteCad(dr("Codigo").ToString, dr("Lote").ToString, dr("FechaCad").ToString, dr("Cantidad").ToString, 1)
                                    End If

                                    odata2.runSp(cnn2, "delete from actuinvtraspasos where Id = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)
                                End If
                            Else

                                If odata2.getDr(cnn2, dr3, "Select * from productos where Codigo='" & dr("Codigo").ToString & "'", sinfo) Then

                                    ssqlinsertal = "Insert Into Productos(Codigo,Nombre,ProvPri,ProvRes,UCompra,UVenta,VentaMin,MCD,Multiplo,Departamento,Grupo,PrecioCompra,PorcentageMin,Porcentage,PrecioVenta,PrecioVentaIVA,PecioVentaMinIVA,IVA,Existencia,id_tbMoneda,PercentIVAret,NombreLargo,IIEPS,isr,ClaveSat,ClaveUnidadSat,MSeries,CargadoInv) " &
                                                            "VALUES('" & dr3("Codigo").ToString & "','" & dr3("Nombre").ToString & "','" & dr3("proveedor").ToString & "',0,'" & dr3("UVenta").ToString & "','" & dr3("UVenta").ToString &
                                                           "','" & dr3("UVenta").ToString & "',1,1,'" & dr3("Depto").ToString & "','" & dr3("Grupo").ToString & "','" & dr3("PrecioCompra").ToString &
                                                          "','0','0','0','" & dr3("PrecioVentaIVA").ToString & "','0','" & dr3("IVA").ToString & "'," & dr("Cantidad").ToString &
                                                         ",1,0,'',0,0,'" & dr3("clavesat").ToString & "','" & dr3("claveunisat").ToString & "',0,0)"
                                    If oData.runSp(cnn, ssqlinsertal, sinfo) Then

                                        MyExist = 0
                                        MyNewEsist = CDec(MyExist) + CDec(dr("Cantidad").ToString)
                                        ssql3 = "insert into Cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,fecha,Usuario,Inicial,Final,Folio) values('" & dr3("Codigo").ToString & "','" & dr3("Nombre").ToString & "','Entrada por Traspaso Nube'," & dr("Cantidad").ToString & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','Nube','" & MyExist & "','" & MyNewEsist & "','')"
                                        oData.runSp(cnn, ssql3, sinfo)

                                        If Trim(dr("Lote").ToString) <> "" Then
                                            actualizarLoteCad(dr("Codigo").ToString, dr("Lote").ToString, dr("FechaCad").ToString, dr("Cantidad").ToString, 1)
                                        End If

                                        odata2.runSp(cnn2, "delete from actuinvtraspasos where Id = " & dr("Id").ToString & "", sinfo)
                                        grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr3("Nombre").ToString, Date.Now)

                                    End If

                                End If


                            End If
                        Next
                    End If
                    cnn2.Close()
                End If
                cnn.Close()
            End If
        End With

    End Sub

    Private Sub subeTraspasoSalida()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Traslados where Cargado=0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdTraspaso As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Subiendo Traspaso Salida folio " & dr("Folio").ToString, Date.Now)
                        My.Application.DoEvents()

                        Dim IdOrigen As Integer = susursalr
                        Dim IdDestino As Integer = 0
                        If odata2.getDr(cnn2, dr3, "select Id from sucursales where nombre = '" & dr("Comisionista").ToString & "'", sinfo) Then
                            IdDestino = dr3(0).ToString
                        End If
                        sucdestino = IdDestino
                        Dim MaxNumTraspasosS As Integer = 0
                        If odata2.getDr(cnn2, dr3, "select MAX(NumTraspasosS) as maxi from traspasos where Origen = " & IdOrigen & "", sinfo) Then
                            If IsNumeric(dr3(0).ToString) Then
                                MaxNumTraspasosS = dr3(0).ToString + 1
                            Else
                                MaxNumTraspasosS = 1
                            End If

                        Else
                            MaxNumTraspasosS = 1
                        End If

                        Dim MaxNumTraspasosE As Integer = 0
                        If odata2.getDr(cnn2, dr3, "select MAX(NumTraspasosE) as maxi from traspasos where Destino = " & IdDestino & "", sinfo) Then
                            If IsNumeric(dr3(0).ToString) Then
                                MaxNumTraspasosE = dr3(0).ToString + 1
                            Else
                                MaxNumTraspasosE = 1
                            End If

                        Else
                            MaxNumTraspasosE = 1
                        End If

                        ssqlinsertal = "INSERT INTO traspasos(NumTraspasosS,NumTraspasosE,Nombre,Fecha,Hora,Origen,Destino,Tipo,CargadoS) " &
                                                   " VALUES (" & MaxNumTraspasosS & "," & MaxNumTraspasosE & ",'TRASLADO','" & Format(CDate(dr("FVenta").ToString), "yyyy-MM-dd") & "','" & dr("HVenta").ToString & "'," & IdOrigen & "," & IdDestino & ",'SALIDA',1)"

                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                            odata2.getDr(cnn2, dr2, "select max(Id) as XD from traspasos", sinfo)
                            maxIdTraspaso = dr2(0).ToString

                            subeTrasDetalle(dr("Folio").ToString, maxIdTraspaso, dr("NUM_TRASLADO").ToString, IdDestino)

                            ssql3 = "update Traslados set Cargado=1 where Folio=" & dr("Folio").ToString
                            If odata.runSp(cnn, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Traspaso Salida folio " & dr("Folio").ToString, Date.Now)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If

        My.Application.DoEvents()
    End Sub

    Private Sub busca_ventasl()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Ventas where cargado=0 order by Folio"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then



            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then


                If odata.getDt(cnn, dt, sSQL, sinfo) Then



                    For Each dr In dt.Rows



                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr1, "select * from ventas where Folio = '" & dr("Folio").ToString & "' and sucursal =" & susursalr & "", sinfo) Then

                            ssqlinsertal = ""
                            grid_eventos.Rows.Insert(0, "Actualiza Venta folio " & dr("Folio").ToString, Date.Now)
                            My.Application.DoEvents()
                            ssqlinsertal = "update ventas set Subtotal = " & Replace(dr("Subtotal").ToString, ",", "") & ", IVA = " & Replace(dr("IVA").ToString, ",", "") & ", Totales = " & Replace(dr("Totales").ToString, ",", "") & ", Descuento = " & Replace(dr("Descuento").ToString, ",", "") & ", Devolucion = " & Replace(dr("Devolucion").ToString, ",", "") & ", ACuenta = " & Replace(dr("ACuenta").ToString, ",", "") & "," &
                                              " Resta = " & Replace(dr("Resta").ToString, ",", "") & ", Status = '" & dr("Status").ToString & "',CostVR='0' where sucursal = " & susursalr & " and Folio = " & dr("Folio").ToString & ""

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                ssql3 = "update Ventas set cargado=1 where Folio=" & dr("Folio").ToString
                                If odata.runSp(cnn, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Act Venta folio " & dr("Folio").ToString, Date.Now)

                                End If
                            End If

                        Else



                            ssqlinsertal = ""
                            grid_eventos.Rows.Insert(0, "Inicia Sincronizacion folio " & dr("Folio").ToString, Date.Now)
                            My.Application.DoEvents()
                            ssqlinsertal = "INSERT INTO ventas(Folio, idCliente, Nombre, Direccion, Subtotal, IVA, Totales, Descuento, Devolucion, ACuenta," &
                                              " Resta, Usuario, FVenta, HVenta, FPago, FCancelado, MontoEfecCanc, Status, Comisionista, Facturado," &
                                              "TipoMov, sucursal) VALUES (" & dr("Folio").ToString & "," & dr("idCliente").ToString & ",'" & dr("Cliente").ToString &
                                              "','" & dr("Direccion").ToString & "'," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Totales").ToString, ",", "") &
                                              "," & Replace(dr("Descuento").ToString, ",", "") & "," & Replace(dr("Devolucion").ToString, ",", "") & "," & Replace(dr("ACuenta").ToString, ",", "") & "," & Replace(dr("Resta").ToString, ",", "") &
                                              ",'" & dr("Usuario").ToString & "','" & Format(CDate(dr("FVenta").ToString), "yyyy-MM-dd") & "','" & dr("HVenta").ToString & "','" & dr("FPago").ToString &
                                              "','" & dr("FCancelado").ToString & "'," & dr("MontoCance").ToString & ",'" & dr("Status").ToString & "','" & dr("Comisionista").ToString &
                                              "','" & dr("Facturado").ToString & "','0'," & susursalr & ")"

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                busca_Detalle(dr("Folio").ToString)
                                ssql3 = "update ventas set Cargado=1 where Folio=" & dr("Folio").ToString
                                If odata.runSp(cnn, ssql3, sinfo) Then
                                Else
                                    MsgBox(sinfo)
                                End If


                            Else
                                MsgBox(sinfo)


                                If odata.runSp(cnn, ssql3, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion folio " & dr("Folio").ToString, Date.Now)
                                End If
                            End If
                        End If
                    Next
                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If
    End Sub

    Private Sub busca_ventasFranquicia()
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sinfo As String = ""
        Dim sSQL As String = "Select max(IdCompra) from Compras"
        Dim sSQL2 As String = "Select * from Ventas where Franquicia=0 order by folio"
        Dim sql3 As String = "Select * from sucursales where id=" & susursalr
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim dt As New DataTable
        Dim dr As DataRow

        Dim dt2 As New DataTable
        Dim dr2 As DataRow
        Dim dt3 As New DataTable
        Dim dr3 As DataRow
        Dim soyultimoid As Integer = 0
        Dim prove As String = ""
        Dim myid As Integer = 0
        Dim grupo As String = ""
        Dim depa As String = ""
        Dim soy As String = ""
        With odata
            If .dbOpen(cnn, sTargetdSincro, sinfo) Then
                If odata.getDr(cnn, dr, sql3, sinfo) Then
                    soy = dr("nombre").ToString
                End If
                If odata2.dbOpen(cnn2, sTargetlocal, sinfo) Then
                    If odata2.getDt(cnn2, dt2, sSQL2, sinfo) Then

                        For Each dr2 In dt2.Rows
                            grid_eventos.Rows.Insert(0, "Inicia Sincronizacion Compra/Venta Franquicia Folio " & dr2("Folio").ToString, Date.Now)
                            If .runSp(cnn, "Insert into compras(Id,NumFactura,NumRemision,Proveedor,Descuento1,Subtotal,IVA,Total,Descuento2,TotalPagar,ACuenta,Resta,FCompra,FPago,Status,Usuario,NumSuc, Bajado) values('" & dr2("Folio").ToString & "','" & dr2("Folio").ToString & "','" & dr2("Folio").ToString & "','" & soy & "','" & dr2("Descuento").ToString & "','" & dr2("Subtotal").ToString & "','0','" & dr2("Totales").ToString & "','" & dr2("Descuento").ToString & "','" & dr2("Resta").ToString & "','" & dr2("ACuenta").ToString & "','" & dr2("Resta").ToString & "','" & Format(Date.Now, "yyyy-MM-dd") & "','" & Format(Date.Now, "yyyy-MM-dd") & "','PAGADO','" & dr2("Usuario").ToString & "'," & dr2("IdCliente").ToString & ", 1)", sinfo) Then
                                ''''''''' INSERTA COMPRAS DETALLE
                                sucu = dr2("IdCliente").ToString
                                If odata.getDr(cnn, dr3, "select max(IdCompra) as XD from compras", sinfo) Then
                                    soyultimoid = dr3(0).ToString
                                End If

                                If dr2("Acuenta").ToString > 0 Then
                                    If .runSp(cnn, "insert into actuAbonoCompras(IdCompra,NumFactura,NumRemision,Proveedor,Concepto,Fecha,Abono,Saldo,MontoEfec,NumSuc,Bajado) values(" & soyultimoid & ",'" & dr2("Folio").ToString & "','" & dr2("Descuento").ToString & "','" & soy & "','ABONO','" & Format(Date.Now, "yyyy-MM-dd") & "','" & dr2("ACuenta").ToString & "','" & dr2("ACuenta").ToString & "','" & dr2("ACuenta").ToString & "','" & dr2("IdCLiente").ToString & "',1)", sinfo) Then
                                    Else
                                        MsgBox(sinfo)
                                    End If
                                End If

                                busca_Detalle2(dr2("Folio").ToString)
                                If .runSp(cnn2, "update Ventas set Franquicia=1 where Folio=" & dr2("Folio").ToString, sinfo) Then
                                    grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Compra/Venta Franquicia Folio " & dr2("Folio").ToString, Date.Now)
                                Else
                                    MsgBox(sinfo)
                                End If
                            Else
                                MsgBox(sinfo)
                            End If
                        Next

                    Else

                    End If
                End If

                cnn.Close()
                cnn2.Close()
            End If
        End With
    End Sub

    Private Sub busca_Detalle2(ByVal Folio As String)
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Ventasdetalle where Folio=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dt3 As New DataTable
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim soyultimoid As Integer = 0
        Dim drloc As DataRow
        Dim prove As String = ""
        Dim myid As Integer = 0
        Dim grupo As String = ""
        Dim depa As String = ""
        Dim soy As String = ""

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then

                    For Each dr In dt.Rows

                        If odata.getDt(cnn, dt3, "Select * from productos where Codigo='" & dr("Codigo").ToString & "'", sinfo) Then
                            For Each dr3 In dt3.Rows
                                prove = dr3("ProvPri").ToString
                                myid = dr3("Id").ToString
                                grupo = dr3("Grupo").ToString
                                depa = dr3("Departamento").ToString
                            Next
                        Else

                        End If

                        If odata2.getDr(cnn2, dr3, "select max(IdCompra) as XD from compras", sinfo) Then
                            soyultimoid = dr3(0).ToString
                        End If


                        If odata2.runSp(cnn2, "Insert into comprasdetalle(Compra_id,Proveedor,NumRemision,Codigo,Nombre,UCompra,Cantidad,Precio,Total,Fecha,Grupo,Depto) values(" & soyultimoid & ",'" & prove & "','" & dr("Folio").ToString & "','" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "','" & dr("Unidad").ToString & "','" & dr("Cantidad").ToString & "','" & dr("Precio").ToString & "','" & dr("Total").ToString & "','" & Format(Date.Now, "yyyy-MM-dd") & "','" & grupo & "','" & depa & "')", sinfo) Then



                            If odata2.runSp(cnn2, "Update productos set PrecioCompra=" & dr("Precio").ToString & " where Codigo='" & dr("Codigo").ToString & "' and NumSuc=" & sucu & "", sinfo) Then
                            Else
                                MsgBox(sinfo)
                            End If

                            If odata2.runSp(cnn2, "insert into actuprecios(codigo,descripcion,tipo,precio,NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "',1,'" & dr("Precio").ToString & "'," & sucu & ")", sinfo) Then
                            Else
                                MsgBox(sinfo)
                            End If

                            If odata2.runSp(cnn2, "insert into actuinvcompras(Codigo,Descripcion,Cantidad,NumSuc,Id_byzinventario) values ('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "','" & dr("Cantidad").ToString & "'," & sucu & "," & myid & ")", sinfo) Then
                            Else
                                MsgBox(sinfo)
                            End If
                        Else
                            MsgBox(sinfo)
                        End If
                    Next
                Else

                End If
            End If
            cnn2.Close()
            cnn.Close()
        End If

    End Sub
    Private Sub busca_Detalle(ByVal Folio As String)
        Dim codigoint As String = ""
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Ventasdetalle where Folio=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim drloc As DataRow

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then

                If odata.getDt(cnn, dt, sSQL, sinfo) Then

                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        codigoint = ""
                        sSQL2 = "Select NombreLargo from productos where Codigo='" & dr("Codigo").ToString & "'"
                        If odata.getDr(cnn, drloc, sSQL2, sinfo) Then
                            codigoint = drloc("NombreLargo").ToString
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO ventasdetalle(id, Folio, Codigo, Nombre, UVenta, Cantidad, CostVR, CostVP, CostVUE, Precio, Total, " &
                                        "PrecioSinIVA, TotalSinIVA, Fecha, Comisionista, Facturado, Depto, Grupo, comensal, Gprint, VDCosteo, Comentario, CUsuario, sucursal, NombreLargo)" &
                                        " VALUES (" & dr("id").ToString & "," & dr("Folio").ToString & ",'" & dr("Codigo").ToString & "','" & Replace(dr("Nombre").ToString, "Ã'", "Ñ") &
                                        "','" & dr("Unidad").ToString & "'," & Replace(dr("Cantidad").ToString, ",", "") & "," & Replace(dr("CostVR").ToString, ",", "") & "," & Replace(dr("CostoVP").ToString, ",", "") &
                                        "," & Replace(dr("CostoVUE").ToString, ",", "") & "," & Replace(dr("Precio").ToString, ",", "") & "," & Replace(dr("Total").ToString, ",", "") & "," & Replace(dr("PrecioSinIVA").ToString, ",", "") &
                                        "," & Replace(dr("TotalSinIVA").ToString, ",", "") & ",'" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & dr("Comisionista").ToString &
                                        "','" & dr("Facturado").ToString & "','" & Replace(dr("Depto").ToString, "Ã'", "Ñ") & "','" & Replace(dr("Grupo").ToString, "Ã'", "Ñ") & "'," &
                                        "0,'0'," & dr("VDCosteo").ToString & ",'','', " & susursalr & ",'" & codigoint & "')"

                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                        Else
                            MsgBox(sinfo)
                        End If
                    Next
                    cnn2.Close()
                Else

                End If
            End If
            cnn.Close()
        Else

        End If
    End Sub

    Private Sub subeTrasDetalle(ByVal Folio As String, ByVal maxId As String, ByVal numTras As String, ByVal vardestino As String)

        Dim cnn3 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn4 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from TrasladosDet where Folio=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim sqllote As String = ""
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim d3 As DataRow
        Dim dr4 As DataRow
        Dim sinfo As String = ""
        Dim odata3 As New ToolKitSQL.myssql
        Dim odata4 As New ToolKitSQL.myssql

        If odata3.dbOpen(cnn3, sTargetlocal, sinfo) Then
            If odata4.dbOpen(cnn4, sTargetdSincro, sinfo) Then
                If odata3.getDt(cnn3, dt4, sSQL, "dtcuatro") Then
                    For Each dr4 In dt4.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO traspasosdetalle(IdTraspaso, Codigo, Nombre, UVenta, Cantidad, Precio, Total, Fecha, Destino, Depto, Grupo,Lote,FechaCad)" &
                                        " VALUES (" & maxId & ",'" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString & "','" & dr4("Unidad").ToString & "'," & dr4("Cantidad").ToString & "," & dr4("Precio").ToString &
                                        "," & dr4("Total").ToString & ",'" & Format(CDate(dr4("Fecha").ToString), "yyyy-MM-dd") & "'," & vardestino &
                                        ",'" & dr4("Depto").ToString & "','" & dr4("Grupo").ToString & "','" & dr4("Lote").ToString & "','" & dr4("FCaduca").ToString & "')"
                        odata4.runSp(cnn4, ssqlinsertal, sinfo)

                        Dim IdProdNube As Integer = 0
                        odata4.getDr(cnn4, d3, "select Id From productos where Codigo = '" & dr4("Codigo").ToString & "' and NumSuc = " & susursalr & "", sinfo)
                        IdProdNube = d3(0).ToString

                        ssqlinsertal = ""
                        ssqlinsertal = "insert into actuinvtraspasos(Codigo,Descripcion,Cantidad,NumSuc,Id_byzinventario,Tipo,Lote,FechaCad) values ('" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString & "'," & dr4("Cantidad").ToString & "," & vardestino & "," & IdProdNube & ",'ENTRADA','" & dr4("Lote").ToString & "','" & dr4("FCaduca").ToString & "')"
                        odata4.runSp(cnn4, ssqlinsertal, sinfo)
                        'If dr4("Lote") <> "" Then
                        '    sqllote = ""
                        '    sqllote = "insert into lotecaducidad(Codigo,Lote,FechaCad,Cantidad,NumSuc,Bajado) values ('" & dr4("Codigo").ToString & "','" & dr4("Lote").ToString & "','" & dr4("FCaduca").ToString & "','" & dr4("Cantidad").ToString & "'," & sucdestino & ",0)"
                        '    If odata4.runSp(cnn4, sqllote, sinfo) Then
                        '    Else
                        '    End If
                        'End If
                    Next
                End If
                cnn4.Close()
            End If
            cnn3.Close()
        End If

    End Sub

    Private Sub bajaTrasEDetalle(ByVal Folio As String, ByVal maxId As String, ByVal numTras As String, ByVal vardestino As String)

        Dim cnn3 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn4 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from traspasosdetalle where IdTraspaso=" & Folio
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt3 As New DataTable
        Dim dt4 As New DataTable
        Dim d3 As DataRow
        Dim dr4 As DataRow
        Dim sinfo As String = ""
        Dim odata3 As New ToolKitSQL.myssql
        Dim odata4 As New ToolKitSQL.myssql

        If odata3.dbOpen(cnn3, sTargetlocal, sinfo) Then
            If odata4.dbOpen(cnn4, sTargetdSincro, sinfo) Then

                If odata4.getDt(cnn4, dt4, sSQL, sinfo) Then
                    For Each dr4 In dt4.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        Dim fecha As Date = dr4("Fecha").ToString

                        ssqlinsertal = "INSERT INTO TrasladosDet(Folio, Codigo, Nombre, Unidad, Cantidad, Precio, Total, Fecha, Comisionista, Depto, Grupo, concepto, num_traslado)" &
                                        " VALUES (" & maxId & ",'" & dr4("Codigo").ToString & "','" & dr4("Nombre").ToString & "','" & dr4("UVenta").ToString & "'," & dr4("Cantidad").ToString & "," & dr4("Precio").ToString &
                                        "," & dr4("Total").ToString & ",'" & Format(fecha, "yyyy-MM-dd") & "','" & vardestino &
                                        "','" & dr4("Depto").ToString & "','" & dr4("Grupo").ToString & "','ENTRADA'," & numTras & ")"

                        odata3.runSp(cnn3, ssqlinsertal, sinfo)

                    Next

                End If

                cnn4.Close()
            End If
            cnn3.Close()

        End If

    End Sub

    Private Sub buscaDevoluciones()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from Devoluciones where Cargado=0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then

            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then

                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Sincronizacion Devolucion Folio " & dr("Folio").ToString, Date.Now)
                        My.Application.DoEvents()
                        ssqlinsertal = "Update ventas set Cargado = 0 where Folio = " & dr("Folio").ToString & ""

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                            If odata2.getDt(cnn2, dt2, "Select * from ventasdetalle where Folio = " & dr("Folio").ToString & " and Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then
                                For Each dr2 In dt2.Rows
                                    If CDec(dr2("Cantidad").ToString) = CDec(dr("Cantidad").ToString) Then
                                        odata2.runSp(cnn2, "Delete From ventasdetalle where id = " & dr2("id").ToString & "", sinfo)
                                        Exit For
                                    ElseIf CDec(dr2("Cantidad").ToString) > CDec(dr("Cantidad").ToString) Then
                                        odata2.runSp(cnn2, "update ventasdetalle set Cantidad = " & CDec(CDec(dr2("Cantidad").ToString) - CDec(dr("Cantidad").ToString)) & " where id = " & dr2("id").ToString & "", sinfo)
                                        Exit For
                                    End If
                                Next
                            End If

                            odata.runSp(cnn, "Update Devoluciones set Cargado = 1 where Folio = " & dr("Folio").ToString & "", sinfo)
                        End If
                    Next

                End If

                cnn2.Close()
            End If



            cnn.Close()
        End If


    End Sub

    Private Sub ExiteProductoSucACTSINEXIPRE(ByVal varCodigo As String, ByVal varDesc As String)
        Dim cnn10 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn210 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim ssql As String = ""
        ssql = "Select Codigo,Nombre,IVA,UVenta,PrecioCompra,PorcMin,PorcMay,PorcMM,PorEsp,Porcentaje,PreMin,PreMay," &
                "PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin1,CantMay1,CantMM1,CantEsp1,CantLst1,CantMin2,CantMay2,CantMM2,CantEsp2," &
                "CantLst2,id_tbMoneda,Departamento,Grupo,Existencia,ClaveSat,UnidadSat,ProvPri,MCD,Multiplo,CodBarra,IIEPS from Productos where Codigo='" & varCodigo & "'"
        Dim ssql2 As String = ""
        Dim insertaractualizar As String = ""
        Dim dt10 As New DataTable
        Dim dr10 As DataRow
        Dim dr210 As DataRow
        Dim sinfo10 As String = ""
        Dim odata10 As New ToolKitSQL.myssql
        Dim odata210 As New ToolKitSQL.myssql

        If odata10.dbOpen(cnn10, sTargetlocal, sinfo10) Then
            If odata210.dbOpen(cnn210, sTargetdSincro, sinfo10) Then

                If odata10.getDt(cnn10, dt10, ssql, sinfo10) Then
                    For Each dr10 In dt10.Rows

                        My.Application.DoEvents()

                        If num_Sucursales > 1 Then

                            dt_Sucursales = New DataTable
                            If odata210.getDt(cnn210, dt_Sucursales, "select * from sucursales", sinfo10) Then
                                For Each Me.dr_Sucursales In dt_Sucursales.Rows

                                    If susursalr <> dr_Sucursales("id").ToString Then

                                        If odata210.getDr(cnn210, dr210, "select * from productos where Codigo='" & dr10("Codigo").ToString & "' and NumSuc = " & dr_Sucursales("id").ToString & "", sinfo10) Then

                                            insertaractualizar = ""
                                            insertaractualizar = "update productos set Depto='" & dr10("Departamento").ToString & "',Grupo='" & dr10("Grupo").ToString & "', clavesat = '" & dr10("ClaveSat").ToString & "', claveunisat = '" & dr10("UnidadSat").ToString & "',proveedor='" & dr10("ProvPri").ToString & "',IVA='" & dr10("IVA").ToString & "',UVenta='" & dr10("UVenta").ToString & "',MCD = " & IIf(IsNumeric(dr10("MCD").ToString), dr10("MCD").ToString, 1) & ", Multiplo = " & IIf(IsNumeric(dr10("Multiplo").ToString), dr10("Multiplo").ToString, 1) & ",CodBarra = '" & dr10("CodBarra").ToString & "', IIEPS = " & IIf(IsNumeric(dr10("IIEPS").ToString), dr10("IIEPS").ToString, 0) & ", Cargado = 1 where Codigo = '" & dr10("Codigo").ToString & "' and NumSuc = " & dr_Sucursales("id").ToString & ""

                                            If odata210.runSp(cnn210, insertaractualizar, sinfo10) Then

                                            End If
                                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr10("Nombre").ToString, Date.Now)
                                        Else
                                            insertaractualizar = ""
                                            insertaractualizar = "INSERT INTO productos(Codigo,Nombre,IVA,UVenta,PrecioCompra,PorcMin,PorcMay,PorcMM,PorcEsp,Porcentaje,PecioVentaMinIVA,PreMay," &
                                                                                  "PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin1,CantMay1,CantMM1,CantEsp1,CantLst1,CantMin2,CantMay2,CantMM2,CantEsp2," &
                                                                                  "CantLst2,id_tbMoneda,NumSuc,Depto,Grupo,proveedor,exitencia,clavesat,claveunisat,MCD,Multiplo,CodBarra,IIEPS,Cargado) " &
                                                                                  " VALUES ('" & dr10("Codigo").ToString & "','" & dr10("Nombre").ToString & "','" & dr10("IVA").ToString & "','" & dr10("UVenta").ToString & "','0','0','0','0','0','0','0','0','0','0','0','0','" & dr10("pres_vol").ToString & "','0','0','0','0','0','0','0','0','0','0','1'," & dr_Sucursales("id").ToString & ",'" & dr10("Departamento").ToString & "','" & dr10("Grupo").ToString & "','" & dr10("ProvPri").ToString & "','0','" & dr10("ClaveSat").ToString & "','" & dr10("UnidadSat").ToString &
                                                                                  "'," & IIf(IsNumeric(dr10("MCD").ToString), dr10("MCD").ToString, 1) & "," & IIf(IsNumeric(dr10("Multiplo").ToString), dr10("Multiplo").ToString, 1) & ",'" & dr10("CodBarra").ToString & "' , " & IIf(IsNumeric(dr10("IIEPS").ToString), dr10("IIEPS").ToString, 0) & ",1)"

                                            If odata210.runSp(cnn210, insertaractualizar, sinfo10) Then

                                            End If
                                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr10("Nombre").ToString, Date.Now)
                                        End If

                                    End If

                                Next
                            End If

                        End If
                    Next
                End If

                dt_Sucursales = New DataTable
                odata210.getDt(cnn210, dt_Sucursales, "select * from sucursales", sinfo10)

                cnn210.Close()
            End If
            cnn10.Close()
        End If

    End Sub

    Private Sub busca_abonos()

        Dim tipop As String = ""
        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from AbonoI where Abono>0 and Cargado=0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then

                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        'tipop = "EFECTIVO"
                        tipop = (dr("FormaPago").ToString)
                        'If IsNumeric(dr("Tarjeta").ToString) Then
                        '    If dr("Tarjeta").ToString > 0 Then
                        '        tipop = "TARJETA"
                        '    End If
                        'End If
                        grid_eventos.Rows.Insert(0, "Inicia Sincronizacion Abono " & dr("NumFolio").ToString, Date.Now)
                        My.Application.DoEvents()
                        ssqlinsertal = "INSERT INTO abono(numnota,idcliente,cliente,fecha,abono,tipo_pago,sucursal) VALUES (" & dr("NumFolio").ToString & "," &
                                        dr("idcliente").ToString & ",'" & dr("cliente").ToString & "','" & Format(CDate(dr("fecha").ToString), "yyyy-MM-dd ") & "'," &
                                        Replace(dr("abono").ToString, ",", "") & ",'" & tipop & "'," & susursalr & ")"
                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                            '  MsgBox(ssqlinsertal)
                            ssql3 = "update AbonoI set cargado=1 where id=" & dr("Id").ToString
                            If odata.runSp(cnn, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Sincronización Abono " & dr("NumFolio").ToString, Date.Now)
                            End If
                        Else
                            MsgBox(sinfo)
                        End If
                    Next
                End If
                cnn2.Close()
            End If
            cnn.Close()
        Else
            MsgBox(sinfo)
        End If
    End Sub

    Private Sub subeVentasF()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select Folio,CodFactura from Ventas where CargadoF=0 and Totales > 0 and CodFactura <> '' and Status <> 'CANCELADA' order by Folio"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAutoFac, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then

                    For Each dr In dt.Rows
                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr1, "select * from det_fact where ticket = '" & dr("CodFactura").ToString & "'", sinfo) Then
                            odata.runSp(cnn, "update Ventas set CargadoF = 1 where Folio = " & dr("Folio").ToString & "", sinfo)
                        Else

                            Dim dt2 As New DataTable
                            Dim dr2 As DataRow
                            sSQL2 = "select * from VentasDetalle where Folio = " & dr("Folio").ToString & ""

                            If odata.getDt(cnn, dt2, sSQL2, sinfo) Then
                                For Each dr2 In dt2.Rows

                                    Dim sinfo10 As String = ""
                                    Dim odata10 As New ToolKitSQL.myssql
                                    Dim dt10 As New DataTable
                                    Dim dr10 As DataRow
                                    Dim varClaveSatDTE As String = ""
                                    Dim varUniMedSatDTE As String = ""
                                    Dim varIvaDTE As String = "0"

                                    If odata10.getDt(cnn, dt10, "select ClaveSat,UnidadSat,IVA from Productos where Codigo = '" & dr2("Codigo").ToString & "'", sinfo) Then
                                        For Each dr10 In dt10.Rows
                                            varClaveSatDTE = dr10("ClaveSat").ToString
                                            varUniMedSatDTE = dr10("UnidadSat").ToString
                                            varIvaDTE = IIf(dr10("IVA").ToString > 0, "16", "0")
                                        Next
                                    End If

                                    Dim opeiva As Decimal = 0
                                    Dim opeivaUni As Decimal = 0
                                    If CDec(varIvaDTE) > 0 Then
                                        opeiva = FormatNumber(CDec(dr2("Total").ToString) / 1.16, 6)
                                        opeivaUni = FormatNumber(opeiva / dr2("Cantidad").ToString, 6)
                                    Else
                                        opeiva = FormatNumber(dr2("Total").ToString, 6)
                                        opeivaUni = FormatNumber(opeiva / dr2("Cantidad").ToString, 6)
                                    End If

                                    ssqlinsertal = ""
                                    ssqlinsertal = "INSERT INTO det_fact(id_f, cve_pro, concepto, unidad, cant, pu, imp, tasa, ret_isr, ret_iva, descu, flete, objetoimp, ticket) " &
                                                                              " VALUES ('','" & varClaveSatDTE & "','" & Trim(RemoverCaracteresEspeciales(dr2("Nombre").ToString)) & "','" & varUniMedSatDTE & "','" & dr2("Cantidad").ToString & "','" & Trim(Replace(opeivaUni, ",", "")) & "','" & Trim(Replace(opeiva, ",", "")) & "','" & varIvaDTE & "','0','0','0.00','0','02','" & dr("CodFactura").ToString & "')"
                                    If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    End If
                                Next

                                odata.runSp(cnn, "update Ventas set CargadoF = 1 where Folio = " & dr("Folio").ToString & "", sinfo)

                            End If

                        End If

                        grid_eventos.Rows.Insert(0, "Finaliza Sincro Fact folio " & dr("Folio").ToString, Date.Now)

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subeProveedores()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "SELECT * FROM Proveedores WHERE Cargado=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim banderaentra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If es_matriz = 1 Then

                            For Each Me.dr_Sucursales In dt_Sucursales.Rows

                                Dim varcardado As Integer = 0

                                If dr_Sucursales("Id").ToString = susursalr Then
                                    varcardado = 0
                                Else
                                    varcardado = 1
                                End If

                                If odata2.getDr(cnn2, dr2, "select * from proveedores where IdLocal = " & dr("Id").ToString & " and NumSuc = " & dr_Sucursales("Id").ToString & " or NComercial = '" & dr("NComercial").ToString & "' and NumSuc = " & dr_Sucursales("Id").ToString & " ", sinfo) Then

                                    ssqlinsertal = ""
                                    ssqlinsertal = "update proveedores set `NComercial` = '" & dr("NComercial").ToString & "', `Compania` = '" & dr("Compañia").ToString & "', `RFC` = '" & dr("RFC").ToString & "', `CURP` = '" & dr("CURP").ToString & "', `Vendedor` = '" & dr("Vendedor").ToString & "', `Calle` = '" & dr("Calle").ToString &
                                                                      "', `Colonia` = '" & dr("Colonia").ToString & "', `Delegacion` = '" & dr("Delegacion").ToString & "', `EntFed` = '" & dr("EntFed").ToString & "', `CP` = '" & dr("CP").ToString & "', `Tel1` = '" & dr("Tel1").ToString & "', `Tel2` = '" & dr("Tel2").ToString & "', `Fax` = '" & dr("Fax").ToString & "', `Ext1` = '" & dr("Ext1").ToString &
                                                                      "', `Ext2` = '" & dr("Ext2").ToString & "', `Ext3` = '" & dr("Ext3").ToString & "', `Localizador` = '" & dr("Localizador").ToString & "', `PIN` = '" & dr("PIN").ToString & "', `TelMobil` = '" & dr("TelMobil").ToString & "', `PagWeb` = '" & dr("PagWeb").ToString & "', `Email` = '" & dr("Email").ToString & "', `NumClient` = '" &
                                                                      dr("NumClient").ToString & "', `TelPart` = '" & dr("TelPart").ToString & "', `VendedorMail` = '" & dr("VendedorMail").ToString & "', `Saldo` = '" & dr("Saldo").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `DiasCredito` = '" & dr("DiasCredito").ToString & "', `NumSuc` = " & dr_Sucursales("Id").ToString &
                                                                      ", Cargado = " & varcardado & " where Id = " & dr2("Id").ToString & " and NumSuc = " & dr_Sucursales("id").ToString & ""
                                    If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                        banderaentra += 1
                                    End If
                                Else

                                    ssqlinsertal = ""
                                    ssqlinsertal = "INSERT INTO proveedores(`IdLocal`, `NComercial`, `Compania`, `RFC`, `CURP`, `Vendedor`, `Calle`, `Colonia`, `Delegacion`, `EntFed`, `CP`, `Tel1`, `Tel2`, `Fax`, `Ext1`, `Ext2`, `Ext3`, `Localizador`, `PIN`, `TelMobil`, `PagWeb`, `Email`, `NumClient`, `TelPart`, `VendedorMail`, `Saldo`, `Credito`, `DiasCredito`, `NumSuc`, `Cargado`) " &
                                                                      " VALUES (" & IIf(varcardado = 0, dr("Id").ToString, 0) & ",'" & dr("NComercial").ToString & "','" & dr("Compañia").ToString & "','" & dr("RFC").ToString & "','" & dr("CURP").ToString & "','" & dr("Vendedor").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString &
                                                                      "','" & dr("Delegacion").ToString & "','" & dr("EntFed").ToString & "','" & dr("CP").ToString & "','" & dr("Tel1").ToString & "','" & dr("Tel2").ToString & "','" & dr("Fax").ToString & "','" & dr("Ext1").ToString & "','" & dr("Ext2").ToString &
                                                                      "','" & dr("Ext3").ToString & "','" & dr("Localizador").ToString & "','" & dr("PIN").ToString & "','" & dr("TelMobil").ToString & "','" & dr("PagWeb").ToString & "','" & dr("Email").ToString & "','" & dr("NumClient").ToString & "','" & dr("TelPart").ToString &
                                                                      "','" & dr("VendedorMail").ToString & "','" & dr("Saldo").ToString & "','" & dr("Credito").ToString & "','" & dr("DiasCredito").ToString & "'," & dr_Sucursales("Id").ToString & ", " & varcardado & ")"
                                    If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                        banderaentra += 1
                                    End If
                                End If

                            Next

                        Else

                            Dim varcardado As Integer = 0

                            If odata2.getDr(cnn2, dr2, "select * from proveedores where IdLocal = " & dr("Id").ToString & " and NumSuc = " & susursalr & " or NComercial = '" & dr("NComercial").ToString & "' and NumSuc = " & susursalr & " ", sinfo) Then

                                ssqlinsertal = ""
                                ssqlinsertal = "update proveedores set `NComercial` = '" & dr("NComercial").ToString & "', `Compania` = '" & dr("Compania").ToString & "', `RFC` = '" & dr("RFC").ToString & "', `CURP` = '" & dr("CURP").ToString & "', `Calle` = '" & dr("Calle").ToString & "', `Colonia` = '" & dr("Colonia").ToString & "', `Delegacion` = '" & dr("Delegacion").ToString & "', `EntFed` = '" & dr("Entidad").ToString & "', `CP` = '" & dr("CP").ToString & "', `Tel1` = '" & dr("Telefono").ToString & "', `PagWeb` = '" & dr("Facebook").ToString & "', `Email` = '" & dr("Correo").ToString & "', `Saldo` = '" & dr("Saldo").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `DiasCredito` = '" & dr("DiasCred").ToString & "', `NumSuc` = " & susursalr & ", Cargado = " & varcardado & " where Id = " & dr2("Id").ToString & " and NumSuc = " & susursalr & ""
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            Else

                                ssqlinsertal = ""
                                ssqlinsertal = "INSERT INTO proveedores(`IdLocal`, `NComercial`, `Compania`, `RFC`, `CURP`, `Calle`, `Colonia`, `Delegacion`, `EntFed`, `CP`, `Tel1`, `PagWeb`, `Email`, `Saldo`, `Credito`, `DiasCredito`, `NumSuc`, `Cargado`) " &
                                                                " VALUES (" & IIf(varcardado = 0, dr("Id").ToString, 0) & ",'" & dr("NComercial").ToString & "','" & dr("Compania").ToString & "','" & dr("RFC").ToString & "','" & dr("CURP").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString &
                                                                "','" & dr("Delegacion").ToString & "','" & dr("Entidad").ToString & "','" & dr("CP").ToString & "','" & dr("Telefono").ToString & "','" & dr("Facebook").ToString & "','" & dr("Correo").ToString & "','" & dr("Saldo").ToString & "','" & dr("Credito").ToString & "','" & dr("DiasCred").ToString & "'," & susursalr & ", " & varcardado & ")"
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            End If

                        End If



                        If banderaentra > 0 Then
                            odata.runSp(cnn, "update Proveedores set Cargado = 1 where Id = " & dr("Id").ToString & "", sinfo)
                            grid_eventos.Rows.Insert(0, "Proveedor sincronizado correctamente " & dr("NComercial").ToString, Date.Now)
                            My.Application.DoEvents()
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaProveedores()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from proveedores where Cargado=1 and NumSuc = " & susursalr & ""

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata.getDr(cnn, dr2, "select * from Proveedores where Id = " & dr("IdLocal").ToString & "", sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update Proveedores set NComercial = '" & dr("NComercial").ToString & "', Compania = '" & dr("Compania").ToString & "', RFC = '" & dr("RFC").ToString & "', CURP = '" & dr("CURP").ToString & "', Calle = '" & dr("Calle").ToString & "', Colonia = '" & dr("Colonia").ToString & "', Delegacion = '" & dr("Delegacion").ToString & "', Entidad = '" & dr("EntFed").ToString & "', CP = '" & dr("CP").ToString & "', Telefono = '" & dr("Tel1").ToString & "', Facebook = '" & dr("PagWeb").ToString & "', Correo = '" & dr("Email").ToString & "', Saldo = " & dr("Saldo").ToString & ", Credito = '" & dr("Credito").ToString & "', DiasCred = " & dr("DiasCredito").ToString & ", Cargado = 1 where Id = " & dr2("Id").ToString & ""

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update proveedores set Cargado = 0 where Id = " & dr("Id").ToString & " and NumSuc = " & susursalr & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Proveedor sincronizado correctamente " & dr("NComercial").ToString, Date.Now)
                            My.Application.DoEvents()

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO Proveedores(NComercial, Compania, RFC, CURP, Calle, Colonia, Delegacion, Entidad, CP, Telefono, Facebook, Correo, Saldo, Credito, DiasCred, Cargado) " &
                                                              " VALUES ('" & dr("NComercial").ToString & "','" & dr("Compania").ToString & "','" & dr("RFC").ToString & "','" & dr("CURP").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString &
                                                              "','" & dr("Delegacion").ToString & "','" & dr("EntFed").ToString & "','" & dr("CP").ToString & "','" & dr("Tel1").ToString & "','" & dr("PagWeb").ToString & "','" & dr("Email").ToString & "','" & dr("Saldo").ToString & "','" & dr("Credito").ToString & "','" & dr("DiasCredito").ToString & "', 1)"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update proveedores set IdLocal = " & dameMaxIdProv() & ", Cargado = 0 where Id = " & dr("Id").ToString & " and NumSuc = " & susursalr & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Proveedor sincronizado correctamente " & dr("NComercial").ToString, Date.Now)

                            My.Application.DoEvents()

                        End If


                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subeClientes()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from Clientes where Cargado=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim banderaentra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If es_matriz = 1 Then

                            For Each Me.dr_Sucursales In dt_Sucursales.Rows

                                My.Application.DoEvents()

                                Dim varcardado As Integer = 0

                                If dr_Sucursales("Id").ToString = susursalr Then
                                    varcardado = 0
                                Else
                                    varcardado = 1
                                End If

                                If odata2.getDr(cnn2, dr2, "select * from clientes where IdLocal = " & dr("Id").ToString & " and NumSuc = " & dr_Sucursales("Id").ToString & " or Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & dr_Sucursales("Id").ToString & " ", sinfo) Then

                                    ssqlinsertal = ""
                                    ssqlinsertal = "update clientes set `Nombre` = '" & dr("Nombre").ToString & "', `RazonSocial` = '" & dr("RazonSocial").ToString & "', `Contacto` = '" & dr("Contacto").ToString & "', `Tipo` = '" & dr("Tipo").ToString & "', `RFC` = '" & dr("RFC").ToString & "', `CURP` = '" & dr("CURP").ToString & "', `Calle` = '" & dr("Calle").ToString &
                                                                      "', `Colonia` = '" & dr("Colonia").ToString & "', `Delegacion` = '" & dr("Delegacion").ToString & "', `Entidad` = '" & dr("Entidad").ToString & "', `CP` = '" & dr("CP").ToString & "', `Telefono1` = '" & dr("Telefono1").ToString & "', `Telefono2` = '" & dr("Telefono2").ToString & "', `Fax` = '" & dr("Fax").ToString & "', `Ext1` = '" & dr("Ext1").ToString &
                                                                      "', `Ext2` = '" & dr("Ext2").ToString & "', `Ext3` = '" & dr("Ext3").ToString & "', `Radio` = '" & dr("Radio").ToString & "', `Nip` = '" & dr("Nip").ToString & "', `Cel` = '" & dr("Cel").ToString & "', `Web` = '" & dr("Web").ToString & "', `Email` = '" & dr("Email").ToString & "', `TelParticular` = '" &
                                                                      dr("TelParticular").ToString & "', `ContactoMail` = '" & dr("ContactoMail").ToString & "', `Cumple` = '" & dr("Cumple").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `Nota` = '" & dr("Nota").ToString & "', `Comisionista` = '" & dr("Comisionista").ToString & "', `DiasCredito` = '" & dr("DiasCredito").ToString & "', `SuspVent` = '" & dr("SuspVent").ToString & "', `CNumberExt` = '" & dr("CNumberExt").ToString & "', `CNumberInt` = '" & dr("CNumberInt").ToString & "', `CPais` = '" & dr("CPais").ToString & "' , `NumSuc` = " & dr_Sucursales("Id").ToString &
                                                                      ", Cargado = " & varcardado & " where Id = " & dr2("Id").ToString & " and NumSuc = " & dr_Sucursales("id").ToString & ""
                                    If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                        banderaentra += 1
                                    End If
                                Else

                                    ssqlinsertal = ""
                                    ssqlinsertal = "INSERT INTO clientes(`IdLocal`, `Nombre`, `RazonSocial`, `Contacto`, `Tipo`, `RFC`, `CURP`, `Calle`, `Colonia`, `Delegacion`, `Entidad`, `CP`, `Telefono1`, `Telefono2`, `Fax`, `Ext1`, `Ext2`, `Ext3`, `Radio`, `Nip`, `Cel`, `Web`, `Email`, `TelParticular`, `ContactoMail`, `Cumple`, `Credito`, `Nota`, `Comisionista`, `DiasCredito`, `SuspVent`, `CNumberExt`, `CNumberInt`, `CPais`, `NumSuc`, `Cargado`) " &
                                                                      " VALUES (" & IIf(varcardado = 0, dr("Id").ToString, 0) & ",'" & dr("Nombre").ToString & "','" & dr("RazonSocial").ToString & "','" & dr("Contacto").ToString & "','" & dr("Tipo").ToString & "','" & dr("RFC").ToString & "','" & dr("CURP").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString &
                                                                      "','" & dr("Delegacion").ToString & "','" & dr("Entidad").ToString & "','" & dr("CP").ToString & "','" & dr("Telefono1").ToString & "','" & dr("Telefono2").ToString & "','" & dr("Fax").ToString & "','" & dr("Ext1").ToString & "','" & dr("Ext2").ToString &
                                                                      "','" & dr("Ext3").ToString & "','" & dr("Radio").ToString & "','" & dr("Nip").ToString & "','" & dr("Cel").ToString & "','" & dr("Web").ToString & "','" & dr("Email").ToString & "','" & dr("TelParticular").ToString & "','" & dr("ContactoMail").ToString &
                                                                      "','" & dr("Cumple").ToString & "','" & Replace(dr("Credito").ToString, ",", "") & "','" & dr("Nota").ToString & "','" & dr("Comisionista").ToString & "','" & dr("DiasCredito").ToString & "'," & dr("SuspVent").ToString & ",'" & dr("CNumberExt").ToString & "','" & dr("CNumberInt").ToString & "','" & dr("CPais").ToString & "'," & dr_Sucursales("Id").ToString & ", " & varcardado & ")"
                                    If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                        banderaentra += 1
                                    End If
                                End If

                            Next

                        Else

                            Dim varcardado As Integer = 0

                            If odata2.getDr(cnn2, dr2, "select * from clientes where IdLocal = " & dr("Id").ToString & " and NumSuc = " & susursalr & " or Nombre = '" & dr("Nombre").ToString & "' and NumSuc = " & susursalr & " ", sinfo) Then

                                ssqlinsertal = ""
                                ssqlinsertal = "update clientes set `Nombre` = '" & dr("Nombre").ToString & "', `RazonSocial` = '" & dr("RazonSocial").ToString & "', `Tipo` = '" & dr("Tipo").ToString & "', `RFC` = '" & dr("RFC").ToString & "', `Calle` = '" & dr("Calle").ToString & "', `Colonia` = '" & dr("Colonia").ToString & "', `Delegacion` = '" & dr("Delegacion").ToString & "', `Entidad` = '" & dr("Entidad").ToString & "', `CP` = '" & dr("CP").ToString & "', `Telefono1` = '" & dr("Telefono").ToString & "', `Email` = '" & dr("Correo").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `Comisionista` = '" & dr("Comisionista").ToString & "', `DiasCredito` = '" & dr("DiasCred").ToString & "', `SuspVent` = '" & dr("Suspender").ToString & "', `CNumberExt` = '" & dr("NExterior").ToString & "', `CNumberInt` = '" & dr("NInterior").ToString & "', `CPais` = '" & dr("Pais").ToString & "' , `NumSuc` = " & susursalr &
                                                                ", Cargado = " & varcardado & " where Id = " & dr2("Id").ToString & " and NumSuc = " & dr_Sucursales("id").ToString & ""
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            Else

                                ssqlinsertal = ""
                                ssqlinsertal = "INSERT INTO clientes(`IdLocal`, `Nombre`, `RazonSocial`, `Tipo`, `RFC`, `Calle`, `Colonia`, `Delegacion`, `Entidad`, `CP`, `Telefono1`, `Email`, `Credito`, `Comisionista`, `DiasCredito`, `SuspVent`, `CNumberExt`, `CNumberInt`, `CPais`, `NumSuc`, `Cargado`) " &
                                                                " VALUES (" & IIf(varcardado = 0, dr("Id").ToString, 0) & ",'" & dr("Nombre").ToString & "','" & dr("RazonSocial").ToString & "','" & dr("Tipo").ToString & "','" & dr("RFC").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString & "','" & dr("Delegacion").ToString & "','" & dr("Entidad").ToString & "','" & dr("CP").ToString & "','" & dr("Telefono").ToString & "','" & dr("Correo").ToString & "','" & Replace(dr("Credito").ToString, ",", "") & "','" & dr("Comisionista").ToString & "','" & dr("DiasCred").ToString & "'," & dr("Suspender").ToString & ",'" & dr("NExterior").ToString & "','" & dr("NInterior").ToString & "','" & dr("Pais").ToString & "'," & susursalr & ", " & varcardado & ")"
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            End If

                        End If




                        If banderaentra > 0 Then
                            odata.runSp(cnn, "update Clientes set Cargado = 1 where Id = " & dr("Id").ToString & "", sinfo)
                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & dr("Nombre").ToString, Date.Now)
                            My.Application.DoEvents()
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaClientes()

        Dim cnn As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from clientes where Cargado=1 and NumSuc = " & susursalr & ""

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdSincro, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata.getDr(cnn, dr2, "select * from Clientes where Id = " & dr("IdLocal").ToString & "", sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update Clientes set `Nombre` = '" & dr("Nombre").ToString & "', `RazonSocial` = '" & dr("RazonSocial").ToString & "', `Tipo` = '" & dr("Tipo").ToString & "', `RFC` = '" & dr("RFC").ToString & "', `Calle` = '" & dr("Calle").ToString & "', `Colonia` = '" & dr("Colonia").ToString & "', `Delegacion` = '" & dr("Delegacion").ToString & "', `Entidad` = '" & dr("Entidad").ToString & "', `CP` = '" & dr("CP").ToString & "', `Telefono` = '" & dr("Telefono1").ToString & "', `Correo` = '" & dr("Web").ToString & "', `Credito` = '" & dr("Credito").ToString & "', `Comisionista` = '" & dr("Comisionista").ToString & "', `DiasCred` = '" & dr("DiasCredito").ToString & "', `Suspender` = '" & dr("SuspVent").ToString & "', `NExterior` = '" & dr("CNumberExt").ToString & "', `NInterior` = '" & dr("CNumberInt").ToString & "', `Pais` = '" & dr("CPais").ToString & "', Cargado = 1 where Id = " & dr2("Id").ToString & ""

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update clientes set Cargado = 0 where Id = " & dr("Id").ToString & " and NumSuc = " & susursalr & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & dr("Nombre").ToString, Date.Now)
                            My.Application.DoEvents()

                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO Clientes(`Id`, `Nombre`, `RazonSocial`, `Tipo`, `RFC`, `Calle`, `Colonia`, `Delegacion`, `Entidad`, `CP`, `Telefono`, `Correo`, `Credito`, `Comisionista`, `DiasCred`, `Suspender`, `NExterior`, `NInterior`, `Pais`, `Cargado`) " &
                                                          " VALUES (" & dameMaxIdCli() + 1 & ",'" & dr("Nombre").ToString & "','" & dr("RazonSocial").ToString & "','" & dr("Tipo").ToString & "','" & dr("RFC").ToString & "','" & dr("Calle").ToString & "','" & dr("Colonia").ToString & "','" & dr("Delegacion").ToString & "','" & dr("Entidad").ToString & "','" & dr("CP").ToString & "','" & dr("Telefono1").ToString & "','" & dr("Web").ToString & "','" & dr("Email").ToString & "','" & Replace(dr("Credito").ToString, ",", "") & "','" & dr("Comisionista").ToString & "','" & dr("DiasCredito").ToString & "'," & dr("SuspVent").ToString & ",'" & dr("CNumberExt").ToString & "','" & dr("CNumberInt").ToString & "','" & dr("CPais").ToString & "', 1)"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update clientes set IdLocal = " & dameMaxIdCli() & ", Cargado = 0 where Id = " & dr("Id").ToString & " and NumSuc = " & susursalr & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & dr("Nombre").ToString, Date.Now)

                            My.Application.DoEvents()

                        End If


                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub ACTPROEXISUC(ByVal varCodigo As String, ByVal varDesc As String)

        Dim cnn100 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2100 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim ssql As String = ""
        ssql = "Select Codigo,Nombre,IVA,UVenta,PrecioCompra,PorcMin,PorcMay,PorcMM,PorcEsp,Porcentaje,PreMin,PreMay," &
                "PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin1,CantMay1,CantMM1,CantEsp1,CantLst1,CantMin2,CantMay2,CantMM2,CantEsp2," &
                "CantLst2,id_tbMoneda,Departamento,Grupo,Existencia,ClaveSat,UnidadSat,ProvPri,MCD,Multiplo,CodBarra,IIEPS from Productos where Codigo='" & varCodigo & "'"
        Dim ssql2 As String = ""
        Dim actpreprosuc As String = ""
        Dim dt100 As New DataTable
        Dim dr100 As DataRow
        Dim dr2100 As DataRow
        Dim sinfo100 As String = ""
        Dim odata100 As New ToolKitSQL.myssql
        Dim odata2100 As New ToolKitSQL.myssql
        Dim newExistt As Double = 0

        If odata100.dbOpen(cnn100, sTargetlocal, sinfo100) Then
            If odata2100.dbOpen(cnn2100, sTargetdSincro, sinfo100) Then

                If odata100.getDt(cnn100, dt100, ssql, sinfo100) Then
                    For Each dr100 In dt100.Rows

                        My.Application.DoEvents()

                        If num_Sucursales > 1 Then

                            dt_Sucursales = New DataTable

                            If odata2100.getDt(cnn2100, dt_Sucursales, "select * from sucursales", sinfo100) Then
                                For Each Me.dr_Sucursales In dt_Sucursales.Rows

                                    If susursalr <> dr_Sucursales("id").ToString Then

                                        If odata2100.getDr(cnn2100, dr2100, "select * from productos where Codigo='" & dr100("Codigo").ToString & "' and NumSuc=" & dr_Sucursales("id").ToString & "", sinfo100) Then

                                            actpreprosuc = ""
                                            actpreprosuc = "update productos set IVA=" & dr100("IVA").ToString & ",UVenta='" & dr100("UVenta").ToString & "',PrecioCompra='" & dr100("PrecioCompra").ToString & "',PorcentageMin='" & dr100("PorcentageMin").ToString & "',PorMay='" & dr100("PorMay") & "',PorMM='" & dr100("PorcMM").ToString & "',PorEsp='" & dr100("PorcEsp").ToString & "',Porcentage='" & dr100("Porcentaje").ToString & "',PecioVentaMinIVA='" & dr100("PreMin").ToString & "',PreMay='" & dr100("PreMay").ToString & "',PreMM='" & dr100("PreMM").ToString & "',PreEsp='" & dr100("PreEsp").ToString & "',PrecioVentaIVA='" & dr100("PrecioVentaIVA").ToString & "',PrecioVenta='" & dr100("PrecioVenta").ToString & "',pres_vol='" & dr100("pres_vol").ToString & "',CantMin='" & dr100("CantMin1").ToString & "',CantMay='" & dr100("CantMay1").ToString & "',CantMM='" & dr100("CantMM1").ToString & "',CantEsp='" & dr100("CantEsp1").ToString & "',CantLta='" & dr100("CantLst1").ToString & "',CantMin2='" & dr100("CantMin2").ToString & "',CantMay2='" & dr100("CantMay2").ToString & "',CantMM2='" & dr100("CantMM2").ToString & "',CantEsp2='" & dr100("CantEsp2").ToString & "',CantLta2='" & dr100("CantLst2").ToString & "',id_tbMoneda='1',Depto='" & dr100("Departamento").ToString & "',Grupo='" & dr100("Grupo").ToString & "',proveedor='" & dr100("ProvPri").ToString & "',exitencia='0',clavesat='" & dr100("ClaveSat").ToString & "',claveunisat='" & dr100("UnidadSat").ToString & "',CodBarra = '" & dr100("CodBarra").ToString & "', IIEPS = " & IIf(IsNumeric(dr100("IIEPS").ToString), dr100("IIEPS").ToString, 0) & ", MCD = " & IIf(IsNumeric(dr100("MCD").ToString), dr100("MCD").ToString, 1) & ", Multiplo = " & IIf(IsNumeric(dr100("Multiplo").ToString), dr100("Multiplo").ToString, 1) & ",Cargado = 1 where Codigo='" & dr100("Codigo").ToString & "' and NumSuc = " & dr_Sucursales("id").ToString & ""

                                            If odata2100.runSp(cnn2100, actpreprosuc, sinfo100) Then

                                            End If
                                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr100("Nombre").ToString, Date.Now)
                                        Else
                                            '''''''''''''''''''''''''''''''''''''''
                                            actpreprosuc = ""
                                            actpreprosuc = "Insert productos(Codigo,Nombre,IVA,UVenta,PrecioCompra,PorcentageMin,PorMay,PorMM,PorEsp,Porcentage,PecioVentaMinIVA,PreMay,PreMM,PreEsp,PrecioVentaIVA,PrecioVenta,pres_vol,CantMin,CantMay,CantMM,CantEsp,CantLta,CantMin2,CantMay2,CantMM2,CantEsp2,CantLta2,id_tbMoneda,Depto,Grupo,proveedor,exitencia,clavesat,claveunisat,CodBarra,IIEPS,MCD,Multiplo,NumSuc,Cargado) values('" & dr100("Codigo").ToString & "','" & dr100("Nombre").ToString & "'," & dr100("IVA").ToString & ",'" & dr100("UVenta").ToString & "','" & dr100("PrecioCompra").ToString & "','" & dr100("PorMin").ToString & "','" & dr100("PorMay").ToString & "','" & dr100("PorMM").ToString & "','" & dr100("PorEsp").ToString & "','" & dr100("Porcentaje").ToString & "','" & dr100("PreMin").ToString & "','" & dr100("PreMay").ToString & "','" & dr100("PreMM").ToString & "','" & dr100("PreEsp").ToString & "','" & dr100("PrecioVentaIVA").ToString & "','" & dr100("PrecioVenta").ToString & "','" & dr100("pres_vol").ToString & "','" & dr100("CantMin1").ToString & "','" & dr100("CantMay1").ToString & "','" & dr100("CantMM1").ToString & "','" & dr100("CantEsp1").ToString & "','" & dr100("CantLst1").ToString & "','" & dr100("CantMin2").ToString & "','" & dr100("CantMay2").ToString & "','" & dr100("CantMM2").ToString & "','" & dr100("CantEsp2").ToString & "','" & dr100("CantLst2").ToString & "','1','" & dr100("Departamento").ToString & "','" & dr100("Grupo").ToString & "','" & dr100("ProvPri").ToString & "','0','" & dr100("ClaveSat").ToString & "','" & dr100("UnidadSat").ToString & "','" & dr100("CodBarra").ToString & "'," & IIf(IsNumeric(dr100("IIEPS").ToString), dr100("IIEPS").ToString, 0) & "," & IIf(IsNumeric(dr100("MCD").ToString), dr100("MCD").ToString, 0) & "," & IIf(IsNumeric(dr100("Multiplo").ToString), dr100("Multiplo").ToString, 0) & "," & dr_Sucursales("id").ToString & ",1)"

                                            If odata2100.runSp(cnn2100, actpreprosuc, sinfo100) Then

                                            End If
                                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr100("Nombre").ToString, Date.Now)
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    Next
                End If
                dt_Sucursales = New DataTable
                odata2100.getDt(cnn2100, dt_Sucursales, "select * from sucursales", sinfo100)

                cnn2100.Close()
            End If
            cnn100.Close()
        End If
    End Sub

    Private Sub actualizarLoteCad(ByVal codigo As String, ByVal lote As String, ByVal fechacad As String, ByVal cantidad As Integer, ByVal tipo As Integer)

        Dim cnn100 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim cnn2100 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = "Select * from LoteCaducidad where Codigo = '" & Trim(codigo) & "' and Cantidad > 0 and Lote='" & lote & "'"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt100 As New DataTable
        Dim dt2100 As New DataTable
        Dim dr100 As DataRow
        Dim sinfo As String = ""
        Dim odata100 As New ToolKitSQL.myssql
        Dim odata2100 As New ToolKitSQL.myssql
        Dim banderaentra As Integer = 0

        If odata100.dbOpen(cnn100, sTargetlocal, sinfo) Then

            If odata2100.dbOpen(cnn2100, sTargetdSincro, sinfo) Then
                If odata100.getDt(cnn100, dt100, sSQL, sinfo) Then
                    For Each dr100 In dt100.Rows

                        My.Application.DoEvents()
                        ssqlinsertal = ""
                        If tipo = 1 Then
                            If Trim(dr100("Lote").ToString) = Trim(lote) Then
                                banderaentra = 1
                                ssqlinsertal = "Update LoteCaducidad set Cantidad = " & CInt(dr100("Cantidad").ToString) + cantidad & " where id = " & dr100("id").ToString & ""
                            End If
                        Else
                            If Trim(dr100("Lote").ToString) = Trim(lote) Then
                                banderaentra = 1
                                ssqlinsertal = "Update LoteCaducidad set Cantidad = " & CInt(dr100("Cantidad").ToString) - cantidad & " where id = " & dr100("id").ToString & ""
                            End If
                        End If
                        If odata100.runSp(cnn100, ssqlinsertal, sinfo) Then

                        End If
                    Next

                    If banderaentra = 0 Then
                        ssqlinsertal = "insert into LoteCaducidad(Codigo,Lote,FechaCaducidad,Cantidad) values('" & Trim(codigo) & "','" & Trim(lote) & "','" & Trim(fechacad) & "'," & Trim(cantidad) & ")"
                        If odata100.runSp(cnn100, ssqlinsertal, sinfo) Then

                        End If
                    End If

                Else
                    ssqlinsertal = ""
                    If tipo = 1 Then
                        ssqlinsertal = "insert into LoteCaducidad(Codigo,Lote,FechaCaducidad,Cantidad) values('" & Trim(codigo) & "','" & Trim(lote) & "','" & Trim(fechacad) & "'," & Trim(cantidad) & ")"
                    Else
                    End If
                    If odata100.runSp(cnn100, ssqlinsertal, sinfo) Then
                    End If
                End If
                cnn2100.Close()
            End If
            cnn100.Close()

        End If

    End Sub
    Private Sub get_sucursales()
        es_matriz = 0
    End Sub

    Private Sub Licencia()
        'Dim ULocal As String
        'Dim Linea As Integer
        'Dim FileSerie As String
        'Dim SerieLib As String
        'Dim SFile As String

        'ULocal = Environment.SystemDirectory & "\1drno1.dll"
        'FileSerie = Environment.SystemDirectory & "\1dsl1.dll"

        'If FileIO.FileSystem.FileExists(FileSerie) = False Then

        '    If FileIO.FileSystem.FileExists(ULocal) Then

        '        Linea = redCont(ULocal) + 1

        '        If Linea <= 0 Or Linea >= 30 Then
        '            frmLicencia.MdiParent = Me
        '            frmLicencia.Show()
        '        Else

        '            If WriteCont(Linea, ULocal) = False Then
        '                End
        '            End If

        '            MsgBox("Perido de evaluación: " & Linea & " de 30")
        '        End If

        '    Else

        '        MsgBox("Perido de evaluación: 1 de 30")

        '        If WriteCont(1, ULocal) = False Then
        '            End
        '        End If

        '    End If
        'Else
        '    SerieLib = frmLicencia.GenLicencia(frmLicencia.SerialNumber())

        '    SFile = redSerie(FileSerie)
        '    If SerieLib <> SFile Then
        '        MsgBox("La licencia de este Sistema Incorrecta.", vbInformation)
        '        End
        '    End If
        'End If
    End Sub

    Public Function redSerie(ByVal root As String) As String
        Dim readFile As New StreamReader(root)
        Dim datos As String

        datos = readFile.ReadLine
        readFile.Close()
        redSerie = datos
    End Function

    Public Function redCont(ByVal root As String) As Integer
        Dim readFile As New StreamReader(root)
        Dim datos As Integer

        datos = readFile.ReadLine
        readFile.Close()
        redCont = datos
    End Function

    Public Function WriteCont(ByVal linea As Integer, ByVal root_file As String) As Boolean
        Dim Datos As Stream
        Dim StrWrite As StreamWriter

        Try
            Datos = File.Open(root_file, IO.FileMode.Create, IO.FileAccess.Write)
            Datos.Seek(0, IO.SeekOrigin.Begin)
            StrWrite = New StreamWriter(Datos)
            StrWrite.WriteLine(linea)
            StrWrite.Close()
            WriteCont = True

        Catch e As IOException
            MsgBox(e.Message)
            WriteCont = False
        End Try
    End Function

    Function dameMaxIdCli() As Integer
        Dim cnnPro As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Id) from Clientes"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Function dameMaxIdProv() As Integer
        Dim cnnPro As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Id) from Proveedores"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(drPro(0).ToString)
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Function RemoverCaracteresEspeciales(input As String) As String
        ' Patrón de expresión regular para encontrar caracteres especiales
        Dim patron As String = "[^\w\s]"

        ' Reemplaza los caracteres especiales con una cadena vacía
        Dim resultado As String = Regex.Replace(input, patron, "")

        Return resultado
    End Function

    Private Sub subeEmpleadosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = "Select * from usuarios where CargadoAndroid=0"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr1 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows
                        My.Application.DoEvents()
                        If odata2.getDr(cnn2, dr1, "select * from usuarios where Nombre = '" & Trim(dr("Nombre").ToString) & "'", sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update usuarios set Reimprimir = 1, Abonos = 1,  Alias = '" & Trim(dr("Alias").ToString) & "', Clave = '" & Trim(dr("Clave").ToString) & "' where Nombre = '" & Trim(dr("Nombre").ToString) & "'"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update usuarios set CargadoAndroid = 1 where IdEmpleado = " & dr("IdEmpleado").ToString & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Empleado " & dr("Nombre").ToString, Date.Now)

                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO usuarios(Nombre, Alias, Clave) " &
                                " VALUES ('" & Trim(dr("Nombre").ToString) & "','" & Trim(dr("Alias").ToString) & "','" & Trim(dr("Clave").ToString) & "')"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update usuarios set CargadoAndroid = 1 where IdEmpleado = " & dr("IdEmpleado").ToString & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Empleado " & dr("Nombre").ToString, Date.Now)

                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subeProductosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Codigo,Nombre,IVA,UVenta,PrecioVentaIVA,Departamento,Grupo,CodBarra,PrecioVenta,PreMay,PreMM,PreEsp from productos where CargadoAndroid=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim newExistt As Double = 0
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr2, "select Id from productos where Codigo = '" & Trim(dr("Codigo").ToString) & "'", sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update productos set Descripcion = '" & dr("Nombre").ToString & "', Depto = '" & dr("Departamento").ToString & "', Grupo = '" & dr("Grupo").ToString & "', IVA = " & dr("IVA").ToString & ", UVenta = '" & dr("UVenta").ToString & "', Precio = " & Replace(dr("PrecioVentaIVA").ToString, ",", "") &
                                            ", CodBarras = '" & dr("CodBarra").ToString & "', PrecioMin = " & dr("PrecioVenta").ToString & ", PrecioMay = " & dr("PreMay").ToString & ", PrecioMM = " & dr("PreMM").ToString & ", PrecioE = " & dr("PreEsp").ToString & " where Id = " & dr2("Id").ToString & ""
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update productos set CargadoAndroid = 1 where Codigo = '" & dr("Codigo").ToString & "'", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr("Nombre").ToString, Date.Now)

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO productos(Codigo,Descripcion,IVA,UVenta,Precio,Depto,Grupo,CodBarras,PrecioMin,PrecioMay,PrecioMM,PrecioE) " &
                                            " VALUES ('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "'," & dr("IVA").ToString & ",'" & dr("UVenta").ToString &
                                            "'," & dr("PrecioVentaIVA").ToString & ",'" & dr("Departamento").ToString & "','" & dr("Grupo").ToString & "','" & dr("CodBarra").ToString &
                                            "'," & dr("PrecioVenta").ToString & "," & dr("PreMay").ToString & "," & dr("PreMM").ToString & "," & dr("PreEsp").ToString & ")"


                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update productos set CargadoAndroid = 1 where Codigo = '" & dr("Codigo").ToString & "'", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Finaliza Sincronizacion Producto " & dr("Nombre").ToString, Date.Now)

                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subeClientesAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from clientes where CargadoAndroid=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim banderaentra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim varcardado As Integer = 0

                        Dim nuevotipo As String = ""

                        Select Case Trim(dr("Tipo").ToString)
                            Case "Lista"
                                nuevotipo = "Lista"
                            Case "Especial"
                                nuevotipo = "Especial"
                            Case "Medio Mayoreo"
                                nuevotipo = "Medio Mayoreo"
                            Case "Mayoreo"
                                nuevotipo = "Mayoreo"
                            Case "Mínimo"
                                nuevotipo = "Minimo"
                            Case Else
                                nuevotipo = "Lista"
                        End Select

                        If odata2.getDr(cnn2, dr2, "select * from clientes where Nombre = '" & Trim(dr("Nombre").ToString) & "'", sinfo) Then
                            ssqlinsertal = ""
                            ssqlinsertal = "update clientes set `TipoCliente` = '" & nuevotipo & "', `Calle` = '" & Trim(dr("Calle").ToString) & "', `Colonia` = '" & Trim(dr("Colonia").ToString) & "', `Delegacion` = '" & Trim(dr("Delegacion").ToString) & "', `NumExt` = '" & Trim(dr("NExterior").ToString) & "', `Telefono` = '" & Trim(dr("Telefono").ToString) &
                                            "', Bajado = 2, Credito = " & IIf(IsNumeric(Replace(Trim(dr("Credito").ToString), ",", "")), Replace(Trim(dr("Credito").ToString), ",", ""), 0) & ", Saldo = " & dameSaldoCliente(dr("Id").ToString) & " where Nombre = '" & Trim(dr("Nombre").ToString) & "'"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                banderaentra += 1
                            End If
                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO clientes(`Nombre`, `RazonSocial`, `Calle`, `Colonia`, `Delegacion`, `NumExt`, `Telefono`,`Bajado`, Credito, Saldo,TipoCliente) " &
                                            " VALUES ('" & Trim(dr("Nombre").ToString) & "','" & Trim(dr("RazonSocial").ToString) & "','" & Trim(dr("Calle").ToString) & "','" & Trim(dr("Colonia").ToString) & "','" & Trim(dr("Delegacion").ToString) & "','" & Trim(dr("NExterior").ToString) & "','" & Trim(dr("Telefono").ToString) & "',2," & IIf(IsNumeric(Replace(Trim(dr("Credito").ToString), ",", "")), Replace(Trim(dr("Credito").ToString), ",", ""), 0) & "," & dameSaldoCliente(dr("Id").ToString) & ",'" & nuevotipo & "')"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                banderaentra += 1
                            End If
                        End If

                        If banderaentra > 0 Then
                            odata.runSp(cnn, "update clientes set CargadoAndroid = 1 where Id = " & dr("Id").ToString & "", sinfo)
                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                            My.Application.DoEvents()
                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameSaldoCliente(ByVal varidcliente As Integer) As Double
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Id) as maxi from abonoi where IdCliente = " & varidcliente
        Dim drPro As DataRow
        Dim drPro1 As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                If odataPro.getDr(cnnPro, drPro1, "Select Saldo from abonoi where Id = " & drPro(0).ToString, sinfoPro) Then
                    cnnPro.Close()
                    Return CInt(IIf(IsNumeric(drPro1(0).ToString), drPro1(0).ToString, 0))
                Else
                    cnnPro.Close()
                    Return 0
                End If
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub subeDatosTicketAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from datosnegocio where CargadoAndroid=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim banderaentra As Integer = 0

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim varcardado As Integer = 0

                        If Trim(dr("Em_RazonSocial").ToString) <> "" Then

                            If odata2.getDr(cnn2, dr2, "select * from datosticket", sinfo) Then
                                ssqlinsertal = ""
                                ssqlinsertal = "update datosticket set RazonSocial = '" & Trim(dr("Em_RazonSocial").ToString) & "', RFC = '" & Trim(dr("Em_rfc").ToString) & "', Sucursal = '', Calle = '" & Trim(dr("Em_calle").ToString) & "', Colonia = '" & Trim(dr("Em_colonia").ToString) & "', DelMun = '" & Trim(dr("Em_Municipio").ToString) & "', Estado = '" & Trim(dr("Em_Estado").ToString) & "', Telefono = '" & Trim(dr("Em_Tel").ToString) & "', Pie = '', Comentario = '', Bajado = 2"
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            Else
                                ssqlinsertal = ""
                                ssqlinsertal = "INSERT INTO datosticket(RazonSocial, RFC, Sucursal, Calle, Colonia, DelMun, Estado, Telefono, Pie, Comentario, Bajado) " &
                                                " VALUES ('" & Trim(dr("Em_RazonSocial").ToString) & "','" & Trim(dr("Em_rfc").ToString) & "','','" & Trim(dr("Em_calle").ToString) & "','" & Trim(dr("Em_colonia").ToString) & "','" & Trim(dr("Em_Municipio").ToString) & "','" & Trim(dr("Em_Estado").ToString) & "','" & Trim(dr("Em_Tel").ToString) & "','','',2)"
                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    banderaentra += 1
                                End If
                            End If

                            If banderaentra > 0 Then
                                odata.runSp(cnn, "update datosnegocio set CargadoAndroid = 1 where Emisor_id = " & dr("Emisor_id").ToString & "", sinfo)
                                grid_eventos.Rows.Insert(0, "Datos Ticket sincronizado correctamente", Date.Now)
                                My.Application.DoEvents()
                            End If

                        Else

                            odata.runSp(cnn, "update datosnegocio set CargadoAndroid = 1 where Id = " & dr("Id").ToString & "", sinfo)
                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                            My.Application.DoEvents()

                        End If



                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaClientesAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from clientes where Bajado=0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If odata.getDr(cnn, dr2, "select * from clientes where Nombre = '" & Trim(dr("Nombre").ToString) & "'", sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update clientes set Calle = '" & Trim(dr("Calle").ToString) & "', Colonia = '" & Trim(dr("Colonia").ToString) & "', Delegacion = '" & Trim(dr("Delegacion").ToString) & "', NExterior = '" & Trim(dr("NumExt").ToString) & "', Telefono = '" & Trim(dr("Telefono").ToString) & "', CargadoAndroid = 1 where Nombre = '" & Trim(dr("Nombre").ToString) & "'"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update clientes set Bajado = 2 where Id = " & dr("Id").ToString & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                            My.Application.DoEvents()

                        Else
                            Dim ope As Integer = dameMaxIdCli()
                            ope = ope + 1
                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO clientes(Id, Nombre, RazonSocial, Calle, Colonia, Delegacion, NExterior, Telefono, CargadoAndroid, Credito) " &
                                            " VALUES (" & ope & ",'" & Trim(dr("Nombre").ToString) & "','" & Trim(dr("Nombre").ToString) & "','" & Trim(dr("Calle").ToString) & "','" & Trim(dr("Colonia").ToString) & "','" & Trim(dr("Delegacion").ToString) & "','" & Trim(dr("NumExt").ToString) & "','" & Trim(dr("Telefono").ToString) & "',1,'0')"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                odata2.runSp(cnn2, "update clientes set Bajado = 2 where Id = " & dr("Id").ToString & "", sinfo)
                            End If

                            grid_eventos.Rows.Insert(0, "Cliente sincronizado correctamente " & Trim(dr("Nombre").ToString), Date.Now)

                            My.Application.DoEvents()

                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaVentasAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from ventas where Bajado=0 order by Id"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim ope As Integer = 0

                        If Trim(dr("NomCliente").ToString) <> "" Then
                            ope = dameIdCli(dr("NomCliente").ToString)
                        End If

                        Dim drvalida As DataRow




                        ' If odata.getDr(cnn, drvalida, "select * from Ventas where HVenta = #" & CDate(dr("FechaHora").ToString) & "#",  sinfo) Then
                        If odata.getDr(cnn, drvalida, "select * from ventas where HVenta2 = '" & dr("FechaHora").ToString & "'", sinfo) Then

                            odata2.runSp(cnn2, "delete from ventas where Id = " & dr("Id").ToString & "", sinfo)
                            odata2.runSp(cnn2, "delete from ventasdetalle where IdVenta = " & dr("Id").ToString & "", sinfo)
                            odata2.runSp(cnn2, "delete from abonos where IdVenta = " & dr("Id").ToString & "", sinfo)

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO ventas(IdCliente, Cliente, Direccion, Subtotal, IVA, Totales, Descuento, Devolucion, ACuenta, Resta, Usuario, FVenta, HVenta, FPago, Status, MontoSinDesc, Cargado, FolioAndroid, CargadoAndroid, HVenta2, Fecha, IP, Formato, FEntrega) " &
                                                " VALUES (" & IIf(ope > 0, ope, dr("IdCliente").ToString) & ",'" & Trim(dr("NomCliente").ToString) & "', ''," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Total").ToString, ",", "") & "," & dr("Descuento").ToString & ",0," & Replace(dr("Acuenta").ToString, ",", "") & "," & Replace(dr("Resta").ToString, ",", "") & ",'" & dr("Usuario").ToString & "','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "','" & dr("Status").ToString & "','" & Replace(dr("Total").ToString, ",", "") & "',1," & dr("Id").ToString & ",1,'" & dr("FechaHora").ToString & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "','" & dameIP2() & "', 'TICKET','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "')"

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                Dim maxventa As Integer = dameMaxIdVta()

                                If dr("NumSuc").ToString > 0 Then
                                    bajaVentasdetalleAndroidSucu(dr("Id").ToString, maxventa, Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss"), dr("Usuario").ToString, dr("NumSuc").ToString)
                                Else
                                    bajaVentasdetalleAndroid(dr("Id").ToString, maxventa, Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss"), dr("Usuario").ToString)
                                End If

                                odata2.runSp(cnn2, "update ventas set Bajado = 2 where Id = " & dr("Id").ToString & "", sinfo)
                            End If


                        End If

                        grid_eventos.Rows.Insert(0, "Venta Nube sincronizado correctamente " & Trim(dr("Id").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaVentasdetalleAndroid(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varfecha As String, ByVal varUsuario As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select VD.*, V.Usuario from ventasdetalle VD, ventas V where V.Id = VD.IdVenta and VD.IdVenta = " & valIdVtaNube & " order by VD.IdLocal"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then

                    Dim dameidlocalnube As String = ""

                    For Each dr In dt.Rows

                        Dim MyExist As String = ""
                        Dim MyNewEsist As String = ""

                        Dim cnnp As MySqlConnection = New MySqlConnection
                        Dim odatap As New ToolKitSQL.myssql
                        Dim drp As DataRow
                        Dim varunidad As String = ""
                        Dim vardepto As String = ""
                        Dim vargpo As String = ""
                        Dim varprecioc As String = "0"
                        Dim varpreciosiniva As Double = 0
                        Dim vartotalsiniva As Double = 0
                        If odatap.dbOpen(cnnp, sTargetlocal, sinfo) Then
                            If odatap.getDr(cnnp, drp, "select * from productos where Codigo = '" & Trim(dr("Codigo").ToString) & "'", sinfo) Then
                                varunidad = Trim(drp("UVenta").ToString)
                                vardepto = Trim(drp("Departamento").ToString)
                                vargpo = Trim(drp("Grupo").ToString)
                                varprecioc = Trim(drp("PrecioCompra").ToString)
                                If IsNumeric(varprecioc) = False Then
                                    varprecioc = "0"
                                End If

                                If CDec(drp("IVA").ToString) > 0 Then
                                    varpreciosiniva = FormatNumber(dr("Precio").ToString / 1.16, 6)
                                    vartotalsiniva = FormatNumber(dr("Total").ToString / 1.16, 6)
                                Else
                                    varpreciosiniva = dr("Precio").ToString
                                    vartotalsiniva = dr("Total").ToString
                                End If

                            End If
                            cnnp.Close()
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO ventasdetalle(Folio, Codigo, Nombre, Cantidad, Unidad, CostoVP, CostoVUE, Precio, Total, PrecioSinIVA, TotalSinIVA, Fecha, Comisionista, Depto, Grupo, VDCosteo, TotalIEPS, TasaIeps, Usuario, FechaCompleta) " &
                                        " VALUES (" & valIdVta & ",'" & Trim(dr("Codigo").ToString) & "', '" & Trim(dr("Descripcion").ToString) & "','" & dr("Cantidad").ToString & "','" & varunidad & "','" & vartotalsiniva & "','" & varprecioc & "','" & dr("Precio").ToString & "','" & dr("Total").ToString & "','" & varpreciosiniva & "','" & vartotalsiniva & "','" & varfecha & "','" & varUsuario & "','" & vardepto & "','" & vargpo & "',0,0,0,'" & varUsuario & "', '" & varfecha & "')"

                        If dameidlocalnube <> dr("IdLocal").ToString Then

                            dameidlocalnube = dr("IdLocal").ToString

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                                If tipoInventario = 1 Then

                                    If odatap.dbOpen(cnnp, sTargetlocal, sinfo) Then
                                        If odatap.getDr(cnnp, drp, "select * from productos where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'", sinfo) Then

                                            MyExist = 0
                                            If CDec(drp("Multiplo").ToString) > 1 And CDec(drp("Existencia").ToString) > 0 Then
                                                MyExist = FormatNumber(CDec(drp("Existencia").ToString), 2)
                                                If Len(dr("Codigo").ToString) > 6 Then
                                                    MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                                Else
                                                    MyNewEsist = CDec(MyExist) - CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString))
                                                End If
                                            Else
                                                MyExist = drp("Existencia").ToString
                                                MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                            End If

                                            Dim sqlnew As String = ""
                                            Dim ssql3 As String = ""

                                            If Len(dr("Codigo").ToString) > 6 Then
                                                sqlnew = "update productos set Existencia = Existencia - " & CDec(dr("Cantidad").ToString) & "  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'"
                                            Else
                                                sqlnew = "update productos set Existencia = Existencia - " & CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString)) & "  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'"
                                            End If

                                            If odata.runSp(cnn, sqlnew, sinfo) Then

                                                If Len(dr("Codigo").ToString) > 6 Then
                                                    ssql3 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Venta Android'," & CDec(dr("Cantidad").ToString) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','" & dr("Usuario").ToString & "','" & MyExist & "','" & MyNewEsist & "','')"
                                                Else
                                                    ssql3 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Venta Android'," & CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','" & dr("Usuario").ToString & "','" & MyExist & "','" & MyNewEsist & "','')"
                                                End If

                                                odata.runSp(cnn, ssql3, sinfo)

                                                grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)

                                            End If

                                        End If
                                        cnnp.Close()
                                    End If

                                End If

                            End If

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "Delete from ventasdetalle where Id = " & dr("Id").ToString
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                            End If

                        End If


                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaVentasdetalleAndroidSucu(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varfecha As String, ByVal varUsuario As String, ByVal varsucu As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select VD.*, V.Usuario from ventasdetalle VD, ventas V where V.Id = VD.IdVenta and VD.IdVenta = " & valIdVtaNube & " order by VD.IdLocal"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then

                    Dim dameidlocalnube As String = ""

                    For Each dr In dt.Rows

                        Dim MyExist As String = ""
                        Dim MyNewEsist As String = ""

                        Dim cnnp As MySqlConnection = New MySqlConnection
                        Dim odatap As New ToolKitSQL.myssql
                        Dim drp As DataRow
                        Dim varunidad As String = ""
                        Dim vardepto As String = ""
                        Dim vargpo As String = ""
                        Dim varprecioc As String = "0"
                        Dim varpreciosiniva As Double = 0
                        Dim vartotalsiniva As Double = 0
                        If odatap.dbOpen(cnnp, sTargetlocal, sinfo) Then
                            If odatap.getDr(cnnp, drp, "select * from productos where Codigo = '" & Trim(dr("Codigo").ToString) & "'", sinfo) Then
                                varunidad = Trim(drp("UVenta").ToString)
                                vardepto = Trim(drp("Departamento").ToString)
                                vargpo = Trim(drp("Grupo").ToString)
                                varprecioc = Trim(drp("PrecioCompra").ToString)
                                If IsNumeric(varprecioc) = False Then
                                    varprecioc = "0"
                                End If

                                If CDec(drp("IVA").ToString) > 0 Then
                                    varpreciosiniva = FormatNumber(dr("Precio").ToString / 1.16, 6)
                                    vartotalsiniva = FormatNumber(dr("Total").ToString / 1.16, 6)
                                Else
                                    varpreciosiniva = dr("Precio").ToString
                                    vartotalsiniva = dr("Total").ToString
                                End If

                            End If
                            cnnp.Close()
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO ventasdetalle(Folio, Codigo, Nombre, Cantidad, Unidad, CostoVP, CostoVUE, Precio, Total, PrecioSinIVA, TotalSinIVA, Fecha, Comisionista, Depto, Grupo, VDCosteo, TotalIEPS, TasaIeps, Usuario, FechaCompleta) " &
                                        " VALUES (" & valIdVta & ",'" & Trim(dr("Codigo").ToString) & "', '" & Trim(dr("Descripcion").ToString) & "','" & dr("Cantidad").ToString & "','" & varunidad & "','" & vartotalsiniva & "','" & varprecioc & "','" & dr("Precio").ToString & "','" & dr("Total").ToString & "','" & varpreciosiniva & "','" & vartotalsiniva & "','" & varfecha & "','" & varUsuario & "','" & vardepto & "','" & vargpo & "',0,0,0,'" & varUsuario & "', '" & varfecha & "')"

                        If dameidlocalnube <> dr("IdLocal").ToString Then

                            dameidlocalnube = dr("IdLocal").ToString

                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then

                                If odatap.dbOpen(cnnp, sTargetlocal, sinfo) Then
                                    If odatap.getDr(cnnp, drp, "select * from productos where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "'", sinfo) Then


                                        Dim existenciaprod2 As Double = 0
                                        Dim drprod2 As DataRow
                                        If odata2.getDr(cnn2, drprod2, "select Existencia from productos2 where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "' and NumSuc = " & varsucu & "", sinfo) Then
                                            existenciaprod2 = drprod2(0).ToString
                                        End If


                                        MyExist = 0
                                        If CDec(drp("Multiplo").ToString) > 1 And CDec(existenciaprod2) > 0 Then
                                            MyExist = FormatNumber(CDec(existenciaprod2), 2)
                                            If Len(dr("Codigo").ToString) > 6 Then
                                                MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                            Else
                                                MyNewEsist = CDec(MyExist) - CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString))
                                            End If
                                        Else
                                            MyExist = existenciaprod2
                                            MyNewEsist = CDec(MyExist) - CDec(dr("Cantidad").ToString)
                                        End If

                                        Dim sqlnew As String = ""
                                        Dim ssql3 As String = ""

                                        If Len(dr("Codigo").ToString) > 6 Then
                                            sqlnew = "update productos2 set Existencia = Existencia - " & CDec(dr("Cantidad").ToString) & "  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "' and NumSuc = " & varsucu & ""
                                        Else
                                            sqlnew = "update productos2 set Existencia = Existencia - " & CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString)) & "  where Codigo = '" & Mid(dr("Codigo").ToString, 1, 6) & "' and NumSuc = " & varsucu & ""
                                        End If

                                        If odata2.runSp(cnn2, sqlnew, sinfo) Then

                                            If Len(dr("Codigo").ToString) > 6 Then
                                                ssql3 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio, NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Venta Android'," & CDec(dr("Cantidad").ToString) & ",'0','" & Format(Date.Now, "yyyy/MM/dd HH:mm:ss") & "','" & dr("Usuario").ToString & "','" & MyExist & "','" & MyNewEsist & "','', " & varsucu & ")"
                                            Else
                                                ssql3 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio, NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Descripcion").ToString & "','Venta Android'," & CDec(CDec(dr("Cantidad").ToString) * CDec(drp("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy/MM/dd HH:mm:ss") & "','" & dr("Usuario").ToString & "','" & MyExist & "','" & MyNewEsist & "','', " & varsucu & ")"
                                            End If

                                            odata2.runSp(cnn2, ssql3, sinfo)

                                            grid_eventos.Rows.Insert(0, "Finaliza Ajuste de Inventario " & dr("Descripcion").ToString, Date.Now)

                                        End If

                                    End If
                                    cnnp.Close()
                                End If

                            End If

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "Delete from ventasdetalle where Id = " & dr("Id").ToString
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                            End If

                        End If


                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameIdCli(ByVal valnombre As String) As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Id from clientes where Nombre = '" & Trim(valnombre) & "' or RazonSocial = '" & Trim(valnombre) & "'"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Function dameMaxIdVta() As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Folio) as maxi from ventas"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub bajaActuVentasAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from ventas where Bajado=1 and Cargado = 1"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim ope As Integer = 0

                        If Trim(dr("NomCliente").ToString) <> "" Then
                            ope = dameIdCli(dr("NomCliente").ToString)
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "UPDATE ventas set ACuenta = " & Replace(dr("Acuenta").ToString, ",", "") & ", Resta = " & Replace(dr("Resta").ToString, ",", "") & ", Status = '" & dr("Status").ToString & "', Cargado = 0 where FolioAndroid = " & dr("Id").ToString & ""

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                            odata2.runSp(cnn2, "update ventas set Bajado = 2, Cargado = 0 where Id = " & dr("Id").ToString & "", sinfo)
                        End If

                        grid_eventos.Rows.Insert(0, "Actu. Venta Nube sincronizado correctamente " & Trim(dr("Id").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaAbonosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from abonos where Bajado=0 order by FechaHora ASC, Id ASC"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim ope As Integer = 0

                        If Trim(dr("NomCliente").ToString) <> "" Then
                            ope = dameIdCli(dr("NomCliente").ToString)
                        End If

                        Dim folioloc As Integer = dameIdVta(dr("IdVenta").ToString, dr("FechaHora").ToString, dr("Id").ToString)

                        ssqlinsertal = ""

                        Dim nuevosaldo As Double = 0
                        If ope > 0 Then

                            Dim drsaldo As DataRow
                            If odata.getDr(cnn, drsaldo, "Select Saldo from abonoi where Id = (Select max(Id) as maxi from abonoi where IdCliente = " & ope & ")", sinfo) Then
                                If IsNumeric(drsaldo("Saldo").ToString) Then
                                    nuevosaldo = CDbl(drsaldo("Saldo").ToString)
                                End If
                            End If

                        End If

                        If Trim(dr("FormaPago").ToString) <> "" Then


                            If ope > 0 Then
                                nuevosaldo = nuevosaldo - CDbl(dr("Abono").ToString)
                            Else
                                nuevosaldo = 0
                            End If



                            Select Case Trim(dr("FormaPago").ToString)
                                Case "Efectivo"
                                    ssqlinsertal = "INSERT INTO abonoi(NumFolio,IdCliente,Cliente,Concepto,Fecha,Hora,Cargo,Abono,Saldo,FormaPago,Monto,Banco,Referencia,Usuario,MontoSF,Comentario,Cargado,CargadoAndroid,FolioAndroid,FechaCompleta) " &
                                        " VALUES ('" & folioloc & "'," & ope & ",'" & Trim(dr("NomCliente").ToString) & "', '" & dr("Tipo").ToString & "', '" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "',0," & dr("Abono").ToString & "," & nuevosaldo & ",'EFECTIVO'," & dr("Abono").ToString & ",'','','" & dr("Usuario").ToString & "',0,'',1,1,'" & dr("CodigoUnicoVenta").ToString & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "')"
                                Case "Tarjeta"
                                    ssqlinsertal = "INSERT INTO abonoi(NumFolio,IdCliente,Cliente,Concepto,Fecha,Hora,Cargo,Abono,Saldo,FormaPago,Monto,Banco,Referencia,Usuario,MontoSF,Comentario,Cargado,CargadoAndroid,FolioAndroid,FechaCompleta) " &
                                        " VALUES ('" & folioloc & "'," & ope & ",'" & Trim(dr("NomCliente").ToString) & "', '" & dr("Tipo").ToString & "', '" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "',0," & dr("Abono").ToString & "," & nuevosaldo & ",'TARJETA DEBITO'," & dr("Abono").ToString & ",'','','" & dr("Usuario").ToString & "',0,'',1,1,'" & dr("CodigoUnicoVenta").ToString & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "')"
                                Case "Trasferencia"
                                    ssqlinsertal = "INSERT INTO abonoi(NumFolio,IdCliente,Cliente,Concepto,Fecha,Hora,Cargo,Abono,Saldo,FormaPago,Monto,Banco,Referencia,Usuario,MontoSF,Comentario,Cargado,CargadoAndroid,FolioAndroid,FechaCompleta) " &
                                        " VALUES ('" & folioloc & "'," & ope & ",'" & Trim(dr("NomCliente").ToString) & "', '" & dr("Tipo").ToString & "', '" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "',0," & dr("Abono").ToString & "," & nuevosaldo & ",'TRANSFERENCIA'," & dr("Abono").ToString & ",'','','" & dr("Usuario").ToString & "',0,'',1,1,'" & dr("CodigoUnicoVenta").ToString & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "')"
                            End Select

                        Else

                            nuevosaldo = nuevosaldo + CDbl(dr("Cargo").ToString)

                            ssqlinsertal = "INSERT INTO abonoi(NumFolio,IdCliente,Cliente,Concepto,Fecha,Hora,Cargo,Abono,Saldo,FormaPago,Monto,Banco,Referencia,Usuario,MontoSF,Comentario,Cargado,CargadoAndroid,FolioAndroid,FechaCompleta) " &
                                        " VALUES ('" & folioloc & "'," & ope & ",'" & Trim(dr("NomCliente").ToString) & "', 'NOTA VENTA', '" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "'," & dr("Cargo").ToString & ",0," & nuevosaldo & ",'',0,'','','" & dr("Usuario").ToString & "', 0, '', 1, 1,'" & dr("CodigoUnicoVenta").ToString & "','" & Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss") & "')"
                        End If

                        Dim drvalida As DataRow

                        If odata.getDr(cnn, drvalida, "select Id from abonoi where FolioAndroid = '" & dr("CodigoUnicoVenta").ToString & "' and Concepto = '" & dr("Tipo").ToString & "'", sinfo) Then
                            odata2.runSp(cnn2, "delete from abonos where Id = " & dr("Id").ToString & "", sinfo)
                            If dr("IdVenta").ToString > 0 And folioloc > 0 Then
                                actualizar_ventas_nube(dr("IdVenta").ToString, folioloc)
                            End If
                        Else
                            If folioloc = 0 Then
                            Else
                                If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                                    odata2.runSp(cnn2, "update abonos set Bajado = 2 where Id = " & dr("Id").ToString & "", sinfo)
                                    actuSaldoCliente(ope, Trim(dr("NomCliente").ToString))
                                End If
                            End If
                        End If

                        grid_eventos.Rows.Insert(0, "Abono Nube sincronizado correctamente " & Trim(dr("Id").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameIdVta(ByVal varIdNube As Integer, ByVal varfechahora As String, ByVal varidabono As Integer) As Integer

        Dim varidvetareal As Integer = 0

        If varIdNube = 0 Then
            Dim cnnPro1 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
            Dim sSQL1 As String = ""
            sSQL1 = "Select Id from ventas where FechaHora = '" & varfechahora & "'"
            Dim drPro1 As DataRow
            Dim sinfoPro1 As String = ""
            Dim odataPro1 As New ToolKitSQL.myssql
            If odataPro1.dbOpen(cnnPro1, sTargetdAndroid, sinfoPro1) Then
                If odataPro1.getDr(cnnPro1, drPro1, sSQL1, sinfoPro1) Then
                    varidvetareal = CInt(IIf(IsNumeric(drPro1(0).ToString), drPro1(0).ToString, 0))
                    If varidvetareal <> 0 Then
                        odataPro1.runSp(cnnPro1, "update abonos set IdVenta = " & varidvetareal & " where Id = " & varidabono & "", sinfoPro1)
                    End If
                End If
                cnnPro1.Close()
            End If
        Else
            varidvetareal = varIdNube
        End If

        If varidvetareal = 0 Then
            Return 0
        End If

        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Folio from ventas where FolioAndroid = " & varidvetareal
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Sub actualizar_ventas_nube(ByVal varidventanube As Integer, ByVal varidventalocal As Integer)

        Dim dametotalventa As Double = 0
        Dim dametotalabonos As Double = 0
        Dim dametotalresta As Double = 0
        Dim dametipo As String = ""

        Dim cnnPro As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        Dim sSQL1 As String = ""
        sSQL = "select Total from ventas WHERE Id = " & varidventanube & ""
        sSQL1 = "select Abono from abonoi WHERE IdVenta = " & varidventanube & " and Tipo = 'ABONO'"
        Dim drPro As DataRow
        Dim dtPro As New DataTable
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetdAndroid, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                dametotalventa = drPro(0).ToString
            End If

            If odataPro.getDt(cnnPro, dtPro, sSQL1, sinfoPro) Then
                For Each drPro In dtPro.Rows
                    dametotalabonos += drPro(0).ToString
                Next
            End If

            If dametotalabonos = dametotalventa Then
                dametotalresta = 0
                dametipo = "PAGADO"
            End If

            If dametotalventa > dametotalabonos Then
                dametotalresta = dametotalventa - dametotalabonos
                dametipo = "RESTA"
            End If

            If dametotalventa < dametotalabonos Then
                dametotalabonos = dametotalventa
                dametotalresta = 0
                dametipo = "PAGADO"
            End If

            odataPro.runSp(cnnPro, "update ventas set Acuenta = " & dametotalabonos & ", Resta = " & dametotalresta & ", Status = '" & dametipo & "' where Id = " & varidventanube & "", sinfoPro)

            cnnPro.Close()
        End If

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            odata.runSp(cnn, "update ventas set ACuenta = '" & dametotalabonos & "', Resta = '" & dametotalresta & "', Status = '" & dametipo & "' where Folio = " & varidventalocal & "", sinfo)
            cnn.Close()
        End If

    End Sub

    Sub actuSaldoCliente(ByVal varidcliente As Integer, ByVal nomcliente As String)
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "update clientes set Saldo = " & dameSaldoCliente(varidcliente) & " WHERE Id = " & dameIdCliNube(Trim(nomcliente)) & ""
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetdAndroid, sinfoPro) Then
            If odataPro.runSp(cnnPro, sSQL, sinfoPro) Then
                My.Application.DoEvents()
                grid_eventos.Rows.Insert(0, "Saldo Cliente sincronizado " & Trim(nomcliente), Date.Now)
            End If
            cnnPro.Close()
        End If
    End Sub

    Function dameIdCliNube(ByVal valnombre As String) As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Id from clientes where Nombre = '" & Trim(valnombre) & "'"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetdAndroid, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub subirVentasAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from ventas where CargadoAndroid=0 order by Folio"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim ope As Integer = 0

                        If Trim(dr("Cliente").ToString) <> "" Then
                            ope = dameIdCliNube(dr("Cliente").ToString)
                        End If

                        If IsNumeric(dr("FolioAndroid").ToString) = False Then

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO ventas(IdCliente,NomCliente,Subtotal,IVA,Total,Acuenta,Resta,Fecha,FechaHora,Usuario,Status,Bajado)" &
                                            " VALUES (" & IIf(ope > 0, ope, dr("IdCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "'," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Totales").ToString, ",", "") & "," & Replace(dr("ACuenta").ToString, ",", "") & "," & Replace(dr("Resta").ToString, ",", "") & ",'" & FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & "','" & Trim(FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & dr("HVenta").ToString) & "','" & dr("Usuario").ToString & "','" & dr("Status").ToString & "',2)"

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                Dim maxventa As Integer = dameMaxIdVtaNube()
                                subirVentasdetalleAndroid(dr("Folio").ToString, maxventa, Trim(FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & " " & dr("HVenta").ToString))
                                subirAbonosAndroid(dr("Folio").ToString, maxventa, dr("HVenta").ToString, dr("Usuario").ToString)
                                odata.runSp(cnn, "update Ventas set CargadoAndroid = 1, FolioAndroid = " & maxventa & " where Folio = " & dr("Folio").ToString & "", sinfo)
                            End If

                        Else

                            If CDec(dr("FolioAndroid").ToString) = 0 Then
                                ssqlinsertal = ""
                                ssqlinsertal = "INSERT INTO ventas(IdCliente,NomCliente,Subtotal,IVA,Total,Acuenta,Resta,Fecha,FechaHora,Usuario,Status,Bajado)" &
                                                " VALUES (" & IIf(ope > 0, ope, dr("IdCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "'," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Totales").ToString, ",", "") & "," & Replace(dr("ACuenta").ToString, ",", "") & "," & Replace(dr("Resta").ToString, ",", "") & ",'" & FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & "','" & Trim(FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & dr("HVenta").ToString) & "','" & dr("Usuario").ToString & "','" & dr("Status").ToString & "',2)"

                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    Dim maxventa As Integer = dameMaxIdVtaNube()
                                    subirVentasdetalleAndroid(dr("Folio").ToString, maxventa, Trim(FormatDateTime(dr("FVenta").ToString, DateFormat.ShortDate) & " " & dr("HVenta").ToString))
                                    subirAbonosAndroid(dr("Folio").ToString, maxventa, dr("HVenta").ToString, dr("Usuario").ToString)
                                    odata.runSp(cnn, "update Ventas set CargadoAndroid = 1, FolioAndroid = " & maxventa & " where Folio = " & dr("Folio").ToString & "", sinfo)
                                End If

                            Else
                                ssqlinsertal = ""
                                ssqlinsertal = "UPDATE ventas SET Acuenta = " & Replace(dr("ACuenta").ToString, ",", "") & ", Resta = " & Replace(dr("Resta").ToString, ",", "") & ", Status = '" & dr("Status").ToString & "', Bajado = 2 WHERE Id = " & dr("FolioAndroid").ToString & ""

                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    subirAbonosAndroid(dr("Folio").ToString, dr("FolioAndroid").ToString, dr("HVenta").ToString, dr("Usuario").ToString)
                                    odata.runSp(cnn, "update Ventas set CargadoAndroid = 1 where Folio = " & dr("Folio").ToString & "", sinfo)
                                End If

                            End If

                        End If




                        grid_eventos.Rows.Insert(0, "Venta sincronizada correctamente " & Trim(dr("Folio").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameMaxIdVtaNube() As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Id) as maxi from ventas"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetdAndroid, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub subirVentasdetalleAndroid(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varfecha As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from VentasDetalle where Folio = " & valIdVtaNube

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim newiva As Double = 0

                        If dr("Total").ToString > dr("TotalSinIVA").ToString Then
                            newiva = 0.16
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO ventasdetalle(IdVenta,Codigo,Descripcion,Cantidad,Precio,Total,IVA,FechaHora,Descuento)" &
                                        " VALUES (" & valIdVta & ",'" & Trim(dr("Codigo").ToString) & "','" & Trim(dr("Nombre").ToString) & "'," & Replace(dr("Cantidad").ToString, ",", "") & "," & Replace(dr("Precio").ToString, ",", "") & "," & Replace(dr("Total").ToString, ",", "") & "," & newiva & ",'" & varfecha & "',0)"

                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subirAbonosAndroid(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varFecha As String, ByVal varUsuario As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from abonoi where NumFolio = " & valIdVtaNube & " and CargadoAndroid=0 order by Id"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        ssqlinsertal = ""

                        Dim ope As Integer = 0

                        If Trim(dr("Cliente").ToString) <> "" Then
                            ope = dameIdCliNube(dr("Cliente").ToString)
                        End If

                        If Trim(dr("Concepto").ToString) = "ABONO" Then
                            ssqlinsertal = "insert into abonos(IdCliente,NomCliente,IdVenta,Fecha,Cargo,Abono,Saldo,Tipo,FormaPago,Usuario,FechaHora,Banco,Referencia,Bajado) " &
                                        " VALUES (" & IIf(ope > 0, ope, dr("idCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "', " & valIdVta & ", '" & FormatDateTime(dr("Fecha").ToString, DateFormat.ShortDate) & "'," & Replace(dr("Cargo").ToString, ",", "") & "," & Replace(dr("Abono").ToString, ",", "") & "," & IIf(IsNumeric(dr("Saldo").ToString) = False, 0, Replace(dr("Saldo").ToString, ",", "")) & ",'" & dr("Concepto").ToString & "','Efectivo','" & varUsuario & "','" & Trim(FormatDateTime(dr("Fecha").ToString, DateFormat.ShortDate) & " " & varFecha) & "','','',2)"
                        ElseIf Trim(dr("Concepto").ToString) = "NOTA VENTA" Then
                            ssqlinsertal = "insert into abonos(IdCliente,NomCliente,IdVenta,Fecha,Cargo,Abono,Saldo,Tipo,FormaPago,Usuario,FechaHora,Banco,Referencia,Bajado) " &
                                        " VALUES (" & IIf(ope > 0, ope, dr("idCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "', " & valIdVta & ", '" & FormatDateTime(dr("Fecha").ToString, DateFormat.ShortDate) & "'," & Replace(dr("Cargo").ToString, ",", "") & "," & Replace(dr("Abono").ToString, ",", "") & "," & IIf(IsNumeric(dr("Saldo").ToString) = False, 0, Replace(dr("Saldo").ToString, ",", "")) & ",'NOTA DE VENTA','','" & varUsuario & "','" & Trim(FormatDateTime(dr("Fecha").ToString, DateFormat.ShortDate) & " " & varFecha) & "','','',2)"
                        End If
                        If ssqlinsertal = "" Then
                            odata.runSp(cnn, "update abonoi set CargadoAndroid = 1 where Id = " & dr("Id").ToString & "", sinfo)
                            actuSaldoCliente(dr("idCliente").ToString, Trim(dr("Cliente").ToString))
                        Else
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "update abonoi set CargadoAndroid = 1 where Id = " & dr("Id").ToString & "", sinfo)
                                actuSaldoCliente(dr("idCliente").ToString, Trim(dr("Cliente").ToString))
                            End If
                        End If


                        grid_eventos.Rows.Insert(0, "Abono sincronizado correctamente " & Trim(dr("Id").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaTraspasosEntradaAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = "Select * from traspasos where Cargado = 1 and Bajado = 0"
        Dim ssqlinsertal As String = ""
        Dim ssql3 As String = ""
        Dim dt As New DataTable
        Dim dt2 As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql

        Dim maxIdTraspaso As Integer = 0

        Dim MyExist As String = ""
        Dim MyNewEsist As String = ""

        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then

                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        ssqlinsertal = ""
                        grid_eventos.Rows.Insert(0, "Bajando Traspaso Entrada folio " & dr("Id").ToString, Date.Now)
                        My.Application.DoEvents()


                        If odata2.getDt(cnn2, dt2, "select * from traspasosdetalle where IdTraspaso = " & dr("Id").ToString & "", sinfo) Then

                            For Each dr2 In dt2.Rows

                                If odata.getDr(cnn, dr3, "select Existencia from productos where Codigo = '" & Mid(dr2("Codigo").ToString, 1, 6) & "'", sinfo) Then

                                    Dim dr4 As DataRow
                                    If odata.getDr(cnn, dr4, "select Codigo,Multiplo from productos where Codigo = '" & dr2("Codigo").ToString & "'", sinfo) Then

                                        MyExist = 0

                                        If CDec(dr4("Multiplo").ToString) > 1 And CDec(dr3("Existencia").ToString) > 0 Then
                                            MyExist = FormatNumber(CDec(dr3("Existencia").ToString), 2)
                                            MyNewEsist = CDec(MyExist) + CDec(CDec(dr2("Cantidad").ToString) * CDec(dr4("Multiplo").ToString))
                                        Else
                                            MyExist = dr3("Existencia").ToString
                                            MyNewEsist = CDec(MyExist) + CDec(dr2("Cantidad").ToString)
                                        End If

                                        If odata.runSp(cnn, "update productos set Existencia = " & CDec(MyNewEsist) & " where Codigo = '" & Mid(dr2("Codigo").ToString, 1, 6) & "'", sinfo) Then

                                            ssql3 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio) values('" & dr2("Codigo").ToString & "','" & dr2("Descripcion").ToString & "','Regreso de Inventario Android'," & CDec(dr2("Cantidad").ToString) & ",'0','" & Now & "','" & dr2("Usuario").ToString & "','" & MyExist & "','" & MyNewEsist & "','')"

                                            odata.runSp(cnn, ssql3, sinfo)

                                        End If

                                    End If

                                End If

                            Next

                            ssql3 = "update traspasos set Cargado=0, Bajado = 2 where Id=" & dr("Id").ToString
                            If odata2.runSp(cnn2, ssql3, sinfo) Then
                                grid_eventos.Rows.Insert(0, "Finaliza Traspaso Entrada folio " & dr("Id").ToString, Date.Now)
                            End If

                        End If

                    Next
                End If

                cnn2.Close()
            End If
            cnn.Close()
        End If


    End Sub

    Private Sub bajaPedidosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from pedidos where Bajado=0 order by Id"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim ope As Integer = 0

                        If Trim(dr("NomCliente").ToString) <> "" Then
                            ope = dameIdCli(dr("NomCliente").ToString)
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO pedidosven(IdCliente, Cliente, Direccion, Subtotal, IVA, Totales, Descuento, ACuenta, Resta, Usuario, Fecha, Hora, FPago, Status, Descto, MontoSinDesc, FolioAndroid, CargadoAndroid, Tipo, IP, Comentario, Formato) " &
                                        " VALUES (" & IIf(ope > 0, ope, dr("IdCliente").ToString) & ",'" & Trim(dr("NomCliente").ToString) & "', ''," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Total").ToString, ",", "") & ",0,0," & Replace(dr("Total").ToString, ",", "") & ",'" & dr("Usuario").ToString & "','" & Format(CDate(dr("Fecha").ToString), "yyyy-MM-dd") & "','" & Format(CDate(dr("FechaHora").ToString), "HH:mm:ss") & "','" & dr("Fecha").ToString & "','PENDIENTE','0','" & Replace(dr("Total").ToString, ",", "") & "'," & dr("Id").ToString & ",1,'PEDIDO','" & dameIP2() & "','','TICKET')"

                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                            Dim maxventa As Integer = dameMaxIdPedidosAndroid()
                            bajaPedidosdetalleAndroid(dr("Id").ToString, maxventa, Format(CDate(dr("FechaHora").ToString), "yyyy-MM-dd HH:mm:ss"), dr("Usuario").ToString)
                            odata2.runSp(cnn2, "update pedidos set Bajado = 2 where Id = " & dr("Id").ToString & "", sinfo)
                        End If

                        grid_eventos.Rows.Insert(0, "Pedido Nube sincronizado correctamente " & Trim(dr("Id").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub bajaPedidosdetalleAndroid(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varfecha As String, ByVal varUsuario As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from pedidosdetalle where IdPedido = " & valIdVtaNube & " order by IdLocal"
        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then

                    Dim dameidlocalnube As String = ""

                    For Each dr In dt.Rows

                        Dim cnnp As MySqlConnection = New MySqlConnection
                        Dim odatap As New ToolKitSQL.myssql
                        Dim drp As DataRow
                        Dim varunidad As String = ""
                        Dim vardepto As String = ""
                        Dim vargpo As String = ""
                        Dim varprecioc As String = "0"
                        Dim varpreciosiniva As Double = 0
                        Dim vartotalsiniva As Double = 0
                        If odatap.dbOpen(cnnp, sTargetlocal, sinfo) Then
                            If odatap.getDr(cnnp, drp, "select * from productos where Codigo = '" & Trim(dr("Codigo").ToString) & "'", sinfo) Then
                                varunidad = Trim(drp("UVenta").ToString)
                                vardepto = Trim(drp("Departamento").ToString)
                                vargpo = Trim(drp("Grupo").ToString)
                                varprecioc = Trim(drp("PrecioCompra").ToString)
                                If IsNumeric(varprecioc) = False Then
                                    varprecioc = "0"
                                End If

                                If CDec(drp("IVA").ToString) > 0 Then
                                    varpreciosiniva = FormatNumber(dr("Precio").ToString / 1.16, 6)
                                    vartotalsiniva = FormatNumber(dr("Total").ToString / 1.16, 6)
                                Else
                                    varpreciosiniva = dr("Precio").ToString
                                    vartotalsiniva = dr("Total").ToString
                                End If

                            End If
                            cnnp.Close()
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO pedidosvendet(Folio, Codigo, Nombre, Cantidad, Unidad, CostoV, Precio, Total, PrecioSIVA, TotalSIVA, Fecha, Usuario, Depto, Grupo, Tipo) " &
                                        " VALUES (" & valIdVta & ",'" & Trim(dr("Codigo").ToString) & "', '" & Trim(dr("Descripcion").ToString) & "','" & dr("Cantidad").ToString & "','" & varunidad & "','" & varpreciosiniva & "','" & dr("Precio").ToString & "','" & dr("Total").ToString & "','" & varpreciosiniva & "','" & vartotalsiniva & "','" & varfecha & "','" & varUsuario & "','" & vardepto & "','" & vargpo & "','PEDIDO')"

                        If dameidlocalnube <> dr("IdLocal").ToString Then
                            dameidlocalnube = dr("IdLocal").ToString
                            If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                            End If
                        Else
                            ssqlinsertal = ""
                            ssqlinsertal = "Delete from pedidosdetalle where Id = " & dr("Id").ToString
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                            End If
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameMaxIdPedidosAndroid() As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Folio) as maxi from pedidosven"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub eliminarPedidosNewAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from pedidoeliminado"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If dr("IdPedidoA").ToString = 0 Then
                            odata.runSp(cnn, "Delete from pedidoeliminado where Id = " & dr("Id").ToString & "", sinfo)
                        Else
                            ssqlinsertal = ""
                            'ssqlinsertal = "Insert into delpedido(IdPedido,NumSuc) values(" & dr("IdPedidoA").ToString & "," & susursalr & ")"
                            ssqlinsertal = "Insert into delpedido(IdPedido,NumSuc) values(" & dr("IdPedidoA").ToString & ",1)"
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                odata.runSp(cnn, "Delete from pedidoeliminado where Id = " & dr("Id").ToString & "", sinfo)
                            End If
                        End If

                        grid_eventos.Rows.Insert(0, "Pedido eliminado correctamente " & Trim(dr("IdPedidoA").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub eliminarPedidosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from delpedido where NumSuc=1"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim varfoliocot As Integer = dameIdPedidosAndroid(dr("IdPedido").ToString)

                        ssqlinsertal = ""
                        ssqlinsertal = "Delete from pedidosven where FolioAndroid = " & dr("IdPedido").ToString & ""
                        If odata.runSp(cnn, ssqlinsertal, sinfo) Then
                            odata.runSp(cnn, "Delete from pedidosvendet where Folio = " & varfoliocot & "", sinfo)

                            If odata2.runSp(cnn2, "Delete from pedidos where Id = " & dr("IdPedido").ToString & "", sinfo) Then
                                odata2.runSp(cnn2, "Delete from pedidosdetalle where IdPedido = " & dr("IdPedido").ToString & "", sinfo)
                            End If

                            odata2.runSp(cnn2, "Delete from delpedido where Id = " & dr("Id").ToString & "", sinfo)

                        End If

                        grid_eventos.Rows.Insert(0, "Pedido eliminado correctamente " & Trim(dr("IdPedido").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameIdPedidosAndroid(ByVal varFolioAndroid As Integer) As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Folio from pedidosven where FolioAndroid = " & varFolioAndroid
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetlocal, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub subirPedidosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select `Folio`, `IdCliente`, `Cliente`, `Direccion`, `Subtotal`, `IVA`, `Totales`, `ACuenta`, `Resta`, `Usuario`, `Fecha`, `Hora`, `Status`, `Tipo`, `Comentario`, `Formato`, `CargadoAndroid`, `FolioAndroid` from pedidosven where CargadoAndroid = 0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If IsNumeric(dr("FolioAndroid").ToString) Then

                            If CDec(dr("FolioAndroid").ToString) > 0 Then
                                odata.runSp(cnn, "update pedidosven set CargadoAndroid = 1 where Folio = " & dr("Folio").ToString & "", sinfo)
                            Else

                                Dim ope As Integer = 0

                                If Trim(dr("Cliente").ToString) <> "" Then
                                    ope = dameIdCliNube(dr("Cliente").ToString)
                                End If

                                ssqlinsertal = ""
                                ssqlinsertal = "INSERT INTO pedidos(IdCliente, NomCliente, Subtotal, IVA, Total, Descuento, ACuenta, Resta, Usuario, Fecha, FechaHora, Status, Bajado) " &
                                                " VALUES (" & IIf(ope > 0, ope, dr("idCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "'," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Totales").ToString, ",", "") & ",0,0," & Replace(dr("Resta").ToString, ",", "") & ",'" & dr("Usuario").ToString & "','" & Format(CDate(dr("Fecha").ToString), "dd/MM/yyyy") & "','" & dr("Hora").ToString & "','RESTA',2)"

                                If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                    Dim maxventa As Integer = dameMaxIdPedidosNube()
                                    subirPedidosdetalleAndroid(dr("Folio").ToString, maxventa, dr("Hora").ToString)
                                    odata.runSp(cnn, "update pedidosven set CargadoAndroid = 1, FolioAndroid = " & maxventa & " where Folio = " & dr("Folio").ToString & "", sinfo)
                                End If

                            End If

                        Else

                            Dim ope As Integer = 0

                            If Trim(dr("Cliente").ToString) <> "" Then
                                ope = dameIdCliNube(dr("Cliente").ToString)
                            End If

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO pedidos(IdCliente, NomCliente, Subtotal, IVA, Total, Descuento, ACuenta, Resta, Usuario, Fecha, FechaHora, Status, Bajado) " &
                                            " VALUES (" & IIf(ope > 0, ope, dr("idCliente").ToString) & ",'" & Trim(dr("Cliente").ToString) & "'," & Replace(dr("Subtotal").ToString, ",", "") & "," & Replace(dr("IVA").ToString, ",", "") & "," & Replace(dr("Totales").ToString, ",", "") & ",0,0," & Replace(dr("Resta").ToString, ",", "") & ",'" & dr("Usuario").ToString & "','" & Format(CDate(dr("Fecha").ToString), "dd/MM/yyyy") & "','" & dr("Hora").ToString & "','RESTA',2)"

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                                Dim maxventa As Integer = dameMaxIdPedidosNube()
                                subirPedidosdetalleAndroid(dr("Folio").ToString, maxventa, dr("Hora").ToString)
                                odata.runSp(cnn, "update pedidosven set CargadoAndroid = 1, FolioAndroid = " & maxventa & " where Folio = " & dr("Folio").ToString & "", sinfo)
                            End If

                        End If

                        grid_eventos.Rows.Insert(0, "Pedido Nube sincronizado correctamente " & Trim(dr("Folio").ToString), Date.Now)

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub subirPedidosdetalleAndroid(ByVal valIdVtaNube As Integer, ByVal valIdVta As Integer, ByVal varfecha As String)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from pedidosvendet where Folio = " & valIdVtaNube

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim newiva As Double = 0

                        If dr("Total").ToString > dr("TotalSIVA").ToString Then
                            newiva = 0.16
                        End If

                        ssqlinsertal = ""
                        ssqlinsertal = "INSERT INTO pedidosdetalle(IdPedido,Codigo,Descripcion,Cantidad,Precio,Total,IVA,FechaHora,Descuento)" &
                                        " VALUES (" & valIdVta & ",'" & Trim(dr("Codigo").ToString) & "','" & Trim(dr("Nombre").ToString) & "'," & Replace(dr("Cantidad").ToString, ",", "") & "," & Replace(dr("Precio").ToString, ",", "") & "," & Replace(dr("Total").ToString, ",", "") & "," & newiva & ",'" & varfecha & "',0)"

                        If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Function dameMaxIdPedidosNube() As Integer
        Dim cnnPro As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Max(Id) as maxi from pedidos"
        Dim drPro As DataRow
        Dim sinfoPro As String = ""
        Dim odataPro As New ToolKitSQL.myssql
        If odataPro.dbOpen(cnnPro, sTargetdAndroid, sinfoPro) Then
            If odataPro.getDr(cnnPro, drPro, sSQL, sinfoPro) Then
                cnnPro.Close()
                Return CInt(IIf(IsNumeric(drPro(0).ToString), drPro(0).ToString, 0))
            Else
                cnnPro.Close()
                Return 0
            End If
        End If
    End Function

    Private Sub eliminarPedidosNubeAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from pedidos where Cargado = 0 and Bajado=2"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata2.getDt(cnn2, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If odata.getDr(cnn, dr2, "select Folio from pedidosven where FolioAndroid = " & dr("Id").ToString & "", sinfo) Then
                            If odata.getDr(cnn, dr2, "select Folio from pedidosven where FolioAndroid = " & dr("Id").ToString & " and Status = 'CANCELADA'", sinfo) Then
                                If odata2.runSp(cnn2, "Delete from pedidos where Id = " & dr("Id").ToString & "", sinfo) Then
                                    odata2.runSp(cnn2, "Delete from pedidosdetalle where IdPedido = " & dr("Id").ToString & "", sinfo)
                                    grid_eventos.Rows.Insert(0, "Pedido Nube eliminado correctamente " & Trim(dr("Id").ToString), Date.Now)
                                End If
                            End If
                        Else
                            If odata2.runSp(cnn2, "Delete from pedidos where Id = " & dr("Id").ToString & "", sinfo) Then
                                odata2.runSp(cnn2, "Delete from pedidosdetalle where IdPedido = " & dr("Id").ToString & "", sinfo)
                                grid_eventos.Rows.Insert(0, "Pedido Nube eliminado correctamente " & Trim(dr("Id").ToString), Date.Now)
                            End If
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub eliminarClienteNubeAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from clienteeliminado where CargadoAndroid = 0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If odata2.runSp(cnn2, "Delete From clientes where Nombre = '" & dr("Nombre").ToString & "'", sinfo) Then
                            odata.runSp(cnn, "update clienteeliminado set CargadoAndroid = 1 where Id = " & dr("Id").ToString & " ", sinfo)
                            grid_eventos.Rows.Insert(0, "Cliente Nube eliminado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Private Sub eliminarProductosNubeAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from productoeliminado where CargadoAndroid = 0"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        If Len(dr("Codigo").ToString) > 6 Then
                            If odata2.runSp(cnn2, "Delete From productos where Codigo = '" & dr("Codigo").ToString & "'", sinfo) Then
                                odata.runSp(cnn, "update productoeliminado set CargadoAndroid = 1 where Id = " & dr("Id").ToString & " ", sinfo)
                                grid_eventos.Rows.Insert(0, "Producto Nube eliminado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                            End If
                        Else
                            If odata2.runSp(cnn2, "Delete From productos where left(Codigo, 6) = '" & Mid(dr("Codigo").ToString, 1, 6) & "'", sinfo) Then
                                odata.runSp(cnn, "update productoeliminado set CargadoAndroid = 1 where Id = " & dr("Id").ToString & " ", sinfo)
                                grid_eventos.Rows.Insert(0, "Producto Nube eliminado correctamente " & Trim(dr("Nombre").ToString), Date.Now)
                            End If
                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub


    Private Sub subirTraspasosAndroid()

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select * from traspasos where Cargado=0 order by Id"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        Dim drvalida As DataRow
                        If odata2.getDr(cnn2, drvalida, "select Id from traspasos where IdLocal = " & dr("Id").ToString & "", sinfo) Then
                            odata.runSp(cnn, "update traspasos set Cargado = 1, IdTraspasoNube = " & drvalida("Id").ToString & " where Id = " & dr("Id").ToString, sinfo)
                        Else
                            Dim ssql3 As String = "insert into traspasos(Usuario,TotalProd,Fecha,Tipo,Bajado,IdLocal)"
                            ssql3 = ssql3 & " values('" & dr("Usuario").ToString & "'," & dr("TotalProd").ToString & ",'" & dr("Fecha").ToString & "','SALIDA',1," & dr("Id").ToString & ")"
                            If odata2.runSp(cnn2, ssql3, sinfo) Then

                                Dim maxidnube As Integer = 0
                                maxidnube = maxIdTrasNube()

                                Dim varidsucursal As Integer = 0
                                varidsucursal = dameIdSucursalNube(dr("Sucursal").ToString)

                                guardarTraspasoDetalleAndroid(dr("Id").ToString, dr("Fecha").ToString, maxidnube, varidsucursal)

                                odata.runSp(cnn, "update traspasos set Cargado = 1, IdTraspasoNube = " & maxidnube & " where Id = " & dr("Id").ToString, sinfo)

                                grid_eventos.Rows.Insert(0, "Traspaso sincronizada correctamente " & Trim(dr("Id").ToString), Date.Now)

                            End If

                        End If

                        My.Application.DoEvents()

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Public Function guardarTraspasoDetalleAndroid(ByVal IdTras As Integer, ByVal Fecha As String, ByVal IdTrasNube As Integer, ByVal varsucursal As String)

        Dim cnn11 As MySqlConnection = New MySqlConnection
        Dim odata11 As New ToolKitSQL.myssql
        Dim ssql3 As String = ""

        Dim cnn3 As MySqlConnection = New MySqlConnection
        Dim dr3 As DataRow
        Dim dt3 As New DataTable
        Dim sinfo As String = ""
        Dim oData3 As New ToolKitSQL.myssql
        With oData3
            If .dbOpen(cnn3, sTargetlocal, sinfo) Then
                If odata11.dbOpen(cnn11, sTargetdAndroid, sinfo) Then

                    If .getDt(cnn3, dt3, "select * from traspasosdetalle where IdTraspaso = " & IdTras, sinfo) Then
                        For Each dr3 In dt3.Rows

                            ssql3 = "insert into traspasosdetalle(IdTraspaso,Codigo,Descripcion,Cantidad) " &
                                "values(" & IdTrasNube & ",'" & dr3("Codigo").ToString & "','" & dr3("Descripcion").ToString & "'," & Replace(dr3("Cantidad").ToString, ",", "") & ")"
                            If odata11.runSp(cnn11, ssql3, sinfo) Then

                                subeProductosTraspasosAndroid(dr3("Codigo").ToString, varsucursal, dr3("Cantidad").ToString)

                                'odata11.runSp(cnn11, "update Productos set Existencia = Existencia - " & CDec(Replace(dg.Rows(i).Cells(2).Value.ToString, ",", "")) & " where Codigo = '" & dg.Rows(i).Cells(0).Value.ToString & "'", sinfo)
                            End If

                        Next
                    End If

                    cnn11.Close()
                End If
                cnn3.Close()
            End If
        End With
    End Function

    Public Function maxIdTrasNube() As Integer
        Dim cnn33 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim oData33 As New ToolKitSQL.myssql
        With oData33
            If .dbOpen(cnn33, sTargetdAndroid, sinfo) Then
                If .getDr(cnn33, dr3, "select max(Id) as XD from traspasos", sinfo) Then
                    cnn33.Close()
                    Return dr3(0).ToString
                End If
                cnn33.Close()
                Return 0
            End If
        End With
    End Function

    Private Sub subeProductosTraspasosAndroid(ByVal varcodigo As String, ByVal varsucursal As String, ByVal varexistencia As Double)

        Dim cnn As MySqlConnection = New MySqlConnection
        Dim cnn2 As MySqlConnection = New MySqlConnection
        Dim sSQL As String = ""
        sSQL = "Select Codigo,Nombre,IVA,UVenta,PrecioVentaIVA,Departamento,Grupo,CodBarra,PrecioVenta,PreMay,PreMM,PreEsp,Multiplo from productos where Codigo = '" & varcodigo & "'"

        Dim sSQL2 As String = ""
        Dim ssqlinsertal As String = ""
        Dim dt As New DataTable
        Dim dr As DataRow
        Dim dr2 As DataRow
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim odata As New ToolKitSQL.myssql
        Dim odata2 As New ToolKitSQL.myssql
        Dim newExistt As Double = 0
        If odata.dbOpen(cnn, sTargetlocal, sinfo) Then
            If odata2.dbOpen(cnn2, sTargetdAndroid, sinfo) Then
                If odata.getDt(cnn, dt, sSQL, sinfo) Then
                    For Each dr In dt.Rows

                        My.Application.DoEvents()

                        If odata2.getDr(cnn2, dr2, "select Id,Existencia from productos2 where Codigo = '" & Trim(dr("Codigo").ToString) & "' and NumSuc = " & varsucursal, sinfo) Then

                            ssqlinsertal = ""
                            ssqlinsertal = "update productos2 set Descripcion = '" & dr("Nombre").ToString & "', Depto = '" & dr("Departamento").ToString & "', Grupo = '" & dr("Grupo").ToString & "', IVA = " & dr("IVA").ToString & ", UVenta = '" & dr("UVenta").ToString & "', Precio = " & Replace(dr("PrecioVentaIVA").ToString, ",", "") & ", CodBarras = '" & dr("CodBarra").ToString & "', PrecioMin = " & dr("PrecioVenta").ToString & ", PrecioMay = " & dr("PreMay").ToString & ", PrecioMM = " & dr("PreMM").ToString & ", PrecioE = " & dr("PreEsp").ToString & ", Existencia = Existencia + " & Replace(varexistencia, ",", "") & ", NumSuc = " & varsucursal & " where Id = " & dr2("Id").ToString & ""
                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                Dim MyExist As String = ""
                                Dim MyNewEsist As String = ""

                                MyExist = 0
                                If CDec(dr("Multiplo").ToString) > 1 And CDec(dr2("Existencia").ToString) > 0 Then
                                    MyExist = FormatNumber(CDec(dr2("Existencia").ToString), 2)
                                    If Len(Mid(dr("Codigo").ToString, 1, 6)) > 6 Then
                                        MyNewEsist = CDec(MyExist) - CDec(varexistencia)
                                    Else
                                        MyNewEsist = CDec(MyExist) - CDec(varexistencia * CDec(dr("Multiplo").ToString))
                                    End If
                                Else
                                    MyExist = dr2("Existencia").ToString
                                    MyNewEsist = CDec(MyExist) - CDec(varexistencia)
                                End If

                                Dim ssql31 As String = ""

                                If Len(dr("Codigo").ToString) > 6 Then
                                    ssql31 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio,NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "','TRASPASO ENTRADA Android'," & CDec(varexistencia) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','ADMINISTRADOR','" & MyExist & "','" & MyNewEsist & "',''," & varsucursal & ")"
                                Else
                                    ssql31 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio,NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "','TRASPASO ENTRADA Android'," & CDec(varexistencia * CDec(dr("Multiplo").ToString)) & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','ADMIN','" & MyExist & "','" & MyNewEsist & "',''," & varsucursal & ")"
                                End If
                                odata2.runSp(cnn2, ssql31, sinfo)

                            End If

                        Else

                            ssqlinsertal = ""
                            ssqlinsertal = "INSERT INTO productos2(Codigo, Descripcion, IVA, UVenta, Precio, Depto, Grupo, CodBarras, PrecioMin,PrecioMay, PrecioMM,PrecioE, Existencia, NumSuc) VALUES ('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "'," & dr("IVA").ToString & ",'" & dr("UVenta").ToString & "'," & dr("PrecioVentaIVA").ToString & ",'" & dr("Departamento").ToString & "','" & dr("Grupo").ToString & "','" & dr("CodBarra").ToString & "'," & dr("PrecioVenta").ToString & "," & dr("PreMay").ToString & "," & dr("PreMM").ToString & "," & dr("PreEsp").ToString & "," & Replace(varexistencia, ",", "") & "," & varsucursal & ")"

                            If odata2.runSp(cnn2, ssqlinsertal, sinfo) Then

                                Dim ssql31 As String = ""
                                ssql31 = "insert into cardex(Codigo,Nombre,Movimiento,Cantidad,Precio,Fecha,Usuario,Inicial,Final,Folio,NumSuc) values('" & dr("Codigo").ToString & "','" & dr("Nombre").ToString & "','TRASPASO ENTRADA Android'," & Replace(varexistencia, ",", "") & ",'0','" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "','ADMIN','0','" & Replace(varexistencia, ",", "") & "',''," & varsucursal & ")"
                                odata2.runSp(cnn2, ssql31, sinfo)

                            End If

                        End If

                    Next

                End If
                cnn2.Close()
            End If
            cnn.Close()
        End If

    End Sub

    Public Function dameIdSucursalNube(varsucu As String) As Integer
        Dim cnn33 As MySqlClient.MySqlConnection = New MySqlClient.MySqlConnection
        Dim dr3 As DataRow
        Dim sinfo As String = ""
        Dim oData33 As New ToolKitSQL.myssql
        With oData33
            If .dbOpen(cnn33, sTargetdAndroid, sinfo) Then
                If .getDr(cnn33, dr3, "select Id from sucursales where nombre = '" & varsucu & "'", sinfo) Then
                    cnn33.Close()
                    Return dr3(0).ToString
                End If
                cnn33.Close()
                Return 0
            End If
        End With
    End Function

End Class
