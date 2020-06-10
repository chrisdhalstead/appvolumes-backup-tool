Imports System.Windows.Forms
Imports System.Threading

Public Class dlgCreateVM

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Form1.create_vm(cmbds.SelectedItem, cmbHosts.SelectedItem)

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub dlgCreateVM_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        cmbHosts.Items.Clear()
        cmbds.Items.Clear()

        For Each item In Form1.ahosts

            cmbHosts.Items.Add(item)

        Next


    End Sub

    Private Sub cmbHosts_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbHosts.SelectedIndexChanged

        For Each item In Form1.get_datastores(cmbHosts.SelectedItem)

            cmbds.Items.Add(item)

        Next


    End Sub
End Class
