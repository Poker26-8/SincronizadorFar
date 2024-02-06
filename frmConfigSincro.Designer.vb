<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmConfigSincro
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmConfigSincro))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txt_contrasena = New System.Windows.Forms.TextBox()
        Me.txt_usuario = New System.Windows.Forms.TextBox()
        Me.txt_base = New System.Windows.Forms.TextBox()
        Me.txt_servidor = New System.Windows.Forms.TextBox()
        Me.btnout = New System.Windows.Forms.Button()
        Me.btnGuardaFormatos = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.cbosucursal = New System.Windows.Forms.ComboBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtContraF = New System.Windows.Forms.TextBox()
        Me.txtUsuarioF = New System.Windows.Forms.TextBox()
        Me.txtBaseF = New System.Windows.Forms.TextBox()
        Me.txtServidorF = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txt_contrasena)
        Me.GroupBox1.Controls.Add(Me.txt_usuario)
        Me.GroupBox1.Controls.Add(Me.txt_base)
        Me.GroupBox1.Controls.Add(Me.txt_servidor)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(286, 132)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Datos sincronizador"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(11, 104)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Contraseña DB"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(11, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Usuario DB"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(11, 52)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Base de datos"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(11, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(46, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Servidor"
        '
        'txt_contrasena
        '
        Me.txt_contrasena.Location = New System.Drawing.Point(113, 100)
        Me.txt_contrasena.Name = "txt_contrasena"
        Me.txt_contrasena.PasswordChar = Global.Microsoft.VisualBasic.ChrW(42)
        Me.txt_contrasena.Size = New System.Drawing.Size(164, 20)
        Me.txt_contrasena.TabIndex = 3
        '
        'txt_usuario
        '
        Me.txt_usuario.Location = New System.Drawing.Point(113, 74)
        Me.txt_usuario.Name = "txt_usuario"
        Me.txt_usuario.Size = New System.Drawing.Size(164, 20)
        Me.txt_usuario.TabIndex = 2
        '
        'txt_base
        '
        Me.txt_base.Location = New System.Drawing.Point(113, 48)
        Me.txt_base.Name = "txt_base"
        Me.txt_base.Size = New System.Drawing.Size(164, 20)
        Me.txt_base.TabIndex = 1
        '
        'txt_servidor
        '
        Me.txt_servidor.Location = New System.Drawing.Point(113, 22)
        Me.txt_servidor.Name = "txt_servidor"
        Me.txt_servidor.Size = New System.Drawing.Size(164, 20)
        Me.txt_servidor.TabIndex = 0
        '
        'btnout
        '
        Me.btnout.BackgroundImage = CType(resources.GetObject("btnout.BackgroundImage"), System.Drawing.Image)
        Me.btnout.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnout.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnout.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnout.Location = New System.Drawing.Point(238, 351)
        Me.btnout.Name = "btnout"
        Me.btnout.Size = New System.Drawing.Size(60, 63)
        Me.btnout.TabIndex = 257
        Me.btnout.Text = "Salir"
        Me.btnout.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnout.UseVisualStyleBackColor = True
        '
        'btnGuardaFormatos
        '
        Me.btnGuardaFormatos.BackgroundImage = CType(resources.GetObject("btnGuardaFormatos.BackgroundImage"), System.Drawing.Image)
        Me.btnGuardaFormatos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
        Me.btnGuardaFormatos.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnGuardaFormatos.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnGuardaFormatos.Location = New System.Drawing.Point(172, 351)
        Me.btnGuardaFormatos.Name = "btnGuardaFormatos"
        Me.btnGuardaFormatos.Size = New System.Drawing.Size(60, 63)
        Me.btnGuardaFormatos.TabIndex = 256
        Me.btnGuardaFormatos.Text = "Guardar"
        Me.btnGuardaFormatos.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.btnGuardaFormatos.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.cbosucursal)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 288)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(286, 57)
        Me.GroupBox3.TabIndex = 255
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Sucursal"
        '
        'cbosucursal
        '
        Me.cbosucursal.FormattingEnabled = True
        Me.cbosucursal.Location = New System.Drawing.Point(11, 22)
        Me.cbosucursal.Name = "cbosucursal"
        Me.cbosucursal.Size = New System.Drawing.Size(265, 21)
        Me.cbosucursal.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.Label7)
        Me.GroupBox2.Controls.Add(Me.Label8)
        Me.GroupBox2.Controls.Add(Me.txtContraF)
        Me.GroupBox2.Controls.Add(Me.txtUsuarioF)
        Me.GroupBox2.Controls.Add(Me.txtBaseF)
        Me.GroupBox2.Controls.Add(Me.txtServidorF)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 150)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(286, 132)
        Me.GroupBox2.TabIndex = 254
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Datos autofacturación"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(11, 104)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Contraseña DB"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(11, 78)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(61, 13)
        Me.Label6.TabIndex = 6
        Me.Label6.Text = "Usuario DB"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(11, 52)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(75, 13)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Base de datos"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(11, 26)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(46, 13)
        Me.Label8.TabIndex = 4
        Me.Label8.Text = "Servidor"
        '
        'txtContraF
        '
        Me.txtContraF.Location = New System.Drawing.Point(113, 100)
        Me.txtContraF.Name = "txtContraF"
        Me.txtContraF.Size = New System.Drawing.Size(164, 20)
        Me.txtContraF.TabIndex = 3
        '
        'txtUsuarioF
        '
        Me.txtUsuarioF.Location = New System.Drawing.Point(113, 74)
        Me.txtUsuarioF.Name = "txtUsuarioF"
        Me.txtUsuarioF.Size = New System.Drawing.Size(164, 20)
        Me.txtUsuarioF.TabIndex = 2
        '
        'txtBaseF
        '
        Me.txtBaseF.Location = New System.Drawing.Point(113, 48)
        Me.txtBaseF.Name = "txtBaseF"
        Me.txtBaseF.Size = New System.Drawing.Size(164, 20)
        Me.txtBaseF.TabIndex = 1
        '
        'txtServidorF
        '
        Me.txtServidorF.Location = New System.Drawing.Point(113, 22)
        Me.txtServidorF.Name = "txtServidorF"
        Me.txtServidorF.Size = New System.Drawing.Size(164, 20)
        Me.txtServidorF.TabIndex = 0
        '
        'frmConfigSincro
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(306, 425)
        Me.Controls.Add(Me.btnout)
        Me.Controls.Add(Me.btnGuardaFormatos)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmConfigSincro"
        Me.Text = "Configuración del Sincronizador"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txt_contrasena As TextBox
    Friend WithEvents txt_usuario As TextBox
    Friend WithEvents txt_base As TextBox
    Friend WithEvents txt_servidor As TextBox
    Friend WithEvents btnout As Button
    Friend WithEvents btnGuardaFormatos As Button
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents cbosucursal As ComboBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtContraF As TextBox
    Friend WithEvents txtUsuarioF As TextBox
    Friend WithEvents txtBaseF As TextBox
    Friend WithEvents txtServidorF As TextBox
End Class
