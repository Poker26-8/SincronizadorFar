<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSincro
    Inherits System.Windows.Forms.Form

    'Form reemplaza a Dispose para limpiar la lista de componentes.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSincro))
        Me.Label2 = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.lbl_direccion = New System.Windows.Forms.Label()
        Me.lbl_nombre = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.grid_eventos = New System.Windows.Forms.DataGridView()
        Me.Column1 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Column2 = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btn_configura = New System.Windows.Forms.Button()
        Me.Timer_datos = New System.Windows.Forms.Timer(Me.components)
        Me.Timer_reconecta = New System.Windows.Forms.Timer(Me.components)
        Me.NotifyIcon1 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.NotifyIcon2 = New System.Windows.Forms.NotifyIcon(Me.components)
        Me.Label4 = New System.Windows.Forms.Label()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.grid_eventos, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.Label2.Dock = System.Windows.Forms.DockStyle.Top
        Me.Label2.Font = New System.Drawing.Font("Segoe UI Historic", 12.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.ForeColor = System.Drawing.Color.White
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Padding = New System.Windows.Forms.Padding(0, 3, 0, 0)
        Me.Label2.Size = New System.Drawing.Size(548, 31)
        Me.Label2.TabIndex = 137
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.lbl_direccion)
        Me.GroupBox1.Controls.Add(Me.lbl_nombre)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(4, 43)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(539, 141)
        Me.GroupBox1.TabIndex = 138
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Datos"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(8, 78)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(511, 21)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Label3"
        Me.Label3.Visible = False
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 107)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(511, 22)
        Me.Label1.TabIndex = 5
        Me.Label1.Text = "Label4"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label1.Visible = False
        '
        'lbl_direccion
        '
        Me.lbl_direccion.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_direccion.Location = New System.Drawing.Point(8, 51)
        Me.lbl_direccion.Name = "lbl_direccion"
        Me.lbl_direccion.Size = New System.Drawing.Size(511, 20)
        Me.lbl_direccion.TabIndex = 4
        Me.lbl_direccion.Text = "Label3"
        '
        'lbl_nombre
        '
        Me.lbl_nombre.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_nombre.Location = New System.Drawing.Point(8, 22)
        Me.lbl_nombre.Name = "lbl_nombre"
        Me.lbl_nombre.Size = New System.Drawing.Size(511, 22)
        Me.lbl_nombre.TabIndex = 3
        Me.lbl_nombre.Text = "Label1"
        Me.lbl_nombre.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.grid_eventos)
        Me.GroupBox2.Location = New System.Drawing.Point(4, 188)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(539, 381)
        Me.GroupBox2.TabIndex = 139
        Me.GroupBox2.TabStop = False
        '
        'grid_eventos
        '
        Me.grid_eventos.AllowUserToAddRows = False
        Me.grid_eventos.AllowUserToDeleteRows = False
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer))
        Me.grid_eventos.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.grid_eventos.BackgroundColor = System.Drawing.Color.White
        Me.grid_eventos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grid_eventos.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Column1, Me.Column2})
        Me.grid_eventos.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grid_eventos.GridColor = System.Drawing.Color.White
        Me.grid_eventos.Location = New System.Drawing.Point(3, 16)
        Me.grid_eventos.Name = "grid_eventos"
        Me.grid_eventos.ReadOnly = True
        Me.grid_eventos.RowHeadersVisible = False
        Me.grid_eventos.Size = New System.Drawing.Size(533, 362)
        Me.grid_eventos.TabIndex = 1
        '
        'Column1
        '
        Me.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.Column1.HeaderText = "Evento"
        Me.Column1.Name = "Column1"
        Me.Column1.ReadOnly = True
        '
        'Column2
        '
        Me.Column2.HeaderText = "Fecha"
        Me.Column2.Name = "Column2"
        Me.Column2.ReadOnly = True
        Me.Column2.Width = 140
        '
        'btn_configura
        '
        Me.btn_configura.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.btn_configura.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.btn_configura.Location = New System.Drawing.Point(4, 9)
        Me.btn_configura.Name = "btn_configura"
        Me.btn_configura.Size = New System.Drawing.Size(17, 10)
        Me.btn_configura.TabIndex = 140
        Me.btn_configura.Text = "Button1"
        Me.btn_configura.UseVisualStyleBackColor = False
        '
        'Timer_datos
        '
        Me.Timer_datos.Interval = 30000
        '
        'Timer_reconecta
        '
        Me.Timer_reconecta.Interval = 10000
        '
        'NotifyIcon1
        '
        Me.NotifyIcon1.Icon = CType(resources.GetObject("NotifyIcon1.Icon"), System.Drawing.Icon)
        Me.NotifyIcon1.Text = "NotifyIcon1"
        Me.NotifyIcon1.Visible = True
        '
        'NotifyIcon2
        '
        Me.NotifyIcon2.BalloonTipText = "Compra ingresada correctamente"
        Me.NotifyIcon2.BalloonTipTitle = "Notificaciones recibidas"
        Me.NotifyIcon2.Icon = CType(resources.GetObject("NotifyIcon2.Icon"), System.Drawing.Icon)
        Me.NotifyIcon2.Text = "NotifyIcon2"
        '
        'Label4
        '
        Me.Label4.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label4.BackColor = System.Drawing.Color.FromArgb(CType(CType(45, Byte), Integer), CType(CType(170, Byte), Integer), CType(CType(220, Byte), Integer))
        Me.Label4.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(225, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(318, 22)
        Me.Label4.TabIndex = 141
        Me.Label4.Text = "Delsscom Sincronizador Farmacias Version 2.2"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'frmSincro
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(548, 578)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btn_configura)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmSincro"
        Me.Text = "Delsscom® Sincronizador Control Negocios Pro"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.grid_eventos, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents lbl_direccion As Label
    Friend WithEvents lbl_nombre As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents grid_eventos As DataGridView
    Friend WithEvents Column1 As DataGridViewTextBoxColumn
    Friend WithEvents Column2 As DataGridViewTextBoxColumn
    Friend WithEvents btn_configura As Button
    Friend WithEvents Timer_datos As Timer
    Friend WithEvents Timer_reconecta As Timer
    Friend WithEvents NotifyIcon1 As NotifyIcon
    Friend WithEvents NotifyIcon2 As NotifyIcon
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
End Class
