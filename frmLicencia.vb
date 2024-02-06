Public Class frmLicencia
    Private Sub frmLicencia_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Public Function SerialNumber() As String
        ' Get the Windows Management Instrumentation object.
        Dim wmi As Object = GetObject("WinMgmts:")

        ' Get the "base boards" (mother boards).
        Dim serial_numbers As String = ""
        Dim mother_boards As Object =
            wmi.InstancesOf("Win32_BaseBoard")
        For Each board As Object In mother_boards
            serial_numbers &= ", " & board.SerialNumber
        Next board
        If serial_numbers.Length > 0 Then serial_numbers =
            serial_numbers.Substring(2)

        Return serial_numbers
    End Function

    Public Function GenLicencia(ByVal noPc As String) As String
        'Cambiar a otro proceso que evite fugas
        Dim PC As String = noPc
        Dim letrasPC As String = ""
        Dim numerosPC As String = ""
        PC = Replace(PC, "/", "")
        For x As Integer = 1 To Len(PC)
            Dim RecortaPC As String = PC
            If IsNumeric(Mid(RecortaPC, x, 1)) Then
                numerosPC = numerosPC & Mid(RecortaPC, x, 1)
            End If
            If Not IsNumeric(Mid(RecortaPC, x, 1)) Then
                letrasPC = letrasPC & Mid(RecortaPC, x, 1)
            End If
            RecortaPC = Mid(RecortaPC, x, 500)
        Next
        Dim EntPC As Long = Convert.ToDecimal(numerosPC)
        Dim i As Byte
        Dim Numeros As String
        Dim Numeros2 As String
        Dim Letras As String
        Dim lic As String = ""
        Dim letters As String = ""
        Dim Car As String = ""
        Dim ope As Double = 0

        ope = Math.Cos(CDec(numerosPC))

        If ope > 0 Then
            PC = Strings.Left(Replace(CStr(ope), ".", "9"), 13)
        Else 'Quita los negativos
            PC = Strings.Left(Replace(CStr(Math.Abs(ope)), ".", "8"), 13)
        End If

        For i = 1 To 12
            Car = CDec(Mid(PC, i, 1)) - CDec(Mid(PC, i + 1, 1))
            Select Case Car

                Case Is = 0
                    letters = letters & "Z"
                Case Is = 1
                    letters = letters & "Y"
                Case Is = 2
                    letters = letters & "X"
                Case Is = 3
                    letters = letters & "W"
                Case Is = 4
                    letters = letters & "V"
                Case Is = 5
                    letters = letters & "a"
                Case Is = 6
                    letters = letters & "B"
                Case Is = 7
                    letters = letters & "C"
                Case Is = 8
                    letters = letters & "d"
                Case Is = 9
                    letters = letters & "E"
                Case Is = -1
                    letters = letters & "f"
                Case Is = -2
                    letters = letters & "g"
                Case Is = -3
                    letters = letters & "H"
                Case Is = -4
                    letters = letters & "i"
                Case Is = -5
                    letters = letters & "j"
                Case Is = -6
                    letters = letters & "k"
                Case Is = -7
                    letters = letters & "L"
                Case Is = -8
                    letters = letters & "M"
                Case Is = -9
                    letters = letters & "n"
                Case Else
                    letters = letters & Car
            End Select
        Next
        For i = 1 To 9 Step 2
            Numeros = Mid(PC, i, 1)
            Letras = Mid(letters, i, 1)
            Numeros2 = Mid(PC, i + 1, 1)
            lic = lic & Numeros & Letras & Numeros2 & "-"
        Next
        lic = Strings.Left(lic, lic.Length - 1)
        GenLicencia = lic

    End Function
End Class