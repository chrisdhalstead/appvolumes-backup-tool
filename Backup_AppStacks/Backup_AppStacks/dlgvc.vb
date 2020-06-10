Imports System.Windows.Forms
Imports VMware.Vim
Imports System.Threading


Public Class dlgvc

    Public t As System.Threading.Thread

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Form1.ssession = ""

        Form1.vcusername = txtusername.Text
        Form1.vcpw = txtpw.Text

        Form1.pb1.Visible = True
        Form1.pb1.BringToFront()

        t = New System.Threading.Thread(AddressOf Form1.get_data)
        t.Start()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Hide()

    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Hide()
    End Sub

    Private Sub Dialog2_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = False

        If Form1.ssessionid = "" Then

            Me.Visible = True

        Else

            Me.Hide()

            Form1.get_data()

            Me.Close()

        End If

    End Sub


    Private Sub txtusername_TextChanged(sender As Object, e As EventArgs) Handles txtusername.TextChanged

        Form1.Reg_Write("Software\VMware Flings\App Volumes Backup", "VC_Username", txtusername.Text)

    End Sub
End Class
