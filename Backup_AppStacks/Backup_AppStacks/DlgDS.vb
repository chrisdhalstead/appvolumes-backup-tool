Imports System.Windows.Forms

Public Class DlgDS

    Public htdsinfo As New Hashtable


    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim item As ListViewItem

        For Each item In Form1.lstAS.CheckedItems

            If item.Text = lblas.Text Then

                item.SubItems(1).Text = cmbDataStore.Text

            End If

        Next

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub

    Private Sub DlgDS_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub DlgDS_VisibleChanged(sender As Object, e As EventArgs) Handles Me.VisibleChanged

    End Sub

    Private Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles chkisreachable.CheckedChanged

    End Sub

    Private Sub Label5_Click(sender As Object, e As EventArgs) Handles Label5.Click

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles chkexists.CheckedChanged

    End Sub

    Private Sub cmbDataStore_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDataStore.SelectedIndexChanged

        Dim sresult As String = htdsinfo(cmbDataStore.SelectedItem)

        Dim aresult As Array = Split(sresult, "|")

        chkexists.Checked = aresult(0)

        chkisreachable.Checked = aresult(1)

        lblhostname.Text = aresult(2)

    End Sub
End Class
