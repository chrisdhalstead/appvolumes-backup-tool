Imports System.Windows.Forms
Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Web
Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security


Public Class dlgav

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        Dim susername As String = txtusername.Text

        If InStr(susername, "\") Then

            susername = Replace(susername, "\", "\\")

        End If

        Dim spw As String = txtpw.Text

        Dim jsonstring As String = "{""username"" : """ & susername & """ , ""password"" : """ & spw & """}"

        Dim data = Encoding.UTF8.GetBytes(jsonstring)

        Form1.theuri = New Uri(Form1.txtAVManager.Text & "/cv_api/sessions")

        If Form1.ssession = "" Then

            Select Case SendRequest(Form1.theuri, data, "application/json", "POST")

                Case "Success"


                Case "Failed"

                    Exit Sub

            End Select


        End If

        Form1.sessionstatus.Text = Form1.ssession

        Form1.get_initial_av_data()

        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()


    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click

        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()


    End Sub

    Public Function AcceptAllCertifications(ByVal sender As Object, ByVal certification As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As System.Net.Security.SslPolicyErrors) As Boolean
        Return True
    End Function

    Function SendRequest(uri As Uri, jsonDataBytes As Byte(), contentType As String, method As String) As String

        Dim res As String = ""

        Try

            'ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3
            'ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)

            Dim req As HttpWebRequest = WebRequest.Create(uri)

            req.ContentType = contentType
            req.Method = method
            req.ContentLength = jsonDataBytes.Length
            req.CookieContainer = Form1.cookies
            req.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf AcceptAllCertifications)

            Dim stream = req.GetRequestStream()
            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length)
            stream.Close()

            Dim response As HttpWebResponse = req.GetResponse
            Dim jsonresponse = req.GetResponse().GetResponseStream()

            For Each cookieValue As Cookie In response.Cookies

                Form1.ssession = cookieValue.ToString

            Next

            Dim reader As New StreamReader(jsonresponse)

            res = reader.ReadToEnd()

            reader.Close()
            response.Close()

            Return "Success"

        Catch ex As WebException

            If InStr(ex.Message, "404") > 0 Then

                MsgBox(ex.Message, MsgBoxStyle.Exclamation, "Error")

                Return "Failed"

            End If

            If InStr(ex.Message, "400") > 0 Then

                Dim responsetext As String

                Dim reader As New StreamReader(ex.Response.GetResponseStream)

                responsetext = reader.ReadToEnd

                Dim aresponse As Array = Split(responsetext, ":")

                Dim sresponse As String = aresponse(1)

                sresponse = sresponse.Remove(sresponse.Length - 1)

                MsgBox("Error Logging into App Volumes Manager: " & vbCrLf & sresponse, MsgBoxStyle.Exclamation, "Error")

                Return "Failed"

            End If

            MsgBox("Error: " & ex.Message, MsgBoxStyle.Exclamation, "Error")
            Return "Failed"

        Catch ex As Exception

            MsgBox("Error: " & ex.Message, MsgBoxStyle.Exclamation, "Error")

            Return "Failed"

        End Try





    End Function


    Private Sub txtusername_TextChanged(sender As Object, e As EventArgs) Handles txtusername.TextChanged

        Form1.Reg_Write("Software\VMware Flings\App Volumes Backup", "AV_Username", txtusername.Text)


    End Sub

    Private Sub dlgav_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
